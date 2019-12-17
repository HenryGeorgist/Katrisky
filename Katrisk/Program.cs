using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katrisk
{
	class Program
	{
		static void Main(string[] args)
		{
			readDataFromFiles();
			//List<LeveeSet> data = new List<LeveeSet>();
			//string[] paths = { @"C:\Users\Q0HECWPL\Documents\FEMA\Katrisk_comparison\Agree_v23_FullEP_ByLeveeID.csv" };
			//foreach (string path in paths)
			//{
			//	string fn = System.IO.Path.GetFileNameWithoutExtension(path);
			//	using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
			//	{
			//		sr.ReadLine();
			//		string[] line;
			//		LeveeSet currentSet = null;
			//		while (!sr.EndOfStream)
			//		{
			//			line = sr.ReadLine().Split(',');
			//			bool match = false;
			//			if (currentSet != null && currentSet.name.Equals(line[0]))
			//			{
			//				match = true;
			//			}
			//			else
			//			{
			//				foreach (LeveeSet ls in data)
			//				{
			//					if (ls.name.Equals(line[0]))
			//					{
			//						currentSet = ls;
			//						match = true;
			//						break;
			//					}
			//				}
			//			}
			//			if (!match)
			//			{
			//				currentSet = new LeveeSet(line[0]);
			//				data.Add(currentSet);
			//			}
			//			currentSet.addData(fn, line);
			//		}
			//	}
			//Parallel.For(0, data.Count,
			//	index =>
			//	{
			//		data[index].writeCurve(fn);
			//	}
			//);
			//}

		}
		private static void readDataFromFiles()
		{
			string directory = @"C:\Temp\KRisk\";
			List<LeveeSet> data = new List<LeveeSet>();
			IEnumerable<string> alts = System.IO.Directory.EnumerateDirectories(directory);
			foreach (string a in alts)
			{
				IEnumerable<string> leveeFiles = System.IO.Directory.EnumerateFiles(a + "\\");
				foreach (string l in leveeFiles)
				{
					LeveeSet set = null;
					bool match = false;
					string lName = System.IO.Path.GetFileNameWithoutExtension(l);
					string aName = System.IO.Path.GetFileNameWithoutExtension(a);
					foreach (LeveeSet s in data)
					{
						if (s.name.Equals(lName))
						{
							match = true;
							set = s;
							break;
						}
					}
					if (!match)
					{
						set = new LeveeSet(lName);
						data.Add(set);
					}
					set.readCurve(aName, lName);
				}
			}
		}
	}
}
