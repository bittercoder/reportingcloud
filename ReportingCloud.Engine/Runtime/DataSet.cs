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
using System.Collections;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace ReportingCloud.Engine
{
	///<summary>
	/// Runtime Information about a set of data; public interface to the definition
	///</summary>
	[Serializable]
	public class DataSet
	{
		Report _rpt;		//	the runtime report
		DataSetDefn _dsd;	//  the true definition of the DataSet
	
		internal DataSet(Report rpt, DataSetDefn dsd)
		{
			_rpt = rpt;
			_dsd = dsd;
		}

		public void SetData(IDataReader dr)
		{
			_dsd.Query.SetData(_rpt, dr, _dsd.Fields, _dsd.Filters);		// get the data (and apply the filters
		}

		public void SetData(DataTable dt)
		{
			_dsd.Query.SetData(_rpt, dt, _dsd.Fields, _dsd.Filters);
		}

		public void SetData(XmlDocument xmlDoc)
		{
			_dsd.Query.SetData(_rpt, xmlDoc, _dsd.Fields, _dsd.Filters);
		}

		public void SetData(IEnumerable ie)
		{
			_dsd.Query.SetData(_rpt, ie, _dsd.Fields, _dsd.Filters);
		}

	}
}
