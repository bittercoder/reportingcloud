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
using System.Globalization;
using ReportingCloud.Engine;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// The Language field in the User collection.
	/// </summary>
	[Serializable]
	internal class FunctionUserLanguage : IExpr
	{
		/// <summary>
		/// Client user language
		/// </summary>
		public FunctionUserLanguage() 
		{
		}

		public TypeCode GetTypeCode()
		{
			return TypeCode.String;
		}

		public bool IsConstant()
		{
			return false;
		}

		public IExpr ConstantOptimization()
		{	
			return this;
		}

		// Evaluate is for interpretation  
		public object Evaluate(Report rpt, Row row)
		{
			return EvaluateString(rpt, row);
		}
		
		public double EvaluateDouble(Report rpt, Row row)
		{	
			throw new Exception("Invalid conversion from Language to double.");
		}
		
		public decimal EvaluateDecimal(Report rpt, Row row)
		{
			throw new Exception("Invalid conversion from Language to Decimal.");
		}

        public int EvaluateInt32(Report rpt, Row row)
        {
            throw new Exception("Invalid conversion from Language to Int32.");
        }
		public string EvaluateString(Report rpt, Row row)
		{
			if (rpt == null || rpt.ClientLanguage == null)
				return CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
			else
				return rpt.ClientLanguage;
		}

		public DateTime EvaluateDateTime(Report rpt, Row row)
		{
			throw new Exception("Invalid conversion from Language to DateTime.");
		}

		public bool EvaluateBoolean(Report rpt, Row row)
		{
			throw new Exception("Invalid conversion from Language to boolean.");
		}
	}
}
