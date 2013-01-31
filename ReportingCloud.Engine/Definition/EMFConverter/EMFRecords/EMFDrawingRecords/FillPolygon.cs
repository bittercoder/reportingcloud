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
    internal class FillPolygon : DrawBase
    {
        internal FillPolygon(Single Xin, Single Yin, Single WidthIn, Single HeightIn, System.Collections.Hashtable ObjectTableIn)
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
            MemoryStream _fs = null;
            BinaryReader _fr = null;
            try
            {
                _fs = new MemoryStream(BitConverter.GetBytes(Flags));
                _fr = new BinaryReader(_fs);
                _fr.ReadByte();
                //Byte 2 is the real flags
                byte RealFlags = _fr.ReadByte();
                // 0 1 2 3 4 5 6 7
                // X X X X X X C S                
                // if C = 1 Data int16 else float!
                bool Compressed = ((RealFlags & (int)Math.Pow(2, 6)) == (int)Math.Pow(2, 6));
                bool BrushIsARGB = ((RealFlags & (int)Math.Pow(2, 7)) == (int)Math.Pow(2, 7));
                _ms = new MemoryStream(RecordData);
                _br = new BinaryReader(_ms);
                Brush b;
                if (BrushIsARGB)
                {
                    byte A, R, G, B;
                    B = _br.ReadByte();
                    G = _br.ReadByte();
                    R = _br.ReadByte();
                    A = _br.ReadByte();
                    b = new SolidBrush(Color.FromArgb(A, R, G, B));
                }
                else
                {
                    UInt32 BrushID = _br.ReadUInt32();
                    EMFBrush EMFb = (EMFBrush)ObjectTable[(byte)BrushID];
                    b = EMFb.myBrush;
                }
                UInt32 NumberOfPoints = _br.ReadUInt32();
                if (Compressed)
                {
                    DoCompressed(NumberOfPoints, _br, b);
                }
                else
                {
                    DoFloat(NumberOfPoints, _br, b);
                }
                return items;
            }
            finally
            {
                if (_br != null)
                    _br.Close();
                if (_ms != null)
                    _ms.Dispose();
                if (_fr != null)
                    _fr.Close();
                if (_fs != null)
                    _fs.Dispose();
            }
        }

        private void DoFloat(UInt32 NumberOfPoints, BinaryReader _br, Brush b)
        {
            PointF[] Ps = new PointF[NumberOfPoints];
            for (int i = 0; i < NumberOfPoints; i++)
            {
                  
                Ps[i].X = (float)(X +_br.ReadSingle() * SCALEFACTOR);
                Ps[i].Y = (float)(Y + _br.ReadSingle() * SCALEFACTOR);  
            }
            DoInstructions(Ps, b);
        }

        private void DoCompressed(UInt32 NumberOfPoints, BinaryReader _br, Brush b)
        {
            PointF[] Ps = new PointF[NumberOfPoints];
            for (int i = 0; i < NumberOfPoints; i++)
            { 
                Int16 px = _br.ReadInt16();
                Int16 py = _br.ReadInt16();
                Ps[i].X = X + px * SCALEFACTOR;//X + px * SCALEFACTOR;
                Ps[i].Y = Y + py * SCALEFACTOR;//Y + py * SCALEFACTOR;
            } 
            DoInstructions(Ps, b);
        }

        private void DoInstructions(PointF[] Ps, Brush b)
        {
            PagePolygon pl = new PagePolygon();
            //pl.X = X * SCALEFACTOR;
            //pl.Y = Y * SCALEFACTOR;
            pl.Points = Ps;
            StyleInfo SI = new StyleInfo();           

            switch (b.GetType().Name)
            {
                case "SolidBrush":
                    System.Drawing.SolidBrush theBrush = (System.Drawing.SolidBrush)b;
                    SI.Color = theBrush.Color;
                    SI.BackgroundColor = theBrush.Color;
                    break;
                case "LinearGradientBrush":
                    System.Drawing.Drawing2D.LinearGradientBrush linBrush = (System.Drawing.Drawing2D.LinearGradientBrush)b;
                    SI.BackgroundGradientType = BackgroundGradientTypeEnum.LeftRight;
                    SI.BackgroundColor = linBrush.LinearColors[0];
                    SI.BackgroundGradientEndColor = linBrush.LinearColors[1];
                    break;
                case "HatchBrush":
                    System.Drawing.Drawing2D.HatchBrush hatBrush = (System.Drawing.Drawing2D.HatchBrush)b;
                    SI.BackgroundColor = hatBrush.BackgroundColor;
                    SI.Color = hatBrush.ForegroundColor;
                    SI.PatternType = StyleInfo.GetPatternType(hatBrush.HatchStyle);
                    break;
                default:
                    break;
            }

            pl.SI = SI;
            items.Add(pl);
        }
    }
}


