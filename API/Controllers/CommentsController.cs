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
    public class CommentsController : Controller
    {
        private YoutubeCloneEntities db = new YoutubeCloneEntities();
        [HttpPost]
        [Authorize]
        public async Task<string> LikeComment(int commentId)
        {
            Comment comment = await db.Comments.FindAsync(commentId);
            CommentLikesOrDislike isdisliked = db.CommentLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.CommentId == commentId && !x.LikeOrDislike).FirstOrDefault();
            CommentLikesOrDislike isliked = db.CommentLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.CommentId == commentId && x.LikeOrDislike).FirstOrDefault();
            if (isdisliked != null)
            {
                comment.Dislikes--;
                db.CommentLikesOrDislikes.Remove(isdisliked);
            }
            if (isliked != null)
            {
                comment.Likes--;
                db.Entry(comment).State = EntityState.Modified;
                CommentLikesOrDislike commentLikes = db.CommentLikesOrDislikes.ToList().Where(x => x.Username == User.Identity.Name && x.CommentId == commentId).FirstOrDefault();
                db.CommentLikesOrDislikes.Remove(commentLikes);
                await db.SaveChangesAsync();
            }
            else
            {

                comment.Likes++;
                db.Entry(comment).State = EntityState.Modified;
                CommentLikesOrDislike commentlikes = new CommentLikesOrDislike
                {

                    CommentId = commentId,
                    Username = User.Identity.Name,
                    LikeOrDislike = true,
                    Videoid = comment.VideoId

                };
                db.CommentLikesOrDislikes.Add(commentlikes);
                await db.SaveChangesAsync();
            }


            return "success";
        }

        [HttpPost]
        [Authorize]
        public async Task<string> DislikeComment(int commentId)
        {
            Comment comment = await db.Comments.FindAsync(commentId);
            CommentLikesOrDislike isliked = db.CommentLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.CommentId == commentId && x.LikeOrDislike).FirstOrDefault();
            CommentLikesOrDislike isdisliked = db.CommentLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.CommentId == commentId && !x.LikeOrDislike).FirstOrDefault();
            if (isliked != null)
            {
                comment.Likes--;
                db.CommentLikesOrDislikes.Remove(isliked);
            }
            if (isdisliked != null)
            {
                comment.Dislikes--;
                db.Entry(comment).State = EntityState.Modified;
                CommentLikesOrDislike commentLikes = db.CommentLikesOrDislikes.Where(x => x.Username == User.Identity.Name && x.CommentId == commentId && !x.LikeOrDislike).FirstOrDefault();
                db.CommentLikesOrDislikes.Remove(commentLikes);
                await db.SaveChangesAsync();
            }
            else
            {
                comment.Dislikes++;
                db.Entry(comment).State = EntityState.Modified;
                CommentLikesOrDislike commentlikes = new CommentLikesOrDislike
                {

                    CommentId = commentId,
                    Username = User.Identity.Name,
                    LikeOrDislike = false,
                    Videoid = comment.VideoId

                };
                db.CommentLikesOrDislikes.Add(commentlikes);
                await db.SaveChangesAsync();
            }



            return "success";
        }

        [HttpPost]
        [Authorize]
        public async Task<string> CreateComment(int videoId, string content)
        {
            User user = db.Users.Where(x => x.Username == User.Identity.Name).First();
            Comment comment = new Comment
            {
                VideoId = videoId,
                CommentContent = content,
                Username = user.Username,
                ProfilePictureUrl = user.ProfilePictureURL,
                Date = DateTime.Now
            };
            db.Comments.Add(comment);
            await db.SaveChangesAsync();

            return "success";
        }
        [HttpPost]
        [Authorize]
        public async Task<string> DeleteComment(int videoId, int commentId)
        {
            Comment comment = await db.Comments.Where(x => x.VideoId == videoId && x.Id == commentId).FirstOrDefaultAsync();
            try
            {
                db.Comments.Remove(comment);
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
        public async Task<string> EditComment(int videoId, string newcomment, int commentId)
        {
            Comment comment = await db.Comments.Where(x => x.VideoId == videoId && x.Id == commentId).FirstOrDefaultAsync();
            comment.Edited = true;
            comment.CommentContent = newcomment;
            try
            {
                db.Entry(comment).State = EntityState.Modified;
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