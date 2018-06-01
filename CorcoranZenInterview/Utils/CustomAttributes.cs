using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorcoranZenInterview.Utils
{

    public class OrderByUriParameterAttribute : Attribute
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public string Description { get; set; }
    }


}