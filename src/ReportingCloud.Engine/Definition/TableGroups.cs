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
	/// TableGroups definition and processing.
	///</summary>
	[Serializable]
	internal class TableGroups : ReportLink
	{
        List<TableGroup> _Items;			// list of TableGroup entries

		internal TableGroups(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			TableGroup tg;
            _Items = new List<TableGroup>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "TableGroup":
						tg = new TableGroup(r, this, xNodeLoop);
						break;
					default:	
						tg=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown TableGroups element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (tg != null)
					_Items.Add(tg);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For TableGroups at least one TableGroup is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach(TableGroup tg in _Items)
			{
				tg.FinalPass();
			}

			return;
		}

		internal float DefnHeight()
		{
			float height=0;
			foreach(TableGroup tg in _Items)
			{
				height += tg.DefnHeight();
			}
			return height;
		}

        internal List<TableGroup> Items
		{
			get { return  _Items; }
		}
	}
}
