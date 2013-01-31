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
using System.IO;
using ReportingCloud.Engine;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// The IExpr interface should be implemented when you want to create a built-in function.
	/// </summary>
	internal interface IExpr
	{
		TypeCode GetTypeCode();			// return the type of the expression
		bool IsConstant();				// expression returns a constant
		IExpr ConstantOptimization();	// constant optimization

		// Evaluate is for interpretation
		object Evaluate(Report r, Row row);				// return an object
		string EvaluateString(Report r, Row row);		// return a string
		double EvaluateDouble(Report r, Row row);		// return a double
		decimal EvaluateDecimal(Report r, Row row);		// return a decimal
        int EvaluateInt32(Report r, Row row);           // return an Int32
		DateTime EvaluateDateTime(Report r, Row row);	// return a DateTime
		bool EvaluateBoolean(Report r, Row row);		// return boolean
	}
}
