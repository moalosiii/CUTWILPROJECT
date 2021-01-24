using Events.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Events.Web.Models
{
    public class ParticipantModelView
    {
        public string name { get; set; }

        public string surname { get; set; }

        public string emailid { get; set; }

        public string password { get; set; }

        public string id { get; set; }
        //this expression is used to display on HTML
        public static Expression<Func<Profile, ParticipantModelView>> ViewModel
        {
            get
            {
                return e => new ParticipantModelView()
                {
                    name = e.name,
                    emailid = e.Emailid,
                    password = e.password,
                    id = e.id
                };
            }
        }
    }
}