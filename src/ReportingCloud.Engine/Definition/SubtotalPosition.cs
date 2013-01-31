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
	/// Handle the matrix subtotal position: before, after
	///</summary>
	internal enum SubtotalPositionEnum
	{
		Before,			// left/above
		After			// right/below

	}

	internal class SubtotalPosition
	{
		static internal SubtotalPositionEnum GetStyle(string s, ReportLog rl)
		{
			SubtotalPositionEnum rs;

			switch (s)
			{		
				case "Before":
					rs = SubtotalPositionEnum.Before;
					break;
				case "After":
					rs = SubtotalPositionEnum.After;
					break;
				default:		
					rl.LogError(4, "Unknown SubtotalPosition '" + s + "'.  Before assumed.");
					rs = SubtotalPositionEnum.Before;
					break;
			}
			return rs;
		}
	}

}
