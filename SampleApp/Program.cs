using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DynamicClassBuilder;

namespace SampleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Loading configuration");

			var config = Configuration.Instance;

			Console.WriteLine($"{nameof(config.SampleDateTime)}: {config.SampleDateTime}");
			Console.WriteLine($"{nameof(config.AnotherDateTime)}: {config.AnotherDateTime}");
			Console.WriteLine($"{nameof(config.SampleInt)}: {config.SampleInt}");
			Console.WriteLine($"{nameof(config.SampleString)}: {config.SampleString}");

			Console.ReadLine();
		}
	}
}
