using System;
using System.ComponentModel.DataAnnotations;

namespace ecom.presentation.website.Models
{
	[MetadataType(typeof(DoctorMetaData))]
	public partial class Doctor
	{
        public TimeSpan? SundayMorningStartTime { get; set; }
        public TimeSpan? SundayMorningEndTime { get; set; }
        public TimeSpan? SundayAfternoonStartTime { get; set; }
        public TimeSpan? SundayAfternoonEndTime { get; set; }
        public TimeSpan? SundayEveningStartTime { get; set; }
        public TimeSpan? SundayEveningEndTime { get; set; }

        public TimeSpan? MondayMorningStartTime { get; set; }
        public TimeSpan? MondayMorningEndTime { get; set; }
        public TimeSpan? MondayAfternoonStartTime { get; set; }
        public TimeSpan? MondayAfternoonEndTime { get; set; }
        public TimeSpan? MondayEveningStartTime { get; set; }
        public TimeSpan? MondayEveningEndTime { get; set; }

        public TimeSpan? TuesdayMorningStartTime { get; set; }
        public TimeSpan? TuesdayMorningEndTime { get; set; }
        public TimeSpan? TuesdayAfternoonStartTime { get; set; }
        public TimeSpan? TuesdayAfternoonEndTime { get; set; }
        public TimeSpan? TuesdayEveningStartTime { get; set; }
        public TimeSpan? TuesdayEveningEndTime { get; set; }

        public TimeSpan? WednesdayMorningStartTime { get; set; }
        public TimeSpan? WednesdayMorningEndTime { get; set; }
        public TimeSpan? WednesdayAfternoonStartTime { get; set; }
        public TimeSpan? WednesdayAfternoonEndTime { get; set; }
        public TimeSpan? WednesdayEveningStartTime { get; set; }
        public TimeSpan? WednesdayEveningEndTime { get; set; }

        public TimeSpan? ThursdayMorningStartTime { get; set; }
        public TimeSpan? ThursdayMorningEndTime { get; set; }
        public TimeSpan? ThursdayAfternoonStartTime { get; set; }
        public TimeSpan? ThursdayAfternoonEndTime { get; set; }
        public TimeSpan? ThursdayEveningStartTime { get; set; }
        public TimeSpan? ThursdayEveningEndTime { get; set; }

        public TimeSpan? FridayMorningStartTime { get; set; }
        public TimeSpan? FridayMorningEndTime { get; set; }
        public TimeSpan? FridayAfternoonStartTime { get; set; }
        public TimeSpan? FridayAfternoonEndTime { get; set; }
        public TimeSpan? FridayEveningStartTime { get; set; }
        public TimeSpan? FridayEveningEndTime { get; set; }

        public TimeSpan? SaturdayMorningStartTime { get; set; }
        public TimeSpan? SaturdayMorningEndTime { get; set; }
        public TimeSpan? SaturdayAfternoonStartTime { get; set; }
        public TimeSpan? SaturdayAfternoonEndTime { get; set; }
        public TimeSpan? SaturdayEveningStartTime { get; set; }
        public TimeSpan? SaturdayEveningEndTime { get; set; }        
    }

	public class DoctorMetaData
	{
		[ScaffoldColumn(false)]
		public int ReviewCount { get; set; }

		[ScaffoldColumn(false)]
		//v[RegularExpression([a])]
		public int LikeCount { get; set; }

        [Required]
        [Display(Name ="Name")]
        public string NameEN { get; set; }

        [Required]
        public string Qualification { get; set; }

        [Required]
        public Nullable<decimal> Fee { get; set; }

        [Required]
        public int SpecialityID { get; set; }

        [Required]
        public int HospitalID { get; set; }

        [Display(Name ="Experience since")]
        public Nullable<System.DateTime> ExperienceFrom { get; set; }
    }
}