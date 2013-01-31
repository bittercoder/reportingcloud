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
	/// Unary minus operator for a integer operand
	/// </summary>
	[Serializable]
	internal class FunctionUnaryMinusInteger : IExpr
	{
		IExpr _rhs;			// rhs

		/// <summary>
		/// Do minus on decimal data type
		/// </summary>
		public FunctionUnaryMinusInteger() 
		{
			_rhs = null;
		}

		public FunctionUnaryMinusInteger(IExpr r) 
		{
			_rhs = r;
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
		}

		public bool IsConstant()
		{
			return _rhs.IsConstant();
		}

		public IExpr ConstantOptimization()
		{
			_rhs = _rhs.ConstantOptimization();
			if (_rhs.IsConstant())
			{
				double d = EvaluateDouble(null, null);
				return new ConstantInteger((int) d);
			}

			return this;
		}

		// Evaluate is for interpretation  (and is relatively slow)
		public object Evaluate(Report rpt, Row row)
		{
			return (int) EvaluateInt32(rpt, row);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{
			double result = _rhs.EvaluateDouble(rpt, row);

			return -result;
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            int result = _rhs.EvaluateInt32(rpt, row);

            return -result;
        }
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			int result = EvaluateInt32(rpt, row);

			return Convert.ToDecimal(result);
		}

		public string EvaluateString(Report rpt, Row row)
		{
			int result = (int) EvaluateDouble(rpt, row);
			return result.ToString();
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			int result = (int) EvaluateDouble(rpt, row);
			return Convert.ToDateTime(result);
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			int result = (int) EvaluateDouble(rpt, row);
			return result == 0? false:true;
		}

		public IExpr Rhs
		{
			get { return  _rhs; }
			set {  _rhs = value; }
		}

	}
}
