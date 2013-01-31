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
	/// The collection of embedded images in the Report.
	///</summary>
	[Serializable]
	internal class EmbeddedImages : ReportLink
	{
        List<EmbeddedImage> _Items;			// list of EmbeddedImage

		internal EmbeddedImages(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
            _Items = new List<EmbeddedImage>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				if (xNodeLoop.Name == "EmbeddedImage")
				{
					EmbeddedImage ei = new EmbeddedImage(r, this, xNodeLoop);
					_Items.Add(ei);
				}
				else
					this.OwnerReport.rl.LogError(4, "Unknown Report element '" + xNodeLoop.Name + "' ignored.");
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For EmbeddedImages at least one EmbeddedImage is required.");
			else
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (EmbeddedImage ei in _Items)
			{
				ei.FinalPass();
			}
			return;
		}

        internal List<EmbeddedImage> Items
		{
			get { return  _Items; }
		}
	}
}
