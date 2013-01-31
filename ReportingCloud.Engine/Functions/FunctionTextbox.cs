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
	/// Reference to a Textbox data value
	/// </summary>
	[Serializable]
	internal class FunctionTextbox : IExpr
	{
		Textbox t;	

		/// <summary>
		/// obtain value of Textbox
		/// </summary>
		public FunctionTextbox(Textbox tb, string uniquename) 
		{
			t=tb;
			if (uniquename == null)	
				return;
			// We need to register this expression with the Textbox
			tb.AddExpressionReference(uniquename);
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Object;
		}

		public bool IsConstant()
		{
			return false;
		}

		public IExpr ConstantOptimization()
		{	// not a constant expression
			return this;
		}

		// Evaluate the value for the expression
		public object Evaluate(Report rpt, Row row)
		{
			return t.Evaluate(rpt, row);
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
			return result.ToString();
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToDateTime(result);
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			object result = Evaluate(rpt, row);
			return Convert.ToBoolean(result);
		}
	}
}
