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
    
    public partial class GetAllDoctors_Result
    {
        public int ID { get; set; }
        public string NameEN { get; set; }
        public string Qualification { get; set; }
        public string Location { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<decimal> Fee { get; set; }
        public int ReviewCount { get; set; }
        public Nullable<bool> HasImage { get; set; }
    }
}
