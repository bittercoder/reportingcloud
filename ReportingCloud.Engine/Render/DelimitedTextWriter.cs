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
using System.IO;
using System.Text;

namespace ReportingCloud.Engine
{
    internal class DelimitedTextWriter 
    {
        private TextWriter textWriter;

        private string columnDelimiter;
        private bool firstColumn = true;

        private char quote = '"';

        public TextWriter TextWriter
        {
            get
            {
                return textWriter;
            }
        }

        public char Quote
        {
            get { return quote; }
        }

        public string ColumnDelimiter
        {
            get { return columnDelimiter; }
        }

        private string rowDelimiter = "\n";

        public string RowDelimiter
        {
            get { return rowDelimiter; }
        }

        protected void WriteQuote()
        {
            textWriter.Write(quote);
        }

        protected void WriteDelimeter()
        {
            if (!firstColumn)
            {
                textWriter.Write(columnDelimiter);
            }

            firstColumn = false;
        }

        public DelimitedTextWriter(TextWriter writer, string delimeter)
        {
            textWriter = writer;
            columnDelimiter = delimeter;
        }

        private void WriteQuoted(object value)
        {
            WriteDelimeter();
            WriteQuote();

            if ( value != null )
                textWriter.Write(value.ToString().Replace("\"", "\"\""));

            WriteQuote();
        }

        private void WriteUnquoted(object value)
        {
            WriteDelimeter();

            if ( value != null )
                textWriter.Write(value);
        }

        public void Write(object value)
        {
            bool isQuoted = true;

            if (value != null)
            {
                Type type = value.GetType();

                if (type.IsPrimitive &&
                    type != typeof(bool) && type != typeof(char))
                {
                    isQuoted = false;
                }
            }

            if (isQuoted)
                WriteQuoted(value);
            else
                WriteUnquoted(value);
        }

        public void Write(string format, params object[] arg)
        {
            WriteQuoted(string.Format(format, arg));
        }

        public void WriteLine()
        {
            textWriter.Write(rowDelimiter);
            firstColumn = true;
        }

        public void WriteLine(object value)
        {
            Write(value);
            WriteLine();
        }

        public void WriteLine(string format, params object[] arg)
        {
            Write(format, arg);
            WriteLine();
        }
    }
}