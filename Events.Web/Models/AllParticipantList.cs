using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackendlessAPI;

namespace Events.Web.Models
{
    public class AllParticipantList
    {
        public IEnumerable<ParticipantModelView> participants { get; set; }
    }
}