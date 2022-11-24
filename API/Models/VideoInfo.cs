using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class VideoInfo
    {
        public Video video { get; set; }
        public List<Comment> comments { get; set; }
        public List<VideoLikesOrDislike> videold { get; set; }

        public User ContentCreator { get; set; }
    }
}