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
	///Indicates whether textboxes should render as elements or attributes.
	///</summary>
	public class DataElementStyle
	{
        static public DataElementStyleEnum GetStyle(string s)
        {
            return GetStyle(s, null);
        }
		static internal DataElementStyleEnum GetStyle(string s, ReportLog rl)
		{
			DataElementStyleEnum rs;

			switch (s)
			{		
				case "Auto":
					rs = DataElementStyleEnum.Auto;
					break;
				case "AttributeNormal":
					rs = DataElementStyleEnum.AttributeNormal;
					break;
				case "ElementNormal":
					rs = DataElementStyleEnum.ElementNormal;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown DataElementStyle '" + s + "'.  AttributeNormal assumed.");
					rs = DataElementStyleEnum.AttributeNormal;
				    break;
			}
			return rs;
		}
	}
}
