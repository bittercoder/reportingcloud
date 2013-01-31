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
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ReportingCloud.ViewerHelper
{
    public class ReportParameter
    {
        private List<string> parameters;

        public ReportParameter()
        {
            parameters = new List<string>();
        }

        /// <summary>
        /// Add a report parameter
        /// </summary>
        public void Add(string variable, string value)
        {
            parameters.Add(variable + "=" + value);
        }

        /// <summary>
        /// Add a integer report parameter
        /// </summary>
        public void Add(string variable, int value)
        {
            Add(variable, value.ToString());
        }

        /// <summary>
        /// Add a report parameter of type byte array converting to base 64 string
        /// </summary>
        public void Add(string variable, byte[] value)
        {
            //convert the byte array to base 64 string
            string base64 = Convert.ToBase64String(value);

            //add to parameter
            Add(variable, base64);
        }

        /// <summary>
        /// Add a report parameter of type image converting to base 64 string
        /// </summary>
        public void Add(string variable, Image value)
        {
            //map the image to png format
            MemoryStream memoryStream = new MemoryStream();
            value.Save(memoryStream, ImageFormat.Png);

            //convert the image map to base 64 string
            string base64 = Convert.ToBase64String(memoryStream.ToArray());
            memoryStream.Close();

            //add to parameter
            Add(variable, base64);
        }

        /// <summary>
        /// Internal join a list of parameters
        /// </summary>
        internal void JoinReportParameter(ReportParameter reportParameter)
        {
            parameters.AddRange(reportParameter.parameters);
        }

        /// <summary>
        /// Return a string with the parameter format
        /// </summary>
        public string GetReportStringParameters()
        {
            string stringParameters = string.Empty;
            for (int i = 0; i < parameters.Count; i++)
            {
                if (i > 0)
                    stringParameters += "&";
                stringParameters += parameters[i];
            }
            return stringParameters;
        }
    }
}