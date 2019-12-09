using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katrisk
{
	class KatriskLine : IComparable<KatriskLine>
	{
		private double _recurranceInterval;
		private double _OEP;//occurance
		private double _AEP;//annual
		private bool _valid = true;
		public bool isValid { get { return _valid; } internal set { _valid = value; } }
		public double recurranceInterval { get { return _recurranceInterval; } internal set { _recurranceInterval = value; } }
		public KatriskLine(string[] r)
		{
			if (r.Length != 4)
			{
				isValid = false;
				return;
			}
			double tmp;
			if (Double.TryParse(r[1], out tmp))
			{
				_recurranceInterval = 1 / tmp;
			}
			else
			{
				//exception?
				isValid = false;
			}
			if (Double.TryParse(r[2], out tmp))
			{
				_OEP = tmp;
			}
			else
			{
				//exception?
				isValid = false;
			}
			if (Double.TryParse(r[3], out tmp))
			{
				_AEP = tmp;
			}
			else
			{
				//exception?
				isValid = false;
			}
		}

		int IComparable<KatriskLine>.CompareTo(KatriskLine other)
		{
			if (other.recurranceInterval > recurranceInterval) return 1;
			if (other.recurranceInterval == recurranceInterval) return 0;
			return -1;
		}
	}
}
