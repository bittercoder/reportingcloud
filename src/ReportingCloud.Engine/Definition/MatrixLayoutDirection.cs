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
	/// Handle Matrix layout direction enumeration: LTR (left to right), RTL (right to left)
	///</summary>
	internal enum MatrixLayoutDirectionEnum
	{
		LTR,				// Left to Right
		RTL					// Right to Left
	}

	internal class MatrixLayoutDirection
	{
		static internal MatrixLayoutDirectionEnum GetStyle(string s, ReportLog rl)
		{
			MatrixLayoutDirectionEnum rs;

			switch (s)
			{		
				case "LTR":
				case "LeftToRight":
					rs = MatrixLayoutDirectionEnum.LTR;
					break;
				case "RTL":
				case "RightToLeft":
					rs = MatrixLayoutDirectionEnum.RTL;
					break;
				default:		
					rl.LogError(4, "Unknown MatrixLayoutDirection '" + s + "'.  LTR assumed.");
					rs = MatrixLayoutDirectionEnum.LTR;
					break;
			}
			return rs;
		}
	}

}
