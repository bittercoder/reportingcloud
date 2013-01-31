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

namespace ReportingCloud.Engine
{
	///<summary>
	/// Handle the image size enumeration.  AutoSize, Fit, FitProportional, Clip
	///</summary>
	public enum ImageSizingEnum
	{
		/// <summary>
		/// The borders should grow/shrink to accommodate the image (Default).
		/// </summary>
		AutoSize,	
		/// <summary>
		/// The image is resized to exactly match the height and width of the image element.
		/// </summary>
		Fit,		
		/// <summary>
		/// The image should be resized to fit, preserving aspect ratio.
		/// </summary>
		FitProportional,	
		/// <summary>
		/// The image should be clipped to fit.		
		/// </summary>
		Clip		
	}
	/// <summary>
	/// Use ImageSizing when you want to take a string and map it to the ImageSizingEnum. 
	/// </summary>
	public class ImageSizing
	{
		/// <summary>
		/// Given a string return the cooresponding enumeration.
		/// </summary>
		/// <param name="s"></param>
		/// <returns>The enumerated value corresponding to the string.</returns>
		static public ImageSizingEnum GetStyle(string s)
		{
			return GetStyle(s, null);
		}

		static internal ImageSizingEnum GetStyle(string s, ReportLog rl)
			{
			ImageSizingEnum rs;

			switch (s)
			{		
				case "AutoSize":
					rs = ImageSizingEnum.AutoSize;
					break;
				case "Fit":
					rs = ImageSizingEnum.Fit;
					break;
				case "FitProportional":
					rs = ImageSizingEnum.FitProportional;
					break;
				case "Clip":
					rs = ImageSizingEnum.Clip;
					break;
				default:		
					if (rl != null)
						rl.LogError(4, "Unknown ImageSizing '" + s + "'.  AutoSize assumed.");

					rs = ImageSizingEnum.AutoSize;
					break;
			}
			return rs;
		}
	}

}
