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
	/// Contains list of DataSource about how to connect to sources of data used by the DataSets.
	///</summary>
	[Serializable]
    public class DataSources : IEnumerable
	{
		Report _rpt;				// Runtime report
		ListDictionary _Items;		// list of report items

		internal DataSources(Report rpt, DataSourcesDefn dsds)
		{
			_rpt = rpt;
			_Items = new ListDictionary();

			// Loop thru all the child nodes
			foreach(DataSourceDefn dsd in dsds.Items.Values)
			{
				DataSource ds = new DataSource(rpt, dsd);
				_Items.Add(dsd.Name.Nm,	ds);
			}
		}

		public DataSource this[string name]
		{
			get 
			{
				return _Items[name] as DataSource;
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
