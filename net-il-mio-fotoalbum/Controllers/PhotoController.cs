using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient.Server;
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

        // Vista Details
        public IActionResult Details(int id)
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Photo > Details");

            Photo? foundedPhoto = _myDatabase.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (foundedPhoto == null)
            {
                return NotFound($"La foto {id} non è stata trovata!");
            }
            else
            {
                return View("Details", foundedPhoto);
            }
        }

        // Creazione di una foto
        [HttpGet]
        public IActionResult Create()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Photo > Create");

            List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
            List<Category> databaseAllCategories = _myDatabase.Categories.ToList();

            foreach (Category category in databaseAllCategories)
            {
                allCategoriesSelectList.Add(
                    new SelectListItem
                    {
                        Text = category.Title,
                        Value = category.Id.ToString()
                    });
            }

            PhotoFormModel model = new PhotoFormModel { Photo = new Photo(), Categories = allCategoriesSelectList };

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
                List<Category> databaseAllCategories = _myDatabase.Categories.ToList();

                foreach (Category category in databaseAllCategories)
                {
                    allCategoriesSelectList.Add(
                        new SelectListItem
                        {
                            Text = category.Title,
                            Value = category.Id.ToString()
                        });
                }

                data.Categories = allCategoriesSelectList;

                return View("Create", data);
            }

            data.Photo.Categories = new List<Category>();

            if (data.SelectedCategoriesId != null)
            {
                foreach (string categorySelectedId in data.SelectedCategoriesId)
                {
                    int intCategorySelectedId = int.Parse(categorySelectedId);

                    Category? categoryInDb = _myDatabase.Categories.Where(category => category.Id == intCategorySelectedId).FirstOrDefault();

                    if (categoryInDb != null)
                    {
                        data.Photo.Categories.Add(categoryInDb);
                    }
                }
            }

            this.SetImageFileFromFormFile(data);

            _myDatabase.Photos.Add(data.Photo);
            _myDatabase.SaveChanges();

            return RedirectToAction("Index");
        }

        // Modifica di una foto
        [HttpGet]
        public IActionResult Update(int id)
        {
            Photo? photoToEdit = _myDatabase.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (photoToEdit == null)
            {
                return NotFound("La foto che vuoi modificare non è stata trovata");
            }
            else
            {
                _myLogger.WriteLog("L'utente è arrivato sulla pagina Photo > Update");

                List<Category> dbCategoryList = _myDatabase.Categories.ToList();
                List<SelectListItem> selectListItem = new List<SelectListItem>();

                foreach (Category category in dbCategoryList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = category.Id.ToString(),
                        Text = category.Title,
                        Selected = photoToEdit.Categories.Any(categoryAssociated => categoryAssociated.Id == category.Id)
                    });
                }

                PhotoFormModel model = new PhotoFormModel { Photo = photoToEdit, Categories = selectListItem };

                return View("Update", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> dbCategoryList = _myDatabase.Categories.ToList();
                List<SelectListItem> selectListItem = new List<SelectListItem>();

                foreach (Category category in dbCategoryList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Value = category.Id.ToString(),
                        Text = category.Title
                    });
                }

                data.Categories = selectListItem;

                return View("Update", data);
            }

            Photo? photoToUpdate = _myDatabase.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (photoToUpdate != null)
            {
                photoToUpdate.Categories.Clear();

                photoToUpdate.Title = data.Photo.Title;
                photoToUpdate.Description = data.Photo.Description;
                photoToUpdate.ImageUrl = data.Photo.ImageUrl;
                photoToUpdate.IsVisible = data.Photo.IsVisible;


                if (data.SelectedCategoriesId != null)
                {
                    foreach (string categorySelectedId in data.SelectedCategoriesId)
                    {
                        int intCategorySelectedId = int.Parse(categorySelectedId);

                        Category? categoryInDb = _myDatabase.Categories.Where(category => category.Id == intCategorySelectedId).FirstOrDefault();

                        if (categoryInDb != null)
                        {
                            photoToUpdate.Categories.Add(categoryInDb);
                        }
                    }
                }

                if (data.ImageFormFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    data.ImageFormFile.CopyTo(stream);
                    photoToUpdate.ImageFile = stream.ToArray();
                }

                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Mi dispiace non è stata trovata la foto da aggiornare");
            }
        }

        // Cancellazione di una foto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Photo? photoToDelete = _myDatabase.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if (photoToDelete != null)
            {
                _myDatabase.Photos.Remove(photoToDelete);
                _myDatabase.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("La foto da eliminare non è stata trovata!");
            }
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
