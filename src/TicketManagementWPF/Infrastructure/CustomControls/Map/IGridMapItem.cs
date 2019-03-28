namespace TicketManagementWPF.Infrastructure.CustomControls.Map
{
	/// <summary>
	/// Needs to place model's item in a grid cell
	/// </summary>
	public interface IGridMapItem
	{
		int Row { get; set; }
		int Column { get; set; }
	}
}
