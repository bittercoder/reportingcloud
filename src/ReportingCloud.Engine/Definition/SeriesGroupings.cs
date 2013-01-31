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
	/// Collection of Chart series groupings.
	///</summary>
	[Serializable]
	internal class SeriesGroupings : ReportLink
	{
        List<SeriesGrouping> _Items;			// list of SeriesGrouping

		internal SeriesGroupings(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			SeriesGrouping sg;
            _Items = new List<SeriesGrouping>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "SeriesGrouping":
						sg = new SeriesGrouping(r, this, xNodeLoop);
						break;
					default:
						sg=null;		// don't know what this is
						break;
				}
				if (sg != null)
					_Items.Add(sg);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For SeriesGroupings at least one SeriesGrouping is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (SeriesGrouping sg in _Items)
			{
				sg.FinalPass();
			}
			return;
		}

        internal List<SeriesGrouping> Items
		{
			get { return  _Items; }
		}
	}
}
