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
	/// Handle the Matrix Cell dataElement enumeration: output, nooutput
	///</summary>
	internal enum MatrixCellDataElementOutputEnum
	{
		Output,				// Indicates the cell should appear in the output
		NoOutput			// Indicates the cell should not appear in the output		
	}
	internal class MatrixCellDataElementOutput
	{
		static internal MatrixCellDataElementOutputEnum GetStyle(string s, ReportLog rl)
		{
			MatrixCellDataElementOutputEnum rs;

			switch (s)
			{		
				case "Output":
					rs = MatrixCellDataElementOutputEnum.Output;
					break;
				case "NoOutput":
					rs = MatrixCellDataElementOutputEnum.NoOutput;
					break;
				default:		
					rl.LogError(4, "Unknown MatrixCellDataElementOutput '" + s + "'.  Output assumed.");
					rs = MatrixCellDataElementOutputEnum.Output;
					break;
			}
			return rs;
		}
	}

}
