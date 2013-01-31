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
	/// Matrix Corner definition and processing.
	///</summary>
	[Serializable]
	internal class Corner : ReportLink
	{
		ReportItems _ReportItems;	// The region that contains the elements of the corner layout
									// This ReportItems collection must contain exactly
									// one ReportItem. The Top, Left, Height and Width
									// for this ReportItem are ignored. The position is
									// taken to be 0, 0 and the size to be 100%, 100%.		
	
		internal Corner(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_ReportItems=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "ReportItems":
						_ReportItems = new ReportItems(r, this, xNodeLoop);
						break;	
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown Corner element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		}
		
		override internal void FinalPass()
		{
			if (_ReportItems != null)
				_ReportItems.FinalPass();
			return;
		}

		internal ReportItems ReportItems
		{
			get { return  _ReportItems; }
			set {  _ReportItems = value; }
		}
	}
}
