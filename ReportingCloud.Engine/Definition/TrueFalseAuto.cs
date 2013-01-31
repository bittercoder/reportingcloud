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
	internal enum TrueFalseAutoEnum
	{
		True,
		False,
		Auto
	}
	
	internal class TrueFalseAuto
	{
		static internal TrueFalseAutoEnum GetStyle(string s, ReportLog rl)
		{
			TrueFalseAutoEnum rs;

			switch (s)
			{		
				case "True":
					rs = TrueFalseAutoEnum.True;
					break;
				case "False":
					rs = TrueFalseAutoEnum.False;
					break;
				case "Auto":
					rs = TrueFalseAutoEnum.Auto;
					break;
				default:		
					rl.LogError(4, "Unknown True False Auto value of '" + s + "'.  Auto assumed.");
					rs = TrueFalseAutoEnum.Auto;
					break;
			}
			return rs;
		}
	}
}
