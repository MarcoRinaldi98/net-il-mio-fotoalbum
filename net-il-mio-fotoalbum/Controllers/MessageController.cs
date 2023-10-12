using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.CustomLoggers;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models.DatabaseModels;

namespace net_il_mio_fotoalbum.Controllers
{
    public class MessageController : Controller
    {
        // Custom Logger
        private ICustomLogger _myLogger;
        // Collegamento al DataBase
        private PhotoContext _myDatabase;

        public MessageController(PhotoContext db, ICustomLogger logger)
        {
            _myLogger = logger;
            _myDatabase = db;
        }

        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Message > Index");

            List<Message> messages = _myDatabase.Messages.ToList();

            return View(messages);
        }
    }
}
