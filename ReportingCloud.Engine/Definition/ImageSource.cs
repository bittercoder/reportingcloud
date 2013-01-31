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
	///  Handles the Image source enumeration.  External, Embedded, Database
	///</summary>
	internal enum ImageSourceEnum
	{
		External,	// The Value contains a constant or
					// expression that evaluates to the location of
					// the image
		Embedded,	// The Value contains a constant
					// or expression that evaluates to the name of
					// an EmbeddedImage within the report.
		Database,	// The Value contains an
					// expression (typically a field in the database)
					// that evaluates to the binary data for the
					// image.
		Unknown		// Illegal or unspecified
	}
	internal class ImageSource
	{
		static internal ImageSourceEnum GetStyle(string s)
		{
			ImageSourceEnum rs;

			switch (s)
			{		
				case "External":
					rs = ImageSourceEnum.External;
					break;
				case "Embedded":
					rs = ImageSourceEnum.Embedded;
					break;
				case "Database":
					rs = ImageSourceEnum.Database;
					break;
				default:		
					rs = ImageSourceEnum.Unknown;
					break;
			}
			return rs;
		}
	}

}
