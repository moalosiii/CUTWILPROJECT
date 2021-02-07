using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static System.Console;

namespace Events.Web.Models
{
    public class listOfSpeakers
    {
        public IEnumerable<SpeakerInputForModel> allSpeakers { get; set; }
        
    }
}