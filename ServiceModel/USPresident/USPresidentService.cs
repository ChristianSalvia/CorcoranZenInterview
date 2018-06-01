using CorcoranZenInterview.Models;
using ServiceModel.USPresident;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;


namespace ServiceModel
{
    public class USPresidentService : IUSPresidentService        
    {
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "CorcoranZenInterview";

        public SheetsService ManageCredentialsGoogle()
        {
            UserCredential credential;

            using (var stream =
                new MemoryStream(App_GlobalResources.Resource1.client_secret))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public IQueryable<USPresidentModel> GetUSPresidentsFromGoogleSheet(string nameFilter = null)
        {
            var service = ManageCredentialsGoogle();

            // Define request parameters.
            String spreadsheetId = "1i2qbKeasPptIrY1PkFVjbHSrLtKEPIIwES6m2l2Mdd8";
            String range = "Sheet1!A2:E";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            var USPresidentList = new List<USPresidentModel>();

            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    var fullName = row.Count > 0 ? row[0] as string: string.Empty;
                    var birthday = row.Count > 1 ? DateTime.Parse(row[1] as string) : (DateTime?)null;
                    var birthplace = row.Count > 2 ? row[2] as string : string.Empty;
                    var deathDay = row.Count > 3 ? DateTime.Parse(row[3] as string) : (DateTime?)null;
                    var deathPlace = row.Count > 4 ? row[4] as string: string.Empty;

                    USPresidentList.Add(new USPresidentModel()
                    {
                        FullName = fullName,
                        Birthday = birthday,
                        Birthplace = birthplace,
                        DeathDay = deathDay,
                        DeathPlace = deathPlace,
                        IsDeath = deathDay.HasValue ? true : false
                    });
                }
            }

            if (nameFilter != null)
            {
                StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase;

                USPresidentList = USPresidentList.Where(x => x.FullName.IndexOf(nameFilter, stringComparison) >= 0).ToList();
            }

            return USPresidentList.OrderByDescending(x => x.IsDeath).AsQueryable();
        }


    }
}
    