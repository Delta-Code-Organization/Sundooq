//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SundooqLanding.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Users
    {
        public Users()
        {
            this.History = new HashSet<History>();
            this.Emails = new HashSet<Email>();
        }
    
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Tags { get; set; }
        public Nullable<int> AccountStatus { get; set; }
        public Nullable<int> RegisteredWith { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<int> Gender { get; set; }
        public string IgnoredTags { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public Nullable<System.DateTime> Registered { get; set; }
    
        public virtual ICollection<History> History { get; set; }
        public virtual ICollection<Email> Emails { get; set; }
    }
}
