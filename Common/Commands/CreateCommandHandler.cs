using AutoMapper;
using Common.Commands;
using Common.Info;
using Common.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace Common {
    public class CreateCommandHandler<TDto>
       : IRequestHandler<CreateCommand<TDto>, TDto>

       where TDto : DTO, new() {

        private readonly IUserAppService _userService;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _cfg;
        private readonly IMediator _mediator;
        private readonly INoteService _noteService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCommandHandler(
             IUserAppService userService,
             IMapper mapper,
             IConfigurationProvider configuration,
             IMediator mediator,
             INoteService noteService,
             IHttpContextAccessor httpContextAccessor) {
            _userService = userService ?? throw new ArgumentNullException("userService");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            _cfg = configuration ?? throw new ArgumentNullException("configuration");
            _mediator = mediator ?? throw new ArgumentNullException("mediator");
            _noteService = noteService ?? throw new ArgumentNullException("noteService");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TDto> Handle(CreateCommand<TDto> request,
            CancellationToken cancellationToken) {

            //var httpContext = _httpContextAccessor.HttpContext;

            //var error = new { error = "error" };
            //httpContext.Response.ContentType = "application/json";
            //httpContext.Response.StatusCode = (int)200;
            //await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(error,
            //    new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }));

            if (string.Equals(typeof(TDto).Name,
                typeof(UserModel).Name.ToString())) {
                UserModel userModel = await _userService
                    .SaveUserAsync((UserModel)(object)request.Model);
                return userModel as TDto;
            }
            return null;
        }
    }
}