using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public interface IPoint
	{
        PointType Type { get; }
        ushort Address { get; }
        ushort RawValue { get; }
		string DisplayValue { get; }
	}
}
