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
	/// Aggregate function: Count
	/// </summary>
	[Serializable]
	internal class FunctionAggrArray : FunctionAggr, IExpr, ICacheData
	{
		string _key;
		/// <summary>
		/// Aggregate function: Count
		/// 
		///	Return type is double
		/// </summary>
        public FunctionAggrArray(List<ICacheData> dataCache, IExpr e, object scp)
            : base(e, scp) 
		{
			_key = "aggrarray" + Interlocked.Increment(ref Parser.Counter).ToString();
			dataCache.Add(this);
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
            bool bSave = true;
            RowEnumerable re = this.GetDataScope(rpt, row, out bSave);
            if (re == null)
                return null;

            object v = GetValue(rpt);
            if (v == null)
            {
                object temp;
                ArrayList ar = new ArrayList(Math.Max(1,re.LastRow - re.FirstRow + 1));
                foreach (Row r in re)
                {
                    temp = _Expr.Evaluate(rpt, r);
                    ar.Add(temp);
                }

                v = ar;
                if (bSave)
                    SetValue(rpt, v);
            }
            return v;
        }
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			return double.MinValue;
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			return decimal.MinValue;
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            return int.MinValue;
        }

		public string EvaluateString(Report rpt, Row row)
		{
			return null;
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
            return DateTime.MinValue;
		}

		private object GetValue(Report rpt)
		{
			object oi = rpt.Cache.Get(_key);
			return oi;
		}

		private void SetValue(Report rpt, object o)
		{
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
