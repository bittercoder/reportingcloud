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
	/// Three value state; true, false, auto (dependent on context)
	///</summary>
	internal enum QueryCommandTypeEnum
	{
		Text,
		StoredProcedure,
		TableDirect
	}
	internal class QueryCommandType
	{
		static internal QueryCommandTypeEnum GetStyle(string s, ReportLog rl)
		{
			QueryCommandTypeEnum rs;

			switch (s)
			{		
				case "Text":
					rs = QueryCommandTypeEnum.Text;
					break;
				case "StoredProcedure":
					rs = QueryCommandTypeEnum.StoredProcedure;
					break;
				case "TableDirect":
					rs = QueryCommandTypeEnum.TableDirect;
					break;
				default:		// user error just force to normal TODO
					rl.LogError(4, "Unknown Query CommandType '" + s + "'.  Text assumed.");
					rs = QueryCommandTypeEnum.Text;
					break;
			}
			return rs;
		}
	}

}
