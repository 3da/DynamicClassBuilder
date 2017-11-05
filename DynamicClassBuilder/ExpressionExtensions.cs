using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DynamicClassBuilder
{
	public static class ExpressionExtensions
	{
		public static PropertyInfo GetPropertyInfo<TObj, TMember>(this Expression<Func<TObj, TMember>> expr)
		{
			MemberExpression member = expr.Body as MemberExpression;
			if (member == null)
				throw new ArgumentException($"Expression '{expr}' refers to a method, not a property.");

			PropertyInfo propInfo = member.Member as PropertyInfo;
			if (propInfo == null)
				throw new ArgumentException($"Expression '{expr}' refers to a field, not a property.");

			return propInfo;
		}

	}
}
