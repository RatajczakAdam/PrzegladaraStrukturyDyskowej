using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using PrzegladaraStrukturyDyskowej.Models;
using Microsoft.AspNetCore.StaticFiles;
using MimeKit;

namespace PrzegladaraStrukturyDyskowej.Controllers
{
    public class FileController : Controller
    {
        private static List<File> test = new List<File>();
        private string root = ConfigurationManager.AppSettings["root"];
        private GetGata data = new GetGata();
        private static bool getFirstData = true;
        // GET: FileController
        public ActionResult Index(string sortBy)
        {
            if (getFirstData)
            {
                test = data.GetValues(root);
                getFirstData = false;
            }
            ViewBag.NameSortParm = sortBy== "Name" ? "Name_desc" : "Name";
            ViewBag.LastWriteTimeSortParm = sortBy == "LastWriteTime" ? "LastWriteTime_desc" : "LastWriteTime";
            ViewBag.WeightByteSortParm = sortBy == "WeightByte" ? "WeightByte_desc" : "WeightByte";
            ViewBag.FileTypeSortParm = sortBy == "FileType" ? "FileType_desc" : "FileType";
           
            switch (sortBy)
            {
                case "Name_desc":
                    test = test.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Name":
                    test = test.OrderBy(s => s.Name).ToList();
                    break;
                case "LastWriteTime_desc":
                    test = test.OrderByDescending(s => s.LastWriteTime).ToList();
                    break;
                case "LastWriteTime":
                    test = test.OrderBy(s => s.LastWriteTime).ToList();
                    break;
                case "WeightByte_desc":
                    test = test.OrderByDescending(s => s.WeightByte).ToList();
                    break;
                case "WeightByte":
                    test = test.OrderBy(s => s.WeightByte).ToList();
                    break;
                case "FileType_desc":
                    test = test.OrderByDescending(s => s.FileType).ToList();
                    break;
                case "FileType":
                    test = test.OrderBy(s => s.FileType).ToList();
                    break;
                default:
                    break;
            }
            return View(test);
        }


        // GET: FileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(test.FirstOrDefault(x => x.Id == id));
        }

        // POST: FileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, File file)
        {
            try
            {
                //string g1= System.IO.Path.Combine(appPath, FilePath);
                if (!String.IsNullOrEmpty(test[id].Path))
                {
                    if (test[id - 1].FileType == "Directory") { System.IO.Directory.Move(root + @"\" + test[id - 1].Path + @"\" + test[id - 1].Name, root + @"\" + test[id - 1].Path + @"\" + file.Name); }
                    else { System.IO.File.Move(root + @"\" + test[id - 1].Path + @"\" + test[id - 1].Name, root + @"\" + test[id - 1].Path + @"\" + file.Name); }
                    test = data.GetValues(root + @"\" + test[id - 1].Path);
                }
                else
                {
                    if (test[id - 1].FileType == "Directory") { System.IO.Directory.Move(System.IO.Path.Combine(root, test[id - 1].Name), System.IO.Path.Combine(root, file.Name)); }
                    else { System.IO.File.Move(System.IO.Path.Combine(root, test[id - 1].Name), System.IO.Path.Combine(root, file.Name)); }
                    test = data.GetValues(root);
                }

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View(test.FirstOrDefault(x => x.Id == id));
            }
        }

        // GET: FileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(test.FirstOrDefault(x => x.Id == id));
        }

        // POST: FileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAsync(int id)
        {
            try
            {
                if (String.IsNullOrEmpty(test[id - 1].Path))
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
                            return View(test.FirstOrDefault(x => x.Id == id));
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
                            System.IO.Directory.Delete(root + @"\" + test[id - 1].Path + @"\" + test[id - 1].Name);
                            test = data.GetValues(root + @"\" + test[id - 1].Path);
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return View(test.FirstOrDefault(x => x.Id == id));
                        }
                    }
                    else
                    {
                        System.IO.File.Delete(root + @"\" + test[id - 1].Path + @"\" + test[id - 1].Name);
                        test = data.GetValues(root + @"\" + test[id - 1].Path);
                        return RedirectToAction(nameof(Index));
                    }
                }

            }

            catch
            {
                return View(test.FirstOrDefault(x => x.Id == id));
            }
        }

        

        public ActionResult OpenDictionary(int id)
        {
                test = data.GetValues(root + @"\" + test[id].Path+ @"\" + test[id - 1].Name);
                return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> DownloadFileFromFileSystem(int id)
        {
            string file = String.IsNullOrEmpty(test[id - 1].Path) ? root + @"\" + test[id - 1].Name : root + @"\" + test[id - 1].Path + @"\" + test[id - 1].Name;
            System.IO.MemoryStream memory = new System.IO.MemoryStream();
            using (System.IO.FileStream stream = new System.IO.FileStream(file, System.IO.FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(file, out contentType);
            return File(memory, contentType, test[id - 1].Name);
        }
    }
}
