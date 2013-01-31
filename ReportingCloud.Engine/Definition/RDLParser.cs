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
using System.IO;
using System.Globalization;
using System.Xml;
using ReportingCloud.Engine;

namespace ReportingCloud.Engine
{
	/// <summary>
	///	The RDLParser class takes an XML representation (either string or DOM) of a
	///	RDL file and compiles a Report.
	/// </summary>
	public class RDLParser
	{
		XmlDocument _RdlDocument;	// the RDL XML syntax
		bool bPassed=false;		// has Report passed definition
		Report _Report=null;	// The report; complete if bPassed true
		NeedPassword _DataSourceReferencePassword;	// password for decrypting data source reference file
		string _Folder;			// folder that will contain report; needed when DataSourceReference used

		/// <summary>
		/// RDLParser takes in an RDL XML file and creates the
		/// definition that will be used at runtime.  It validates
		/// that the syntax is correct according to the specification.
		/// </summary>
		public RDLParser(String xml) 
		{
			try 
			{
				_RdlDocument = new XmlDocument();
				_RdlDocument.PreserveWhitespace = false;
				_RdlDocument.LoadXml(xml);
			}
			catch (XmlException ex)
			{
				throw new ParserException("Error: XML failed " + ex.Message);
			}
		}

		/// <summary>
		/// RDLParser takes in an RDL XmlDocument and creates the
		/// definition that will be used at runtime.  It validates
		/// that the syntax is correct according to the specification.
		/// </summary>		
		public RDLParser(XmlDocument xml) // preparsed XML
		{
			_RdlDocument = xml;
		}

		internal XmlDocument RdlDocument
		{
			get { return _RdlDocument; }
			set 
			{
				// With a new document existing report is not valid
				_RdlDocument = value; 
				bPassed = false;
				_Report = null;
			}
		}

		/// <summary>
		/// Get the compiled report.
		/// </summary>
		public Report Report
		{
			// Only return a report if it has been fully constructed
			get 
			{
				if (bPassed)
					return  _Report; 
				else
					return null;
			}
		}

		/// <summary>
		/// Returns a parsed RPL report instance.
		/// </summary>
		/// 
		/// <returns>A Report instance.</returns>
		public Report Parse()
		{
			return Parse(0);
		}
		
		internal Report Parse(int oc)
			{
			if (_RdlDocument == null)	// no document?
				return null;			// nothing to do
			else if (bPassed)			// If I've already parsed it
				return _Report;			// then return existing Report
			//  Need to create a report.
			XmlNode xNode;
			xNode = _RdlDocument.LastChild;
			if (xNode == null || xNode.Name != "Report")
			{
				throw new ParserException("Error: RDL doesn't contain a report element. ");
			}
			
			ReportLog rl = new ReportLog();		// create a report log

			ReportDefn rd = new ReportDefn(xNode, rl, this._Folder, this._DataSourceReferencePassword, oc);
			_Report = new Report(rd);
			
			bPassed = true;

			return _Report;
		}

		/// <summary>
		/// For shared data sources, the DataSourceReferencePassword is the user phrase
		/// used to decrypt the report.
		/// </summary>
        public NeedPassword DataSourceReferencePassword
		{
			get { return _DataSourceReferencePassword; }
			set { _DataSourceReferencePassword = value; }
		}
		/// <summary>
		/// Folder is the location of the report.
		/// </summary>
		public string Folder
		{
			get { return _Folder; }
			set { _Folder = value; }
		}
	}
}
