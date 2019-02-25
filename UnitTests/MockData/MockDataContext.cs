using Common;
using System.Collections.Generic;
using Common.Info;

namespace XUnitTestProject {
    public static class MockDataContext {
        public static List<UserModel> UserDtoList {
            get {
                return new List<UserModel>() {
                    new UserModel(){
                    Email = "email11@mail.com",
                    FirstName = "hello",
                    LastName = "world",
                    Password = "12312321",
                    RememberMe = false,
                    ResetAnswer = "dog",
                    Note = new HashSet<NotesModel>() {
                            new NotesModel(){
                                Lang =2,Text="hello world!!"
                            }
                        }
                    }
                };
            }
        }
        public static UserModel UserDto {
            get {
                return
                    new UserModel() {
                        Email = "email11@mail.com",
                        FirstName = "hello",
                        LastName = "world",
                        Password = "12312321",
                        RememberMe = false,
                        ResetAnswer = "dog",
                        Note = new HashSet<NotesModel>() {
                            new NotesModel(){
                                Lang =2,Text="hello world!!"
                            }
                        }
                    };
            }
        }

        public static List<User> UserEntityList {
            get {
                return new List<User>() {
                    new User(){
                        Id=1,
                    Email = "email11@mail.com",
                    FirstName = "hello",
                    LastName = "world",
                    IsAdmin=false,
                    Password = "12312321",
                    RememberMe = false,
                    ResetAnswer = "dog",
                    Note = new HashSet<Notes>() {
                            new Notes(){
                                Id=1,
                                Lang =2,
                                Text ="hello world!!"
                            }
                        }
                    }
                };
            }
        }

        public static User UserEntity {
            get {
                return new User() {
                    Id = 1,
                    Email = "email11@mail.com",
                    FirstName = "hello",
                    LastName = "world",
                    IsAdmin = false,
                    Password = "12312321",
                    RememberMe = false,
                    ResetAnswer = "dog",
                    Note = new HashSet<Notes>() {
                            new Notes(){
                                Id=1,
                                Lang =2,
                                Text ="hello world!!"
                            }
                        }
                };
            }
        }
    }
}