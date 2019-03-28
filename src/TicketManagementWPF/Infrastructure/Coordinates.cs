namespace TicketManagementWPF.Infrastructure
{
	internal struct Coordinates
	{
		public int Row { get; private set; }
		public int Column { get; private set; }

		public Coordinates(int row, int column)
		{
			Row = row;
			Column = column;
		}
	}
}
