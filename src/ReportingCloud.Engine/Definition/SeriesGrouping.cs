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
using System.Xml;

namespace ReportingCloud.Engine
{
	///<summary>
	/// Chart Series grouping (both dynamic and static).
	///</summary>
	[Serializable]
	internal class SeriesGrouping : ReportLink
	{
		DynamicSeries _DynamicSeries;	// Dynamic Series headings for this grouping
		StaticSeries _StaticSeries;		// Static Series headings for this grouping	
		Style _Style;					// border and background properties for series legend itmes and data points
										//   when dynamic exprs are evaluated per group instance
	
		internal SeriesGrouping(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_DynamicSeries=null;
			_StaticSeries=null;
			_Style=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "DynamicSeries":
						_DynamicSeries = new DynamicSeries(r, this, xNodeLoop);
						break;
					case "StaticSeries":
						_StaticSeries = new StaticSeries(r, this, xNodeLoop);
						break;
					case "Style":
						_Style = new Style(OwnerReport, this, xNodeLoop);
						OwnerReport.rl.LogError(4, "Style element in SeriesGrouping is currently ignored."); // TODO
						break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown SeriesGrouping element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		}
		
		override internal void FinalPass()
		{
			if (_DynamicSeries != null)
				_DynamicSeries.FinalPass();
			if (_StaticSeries != null)
				_StaticSeries.FinalPass();
			if (_Style != null)
				_Style.FinalPass();

			return;
		}

		internal DynamicSeries DynamicSeries
		{
			get { return  _DynamicSeries; }
			set {  _DynamicSeries = value; }
		}

		internal StaticSeries StaticSeries
		{
			get { return  _StaticSeries; }
			set {  _StaticSeries = value; }
		}

		internal Style Style
		{
			get { return  _Style; }
			set {  _Style = value; }
		}

	}
}
