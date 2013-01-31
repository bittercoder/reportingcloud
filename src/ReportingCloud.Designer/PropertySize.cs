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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;            // need this for the properties metadata
using System.Drawing.Design;
using System.Xml;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ReportingCloud.Designer
{
    /// <summary>
    /// PropertyExpr - 
    /// </summary>
    [TypeConverter(typeof(PropertySizeConverter))]
    internal class PropertySize
    {
        PropertyReportItem _pri;
        string _h;
        string _w;

        internal PropertySize(PropertyReportItem pri, string h, string w)
        {
            _pri = pri;
            _h = h;
            _w = w;
        }
        [RefreshProperties(RefreshProperties.Repaint)]
        public string Height
        {
            get { return _h; }
            set 
            {
                DesignerUtility.ValidateSize(value, true, false);
                _h = value;
                _pri.SetValue("Height", value);
            }
        }
        [RefreshProperties(RefreshProperties.Repaint)]
        public string Width
        {
            get { return _w; }
            set 
            {
                DesignerUtility.ValidateSize(value, true, false);
                _w = value;
                _pri.SetValue("Width", value);
            }
        }
    }

    internal class PropertySizeConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertySize)
            {
                PropertySize pe = value as PropertySize;
                return string.Format("({0}, {1})", pe.Height, pe.Width);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}