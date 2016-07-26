using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.PowerBI.Api;

namespace PowerBI.Models
{
    public class ReportViewModel
    {
        public IReport Report { get; set; }
        public string AccessToken { get; set; }
    }
}