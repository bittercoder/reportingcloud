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
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;

namespace ReportingCloud.Engine
{
    internal enum EmfObjectType
    { 
        invalid = 0x00000000,
        brush = 0x00000001,
        pen = 0x00000002,
        path = 0x00000003,
        region = 0x00000004,
        image = 0x00000005,
        font = 0x00000006,
        stringformat = 0x00000007,
        ImageAttributes = 0x00000008,
        CustomLineType = 0x00000009
    }
    internal class EMFRecordObject
    {
        public byte ObjectID;
        public EmfObjectType ObjectType;

        internal static EMFRecordObject getObject(int flags, byte[] RecordData)
        {
            MemoryStream _ms = null;
            BinaryReader _br = null;
            try
            {
                //Put the Flags into a stream and then use a binary Reader to read the Flags
                _ms = new MemoryStream(BitConverter.GetBytes(flags));
                _br = new BinaryReader(_ms);
                //ObjectID is least significant byte (which will be the first byte in the byte array due to Little Endian)       
                byte Objectid = _br.ReadByte();
                //Object Type next...
                byte ObjectTyp = _br.ReadByte();
                //Don't know what to do if this object continues on the next one!
                bool ContinuesOnNextObject = ((ObjectTyp & 128) == 128);
                if (ContinuesOnNextObject)
                    ObjectTyp ^= 128;

                switch ((UInt16)ObjectTyp)
                {
                    case (UInt16)EmfObjectType.invalid: 
                        break;
                    case (UInt16)EmfObjectType.brush:
                        EMFBrush Obrush = EMFBrush.getEMFBrush(RecordData);
                        Obrush.ObjectID = Objectid;
                        return Obrush;
                    case (UInt16)EmfObjectType.pen:
                        EMFPen OPen = EMFPen.getEMFPen(RecordData);
                        OPen.ObjectID = Objectid;
                        return OPen;
                    case (UInt16)EmfObjectType.path:
                        break;
                    case (UInt16)EmfObjectType.region:
                        break;
                    case (UInt16)EmfObjectType.image:
                        break;
                    case (UInt16)EmfObjectType.font:
                        EMFFont OFont = EMFFont.getEMFFont(RecordData);
                        OFont.ObjectID = Objectid;
                        return OFont;                     
                    case (UInt16)EmfObjectType.stringformat:
                        EMFStringFormat Ostringformat = EMFStringFormat.getEMFStringFormat(RecordData);
                        Ostringformat.ObjectID = Objectid;
                        return Ostringformat;                       
                    case (UInt16)EmfObjectType.ImageAttributes:
                        break;
                    case (UInt16)EmfObjectType.CustomLineType:
                        break;
                }
                return null;
            } 
            catch (Exception e)            
            {               
                throw e;
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
