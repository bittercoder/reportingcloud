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
	/// Handle Legend position enumeration: TopLeft, LeftTop, ...
	///</summary>
	public enum LegendPositionEnum
	{
		TopLeft,
		TopCenter,
		TopRight,
		LeftTop,
		LeftCenter,
		LeftBottom,
		RightTop,
		RightCenter,
		RightBottom,
		BottomRight,
		BottomCenter,
		BottomLeft
	}
	public class LegendPosition
	{
        static public LegendPositionEnum GetStyle(string s)
        {
            return LegendPosition.GetStyle(s, null);
        }
		static internal LegendPositionEnum GetStyle(string s, ReportLog rl)
		{
			LegendPositionEnum rs;

			switch (s)
			{		
				case "TopLeft":
					rs = LegendPositionEnum.TopLeft;
					break;
				case "TopCenter":
					rs = LegendPositionEnum.TopCenter;
					break;
				case "TopRight":
					rs = LegendPositionEnum.TopRight;
					break;
				case "LeftTop":
					rs = LegendPositionEnum.LeftTop;
					break;
				case "LeftCenter":
					rs = LegendPositionEnum.LeftCenter;
					break;
				case "LeftBottom":
					rs = LegendPositionEnum.LeftBottom;
					break;
				case "RightTop":
					rs = LegendPositionEnum.RightTop;
					break;
				case "RightCenter":
					rs = LegendPositionEnum.RightCenter;
					break;
				case "RightBottom":
					rs = LegendPositionEnum.RightBottom;
					break;
				case "BottomRight":
					rs = LegendPositionEnum.BottomRight;
					break;
				case "BottomCenter":
					rs = LegendPositionEnum.BottomCenter;
					break;
				case "BottomLeft":
					rs = LegendPositionEnum.BottomLeft;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown LegendPosition '" + s + "'.  RightTop assumed.");
					rs = LegendPositionEnum.RightTop;
					break;
			}
			return rs;
		}
	}

}
