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
    
    public partial class UserLike
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int DoctorID { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Updated { get; set; }
        public bool IsActive { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}
