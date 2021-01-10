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

            var upcomingEvents = events.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = events.Where(e => e.StartDateTime <= DateTime.Now);
            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
        }

        // GET: Events
        [HttpGet]
        public ActionResult Create()
        {
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
                //save to backendless
                saveEventObj = Backendless.Data.Of("Event").Save(eventObj);


                var e = new Event()
                {
                    AuthorId = this.User.Identity.GetUserId(),
                    Title = model.Title,
                    StartDateTime = model.StartDateTime,
                    Duration = model.Duration,
                    Description = model.Description,
                    Location = model.Location,
                    IsPublic = model.IsPublic,
                    Id = saveEventObj["objectId"].ToString()


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


        private Event LoadEvent(string id)
        {
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventToEdit = this.db.Events
                .Where(e => e.Id == id)
                .FirstOrDefault(e => e.AuthorId == currentUserId || isAdmin);
            return eventToEdit;
        }


    }
}