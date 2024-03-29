﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackendlessAPI;

namespace Events.Data
{
    public class Event
    {
        public Event()
        {
            this.Comments = new HashSet<Comment>();
            this.IsPublic = true;
            this.StartDateTime = DateTime.Now;
        }

        public string Id { get; set; }

        public string objectId { get; set;}

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        public TimeSpan? Duration { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public string Description { get; set; }

        [MaxLength(200)]
        public string Location { get; set; }

        public bool IsPublic { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
