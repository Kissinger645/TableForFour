using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dinner.Models
{
    public class Dislike
    {
        public int Id { get; set; }

        public int ThisCouple { get; set; }

        [ForeignKey("ThisCouple")]
        public virtual Couple First { get; set; }

        public int OtherCouple { get; set; }

        [ForeignKey("OtherCouple")]
        public virtual Couple Second { get; set; }
    }
}