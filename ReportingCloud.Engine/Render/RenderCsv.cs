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
using ReportingCloud.Engine;
using System.IO;
using System.Collections;
using System.Text;

namespace ReportingCloud.Engine
{
    ///<summary>
    ///The primary class to "run" a report to XML
    ///</summary>
    internal class RenderCsv : IPresent
    {
        Report report;					// report
        DelimitedTextWriter tw;				// where the output is going

        public RenderCsv(Report report, IStreamGen sg)
        {
            this.report = report;
            tw = new DelimitedTextWriter(sg.GetTextWriter(), ",");
        }

        public Report Report()
        {
            return report;
        }

        public bool IsPagingNeeded()
        {
            return false;
        }

        public void Start()
        {
        }

        public void End()
        {

        }									

        public void RunPages(Pages pgs)
        {
        }					

        public void BodyStart(Body b)
        {
        }

        public void BodyEnd(Body b)
        {
        }

        public void PageHeaderStart(PageHeader ph)
        {
        }

        public void PageHeaderEnd(PageHeader ph)
        {
            if (ph.PrintOnFirstPage||ph.PrintOnLastPage) {tw.WriteLine();}           
        }

        public void PageFooterStart(PageFooter pf)
        {
        }

        public void PageFooterEnd(PageFooter pf)
        {
            if (pf.PrintOnLastPage || pf.PrintOnFirstPage) {tw.WriteLine();}
        }

        public void Textbox(Textbox tb, string t, Row r)
        {
            object value = tb.Evaluate(report, r);
            tw.Write(value);
        }	
        
        public void DataRegionNoRows(DataRegion d, string noRowsMsg)
        {
        }

        public bool ListStart(List l, Row r)
        {
            return true;
        }
        
        public void ListEnd(List l, Row r)
        {
            tw.WriteLine();
        }
        
        public void ListEntryBegin(List l, Row r)
        {
        }
        public void ListEntryEnd(List l, Row r)
        {
            tw.WriteLine();
        }

        public bool TableStart(Table t, Row r)
        {
            return true;
        }
        
        public void TableEnd(Table t, Row r)
        {
        }
        
        public void TableBodyStart(Table t, Row r)
        {
        }
        
        public void TableBodyEnd(Table t, Row r)
        {
        }
        
        public void TableFooterStart(Footer f, Row r)
        {
        }
        
        public void TableFooterEnd(Footer f, Row r)
        {
        }
        
        public void TableHeaderStart(Header h, Row r)
        {
        }
        
        public void TableHeaderEnd(Header h, Row r)
        {
        }
        
        public void TableRowStart(TableRow tr, Row r)
        {
        }
        
        public void TableRowEnd(TableRow tr, Row r)
        {
            tw.WriteLine();
        }
        
        public void TableCellStart(TableCell t, Row r)
        {
        }
        
        public void TableCellEnd(TableCell t, Row r)
        {
        }

        public bool MatrixStart(Matrix m, MatrixCellEntry[,] matrix, Row r, int headerRows, int maxRows, int maxCols)
        {
            return true;
        }
        
        public void MatrixColumns(Matrix m, MatrixColumns mc)
        {
        }

        public void MatrixRowStart(Matrix m, int row, Row r)
        {
        }
        
        public void MatrixRowEnd(Matrix m, int row, Row r)
        {
            tw.WriteLine();
        }
        
        public void MatrixCellStart(Matrix m, ReportItem ri, int row, int column, Row r, float h, float w, int colSpan)
        {
        }

        public void MatrixCellEnd(Matrix m, ReportItem ri, int row, int column, Row r)
        {
        }

        public void MatrixEnd(Matrix m, Row r)
        {
        }
        
        public void Chart(Chart c, Row r, ChartBase cb)
        {
        }

        public void Image(Image i, Row r, string mimeType, Stream io)
        {
        }

        public void Line(Line l, Row r)
        {
        }

        public bool RectangleStart(Rectangle rect, Row r)
        {
            return true;
        }
        
        public void RectangleEnd(Rectangle rect, Row r)
        {
        }	
        
        public void Subreport(Subreport s, Row r)
        {
        }

        public void GroupingStart(Grouping g)
        {
        }
        
        public void GroupingInstanceStart(Grouping g)
        {
        }
        
        public void GroupingInstanceEnd(Grouping g)
        {
        }
        
        public void GroupingEnd(Grouping g)
        {
        }
    }
}
