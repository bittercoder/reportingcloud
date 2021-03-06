/*
�--------------------------------------------------------------------�
| ReportingCloud - Engine                                            |
| Copyright (c) 2010, FlexibleCoder.                                 |
| https://sourceforge.net/projects/reportingcloud                    |
�--------------------------------------------------------------------�
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
�--------------------------------------------------------------------�
*/

using System;

namespace ReportingCloud.Engine
{
	///<summary>
	/// Chart type and marker definition and processing.
	///</summary>
	internal enum ChartTypeEnum
	{
		Column,
		Bar,
		Line,
		Pie,
		Scatter,
		Bubble,
		Area,
		Doughnut,
		Stock,
        Map,
		Unknown
	}

	internal enum ChartMarkerEnum			// used for line point markers
	{									// the order reflects the usage order as well (see GetSeriesMarkers in ChartBase)
		Circle=0,
		Square=1,
		Triangle=2,
		Plus=3,
		X=4,
		Diamond=5,						// TODO: diamond doesn't draw well in small size
		Count=6,						// must equal the number of shapes
		None=Count+1,					// above Count
		Bubble=Count+2,					// above None
        Line=Count+3                    // For scatter lines GJL
	}

	internal class ChartType
	{
		static internal ChartTypeEnum GetStyle(string s)
		{
			ChartTypeEnum ct;

			switch (s)
			{		
				case "Column":
					ct = ChartTypeEnum.Column;
					break;
				case "Bar":
					ct = ChartTypeEnum.Bar;
					break;
				case "Line":
					ct = ChartTypeEnum.Line;
					break;
				case "Pie":
					ct = ChartTypeEnum.Pie;
					break;
				case "Scatter":
					ct = ChartTypeEnum.Scatter;
					break;
				case "Bubble":
					ct = ChartTypeEnum.Bubble;
					break;
				case "Area":
					ct = ChartTypeEnum.Area;
					break;
				case "Doughnut":
					ct = ChartTypeEnum.Doughnut;
					break;
				case "Stock":
					ct = ChartTypeEnum.Stock;
					break;
                case "Map":
                    ct = ChartTypeEnum.Map;
                    break;
				default:		// unknown type
					ct = ChartTypeEnum.Unknown;
					break;
			}
			return ct;
		}
	}

}
