using Events.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Events.Web.Models
{
    public class EventViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        [Display(Name = "start Date and Time")]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "end Date and Time")]
        public DateTime endDateTime { get; set; }

        public TimeSpan? Duration { get; set; }

        public int maxCount { get; set; }

        public string Author { get; set; }

        public string Location { get; set; }

        //this helps us get the event type and save 
        //it in the event type table
        public EventType eventstype { get;set; }

        //now let's get the event type name and 
        //that will help us to get a specific event.
        public string EventTypeName { get; set; }

        public static Expression<Func<Event, EventViewModel>> ViewModel
        {
            get
            {
                return e => new EventViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDateTime = e.StartDateTime,
                    endDateTime = e.endTime,
                    Duration = e.Duration,
                    Location = e.Location,
                    Author = e.Author.FullName,
                    eventstype = e.EventType,
                    EventTypeName = e.EventType.name 
                    //this eventType is to get the event type and be able to generate it to a specific category
                    //or type
                    
                };
            }
        }
    }
}