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
	/// Represents the report item for a line.
	///</summary>
	[Serializable]
	internal class Line : ReportItem
	{
		// Line has no additional elements/attributes beyond ReportItem
		internal Line(ReportDefn r, ReportLink p, XmlNode xNode) : base(r,p,xNode)
		{
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				// nothing beyond reportitem for now
				if (!ReportItemElement(xNodeLoop))	// try at ReportItem level
				{					
					// don't know this element - log it
					OwnerReport.rl.LogError(4, "Unknown Line element " + xNodeLoop.Name + " ignored.");
				}
			}
		}
		override internal void Run(IPresent ip, Row row)
		{
			ip.Line(this, row);
		}

		override internal void RunPage(Pages pgs, Row row)
		{
			Report r = pgs.Report;
            bool bHidden = IsHidden(r, row);

			SetPagePositionBegin(pgs);
			PageLine pl = new PageLine();
            SetPagePositionAndStyle(r, pl, row);
            if (!bHidden)
			    pgs.CurrentPage.AddObject(pl);
			SetPagePositionEnd(pgs, pl.Y);
		}

		internal float GetX2(Report rpt)
		{
			float x2=GetOffsetCalc(rpt)+LeftCalc(rpt);
			if (Width != null)
				x2 += Width.Points;
			return x2;
		}		

		internal int iX2
		{
			get 
			{
				int x2=0;
				if (Left != null)
					x2 = Left.Size;
				if (Width != null)
					x2 += Width.Size;
				return x2;
			}
		}

        internal int iY2
        {
            get
            {
                int y2 = 0;
                if (Top != null)
                    y2 = Top.Size;
                if (Height != null)
                    y2 += Height.Size;
                return y2;
            }
        }

        internal float Y2
        {
            get
            {
                float y2 = 0;
                if (Top != null)
                    y2 = Top.Points;
                if (Height != null)
                    y2 += Height.Points;
                return y2;
            }
        }
    }
}
