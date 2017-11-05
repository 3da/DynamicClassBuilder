using System;

namespace DynamicClassBuilder.Models
{
	public interface ISimplePropertyInfo
	{
		string Name { get; }

		Type Type { get; }
	}
}
