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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ReportingCloud.Viewer
{
    public partial class DialogWait : Form
    {
        private DateTime Started;
        private Viewer _viewer;

        public DialogWait(Viewer viewer)
        {
            InitializeComponent();
            _viewer = viewer;
            Started = DateTime.Now;
            timer1.Interval = 1000;
            timer1_Tick(null,null);
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan time = DateTime.Now - Started;
            lblTimeTaken.Text = (((time.Days * 24 + time.Hours) * 60) + time.Minutes) + " Minutes " + time.Seconds + " Seconds";
            //lblStatus.Text = _viewer.ReportStatus();
            Application.DoEvents();
        }
    }
}