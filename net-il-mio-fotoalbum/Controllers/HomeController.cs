using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.CustomLoggers;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;
using System.Diagnostics;

namespace net_il_mio_fotoalbum.Controllers
{
    public class HomeController : Controller
    {
        // Custom Logger
        private ICustomLogger _myLogger;
        // Collegamento al DataBase
        private PhotoContext _myDatabase;

        public HomeController(PhotoContext db, ICustomLogger logger)
        {
            _myLogger = logger;
            _myDatabase = db;
        }

        // Vista Index
        public IActionResult Index()
        {
            _myLogger.WriteLog("L'utente è arrivato sulla pagina Home > Index");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}