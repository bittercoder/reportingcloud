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
	/// <summary>
	/// Represents an exception throwed by lexer and parser.
	/// </summary>
	internal class ParserException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the ParserException class with the
		/// specified message.
		/// </summary>
		/// <param name="message">A message.</param>
		internal ParserException(string message) : base(message)
		{
			// used base
		}
	}
}
