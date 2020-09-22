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

namespace TeploServices.Controllers
{
    public class AdminMessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AdminMessages
        public async Task<ActionResult> Index()
        {
            var messages = db.Messages.Include(m => m.Service).Include(m => m.User).OrderByDescending(d=>d.Date);
            return View(await messages.ToListAsync());
        }

        // GET: AdminMessages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: AdminMessages/Create
        public ActionResult Create()
        {
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: AdminMessages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "MessageId,SNP,Date,Phone,Email,Text,ServiceId,CallTime,UserId")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Messages.Add(message);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "Name", message.ServiceId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", message.UserId);
            return View(message);
        }

        // GET: AdminMessages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "Name", message.ServiceId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", message.UserId);
            return View(message);
        }

        // POST: AdminMessages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "MessageId,SNP,Date,Phone,Email,Text,ServiceId,CallTime,UserId")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "Name", message.ServiceId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", message.UserId);
            return View(message);
        }

        // GET: AdminMessages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = await db.Messages.FindAsync(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: AdminMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Message message = await db.Messages.FindAsync(id);
            db.Messages.Remove(message);
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
