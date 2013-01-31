/*
·--------------------------------------------------------------------·
| ReportingCloud - Engine                                            |
| Copyright (c) 2010, FlexibleCoder.                                 |
| https://sourceforge.net/projects/reportingcloud                    |
·--------------------------------------------------------------------·
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
·--------------------------------------------------------------------·
*/

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using ReportingCloud.Engine;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// Identifier
	/// </summary>
	[Serializable]
	internal class Identifier : IExpr
	{
		string _Value;		// value of the identifier

		/// <summary>
		/// passed class name, function name, and args for evaluation
		/// </summary>
		public Identifier(string v) 
		{
			string lv = v.ToLower();
			if (lv == "null" || lv == "nothing")
				_Value = null;
			else
				_Value = v;
		}

		internal bool IsNothing
		{
			get { return _Value == null? true: false; }
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;			// TODO
		}

		public bool IsConstant()
		{
			return false;
		}

		public IExpr ConstantOptimization()
		{	
			return this;
		}

		public object Evaluate(Report rpt, Row row)
		{	
			return _Value;
		}

		public double EvaluateDouble(Report rpt, Row row)
		{
			return Convert.ToDouble(Evaluate(rpt, row));
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			return Convert.ToDecimal(Evaluate(rpt, row));
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            return Convert.ToInt32(Evaluate(rpt, row));
        }

		public string EvaluateString(Report rpt, Row row)
		{
			return Convert.ToString(Evaluate(rpt, row));
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			return Convert.ToDateTime(Evaluate(rpt, row));
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			return Convert.ToBoolean(Evaluate(rpt, row));
		}
	}
}
