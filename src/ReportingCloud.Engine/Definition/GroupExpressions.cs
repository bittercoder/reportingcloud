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
	/// Collection of group expressions.
	///</summary>
	[Serializable]
	internal class GroupExpressions : ReportLink
	{
        List<GroupExpression> _Items;			// list of GroupExpression

		internal GroupExpressions(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			GroupExpression g;
            _Items = new List<GroupExpression>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "GroupExpression":
						g = new GroupExpression(r, this, xNodeLoop);
						break;
					default:	
						g=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown GroupExpressions element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (g != null)
					_Items.Add(g);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "GroupExpressions require at least one GroupExpression be defined.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (GroupExpression g in _Items)
			{
				g.FinalPass();
			}
			return;
		}

        internal List<GroupExpression> Items
		{
			get { return  _Items; }
		}
	}
}
