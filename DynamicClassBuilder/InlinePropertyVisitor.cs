using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DynamicClassBuilder.Models;

namespace DynamicClassBuilder
{
	public class InlinePropertyVisitor : ExpressionVisitor
	{
		private readonly ISimplePropertyInfo _propertyInfo;

		private readonly PropertyInfo _namePropInfo;

		public InlinePropertyVisitor(ISimplePropertyInfo propertyInfo)
		{
			_propertyInfo = propertyInfo;

			Expression<Func<ISimplePropertyInfo, string>> nameExpr = q => q.Name;

			_namePropInfo = nameExpr.GetPropertyInfo();
		}

		protected override Expression VisitMember(MemberExpression node)
		{
			if (node.Member == _namePropInfo)
			{
				return Expression.Constant(_propertyInfo.Name);
			}

			return base.VisitMember(node);
		}

		protected override Expression VisitLambda<T>(Expression<T> node)
		{
			return Expression.Lambda(Visit(node.Body));
		}
	}
}
