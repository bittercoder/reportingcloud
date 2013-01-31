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
using System.Collections;
using System.Collections.Generic;

namespace ReportingCloud.Engine
{
	/// <summary>
	/// Represents a list of the tokens.
	/// </summary>
	internal class TokenList : IEnumerable
	{
		private List<Token> tokens = null;

		internal TokenList()
		{
			tokens = new List<Token>();
		}

		internal void Add(Token token)
		{
			tokens.Add(token);
		}

		internal void Push(Token token)
		{
			tokens.Insert(0, token);
		}

		internal Token Peek()
		{
			return tokens[0];
		}

		internal Token Extract()
		{
			Token token = tokens[0];
			tokens.RemoveAt(0);
			return token;
		}

		internal int Count
		{
			get
			{
				return tokens.Count;
			}
		}

		public IEnumerator GetEnumerator()
		{
			return tokens.GetEnumerator();
		}
	}
}
