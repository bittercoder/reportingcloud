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
using System.Collections.Specialized;
using System.Xml;

namespace ReportingCloud.Engine
{
	///<summary>
	/// The sets of data (defined by DataSet) that are retrieved as part of the Report.
	///</summary>
	[Serializable]
    public class DataSets : IEnumerable
	{
		Report _rpt;				// runtime report
		IDictionary _Items;			// list of report items

		internal DataSets(Report rpt, DataSetsDefn dsn)
		{
			_rpt = rpt;

			if (dsn.Items.Count < 10)
				_Items = new ListDictionary();	// Hashtable is overkill for small lists
			else
				_Items = new Hashtable(dsn.Items.Count);

			// Loop thru all the child nodes
			foreach(DataSetDefn dsd in dsn.Items.Values)
			{
				DataSet ds = new DataSet(rpt, dsd);
				_Items.Add(dsd.Name.Nm, ds);
			}
		}
		
		public DataSet this[string name]
		{
			get 
			{
				return _Items[name] as DataSet;
			}
		}

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return _Items.Values.GetEnumerator();
        }

        #endregion
    }
}
