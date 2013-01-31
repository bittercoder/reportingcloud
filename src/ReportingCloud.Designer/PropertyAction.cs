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
    /// PropertyAction - 
    /// </summary>
    [TypeConverter(typeof(PropertyActionConverter)),
        Editor(typeof(PropertyActionUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
    internal class PropertyAction
    {
        PropertyReportItem pri;

        internal PropertyAction(PropertyReportItem ri)
        {
            pri = ri;
        }
        
        public override string ToString()
        {
            string result = "";
            DesignXmlDraw dr = pri.Draw;
            XmlNode aNode = dr.GetNamedChildNode(pri.Node, "Action");
            if (aNode == null)
                result = "None";
            else
            {
                XmlNode vLink = dr.GetNamedChildNode(aNode, "Hyperlink");
                if (vLink != null)
                {	// Hyperlink specified
                    result = string.Format("Hyperlink: {0}", vLink.InnerText);
                }
                else
                {
                    vLink = dr.GetNamedChildNode(aNode, "Drillthrough");
                    if (vLink != null)
                    {	// Drillthrough specified
                        result = string.Format("Drillthrough: {0}", dr.GetElementValue(vLink, "ReportName", ""));
                    }
                    else
                    {
                        vLink = dr.GetNamedChildNode(aNode, "BookmarkLink");
                        if (vLink != null)
                        {	// BookmarkLink specified
                            result = string.Format("BookmarkLink: {0}", vLink.InnerText);
                        }
                    }
                }
            }

            return result;
        }
    }

    internal class PropertyActionConverter : StringConverter
    {
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PropertyAction))
                return true;
            
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, 
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyAction)
            {
                PropertyAction pa = value as PropertyAction;
                return pa.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }

    internal class PropertyActionUIEditor : UITypeEditor
    {
        internal PropertyActionUIEditor()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context,
                                        IServiceProvider provider,
                                        object value)
        {

            if ((context == null) || (provider == null))
                return base.EditValue(context, provider, value);

            // Access the Property Browser's UI display service
            IWindowsFormsEditorService editorService =
                (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (editorService == null)
                return base.EditValue(context, provider, value);

            // Create an instance of the UI editor form
            IReportItem iri = context.Instance as IReportItem;
            if (iri == null)
                return base.EditValue(context, provider, value);
            PropertyReportItem pre = iri.GetPRI();

            PropertyAction pa = value as PropertyAction;
            if (pa == null)
                return base.EditValue(context, provider, value);

            SingleCtlDialog scd = new SingleCtlDialog(pre.DesignCtl, pre.Draw, pre.Nodes, SingleCtlTypeEnum.InteractivityCtl, null);
            try
            {
                // Display the UI editor dialog
                if (editorService.ShowDialog(scd) == DialogResult.OK)
                {
                    // Return the new property value from the UI editor form
                    return new PropertyAction(pre);
                }
            }
            finally
            {
                scd.Dispose();
            }
            return base.EditValue(context, provider, value);
        }
    }
}