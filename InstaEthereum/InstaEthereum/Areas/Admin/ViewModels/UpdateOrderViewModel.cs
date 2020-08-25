using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace InstaEthereum.Areas.Admin.ViewModels
{
    public class UpdateOrderViewModel
    {
        public int OrderId { get; set; }

        public IEnumerable<MyDropDown> TransactionStatus { get; set; }

        [Display(Name = "Transaction Status")]
        public byte TransactionStatusId { get; set; }

        [Display(Name = "Transaction No")]
        public string EthTxnNo { get; set; }
    }

    public class MyDropDown
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}