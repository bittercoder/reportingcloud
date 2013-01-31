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

namespace ReportingCloud.Engine
{
	///<summary>
	///Data types
	///</summary>
	public class DataType
	{
        static public Type GetStyleType(string s)
        {
            TypeCode t = GetStyle(s, (ReportDefn)null);
            return XmlUtil.GetTypeFromTypeCode(t);
        }

		static internal TypeCode GetStyle(string s, ReportDefn r)
		{
			TypeCode rs;

			if (s.StartsWith("System."))
				s = s.Substring(7);

			switch (s)
			{		
				case "Boolean":
					rs = TypeCode.Boolean;
					break;
				case "DateTime":
					rs = TypeCode.DateTime;
					break;
				case "Decimal":
					rs = TypeCode.Decimal;
					break;
                case "Byte":
				case "Integer":
				case "Int16":
				case "Int32":
					rs = TypeCode.Int32;
					break;   
				case "Int64":
					rs = TypeCode.Int64;
					break;   
				case "Float":
				case "Single":
				case "Double":
					rs = TypeCode.Double;
					break;
				case "String":
				case "Char":
					rs = TypeCode.String;
					break;
				default:		// user error
					rs = TypeCode.Object;
                    if (r != null)
					    r.rl.LogError(4, string.Format("'{0}' is not a recognized type, assuming System.Object.", s));
					break;
			}
			return rs;
		}

		static internal bool IsNumeric(TypeCode tc)
		{
			switch (tc)
			{
		        case TypeCode.Byte:
				case TypeCode.Int64:
				case TypeCode.Int32:
				case TypeCode.Double:
				case TypeCode.Decimal:
					return true;
				default:		// user error
					return false;
			}
		}
	}

}
