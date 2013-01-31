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
    [TypeConverter(typeof(PropertyLocationConverter))]
    internal class PropertyLocation
    {
        PropertyReportItem _pri;
        string _left;
        string _top;

        internal PropertyLocation(PropertyReportItem pri, string x, string y)
        {
            _pri = pri;
            _left = x;
            _top = y;
        }
        [RefreshProperties(RefreshProperties.Repaint)]
        public string Left
        {
            get { return _left; }
            set 
            {
                DesignerUtility.ValidateSize(value, true, false);
                _left = value;
                _pri.SetValue("Left", value);
            }
        }
        [RefreshProperties(RefreshProperties.Repaint)]
        public string Top
        {
            get { return _top; }
            set 
            {
                DesignerUtility.ValidateSize(value, true, false);
                _top = value;
                _pri.SetValue("Top", value);
            }
        }
    }

    internal class PropertyLocationConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyLocation)
            {
                PropertyLocation pe = value as PropertyLocation;
                return string.Format("({0}, {1})", pe.Left, pe.Top);
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
}