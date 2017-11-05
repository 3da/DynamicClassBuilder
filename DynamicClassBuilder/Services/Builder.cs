using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using DynamicClassBuilder.Models;

namespace DynamicClassBuilder.Services
{
	internal class Builder<T> : IBuilder<T>
	{
		private static Lazy<ModuleBuilder> _mouduleBuilder = new Lazy<ModuleBuilder>(BuildModule);

		private static ModuleBuilder ModuleBuilder => _mouduleBuilder.Value;


		private readonly Dictionary<PropertyInfo, LambdaExpression> _propertyImpl = new Dictionary<PropertyInfo, LambdaExpression>();

		private readonly Dictionary<Type, Delegate> _delegates = new Dictionary<Type, Delegate>();


		private static ModuleBuilder BuildModule()
		{
			AssemblyName assemblyName = new AssemblyName("DynamicTypes");
			AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
			return assemblyBuilder.DefineDynamicModule("DynamicTypes");
		}


		public IBuilder<T> Implement<TMember>(Expression<Func<T, TMember>> member, Expression<Func<TMember>> implementationBody)
		{
			_propertyImpl[member.GetPropertyInfo()] = implementationBody;
			return this;
		}

		public IBuilder<T> Implement<TMember>(Func<ISimplePropertyInfo, Expression<Func<TMember>>> func)
		{
			_delegates[typeof(TMember)] = func;
			return this;
		}

		public T Build()
		{
			var baseType = typeof(T).GetTypeInfo();

			var typeBuilder = baseType.IsInterface
					? ModuleBuilder.DefineType(typeof(T).Name + "DynamicImpl", TypeAttributes.Class, null, new Type[] { baseType })
					: ModuleBuilder.DefineType(typeof(T).Name + "DynamicImpl", TypeAttributes.Class, baseType);

			foreach (var propertyInfo in baseType.DeclaredProperties)
			{
				var propertyGetMethod = propertyInfo.GetMethod;
				if (!propertyGetMethod.IsVirtual)
					continue;

				var internalMethod = typeBuilder.DefineMethod(propertyGetMethod.Name + "Internal",
					MethodAttributes.Private | MethodAttributes.Static, propertyGetMethod.ReturnType, null);

				var prop = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.None, propertyInfo.PropertyType, null);

				var getter = typeBuilder.DefineMethod(propertyGetMethod.Name,
					MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual,
					propertyGetMethod.ReturnType, null);

				if (!_propertyImpl.TryGetValue(propertyInfo, out var impl))
				{
					Delegate deleg;

					if (!_delegates.TryGetValue(prop.PropertyType, out deleg))
						throw new Exception($"Implementation for type '{prop.PropertyType}' has not found");


					var simplePropInfo = new SimplePropertyInfo
					{
						Name = propertyInfo.Name,
						Type = propertyInfo.PropertyType
					};

					impl = (LambdaExpression)deleg.DynamicInvoke(simplePropInfo);

					var visitor = new InlinePropertyVisitor(simplePropInfo);

					impl = (LambdaExpression)visitor.Visit(impl);
				}


				impl.CompileToMethod(internalMethod);

				var ilGenerator = getter.GetILGenerator();

				ilGenerator.Emit(OpCodes.Call, internalMethod);
				ilGenerator.Emit(OpCodes.Ret);

				prop.SetGetMethod(getter);

				typeBuilder.DefineMethodOverride(getter, propertyGetMethod);
			}
			var resultType = typeBuilder.CreateTypeInfo();

			var instance = Activator.CreateInstance(resultType.AsType());

			return (T)instance;

		}
	}
}
