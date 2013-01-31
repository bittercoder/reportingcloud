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
using System.Collections.Specialized;
using System.Xml;

namespace ReportingCloud.Engine
{
	///<summary>
	/// The sets of data (defined by DataSet) that are retrieved as part of the Report.
	///</summary>
	[Serializable]
	internal class DataSetsDefn : ReportLink
	{
		IDictionary _Items;			// list of report items

		internal DataSetsDefn(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			if (xNode.ChildNodes.Count < 10)
				_Items = new ListDictionary();	// Hashtable is overkill for small lists
			else
				_Items = new Hashtable(xNode.ChildNodes.Count);

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				if (xNodeLoop.Name == "DataSet")
				{
					DataSetDefn ds = new DataSetDefn(r, this, xNodeLoop);
					if (ds != null && ds.Name != null)
						_Items.Add(ds.Name.Nm, ds);
				}
			}
		}

		internal DataSetDefn this[string name]
		{
			get 
			{
				return _Items[name] as DataSetDefn;
			}
		}
		
		override internal void FinalPass()
		{
			foreach (DataSetDefn ds in _Items.Values)
			{
				ds.FinalPass();
			}
			return;
		}

		internal bool GetData(Report rpt)
		{
            bool haveRows = false;
			foreach (DataSetDefn ds in _Items.Values)
			{
				haveRows |= ds.GetData(rpt);
			}

			return haveRows;
		}

		internal IDictionary Items
		{
			get { return  _Items; }
		}
	}
}
