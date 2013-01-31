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
using System.Globalization;
using ReportingCloud.Engine;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// Constant decimal.
	/// </summary>
	[Serializable]
	internal class ConstantDecimal : IExpr
	{
		decimal _Value;		// value of the constant

		/// <summary>
		/// passed string of the number
		/// </summary>
		public ConstantDecimal(string v) 
		{
			_Value = Convert.ToDecimal(v, NumberFormatInfo.InvariantInfo);
		}

		public ConstantDecimal(decimal v) 
		{
			_Value = v;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Decimal;
		}

		public bool IsConstant()
		{
			return true;
		}

		public IExpr ConstantOptimization()
		{	// already constant expression
			return this;
		}

		public object Evaluate(Report rpt, Row row)
		{
			return _Value;
		}

		public string EvaluateString(Report rpt, Row row)
		{
			return Convert.ToString(_Value);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			return Convert.ToDouble(_Value);
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            return Convert.ToInt32(_Value);
        }
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			return _Value;
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			return Convert.ToDateTime(_Value);
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			return Convert.ToBoolean(_Value);
		}
	}
}
