using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Events.Data;
using System.Linq.Expressions;

namespace Events.Web.Models
{
    public class SpeakerInputForModel
    {
        //this class will be used to put the edit for the speaker updating of the speaker
        [Display(Name = "Name *")]
        public string Name { get; set; }
        
        [Display(Name = "Surname *")]
        public string  Surname { get; set; }
        
        [Display(Name = "Email Address *")]
        public string email { get; set; }
        //add topic later for now leave it empty in the CreateFormEvent
        public string Topic { get; set; }

        [Display(Name = "Phone number  *")]
        public string Phonenumber { get; set; }

        public string SpeakerTopic { get; set; }

        public string eventToSpeak { get; set; }
        
        public string id { get; set; }

        public static SpeakerInputForModel CreateFromEvent(Profile s)
        {
            return new SpeakerInputForModel()
            {
                Name = s.name,
                Surname = s.surname,
                email = s.Emailid,
                Phonenumber = s.PhoneNumberid,
                SpeakerTopic = s.SpeakerTopic,
                eventToSpeak = s.eventToSpeak
                //get event type with eventtypeid instead of eventtype
                

            };
        }
        //this i had to add because my views were coming here and just not finding other variables
        //if this doesn't work i am going to delete it.
        public static Expression<Func<Profile, SpeakerInputForModel>> ViewModel
        {
            get
            {
                return e => new SpeakerInputForModel()
                {
                    Name = e.name,
                    email = e.Emailid,
                    Surname = e.surname,
                    id = e.id,
                    Phonenumber = e.PhoneNumberid
                };
            }
        }
    }
}