namespace Cubipool.Entity
{
	public class Constant
	{
		public int Id { get; set; }
		public int RelatedTableId { get; set; }
		public string RelatedTableName { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Position { get; set; }

		public bool IsInternal { get; set; }
		public bool IsEditable { get; set; }
		public bool IsEnabled { get; set; }
		public int? Dependency { get; set; }
	}
}