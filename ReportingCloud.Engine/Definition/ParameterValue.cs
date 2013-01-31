/*
�--------------------------------------------------------------------�
| ReportingCloud - Engine                                            |
| Copyright (c) 2010, FlexibleCoder.                                 |
| https://sourceforge.net/projects/reportingcloud                    |
�--------------------------------------------------------------------�
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
�--------------------------------------------------------------------�
*/

using System;
using System.Xml;

namespace ReportingCloud.Engine
{
	///<summary>
	/// The value of a report paramenter.
	///</summary>
	[Serializable]
	internal class ParameterValue : ReportLink
	{
		Expression _Value;		// Possible value (variant) for the parameter
								// For Boolean: use "true" and "false"
								// For DateTime: use ISO 8601
								// For Float: use "." as the optional decimal separator.
		Expression _Label;		// Label (string) for the value to display in the UI
								// If not supplied, the _Value is used as the label. If
								// _Value not supplied, _Label is the empty string;
	
		internal ParameterValue(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Value=null;
			_Label=null;

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "Value":
						_Value = new Expression(r, this, xNodeLoop, ExpressionType.Variant);
						break;
					case "Label":
						_Label = new Expression(r, this, xNodeLoop, ExpressionType.String);
						break;
					default:
						break;
				}
			}
		

		}

		// Handle parsing of function in final pass
		override internal void FinalPass()
		{
			if (_Value != null)
				_Value.FinalPass();
			if (_Label != null)
				_Label.FinalPass();
			return;
		}

		internal Expression Value
		{
			get { return  _Value; }
			set {  _Value = value; }
		}


		internal Expression Label
		{
			get { return  _Label; }
			set {  _Label = value; }
		}
	}
}
