/*
·--------------------------------------------------------------------·
| ReportingCloud - ViewerHelper                                    |
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

using System.Collections;
using System.Collections.Generic;

namespace ReportingCloud.ViewerHelper
{
    public class ReportData
    {
        List<ReportViewerData> datas;

        public ReportData()
        {
            datas = new List<ReportViewerData>();
        }

        public ReportData(string dataName, IList data)
            : this()
        {
            Add(dataName, data);
        }

        /// <summary>
        /// Add a ilist object associate with a data name
        /// </summary>
        public void Add(string dataName, IList data)
        {
            datas.Add(new ReportViewerData(dataName, data));
        }

        /// <summary>
        /// Internal join a list of data
        /// </summary>
        internal void JoinReportData(ReportData reportData)
        {
            datas.AddRange(reportData.datas);
        }

        /// <summary>
        /// Return the report viewer data
        /// </summary>
        public ReportViewerData GetReportViewerData(int index)
        {
            return datas[index] as ReportViewerData;
        }

        /// <summary>
        /// Return the report viewer total objects
        /// </summary>
        public int GetCount()
        {
            return datas.Count;
        }
    }

    public class ReportViewerData
    {
        public ReportViewerData(string dataName, IList data)
        {
            this.DataName = dataName;
            this.Data = data;
        }

        public string DataName { get; set; }
        public IList Data { get; set; }
    }
}