using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Events.Data;

namespace Events.Web.Models
{
    public class EventTypeViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public static Expression<Func<EventType, EventTypeViewModel>> ViewModel
        {
            get
            {
                return et => new EventTypeViewModel()
                {
                    Id = et.id,
                    Name = et.name
                };
            }
        }
    }
}