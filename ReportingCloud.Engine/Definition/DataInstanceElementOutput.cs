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
	/// DataInstanceElement definition and processing.
	///</summary>
	public enum DataInstanceElementOutputEnum
	{
		Output,			// Indicates the list instances should appear in the output
		NoOutput		// Indicates the list instances should not appear in the output		
	}

	public class DataInstanceElementOutput
	{
        static public DataInstanceElementOutputEnum GetStyle(string s)
        {
            return GetStyle(s, null);
        }
		static internal DataInstanceElementOutputEnum GetStyle(string s, ReportLog rl)
		{
			DataInstanceElementOutputEnum rs;

			switch (s)
			{		
				case "Output":
					rs = DataInstanceElementOutputEnum.Output;
					break;
				case "NoOutput":
					rs = DataInstanceElementOutputEnum.NoOutput;
					break;
				default:		
                    if (rl != null)
					    rl.LogError(4, "Unknown DataInstanceElementOutput '" + s + "'.  Output assumed.");
					rs = DataInstanceElementOutputEnum.Output;
					break;
			}
			return rs;
		}
	}

}
