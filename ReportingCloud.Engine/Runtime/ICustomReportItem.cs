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
using System.Drawing;
using System.Xml;
using ReportingCloud.Engine;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// ICustomReportItem defines the protocol for implementing a CustomReportItem
	/// </summary>

	public interface ICustomReportItem : IDisposable    
	{
        bool IsDataRegion();                            // Does CustomReportItem require DataRegions
        void DrawImage(System.Drawing.Bitmap bm);       // Draw the image in the passed bitmap; do SetParameters first
        void DrawDesignerImage(System.Drawing.Bitmap bm);   // Design time: Draw the designer image in the passed bitmap;
        void SetProperties(IDictionary<string, object> parameters); // Set the runtime properties
        object GetPropertiesInstance(XmlNode node);     // Design time: return class representing properties
        void SetPropertiesInstance(XmlNode node, object inst);  // Design time: given class representing properties set the XML custom properties
        string GetCustomReportItemXml();                // Design time: return string with <CustomReportItem> ... </CustomReportItem> syntax for the insert
    }

}
