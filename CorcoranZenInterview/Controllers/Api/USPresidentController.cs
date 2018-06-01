using CorcoranZenInterview.Models;
using CorcoranZenInterview.Utils;
using ServiceModel;
using ServiceModel.USPresident;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Mvc;

namespace CorcoranZenInterview.Controllers.Api
{
   

    public class USPresidentController : ApiController
    {
        private IUSPresidentService service = new USPresidentService();

        /// <summary>
        /// Get List of US Presidents. See inner details for order by filter parameter.   
        /// </summary>    
        [System.Web.Http.HttpGet]
        [EnableQueryAttribute(AllowedOrderByProperties = "Birthday, DeathDay, IsDeath", EnsureStableOrdering =false)]
        [OrderByUriParameterAttribute(Description = "Will order by Birthday or Deathday in descending or ascending order. Ex: /api/USPresident?$orderby=DeathDay asc", Name = "$orderby={Birthday/DeathDay} {asc/desc}", Type = typeof(string))]
        public IQueryable<USPresidentModel> Get()
        {
            var presidentList = service.GetUSPresidentsFromGoogleSheet();
            return presidentList;
        }

        /// <summary>
        /// Get List of US Presidents filtered by name
        /// </summary> 
        /// <param name="name">The name that will be filtered.</param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public IQueryable<USPresidentModel> GetUSPresident (string name)
        {
            var president = service.GetUSPresidentsFromGoogleSheet(name);           
            return president;
        }
    }
}
