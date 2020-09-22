using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeploServices.Models;
using PagedList;

namespace TeploServices.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<ActionResult> Index()
        {
            var reviews = db.Reviews.Include(r => r.User).OrderByDescending(a=>a.Date).Take(3);
            return View(await reviews.ToListAsync());
          
        }
        [Authorize]
        public async Task<ActionResult> Admin()
        {
            return View();

        }
        // GET: Reviews
        public ActionResult Reviews(int? page)
        {
            var reviews = db.Reviews.Include(u => u.User).OrderBy(a=>a.Date).ToPagedList(page ?? 1,5);
            return View(reviews.ToPagedList(page ?? 1, 12));
        }
        //public ActionResult SendReviews()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SendReviews([Bind(Include = "ReviewId,Phone,Date,Text,UserId")] Review review)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Reviews.Add(review);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }


        //    return View(review);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reviews([Bind(Include = "ReviewId,Phone,Date,Text,UserId,SNP")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }


            return View(review);
        }
        // GET: Messages/Create
        public ActionResult Messages()
        {
            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "Name");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Messages([Bind(Include = "MessageId,SNP,Date,Phone,Email,Text,ServiceId,CallTime,UserId")] Message message)
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}