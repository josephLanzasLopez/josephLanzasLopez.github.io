using LN_WEB.Repositories;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace LN_WEB.Controllers
{
    public class StorageFileController : Controller
    {

        RepositoryStorageFile repo;

        //GET: UploadFile
        public ActionResult UploadFile()
        {
            return View();
        }
        // POST: UploadFile
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            Stream stream = file.InputStream;
            repo.UploadFile(file.FileName, stream);
            ViewBag.Mensaje = "El fichero seleccionado se ha subido a Azure correctamente.";
            return View();
        }
    }
}