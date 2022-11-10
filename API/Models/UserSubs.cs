using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class UserSubs
    {
        public User user { get; set; }

        public List<Subscriber> subscribers { get; set; }
    }
}