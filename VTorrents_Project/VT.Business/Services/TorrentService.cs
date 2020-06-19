using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using VT.Business.DTOs;
using VT.Data;
using VT.Models.Entities;

namespace VT.Business.Services
{
    public class TorrentService
    {
        private UserService userService = new UserService();
        private CatalogService catalogService = new CatalogService();
        private SubTypeService subTypeService = new SubTypeService();
        public IEnumerable<TorrentDto> GetAll()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrents = unitOfWork.TorrentRepository.GetAll(t=>t.IsDeleted == false);

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }

        public IEnumerable<TorrentDto> GetAllWithDeleted()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrents = unitOfWork.TorrentRepository.GetAll();

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }
        public TorrentDto GetById(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrent = unitOfWork.TorrentRepository.GetById(id);

                return torrent == null ? null : new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                };
            }
        }

        public bool Create(TorrentDto torrentDto)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrent = new Torrent()
                {
                    Id = torrentDto.Id,
                    IsDeleted = false,
                    DeletedOn = torrentDto.DeletedOn,
                    Title = torrentDto.Title,
                    Description = torrentDto.Description,
                    TimesDownloaded = 0,
                    UploadedOn = DateTime.Now,
                    UploaderId = torrentDto.Uploader.Id,
                    CatalogId = torrentDto.Catalog.Id,
                    SubTypeId = torrentDto.SybType.Id
                };

                CatalogDto catalog = catalogService.GetById(torrent.CatalogId);
                catalog.TorrentNum += 1;

                if (!catalogService.Update(catalog))
                {
                    return false;
                }

                unitOfWork.TorrentRepository.Create(torrent);

                return unitOfWork.Save();
            }
        }

        public bool Update(TorrentDto torrentDto)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var result = unitOfWork.TorrentRepository.GetById(torrentDto.Id);

                if (result == null)
                {
                    return false;
                }

                int oldCatalog = result.CatalogId;
                int newCatalog = torrentDto.Catalog.Id;

                result.Id = torrentDto.Id;
                result.IsDeleted = torrentDto.IsDeleted;
                result.DeletedOn = torrentDto.DeletedOn;
                result.Title = torrentDto.Title;
                result.Description = torrentDto.Description;
                result.CatalogId = torrentDto.Catalog.Id;
                result.SubTypeId = torrentDto.SybType.Id;

                CatalogDto catalogNew = catalogService.GetById(newCatalog);
                CatalogDto catalogOld = catalogService.GetById(oldCatalog);
                catalogNew.TorrentNum += 1;
                catalogOld.TorrentNum -= 1;

                if (!catalogService.Update(catalogNew))
                {
                    return false;
                }

                if (!catalogService.Update(catalogOld))
                {
                    return false;
                }

                unitOfWork.TorrentRepository.Update(result);

                return unitOfWork.Save();
            }
        }

        public bool Delete(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Torrent result = unitOfWork.TorrentRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                if (!result.IsDeleted)
                {
                    CatalogDto catalog = catalogService.GetById(result.CatalogId);
                    catalog.TorrentNum -= 1;

                    if (!catalogService.Update(catalog))
                    {
                        return false;
                    }
                }

                UserToTorrentService userToTorrentService = new UserToTorrentService();

                List<UserToTorrentDto> list = userToTorrentService.GetAll().Where(us=>us.Torrent.Id == id).ToList();

                foreach (var item in list)
                {
                    userToTorrentService.Delete(item.Id);
                }

                unitOfWork.TorrentRepository.Delete(result);

                return unitOfWork.Save();
            }
        }

        public bool FakeDelete(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Torrent result = unitOfWork.TorrentRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                CatalogDto catalog = catalogService.GetById(result.CatalogId);
                catalog.TorrentNum -= 1;

                if (!catalogService.Update(catalog))
                {
                    return false;
                }

                result.IsDeleted = true;
                result.DeletedOn = DateTime.Now;
                unitOfWork.TorrentRepository.Update(result);

                return unitOfWork.Save();
            }
        }

        public IEnumerable<TorrentDto> GetAllByUploader(int uploaderId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrents = unitOfWork.TorrentRepository.GetAll(t => t.UploaderId == uploaderId && t.IsDeleted == false);

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }

        public IEnumerable<TorrentDto> GetAllByUploaderWithDeleted(int uploaderId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrents = unitOfWork.TorrentRepository.GetAll(t => t.UploaderId == uploaderId);

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }
        public IEnumerable<TorrentDto> GetAllByDownloader(int downloaderId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var userToTorrents = unitOfWork.UserToTorrentRepository.GetAll(ut => ut.DownloaderId == downloaderId);

                List<int> torrentIds = new List<int>();

                foreach (var item in userToTorrents)
                {
                    torrentIds.Add(item.TorrentId);
                }

                var torrents = unitOfWork.TorrentRepository.GetAll(t => torrentIds.Contains(t.Id) && t.IsDeleted == false);

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }

        public IEnumerable<TorrentDto> GetAllByCatalog(int catalogId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrents = unitOfWork.TorrentRepository.GetAll(t => t.CatalogId == catalogId && t.IsDeleted == false);

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }

        public IEnumerable<TorrentDto> GetAllBySubType(int subtypeId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrents = unitOfWork.TorrentRepository.GetAll(t => t.SubTypeId == subtypeId && t.IsDeleted == false);

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }

        public IEnumerable<TorrentDto> GetAllWithTitle(int subtypeId, String title)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrents = unitOfWork.TorrentRepository.GetAll(t => t.Title.Contains(title) && t.SubTypeId == subtypeId && t.IsDeleted == false);

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }

        public IEnumerable<TorrentDto> GetAllByDownloaderWithDeleted(int downloaderId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var userToTorrents = unitOfWork.UserToTorrentRepository.GetAll(ut => ut.DownloaderId == downloaderId);

                List<int> torrentIds = new List<int>();

                foreach (var item in userToTorrents)
                {
                    torrentIds.Add(item.TorrentId);
                }

                var torrents = unitOfWork.TorrentRepository.GetAll(t => torrentIds.Contains(t.Id));

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }

        public IEnumerable<TorrentDto> GetAllByCatalogWithDeleted(int catalogId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrents = unitOfWork.TorrentRepository.GetAll(t => t.CatalogId == catalogId);

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }

        public IEnumerable<TorrentDto> GetAllBySubTypeWithDeleted(int subtypeId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrents = unitOfWork.TorrentRepository.GetAll(t => t.SubTypeId == subtypeId);

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }

        public IEnumerable<TorrentDto> GetAllWithTitleWithDeleted(int subtypeId, String title)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var torrents = unitOfWork.TorrentRepository.GetAll(t => t.Title.Contains(title) && t.SubTypeId == subtypeId);

                return torrents.Select(torrent => new TorrentDto
                {
                    Id = torrent.Id,
                    IsDeleted = torrent.IsDeleted,
                    DeletedOn = torrent.DeletedOn,
                    Title = torrent.Title,
                    Description = torrent.Description,
                    TimesDownloaded = torrent.TimesDownloaded,
                    UploadedOn = torrent.UploadedOn,
                    Uploader = userService.GetById(torrent.UploaderId),
                    Catalog = catalogService.GetById(torrent.CatalogId),
                    SybType = subTypeService.GetById(torrent.SubTypeId)
                });
            }
        }

        public bool Download(int id,int downloaderId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                UserToTorrentService userToTorrentService = new UserToTorrentService();

                Torrent result = unitOfWork.TorrentRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                if (!userToTorrentService.Create(new UserToTorrentDto
                {
                    Downloader = userService.GetById(downloaderId),
                    Torrent = GetById(id),
                    IsDeleted = false
                }))
                {
                    return false;
                }

                result.TimesDownloaded += 1;
                unitOfWork.TorrentRepository.Update(result);

                return unitOfWork.Save();
            }
        }

    }
}
