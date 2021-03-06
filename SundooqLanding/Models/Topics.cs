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
    
    public partial class Topics
    {
        public Topics()
        {
            this.History = new HashSet<History>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string EncodedTitle { get; set; }
        public Nullable<int> Source { get; set; }
        public string Descr { get; set; }
        public Nullable<System.DateTime> PubDate { get; set; }
        public Nullable<int> FB { get; set; }
        public Nullable<int> TW { get; set; }
        public Nullable<int> LocalViews { get; set; }
        public Nullable<int> LocalShares { get; set; }
        public Nullable<int> Rank { get; set; }
        public string Tags { get; set; }
        public string URL { get; set; }
        public string Img { get; set; }
    
        public virtual ICollection<History> History { get; set; }
        public virtual Sources Sources { get; set; }
    }
}
