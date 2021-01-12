using Events.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Events.Web.Controllers
{
    public class ParticipantController : BaseController
    {
        // GET: Participant
        public ActionResult Participants()
        {
            string currentUserId = User.Identity.GetUserId();
            var participants = this.db.ParticipantProfile
                .Where(e=>e.id != null)
                .Select(ParticipantModelView.ViewModel);

            var participators = participants.Where(e=> e.id != null);

            return View(new AllParticipantList()
            {
                //return a list of all participants
                participants = participators

            });
        }
    }
}