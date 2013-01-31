/*
�--------------------------------------------------------------------�
| ReportingCloud - Designer                                          |
| Copyright (c) 2010, FlexibleCoder.                                 |
| https://sourceforge.net/projects/reportingcloud                    |
�--------------------------------------------------------------------�
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
�--------------------------------------------------------------------�
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
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
    /// PropertyTable - The Table Properties
    /// </summary>
    [TypeConverter(typeof(PropertyTableConverter)),
       Editor(typeof(PropertyTableUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
    internal class PropertyTable : PropertyDataRegion
    {
        internal PropertyTable(DesignXmlDraw d, DesignCtl dc, List<XmlNode> ris)
            : base(d, dc, ris)
        {
        }
    }
    internal class PropertyTableConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is PropertyTable)
            {
                PropertyTable pe = value as PropertyTable;
                return pe.Name;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

    }
    internal class PropertyTableUIEditor : UITypeEditor
    {
        internal PropertyTableUIEditor()
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
            PropertyReportItem pri = iri.GetPRI();

            PropertyTable pt = value as PropertyTable;
            if (pt == null)
                return base.EditValue(context, provider, value);

            //SingleCtlDialog scd = new SingleCtlDialog(pri.DesignCtl, pri.Draw, pri.Nodes, SingleCtlTypeEnum.BorderCtl, pb.Names);
            DesignCtl dc = pri.DesignCtl;
            DesignXmlDraw dp = dc.DrawCtl;
            if (dp.SelectedCount != 1)
                return base.EditValue(context, provider, value); 
            XmlNode riNode = dp.SelectedList[0];
            XmlNode table = dp.GetTableFromReportItem(riNode);
            if (table == null)
                return base.EditValue(context, provider, value);
            XmlNode tc = dp.GetTableColumn(riNode);
            XmlNode tr = dp.GetTableRow(riNode);

            List<XmlNode> ar = new List<XmlNode>();		// need to put this is a list for dialog to handle
            ar.Add(table);
            dc.UndoObject.StartUndoGroup("Table Dialog");
            using (PropertyDialog pd = new PropertyDialog(dp, ar, PropertyTypeEnum.ReportItems, tc, tr))
            {
                // Display the UI editor dialog
                DialogResult dr = editorService.ShowDialog(pd);
                dc.UndoObject.EndUndoGroup(pd.Changed || dr == DialogResult.OK);
                if (pd.Changed || dr == DialogResult.OK)
                {
                    dp.Invalidate();
                    return new PropertyTable(dp, dc, pt.Nodes);
                }

                return base.EditValue(context, provider, value);
            }
        }

    }

}
