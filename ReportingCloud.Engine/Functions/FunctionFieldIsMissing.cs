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
	/// IsMissing attribute
	/// </summary>
	[Serializable]
	internal class FunctionFieldIsMissing : FunctionField
	{
		/// <summary>
		/// Determine if value of Field is available
		/// </summary>
		public FunctionFieldIsMissing(Field fld) : base(fld)
		{
		}
		public FunctionFieldIsMissing(string method) : base(method)
		{
		}

		public override TypeCode GetTypeCode()
		{
			return TypeCode.Boolean;
		}

		public override bool IsConstant()
		{
			return false;
		}

		public override IExpr ConstantOptimization()
		{	
			return this;	// not a constant
		}

		// 
		public override object Evaluate(Report rpt, Row row)
		{
			return EvaluateBoolean(rpt, row);
		}
		
		public override double EvaluateDouble(Report rpt, Row row)
		{
			return EvaluateBoolean(rpt, row)? 1: 0;
		}
		
		public override decimal EvaluateDecimal(Report rpt, Row row)
		{
			return EvaluateBoolean(rpt, row)? 1m: 0m;
		}

		public override string EvaluateString(Report rpt, Row row)
		{
			return EvaluateBoolean(rpt, row)? "True": "False";
		}

		public override DateTime EvaluateDateTime(Report rpt, Row row)
		{
			return DateTime.MinValue;
		}

		public override bool EvaluateBoolean(Report rpt, Row row)
		{
			object o = base.Evaluate(rpt, row);
			return o == null? true: false;
		}
	}
}
