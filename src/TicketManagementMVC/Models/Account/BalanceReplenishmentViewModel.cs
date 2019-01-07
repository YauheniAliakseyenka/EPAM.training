using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TicketManagementMVC.Infrastructure;
using TicketManagementMVC.Infrastructure.Attributes;

namespace TicketManagementMVC.Models
{
    public class BalanceReplenishmentViewModel
    {
		[Display(ResourceType = typeof(I18N.Resource), Name = "AmounLabel")]
		[DataType(DataType.Currency)]
		public decimal Amount { get; set; }
    }
}