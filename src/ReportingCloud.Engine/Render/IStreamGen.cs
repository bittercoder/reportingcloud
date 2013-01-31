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
using ReportingCloud.Engine;
using System.IO;
using System.Collections;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// Interface for obtaining streams for generation of reports
	/// </summary>

	public interface IStreamGen
	{
		Stream GetStream();								// get the main writer if not using TextWriter
		TextWriter GetTextWriter();						// gets the main text writer
		Stream GetIOStream(out string relativeName, string extension);	// get an IO stream, providing relative name as well- to main stream
		void CloseMainStream();							// closes the main stream
	}
}
