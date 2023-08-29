using System.ComponentModel.DataAnnotations;

namespace CalifornianHealthFrontendUpdated.Models
{
    public class Consultant : CalifornianHealthLib.Models.Consultant
    {
        [StringLength(100)] public string DropDownTitle { get; set; } = null;
        
    }
}