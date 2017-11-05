using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApp
{
	public interface IConfiguration
	{
		int SampleInt { get; }

		string SampleString { get; }

		DateTime SampleDateTime { get; }

		DateTime AnotherDateTime { get; }
	}
}
