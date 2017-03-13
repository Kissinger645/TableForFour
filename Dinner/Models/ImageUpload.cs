using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Dinner.Models
{
        public class ImageUploadViewModel
        {
            [Required]
            public string Caption { get; set; }

            [Required]
            public HttpPostedFile File { get; set; }
        }

        public class ImageUpload
        {
            public int Id { get; set; }
            public string Caption { get; set; }
            public string File { get; set; }

            public virtual string FilePath
            {
                get
                {
                    return $"/Uploads/{File}";
                }
            }
        }
    }