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
using System.Collections;
using System.Collections.Generic;
using ReportingCloud.Engine;
using System.IO;

namespace ReportingCloud.Engine
{
	
	///<summary>
	/// An implementation of IStreamGen.  Constructor is passed the name of a
	/// file.  The first file uses that name.  If subsequant files are needed 
	/// then a number suffix is generated sequentially.	 e.g. afile.html, afile2.gif,
	/// afile3.jpeg, ...
	///</summary>

	public class OneFileStreamGen : IStreamGen, IDisposable
	{
		string _Directory;
		StreamWriter _SW;
		Stream _io;
		string _FileName;
		List<string> _FileList;
		int _nextFileNumber=1;
		bool _Overwrite;

		public OneFileStreamGen(string filename, bool bOverwrite)
		{
			_Overwrite = bOverwrite;
			string ext = Path.GetExtension(filename).Substring(1);	// extension (without the '.')
			_Directory = Path.GetDirectoryName(filename);
			_FileName = Path.GetFileNameWithoutExtension(filename);

			_FileList = new List<string>();

			string relativeName;
			_io = GetIOStream(out relativeName, ext);
		}

		internal List<string> FileList
		{
			get { return _FileList; }
		}
		
		internal string FileName
		{
			get { return _FileName; }
		}

		#region IStreamGen Members
		public void CloseMainStream()
		{
			if (_SW != null)
			{
				_SW.Close();
				_SW = null;
			}
			if (_io != null)
			{
				_io.Close();
				_io = null;
			}
			return;
		}

		public Stream GetStream()
		{
			return this._io;
		}

		public TextWriter GetTextWriter()
		{
			if (_SW == null)
				_SW = new StreamWriter(_io);
			return _SW;
		}

		// create a new file in the directory specified and return
		//   a Stream caller can then write to.   relativeName is filled in with
		//   name we generate (sans the directory).
		public Stream GetIOStream(out string relativeName, string extension)
		{
			Stream io=null;

			// Obtain a new file name
			string filename = string.Format("{0}{1}{2}{3}.{4}",
				_Directory,						// directory
				Path.DirectorySeparatorChar,	// "\"
				_FileName,						// filename
				(this._nextFileNumber > 1? _nextFileNumber.ToString(): ""),		// suffix: first file doesn't need number suffix
				extension);						// extension
			_nextFileNumber++;			// increment to next file

			FileInfo fi = new FileInfo(filename);
			if (fi.Exists)
			{
				if (_Overwrite)
					fi.Delete();
				else
					throw new Exception(string.Format("File {0} already exists.", filename));
			}

			relativeName = Path.GetFileName(filename);
			io = fi.Create();
			_FileList.Add(filename);
			return io; 
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (_SW != null)
			{
				_SW.Flush();
				_SW.Close();
				_SW = null;
			}
			if (_io != null)
			{
				_io.Flush();
				_io.Close();
				_io = null;
			}
		}

		#endregion
	}
}
