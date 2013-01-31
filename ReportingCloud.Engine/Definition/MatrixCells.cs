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
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace ReportingCloud.Engine
{
	///<summary>
	/// Collection of matrix cells.
	///</summary>
	[Serializable]
	internal class MatrixCells : ReportLink
	{
        List<MatrixCell> _Items;			// list of MatrixCell

		internal MatrixCells(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			MatrixCell m;
            _Items = new List<MatrixCell>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "MatrixCell":
						m = new MatrixCell(r, this, xNodeLoop);
						break;
					default:	
						m=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown MatrixCells element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (m != null)
					_Items.Add(m);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For MatrixCells at least one MatrixCell is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (MatrixCell m in _Items)
			{
				m.FinalPass();
			}
			return;
		}

        internal List<MatrixCell> Items
		{
			get { return  _Items; }
		}
	}
}
