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
	/// The collection of parameters for a subreport.
	///</summary>
	[Serializable]
	internal class SubReportParameters : ReportLink
	{
        List<SubreportParameter> _Items;			// list of SubreportParameter

		internal SubReportParameters(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			SubreportParameter rp;
            _Items = new List<SubreportParameter>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Parameter":
						rp = new SubreportParameter(r, this, xNodeLoop);
						break;
					default:	
						rp=null;		// don't know what this is
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown SubreportParameters element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
				if (rp != null)
					_Items.Add(rp);
			}
			if (_Items.Count > 0)
                _Items.TrimExcess();
		}
		
		override internal void FinalPass()
		{
			foreach (SubreportParameter rp in _Items)
			{
				rp.FinalPass();
			}
			return;
		}

        internal List<SubreportParameter> Items
		{
			get { return  _Items; }
		}
	}
}
