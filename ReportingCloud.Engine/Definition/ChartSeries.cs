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

namespace ReportingCloud.Engine
{
	///<summary>
	/// Chart series definition and processing.
	///</summary>
	[Serializable]
	internal class ChartSeries : ReportLink
	{
        String _Colour;
        DataPoints _Datapoints;	// Data points within a series
		PlotTypeEnum _PlotType;		// Indicates whether the series should be plotted
								// as a line in a Column chart. If set to auto,
								// should be plotted per the primary chart type.
								// Auto (Default) | Line	
        String _YAxis;          //Indicates if the series uses the left or right axis. GJL 140208
        bool _NoMarker;          //Indicates if the series should not show its plot markers. GJL 300508
        String _LineSize;
	
		internal ChartSeries(ReportDefn r, ReportLink p, XmlNode xNode) : base(r, p)
		{
			_Datapoints=null;
			_PlotType=PlotTypeEnum.Auto;
            _YAxis = "Left";
            _NoMarker = false;
            _LineSize = "Regular";

			// Loop thru all the child nodes
			foreach(XmlNode xNodeLoop in xNode.ChildNodes)
			{
				if (xNodeLoop.NodeType != XmlNodeType.Element)
					continue;
				switch (xNodeLoop.Name)
				{
					case "DataPoints":
						_Datapoints = new DataPoints(r, this, xNodeLoop);
						break;
					case "PlotType":
                        _PlotType = Engine.PlotType.GetStyle(xNodeLoop.InnerText, OwnerReport.rl);
						break;
                    case "YAxis":
                    case "fyi:YAxis":
                        _YAxis = xNodeLoop.InnerText;
                        break;
                    case "NoMarker":
                    case "fyi:NoMarker":
                        _NoMarker = Boolean.Parse(xNodeLoop.InnerText);
                        break;
                    case "LineSize":
                    case "fyi:LineSize":
                        _LineSize = xNodeLoop.InnerText;
                        break;
                    case "fyi:Color":
                    case "Color":
                    case "Colour":
                        _Colour = xNodeLoop.InnerText;
                        break;
					default:	
						// don't know this element - log it
						OwnerReport.rl.LogError(4, "Unknown ChartSeries element '" + xNodeLoop.Name + "' ignored.");
						break;
				}
			}
			if (_Datapoints == null)
				OwnerReport.rl.LogError(8, "ChartSeries requires the DataPoints element.");
		}
		
		override internal void FinalPass()
		{
			if (_Datapoints != null)
				_Datapoints.FinalPass();
			return;
		}

		internal DataPoints Datapoints
		{
			get { return  _Datapoints; }
			set {  _Datapoints = value; }
		}

		internal PlotTypeEnum PlotType
		{
			get { return  _PlotType; }
			set {  _PlotType = value; }
		}

        internal String Colour
        {
            get { return _Colour; }
            set { _Colour = value; }
        }

        internal String YAxis
        {
            get { return _YAxis; }
            set { _YAxis = value; }
        }

        internal bool NoMarker
        {
            get { return _NoMarker; }
            set { _NoMarker = value; }
        }

        internal string LineSize
        {
            get { return _LineSize; }
            set { _LineSize = value; }
        }
	}
}
