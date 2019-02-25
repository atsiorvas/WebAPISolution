using Xunit;
using Moq;
using Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repository;
using AutoMapper;
using System;
using Common.Info;
using System.Linq.Expressions;

namespace XUnitTestProject {
    public partial class GetUser_should {

        private readonly IMapper _mockMapper;
        private readonly GenericRepository<User> _mockGenericRepository;

        private const string EMAIL = "email11@test.com";

        public GetUser_should() {

            _mockMapper = this.MockMapper();
            _mockGenericRepository = this.MockRepo();
        }

        [Fact]
        public async Task CanGetUserAsync_Test() {

            Expression<Func<User, bool>> filter =
                u => u.Email == EMAIL;

            var actual =
                await _mockGenericRepository.GetAsync(false, filter);

            Assert.NotNull(actual);
            Assert.IsType<User>(actual);
        }

        private GenericRepository<User> MockRepo() {

            var mockGenericRepository = new Mock<GenericRepository<User>>();


            mockGenericRepository
                .Setup(m => m.GetAsync(It.IsAny<bool>(), It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult(MockDataContext.UserEntity));

            if (mockGenericRepository == null)
                throw new ArgumentNullException("mockGenericRepository");

            return mockGenericRepository.Object;
        }

        private IMapper MockMapper() {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<User, UserModel>(It.IsAny<User>()))
               .Returns(
               (User target) => MockDataContext.UserDto);

            mockMapper.Setup(m => m.Map<UserModel, User>(It.IsAny<UserModel>()))
                .Returns(MockDataContext.UserEntity);

            if (mockMapper == null)
                throw new ArgumentNullException("mockMapper");

            return mockMapper.Object;
        }
    }
}