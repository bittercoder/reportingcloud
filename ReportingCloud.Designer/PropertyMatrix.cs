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
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using ReportingCloud.Engine;

namespace ReportingCloud.Designer
{
    /// <summary>
    /// PropertyMatrix - The Table Properties
    /// </summary>
    [TypeConverter(typeof(PropertyMatrixConverter))]
    internal class PropertyMatrix : PropertyDataRegion
    {
        internal PropertyMatrix(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris)
            : base(d, dc, ris)
        {
        }
    }
    internal class PropertyMatrixConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyMatrix)
            {
                PropertyMatrix pe = value as PropertyMatrix;
                return pe.Name;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

}
