//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Events.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Profile
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string password { get; set; }
        public string id { get; set; }
        public string Emailid { get; set; }
        public string PhoneNumberid { get; set; }
        public string Addressid { get; set; }
        //public string profileEmail { get; set; }//just added this field also on SQL
    
        public virtual Address Address { get; set; }
        //public virtual Admin Admin { get; set; }
        //public virtual Email Email { get; set; }
        //public virtual Participant Participant { get; set; }
        //public virtual PhoneNumber PhoneNumber { get; set; }
        //public virtual Speaker Speaker { get; set; }
        //public virtual Sponsor Sponsor { get; set; }
    }
}
