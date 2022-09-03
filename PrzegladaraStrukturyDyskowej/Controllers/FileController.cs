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
            if (test.Count==0)
            {
                test = data.GetValues(root);
            }
            return View(test);
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
                //string g1= System.IO.Path.Combine(appPath, FilePath);
                if (!String.IsNullOrEmpty(file.path))
                {
                    if (test[id - 1].FileType == "Directory") { System.IO.Directory.Move(root + @"\" + test[id - 1].path + @"\" + test[id - 1].Name, root + @"\" + test[id - 1].path + @"\" + file.Name); }
                    else { System.IO.File.Move(root + @"\" + test[id - 1].path + @"\" + test[id - 1].Name, root + @"\" + test[id - 1].path + @"\" + file.Name); }
                    test = data.GetValues(root + @"\" + test[id-1].path);
                 }
                else
                {
                    if (test[id-1].FileType == "Directory") { System.IO.Directory.Move(System.IO.Path.Combine(root, test[id - 1].Name), System.IO.Path.Combine(root, file.Name)); }
                    else { System.IO.File.Move(System.IO.Path.Combine(root, test[id - 1].Name), System.IO.Path.Combine(root, file.Name)); }
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
        public ActionResult Delete(int id, File file)
        {
            try
            {
                if (String.IsNullOrEmpty(test[id - 1].path))
                {
                    if (test[id - 1].FileType == "Directory")
                    {
                        if (data.GetValues(root + @"\" + test[id - 1].Name).Count() == 0)
                        {
                            System.IO.Directory.Delete(root + @"\" + test[id - 1].Name);
                            test = data.GetValues(root);
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return View(test.FirstOrDefault(x => x.id == id));
                        }
                    }
                    else
                    {
                        System.IO.File.Delete(root + @"\" + test[id - 1].Name);
                        test = data.GetValues(root);
                        return RedirectToAction(nameof(Index));
                    }
                }

                else
                {
                    if (test[id - 1].FileType == "Directory")
                    {
                        if (data.GetValues(root + @"\" + test[id - 1].Name).Count() == 0)
                        {
                            System.IO.Directory.Delete(root + @"\" + test[id - 1].path + @"\" + test[id - 1].Name);
                            test = data.GetValues(root + @"\" + test[id - 1].path);
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return View(test.FirstOrDefault(x => x.id == id));
                        }
                    }
                    else
                    {
                        System.IO.File.Delete(root + @"\" + test[id - 1].path + @"\" + test[id - 1].Name);
                        test = data.GetValues(root + @"\" + test[id - 1].path);
                        return RedirectToAction(nameof(Index));
                    }
                }

            }

            catch
            {
                return View(test.FirstOrDefault(x => x.id == id));
            }
        }
    }
}
