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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Text;
using System.IO;
using System.Reflection;
using ReportingCloud.Engine;

namespace ReportingCloud.Designer
{
	/// <summary>
	/// CustomReportItemCtl provides property values for a CustomReportItem
	/// </summary>
	internal class CustomReportItemCtl : System.Windows.Forms.UserControl, IProperty
	{
        private List<XmlNode> _ReportItems;
        private DesignXmlDraw _Draw;
        private string _Type;
        private PropertyGrid pgProps;
        private Button bExpr;
        private XmlNode _RiNode;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        internal CustomReportItemCtl(DesignXmlDraw dxDraw, List<XmlNode> reportItems)
		{
			_Draw = dxDraw;
            this._ReportItems = reportItems;
            _Type = _Draw.GetElementValue(_ReportItems[0], "Type", "");
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// Initialize form using the style node values
			InitValues();			
		}

		private void InitValues()
		{
            ICustomReportItem cri=null;
            try
            {
                cri = EngineConfig.CreateCustomReportItem(_Type);
                _RiNode = _Draw.GetNamedChildNode(_ReportItems[0], "CustomProperties").Clone();
                object props = cri.GetPropertiesInstance(_RiNode);
                pgProps.SelectedObject = props;
            }
            catch
            {
                return;
            }
            finally
            {
                if (cri != null)
                    cri.Dispose();
            }
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pgProps = new System.Windows.Forms.PropertyGrid();
            this.bExpr = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pgProps
            // 
            this.pgProps.Location = new System.Drawing.Point(13, 17);
            this.pgProps.Name = "pgProps";
            this.pgProps.Size = new System.Drawing.Size(406, 260);
            this.pgProps.TabIndex = 3;
            // 
            // bExpr
            // 
            this.bExpr.Font = new System.Drawing.Font("Arial", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bExpr.Location = new System.Drawing.Point(422, 57);
            this.bExpr.Name = "bExpr";
            this.bExpr.Size = new System.Drawing.Size(22, 16);
            this.bExpr.TabIndex = 4;
            this.bExpr.Tag = "sd";
            this.bExpr.Text = "fx";
            this.bExpr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bExpr.Click += new System.EventHandler(this.bExpr_Click);
            // 
            // CustomReportItemCtl
            // 
            this.Controls.Add(this.bExpr);
            this.Controls.Add(this.pgProps);
            this.Name = "CustomReportItemCtl";
            this.Size = new System.Drawing.Size(464, 304);
            this.ResumeLayout(false);

		}
		#endregion

		public bool IsValid()
		{
			return true;
		}

		public void Apply()
		{
            ICustomReportItem cri = null;
            try
            {
                cri = EngineConfig.CreateCustomReportItem(_Type);
                foreach (XmlNode node in _ReportItems)
                {
                    cri.SetPropertiesInstance(_Draw.GetNamedChildNode(node, "CustomProperties"), 
                        pgProps.SelectedObject);
                }
            }
            catch
            {
                return;
            }
            finally
            {
                if (cri != null)
                    cri.Dispose();
            }
            return;
		}

        private void bExpr_Click(object sender, EventArgs e)
        {
            GridItem gi = this.pgProps.SelectedGridItem;
            
            XmlNode sNode = _ReportItems[0];
            DialogExprEditor ee = new DialogExprEditor(_Draw, gi.Value.ToString(), sNode, false);
            try
            {
                DialogResult dr = ee.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    // There's probably a better way without reflection but this works fine.
                    string nm = gi.Label;
                    object sel = pgProps.SelectedObject;
                    Type t = sel.GetType();
                    PropertyInfo pi = t.GetProperty(nm);
                    MethodInfo mi = pi.GetSetMethod();
                    object[] oa = new object[1];
                    oa[0] = ee.Expression;
                    mi.Invoke(sel, oa);
                    gi.Select();
                }
            }
            finally
            {
                ee.Dispose();
            }
        }

	}
}
