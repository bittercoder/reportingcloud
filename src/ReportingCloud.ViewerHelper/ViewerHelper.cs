/*
·--------------------------------------------------------------------·
| ReportingCloud - ViewerHelper                                    |
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
using System.Threading;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

namespace ReportingCloud.ViewerHelper
{
    public class ViewerHelper
    {
        public enum RenderType
        {
            PDF,
            TIF,
            TIFBW,
            CSV,
            RTF,
            XLSX,
            XML,
            HTML,
            MHTML
        }
        
        private string reportFolder = null;
        private ReportParameter parameters;
        private ReportData datas;
        private ReportingCloud.Viewer.Viewer reportViewer;

        public ViewerHelper(string reportFolder)
        {
            this.reportFolder = reportFolder;
            parameters = new ReportParameter();
            datas = new ReportData();
            reportViewer = new ReportingCloud.Viewer.Viewer();
            reportViewer.ShowWaitDialog = false;
        }

        /// <summary>
        /// Use embedded report files
        /// </summary>
        public static bool EmbeddedReportFiles { get; set; }

        /// <summary>
        /// Add a report parameter object to the list of parameters
        /// </summary>
        public void AddParameter(ReportParameter reportParameter)
        {
            parameters.JoinReportParameter(reportParameter);
        }

        /// <summary>
        /// Add a parameter
        /// </summary>
        public void AddParameter(string variable, string value)
        {
            parameters.Add(variable, value);
        }

        /// <summary>
        /// Add a parameter of type image
        /// </summary>
        public void AddParameter(string variable, Image value)
        {
            parameters.Add(variable, value);
        }

        /// <summary>
        /// Add a parameter of type byte array
        /// </summary>
        public void AddParameter(string variable, byte[] value)
        {
            parameters.Add(variable, value);
        }

        /// <summary>
        /// Add a report data object to the list of datas
        /// </summary>
        public void AddData(ReportData reportData)
        {
            datas.JoinReportData(reportData);
        }

        /// <summary>
        /// Add a ilist object data
        /// </summary>
        public void AddData(string dataName, IList data)
        {
            datas.Add(dataName, data);
        }

        /// <summary>
        /// Load the viewer with the default render type (pdf)
        /// </summary>
        public string Load(string report)
        {
            return Load(report, RenderType.PDF);
        }

        /// <summary>
        /// Load the viewer with an specific render type
        /// </summary>
        public string Load(string report, RenderType renderType)
        {
            //read the report stream
            string contents = null;

            //read the report stream from an embedded resource or an external file
            System.IO.Stream stream = null;
            if (EmbeddedReportFiles)
                stream = Assembly.GetCallingAssembly().GetManifestResourceStream(report);
            else
                stream = File.OpenRead(report);

            using (StreamReader reader = new StreamReader(stream))
                contents = reader.ReadToEnd();

            //load the parameters into the viewer
            reportViewer.Parameters = parameters.GetReportStringParameters();

            //load the report contents into the report viewer
            reportViewer.SourceRdl = contents;

            try
            {
                //reload the data objects into the report viewer
                for (int i = 0; i < datas.GetCount(); i++)
                    reportViewer.Report.DataSets[datas.GetReportViewerData(i).DataName].SetData(
                        datas.GetReportViewerData(i).Data);

                //refresh the report
                reportViewer.Rebuild();

                //unique identifier for the file name
                string fileName = Guid.NewGuid().ToString();

                //render the document
                reportViewer.SaveAs(string.Format(@"{0}\{1}.pdf", reportFolder, fileName), renderType.ToString());

                //return the saved file
                return string.Format("{0}.pdf", fileName);
            }
            catch (NullReferenceException ex)
            {
                throw new Exception(string.Format("Could not load the report !\r\n{0}", ex.Message));
            }
            catch (ArgumentException)
            {
                //can not pass the report argument because the report viewer could not reload the data
                //because there is an error in the report definition, so te report it self will print the error
            }
            return null;
        }
    }
}