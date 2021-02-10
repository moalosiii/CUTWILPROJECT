using Events.Data;
using Events.Web.Extensions;
using Events.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BackendlessAPI;
using System.Net.Mail;
using System.Net;
using BackendlessAPI.Persistence;
using BackendlessAPI.Async;

namespace Events.Web.Controllers
{
    [Authorize]
    public class EventsController : BaseController
    {

        public ActionResult My()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var events = this.db.Events
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime)
                .Select(EventViewModel.ViewModel);

            //to get the list queried, you have to come here and in the MY page/controller
            //query according to event categories
            var entertainment = events.Where(e => e.EventTypeName == "Entertainment");
            var sports = events.Where(e => e.EventTypeName == "Sports");
            var conference = events.Where(e => e.EventTypeName == "Conference");
            var graduation = events.Where(e => e.EventTypeName == "Graduation");

            //Query according to dates and times.
            var upcomingEvents = events.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = events.Where(e => e.StartDateTime <= DateTime.Now);
            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents,
                Entertainment = entertainment, // this is also defined in the upcomingPassedEventsViewModel
                Sports = sports,
                Conference = conference,
                Graduation = graduation
            });
        }

        public ActionResult GraduationsEvents()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var events = this.db.Events
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime)
                .Select(EventViewModel.ViewModel);

            //to get the list queried, you have to come here and in the MY page/controller
            //query according to event categories
            var graduations = events.Where(e => e.EventTypeName == "Graduation");
            return View(new UpcomingPassedEventsViewModel()//use this Event model View since it has our categories in it 
            {
                Graduation = graduations
            });
        }

        public ActionResult SportsEvents()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var events = this.db.Events
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime)
                .Select(EventViewModel.ViewModel);

            //to get the list queried, you have to come here and in the MY page/controller
            //query according to event categories
            var sports = events.Where(e => e.EventTypeName == "Sports");
            return View(new UpcomingPassedEventsViewModel()//use this Event model View since it has our categories in it 
            {
                Sports = sports
            });
        }
        //list all the conference events:
        public ActionResult ConferenceEvents()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var events = this.db.Events
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime)
                .Select(EventViewModel.ViewModel);

            //to get the list queried, you have to come here and in the MY page/controller
            //query according to event categories
            var conference = events.Where(e => e.EventTypeName == "Conference");
            return View(new UpcomingPassedEventsViewModel()//use this Event model View since it has our categories in it 
            {
                Conference = conference
            });
        }
        //list all the Entertainment events:
        public ActionResult EntertainmentEvents()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var events = this.db.Events
                .Where(e => e.AuthorId == currentUserId)
                .OrderBy(e => e.StartDateTime)
                .Select(EventViewModel.ViewModel);

            //to get the list queried, you have to come here and in the MY page/controller
            //query according to event categories
            var entertainment = events.Where(e => e.EventTypeName == "Entertainment");
            return View(new UpcomingPassedEventsViewModel()//use this Event model View since it has our categories in it 
            {
                Entertainment = entertainment
            });
        }

        public ActionResult Participants()
        {
            //collect all the participants we have in Backendless
            //then afterwards, save them in them in our SQL
            //and then we gonna have to retreave them and display
            //them on the list of participants page and i think
            //that can do a trick.

            //load allContacts:
            DataQueryBuilder queryBuilder = DataQueryBuilder.Create();
            queryBuilder.AddGroupBy("name");

            //string WhereClause = "'id'='NOT NULL'";
            //we are using the table 'users' to fill Participants dictionary
            IList<Dictionary<string, object>> Participants = Backendless.Data.Of("Users").Find(queryBuilder);
            Dictionary<string, object> ParticipantObject = new Dictionary<string, object>();

            Profile userProfile = new Profile();

            foreach (var allPartis in Participants)
            {

                var p = new Profile()
                {
                    //since we are getting the object value associated with the specified key(s): 

                    Emailid = allPartis["email"].ToString(),
                    name = allPartis["name"].ToString(),
                    surname = allPartis["email"].ToString(),
                    id = allPartis["objectId"].ToString()
                };

                //save each object to the profile database in SQL before leaving the foreach
                if (allPartis.Equals(p.id))
                {
                    //save changes to database
                    this.db.ParticipantProfile.Add(p);
                    this.db.SaveChanges();
                }



            }
            //then afterwards save all of them. either inside this foreach or after this foreach is complete


            Participants.Any();

            string currentUserId = User.Identity.GetUserId();
            var participants = this.db.ParticipantProfile
                .Where(e => e.id != null)
                .Select(ParticipantModelView.ViewModel);

            var participators = participants.Where(e => e.id != null);

            return View(new AllParticipantList()
            {
                //return a list of all participants
                participants = participators

            });
        }

        //this is basically event creation and now we are about to create a sponsor.
        //let's get it!!!
        // GET: Events
        [HttpGet]
        public ActionResult Create()
        {
            using (ApplicationDbContext speakerEntity = new ApplicationDbContext())
            {


                var myDatabaseEF = new SelectList(speakerEntity.ParticipantProfile.Where(e => e.Speaker.id != null).ToList(),"id","name");
                ViewData["DBSpeakers"] = myDatabaseEF;
            }
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventInputModel model)
        {
            
            // assign an ID for backendless
            //save to backendless first
            //save objectId to our Event below            

            Dictionary<string, object> eventObj = new Dictionary<string, object>();
            Dictionary<string, object> saveEventObj;



            if (model != null && this.ModelState.IsValid)
            {
                //for backendless
                eventObj["name"] = model.Title;
                eventObj["description"] = model.Description;
                eventObj["startTime"] = model.StartDateTime;
                eventObj["endTime"] = model.endDateTime;
                eventObj["Location"] = model.Location;
                eventObj["EventType"] = model.EventType;
                eventObj["IsPublic"] = model.IsPublic;
                eventObj["MaxCount"] = model.maxCount; 
                //get count from model view from the form
                //save to backendless
                saveEventObj = Backendless.Data.Of("Event").Save(eventObj);
                
                
                //do something like this for participants
                var e = new Event()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Title = model.Title,
                    StartDateTime = model.StartDateTime,
                    endTime = model.endDateTime,
                    Duration = model.Duration,
                    Description = model.Description,
                    Location = model.Location,
                    IsPublic = model.IsPublic,
                    EventType = new EventType()
                    {
                        id = saveEventObj["objectId"].ToString(),
                        name = model.EventType
                    },
                    
                    Id = saveEventObj["objectId"].ToString(),
                    
                    //add speaker in here                       
                };
                //save to backendless

                this.db.Events.Add(e);
                this.db.SaveChanges();
                this.AddNotification("Event created.", NotificationType.INFO);
                return this.RedirectToAction("My");
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var eventToEdit = this.LoadEvent(id);
            if (eventToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }
            var model = EventInputModel.CreateFromEvent(eventToEdit);
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, EventInputModel model)
        {
            var eventToEdit = this.LoadEvent(id);
            if (eventToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            if (model != null && this.ModelState.IsValid)
            {
                //create backendless object and update fields


                eventToEdit.Title = model.Title;
                eventToEdit.StartDateTime = model.StartDateTime;
                eventToEdit.Duration = model.Duration;
                eventToEdit.Description = model.Description;
                eventToEdit.Location = model.Location;
                eventToEdit.IsPublic = model.IsPublic;

                string WhereClause = id;
                Dictionary<string, object> savedEvent = Backendless.Data.Of("Event").FindById(WhereClause);

                savedEvent["name"] = model.Title;
                savedEvent["startTime"] = model.StartDateTime;
                savedEvent["description"] = model.Description;

                Backendless.Persistence.Of("Event").Save(savedEvent);//update
                this.db.SaveChanges();
                this.AddNotification("Event edited.", NotificationType.INFO);
                return this.RedirectToAction("My");
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {

            var eventToDelete = this.LoadEvent(id);
            if (eventToDelete == null)
            {
                this.AddNotification("Cannot delete event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            var model = EventInputModel.CreateFromEvent(eventToDelete);
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, EventInputModel model)
        {
            //load id to objectId for backendless

            var eventToDelete = this.LoadEvent(id);
            if (eventToDelete == null)
            {
                this.AddNotification("Cannot delete event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }
            //delete from backendless using ID ERROR
            //Long result = Backendless.Data.Of("TABLE-NAME").Remove("WHERE ");
            //let's find an object first before we delete it.
            string WhereClause = "objectId = '" + id + "'";
            Backendless.Data.Of("Event").Remove(WhereClause);

            this.db.Events.Remove(eventToDelete);
            this.db.SaveChanges();
            this.AddNotification("Event deleted.", NotificationType.INFO);
            return this.RedirectToAction("My");
        }

        public ActionResult MailListing()
        {

            return View();
        }

        [HttpPost]
        public ActionResult MailListing(SendMailViewModel model)
        {
            MailMessage mm = new MailMessage("wilfleeta@gmail.com", model.To, model.Subject, model.Body);
            mm.Subject = model.Subject;
            mm.Body = model.Body;
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("wilfleeta@gmail.com", "fleet'a@2020");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = nc;
            smtp.Send(mm);
            this.AddNotification("Email Sent", NotificationType.INFO);
            return View();
        }


        private Event LoadEvent(string id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventToEdit = this.db.Events
                .Where(e => e.Id == id)
                .FirstOrDefault(e => e.AuthorId == currentUserId || isAdmin);
            return eventToEdit;
        }

        //this is basically event creation and now we are about to create a sponsor.
        //let's get it!!!

        public ActionResult CreateSpeaker()
        {
            //he had something in here for his recordCard, i don't
            return View();
        }

        //create speaker is successful
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSpeaker(SpeakerInputForModel model)
        {
            Dictionary<string, object> speakerObj = new Dictionary<string, object>();
            Dictionary<string, object> saveSpeakerObj;
                


            if (model != null && this.ModelState.IsValid)
            {
                //for backendless
                speakerObj["name"] = model.Name;
                speakerObj["surname"] = model.Surname;
                speakerObj["Phone"] = model.Phonenumber;
                speakerObj["email"] = model.email;
                speakerObj["password"] = "speakerPassword";

                //save to backendless
                saveSpeakerObj = Backendless.Data.Of("Users").Save(speakerObj);

                //do something like this for participants

                var s = new Profile()
                {
                    name = model.Name,
                    surname = model.Surname,
                    PhoneNumberid = model.Phonenumber,
                    Emailid = model.email,
                    Speaker = new Speaker()
                    {
                        id = saveSpeakerObj["objectId"].ToString(),
                        topic = "Event speaker"
                    },
                    id = saveSpeakerObj["objectId"].ToString()
                };

                //save to backendless

                this.db.ParticipantProfile.Add(s);
                this.db.SaveChanges();
                this.AddNotification("Event created.", NotificationType.INFO);
                return this.RedirectToAction("ListSpeakers");// this redirect doesn't seem to work, check!!!
            }
            return View(model);
        }


        //we have to list all our speakers now
        public ActionResult ListSpeakers()
        {
            string currentUserId = this.User.Identity.GetUserId();
            var ourSpeakers = this.db.ParticipantProfile
                .Where(e => e.Speaker.id != null)
                .OrderBy(e => e.name)
                .Select(SpeakerInputForModel.ViewModel);// check!!!!

            var allSpeakers = ourSpeakers.Where(e => e.id != null);

            //check the logic and test it out here:
            return View( new listOfSpeakers()
            {
                allSpeakers = allSpeakers
            });
        }


        //lets edit the speaker and get it over and done with!!!
        [HttpGet]
        [AllowAnonymous]
        public ActionResult EditSpeaker(string id)
        {
            var speakerToEdit = this.LoadSpeaker(id);
            if (speakerToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }
            var model = SpeakerInputForModel.CreateFromEvent(speakerToEdit);
            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult EditSpeaker(string id, SpeakerInputForModel model)
        {
            //this was used to load events. now lets one where we load speakers
            var speakerToEdit = LoadSpeaker(id);
            if (speakerToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("ListSpeakers");
            }

            if (model != null && this.ModelState.IsValid)
            {
                //create backendless object and update fields

                speakerToEdit.name = model.Name;
                speakerToEdit.surname = model.Surname;
                speakerToEdit.Emailid = model.email;
                speakerToEdit.PhoneNumberid = model.Phonenumber;

                BackendlessUser updateSpeaker = new BackendlessUser();
                //for backendless
                string WhereClause = id;
                Dictionary<string, object> savedSpeaker = Backendless.Data.Of("Users").FindById(WhereClause);

                updateSpeaker.SetProperty("name", model.Name);
                updateSpeaker.SetProperty("surname", model.Surname);
                updateSpeaker.SetProperty("Phone", model.Phonenumber);
                updateSpeaker.SetProperty("email", model.email);
                updateSpeaker.SetProperty("password", "SpeakerPassword");
                
                //is there a way we can get the id of this object!!!!????

                AsyncCallback<BackendlessUser> updateCallback = new AsyncCallback<BackendlessUser>(
                    speakerUpdate =>
                    {
                        //code here
                    },
                    fault =>
                    {
                        //code here
                    });

                 // user object retrieval is out of scope of this example
                Backendless.UserService.Update(updateSpeaker, updateCallback);

                Backendless.Persistence.Of("Users").Save(savedSpeaker);//update speaker...
                this.db.SaveChanges();
                this.AddNotification("Event edited.", NotificationType.INFO);
                return this.RedirectToAction("My");
            }
            return this.View(model);
        }


        //so basically we need to have a speaker loaded to the editing of it
        private Profile LoadSpeaker(string id)
        {

            var currentUserId = this.User.Identity.GetUserId();// we don't need this part..
            var isAdmin = this.IsAdmin();//we also don't need this part
            var speakerToEdit = db.ParticipantProfile
                .Where(e => e.id == id)
                .FirstOrDefault(e => e.id == id);


            return speakerToEdit as Profile;




        }

    }
}