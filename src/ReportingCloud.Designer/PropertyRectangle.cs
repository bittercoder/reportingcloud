/*
·--------------------------------------------------------------------·
| ReportingCloud - Designer                                          |
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
using System.Text;
using System.Drawing;
using System.ComponentModel;            // need this for the properties metadata
using System.Xml;
using System.Text.RegularExpressions;
using ReportingCloud.Engine;

namespace ReportingCloud.Designer
{
    /// <summary>
    /// PropertyRectangle - The Rectangle specific Properties
    /// </summary>
    
    internal class PropertyRectangle : PropertyReportItem
    {
        internal PropertyRectangle(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris) : base(d, dc, ris)
        {
        }
        [CategoryAttribute("Rectangle"),
           DescriptionAttribute("Determines if report will start a new page at the top of the rectangle.")]
        public bool PageBreakAtStart
        {
            get { return this.Draw.GetElementValue(this.Node, "PageBreakAtStart", "false").ToLower() == "true" ? true : false; }
            set
            {
                this.SetValue("PageBreakAtStart", value ? "true" : "false");
            }
        }
        [CategoryAttribute("Rectangle"),
           DescriptionAttribute("Determines if report will start a new page after the bottom of the rectangle.")]
        public bool PageBreakAtEnd
        {
            get { return this.Draw.GetElementValue(this.Node, "PageBreakAtEnd", "false").ToLower() == "true" ? true : false; }
            set
            {
                this.SetValue("PageBreakAtEnd", value ? "true" : "false");
            }
        }

    }
}
