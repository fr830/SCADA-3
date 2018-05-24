using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
	public interface IStorage
	{
		List<IPoint> GetPoints(List<Tuple<ushort, ushort>> pointIds);
	}
}
