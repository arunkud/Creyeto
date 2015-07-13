//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class DoctorSchedule
    {
        public DoctorSchedule()
        {
            this.Appointments = new HashSet<Appointment>();
        }
    
        public int ID { get; set; }
        public int DoctorID { get; set; }
        public string DayInWeek { get; set; }
        public System.TimeSpan Duration { get; set; }
        public Nullable<System.TimeSpan> MorningStartTime { get; set; }
        public Nullable<int> MorningSlots { get; set; }
        public Nullable<System.TimeSpan> MorningEndTime { get; set; }
        public Nullable<System.TimeSpan> AfternoonStartTime { get; set; }
        public Nullable<int> AfternoonSlots { get; set; }
        public Nullable<System.TimeSpan> AfternoonEndTime { get; set; }
        public Nullable<System.TimeSpan> EveningStartTime { get; set; }
        public Nullable<int> EveningSlots { get; set; }
        public Nullable<System.TimeSpan> EveningEndTime { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Updated { get; set; }
        public Nullable<bool> IsActive { get; set; }
    
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
