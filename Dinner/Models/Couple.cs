using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Dinner.Models
{
    public class Couple
    {
        public int Id { get; set; }

        public string CurrentUser { get; set; }

        [ForeignKey("CurrentUser")]
        public virtual ApplicationUser DbUser { get; set; }

        public string UserName { get; set; }
        public string Bio { get; set; }

        public int ProfilePic { get; set; }

        [ForeignKey("ProfilePic")]
        public virtual ImageUpload Image { get; set; }

        [Display(Name="Zip Code")]
        public int ZipCode { get; set; }

        public string Phone { get; set; }

        [Display(Name = "Our Ages")]
        public string Age { get; set; }

        [Display(Name = "Our Sexual Orientation")]
        public string Orientation { get; set; }

        [Display(Name = "Foods We Like")]
        public string FavoriteFoods { get; set; }

        [Display(Name = "Other Couples Age")]
        public string AgePreference { get; set; }

        [Display(Name = "Other Couples Sexual Orietation")]
        public string SexualPreference { get; set; }

        [Display(Name = "We Prefer To Spend")]
        public string PricePreference { get; set; }
    }
}