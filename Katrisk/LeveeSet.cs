using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katrisk
{
	public class LeveeSet
	{
		private Dictionary<string, Levee> _set;
		private string _name;
		public string name { get { return _name;} internal set { _name = value; } }
		public LeveeSet(string inputName)
		{
			name = inputName;
			_set = new Dictionary<string, Levee>();
		}
		public Dictionary<string, Levee> levees { get { return _set; } }
		public bool addData(string alternative, string[] row)
		{
			if (row[0].Equals(_name))
			{
				if (!_set.Keys.Contains(alternative))
				{
					_set.Add(alternative, new Levee(_name));
				}
				return _set[alternative].addRow(row);
			}
			return false;
		}

		internal void writeCurve(string fn, string directory)
		{
			string fName = name.Replace("\"", "");
			fName = fName.Replace(";", "-");
			string path = directory + fn + "\\";
			if (!System.IO.Directory.Exists(path)) { System.IO.Directory.CreateDirectory(path); }
			if (!System.IO.File.Exists(path + fName + ".csv"))
			{
				using (System.IO.StreamWriter sw = new System.IO.StreamWriter(path + fName + ".csv"))
				{
					sw.Write(_set[fn].writeCurve());
				}
			}
			else
			{
				//skip the file - it has already been updated.
			}
		}
		public void readCurve(string fn, string n)
		{
			name = n;
			string path = @"C:\Temp\KRisk\" + fn + "\\" + name + ".csv";
			if (System.IO.File.Exists(path))
			{
				string[] lineArray;
				List<Point> pointArray = new List<Point>();
				using (System.IO.StreamReader sw = new System.IO.StreamReader(path))
				{
					sw.ReadLine();
					while (!sw.EndOfStream)
					{
						lineArray = sw.ReadLine().Split(',');
						pointArray.Add(new Point(Double.Parse(lineArray[0]), Double.Parse(lineArray[1])));
					}
				}
				_set.Add(fn, new Levee(name));
				_set[fn].pointsToData(pointArray);
			}
		}
		public Point maxDifference()
		{
			double diff = 0.0;
			double diffPer = 0.0;
			if (_set.Values.Count < 2) return new Point(diff, diff);
			List<List<Point>> stdLevees = new List<List<Point>>();
			List<double> stpb = _set.Values.Last().standardProbs;
			foreach (Levee l in _set.Values)
			{
				stdLevees.Add(l.getStandardCurve());
			}
			double tmp = 0;
			int idx = 0;
			for (int i = 0; i < stpb.Count; i++)
			{
				// not sure how to deal with many levees...
				double first = stdLevees[0][i].Y;
				double second = stdLevees[1][i].Y;
				double max = Math.Max(first, second);
				double min = Math.Min(first, second);

				tmp =  max - min ;
				if (diff < tmp)
				{
					diff = tmp;
					diffPer = tmp / max;
					idx = i;
				}
			}
			return new Point(stpb[idx],diffPer);
		}
	}
}
