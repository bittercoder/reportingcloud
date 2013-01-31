/*
·--------------------------------------------------------------------·
| ReportingCloud - Viewer                                            |
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Printing;
using ReportingCloud.Viewer;

namespace ReportingCloud.Reader
{
	/// <summary>
	/// RdlReader is a application for displaying reports based on RDL.
	/// </summary>
	public class MDIChild : Form
	{
		private ReportingCloud.Viewer.Viewer viewer1;

		public MDIChild(int width, int height)
		{
            this.viewer1 = new ReportingCloud.Viewer.Viewer();
			this.SuspendLayout();
			// 
			// viewer1
			// 
            this.viewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewer1.Location = new System.Drawing.Point(0, 0);
            this.viewer1.Name = "viewer1";
            this.viewer1.Size = new System.Drawing.Size(width, height);
            this.viewer1.TabIndex = 0;
			// 
			// RdlReader
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(width, height);
			this.Controls.Add(this.viewer1);
			this.Name = "";
			this.Text = "";
			this.ResumeLayout(false);
		}

		/// <summary>
		/// The RDL file that should be displayed.
		/// </summary>
		public string SourceFile
		{
			get {return this.viewer1.SourceFile;}
			set 
			{
				this.viewer1.SourceFile = value;
				this.viewer1.Refresh();		// force the repaint
			}
		}

		public Viewer.Viewer Viewer
		{
			get {return this.viewer1;}
		}
	}
}
