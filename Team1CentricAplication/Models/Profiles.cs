using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using PagedList;

namespace Team1CentricAplication.Models
{
    public class Profiles
    {
        public Guid profilesID { get; set; }

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

        [Display(Name = "Profile Picture")]
        public string profilePicture { get; set; }

        [Display(Name = "User's role")]
        public roles role { get; set; }
        public enum roles
        {
            admin = 0,
            employee = 1

        }

   

        //public Guid recognizor { get; set; }

        //public virtual Values recognizor  { get; set; }
        [ForeignKey("recognizor")]
        public ICollection<Values> AwardNominator { get; set; }
        [ForeignKey("profilesID")]
        public ICollection<Values> AwardRecipient { get; set; }
    }
}