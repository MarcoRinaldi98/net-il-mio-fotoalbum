using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models.DatabaseModels;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IRepositoryPhotos _repoPhotos;

        public PhotosController(IRepositoryPhotos repoPhotos)
        {
            _repoPhotos = repoPhotos;
        }

        [HttpGet]
        public IActionResult GetPhotos()
        {
            List<Photo> photos = _repoPhotos.GetPhotos();

            return Ok(photos);
        }

        [HttpGet]
        public IActionResult SearchPhotos(string? search)
        {
            List<Photo> foundedPhotos = new List<Photo>();
            if (search == null)
            {
                foundedPhotos = _repoPhotos.GetPhotos();
            }
            else
            {
                foundedPhotos = _repoPhotos.GetPhotosByTitle(search);
            }
            return Ok(foundedPhotos);
        }
    }
}
