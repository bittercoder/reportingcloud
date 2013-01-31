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
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace ReportingCloud.Engine
{
	///<summary>
	/// Contains information about which classes to instantiate during report initialization.
	/// These instances can then be used in expressions throughout the report.
	///</summary>
	[Serializable]
	internal class Classes : ReportLink, IEnumerable
	{
        List<ReportClass> _Items;			// list of report class

		internal Classes(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
            _Items = new List<ReportClass>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				if (xNodeLoop.Name == "Class")
				{
					ReportClass rc = new ReportClass(r, this, xNodeLoop);
					_Items.Add(rc);
				}
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For Classes at least one Class is required.");
			else
                _Items.TrimExcess();
		}
		
		internal ReportClass this[string s]
		{
			get 
			{
				foreach (ReportClass rc in _Items)
				{
					if (rc.InstanceName.Nm == s)
						return rc;
				}
				return null;
			}
		}

		override internal void FinalPass()
		{
			foreach (ReportClass rc in _Items)
			{
				rc.FinalPass();
			}
			return;
		}

		internal void Load(Report rpt)
		{
			foreach (ReportClass rc in _Items)
			{
				rc.Load(rpt);
			}
			return;
		}

        internal List<ReportClass> Items
		{
			get { return  _Items; }
		}
		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _Items.GetEnumerator();
		}

		#endregion
	}
}
