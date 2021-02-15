using Events.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace Events.Web.Models
{
    public class EventDetailsViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }//name of the event

        public string AuthorId { get; set; }

        public DateTime StartDateTime { get; set; }//if i were to return it as DateTime2, will that be a problem though??

        public DateTime EndDateTime { get; set; }

        //this helps to get the event type
        public string  EventTypeName { get; set; }

        public string EventSpeaker { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public static Expression<Func<Event, EventDetailsViewModel>> ViewModel
        {
            get
            {
                return e => new EventDetailsViewModel()
                {
                    Id = e.Id,
                    Description = e.Description,
                    Comments = e.Comments.AsQueryable().Select(CommentViewModel.ViewModel),
                    AuthorId = e.Author.Id,
                    EventTypeName = e.EventType.name,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.endTime,
                    Title = e.Title
                };
            }
        }


    }
}