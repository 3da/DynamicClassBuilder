using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicClassBuilder;
using DynamicClassBuilder.Services;

namespace SampleApp
{
	public static class Configuration
	{


		private static readonly Lazy<IConfiguration> _lazy;

		public static IConfiguration Instance => _lazy.Value;

		static Configuration()
		{
			_lazy = new Lazy<IConfiguration>(() =>
			{
				var dynamicConfigBuilder = BuilderFactory.Create<IConfiguration>()
					.Implement(q => q.AnotherDateTime, () => DateTime.UtcNow)
					.Implement<string>(q => () => ConfigurationManager.AppSettings[q.Name])
					.Implement<int>(q => () => Convert.ToInt32(ConfigurationManager.AppSettings[q.Name]))
					.Implement<DateTime>(q => () => DateTime.Parse(ConfigurationManager.AppSettings[q.Name]));

				return dynamicConfigBuilder.Build();
			});
		}
	}
}
