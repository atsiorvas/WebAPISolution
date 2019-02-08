using AutoMapper;
using Common;
using Common.Data;
using Common.Info;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service {
    public class AlertService {

        private readonly UnitOfWork _unitOfWork;
        private const bool Eager = true;
        private readonly IConfigurationProvider _cfg;
        private readonly IMapper _mapper;
        public AlertService(
              UnitOfWork unitOfWork,
              IMapper mapper) {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException("unitOfWork");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
        }
        public async Task<AlertInfo> CreateAsync(AlertsParameters alertParameters) {
            if (alertParameters == null) {
                return null;
            }

            Alert alert = this.CreateAndPopulate(alertParameters);

            try {
                await _unitOfWork.AlertRepository.SaveAsync(alert);
                return _mapper.Map<AlertInfo>(alert);
            } catch (Exception ex) {
                return null;
            }
        }

        public Alert CreateAndPopulate(
            AlertsParameters alertParameters) {

            Alert alert = new Alert();
            alert.Arguments = GetArguments(alertParameters.Arguments);
            alert.DateSent = alertParameters.FromDate;
            alert.Text = alertParameters.Text;
            alert.DateCreated = alertParameters.DateCreated;
            alert.Text = alertParameters.Text;
            alert.FromDate = alertParameters.FromDate;
            alert.ToDate = alertParameters.ToDate;
            alert.User = alertParameters.User;
            return alert;
        }

        private string GetArguments(IList<string> args) {
            return string.Join(',', args);
        }

        private IList<string> ArgumentsToList(string args) {
            return args.Split(',', StringSplitOptions.None).ToList();
        }
    }
}
