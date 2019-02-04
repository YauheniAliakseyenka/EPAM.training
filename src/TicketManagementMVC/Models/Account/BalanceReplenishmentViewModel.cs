using System.ComponentModel.DataAnnotations;

namespace TicketManagementMVC.Models
{
    public class BalanceReplenishmentViewModel
    {
		[Display(ResourceType = typeof(ProjectResources.AccountResource), Name = "Amount")]
		[DataType(DataType.Currency)]
		public decimal Amount { get; set; }
    }
}