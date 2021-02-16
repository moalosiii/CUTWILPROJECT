using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.Web.Models
{
    public class AllParticipantList
    {
        public IEnumerable<ParticipantModelView> participants { get; set; }
        //public IEnumerable<Dictionary<string, object>> backendlessRegisteredUsers { get; set; }
        
    }
}