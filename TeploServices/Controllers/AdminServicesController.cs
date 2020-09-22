using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeploServices.Models;
using System.IO;

namespace TeploServices.Controllers
{
    public class AdminServicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminServices
        public async Task<ActionResult> Index()
        {
            var services = db.Services.Include(s => s.Type);
            return View(await services.ToListAsync());
        }

        // GET: AdminServices/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: AdminServices/Create
        public ActionResult Create()
        {
            ViewBag.TypesId = new SelectList(db.Types, "TypesId", "Name");
            return View();
        }

        // POST: AdminServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ServiceId,Name,Price,Text,TypesId")] Service service, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid && upload!=null)
            {
                //создания и загрузка файла
                string dirSourse = System.IO.Path.GetFileName(upload.FileName);
                string dirnewSourse = Server.MapPath("~/Resourses/" + dirSourse);
                string path = Server.MapPath("~/Resourses");
                string NameFile;
                Directory.CreateDirectory(path);

                if (System.IO.File.Exists(dirnewSourse))
                {
                    int i = 1;
                    string newname;
                    do
                    {
                        i++;
                        newname = Path.GetFileNameWithoutExtension(dirnewSourse)
                               + i + Path.GetExtension(dirnewSourse); //Новое имя файла
                    }
                    while (System.IO.File.Exists(path + "/" + newname));

                    upload.SaveAs(path + "/" + newname);  //Сохранить файл с новым именем
                    NameFile = newname;
                }
                else
                {
                    //Такого файла не существует в конечной папке, можно копировать
                    upload.SaveAs(path + "/" + dirSourse); //Сохранить файл
                   NameFile = dirSourse;
                }

                service.Photo = NameFile;
                db.Services.Add(service);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.TypesId = new SelectList(db.Types, "TypesId", "Name", service.TypesId);
            return View(service);
        }

        // GET: AdminServices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            ViewBag.TypesId = new SelectList(db.Types, "TypesId", "Name", service.TypesId);
            return View(service);
        }

        // POST: AdminServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ServiceId,Name,Price,Text,TypesId")] Service service, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                var thisservice = db.Services.Find(service.ServiceId);
                thisservice.Name = service.Name;
                thisservice.Price = service.Price;
                thisservice.Text = service.Text;
                thisservice.TypesId = service.TypesId;

                if (upload != null)
                {
                    //создания и загрузка файла
                    string dirSourse = System.IO.Path.GetFileName(upload.FileName);
                    string dirnewSourse = Server.MapPath("~/Resourses/" + dirSourse);
                    string path = Server.MapPath("~/Resourses");
                    string NameFile;
                    Directory.CreateDirectory(path);

                    if (System.IO.File.Exists(dirnewSourse))
                    {
                        int i = 1;
                        string newname;
                        do
                        {
                            i++;
                            newname = Path.GetFileNameWithoutExtension(dirnewSourse)
                                   + i + Path.GetExtension(dirnewSourse); //Новое имя файла
                        }
                        while (System.IO.File.Exists(path + "/" + newname));

                        upload.SaveAs(path + "/" + newname);  //Сохранить файл с новым именем
                        NameFile = newname;
                    }
                    else
                    {
                        //Такого файла не существует в конечной папке, можно копировать
                        upload.SaveAs(path + "/" + dirSourse); //Сохранить файл
                        NameFile = dirSourse;
                    }

                    thisservice.Photo = NameFile;
                        }
                db.Entry(thisservice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.TypesId = new SelectList(db.Types, "TypesId", "Name", service.TypesId);
            return View(service);
        }

        // GET: AdminServices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: AdminServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Service service = await db.Services.FindAsync(id);
            db.Services.Remove(service);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
