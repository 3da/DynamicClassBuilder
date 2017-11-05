using System;
using System.Linq.Expressions;
using DynamicClassBuilder.Models;

namespace DynamicClassBuilder.Services
{
	public interface IBuilder<T>
	{
		IBuilder<T> Implement<TMember>(Expression<Func<T, TMember>> member, Expression<Func<TMember>> implementationBody);

		IBuilder<T> Implement<TMember>(Func<ISimplePropertyInfo, Expression<Func<TMember>>> func);

		T Build();
	}
}
