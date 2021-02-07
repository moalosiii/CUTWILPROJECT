using Events.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Events.Web.Models
{
    public class SpeakerInputModel
    {
        public string name { get; set; }

        public string surname { get; set; }

        public string emailid { get; set; }

        //public string password { get; set; }// no need to have password as part of the information one ads
        public string Topic { get; set; }

        public string PhoneNumber { get; set; }

        public string id { get; set; }
        //this expression is used to display on HTML
        public static Expression<Func<Profile, SpeakerInputModel>> ViewModel
        {
            get
            {
                return e => new SpeakerInputModel()
                {
                    name = e.name,
                    emailid = e.Emailid,
                    id = e.id,
                    PhoneNumber = e.PhoneNumberid
                };
            }
        }
    }
}