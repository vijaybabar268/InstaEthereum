using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaEthereum.Areas.Admin.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }

        /*Orders*/
        public int TotalOrders { get; set; }

        public int pendingOrders { get; set; }

        public int CompleteOrders { get; set; }

        public int InCompleteOrders { get; set; }
    }
}