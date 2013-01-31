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
	/// ChartData definition and processing.
	///</summary>
	[Serializable]
	internal class ChartData : ReportLink
	{
        List<ChartSeries> _Items;			// list of chart series

		internal ChartData(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			ChartSeries cs;
            _Items = new List<ChartSeries>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "ChartSeries":
						cs = new ChartSeries(r, this, xNodeLoop);
						break;
					default:	
						cs=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ChartData element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (cs != null)
					_Items.Add(cs);
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For ChartData at least one ChartSeries is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (ChartSeries cs in _Items)
			{
				cs.FinalPass();
			}
			return;
		}

        internal List<ChartSeries> Items
		{
			get { return  _Items; }
		}
	}
}
