using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class VideoAndComments
    {
        public Video video { get; set; }
        public List<Comment> comments { get; set; }

    }
}