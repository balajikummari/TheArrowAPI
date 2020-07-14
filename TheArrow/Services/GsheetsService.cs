using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System.IO;

namespace TheArrow.Services
{
    public class GsheetsService
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets, DriveService.Scope.Drive };
        static readonly string ApplicationName = "TheArrowApi";

        static SheetsService sheetService;
        static DriveService driveService;

        public GsheetsService()
        {
            InitService();
        }

        private void InitService()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("gsheets_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }

            sheetService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        public string CreateSheet(string BusinessName)
        {
            Spreadsheet localSh = new Spreadsheet();
            localSh.Sheets = new List<Sheet>();
            localSh.Sheets.Add(new Sheet()
            {
                Properties = new SheetProperties()
                {
                    Title = "Products"
                }
            });

            localSh.Properties = new SpreadsheetProperties()
            {
                Title = BusinessName
            };

            Spreadsheet spreadsheet = sheetService.Spreadsheets.Create(localSh).Execute();

            Permission userPermission = new Permission()
            {
                Type = "user",
                Role = "writer",
                EmailAddress = "balajibobby3@gmail.com"
            };

            driveService.Permissions.Create(userPermission, spreadsheet.SpreadsheetId).Execute();
            return spreadsheet.SpreadsheetUrl;
        }

    }
}
