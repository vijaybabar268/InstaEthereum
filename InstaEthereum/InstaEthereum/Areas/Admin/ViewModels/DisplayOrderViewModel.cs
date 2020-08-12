using InstaEthereum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InstaEthereum.Areas.Admin.ViewModels
{
    public class DisplayOrderViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
    }
}