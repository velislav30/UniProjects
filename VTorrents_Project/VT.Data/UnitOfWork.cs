using System;
using System.Collections.Generic;
using System.Text;
using VT.Models.Entities;

namespace VT.Data
{
    public class UnitOfWork : IDisposable
    {
        private readonly VTorrentsDbContext dbContext;
        private BaseRepository<User> userRepository;
        private BaseRepository<Torrent> torrentRepository;
        private BaseRepository<Catalog> catalogRepository;
        private BaseRepository<SubType> subtypeRepository;
        private BaseRepository<UserToTorrent> userToTorrentRepository;
        private bool disposed = false;

        public UnitOfWork()
        {
            this.dbContext = new VTorrentsDbContext();
            dbContext.Database.EnsureCreated();
        }

        public BaseRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new BaseRepository<User>(dbContext);
                }

                return userRepository;
            }
        }

        public BaseRepository<Torrent> TorrentRepository
        {
            get
            {
                if (this.torrentRepository == null)
                {
                    this.torrentRepository = new BaseRepository<Torrent>(dbContext);
                }

                return torrentRepository;
            }
        }

        public BaseRepository<Catalog> CatalogRepository
        {
            get
            {
                if (this.catalogRepository == null)
                {
                    this.catalogRepository = new BaseRepository<Catalog>(dbContext);
                }

                return catalogRepository;
            }
        }

        public BaseRepository<SubType> SubTypeRepository
        {
            get
            {
                if (this.subtypeRepository == null)
                {
                    this.subtypeRepository = new BaseRepository<SubType>(dbContext);
                }

                return subtypeRepository;
            }
        }

        public BaseRepository<UserToTorrent> UserToTorrentRepository
        {
            get
            {
                if (this.userToTorrentRepository == null)
                {
                    this.userToTorrentRepository = new BaseRepository<UserToTorrent>(dbContext);
                }

                return userToTorrentRepository;
            }
        }

        public bool Save()
        {
            try
            {
                dbContext.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }

                disposed = true;
            }
        }
    }
}