using AutoMapper;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service {
    class AlertService {

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

    }
}
