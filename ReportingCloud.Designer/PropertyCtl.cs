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
using ReportingCloud.Engine;

namespace ReportingCloud.Designer
{
	/// <summary>
	/// Summary description for ReportCtl.
	/// </summary>
	internal class PropertyCtl : System.Windows.Forms.UserControl
	{
		private DesignXmlDraw _Draw;
        private DesignCtl _DesignCtl;
        private ICollection _NameCollection = null;
        private Label label1;
        private PropertyGrid pgSelected;
        private Button bClose;
        private ComboBox cbReportItems; 
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private readonly string REPORT = "*Report*"; 
        private readonly string GROUP = "*Group Selection*"; 
        private readonly string NONAME = "*Unnamed*"; 

        internal PropertyCtl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

        internal void Reset()
        {
            _Draw = null;
            _DesignCtl = null;
            this.pgSelected.SelectedObject = null;
            cbReportItems.Items.Clear();
            _NameCollection = null;
        }

        internal void ResetSelection(DesignXmlDraw d, DesignCtl dc)
        {
            _Draw = d;
            _DesignCtl = dc;

            if (_Draw == null)
            {
                this.pgSelected.SelectedObject = null;
                cbReportItems.Items.Clear();

                return;
            }
            SetPropertyNames();

            if (_Draw.SelectedCount == 0)
            {
                this.pgSelected.SelectedObject = new PropertyReport(_Draw, dc);
                cbReportItems.SelectedItem = REPORT;
            }
            else if (SingleReportItemType())
            {
                XmlNode n = _Draw.SelectedList[0];
                if (_Draw.SelectedCount > 1)
                {
                    int si = cbReportItems.Items.Add(GROUP);
                    cbReportItems.SelectedIndex = si;
                }
                else
                {
                    XmlAttribute xAttr = n.Attributes["Name"];
                    if (xAttr == null)
                    {
                        int si = cbReportItems.Items.Add(NONAME);
                        cbReportItems.SelectedIndex = si;
                    }
                    else
                        cbReportItems.SelectedItem = xAttr.Value;
                }
                switch (n.Name)
                {
                    case "Textbox":
                        this.pgSelected.SelectedObject = new PropertyTextbox(_Draw, dc, _Draw.SelectedList);
                        break;
                    case "Rectangle":
                        this.pgSelected.SelectedObject = new PropertyRectangle(_Draw, dc, _Draw.SelectedList);
                        break;
                    case "Chart":
                        this.pgSelected.SelectedObject = new PropertyChart(_Draw, dc, _Draw.SelectedList);
                        break;
                    case "Image":
                        this.pgSelected.SelectedObject = new PropertyImage(_Draw, dc, _Draw.SelectedList);
                        break;
                    case "List":
                        this.pgSelected.SelectedObject = new PropertyList(_Draw, dc, _Draw.SelectedList);
                        break;
                    case "Subreport":
                        this.pgSelected.SelectedObject = new PropertySubreport(_Draw, dc, _Draw.SelectedList);
                        break;
                    case "CustomReportItem":
                    default:
                        this.pgSelected.SelectedObject = new PropertyReportItem(_Draw, dc, _Draw.SelectedList);
                        break;
                }
            }
            else
            {
                int si = cbReportItems.Items.Add(GROUP);
                cbReportItems.SelectedIndex = si;
                this.pgSelected.SelectedObject = new PropertyReportItem(_Draw, dc, _Draw.SelectedList);
            }
        }
        /// <summary>
        /// Fills out the names of the report items available in the report and all other objects with names
        /// </summary>
        private void SetPropertyNames()
        {
            if (_NameCollection != _Draw.ReportNames.ReportItemNames)
            {
                cbReportItems.Items.Clear();
                _NameCollection = _Draw.ReportNames.ReportItemNames;
            }
            else
            {   // ensure our list count is the same as the number of report items
                int count = cbReportItems.Items.Count;
                if (cbReportItems.Items.Contains(this.REPORT))
                    count--;
                if (cbReportItems.Items.Contains(this.GROUP))
                    count--;
                if (cbReportItems.Items.Contains(this.NONAME))
                    count--;
                if (count != _NameCollection.Count)
                    cbReportItems.Items.Clear();        // we need to rebuild
            }

            if (cbReportItems.Items.Count == 0)
            {
                cbReportItems.Items.Add(this.REPORT);
                foreach (object o in _NameCollection)
                {
                    cbReportItems.Items.Add(o);
                }
            }
            else
            {
                try
                {
                    cbReportItems.Items.Remove(this.GROUP);
                }
                catch { }
                try
                {
                    cbReportItems.Items.Remove(this.NONAME);
                }
                catch { }
            }
        }
        /// <summary>
        /// Returns true if all selected reportitems are of the same type
        /// </summary>
        /// <returns></returns>
        private bool SingleReportItemType()
        {
            if (_Draw.SelectedCount == 1)
                return true;
            string t = _Draw.SelectedList[0].Name;
            if (t == "CustomReportItem")        // when multiple CustomReportItem don't do group change.
                return false;
            for (int i = 1; i < _Draw.SelectedList.Count; i++)
            {
                if (t != _Draw.SelectedList[i].Name)
                    return false;
            }
            return true;
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
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
            this.label1 = new System.Windows.Forms.Label();
            this.pgSelected = new System.Windows.Forms.PropertyGrid();
            this.bClose = new System.Windows.Forms.Button();
            this.cbReportItems = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Properties";
            // 
            // pgSelected
            // 
            this.pgSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pgSelected.Location = new System.Drawing.Point(0, 40);
            this.pgSelected.Name = "pgSelected";
            this.pgSelected.Size = new System.Drawing.Size(240, 240);
            this.pgSelected.TabIndex = 2;
            // 
            // bClose
            // 
            this.bClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bClose.CausesValidation = false;
            this.bClose.FlatAppearance.BorderSize = 0;
            this.bClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bClose.Location = new System.Drawing.Point(223, -2);
            this.bClose.Name = "bClose";
            this.bClose.Size = new System.Drawing.Size(15, 20);
            this.bClose.TabIndex = 3;
            this.bClose.Text = "x";
            this.bClose.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // cbReportItems
            // 
            this.cbReportItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbReportItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbReportItems.FormattingEnabled = true;
            this.cbReportItems.Location = new System.Drawing.Point(0, 18);
            this.cbReportItems.Name = "cbReportItems";
            this.cbReportItems.Size = new System.Drawing.Size(240, 21);
            this.cbReportItems.TabIndex = 4;
            this.cbReportItems.SelectedIndexChanged += new System.EventHandler(this.cbReportItems_SelectedIndexChanged);
            // 
            // PropertyCtl
            // 
            this.Controls.Add(this.cbReportItems);
            this.Controls.Add(this.bClose);
            this.Controls.Add(this.pgSelected);
            this.Controls.Add(this.label1);
            this.Name = "PropertyCtl";
            this.Size = new System.Drawing.Size(240, 280);
            this.ResumeLayout(false);

		}
		#endregion

        private void bClose_Click(object sender, EventArgs e)
        {
            ReportDesigner rd = this.Parent as ReportDesigner;
            if (rd == null)
                return;
            rd.ShowProperties(false);
        }

        private void cbReportItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ri_name = cbReportItems.SelectedItem as string;
            if (ri_name == GROUP || ri_name == NONAME)
                return;
            if (ri_name == REPORT)
            {
                // handle request for change to report property
                if (_Draw.SelectedCount == 0)   // we're already on report
                    return;
                _DesignCtl.SetSelection(null);
                return;
            }

            // handle request to change selected report item
            XmlNode ri_node = _Draw.ReportNames.GetRINodeFromName(ri_name);
            if (ri_node == null)
                return;
            if (_Draw.SelectedCount == 1 &&
                _Draw.SelectedList[0] == ri_node)
                return;  // we're already selected!
            _DesignCtl.SetSelection(ri_node);
        }

    }
}
