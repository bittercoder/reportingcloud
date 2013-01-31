/*
�--------------------------------------------------------------------�
| ReportingCloud - Engine                                            |
| Copyright (c) 2010, FlexibleCoder.                                 |
| https://sourceforge.net/projects/reportingcloud                    |
�--------------------------------------------------------------------�
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
�--------------------------------------------------------------------�
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ReportingCloud.Engine
{
    //takes the record data and returns the instructions for drawing...I guess.    
    internal class Comment : DrawBase
    {
        internal Comment(Single Xin, Single Yin, Single WidthIn, Single HeightIn, System.Collections.Hashtable ObjectTableIn)
        {
            X = Xin;
            Y = Yin;
            Width = WidthIn;
            Height = HeightIn;
            ObjectTable = ObjectTableIn;
            items = new List<PageItem>();
        }

        public List<PageItem> Process(int Flags, byte[] RecordData)
        {
            MemoryStream _ms = null;
            BinaryReader _br = null;
            try
            {
                _ms = new MemoryStream(RecordData);
                _br = new BinaryReader(_ms);

                Byte[] PrivateData = _br.ReadBytes(RecordData.Length);
                //Ok we should have our private data which I am storing my tooltip value in...
                //now lets interpret it...            
                string PData = new System.Text.ASCIIEncoding().GetString(PrivateData);
                //If string starts with "ToolTip" then lets do something with it.. otherwise I don't care about it.
                if (PData.StartsWith("ToolTip"))
                {
                    PageRectangle pr = new PageRectangle();
                    StyleInfo si = new StyleInfo();
                    pr.SI = si;
                    //si.BackgroundColor = Color.Blue;// Just a test to see where the tooltip is being drawn
                    string[] ttd = PData.Split('|');
                    pr.Tooltip = ttd[0].Split(':')[1];
                    pr.X = X + Single.Parse(ttd[1].Split(':')[1]) * SCALEFACTOR;
                    pr.Y = Y + Single.Parse(ttd[2].Split(':')[1]) * SCALEFACTOR;
                    pr.W = Single.Parse(ttd[3].Split(':')[1]) * SCALEFACTOR;
                    pr.H = Single.Parse(ttd[4].Split(':')[1]) * SCALEFACTOR;
                    items.Add(pr);
                }
                else if (PData.StartsWith("PolyToolTip"))
                {
                    PagePolygon pp = new PagePolygon();
                    StyleInfo si = new StyleInfo();
                    pp.SI = si;
                    //si.BackgroundColor = Color.Blue;// Just a test to see where the tooltip is being drawn
                    string[] ttd = PData.Split('|');
                    PointF[] pts = new PointF[(ttd.Length - 1) / 2];
                    pp.Points = pts;
                    pp.Tooltip = ttd[0].Split(':')[1];
                    for (int i = 0; i < pts.Length; i++)
                    {
                        pts[i].X = X + Single.Parse(ttd[i*2 +1]) * SCALEFACTOR;
                        pts[i].Y = Y + Single.Parse(ttd[i*2 +2]) * SCALEFACTOR;
                    }
                    items.Add(pp);
                }
                return items;
            }

            finally
            {
                if (_br != null)
                    _br.Close();
                if (_ms != null)
                    _ms.Dispose();

            }
        }
    }
}
