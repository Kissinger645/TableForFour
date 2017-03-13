using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dinner.Models
{
    public class Couple
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ZipCode { get; set; }
        public string Phone { get; set; }
        public string Age { get; set; }
        public string SexualPref { get; set; }
    }
}