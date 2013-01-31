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
	
	internal enum DataLabelPositionEnum
	{
		Auto,
		Top,
		TopLeft,
		TopRight,
		Left,
		Center,
		Right,
		BottomRight,
		Bottom,
		BottomLeft
	}

	internal class DataLabelPosition
	{
		static internal DataLabelPositionEnum GetStyle(string s, ReportLog rl)
		{
			DataLabelPositionEnum dlp;

			switch (s)
			{		
				case "Auto":
					dlp = DataLabelPositionEnum.Auto;
					break;
				case "Top":
					dlp = DataLabelPositionEnum.Top;
					break;
				case "TopLeft":
					dlp = DataLabelPositionEnum.TopLeft;
					break;
				case "TopRight":
					dlp = DataLabelPositionEnum.TopRight;
					break;
				case "Left":
					dlp = DataLabelPositionEnum.Left;
					break;
				case "Center":
					dlp = DataLabelPositionEnum.Center;
					break;
				case "Right":
					dlp = DataLabelPositionEnum.Right;
					break;
				case "BottomRight":
					dlp = DataLabelPositionEnum.BottomRight;
					break;
				case "Bottom":
					dlp = DataLabelPositionEnum.Bottom;
					break;
				case "BottomLeft":
					dlp = DataLabelPositionEnum.BottomLeft;
					break;
				default:		
					rl.LogError(4, "Unknown DataLablePosition '" + s + "'.  Auto assumed.");
					dlp = DataLabelPositionEnum.Auto;
					break;
			}
			return dlp;
		}
	}


}
