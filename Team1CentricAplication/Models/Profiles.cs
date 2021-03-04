using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team1CentricAplication.Models
{
    public class Profiles
    {
            public int profilesID { get; set; }

            [Display(Name = "First Name")]
            [Required(ErrorMessage = "Entry required - First name is required")]

            public string firstName { get; set; }

            [Display(Name = "Last Name")]
            [Required(ErrorMessage = "Entry required - First name is required")]
            public string lastName { get; set; }

            [Display(Name = "Phone Number")]
            [Required]
            [StringLength(12)]
            public string phone { get; set; }

            [Display(Name = "Email Address")]
            [Required]
            [DataType(DataType.EmailAddress)]
            public string email { get; set; }

            [Display(Name = "Location")]
            public string city { get; set; }

            [Display(Name = "Zip Code")]
            [Required]
            [StringLength(5)]
            public string zip { get; set; }

            [Display(Name = "Profile Name")]
            public string fullName
            {
                get
                {
                    return lastName + ", " + firstName;
                }
            }

        
    }
}