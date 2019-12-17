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
			//readDataFromNewFiles();
			readDataFromOriginalFile();

		}
		private static void readDataFromOriginalFile()
		{
			List<LeveeSet> data = new List<LeveeSet>();
			string[] paths = { @"C:\Users\Q0HECWPL\Documents\FEMA\Katrisk_comparison\Agree_Orig_FluvialOnly_FullEP_ByLeveeID.csv" };
			foreach (string path in paths)
			{
				string fn = System.IO.Path.GetFileNameWithoutExtension(path);
				using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
				{
					sr.ReadLine();
					string[] line;
					LeveeSet currentSet = null;
					while (!sr.EndOfStream)
					{
						line = sr.ReadLine().Split(',');
						bool match = false;
						if (currentSet != null && currentSet.name.Equals(line[0]))
						{
							match = true;
						}
						else
						{
							foreach (LeveeSet ls in data)
							{
								if (ls.name.Equals(line[0]))
								{
									currentSet = ls;
									match = true;
									break;
								}
							}
						}
						if (!match)
						{
							currentSet = new LeveeSet(line[0]);
							data.Add(currentSet);
						}
						currentSet.addData(fn, line);
					}
				}
				Parallel.For(0, data.Count,
					index =>
					{
						foreach (Levee l in data[index].levees.Values)
						{
							System.Console.WriteLine("Levee: " + l.name);
							foreach (Ordinate o in l.ordinates)
							{
								foreach (string m in o.messages)
								{
									System.Console.WriteLine(m);
								}
							}
						}
						//data[index].writeCurve(fn);
					}
				);
			}
		}
		private static void readDataFromNewFiles()
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
			foreach (LeveeSet ls in data)
			{
				Point diff = ls.maxDifference();
				if (Math.Abs(diff.Y) > 100)
				{
					System.Console.WriteLine("A max difference of " + diff.Y + " occured at probability " + diff.X + " for levee " + ls.name);
				}
			}
			Console.Read();
		}
	}
}
