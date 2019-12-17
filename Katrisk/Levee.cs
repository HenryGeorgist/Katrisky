using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katrisk
{
	class Levee
	{
		private string _name;
		private List<Ordinate> _data;
		public string name
		{
			get
			{
				return _name;
			}
			internal set
			{
				_name = value;
			}
		}
		public Levee(string name)
		{
			_name = name;
			_data = new List<Ordinate>();
		}
		public bool addRow(string[] vals)
		{
			if (String.IsNullOrEmpty(_name))
			{
				name = vals[0];
			}
			Ordinate kl = new Ordinate(vals);
			_data.Add(kl);
			return kl.isValid;
		}
		internal void sort()
		{
			//implemented sortable on KatriskLine
			_data.Sort();
		}
		internal List<Point> convertToPoints()
		{
			List < Point > points = new List<Point>();
			foreach (Ordinate o in _data)
			{
				points.Add(o.convertToPoint());
			}
			return points;
		}
		internal void pointsToData(List<Point> points)
		{
			_data = new List<Ordinate>();
			foreach (Point p in points)
			{
				_data.Add(new Ordinate(p));
			}
		}
		internal StringBuilder writeCurve()
		{
			return computeEP();
		}

		private List<Point> VisvaligamWhyattSimplify(int numToKeep, List<Point> points)
		{
			//'http://bost.ocks.org/mike/simplify/
			int removeLimit = _data.Count - numToKeep;
			int minIndex;
			double minArea;
			double tmpArea;
			for (int i = 0; i < removeLimit; i++)
			{
				minIndex = 1;
				minArea = area(points[0], points[1], points[2]);
				for (int j = 2; j < points.Count - 1; j++)
				{
					tmpArea = area(points[j - 1], points[j], points[j + 1]);
					if (tmpArea < minArea)
					{
						minIndex = j;
						minArea = tmpArea;
					}
				}
				points.RemoveAt(minIndex);
			}
			return points;

		}
		private double area(Point point1, Point point2, Point point)
		{
			return Math.Abs((point1.X * point2.Y + point2.X * point.Y + point.X * point1.Y - point2.X * point1.Y - point.X * point2.Y - point1.X * point.Y) * 0.5);
		}
		public StringBuilder computeEP()
		{
			sort();
			//aggregate 
			List<Point> points = convertToPoints();
			List<Point> simpleData = VisvaligamWhyattSimplify(500, points);
			pointsToData(simpleData);
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("X,Y");
			foreach (Point p in simpleData)
			{
				sb.AppendLine(p.X + "," + p.Y);
			}
			return sb;
			//thin based on VW with normalized probability
		}
	}
}
