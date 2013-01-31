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
	/// When Query is database SQL; QueryColumn represents actual database column.
	///</summary>
	[Serializable]
	internal class QueryColumn
	{
		internal int colNum;			// Column # in query select
		internal string colName;		// Column name in query
		internal TypeCode _colType;	// TypeCode in query

		internal QueryColumn(int colnum, string name, TypeCode c)
		{
			colNum = colnum;
            colName = name.TrimEnd('\0');
			_colType = c;
		}

		internal TypeCode colType
		{
			// Treat Char as String for queries: <sigh> drivers sometimes confuse char and string types
			//    telling me a type is char but actually returning a string (Mono work around)
			get {return _colType == TypeCode.Char? TypeCode.String: _colType; }
		}
	}
}
