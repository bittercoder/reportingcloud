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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ReportingCloud.Engine
{
    class DrawBase
    {
        protected const float SCALEFACTOR = 72f / 96f;
        protected System.Collections.Hashtable ObjectTable;
        protected Single X;
        protected Single Y;
        protected Single Width;
        protected Single Height;
        protected List<PageItem> items;

        protected static BorderStyleEnum getLineStyle(Pen p)
        {
            BorderStyleEnum ls = BorderStyleEnum.Solid;
            switch (p.DashStyle)
            {               
                case DashStyle.Dash:
                    ls = BorderStyleEnum.Dashed;
                    break;
                case DashStyle.DashDot:
                    ls = BorderStyleEnum.Dashed;
                    break;
                case DashStyle.DashDotDot:
                    ls = BorderStyleEnum.Dashed;
                    break;
                case DashStyle.Dot: 
                    ls = BorderStyleEnum.Dotted;
                    break;
                case DashStyle.Solid:
                    ls = BorderStyleEnum.Solid;
                    break;
                case DashStyle.Custom:
                    ls = BorderStyleEnum.Solid;
                    break;
                default:                   
                    break;
            }  
            return ls;
        }

       
    }

    
}
