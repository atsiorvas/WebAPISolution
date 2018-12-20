using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Common.Commands;
using Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository;

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
        public ActionResult GetUsersWithPagination(
            [FromQuery(Name = "orderBy")]string orderBy,
            [FromQuery(Name = "searchString")]string searchString,
            [FromQuery(Name = "email")]string email,
            [FromQuery(Name = "pageSize")]int pageSize = 2,
            [FromQuery(Name = "pageNumber")]int pageNumber = 1
            ) {

            /*
             * configure properties
             */
            email = !string.IsNullOrEmpty(email) ? email : string.Empty;
            string nameSort = !string.IsNullOrEmpty(orderBy) ? orderBy : "";
            string currentFilter = !string.IsNullOrEmpty(searchString) ? searchString : "";

            var paginated = _userService
                .GetUserPaging(email, searchString, nameSort,
                pageNumber, pageSize);
            
            return Ok(paginated);
        }

        [Route("AddUserWithNotesAsync")]
        [HttpPut]
        public async Task<ActionResult<UserModel>>
            AddUserWithNotesAsync(UserModel userModel) {

            var createUserCommand = new
                CreateCommand<UserModel>(userModel);

            var user = await _mediator.Send(createUserCommand, new CancellationToken());

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

                //var query =
                //     from note in _context.Notes.AsParallel()
                //     join user in _context.User.AsParallel() on note.UserId equals user.Id
                //     where user.Email == email
                //     select notes.ToList();

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
                    .Get(filter: q => q.User.Email == email,
                    orderBy: q => q.OrderBy(n => n.Id));

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

            //var requestCreateGetUserCommand = new
            //    GetCommandAsync<UserModel>(email);

            //var user = await _mediator.Send(requestCreateGetUserCommand);
            var user = await _context.User.Where(u => u.Email == email)
                .Include(u => u.Note).FirstOrDefaultAsync();

            if (user == null) {
                return NotFound();
            }
            return Ok(user);
        }

        [Route("deleteUser")]
        [HttpGet]
        public async Task<ActionResult<bool>> DeleteUser(
            [FromQuery(Name = "email")]string email) {

            var requestDeleteUserCommand = new
                DeleteAsyncCommand<UserModel>(email);

            return await _mediator.Send(requestDeleteUserCommand);
        }

        [Route("sendNotification")]
        [HttpGet]
        public ActionResult<AlertsParameters> SendNotification() {
            AlertsParameters _alertsParameters
                = new AlertsParameters
                    .Builder()
                    .WithArguments(new List<string>() { "hello", "space" })
                    .WithFromDate(DateTime.UtcNow.Date)
                    .WithToDate(DateTime.UtcNow.AddDays(1))
                    .build();
            return Ok(_alertsParameters);
        }

        [Route("changeNoteBy")]
        [HttpPost]
        public async Task<ActionResult<bool>> UpdateNotesByUserAsync(
            [FromQuery(Name = "email")]string email, NotesModel noteChanges) {

            // var logger = new LoggerEvent(noteChanges, email);
            //await _mediator.Publish(logger);

            //List<NotesModel> notes = await _noteService.ModifyNoteByAsync(email, noteChanges);
            return Ok(true);
        }

        [Route("changeUsersNotes")]
        [HttpPost]
        public async Task<ActionResult<bool>> UpdateUserAsync(
            UserModel userModel) {

            // var logger = new LoggerEvent(noteChanges, email);
            //await _mediator.Publish(logger);

            bool success = await _userService.UpdateUser(userModel);
            return Ok(success);
        }
        [Route("changeUser")]
        [HttpPost]
        public async Task<ActionResult<bool>> ChangeUserAsync(
            UserModel userModel) {

            bool success = await _userService.ChangeUser(userModel);
            return Ok(success);
        }
    }
}
