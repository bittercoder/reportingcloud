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
	/// CategoryGroupings definition and processing.
	///</summary>
	[Serializable]
	internal class CategoryGroupings : ReportLink
	{
        List<CategoryGrouping> _Items;			// list of category groupings

		internal CategoryGroupings(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			CategoryGrouping cg;
            _Items = new List<CategoryGrouping>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "CategoryGrouping":
						cg = new CategoryGrouping(r, this, xNodeLoop);
						break;
					default:
						cg=null;		// don't know what this is
						break;
				}
				if (cg != null)
					_Items.Add(cg);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For CategoryGroupings at least one CategoryGrouping is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (CategoryGrouping cg in _Items)
			{
				cg.FinalPass();
			}
			return;
		}

        internal List<CategoryGrouping> Items
		{
			get { return  _Items; }
		}
	}
}
