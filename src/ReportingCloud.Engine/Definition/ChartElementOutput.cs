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
	/// ChartElementOutput parsing.
	///</summary>
	internal enum ChartElementOutputEnum
	{
		Output,
		NoOutput
	}

	internal class ChartElementOutput
	{
		static internal ChartElementOutputEnum GetStyle(string s, ReportLog rl)
		{
			ChartElementOutputEnum ceo;

			switch (s)
			{		
				case "Output":
					ceo = ChartElementOutputEnum.Output;
					break;
				case "NoOutput":
					ceo = ChartElementOutputEnum.NoOutput;
					break;
				default:		
					rl.LogError(4, "Unknown ChartElementOutput '" + s + "'.  Output assumed.");
					ceo = ChartElementOutputEnum.Output;
					break;
			}
			return ceo;
		}
	}


}
