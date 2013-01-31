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
using System.Reflection;

namespace ReportingCloud.Engine
{
	///<summary>
	/// CodeModules definition and processing.
	///</summary>
	[Serializable]
	internal class CodeModules : ReportLink, IEnumerable
	{
        List<CodeModule> _Items;			// list of code module

		internal CodeModules(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
            _Items = new List<CodeModule>();
			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				if (xNodeLoop.Name == "CodeModule")
				{
					CodeModule cm = new CodeModule(r, this, xNodeLoop);
					_Items.Add(cm);
				}
				else
				{
					// don't know this element - log it
					OwnerReport.rl.LogError(4, "Unknown CodeModules element '" + xNodeLoop.Name + "' ignored.");
				}
			}
			if (_Items.Count == 0)
				OwnerReport.rl.LogError(8, "For CodeModules at least one CodeModule is required.");
			else
                _Items.TrimExcess();
		}
		/// <summary>
		/// Return the Type given a class name.  Searches the CodeModules that are specified
		/// in the report.
		/// </summary>
		internal Type this[string s]
		{
			get 
			{
				Type tp=null;
                if (s == string.Empty)
                    return null;
				try
				{
					// loop thru all the codemodules looking for the assembly
					//  that contains this type
					foreach (CodeModule cm in _Items)
					{
						Assembly a = cm.LoadedAssembly();
						if (a != null)
						{
							tp = a.GetType(s,false,true);
							if (tp != null)
								break;
						}
					}
				}
				catch(Exception ex) 
				{
					OwnerReport.rl.LogError(4, string.Format("Exception finding type. {0}", ex.Message));
				}
				return tp;
			}
		}
		
		override internal void FinalPass()
		{
			foreach (CodeModule cm in _Items)
			{
				cm.FinalPass();
			}
			return;
		}

		internal void LoadModules()
		{
			foreach (CodeModule cm in _Items)
			{
				cm.LoadedAssembly();
			}
		}

        internal List<CodeModule> Items
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
