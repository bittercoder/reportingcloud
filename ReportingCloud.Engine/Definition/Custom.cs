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

namespace ReportingCloud.Engine
{
	///<summary>
	///The Custom element allows report design tools to pass information to report output components.
	///This element may contain any valid XML. The engine will simply pass the contents of Custom
	///unchanged. Client applications using the Custom element are recommended to place their
	///custom properties under their own single subelement of Custom, defining a namespace for that
	///node.
	///  Example: 
	///   <Table><Custom><HTML><SortAble>True</SortAble></HTML></Custom> .... </Table>
	///     The HTML renderer uses this information to generate JavaScript to allow
	///     user sorting of the table in the browser.
	///</summary>
	[Serializable]
	internal class Custom : ReportLink
	{
		//The Custom element allows report design tools to pass information to report output components.
		//This element may contain any valid XML. The engine will simply pass the contents of Custom
		//unchanged. Client applications using the Custom element are recommended to place their
		//custom properties under their own single subelement of Custom, defining a namespace for that
		//node.
		//  Example: 
		//   <Table><Custom><HTML><SortAble>True</SortAble></HTML> .... </Table>
		//     The HTML renderer uses this information to generate JavaScript to allow
		//     user sorting of the table in the browser.
		string _XML;	// custom information for report.
		XmlDocument _CustomDoc;		// XML document just for Custom subtree
	
		internal Custom(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_XML= xNode.OuterXml;	// this includes the "Custom" tag at the top level

			// Put the subtree into its own document
			XmlDocument doc = new XmlDocument();
			doc.PreserveWhitespace = false;
			doc.LoadXml(_XML);
			_CustomDoc = doc;

		}
		
		override internal void FinalPass()
		{
			return;
		}

		internal string XML
		{
			get { return  _XML; }
			set {  _XML = value; }
		}

		internal XmlNode CustomXmlNode
		{
			get 
			{ 
				XmlNode xNode;
				xNode = _CustomDoc.LastChild;
				return  xNode; 
			}
		}
	}
}
