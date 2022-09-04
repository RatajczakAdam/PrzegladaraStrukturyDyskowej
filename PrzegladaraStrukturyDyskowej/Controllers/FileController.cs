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
        private static List<File> fileDictArr = new List<File>();
        private string root = ConfigurationManager.AppSettings["root"];
        private GetGata data = new GetGata();
        private static bool getFirstData = true;
        // GET: FileController
        public ActionResult Index(string sortBy)
        {
            if (getFirstData)
            {
                fileDictArr = data.GetValues(root);
                getFirstData = false;
            }
            ViewBag.NameSortParm = sortBy== "Name" ? "Name_desc" : "Name";
            ViewBag.LastWriteTimeSortParm = sortBy == "LastWriteTime" ? "LastWriteTime_desc" : "LastWriteTime";
            ViewBag.WeightByteSortParm = sortBy == "WeightByte" ? "WeightByte_desc" : "WeightByte";
            ViewBag.FileTypeSortParm = sortBy == "FileType" ? "FileType_desc" : "FileType";
           
            switch (sortBy)
            {
                case "Name_desc":
                    fileDictArr = fileDictArr.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Name":
                    fileDictArr = fileDictArr.OrderBy(s => s.Name).ToList();
                    break;
                case "LastWriteTime_desc":
                    fileDictArr = fileDictArr.OrderByDescending(s => s.LastWriteTime).ToList();
                    break;
                case "LastWriteTime":
                    fileDictArr = fileDictArr.OrderBy(s => s.LastWriteTime).ToList();
                    break;
                case "WeightByte_desc":
                    fileDictArr = fileDictArr.OrderByDescending(s => s.WeightByte).ToList();
                    break;
                case "WeightByte":
                    fileDictArr = fileDictArr.OrderBy(s => s.WeightByte).ToList();
                    break;
                case "FileType_desc":
                    fileDictArr = fileDictArr.OrderByDescending(s => s.FileType).ToList();
                    break;
                case "FileType":
                    fileDictArr = fileDictArr.OrderBy(s => s.FileType).ToList();
                    break;
                default:
                    break;
            }
            return View(fileDictArr);
        }


        // GET: FileController/Edit/5
        public ActionResult Edit(int id)
        {
            File edit = fileDictArr.FirstOrDefault(x => x.Id == id);
            if (!edit.Atributes.Contains("Directory")) edit.Name = edit.Name.TrimEnd(edit.FileType.ToCharArray());
            return View(edit);
        }

        // POST: FileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, File file)
        {
            try
            {
                if (!String.IsNullOrEmpty(fileDictArr[id].Path))
                {
                    if (fileDictArr[id - 1].Atributes.Contains("Directory")) { System.IO.Directory.Move(root + @"\" + fileDictArr[id - 1].Path + @"\" + fileDictArr[id - 1].Name, root + @"\" + fileDictArr[id - 1].Path + @"\" + file.Name); }
                    else { System.IO.File.Move(root + @"\" + fileDictArr[id - 1].Path + @"\" + fileDictArr[id - 1].Name+fileDictArr[id - 1].FileType, root + @"\" + fileDictArr[id - 1].Path + @"\" + file.Name + fileDictArr[id - 1].FileType); }
                    fileDictArr = data.GetValues(root + @"\" + fileDictArr[id - 1].Path);
                }
                else
                {
                    if (fileDictArr[id - 1].Atributes.Contains("Directory")) { System.IO.Directory.Move(System.IO.Path.Combine(root, fileDictArr[id - 1].Name), System.IO.Path.Combine(root, file.Name)); }
                    else { System.IO.File.Move(System.IO.Path.Combine(root, fileDictArr[id - 1].Name), System.IO.Path.Combine(root, file.Name + fileDictArr[id - 1].FileType)); }
                    fileDictArr = data.GetValues(root);
                }

                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View(fileDictArr.FirstOrDefault(x => x.Id == id));
            }
        }

        // GET: FileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(fileDictArr.FirstOrDefault(x => x.Id == id));
        }

        // POST: FileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAsync(int id)
        {
            try
            {
                if (String.IsNullOrEmpty(fileDictArr[id - 1].Path))
                {
                    if (fileDictArr[id - 1].Atributes.Contains("Directory"))
                    {
                        if (data.GetValues(root + @"\" + fileDictArr[id - 1].Name).Count() == 0)
                        {
                            System.IO.Directory.Delete(root + @"\" + fileDictArr[id - 1].Name);
                            fileDictArr = data.GetValues(root);
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return View(fileDictArr.FirstOrDefault(x => x.Id == id));
                        }
                    }
                    else
                    {
                        System.IO.File.Delete(root + @"\" + fileDictArr[id - 1].Name);
                        fileDictArr = data.GetValues(root);
                        return RedirectToAction(nameof(Index));
                    }
                }

                else
                {
                    if (fileDictArr[id - 1].Atributes.Contains("Directory"))
                    {
                        if (data.GetValues(root + @"\" + fileDictArr[id - 1].Name).Count() == 0)
                        {
                            System.IO.Directory.Delete(root + @"\" + fileDictArr[id - 1].Path + @"\" + fileDictArr[id - 1].Name);
                            fileDictArr = data.GetValues(root + @"\" + fileDictArr[id - 1].Path);
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            return View(fileDictArr.FirstOrDefault(x => x.Id == id));
                        }
                    }
                    else
                    {
                        System.IO.File.Delete(root + @"\" + fileDictArr[id - 1].Path + @"\" + fileDictArr[id - 1].Name);
                        fileDictArr = data.GetValues(root + @"\" + fileDictArr[id - 1].Path);
                        return RedirectToAction(nameof(Index));
                    }
                }

            }

            catch
            {
                return View(fileDictArr.FirstOrDefault(x => x.Id == id));
            }
        }

        

        public ActionResult OpenDictionary(int id)
        {
                fileDictArr = data.GetValues(root + @"\" + fileDictArr[id-1].Path+ @"\" + fileDictArr[id - 1].Name);
                return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> DownloadFileFromFileSystem(int id)
        {
            try
            {
                string file = String.IsNullOrEmpty(fileDictArr[id - 1].Path) ? root + @"\" + fileDictArr[id - 1].Name : root + @"\" + fileDictArr[id - 1].Path + @"\" + fileDictArr[id - 1].Name;
                System.IO.MemoryStream memory = new System.IO.MemoryStream();
                using (System.IO.FileStream stream = new System.IO.FileStream(file, System.IO.FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                string contentType;
                new FileExtensionContentTypeProvider().TryGetContentType(file, out contentType);
                return File(memory, contentType, fileDictArr[id - 1].Name);
            }
            catch 
            {
                return RedirectToAction(nameof(Index));
            }
        }
        public ActionResult GoBack()
        {
            string newPatch = data.GetOldPatch(fileDictArr[0].Path);
            fileDictArr = data.GetValues(root+@"\"+ newPatch);
            return RedirectToAction(nameof(Index));
        }
    }
}
