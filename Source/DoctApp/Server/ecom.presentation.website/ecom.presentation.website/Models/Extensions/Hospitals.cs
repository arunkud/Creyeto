using System.ComponentModel.DataAnnotations;

namespace ecom.presentation.website.Models
{
    [MetadataType(typeof(HospitalMetaData))]
    public partial class Hospital
    {

    }

    public class HospitalMetaData
    {
        [ScaffoldColumn(false)]
        public int ReviewCount { get; set; }

        [ScaffoldColumn(false)]
        //v[RegularExpression([a])]
        public int LikeCount { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string NameEN { get; set; }        
    }
}