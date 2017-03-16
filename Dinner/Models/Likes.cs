using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dinner.Models
{
    public class Likes
    {
        public int Id { get; set; }

        public string ThisCouple { get; set; }

        [ForeignKey("ThisCouple")]
        public virtual ApplicationUser First { get; set; }

        public string OtherCouple { get; set; }

        [ForeignKey("OtherCouple")]
        public virtual ApplicationUser Second { get; set; }
    }
}