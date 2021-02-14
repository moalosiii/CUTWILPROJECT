using Events.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Events.Web.Models;
using System.Web.Mvc;

namespace Events.Web.Models
{
    public class EventInputModel
    {
        [Required(ErrorMessage = "Event title is required.")]
        [StringLength(200, ErrorMessage = "The {0} must be between {2} and {1} characters long.",
            MinimumLength = 1)]
        [Display(Name = "Title *")]
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "start Date and Time *")]
        public DateTime StartDateTime { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "end Date and Time *")]//use the if statement for defense programming
        public DateTime endDateTime { get; set; }

        public TimeSpan? Duration { get; set; }

        public string Description { get; set; }

        public string EventType { get; set; }

        //get number of participants
        public int maxCount { get; set; }

        public string SpeakerName { get; set; }

        public string SpeakerId { get; set; }


        public string SpeakerSurname { get; set; }

        public string Speakeremail { get; set; }

        public string SpeakerTopic { get; set; }

        public string SpeakerphoneNumber { get; set; }

        public SelectList Speakers { get; set; }

        //let's get all the list of speakers in here:

        [MaxLength(200)]
        public string Location { get; set; }

        [Display(Name = "Is Public?")]
        public bool IsPublic { get; set; }

        //get the list of speakers for an event and be able to use it to assign a speaker
        
        public static EventInputModel CreateFromEvent(Event e)
        {
            return new EventInputModel()
            {
                Title = e.Title,
                StartDateTime = e.StartDateTime,
                endDateTime = e.endTime,
                Duration = e.Duration,
                Location = e.Location,
                Description = e.Description,
                IsPublic = e.IsPublic,
                //get event type with eventtypeid instead of eventtype
                EventType = e.EventTypeid,//remember we use profile to add speakers and we test them with ids
                
            };
        }
        //add speaker and see if you can return it.
        
        
    }
}