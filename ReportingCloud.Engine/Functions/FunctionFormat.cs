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
	/// Format function: Format(expr, string expr format)
	/// </summary>
	[Serializable]
	internal class FunctionFormat : IExpr
	{
		IExpr _Formatee;	// object to format
		IExpr _Format;		// format string
		/// <summary>
		/// Format an object
		/// </summary>
		public FunctionFormat(IExpr formatee, IExpr format) 
		{
			_Formatee = formatee;
			_Format= format;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.String;
		}

		// 
		public object Evaluate(Report rpt, Row row)
		{
			return EvaluateString(rpt, row);
		}

		public bool IsConstant()
		{
			if (_Formatee.IsConstant())
				return _Format.IsConstant();

			return false;
		}
	
		public IExpr ConstantOptimization()
		{
			_Formatee = _Formatee.ConstantOptimization();
			_Format = _Format.ConstantOptimization();
			if (_Formatee.IsConstant() && _Format.IsConstant())
			{
				string s = EvaluateString(null, null);
				return new ConstantString(s);
			}

			return this;
		}
		
		public bool EvaluateBoolean(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);

			return Convert.ToBoolean(result);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);

			return Convert.ToDouble(result);
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);
			return Convert.ToDecimal(result);
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            string result = EvaluateString(rpt, row);
            return Convert.ToInt32(result);
        }

		public string EvaluateString(Report rpt, Row row)
		{
			object o = _Formatee.Evaluate(rpt, row);
			if (o == null)
				return null;
			string format = _Format.EvaluateString(rpt, row);
			if (format == null)
				return o.ToString();	// just return string version of object

			string result=null;
			try
			{
				result = String.Format("{0:" + format + "}", o);
			}
			catch 		// invalid format string specified
			{			//    treat as a weak error
				result = o.ToString();
			}
			return result;
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			string result = EvaluateString(rpt, row);
			return Convert.ToDateTime(result);
		}
	}
}
