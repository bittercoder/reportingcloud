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
using System.Globalization;
using ReportingCloud.Engine;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// Obtain the the Field's value from a row.
	/// </summary>
	[Serializable]
	internal class FunctionField : IExpr
	{
		protected Field f;	
		private string _Name;		// when we have an unresolved field;

		/// <summary>
		/// obtain value of Field
		/// </summary>
		public FunctionField(Field fld) 
		{
			f = fld;
		}

		public FunctionField(string name) 
		{
			_Name = name;
		}

		public string Name
		{
			get {return _Name;}
		}

		public virtual TypeCode GetTypeCode()
		{
			return f.RunType;
		}

		public virtual Field Fld
		{
			get { return f; }
			set { f = value; }
		}

		public virtual bool IsConstant()
		{
			return false;
		}

		public virtual IExpr ConstantOptimization()
		{	
			if (f.Value != null)
				return 	f.Value.ConstantOptimization();

			return this;	// not a constant
		}

		// 
		public virtual object Evaluate(Report rpt, Row row)
		{
			if (row == null)
				return null;
			object o;
			if (f.Value != null)
				o = f.Value.Evaluate(rpt, row);
			else
				o = row.Data[f.ColumnNumber];

            if (o == DBNull.Value)
            {
                if (IsNumericType(f.RunType))
                    return double.NaN;
                else
                    return null;
            }
			if (f.RunType == TypeCode.String && o is char)	// work around; mono odbc driver confuses string and char
				o = Convert.ChangeType(o, TypeCode.String);
			
			return o;
		}

        private bool IsNumericType(TypeCode tc)
        {
            switch (tc)
            {
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

		public virtual double EvaluateDouble(Report rpt, Row row)
		{
			if (row == null)
				return Double.NaN;
			return Convert.ToDouble(Evaluate(rpt, row), NumberFormatInfo.InvariantInfo);
		}
		
		public virtual decimal EvaluateDecimal(Report rpt, Row row)
		{
			if (row == null)
				return decimal.MinValue;
			return Convert.ToDecimal(Evaluate(rpt, row), NumberFormatInfo.InvariantInfo);
		}

        public virtual int EvaluateInt32(Report rpt, Row row)
        {
            if (row == null)
                return int.MinValue;
            return Convert.ToInt32(Evaluate(rpt, row), NumberFormatInfo.InvariantInfo);
        }

		public virtual string EvaluateString(Report rpt, Row row)
		{
			if (row == null)
				return null;
			return Convert.ToString(Evaluate(rpt, row));
		}

		public virtual DateTime EvaluateDateTime(Report rpt, Row row)
		{
			if (row == null)
				return DateTime.MinValue;
			return Convert.ToDateTime(Evaluate(rpt, row));
		}

		public virtual bool EvaluateBoolean(Report rpt, Row row)
		{
			if (row == null)
				return false;
			return Convert.ToBoolean(Evaluate(rpt, row));
		}
	}
}
