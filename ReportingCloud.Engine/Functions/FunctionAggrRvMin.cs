/*
�--------------------------------------------------------------------�
| ReportingCloud - Engine                                            |
| Copyright (c) 2010, FlexibleCoder.                                 |
| https://sourceforge.net/projects/reportingcloud                    |
�--------------------------------------------------------------------�
| This library is free software; you can redistribute it and/or      |
| modify it under the terms of the GNU Lesser General Public         |
| License as published by the Free Software Foundation; either       |
| version 2.1 of the License, or (at your option) any later version. |
|                                                                    |
| This library is distributed in the hope that it will be useful,    |
| but WITHOUT ANY WARRANTY; without even the implied warranty of     |
| MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  |
| Lesser General Public License for more details.                    |
|                                                                    |
| GNU LGPL: http://www.gnu.org/copyleft/lesser.html                  |
�--------------------------------------------------------------------�
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using ReportingCloud.Engine;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// Aggregate function: RunningValue min
	/// </summary>
	[Serializable]
	internal class FunctionAggrRvMin : FunctionAggr, IExpr, ICacheData
	{
		private TypeCode _tc;		// type of result: decimal or double
		private string _key;		// key for caching between invocations
		/// <summary>
		/// Aggregate function: Running Max returns the highest value to date within a group
		///	Return type is same as input expression	
		/// </summary>
        public FunctionAggrRvMin(List<ICacheData> dataCache, IExpr e, object scp)
            : base(e, scp) 
		{
			_key = "aggrrvmin" + Interlocked.Increment(ref Parser.Counter).ToString();

			// Determine the result
			_tc = e.GetTypeCode();
			dataCache.Add(this);
		}

		public TypeCode GetTypeCode()
		{
			return _tc;
		}

		public object Evaluate(Report rpt, Row row)
		{
			bool bSave=true;
			IEnumerable re = this.GetDataScope(rpt, row, out bSave);
			if (re == null)
				return null;

			Row startrow=null;
			foreach (Row r in re)
			{
				startrow = r;			// We just want the first row
				break;
			}

			object v = GetValue(rpt);
			object current_value = _Expr.Evaluate(rpt, row);
			if (row == startrow)
			{}
			else
			{
				if (current_value == null)
					return v;
				else if (v == null)
				{}
				else if (Filter.ApplyCompare(_tc, v, current_value) > 0)
				{}
				else
					return v;
			}
			SetValue(rpt, current_value);
			return current_value;
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToDouble(result);
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToDecimal(result);
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            object result = Evaluate(rpt, row);
            return Convert.ToInt32(result);
        }

		public string EvaluateString(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToString(result);
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToDateTime(result);
		}
		private object GetValue(Report rpt)
		{
			return rpt.Cache.Get(_key);
		}

		private void SetValue(Report rpt, object o)
		{
			if (o == null)
				rpt.Cache.Remove(_key);
			else
				rpt.Cache.AddReplace(_key, o);
		}
		#region ICacheData Members

		public void ClearCache(Report rpt)
		{
			rpt.Cache.Remove(_key);
		}

		#endregion
	}
}