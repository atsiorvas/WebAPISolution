using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Common.Commands;
using Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository;
using Service;

namespace UserService.Controllers {
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly IUserAppService _userService;
        private readonly UserContext _context;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _cfg;
        private readonly IMediator _mediator;
        private readonly UnitOfWork _unitOfWork;
        private readonly ILogger<UserController> _logger;
        private readonly INoteService _noteService;

        public UserController(
            IUserAppService userService,
            UserContext context,
            IMapper mapper,
            IConfigurationProvider configuration,
            IMediator mediator,
            UnitOfWork unitOfWork,
            ILogger<UserController> logger,
            INoteService noteService) {
            _userService = userService ?? throw new ArgumentNullException("userService");
            _context = context ?? throw new ArgumentNullException("context");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            _cfg = configuration ?? throw new ArgumentNullException("configuration");
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException("UnitOfWork");
            _mediator = mediator ?? throw new ArgumentNullException("mediator");
            _logger = logger ?? throw new ArgumentNullException("logger");
            _noteService = noteService ?? throw new ArgumentNullException("noteService");
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserModel>> LoginAsync(LoginModel login) {
            var user = await _userService.LoginAsync(login);
            if (user != null) {
                return Ok(user);
            }
            return BadRequest();
        }

        // POST api/values
        [HttpPut("register")]
        public async Task<ActionResult<UserModel>> Register(UserModel userRegister) {
            var user = await _userService.RegisterAsync(userRegister);
            if (user != null) {
                return Ok(user);
            }
            return BadRequest();

        }

        // Get Users with pagination
        /*
         * https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/sort-filter-page?view=aspnetcore-2.1
         */
        [HttpGet("getUsersWithPaginationAsync")]
        public async Task<ActionResult> GetUsersWithPaginationAsync(
            [FromQuery(Name = "orderBy")]string orderBy,
            [FromQuery(Name = "searchString")]string searchString,
            [FromQuery(Name = "pageSize")]int pageSize = 2,
            [FromQuery(Name = "pageNumber")]int pageNumber = 1
            ) {

            /*
             * configure properties
             */
            string nameSort = !string.IsNullOrEmpty(orderBy) ? orderBy : "";
            string currentFilter = !string.IsNullOrEmpty(searchString) ? searchString : "";

            IQueryable<User> query = null;

            if (!string.IsNullOrEmpty(searchString)) {
                query = _context.User.Include(u => u.Note)
                    .Where(s => s.LastName.Contains(searchString)
                                         || s.FirstName.Contains(searchString));
            } else {
                query = from user in _context.User
                        .Include(u => u.Note)
                        select user;
            }

            switch (nameSort) {
                case "last_name_desc":
                    query = query?.OrderByDescending(s => s.LastName);
                    break;
                case "first_name_desc":
                    query = query?.OrderByDescending(s => s.FirstName);
                    break;
                case "email_desc":
                    query = query?.OrderByDescending(s => s.Email);
                    break;
                default:
                    query = query?.OrderBy(s => s.LastName);
                    break;
            }

            PaginatedList<User, UserModel> pagination = new PaginatedList<User, UserModel>(_mapper);
            PageInfo<UserModel> paginationList = await pagination.CreateItemsAsync(
                query.AsNoTracking() ?? throw new ArgumentException("Null source"),
                pageNumber,
                pageSize);

            return Ok(new {
                hasNextPage = paginationList.HasNextPage,
                pageNumber = paginationList.PageNumber,
                pageSize = paginationList.PageSize,
                totalSize = paginationList.TotalSize,
                totalPages = paginationList.TotalPages,
                users = paginationList.Items
            });
        }

        [Route("AddUserWithNotesAsync")]
        [HttpPut]
        public async Task<ActionResult<UserModel>>
            AddUserWithNotesAsync(UserModel userModel) {

            var createUserCommand = new
                CreateCommand<UserModel>(userModel);

            var user = await _mediator.Send(userModel, new CancellationToken());

            if (user == null) {
                return BadRequest(new { msg = "cannot add record" });
            }
            return user;
        }

        [Route("addNotesForUser")]
        [HttpPost]
        public async Task<List<NotesModel>> AddNotesForUserAsync(
            [FromQuery(Name = "email")]string email,
            [FromBody] List<NotesModel> newNotes) {

            if (string.IsNullOrEmpty(email)) {
                return null;
            }
            try {
                List<NotesModel> getNewNotes = await _noteService.SaveNotesByAsync(email, newNotes);
                return getNewNotes;
            } catch (Exception) {
                return null;
            }
        }

        [Route("getNotesAsync")]
        [HttpGet]
        public async Task<List<NotesModel>> GetNotesAsync(string email) {
            try {

                if (string.IsNullOrEmpty(email)) {
                    return null;
                }
                DbSet<User> users = _context.User;
                DbSet<Notes> notes = _context.Notes;
                //IEnumerable<Notes> query =
                //     from notes in _context.Notes
                //     join user in _context.User on notes.UserId equals user.UserId
                //     where user.Email == email
                //     select notes;

                //IEnumerable<NotesModel> query =
                //    notes
                //    .Join(
                //        users,
                //        note => note.UserId,
                //        user => user.UserId,
                //        (note, user) => new { note, user }
                //     )
                //    .Where(combineMod => combineMod.user.Email == email)
                //    .Select(comb => comb.note)
                //    .ProjectTo<NotesModel>(_cfg);
                //    using (var dbContextTransaction = _context.Database.BeginTransaction()) {

                //        try {

                //            Task<List<NotesModel>> query =
                //             notes
                //            .Where(note => note.user.Email == email)
                //            .ProjectTo<NotesModel>(_cfg).ToListAsync();

                //            User user =
                //                _context.User
                //                .Where(cuser => cuser.Email == email)
                //                .FirstOrDefault();

                //            //var firstName = _context.Entry(user).Property(p => p.FirstName).CurrentValue;

                //            user.RememberMe = true;

                //            _context.Attach(user);

                //            // do another changes
                //            _context.SaveChanges();

                //            dbContextTransaction.Commit();
                //            return await query;
                //        } catch (Exception ex) {
                //            return null;
                //        }
                //    }
                //} catch (Exception ex) {
                //    throw ex;
                //}

                //IDisposable subscription = source.AsObservable().Subscribe(
                //  x => _logger.LogDebug("OnNext: {0}", x),
                //  ex => _logger.LogDebug("OnError: {0}", ex.Message),
                //  () => _logger.LogDebug("OnCompleted"));
                //subscription.Dispose();

                IEnumerable<Notes> notesList = _unitOfWork.NoteRepository
                    .Get(filter: q => q.user.Email == email,
                    orderBy: q => q.OrderBy(n => n.NoteId));

                bool exist = await _unitOfWork.UserRepository
                    .IsExistsAsync(filter: q => q.Email == email);

                var noteListToModel = _mapper.Map<List<NotesModel>>(notesList);
                return noteListToModel;

                //Task<List<NotesModel>> query =
                //         notes
                //        .Where(note => note.user.Email == email)
                //        .ProjectTo<NotesModel>(_cfg).ToListAsync();

                //User user =
                //    _context.User
                //    .Where(cuser => cuser.Email == email)
                //    .FirstOrDefault();

            } catch (Exception ex) {
                _logger.LogError("Exception: ", ex);
                return null;
            }
        }

        [Route("getUserWithNotesAsync")]
        [HttpGet]
        public async Task<ActionResult<UserModel>> GetUserWithNotesAsync(
            [FromQuery(Name = "email")]string email) {

            var requestCreateGetUserCommand = new
                GetUserCommandAsync(email);

            var user = await _mediator.Send(requestCreateGetUserCommand);

            if (user == null) {
                return NotFound();
            }
            return user;
        }

        [Route("deleteUser")]
        [HttpGet]
        public async Task<ActionResult<bool>> DeleteUser(
            [FromQuery(Name = "email")]string email) {

            var requestDeleteUserCommand = new
                DeleteUserAsyncCommand(email);

            return await _mediator.Send(requestDeleteUserCommand);
        }

    }
}
