using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VT.Business.DTOs;
using VT.Data;
using VT.Models.Entities;

namespace VT.Business.Services
{
    public class CatalogService
    {
        private UserService userService = new UserService();
        public IEnumerable<CatalogDto> GetAll()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var catalogs = unitOfWork.CatalogRepository.GetAll(c=>c.IsDeleted == false);

                return catalogs.Select(catalog => new CatalogDto
                {
                    Id = catalog.Id,
                    IsDeleted = catalog.IsDeleted,
                    DeletedOn = catalog.DeletedOn,
                    Title = catalog.Title,
                    CreatedOn = catalog.CreatedOn,
                    TorrentNum = catalog.TorrentNum,
                    LastDownloadedFrom = catalog.LastDownloadedFrom,
                    Creator = userService.GetById(catalog.CreatorId)
                });
            }
        }

        public IEnumerable<CatalogDto> GetAllWithDeleted()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var catalogs = unitOfWork.CatalogRepository.GetAll();

                return catalogs.Select(catalog => new CatalogDto
                {
                    Id = catalog.Id,
                    IsDeleted = catalog.IsDeleted,
                    DeletedOn = catalog.DeletedOn,
                    Title = catalog.Title,
                    CreatedOn = catalog.CreatedOn,
                    TorrentNum = catalog.TorrentNum,
                    LastDownloadedFrom = catalog.LastDownloadedFrom,
                    Creator = userService.GetById(catalog.CreatorId)
                });
            }
        }


        public CatalogDto GetById(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var catalog = unitOfWork.CatalogRepository.GetById(id);

                return catalog == null ? null : new CatalogDto
                {
                    Id = catalog.Id,
                    IsDeleted = catalog.IsDeleted,
                    DeletedOn = catalog.DeletedOn,
                    Title = catalog.Title,
                    CreatedOn = catalog.CreatedOn,
                    TorrentNum = catalog.TorrentNum,
                    LastDownloadedFrom = catalog.LastDownloadedFrom,
                    Creator = userService.GetById(catalog.CreatorId)
                };
            }
        }

        public bool Create(CatalogDto catalogDto)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var catalog = new Catalog()
                {
                    Id = catalogDto.Id,
                    IsDeleted = false,
                    DeletedOn = catalogDto.DeletedOn,
                    Title = catalogDto.Title,
                    TorrentNum = 0,
                    CreatedOn = DateTime.Now,
                    CreatorId = catalogDto.Creator.Id
                };

                if (catalog.Title.Equals("Unsorted"))
                {
                    return true;
                }

                unitOfWork.CatalogRepository.Create(catalog);

                return unitOfWork.Save();
            }
        }

        public bool Update(CatalogDto catalogDto)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var result = unitOfWork.CatalogRepository.GetById(catalogDto.Id);

                if (result == null)
                {
                    return false;
                }

                if (catalogDto.Title.Equals("Unsorted"))
                {
                    return true;
                }

                result.Id = catalogDto.Id;
                result.IsDeleted = catalogDto.IsDeleted;
                result.DeletedOn = catalogDto.DeletedOn;
                result.Title = catalogDto.Title;
                result.TorrentNum = catalogDto.TorrentNum;
                result.CreatedOn = catalogDto.CreatedOn;
                result.CreatorId = catalogDto.Creator.Id;

                unitOfWork.CatalogRepository.Update(result);

                return unitOfWork.Save();
            }
        }

        public bool Delete(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Catalog result = unitOfWork.CatalogRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                if (result.Title.Equals("Unsorted"))
                {
                    return true;
                }

                TorrentService torrentService = new TorrentService();
                SubTypeService subTypeService = new SubTypeService();

                List<TorrentDto> torrents = torrentService.GetAllByCatalogWithDeleted(id).ToList();

                CatalogDto unsortedC = GetAllWithTitle("Unsorted").FirstOrDefault();
                SubTypeDto unsortedS = subTypeService.GetAllWithTitle(unsortedC.Id,"Unsorted").FirstOrDefault();

                foreach (var item in torrents)
                {
                    item.Catalog = unsortedC;
                    item.SybType = unsortedS;
                    torrentService.Update(item);
                }

                foreach (var item in subTypeService.GetAllByCatalog(result.Id))
                {
                    subTypeService.Delete(item.Id);
                }

                unitOfWork.CatalogRepository.Delete(result);

                return unitOfWork.Save();
            }
        }

        public bool FakeDelete(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Catalog result = unitOfWork.CatalogRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                if (result.Title.Equals("Unsorted"))
                {
                    return true;
                }

                TorrentService torrentService = new TorrentService();
                SubTypeService subTypeService = new SubTypeService();

                List<TorrentDto> torrents = torrentService.GetAllByCatalogWithDeleted(id).ToList();

                CatalogDto unsortedC = GetAllWithTitle("Unsorted").FirstOrDefault();
                SubTypeDto unsortedS = subTypeService.GetAllWithTitle(unsortedC.Id, "Unsorted").FirstOrDefault();

                foreach (var item in torrents)
                {
                    item.Catalog = unsortedC;
                    item.SybType = unsortedS;
                    torrentService.Update(item);
                }

                foreach (var item in subTypeService.GetAllByCatalog(result.Id))
                {
                    item.IsDeleted = true;
                    item.DeletedOn = DateTime.Now;
                    subTypeService.Update(item);
                }

                result.IsDeleted = true;
                result.DeletedOn = DateTime.Now;
                unitOfWork.CatalogRepository.Update(result);

                return unitOfWork.Save();
            }
        }

        public IEnumerable<CatalogDto> GetAllWithTitle(String title)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var catalogs = unitOfWork.CatalogRepository.GetAll(c=> c.Title.Contains(title) && c.IsDeleted==false);

                return catalogs.Select(catalog => new CatalogDto
                {
                    Id = catalog.Id,
                    IsDeleted = catalog.IsDeleted,
                    DeletedOn = catalog.DeletedOn,
                    Title = catalog.Title,
                    CreatedOn = catalog.CreatedOn,
                    TorrentNum = catalog.TorrentNum,
                    LastDownloadedFrom = catalog.LastDownloadedFrom,
                    Creator = userService.GetById(catalog.CreatorId)
                });
            }
        }

        public IEnumerable<CatalogDto> GetAllWithTitleWithDeleted(String title)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var catalogs = unitOfWork.CatalogRepository.GetAll(c => c.Title.Contains(title));

                return catalogs.Select(catalog => new CatalogDto
                {
                    Id = catalog.Id,
                    IsDeleted = catalog.IsDeleted,
                    DeletedOn = catalog.DeletedOn,
                    Title = catalog.Title,
                    CreatedOn = catalog.CreatedOn,
                    TorrentNum = catalog.TorrentNum,
                    LastDownloadedFrom = catalog.LastDownloadedFrom,
                    Creator = userService.GetById(catalog.CreatorId)
                });
            }
        }
    }
}
