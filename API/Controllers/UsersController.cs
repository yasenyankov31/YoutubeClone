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
            var dataItem = db.Users.ToList().Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefault();
            if (dataItem!=null)
            {

                FormsAuthentication.SetAuthCookie(dataItem.Username, false);
                if (!User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Home");

                }
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
        public async Task<string> GetUserPhoto()
        {
            if (User.Identity.IsAuthenticated)
            {
                User user = await db.Users.Where(x=>x.Username ==User.Identity.Name).FirstOrDefaultAsync();
                return user.ProfilePictureURL;
            }
            return "none";
        
        }
        [Authorize]
        [AllowAnonymous]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        // GET: Users/Details/5
        public async Task<ActionResult> Details(int? id)
        {            
            if (User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    id = db.Users.Where(x => x.Username == User.Identity.Name).FirstOrDefault().Id;
                }
                User user = await db.Users.FindAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                id = db.Users.Where(x => x.Username == User.Identity.Name).FirstOrDefault().Id;
                User user = await db.Users.FindAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }

        }

        // GET: Users/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        private static Random random = new Random();
        public static string RandomName(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Include = "Id,Username,Password,ProfilePictureURL,BackgroundPictureURL,PhoneNumber,Email,Location,Subscribers,Role")] User user, 
            HttpPostedFileBase ProfilePicture, HttpPostedFileBase BackgroundPicture)
        {
            user.Subscribers = 0;
            user.Role = "User";

            user.ProfilePictureURL = SaveFile(ProfilePicture);
            user.BackgroundPictureURL = SaveFile(BackgroundPicture);
            if (ProfilePicture == null)
            {
                user.ProfilePictureURL = "/DefaultPhotos/anonymousProfile.png";
            }
            if (BackgroundPicture == null)
            {
                user.BackgroundPictureURL = "/DefaultPhotos/anonymousBackground.png";
            }
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    id = db.Users.Where(x => x.Username == User.Identity.Name).FirstOrDefault().Id;
                }
                User user = await db.Users.FindAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View(user);
            }
            else
            {
                id = db.Users.Where(x => x.Username == User.Identity.Name).FirstOrDefault().Id;
                User user = await db.Users.FindAsync(id);

                return View(user);
            }

        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Username,Password,ProfilePictureURL,BackgroundPictureURL,PhoneNumber,Email,Location,Subscribers,Role")] User user,
            HttpPostedFileBase ProfilePicture, HttpPostedFileBase BackgroundPicture)
        {
            YoutubeCloneEntities database = new YoutubeCloneEntities();
            User userinfo =await database.Users.FindAsync(user.Id);
            user.Role = userinfo.Role;
            user.Subscribers = userinfo.Subscribers;

            if (ProfilePicture != null)
            {
                user.ProfilePictureURL = SaveFile(ProfilePicture);
                db.Videos.Where(x => x.CreatorId == user.Id).ToList().ForEach(x => x.CreatorPhotoUrl = user.ProfilePictureURL);
                db.Comments.Where(x => x.Username == user.Username).ToList().ForEach(x => x.ProfilePictureUrl = user.ProfilePictureURL);
            }
            if (BackgroundPicture != null)
            {
                user.BackgroundPictureURL = SaveFile(BackgroundPicture);
            }
            if (user.Username!=""&&user.Username!=userinfo.Username)
            {
                FormsAuthentication.SetAuthCookie(user.Username, false);
                db.Videos.Where(x => x.CreatorId == user.Id).ToList().ForEach(x => x.CreatorName = user.Username);
            }

            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Edit");
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
            string fileName = RandomName(8) + ".png";
            if (file != null && file.ContentLength > 0)
            {
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/PhotoFiles/"),fileName);
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
