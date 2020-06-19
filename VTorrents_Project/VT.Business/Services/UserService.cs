using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VT.Business.DTOs;
using VT.Data;
using VT.Models.Entities;

namespace VT.Business.Services
{
    public class UserService
    {
        public IEnumerable<UserDto> GetAll()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var users = unitOfWork.UserRepository.GetAll(u=>u.IsDeleted == false);

                return users.Select(user => new UserDto
                {
                    Id = user.Id,
                    IsDeleted = user.IsDeleted,
                    DeletedOn = user.DeletedOn,
                    Username = user.Username,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedOn = user.CreatedOn,
                    LastLoggedIn = user.LastLoggedIn,
                    isMod = user.isMod,
                    isAdmin = user.isAdmin
                });
            }
        }

        public IEnumerable<UserDto> GetAllWithDeleted()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var users = unitOfWork.UserRepository.GetAll();

                return users.Select(user => new UserDto
                {
                    Id = user.Id,
                    IsDeleted = user.IsDeleted,
                    DeletedOn = user.DeletedOn,
                    Username = user.Username,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedOn = user.CreatedOn,
                    LastLoggedIn = user.LastLoggedIn,
                    isMod = user.isMod,
                    isAdmin = user.isAdmin
                });
            }
        }


        public UserDto GetById(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var user = unitOfWork.UserRepository.GetById(id);

                return user == null ? null : new UserDto
                {
                    Id = user.Id,
                    IsDeleted = user.IsDeleted,
                    DeletedOn = user.DeletedOn,
                    Username = user.Username,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedOn = user.CreatedOn,
                    LastLoggedIn = user.LastLoggedIn,
                    isMod = user.isMod,
                    isAdmin = user.isAdmin
                };
            }
        }

        public bool Create(UserDto userDto)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var user = new User()
                {
                    IsDeleted = false,
                    DeletedOn = userDto.DeletedOn,
                    Username = userDto.Username,
                    Password = userDto.Password,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    CreatedOn = DateTime.Now,
                    LastLoggedIn = DateTime.Now,
                    isMod = userDto.isMod,
                    isAdmin = userDto.isAdmin
                };

                if (user.Username.Equals("deleted"))
                {
                    return true;
                }

                unitOfWork.UserRepository.Create(user);

                return unitOfWork.Save();
            }
        }

        public bool Update(UserDto userDto)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var result = unitOfWork.UserRepository.GetById(userDto.Id);

                if (userDto.Username.Equals("deleted"))
                {
                    return true;
                }

                if (result == null)
                {
                    return false;
                }

                result.IsDeleted = userDto.IsDeleted;
                result.DeletedOn = userDto.DeletedOn;
                result.Username = userDto.Username;
                result.Password = userDto.Password;
                result.FirstName = userDto.FirstName;
                result.LastName = userDto.LastName;
                result.Email = userDto.Email;
                result.CreatedOn = userDto.CreatedOn;
                result.LastLoggedIn = userDto.LastLoggedIn;
                result.isMod = userDto.isMod;
                result.isAdmin = userDto.isAdmin;

                unitOfWork.UserRepository.Update(result);

                return unitOfWork.Save();
            }
        }

        public bool Delete(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                User result = unitOfWork.UserRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                if (result.Username.Equals("deleted"))
                {
                    return true;
                }

                TorrentService torrentService = new TorrentService();

                List<TorrentDto> torrentList = torrentService.GetAllByUploaderWithDeleted(id).ToList();
                UserDto deleted = GetAllWithUsername("deleted").FirstOrDefault();

                foreach (var item in torrentList)
                {
                    item.Uploader = deleted;
                    torrentService.Update(item);

                }

                UserToTorrentService userToTorrentService = new UserToTorrentService();

                List<UserToTorrentDto> list = userToTorrentService.GetAll().Where(us => us.Downloader.Id == id).ToList();

                foreach (var item in list)
                {
                    userToTorrentService.Delete(item.Id);
                }

                unitOfWork.UserRepository.Delete(result);

                return unitOfWork.Save();
            }
        }

        public bool FakeDelete(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                User result = unitOfWork.UserRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                if (result.Username.Equals("deleted"))
                {
                    return true;
                }

                result.IsDeleted = true;
                result.DeletedOn = DateTime.Now;
                unitOfWork.UserRepository.Update(result);

                return unitOfWork.Save();
            }
        }

        public IEnumerable<UserDto> GetAllWithUsername(String username)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var users = unitOfWork.UserRepository.GetAll(u=> u.Username.Contains(username) && u.IsDeleted == false);

                return users.Select(user => new UserDto
                {
                    Id = user.Id,
                    IsDeleted = user.IsDeleted,
                    DeletedOn = user.DeletedOn,
                    Username = user.Username,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedOn = user.CreatedOn,
                    LastLoggedIn = user.LastLoggedIn,
                    isMod = user.isMod,
                    isAdmin = user.isAdmin
                });
            }
        }

        public IEnumerable<UserDto> GetAllWithUsernameWithDeleted(String username)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var users = unitOfWork.UserRepository.GetAll(u => u.Username.Contains(username));

                return users.Select(user => new UserDto
                {
                    Id = user.Id,
                    IsDeleted = user.IsDeleted,
                    DeletedOn = user.DeletedOn,
                    Username = user.Username,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedOn = user.CreatedOn,
                    LastLoggedIn = user.LastLoggedIn,
                    isMod = user.isMod,
                    isAdmin = user.isAdmin
                });
            }
        }

        public UserDto GetUserByLoginCredentials(String username,String password)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var user = unitOfWork.UserRepository.GetAll(u => u.Username.Equals(username) && u.Password.Equals(password)).FirstOrDefault();

                return user == null ? null : new UserDto
                {
                    Id = user.Id,
                    IsDeleted = user.IsDeleted,
                    DeletedOn = user.DeletedOn,
                    Username = user.Username,
                    Password = user.Password,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    CreatedOn = user.CreatedOn,
                    LastLoggedIn = user.LastLoggedIn,
                    isMod = user.isMod,
                    isAdmin = user.isAdmin
                };
            }
        }
    }
}