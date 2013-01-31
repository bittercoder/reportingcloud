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
using System.Reflection;
using ReportingCloud.Engine;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// Binary operator
	/// </summary>
	[Serializable]
	internal abstract class FunctionBinary
	{
		public IExpr _lhs;			// lhs 
		public IExpr _rhs;			// rhs

		/// <summary>
		/// Arbitrary binary operater; might be a
		/// </summary>
		public FunctionBinary() 
		{
			_lhs = null;
			_rhs = null;
		}

		public FunctionBinary(IExpr l, IExpr r) 
		{
			_lhs = l;
			_rhs = r;
		}

		public bool IsConstant()
		{
			if (_lhs.IsConstant())
				return _rhs.IsConstant();

			return false;
		}

//		virtual public bool EvaluateBoolean(Report rpt, Row row)
//		{
//			return false;
//		}

		public IExpr Lhs
		{
			get { return  _lhs; }
			set {  _lhs = value; }
		}

		public IExpr Rhs
		{
			get { return  _rhs; }
			set {  _rhs = value; }
		}
	}
}
