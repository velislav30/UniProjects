using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VT.Business.DTOs;
using VT.Data;
using VT.Models.Entities;

namespace VT.Business.Services
{
    public class SubTypeService
    {
        private UserService userService = new UserService();
        private CatalogService catalogService = new CatalogService();
        public IEnumerable<SubTypeDto> GetAll()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var subTypes = unitOfWork.SubTypeRepository.GetAll(s=>s.IsDeleted == false);

                return subTypes.Select(subType => new SubTypeDto
                {
                    Id = subType.Id,
                    IsDeleted = subType.IsDeleted,
                    DeletedOn = subType.DeletedOn,
                    Title = subType.Title,
                    CreatedOn = subType.CreatedOn,
                    Creator = userService.GetById(subType.CreatorId),
                    Catalog = catalogService.GetById(subType.CatalogId)
                });
            }
        }

        public IEnumerable<SubTypeDto> GetAllWithDeleted()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var subTypes = unitOfWork.SubTypeRepository.GetAll();

                return subTypes.Select(subType => new SubTypeDto
                {
                    Id = subType.Id,
                    IsDeleted = subType.IsDeleted,
                    DeletedOn = subType.DeletedOn,
                    Title = subType.Title,
                    CreatedOn = subType.CreatedOn,
                    Creator = userService.GetById(subType.CreatorId),
                    Catalog = catalogService.GetById(subType.CatalogId)
                });
            }
        }


        public SubTypeDto GetById(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var subType = unitOfWork.SubTypeRepository.GetById(id);

                return subType == null ? null : new SubTypeDto
                {
                    Id = subType.Id,
                    IsDeleted = subType.IsDeleted,
                    DeletedOn = subType.DeletedOn,
                    Title = subType.Title,
                    CreatedOn = subType.CreatedOn,

                    Creator = userService.GetById(subType.CreatorId),
                    Catalog = catalogService.GetById(subType.CatalogId)
                };
            }
        }

        public bool Create(SubTypeDto subTypeDto)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var subType = new SubType()
                {
                    Id = subTypeDto.Id,
                    IsDeleted = false,
                    DeletedOn = subTypeDto.DeletedOn,
                    Title = subTypeDto.Title,
                    CreatedOn = DateTime.Now,
                    CreatorId = subTypeDto.Creator.Id,
                    CatalogId = subTypeDto.Catalog.Id
                };

                if (subType.Title.Equals("Unsorted"))
                {
                    return true;
                }

                unitOfWork.SubTypeRepository.Create(subType);

                return unitOfWork.Save();
            }
        }

        public bool Update(SubTypeDto subTypeDto)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var result = unitOfWork.SubTypeRepository.GetById(subTypeDto.Id);

                if (result == null)
                {
                    return false;
                }

                if (subTypeDto.Title.Equals("Unsorted"))
                {
                    return true;
                }

                result.Id = subTypeDto.Id;
                result.IsDeleted = subTypeDto.IsDeleted;
                result.DeletedOn = subTypeDto.DeletedOn;
                result.Title = subTypeDto.Title;
                result.CreatedOn = subTypeDto.CreatedOn;
                result.CreatorId = subTypeDto.Creator.Id;
                result.CatalogId = subTypeDto.Catalog.Id;

                unitOfWork.SubTypeRepository.Update(result);

                return unitOfWork.Save();
            }
        }

        public bool Delete(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                SubType result = unitOfWork.SubTypeRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                if (result.Title.Equals("Unsorted"))
                {
                    return true;
                }

                TorrentService torrentService = new TorrentService();

                List<TorrentDto> torrents = torrentService.GetAllBySubTypeWithDeleted(id).ToList();

                CatalogDto unsortedC = catalogService.GetAllWithTitle("Unsorted").FirstOrDefault();
                SubTypeDto unsortedS = GetAllWithTitle(unsortedC.Id, "Unsorted").FirstOrDefault();

                foreach (var item in torrents)
                {
                    item.Catalog = unsortedC;
                    item.SybType = unsortedS;
                    torrentService.Update(item);
                }

                unitOfWork.SubTypeRepository.Delete(result);

                return unitOfWork.Save();
            }
        }

        public bool FakeDelete(int id)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                SubType result = unitOfWork.SubTypeRepository.GetById(id);

                if (result == null)
                {
                    return false;
                }

                if (result.Title.Equals("Unsorted"))
                {
                    return true;
                }

                TorrentService torrentService = new TorrentService();

                List<TorrentDto> torrents = torrentService.GetAllBySubTypeWithDeleted(id).ToList();

                CatalogDto unsortedC = catalogService.GetAllWithTitle("Unsorted").FirstOrDefault();
                SubTypeDto unsortedS = GetAllWithTitle(unsortedC.Id, "Unsorted").FirstOrDefault();

                foreach (var item in torrents)
                {
                    item.Catalog = unsortedC;
                    item.SybType = unsortedS;
                    torrentService.Update(item);
                }

                result.IsDeleted = true;
                result.DeletedOn = DateTime.Now;
                unitOfWork.SubTypeRepository.Update(result);

                return unitOfWork.Save();
            }
        }

        public IEnumerable<SubTypeDto> GetAllByCatalog(int catalogId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var subTypes = unitOfWork.SubTypeRepository.GetAll(s => s.CatalogId == catalogId && s.IsDeleted == false);

                return subTypes.Select(subType => new SubTypeDto
                {
                    Id = subType.Id,
                    IsDeleted = subType.IsDeleted,
                    DeletedOn = subType.DeletedOn,
                    Title = subType.Title,
                    CreatedOn = subType.CreatedOn,
                    Creator = userService.GetById(subType.CreatorId),
                    Catalog = catalogService.GetById(subType.CatalogId)
                });
            }
        }

        public IEnumerable<SubTypeDto> GetAllByCatalogWithDeleted(int catalogId)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var subTypes = unitOfWork.SubTypeRepository.GetAll(s => s.CatalogId == catalogId);

                return subTypes.Select(subType => new SubTypeDto
                {
                    Id = subType.Id,
                    IsDeleted = subType.IsDeleted,
                    DeletedOn = subType.DeletedOn,
                    Title = subType.Title,
                    CreatedOn = subType.CreatedOn,
                    Creator = userService.GetById(subType.CreatorId),
                    Catalog = catalogService.GetById(subType.CatalogId)
                });
            }
        }

        public IEnumerable<SubTypeDto> GetAllWithTitle(int catalogId,String title)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var subTypes = unitOfWork.SubTypeRepository.GetAll(s => s.CatalogId == catalogId && s.Title.Contains(title) && s.IsDeleted == false);

                return subTypes.Select(subType => new SubTypeDto
                {
                    Id = subType.Id,
                    IsDeleted = subType.IsDeleted,
                    DeletedOn = subType.DeletedOn,
                    Title = subType.Title,
                    CreatedOn = subType.CreatedOn,

                    Creator = userService.GetById(subType.CreatorId),
                    Catalog = catalogService.GetById(subType.CatalogId)
                });
            }
        }

        public IEnumerable<SubTypeDto> GetAllWithTitleWithDeleted(int catalogId, String title)
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                var subTypes = unitOfWork.SubTypeRepository.GetAll(s => s.CatalogId == catalogId && s.Title.Contains(title));

                return subTypes.Select(subType => new SubTypeDto
                {
                    Id = subType.Id,
                    IsDeleted = subType.IsDeleted,
                    DeletedOn = subType.DeletedOn,
                    Title = subType.Title,
                    CreatedOn = subType.CreatedOn,

                    Creator = userService.GetById(subType.CreatorId),
                    Catalog = catalogService.GetById(subType.CatalogId)
                });
            }
        }
    }
}