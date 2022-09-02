using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using PrzegladaraStrukturyDyskowej.Models;


namespace PrzegladaraStrukturyDyskowej.Controllers
{
    public class FileController : Controller
    {
        private static List<File> test = new List<File>();
        private string root = ConfigurationManager.AppSettings["root"];
        private GetGata data = new GetGata();
        
        // GET: FileController
        public ActionResult Index()
        {
            test = data.GetValues(root);
            return View(test);
        }

        // GET: FileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FileController/Create
        public ActionResult Create()
        {
            return View();
        }



        // GET: FileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(test.FirstOrDefault(x=>x.id==id));
        }

        // POST: FileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, File file)
        {
            try
            {

                if (!String.IsNullOrEmpty(file.path))
                {
                    System.IO.File.Copy(root + @"\" + test[id].path + @"\" + test[id].Name, root + @"\" + test[id].path + @"\" + file.Name);
                    System.IO.File.Delete(root + @"\" + test[id].path + @"\" + test[id].Name);
                    test = data.GetValues(root + @"\" + test[id].path);
                 }
                else
                {
                    System.IO.File.Copy(root + @"\" + test[id].Name, root + @"\" + file.Name);
                    System.IO.File.Delete(root + @"\" + test[id-1].Name);
                test = data.GetValues(root);
            }
                
                return RedirectToAction(nameof(Index));

            }
            catch
            {
               return View(test.FirstOrDefault(x => x.id == id));
            }
        }

        // GET: FileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(test.FirstOrDefault(x => x.id == id));
        }

        // POST: FileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
