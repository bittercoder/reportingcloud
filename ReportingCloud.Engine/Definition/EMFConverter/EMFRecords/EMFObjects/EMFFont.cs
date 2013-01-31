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

namespace ReportingCloud.Engine
{
    class EMFFont : EMFRecordObject
    {
        public Font myFont;

        internal EMFFont()
        {
            ObjectType = EmfObjectType.font;
        }

        internal static EMFFont getEMFFont(byte[] RecordData)
        {
            return Process(RecordData);
        }

        private static EMFFont Process(byte[] RecordData)
        {
            //put the Data into a stream and use a binary reader to read the data
            MemoryStream _ms = null;
            BinaryReader _br = null;
            try
            {
                _ms = new MemoryStream(RecordData);
                _br = new BinaryReader(_ms);
                UInt32 Version = _br.ReadUInt32();
                Single EmSize = _br.ReadSingle();
                UInt32 SizeUnit = _br.ReadUInt32();
                Int32 FontStyleFlags = _br.ReadInt32();
                _br.ReadUInt32();
                UInt32 NameLength = _br.ReadUInt32();
                char[] FontFamily = new char[NameLength]; 
                System.Text.UnicodeEncoding d = new System.Text.UnicodeEncoding();
                d.GetChars(_br.ReadBytes((int)NameLength * 2),0,(int)NameLength * 2,FontFamily,0);                
                Font aFont = new Font(new String(FontFamily), EmSize, (FontStyle)FontStyleFlags, (GraphicsUnit)SizeUnit);
                EMFFont ThisFont = new EMFFont();
                ThisFont.myFont = aFont;
                return ThisFont;
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
