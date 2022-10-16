using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using API.Models;
using System.Web.Security;
using System.IO;

namespace API.Controllers
{
    public class UsersController : Controller
    {
        private YoutubeCloneEntities db = new YoutubeCloneEntities();

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User model,string returnUrl)
        {
            var dataItem = db.Users.ToList().Where(x => x.Email == model.Email && x.Password == model.Password).First();
            if (dataItem!=null)
            {
                FormsAuthentication.SetAuthCookie(dataItem.Username,false);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length>1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index","Home");
                }
            }
            else
            {
                ModelState.AddModelError("","Invalid user/pass");
                return View();
            }
           
        }
        [Authorize]
        [AllowAnonymous]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles ="Admin")]
        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Username,Password,ProfilePictureURL,BackgroundPictureURL,PhoneNumber,Email,Location,Subscribers,Role")] User user, 
            HttpPostedFileBase ProfilePicture, HttpPostedFileBase BackgroundPicture)
        {
            user.Subscribers = 0;
            user.Role = "User";
            user.ProfilePictureURL = SaveFile(ProfilePicture);
            user.BackgroundPictureURL = SaveFile(BackgroundPicture);
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Username,Password,ProfilePictureURL,BackgroundPictureURL,PhoneNumber,Email,Location,Subscribers,Role")] User user,
            HttpPostedFileBase ProfilePicture, HttpPostedFileBase BackgroundPicture)
        {
            YoutubeCloneEntities database = new YoutubeCloneEntities();
            User userinfo = database.Users.Where(x => x.Id == user.Id).First();
            user.Role = userinfo.Role;
            user.Subscribers = userinfo.Subscribers;

            if (ProfilePicture != null)
            {
                user.ProfilePictureURL = SaveFile(ProfilePicture);
            }
            if (BackgroundPicture != null)
            {
                user.BackgroundPictureURL = SaveFile(BackgroundPicture);
            }

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public string SaveFile(HttpPostedFileBase file)
        {
            string fileName = string.Empty;
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/PhotoFiles/"), fileName);
                file.SaveAs(path);
            }
            return fileName;
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
