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
		private List<KatriskLine> _data;
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
			_data = new List<KatriskLine>();
		}
		public bool addRow(string[] vals)
		{
			if (String.IsNullOrEmpty(_name))
			{
				name = vals[0];
			}
			KatriskLine kl = new KatriskLine(vals);
			_data.Add(kl);
			return kl.isValid;
		}
		internal void sort()
		{
			//implemented sortable on KatriskLine
			_data.Sort();
		}
		public void computeEP()
		{
			sort();
			//aggregate 
			//thin based on VW with normalized probability
		}
	}
}
