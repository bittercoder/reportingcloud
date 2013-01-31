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

namespace ReportingCloud.Engine
{	
	///<summary>
	///The primary class to "run" a report to the supported output presentation types
	///</summary>

	public enum OutputPresentationType
	{
		HTML,
		PDF,
		XML,
		ASPHTML,
		Internal,
		MHTML,
        CSV,
        RTF,
        Excel,
        TIF,
        TIFBW                   // black and white tif
	}

	[Serializable]
	public class ProcessReport
	{
		Report r;					// report
		IStreamGen _sg;

		public ProcessReport(Report rep, IStreamGen sg)
		{
			if (rep.rl.MaxSeverity > 4)
				throw new Exception("Report has errors.  Cannot be processed.");

			r = rep;
			_sg = sg;
		}

		public ProcessReport(Report rep)
		{
			if (rep.rl.MaxSeverity > 4)
				throw new Exception("Report has errors.  Cannot be processed.");

			r = rep;
			_sg = null;
		}

		// Run the report passing the parameter values and the output
		public void Run(IDictionary parms, OutputPresentationType type)
		{
			r.RunGetData(parms);

			r.RunRender(_sg, type);

			return;
		}

	}
}
