using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.CustomLoggers;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using net_il_mio_fotoalbum.Models.DatabaseModels;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
        // Custom Logger
        private ICustomLogger _myLogger;
        // Collegamento al DataBase
        private PhotoContext _myDatabase;

        public PhotoController(PhotoContext db, ICustomLogger logger)
        {
            _myLogger = logger;
            _myDatabase = db;
        }

        // Vista Index
        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Photo > Index");

            List<Photo> photos = _myDatabase.Photos.Include(photo => photo.Categories).ToList<Photo>();

            return View(photos);
        }


        public void SetImageFileFromFormFile(PhotoFormModel formData)
        {
            if (formData.ImageFormFile == null)
            {
                return;
            }

            MemoryStream stream = new MemoryStream();
            formData.ImageFormFile.CopyTo(stream);
            formData.Photo.ImageFile = stream.ToArray();
        }
    }
}
