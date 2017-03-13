using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dinner.Models
{
    public class MatchedCouple
    {
        public int Id { get; set; }
        public string Suggestions { get; set; }

        public string FirstCouple { get; set; }

        [ForeignKey("FirstCouple")]
        public virtual ApplicationUser First { get; set; }

        public string SecondCouple { get; set; }

        [ForeignKey("SecondCouple")]
        public virtual ApplicationUser Second { get; set; }
    }
}