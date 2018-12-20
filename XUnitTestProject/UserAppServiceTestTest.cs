using Xunit;
using Moq;
using Common;
using System.Collections.Generic;
using Common.Interface;
using System.Threading.Tasks;
using Service;
using Repository;
using MediatR;
using AutoMapper;
using System;
using System.Linq;
using System.Numerics;
using System.Globalization;

namespace XUnitTestProject {
    public class UserRepositoryTest {

        private readonly IUserRepository _mockUserRepository;
        private readonly IMapper _mockMapper;

        private const string email = "email11@test.com";

        public UserRepositoryTest() {

            List<UserModel> userWithNotesDto = new List<UserModel>() {
                new UserModel(){
                    Email = "email11@test.com",
                    FirstName = "Kostas",
                    LastName = "sokos",
                    Password = "122222",
                    RememberMe = false,
                    ResetAnswer = "dog",
                    Note = new HashSet<NotesModel>() {
                            new NotesModel(){Lang=1,Text="adasdasdasda"},
                            new NotesModel(){Lang=2,Text="asdasdqweqw"},
                            new NotesModel(){Lang=3,Text="asasdasdasdadaweqwewq"},
                        }
                }
            };
            List<User> userWithNotesDao = new List<User>() {
                    new User() {
                        Email="email11@test.com",
                        FirstName="nikos",
                        LastName="konos",
                        Password="12312312",
                        IsAdmin=false,
                        RememberMe=false,
                        ResetAnswer="dog",
                        Id= 1,
                        Note= new HashSet<Notes>() {
                            new Notes(){Id=1,Lang=2,Text="hello world!!",UserId= 1}
                        }
                    }
            };

            Mock<UnitOfWork> mockUnityOfWork = new Mock<UnitOfWork>();
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();
            Mock<IMapper> mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<User, UserModel>(It.IsAny<User>()))
                .Returns(
                (User target) => {
                    var userNotes = target.Note.ToArray();
                    return new UserModel() {
                        Email = target.Email,
                        FirstName = target.FirstName,
                        LastName = target.LastName,
                        Password = target.Password,
                        RememberMe = target.RememberMe,
                        ResetAnswer = target.ResetAnswer,
                        Note = new HashSet<NotesModel>() {
                            new NotesModel(){
                                Lang =userNotes[0].Lang,
                                Text=userNotes[0].Text
                            }
                        }
                    };
                });
            mockMapper.Setup(m => m.Map<UserModel, User>(It.IsAny<UserModel>()))
                .Returns(new User() {
                    Email = "email11@test.com",
                    FirstName = "hello",
                    LastName = "world",
                    IsAdmin = false,
                    Password = "12312321",
                    RememberMe = false,
                    ResetAnswer = "dog",
                    Note = new HashSet<Notes>() {
                            new Notes(){Id=1,Lang=2,Text="hello world!!",UserId= 1}
                        }
                });

            mockUserRepository.Setup(m => m.GetUserAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    (string e) => {
                        var query = userWithNotesDao.Where(u => u.Email == e)
                            .SingleOrDefault();
                        return _mockMapper.Map<User, UserModel>(query);
                    }
                );

            mockUserRepository.Setup(m => m.GetUserWithNotesAsync(It.IsAny<string>()))
                .ReturnsAsync(
                (string e) => {
                    var user = userWithNotesDao.Where(u => u.Email == e).SingleOrDefault();
                    return _mockMapper.Map<User, UserModel>(user);
                });

            mockUserRepository.Setup(m => m.DeleteUserAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            mockUserRepository.Setup(m => m.SaveUserAsync(It.IsAny<UserModel>()))
                .ReturnsAsync(
                (UserModel target) => {
                    var user = _mockMapper.Map<UserModel, User>(target);

                    if (user.Id.Equals(default(int))) {
                        userWithNotesDao.Add(user);
                    }

                    return target;
                });

            _mockUserRepository = mockUserRepository.Object
                ?? throw new ArgumentNullException("mockUserRepository");
            _mockMapper = mockMapper.Object
                ?? throw new ArgumentNullException("mockMapper");
        }

        [Fact]
        public async Task CanGetUserAsync_Test() {

            var actual = await _mockUserRepository.GetUserAsync(email);

            Assert.NotNull(actual);
            Assert.IsType<UserModel>(actual);

        }

        [Fact]
        public async Task CanGetUserWithNotesAsync_Test() {

            var actual = await _mockUserRepository.GetUserWithNotesAsync(email);

            Assert.NotNull(actual);
            Assert.IsType<UserModel>(actual);
            Assert.NotNull(actual.Note);
            Assert.NotEmpty(actual.Note);
            //Assert.IsType<List<NotesModel>>(actual.Note);
        }

        [Fact]
        public async Task CanDeleteUserAsync_Test() {

            var actual = await _mockUserRepository.DeleteUserAsync(email);

            Assert.True(actual);
        }

        [Fact]
        public async Task CanSaveUserAsync_Test() {

            UserModel userToAdd = new UserModel() {
                Email = "email13@test.com",
                FirstName = "Kostas",
                LastName = "Ntinos",
                Password = "1321312",
                RememberMe = false,
                ResetAnswer = "dog",
                Note = new HashSet<NotesModel>() {
                    new NotesModel() {
                        Lang=1,
                        Text="adasdada"
                    }
                }
            };

            var actual = await _mockUserRepository.SaveUserAsync(userToAdd);
        }
    }
}