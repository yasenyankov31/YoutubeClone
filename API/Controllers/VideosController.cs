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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    public class VideosController : Controller
    {
        private YoutubeCloneEntities db = new YoutubeCloneEntities();
        private string videoAddress = "~/VideoFiles";


        [HttpPost]
        [Authorize]
        public async Task<string> LikeVideo(int videoId)
        {
            Video video = await db.Videos.FindAsync(videoId);
            VideoLikesOrDislike isdisliked = db.VideoLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.VideoId == videoId && !x.LikeOrDislike).FirstOrDefault();
            if (isdisliked != null)
            {
                video.Dislikes--;
                db.VideoLikesOrDislikes.Remove(isdisliked);
            }

            video.Likes++;
            db.Entry(video).State = EntityState.Modified;
            VideoLikesOrDislike vidlikes = new VideoLikesOrDislike
            {

                VideoId = videoId,
                Username = db.Users.Where(x => x.Username == User.Identity.Name).FirstOrDefault().Username,
                LikeOrDislike = true

            };
            db.VideoLikesOrDislikes.Add(vidlikes);
            await db.SaveChangesAsync();

            return "success";
        }

        [HttpPost]
        [Authorize]
        public async Task<string> RemoveLikeVideo(int videoId)
        {
            Video video = await db.Videos.FindAsync(videoId);
            video.Likes--;
            db.Entry(video).State = EntityState.Modified;
            VideoLikesOrDislike videoLikes = db.VideoLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.VideoId == videoId && x.LikeOrDislike).FirstOrDefault();
            db.VideoLikesOrDislikes.Remove(videoLikes);
            await db.SaveChangesAsync();

            return "success";
        }

        [HttpPost]
        [Authorize]
        public async Task<string> DislikeVideo(int videoId)
        {
            Video video = await db.Videos.FindAsync(videoId);
            VideoLikesOrDislike isliked = db.VideoLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.VideoId == videoId && x.LikeOrDislike).FirstOrDefault();
            if (isliked != null)
            {
                video.Likes--;
                db.VideoLikesOrDislikes.Remove(isliked);
            }

            video.Dislikes++;
            db.Entry(video).State = EntityState.Modified;
            VideoLikesOrDislike vidlikes = new VideoLikesOrDislike
            {

                VideoId = videoId,
                Username = db.Users.Where(x => x.Username == User.Identity.Name).FirstOrDefault().Username,
                LikeOrDislike = false

            };
            db.VideoLikesOrDislikes.Add(vidlikes);
            await db.SaveChangesAsync();

            return "success";
        }

        [HttpPost]
        [Authorize]
        public async Task<string> RemoveDislikeVideo(int videoId)
        {
            Video video = await db.Videos.FindAsync(videoId);
            video.Dislikes--;
            db.Entry(video).State = EntityState.Modified;
            VideoLikesOrDislike videoLikes = db.VideoLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.VideoId == videoId && !x.LikeOrDislike).FirstOrDefault();
            db.VideoLikesOrDislikes.Remove(videoLikes);
            await db.SaveChangesAsync();

            return "success";
        }

        [HttpPost]
        [Authorize]
        public string MultiUpload(string id, string fileName)
        {
            var chunkNumber = id;
            var chunks = Request.InputStream;
            string path = Server.MapPath(videoAddress + "/Temp");
            string newpath = Path.Combine(path, fileName + chunkNumber);
            using (FileStream fs = System.IO.File.Create(newpath))
            {
                byte[] bytes = new byte[3757000];
                int bytesRead;
                while ((bytesRead = Request.InputStream.Read(bytes, 0, bytes.Length)) > 0)
                {
                    fs.Write(bytes, 0, bytesRead);
                }
            }
            return "done";
        }
        [HttpPost]
        [Authorize]
        public async Task<string> Create(string fileName, string complete, string videoName, string description, HttpPostedFileBase thumbnail)
        {

            string tempPath = Server.MapPath(videoAddress + "/Temp");
            string videoPath = Server.MapPath(videoAddress);
            string newPath = Path.Combine(tempPath, fileName);
            if (complete == "1")
            {
                string[] filePaths = Directory.GetFiles(tempPath).Where(p => p.Contains(fileName)).OrderBy(p => Int32.Parse(p.Replace(fileName, "$").Split('$')[1])).ToArray();
                foreach (string filePath in filePaths)
                {
                    MergeFiles(newPath, filePath);
                }
            }
            User CreatorInfo = db.Users.Where(x => x.Username == User.Identity.Name).First();
            System.IO.File.Move(Path.Combine(tempPath, fileName), Path.Combine(videoPath, fileName));
            Video video = new Video
            {
                CreatorId = CreatorInfo.Id,
                VideoName = videoName,
                Description = HttpUtility.UrlDecode(description).Replace("script", "sсript"),
                ThumbnailURL = SaveFile(thumbnail),
                VideoURL = fileName,
                DateCreated = DateTime.Now,
                CreatorPhotoUrl = CreatorInfo.ProfilePictureURL,
                CreatorName = CreatorInfo.Username
            };

            db.Videos.Add(video);
            await db.SaveChangesAsync();
            return "success";
        }
        [Authorize]
        private static void MergeFiles(string file1, string file2)
        {
            FileStream fs1 = null;
            FileStream fs2 = null;
            try
            {
                fs1 = System.IO.File.Open(file1, FileMode.Append);
                fs2 = System.IO.File.Open(file2, FileMode.Open);
                byte[] fs2Content = new byte[fs2.Length];
                fs2.Read(fs2Content, 0, (int)fs2.Length);
                fs1.Write(fs2Content, 0, (int)fs2.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                if (fs1 != null) fs1.Close();
                if (fs2 != null) fs2.Close();
                System.IO.File.Delete(file2);
            }
        }
        // GET: Videos
        [Authorize]
        public async Task<ActionResult> Index()
        {
            int UserId = db.Users.Where(x => x.Username == User.Identity.Name).First().Id;
            var videos = db.Videos.Where(x => x.CreatorId == UserId);
            return View(await videos.ToListAsync());
        }

        // GET: Videos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.ProfileUrl = "/DefaultPhotos/anonymousProfile.png";
            }
            else
            {
                ViewBag.ProfileUrl = db.Users.Where(x => x.Username == User.Identity.Name).FirstOrDefault().ProfilePictureURL;
            }

            Video video = await db.Videos.FindAsync(id);
            video.Views += 1;
            db.Entry(video).State = EntityState.Modified;
            await db.SaveChangesAsync();
            VideoInfo videoAndComments = new VideoInfo
            {
                comments = await db.Comments.Where(x => x.VideoId == video.Id).ToListAsync(),
                video = video,
                videolikes = await db.VideoLikesOrDislikes.Where(x => x.VideoId == video.Id).ToListAsync(),
                ContentCreator = db.Users.Where(x => x.Id == video.CreatorId).FirstOrDefault(),
                commentlikes = await db.CommentLikesOrDislikes.Where(x => x.Videoid == video.Id).ToListAsync(),
                replies = await db.Replies.Where(x => x.VideoId == video.Id).ToListAsync()
            };
            ViewBag.isSubed = "";
            if (db.Subscribers.Where(x => x.Username == User.Identity.Name).FirstOrDefault() != null)
            {
                ViewBag.isSubed = "full";
            }


            if (video == null)
            {
                return HttpNotFound();
            }
            return View(videoAndComments);
        }
        [Authorize]
        // GET: Videos/Create
        public ActionResult Create()
        {
            return View();
        }
        public async Task<ActionResult> AllVideos()
        {

            return View(await db.Videos.ToListAsync());
        }


        [Authorize]
        // GET: Videos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = await db.Videos.FindAsync(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public async Task<string> Edit(int id, string fileName, string complete, string videoName, string description, HttpPostedFileBase thumbnail)
        {
            if (fileName != null)
            {
                string tempPath = Server.MapPath(videoAddress + "/Temp");
                string videoPath = Server.MapPath(videoAddress);
                string newPath = Path.Combine(tempPath, fileName);
                if (complete == "1")
                {
                    string[] filePaths = Directory.GetFiles(tempPath).Where(p => p.Contains(fileName)).OrderBy(p => Int32.Parse(p.Replace(fileName, "$").Split('$')[1])).ToArray();
                    foreach (string filePath in filePaths)
                    {
                        MergeFiles(newPath, filePath);
                    }
                }

                System.IO.File.Move(Path.Combine(tempPath, fileName), Path.Combine(videoPath, fileName));
            }

            Video video = await db.Videos.FindAsync(id);
            if (videoName != null)
            {
                video.VideoName = videoName;
            }
            if (description != null)
            {
                video.Description = HttpUtility.UrlDecode(description).Replace("script", "sсript");
            }
            if (thumbnail != null)
            {
                video.ThumbnailURL = SaveFile(thumbnail);
            }
            if (fileName != null)
            {
                video.VideoURL = fileName;
            }
            db.Entry(video).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return "success";

        }

        // GET: Videos/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = await db.Videos.FindAsync(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Video video = await db.Videos.FindAsync(id);
            db.Videos.Remove(video);
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
