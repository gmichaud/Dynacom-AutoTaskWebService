﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DynacomAutoTaskWebService
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DynacomDataContext : DbContext
    {
        public DynacomDataContext()
            : base("name=DynacomDataContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DynaAutoTasks> DynaAutoTasks { get; set; }
        public virtual DbSet<DynaUsers> DynaUsers { get; set; }
    }
}
