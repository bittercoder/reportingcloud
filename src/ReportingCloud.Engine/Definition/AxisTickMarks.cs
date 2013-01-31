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
	/// AxisTickMarks definition and processing.
	///</summary>
	public enum AxisTickMarksEnum
	{
		None,
		Inside,
		Outside,
		Cross
	}

	public class AxisTickMarks
	{
        static public AxisTickMarksEnum GetStyle(string s)
        {
            return AxisTickMarks.GetStyle(s, null);
        }

		static internal AxisTickMarksEnum GetStyle(string s, ReportLog rl)
		{
			AxisTickMarksEnum rs;

			switch (s)
			{		
				case "None":
					rs = AxisTickMarksEnum.None;
					break;
				case "Inside":
					rs = AxisTickMarksEnum.Inside;
					break;
				case "Outside":
					rs = AxisTickMarksEnum.Outside;
					break;
				case "Cross":
					rs = AxisTickMarksEnum.Cross;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown Axis Tick Mark '" + s + "'.  None assumed.");
					rs = AxisTickMarksEnum.None;
					break;
			}
			return rs;
		}
	}

}
