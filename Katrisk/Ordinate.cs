using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katrisk
{
	public class Ordinate : IComparable<Ordinate>
	{
		private double _recurranceInterval;
		private double _OEP;//occurance
		private double _AEP;//annual
		private bool _valid = true;
		private List<string> _messages = new List<string>();
		public List<string> messages { get { return _messages; } }
		public bool isValid { get { return _valid; } internal set { _valid = value; } }
		public double recurranceInterval { get { return _recurranceInterval; } internal set { _recurranceInterval = value; } }
		public Ordinate(Point p)
		{
			_recurranceInterval = p.X;
			_OEP = p.Y;
			validate();
		}

		private void validate()
		{
			if ((_recurranceInterval >= 1))
			{
				isValid = false;
				_messages.Add("Probability is 1 or greater");
			}
			if ((_recurranceInterval < 0))
			{
				isValid = false;
				_messages.Add("Probability is less than 0");
			}
			
			if ((_OEP < 0))
			{
				isValid = false;
				_messages.Add("Damages are less than 0");
			}
		}

		public Ordinate(string[] r)
		{
			if (r.Length != 4)
			{
				isValid = false;
				_messages.Add("input string from original Katrisk output is not equal to 4 values");
				//return;
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
				_messages.Add("recurrance interval could not parse to double");
			}
			if (Double.TryParse(r[2], out tmp))
			{
				_OEP = tmp;
			}
			else
			{
				//exception?
				isValid = false;
				_messages.Add("OEP could not parse to double");
			}
			if (Double.TryParse(r[3], out tmp))
			{
				_AEP = tmp;
			}
			else
			{
				//exception?
				isValid = false;
				_messages.Add("AEP could not parse to double");
			}
			if (_OEP > _AEP) _messages.Add("AEP is less than OEP for " + r[0]);
			validate();
		}
		public double X { get { return _recurranceInterval; } }
		public double Y { get { return _OEP; } }
		public Point convertToPoint()
		{
			return new Point(X, Y);
		}
		int IComparable<Ordinate>.CompareTo(Ordinate other)
		{
			if (other.recurranceInterval > recurranceInterval) return 1;
			if (other.recurranceInterval == recurranceInterval) return 0;
			return -1;
		}
	}
}
