using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katrisk
{
	public class Point
	{
		public double X { get; internal set; }
		public double Y { get; internal set; }
		public Point(double x, double y)
		{
			X = x;
			Y = y;
		}
	}
}
