using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using PagedList;

namespace Team1CentricAplication.Models
{
    public class Values
    {
        public int valuesId { get; set; }

        [Display(Name = "Award")]
        [Required(ErrorMessage = "Entry required")]
        public CoreValue nominatedValues { get; set; }

        [Display(Name = "Award Nominee")]
        //[Required(ErrorMessage = "Entry required")]

        public Guid profilesID { get; set; }

        [Display(Name = "Award Nominator")]

        public Guid? recognizor { get; set; }


        [Display(Name = "Recognition Note")]
        [Required(ErrorMessage = "Entry required")]
        public string recognitionNote { get; set; }

        [Display(Name = "Date of Recognition")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]


        public DateTime? recognizationDate { get; set; }
        public enum CoreValue
        {
            Excellence = 1,
            Integrity = 2,
            Stewardship = 3,
            Innovate = 4,
            Balance = 5
        }
        [ForeignKey("recognizor")]
        public virtual Profiles AwardNominator { get; set; }
        [ForeignKey("profilesID")]
        public virtual Profiles AwardRecipient { get; set; }

        [ForeignKey("profilesID")]
        public virtual Profiles AwardNominee { get; set; }

        public ICollection<Profiles> Profiles { get; set; }
    }
}