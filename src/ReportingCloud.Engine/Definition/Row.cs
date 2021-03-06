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
using System.Xml;

namespace ReportingCloud.Engine
{
	///<summary>
	/// A Row in a data set.
	///</summary>
	internal class Row
	{
		int _RowNumber;		// Original row #
		int _Level;			// Usually 0; set when row is part of group with ParentGroup (ie recursive hierarchy)
		GroupEntry _GroupEntry;		//   like level; 
		Rows _R;			// Owner of row collection
		object[] _Data;		// Row of data
	
		internal Row(Rows r, Row rd)			// Constructor that uses existing Row data
		{
			_R = r;
			_Data = rd.Data;
			_Level = rd.Level;
		}

		internal Row(Rows r, int columnCount)
		{
			_R = r;
			_Data = new object[columnCount];
			_Level=0;
		}

		internal object[] Data
		{
			get { return  _Data; }
			set { _Data = value; }
		}

		internal Rows R
		{
			get { return  _R; }
			set { _R = value; }
		}

		internal GroupEntry GroupEntry
		{
			get { return  _GroupEntry; }
			set {  _GroupEntry = value; }
		}

		internal int Level
		{
			get { return  _Level; }
			set {  _Level = value; }
		}

		internal int RowNumber
		{
			get { return  _RowNumber; }
			set {  _RowNumber = value; }
		}
	}
}
