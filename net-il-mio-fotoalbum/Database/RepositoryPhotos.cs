using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Models.DatabaseModels;

namespace net_il_mio_fotoalbum.Database
{
    public class RepositoryPhotos : IRepositoryPhotos
    {
        private PhotoContext _db;

        public RepositoryPhotos(PhotoContext db)
        {
            _db = db;
        }

        public List<Photo> GetPhotos()
        {
            List<Photo> photos = _db.Photos.Include(photo => photo.Categories).ToList();
            return photos;
        }

        public List<Photo> GetPhotosByTitle(string title)
        {
            List<Photo> foundedPhotos = _db.Photos.Where(photo => photo.Title.ToLower().Contains(title.ToLower())).ToList();

            return foundedPhotos;
        }
    }
}
