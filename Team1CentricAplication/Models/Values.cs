using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team1CentricAplication.Models
{
    public class Values
    {
        public int valuesId { get; set; }
        
        public string nominatedValues { get; set; }
        public Guid profilesID { get; set; }
        public virtual Profiles Profiles { get; set; }


    }
}