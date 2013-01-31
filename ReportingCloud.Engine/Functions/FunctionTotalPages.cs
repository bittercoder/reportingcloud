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
using System.IO;
using ReportingCloud.Engine;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// Total Pages
	/// </summary>
	[Serializable]
	internal class FunctionTotalPages : IExpr
	{
		/// <summary>
		/// Total page count; relys on PageHeader, PageFooter to set Report.TotalPages
		/// </summary>
		public FunctionTotalPages() 
		{
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.Int32;
		}

		public bool IsConstant()
		{
			return false;
		}

		public IExpr ConstantOptimization()
		{	// not a constant expression
			return this;
		}

		// Evaluate is for interpretation  
		public object Evaluate(Report rpt, Row row)
		{
            return rpt == null ? (int) 1 : (int) rpt.TotalPages;
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{	
			return rpt == null? 1: rpt.TotalPages;
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            return rpt == null ? 1 : rpt.TotalPages;
        }
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			int result = EvaluateInt32(rpt, row);

			return Convert.ToDecimal(result);
		}

		public string EvaluateString(Report rpt, Row row)
		{
			int result = EvaluateInt32(rpt, row);
			return result.ToString();
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			int result = EvaluateInt32(rpt, row);
			return Convert.ToDateTime(result);
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			int result = EvaluateInt32(rpt, row);
			return Convert.ToBoolean(result);
		}
		
	}
}
