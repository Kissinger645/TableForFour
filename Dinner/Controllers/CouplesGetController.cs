using Dinner.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Dinner.Controllers
{
    public class CouplesGetController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [ResponseType(typeof(Couple))]
        public IHttpActionResult Get()
        {
            var listOfCouples= db.Couples.Select(r => new
            {
                UserName = r.UserName,
                Bio = r.Bio
            });

            return Ok(listOfCouples);
            
        }
    }
}
