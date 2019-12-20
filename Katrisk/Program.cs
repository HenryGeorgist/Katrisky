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

			if (args.Length < 2)
			{

				string process = args[0];
				if (process.Equals("Simplify"))
				{
					string filePath = args[1];
					if (args.Length < 3)
					{
						Console.WriteLine("Need three arguments for Simplify");
					}
					else
					{
						string directory = args[3];
						if (!System.IO.Directory.Exists(directory))
						{
							Console.WriteLine("Creating " + directory);
							System.IO.Directory.CreateDirectory(directory);
						}
						if (System.IO.File.Exists(filePath))
						{
							if (System.IO.Path.GetExtension(filePath).Equals(".csv"))
							{
								readDataFromOriginalFile(filePath, directory);
							}
							else
							{
								System.Console.WriteLine("Expected .csv got " + filePath);
							}
						}
						else
						{
							System.Console.WriteLine(filePath + " does not exist");
						}
					}
				}
				else if (process.Equals("Compare"))
				{
					string dir = args[1];
					if (System.IO.Directory.Exists(dir))
					{
						readDataFromNewFiles(dir);
					}
					else
					{
						System.Console.WriteLine(dir + " does not exist, try Simplify first, or make sure the directory matches the directory you created.");
					}


				}
				else
				{
					Console.WriteLine("Expected Simplify or Compare got: " + args[0]);
				}
				Console.WriteLine("Process " + process + " complete");
			}
			else
			{
				Console.Write("Expected at least 2 arguments, refer to directions and try again");
			}
			
			Console.Read();
		}
		private static void readDataFromOriginalFile(string filePath, string directory)
		{
			List<LeveeSet> data = new List<LeveeSet>();
			string[] paths = { filePath };
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
						////check for errors in the original files
						//foreach (Levee l in data[index].levees.Values)
						//{
						//	System.Console.WriteLine("Levee: " + l.name);
						//	foreach (Ordinate o in l.ordinates)
						//	{
						//		foreach (string m in o.messages)
						//		{
						//			System.Console.WriteLine(m);
						//		}
						//	}
						//}
						data[index].writeCurve(fn, directory);
					}
				);
			}
		}
		private static void readDataFromNewFiles(string directory)
		{

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
				if (Math.Abs(diff.Y) > 0.1)
				{
					System.Console.WriteLine("A max difference of " + diff.Y + " occured at probability " + diff.X + " for levee " + ls.name);
				}
			}
			//Console.Read();
		}
	}
}
