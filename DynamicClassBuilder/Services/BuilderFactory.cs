namespace DynamicClassBuilder.Services
{
	public static class BuilderFactory
	{
		public static IBuilder<T> Create<T>()
		{
			return new Builder<T>();
		}
	}
}
