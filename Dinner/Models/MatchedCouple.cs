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

        public int FirstCouple { get; set; }

        [ForeignKey("FirstCouple")]
        public virtual Couple First { get; set; }

        public int SecondCouple { get; set; }

        [ForeignKey("SecondCouple")]
        public virtual Couple Second { get; set; }
    }
}