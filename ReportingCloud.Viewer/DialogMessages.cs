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

namespace ReportingCloud.Viewer
{
	/// <summary>
	/// DialogMessage is used in place of a message box when the text can be large
	/// </summary>
	public class DialogMessages : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button bOK;
		private System.Windows.Forms.TextBox tbMessages;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DialogMessages(IList msgs)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			string[] lines = new string[msgs.Count];
			int l=0;
			foreach (string msg in msgs)
			{
				lines[l++] = msg;
			}
			tbMessages.Lines = lines;
			return;
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.bOK = new System.Windows.Forms.Button();
			this.tbMessages = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// bOK
			// 
			this.bOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.bOK.Location = new System.Drawing.Point(200, 208);
			this.bOK.Name = "bOK";
			this.bOK.TabIndex = 0;
			this.bOK.Text = "OK";
			// 
			// tbMessages
			// 
			this.tbMessages.Location = new System.Drawing.Point(16, 16);
			this.tbMessages.Multiline = true;
			this.tbMessages.Name = "tbMessages";
			this.tbMessages.ReadOnly = true;
			this.tbMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbMessages.Size = new System.Drawing.Size(448, 176);
			this.tbMessages.TabIndex = 9;
			this.tbMessages.Text = "";
			// 
			// DialogMessages
			// 
			this.AcceptButton = this.bOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.bOK;
			this.ClientSize = new System.Drawing.Size(482, 240);
			this.Controls.Add(this.tbMessages);
			this.Controls.Add(this.bOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DialogMessages";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Report Warnings";
			this.ResumeLayout(false);

		}
		#endregion

	}
}
