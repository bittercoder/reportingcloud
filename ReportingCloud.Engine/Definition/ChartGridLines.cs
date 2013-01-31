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
	/// ChartGridLines definition and processing.
	///</summary>
	[Serializable]
	internal class ChartGridLines : ReportLink
	{
		bool _ShowGridLines;	// Indicates the gridlines should be shown
		Style _Style;			// Line style properties for the gridlines and tickmarks
		
		internal ChartGridLines(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_ShowGridLines=true;
			_Style=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "ShowGridLines":
						_ShowGridLines = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					case "Style":
						_Style = new Style(r, this, xNodeLoop);
						break;
					default:	// TODO
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ChartGridLines element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		

		}
		
		override internal void FinalPass()
		{
			if (_Style != null)
				_Style.FinalPass();
			return;
		}

		internal bool ShowGridLines
		{
			get { return  _ShowGridLines; }
			set {  _ShowGridLines = value; }
		}

		internal Style Style
		{
			get { return  _Style; }
			set {  _Style = value; }
		}
	}
}
