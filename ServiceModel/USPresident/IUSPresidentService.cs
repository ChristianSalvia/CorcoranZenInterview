using CorcoranZenInterview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel.USPresident
{
    public interface IUSPresidentService
    {
        IQueryable<USPresidentModel> GetUSPresidentsFromGoogleSheet(string nameFilter = null);
    }
}
