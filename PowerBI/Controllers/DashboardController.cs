using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Security;
using PowerBI.Models;
using System.Threading.Tasks;
using Microsoft.Rest;
using Microsoft.PowerBI.Api.Beta;

namespace PowerBI.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Dashboard/

        private readonly string workspaceCollection;
        private readonly string workspaceId;
        private readonly string accessKey;
        private readonly string apiUrl;



        public DashboardController()
        {
            this.workspaceCollection = ConfigurationManager.AppSettings["powerbi:WorkspaceCollection"];
            this.workspaceId = ConfigurationManager.AppSettings["powerbi:WorkspaceId"];
            this.accessKey = ConfigurationManager.AppSettings["powerbi:AccessKey"];
            this.apiUrl = ConfigurationManager.AppSettings["powerbi:ApiUrl"];
        }


        public async Task<ActionResult> Report(string reportId)
        {

            var devToken = PowerBIToken.CreateDevToken(this.workspaceCollection, this.workspaceId);
            using (var client = this.CreatePowerBIClient(devToken))
            {
                var reportsResponse = await client.Reports.GetReportsAsync(this.workspaceCollection, this.workspaceId);
                var report = reportsResponse.Value.FirstOrDefault(r => r.Id == reportId);
                var embedToken = PowerBIToken.CreateReportEmbedToken(this.workspaceCollection, this.workspaceId, report.Id);

                var viewModel = new ReportViewModel
                {
                    Report = report,
                    AccessToken = embedToken.Generate(this.accessKey)
                };
                return View(viewModel);
            }

        }

        private IPowerBIClient CreatePowerBIClient(PowerBIToken token)
        {
            var jwt = token.Generate(accessKey);
            var credentials = new TokenCredentials(jwt, "AppToken");
            var client = new PowerBIClient(credentials)
            {
                BaseUri = new Uri(apiUrl)
            };

            return client;
        }

    }
}
