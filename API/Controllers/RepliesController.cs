using API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace API.Controllers
{
    public class RepliesController : Controller
    {
        private YoutubeCloneEntities db = new YoutubeCloneEntities();
        [HttpPost]
        [Authorize]
        public async Task<string> LikeReply(int ReplyId)
        {
            Reply Reply = await db.Replies.FindAsync(ReplyId);
            ReplyLikesOrDislike isdisliked = db.ReplyLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.ReplyId == ReplyId && !x.LikeOrDislike).FirstOrDefault();
            ReplyLikesOrDislike isliked = db.ReplyLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.ReplyId == ReplyId && x.LikeOrDislike).FirstOrDefault();
            if (isdisliked != null)
            {
                Reply.Dislikes--;
                db.ReplyLikesOrDislikes.Remove(isdisliked);
            }

            if (isliked!=null)
            {
                Reply.Likes--;
                db.Entry(Reply).State = EntityState.Modified;
                ReplyLikesOrDislike ReplyLikes = db.ReplyLikesOrDislikes.ToList().Where(x => x.Username == User.Identity.Name && x.ReplyId == ReplyId).FirstOrDefault();
                db.ReplyLikesOrDislikes.Remove(ReplyLikes);
                await db.SaveChangesAsync();
            }
            else
            {
                Reply.Likes++;
                db.Entry(Reply).State = EntityState.Modified;
                ReplyLikesOrDislike Replylikes = new ReplyLikesOrDislike
                {

                    ReplyId = ReplyId,
                    Username = User.Identity.Name,
                    LikeOrDislike = true,
                    Videoid = Reply.VideoId

                };
                db.ReplyLikesOrDislikes.Add(Replylikes);
                await db.SaveChangesAsync();
            }



            return "success";
        }

        [HttpPost]
        [Authorize]
        public async Task<string> DislikeReply(int ReplyId)
        {
            Reply Reply = await db.Replies.FindAsync(ReplyId);
            ReplyLikesOrDislike isliked = db.ReplyLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.ReplyId == ReplyId && x.LikeOrDislike).FirstOrDefault();
            ReplyLikesOrDislike isdisliked = db.ReplyLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.ReplyId == ReplyId && !x.LikeOrDislike).FirstOrDefault();
            if (isliked != null)
            {
                Reply.Likes--;
                db.ReplyLikesOrDislikes.Remove(isliked);
            }
            if (isdisliked!=null)
            {
                Reply.Dislikes--;
                db.Entry(Reply).State = EntityState.Modified;
                ReplyLikesOrDislike ReplyLikes = db.ReplyLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.ReplyId == ReplyId && !x.LikeOrDislike).FirstOrDefault();
                db.ReplyLikesOrDislikes.Remove(ReplyLikes);
                await db.SaveChangesAsync();
            }
            else
            {
                Reply.Dislikes++;
                db.Entry(Reply).State = EntityState.Modified;
                ReplyLikesOrDislike Replylikes = new ReplyLikesOrDislike
                {

                    ReplyId = ReplyId,
                    Username = User.Identity.Name,
                    LikeOrDislike = false,
                    Videoid = Reply.VideoId

                };
                db.ReplyLikesOrDislikes.Add(Replylikes);
                await db.SaveChangesAsync();
            }



            return "success";
        }

        [HttpPost]
        [Authorize]
        public async Task<string> CreateReply(int videoId,int commentId, string content)
        {
            User user = db.Users.Where(x => x.Username == User.Identity.Name).First();
            Reply Reply = new Reply
            {
                VideoId = videoId,
                CommentId= commentId,
                ReplyContent = content,
                Username = user.Username,
                ProfilePictureUrl = user.ProfilePictureURL,
                Date = DateTime.Now
            };
            db.Replies.Add(Reply);
            await db.SaveChangesAsync();

            return "success";
        }
        [HttpPost]
        [Authorize]
        public async Task<string> DeleteReply(int ReplyId)
        {
            Reply Reply = await db.Replies.Where(x =>x.Id == ReplyId).FirstOrDefaultAsync();
            try
            {
                db.Replies.Remove(Reply);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return "error" + ex;
            }

            return "success";
        }

        [HttpPost]
        [Authorize]
        public async Task<string> EditReply(string newReply, int ReplyId)
        {
            Reply Reply = await db.Replies.Where(x=>x.Id == ReplyId).FirstOrDefaultAsync();
            Reply.Edited = true;
            Reply.ReplyContent = newReply;
            try
            {
                db.Entry(Reply).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return "error" + ex;
            }

            return "success";
        }
    }
}