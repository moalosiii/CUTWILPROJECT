using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Events.Web.Models
{
    public class EventSpeakerInputModels
    {
        //get all two models in here to use them in the event form
        public EventInputModel eventInputModeling { get; set; }
        public listOfSpeakers allSpeakers { get; set; }

    }
}