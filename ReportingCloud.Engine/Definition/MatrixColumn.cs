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
	/// Handle a Matrix column.
	///</summary>
	[Serializable]
	internal class MatrixColumn : ReportLink
	{
		RSize _Width;		// Width of each detail cell in this column
	
		internal MatrixColumn(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Width=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Width":
						_Width = new RSize(r, xNodeLoop);
						break;
					default:
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown MatrixColumn element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
		}
		
		override internal void FinalPass()
		{
			return;
		}

		internal RSize Width
		{
			get { return  _Width; }
			set {  _Width = value; }
		}
	}
}
