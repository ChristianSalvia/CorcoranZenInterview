using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorcoranZenInterview.Models
{
    public class USPresidentModel
    {
        public bool IsDeath {
            get; set;               
        }

        public string FullName { get; set; }
        public DateTime? Birthday { get; set; }
        public string Birthplace { get; set; }
        public DateTime? DeathDay { get; set; }
        public string DeathPlace { get; set; }
    }
}