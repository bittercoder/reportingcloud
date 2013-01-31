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
	/// ChartPalette enum handling.
	///</summary>
	internal enum ChartPaletteEnum
	{
		Default,
		EarthTones,
		Excel,
		GrayScale,
		Light,
		Pastel,
		SemiTransparent,// 20022008 AJM GJL
        Patterned, //GJL
        PatternedBlack,// 20022008 AJM GJL
        Custom
	}

	internal class ChartPalette
	{
		static internal ChartPaletteEnum GetStyle(string s, ReportLog rl)
		{
			ChartPaletteEnum p;

			switch (s)
			{		
				case "Default":
					p = ChartPaletteEnum.Default;
					break;
				case "EarthTones":
					p = ChartPaletteEnum.EarthTones;
					break;
				case "Excel":
					p = ChartPaletteEnum.Excel;
					break;
				case "GrayScale":
					p = ChartPaletteEnum.GrayScale;
					break;
				case "Light":
					p = ChartPaletteEnum.Light;
					break;
				case "Pastel":
					p = ChartPaletteEnum.Pastel;
					break;
				case "SemiTransparent":
					p = ChartPaletteEnum.SemiTransparent;
					break;
                case "Patterned": //GJL
                    p = ChartPaletteEnum.Patterned;
                    break;
                case "PatternedBlack": //GJL
                    p = ChartPaletteEnum.PatternedBlack;
                    break;
                case "Custom":
                    p = ChartPaletteEnum.Custom;
                    break;
				default:		
					rl.LogError(4, "Unknown ChartPalette '" + s + "'.  Default assumed.");
					p = ChartPaletteEnum.Default;
					break;
			}
			return p;
		}
	}

}
