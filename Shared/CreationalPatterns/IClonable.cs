using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
	interface IClonable<T>
	{
		public T DeepClone();
		public T ShallowClone();
	}
}
