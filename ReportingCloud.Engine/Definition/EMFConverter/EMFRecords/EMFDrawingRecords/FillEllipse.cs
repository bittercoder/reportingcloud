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

namespace ReportingCloud.Engine
{
    internal class FillEllipse : DrawBase
    {
        internal FillEllipse(Single Xin, Single Yin, Single WidthIn, Single HeightIn, System.Collections.Hashtable ObjectTableIn)
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
                //Byte 1 will be ignored
                byte RealFlags = _fr.ReadByte();
                //Byte 2 will be brushtype
                // 0 1 2 3 4 5 6 7
                // X X X X X X C S
                // if S = 1 brushID is an ARGBobject, else its an Index to object table
                // if C = 1 int16, 0 = Points are Float (ignore P)              
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
                if (Compressed)
                {
                    Int16 Xp = _br.ReadInt16();
                    Int16 Yp = _br.ReadInt16();
                    Int16 Wid = _br.ReadInt16();
                    Int16 Hgt = _br.ReadInt16();
                    DoInstructions(Xp, Yp, Wid, Hgt, b);
                }
                else
                {
                    Single Xp = _br.ReadSingle();
                    Single Yp = _br.ReadSingle();
                    Single Wid = _br.ReadSingle();
                    Single Hgt = _br.ReadSingle();
                    DoInstructions(Xp, Yp, Wid, Hgt, b);
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

        private void DoInstructions(Single Xp, Single Yp, Single Wid, Single Hgt, Brush b)
        {
            switch (b.GetType().Name)
            {
                case "SolidBrush":
                    System.Drawing.SolidBrush theBrush = (System.Drawing.SolidBrush)b;
                    Color col = theBrush.Color;  
                    PageEllipse pl = new PageEllipse();
                    pl.X = X + Xp * SCALEFACTOR;
                    pl.Y = Y + Yp * SCALEFACTOR;
                    pl.W = Wid * SCALEFACTOR;
                    pl.H = Hgt * SCALEFACTOR;

                    StyleInfo SI = new StyleInfo();                   
                    SI.BackgroundColor = col;
                    pl.SI = SI;
                    items.Add(pl);  



                    //Lines.AppendFormat("\r\n"); //CrLf
                    //Lines.AppendFormat("q\t"); //Push graphics state onto stack
                    //Lines.AppendFormat("{0} w\t", 1 * ScaleX); //set width of path 
                    //Lines.AppendFormat("{0} {1} {2} RG\t", Math.Round(theBrush.Color.R / 255.0, 3), Math.Round(theBrush.Color.G / 255.0, 3), Math.Round(theBrush.Color.B / 255.0, 3)); //Set RGB colours
                    //Lines.AppendFormat("{0} {1} {2} rg\t", Math.Round(theBrush.Color.R / 255.0, 3), Math.Round(theBrush.Color.G / 255.0, 3), Math.Round(theBrush.Color.B / 255.0, 3)); //Set RGB colours
                    ////Need some bezier curves to  draw an ellipse.. we can't draw a circle, but we can get close.
                    //Double k = 0.5522847498;
                    //Double RadiusX = (Wid / 2.0) * ScaleX;
                    //Double RadiusY = (Hgt / 2.0) * ScaleY;
                    //Double Y4 = Y + Height - Yp * ScaleY;
                    //Double X1 = Xp * ScaleX + X;
                    //Double Y1 = Y4 - RadiusY;
                    //Double X4 = X1 + RadiusX;
                    //Lines.AppendFormat("{0} {1} m\t", X1, Y1);//FirstPoint..
                    //Double kRy = k * RadiusY;
                    //Double kRx = k * RadiusX;
                    ////Control Point 1 will be on the same X as point 1 and be -kRy Y
                    //Double X2 = X1;
                    //Double Y2 = Y1 + kRy;
                    //Double X3 = X4 - kRx;
                    //Double Y3 = Y4;
                    //Lines.AppendFormat("{0} {1} {2} {3} {4} {5}  c\t", X2, Y2, X3, Y3, X4, Y4); //Upper Left Quadrant
                    //X1 += 2 * RadiusX;
                    //X2 = X1;
                    //X3 += 2 * kRx;
                    //Lines.AppendFormat("{0} {1} {2} {3} {4} {5}  c\t", X3, Y3, X2, Y2, X1, Y1); //Upper Right Quadrant
                    //Y2 -= 2 * kRy;
                    //Y3 -= 2 * RadiusY;
                    //Y4 = Y3;
                    //Lines.AppendFormat("{0} {1} {2} {3} {4} {5}  c\t", X2, Y2, X3, Y3, X4, Y4); //Lower Right Quadrant
                    //X1 -= 2 * RadiusX;
                    //X2 = X1;
                    //X3 -= 2 * kRx;
                    //Lines.AppendFormat("{0} {1} {2} {3} {4} {5}  c\t", X3, Y3, X2, Y2, X1, Y1); //Lower Right Quadrant
                    //Lines.AppendFormat("f\t");//fill path
                    //Lines.AppendFormat("Q\t");//Pop graphics state from stack 
                    break;
            }
        }
    }
}
