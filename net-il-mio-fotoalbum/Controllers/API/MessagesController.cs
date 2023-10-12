using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models.DatabaseModels;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        // Collegamento al DataBase
        private PhotoContext _db;

        public MessagesController(PhotoContext db)
        {
            _db = db;
        }

        [HttpPost]
        public IActionResult CreateMessage([FromBody] Message message)
        {
            try
            {
                _db.Messages.Add(message);
                _db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
