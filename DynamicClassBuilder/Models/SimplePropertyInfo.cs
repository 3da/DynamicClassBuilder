using System;

namespace DynamicClassBuilder.Models
{
	public class SimplePropertyInfo : ISimplePropertyInfo
	{
		public string Name { get; set; }

		public Type Type { get; set; }
	}
}
