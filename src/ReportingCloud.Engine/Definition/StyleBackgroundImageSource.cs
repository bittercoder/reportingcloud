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
	/// Style Background image source enumeration
	///</summary>

	internal enum StyleBackgroundImageSourceEnum
	{
		External,		// The Value contains a constant or
		// expression that evaluates to for the location
		// of the image
		Embedded,		// The Value contains a constant
		// or expression that evaluates to the name of
		// an EmbeddedImage within the report
		Database,		// The Value contains an expression
		// (a field in the database) that evaluates to the
		// binary data for the image.
		Unknown			// Illegal (or no) value specified
	}

	internal class StyleBackgroundImageSource
	{
		static internal StyleBackgroundImageSourceEnum GetStyle(string s)
		{
			StyleBackgroundImageSourceEnum rs;

			switch (s)
			{		
				case "External":
					rs = StyleBackgroundImageSourceEnum.External;
					break;
				case "Embedded":
					rs = StyleBackgroundImageSourceEnum.Embedded;
					break;
				case "Database":
					rs = StyleBackgroundImageSourceEnum.Database;
					break;
				default:		// user error just force to normal TODO
					rs = StyleBackgroundImageSourceEnum.External;
					break;
			}
			return rs;
		}
	}

}
