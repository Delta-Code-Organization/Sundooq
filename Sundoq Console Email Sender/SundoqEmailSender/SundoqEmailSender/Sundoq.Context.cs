﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SundoqEmailSender
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SundooqDBEntities : DbContext
    {
        public SundooqDBEntities()
            : base("name=SundooqDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Admins> Admins { get; set; }
        public DbSet<Emails> Emails { get; set; }
        public DbSet<Faq> Faq { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Sources> Sources { get; set; }
        public DbSet<Topics> Topics { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
