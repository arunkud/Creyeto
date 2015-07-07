﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ecom.presentation.website.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class ecomEntities : DbContext
    {
        public ecomEntities()
            : base("name=ecomEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DoctorView> DoctorViews { get; set; }
    
        public virtual ObjectResult<GetAllDoctors_Result> GetAllDoctors(Nullable<int> cityID, Nullable<int> cityLocationID, Nullable<int> specialityID)
        {
            var cityIDParameter = cityID.HasValue ?
                new ObjectParameter("cityID", cityID) :
                new ObjectParameter("cityID", typeof(int));
    
            var cityLocationIDParameter = cityLocationID.HasValue ?
                new ObjectParameter("cityLocationID", cityLocationID) :
                new ObjectParameter("cityLocationID", typeof(int));
    
            var specialityIDParameter = specialityID.HasValue ?
                new ObjectParameter("specialityID", specialityID) :
                new ObjectParameter("specialityID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAllDoctors_Result>("GetAllDoctors", cityIDParameter, cityLocationIDParameter, specialityIDParameter);
        }
    
        public virtual int GetAvailableSlots(Nullable<int> doctorID, Nullable<System.DateTime> date)
        {
            var doctorIDParameter = doctorID.HasValue ?
                new ObjectParameter("DoctorID", doctorID) :
                new ObjectParameter("DoctorID", typeof(int));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetAvailableSlots", doctorIDParameter, dateParameter);
        }
    }
}