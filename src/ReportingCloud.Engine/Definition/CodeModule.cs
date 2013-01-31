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
using System.Reflection;

namespace ReportingCloud.Engine
{
	///<summary>
	/// CodeModule definition and processing.
	///</summary>
	[Serializable]
	internal class CodeModule : ReportLink
	{
		string _CodeModule;	// Name of the code module to load
		[NonSerialized] Assembly _LoadedAssembly=null;	// 
		[NonSerialized] bool bLoadFailed=false;
	
		internal CodeModule(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_CodeModule=xNode.InnerText;
		}

		internal Assembly LoadedAssembly()
		{
			if (bLoadFailed)		// We only try to load once.
				return null;

			if (_LoadedAssembly == null)
			{
				try
				{
					_LoadedAssembly = XmlUtil.AssemblyLoadFrom(_CodeModule);
				}
				catch (Exception e)
				{
					OwnerReport.rl.LogError(4, String.Format("CodeModule {0} failed to load.  {1}",
						_CodeModule, e.Message));
					bLoadFailed = true;
				}
			}
			return _LoadedAssembly;
		}

		override internal void FinalPass()
		{
			return;
		}

		internal string CdModule
		{
			get { return  _CodeModule; }
			set {  _CodeModule = value; }
		}
	}
}
