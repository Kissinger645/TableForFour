using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dinner.Models
{
    public class Messages
    {
        public int Id { get; set; }
        public string FromCouple { get; set; }
        public string ToCouple { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}