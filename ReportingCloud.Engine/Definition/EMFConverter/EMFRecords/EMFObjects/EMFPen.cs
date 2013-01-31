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
    [Flags]
    internal enum PenDataFlags
    {
        PenDataTransform = 0x00000001,
        PenDataStartCap = 0x00000002,
        PenDataEndCap = 0x00000004,
        PenDataJoin = 0x00000008,
        PenDataMiterLimit = 0x00000010,
        PenDataLineStyle =0x00000020,
        PenDataDashedLineCap =0x00000040,
        PenDataDashedLineOffset =0x00000080,
        PenDataDashedLine =0x00000100,
        PenDataNonCenter =0x00000200,
        PenDataCompoundLine =0x00000400,
        PenDataCustomStartCap =0x00000800,
        PenDataCustomEndCap =0x00001000,   
    }

    internal enum CustomLineCapDataType 
    {
        CustomLineCapDataTypeDefault = 0x00000000, 
        CustomLineCapDataTypeAdjustableArrow = 0x00000001 
    }


    internal class EMFPen : EMFRecordObject
    {
        public Pen myPen; 

        private EMFPen(byte[] RecordData)        
        {
            ObjectType = EmfObjectType.pen;            
            
            //put the Data into a stream and use a binary reader to read the data
            MemoryStream _ms = new MemoryStream(RecordData);
            BinaryReader _br = new BinaryReader(_ms);
            myPen = new Pen(Color.Black); //default just for now..
            UInt32 Version = _br.ReadUInt32();
            UInt32 Unknown = _br.ReadUInt32();
            UInt32 Flags = _br.ReadUInt32();
             _br.ReadUInt32(); //PenUnit NOT SURE...
            myPen.Width = _br.ReadSingle();           
            //Rest of data depends on Flags!
            byte[] Transform;          
            Single[] DashLengths;          
            Single[] CompoundLineData;           

            //For now this will be nowhere near fully implemented... Just getting what I need to get it to work!
            if ((Flags & (UInt32)PenDataFlags.PenDataTransform) == (UInt32)PenDataFlags.PenDataTransform)
            {
                //Read the next 24 bytes... A PenDataTransformObject
                Transform = _br.ReadBytes(24);
            }
            if ((Flags & (UInt32)PenDataFlags.PenDataStartCap) == (UInt32)PenDataFlags.PenDataStartCap)
            {
                myPen.StartCap = (System.Drawing.Drawing2D.LineCap)_br.ReadInt32();               
            }
            if ((Flags & (UInt32)PenDataFlags.PenDataEndCap) == (UInt32)PenDataFlags.PenDataEndCap)
            {
                myPen.EndCap = (System.Drawing.Drawing2D.LineCap)_br.ReadInt32();
            }
            if ((Flags & (UInt32)PenDataFlags.PenDataJoin) == (UInt32)PenDataFlags.PenDataJoin)
            {
                myPen.LineJoin = (System.Drawing.Drawing2D.LineJoin) _br.ReadUInt32();               
            }
            if ((Flags & (UInt32)PenDataFlags.PenDataMiterLimit) == (UInt32)PenDataFlags.PenDataMiterLimit)
            {
                myPen.MiterLimit = _br.ReadSingle();               
            }
            if ((Flags & (UInt32)PenDataFlags.PenDataLineStyle) == (UInt32)PenDataFlags.PenDataLineStyle)
            {
                myPen.DashStyle = (System.Drawing.Drawing2D.DashStyle) _br.ReadInt32();              
            }
            if ((Flags & (UInt32)PenDataFlags.PenDataDashedLineCap) == (UInt32)PenDataFlags.PenDataDashedLineCap)
            {
                myPen.DashCap = (System.Drawing.Drawing2D.DashCap) _br.ReadUInt32();             
            }
            if ((Flags & (UInt32)PenDataFlags.PenDataDashedLineOffset) == (UInt32)PenDataFlags.PenDataDashedLineOffset)
            {
                myPen.DashOffset = _br.ReadSingle();              
            }
            if ((Flags & (UInt32)PenDataFlags.PenDataDashedLine) == (UInt32)PenDataFlags.PenDataDashedLine)
            {
                //Woo-hoo... A Variable length field!...
                UInt32 DashedLineDataSize = _br.ReadUInt32();//Number of floats to read...
                DashLengths = new Single[DashedLineDataSize];
                for (int i = 0; i < DashedLineDataSize; i++)
                {
                    DashLengths[i] = _br.ReadSingle();
                }
                myPen.DashPattern = DashLengths;
            }

            if ((Flags & (UInt32)PenDataFlags.PenDataNonCenter) == (UInt32)PenDataFlags.PenDataNonCenter)
            {               
                myPen.Alignment = (System.Drawing.Drawing2D.PenAlignment) _br.ReadInt32();             
            }
            if ((Flags & (UInt32)PenDataFlags.PenDataCompoundLine) == (UInt32)PenDataFlags.PenDataCompoundLine)
            {
                //Joy...more variable length...
                UInt32 CompoundLineDataSize = _br.ReadUInt32();//Number of floats to read...
                CompoundLineData = new Single[CompoundLineDataSize];
                for (int i = 0; i < CompoundLineDataSize; i++)
                {
                    CompoundLineData[i] = _br.ReadSingle();
                }
                myPen.CompoundArray = CompoundLineData;
            }

            if ((Flags & (UInt32)PenDataFlags.PenDataCustomStartCap) == (UInt32)PenDataFlags.PenDataCustomStartCap)
            {
                //Again...variable length...Hope we don't get this one any time soon... I'm not implementing it!
                UInt32 CustomStartCapSize = _br.ReadUInt32();
                _br.ReadBytes((int)CustomStartCapSize);
            }
            if ((Flags & (UInt32)PenDataFlags.PenDataCustomEndCap) == (UInt32)PenDataFlags.PenDataCustomEndCap)
            {
                //Again...variable length...Hope we don't get this one any time soon... I'm not implementing it either!
                UInt32 CustomEndCapSize = _br.ReadUInt32();
                _br.ReadBytes((int)CustomEndCapSize);
            }
            //Ok - the rest of the bytes are going to be a brush object
            EMFBrush myBrush = EMFBrush.getEMFBrush(_br.ReadBytes((int)(_br.BaseStream.Length - _br.BaseStream.Position)));
            //Now we can make a pen for storage...           
            myPen.Brush = myBrush.myBrush; 
        }

        internal static EMFPen getEMFPen(byte[] RecordData)
        {
            return new EMFPen(RecordData);         
        }      
               
    }
}
