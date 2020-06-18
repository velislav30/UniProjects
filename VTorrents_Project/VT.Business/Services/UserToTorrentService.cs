using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VT.Business.DTOs;
using VT.Data;
using VT.Models.Entities;

namespace VT.Business.Services
{
    public class UserToTorrentService
    {
        private UserService userService = new UserService();
        private TorrentService torrentService = new TorrentService();
        public IEnumerable<UserToTorrentDto> GetAll()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var userToTorrents = unitOfWork.UserToTorrentRepository.GetAll();

                return userToTorrents.Select(userToTorrent => new UserToTorrentDto
                {
                    Id = userToTorrent.Id,
                    IsDeleted = userToTorrent.IsDeleted,
                    DeletedOn = userToTorrent.DeletedOn,
                    Downloader = userService.GetById(userToTorrent.DownloaderId),
                    Torrent = torrentService.GetById(userToTorrent.TorrentId)
                });
            }
        }

        public UserToTorrentDto GetById(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var userToTorrent = unitOfWork.UserToTorrentRepository.GetById(id);

                return userToTorrent == null ? null : new UserToTorrentDto
                {
                    Id = userToTorrent.Id,
                    IsDeleted = userToTorrent.IsDeleted,
                    DeletedOn = userToTorrent.DeletedOn,

                    Downloader = userService.GetById(userToTorrent.DownloaderId),
                    Torrent = torrentService.GetById(userToTorrent.TorrentId)
                };
            }
        }

        public bool Create(UserToTorrentDto userToTorrentDto)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var userToTorrent = new UserToTorrent()
                {
                    IsDeleted = false,
                    DeletedOn = userToTorrentDto.DeletedOn,
                    DownloaderId = userToTorrentDto.Downloader.Id,
                    TorrentId = userToTorrentDto.Torrent.Id
                };

                unitOfWork.UserToTorrentRepository.Create(userToTorrent);

                return unitOfWork.Save();
            }
        }

        public bool Update(UserToTorrentDto userToTorrentDto)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var result = unitOfWork.UserToTorrentRepository.GetById(userToTorrentDto.Id);

                if (result == null)
                {
                    return false;
                }

                result.Id = userToTorrentDto.Id;
                result.IsDeleted = userToTorrentDto.IsDeleted;
                result.DeletedOn = userToTorrentDto.DeletedOn;
                result.DownloaderId = userToTorrentDto.Downloader.Id;
                result.TorrentId = userToTorrentDto.Torrent.Id;

                unitOfWork.UserToTorrentRepository.Update(result);

                return unitOfWork.Save();
            }
        }

        public bool Delete(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                UserToTorrent result = unitOfWork.UserToTorrentRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                unitOfWork.UserToTorrentRepository.Delete(result);

                return unitOfWork.Save();
            }
        }

        public bool FakeDelete(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                UserToTorrent result = unitOfWork.UserToTorrentRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                result.IsDeleted = true;
                result.DeletedOn = DateTime.Now;
                unitOfWork.UserToTorrentRepository.Update(result);

                return unitOfWork.Save();
            }
        }

        public IEnumerable<UserToTorrentDto> GetAllByDownloader(int downloaderId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var userToTorrents = unitOfWork.UserToTorrentRepository.GetAll(ut=> ut.DownloaderId == downloaderId);

                return userToTorrents.Select(userToTorrent => new UserToTorrentDto
                {
                    Id = userToTorrent.Id,
                    IsDeleted = userToTorrent.IsDeleted,
                    DeletedOn = userToTorrent.DeletedOn,
                    Downloader = userService.GetById(userToTorrent.DownloaderId),
                    Torrent = torrentService.GetById(userToTorrent.TorrentId)
                });
            }
        }
    }
}