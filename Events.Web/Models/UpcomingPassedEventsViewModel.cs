using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.Web.Models
{
    public class UpcomingPassedEventsViewModel
    {

        public IEnumerable<EventViewModel> UpcomingEvents { get; set; }

        public IEnumerable<EventViewModel> PassedEvents { get; set; }

        public IEnumerable<EventViewModel> Entertainment { get; set; } //get alll the Conference query

        public IEnumerable<EventViewModel> Sports { get; set; } //get alll the Conference query

        public IEnumerable<EventViewModel> Graduation { get; set; } //get alll the Conference query

        public IEnumerable<EventViewModel> Conference { get; set; } //get alll the Conference query

    }
}