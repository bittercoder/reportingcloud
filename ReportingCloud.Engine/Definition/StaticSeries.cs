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
	/// Collection of chart static series.
	///</summary>
	[Serializable]
	internal class StaticSeries : ReportLink
	{
        List<StaticMember> _Items;			// list of StaticMember

		internal StaticSeries(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			StaticMember sm;
            _Items = new List<StaticMember>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "StaticMember":
						sm = new StaticMember(r, this, xNodeLoop);
						break;
					default:	
						sm=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown StaticSeries element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (sm != null)
					_Items.Add(sm);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For StaticSeries at least one StaticMember is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (StaticMember sm in _Items)
			{
				sm.FinalPass();
			}
			return;
		}

        internal List<StaticMember> Items
		{
			get { return  _Items; }
		}
	
	}
}
