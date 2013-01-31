/*
·--------------------------------------------------------------------·
| ReportingCloud - Engine                                            |
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
using System.Xml;
using System.IO;

namespace ReportingCloud.Engine
{
	///<summary>
	/// Definition of the header rows for a table.
	///</summary>
	[Serializable]
	internal class Header : ReportLink, ICacheData
	{
		TableRows _TableRows;	// The header rows for the table or group
		bool _RepeatOnNewPage;	// Indicates this header should be displayed on
								// each page that the table (or group) is displayed		

		internal Header(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_TableRows=null;
			_RepeatOnNewPage=false;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "TableRows":
						_TableRows = new TableRows(r, this, xNodeLoop);
						break;
					case "RepeatOnNewPage":
						_RepeatOnNewPage = XmlUtil.Boolean(xNodeLoop.InnerText, OwnerReport.rl);
						break;
					default:
						break;
				}
			}
			if (_TableRows == null)
				OwnerReport.rl.LogError(8, "Header requires the TableRows element.");
		}
		
		override internal void FinalPass()
		{
			_TableRows.FinalPass();

			OwnerReport.DataCache.Add(this);
			return;
		}

		internal void Run(IPresent ip, Row row)
		{
			_TableRows.Run(ip, row);
			return;
		}

        internal void RunPage(Pages pgs, Row row)
        {
            WorkClass wc = this.GetValue(pgs.Report);

            if (wc.OutputRow == row && wc.OutputPage == pgs.CurrentPage)
                return;

            Page p = pgs.CurrentPage;

            //we need to run through all parent  groupings to see if there are groups that we are to repeat on new page
            float height = p.YOffset + HeightOfRows(pgs, row);
            if (height > pgs.BottomOfPage)
            {
                bool bRepeatedParent = this.RepeatOnNewPage;
                Table t = OwnerTable;
                if (Parent.GetType() == typeof(TableGroup))
                {
                    TableGroup tg = (TableGroup)Parent;
                    TableGroups tmp = (TableGroups)tg.Parent;

                    for (int i = 0; i < tmp.Items.Count; i++)
                    {
                        if (tmp.Items[i] == tg) //if we reached current header - break(no need to look at child groups)
                            break;
                        if (tmp.Items[i].Header._RepeatOnNewPage)
                        {
                            bRepeatedParent = true;
                            break;
                        }
                    }
                }
                //if we have repeated parent group - we call RunPageHeader to repeat them
                //if current header is repeating too - we return(as we already put it)
                //if no - we put it to new page
                p = t.RunPageNew(pgs, p);
                if (bRepeatedParent)
                {
                    t.RunPageHeader(pgs, row, false, null);
                    if (this.RepeatOnNewPage)
                        return;
                }
            }
            //this will add current header
            _TableRows.RunPage(pgs, row);
            wc.OutputRow = row;
            wc.OutputPage = pgs.CurrentPage;
            return;
        }

		internal Table OwnerTable
		{
			get 
			{
				for (ReportLink rl = this.Parent; rl != null; rl = rl.Parent)
				{
					if (rl is Table)
						return rl as Table;
				}

				return null; 
			}
		}

		internal TableRows TableRows
		{
			get { return  _TableRows; }
			set {  _TableRows = value; }
		}
 
		internal float HeightOfRows(Pages pgs, Row r)
		{
			return _TableRows.HeightOfRows(pgs, r);
		}

		internal bool RepeatOnNewPage
		{
			get { return  _RepeatOnNewPage; }
			set {  _RepeatOnNewPage = value; }
		}
		#region ICacheData Members

		public void ClearCache(Report rpt)
		{
			rpt.Cache.Remove(this, "wc");
		}

		#endregion

		private WorkClass GetValue(Report rpt)
		{
			WorkClass wc = rpt.Cache.Get(this, "wc") as WorkClass;
			if (wc == null)
			{
				wc = new WorkClass();
				rpt.Cache.Add(this, "wc", wc);
			}
			return wc;
		}

		private void SetValue(Report rpt, WorkClass w)
		{
			rpt.Cache.AddReplace(this, "wc", w);
		}

		class WorkClass
		{
			internal Row OutputRow;		// the previous outputed row
			internal Page OutputPage;	// the previous outputed row
			internal WorkClass()
			{
				OutputRow = null;
				OutputPage = null;
			}
		}
	}
}
