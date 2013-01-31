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

namespace ReportingCloud.Engine
{
     internal class EMFSetPageTransform
    {
        internal System.Drawing.GraphicsUnit PageUnit;
        internal bool postMultiplyTransform;
        internal Single PageScale;
        internal static EMFSetPageTransform getTransform(int flags, byte[] RecordData)
        {
            return new EMFSetPageTransform(flags, RecordData);
        }

        internal EMFSetPageTransform(int flags, byte[] RecordData)
        {
            MemoryStream _fs = null;
            BinaryReader _fr = null;
            try
            {
                _fs = new MemoryStream(BitConverter.GetBytes(flags));
                _fr = new BinaryReader(_fs);

                //PageUnit...
                UInt16 PageU = _fr.ReadByte();
                PageUnit = (System.Drawing.GraphicsUnit)PageU;

                UInt16 RealFlags = _fr.ReadByte();
                //XXXXXAXX - if A = 1 the transform matrix is post-multiplied else pre-multiplied...
                //01234567
                postMultiplyTransform = ((RealFlags & (UInt16)Math.Pow(2, 5)) == Math.Pow(2, 5));
                PageScale = BitConverter.ToSingle(RecordData, 0);
                
            }
            finally
           {
               if (_fr != null)
                   _fr.Close();
               if (_fs != null)
                   _fs.Dispose();
               
           }
        }
    }
}
