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
    /// <summary>
    /// The Entity class holds a number of static functions for support entity functions.
    /// </summary>
    sealed internal class Entity
    {
        /// <summary>
        /// Obtains the entity object value from the given property name
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns>object value</returns>
        static public object GetObject(object entity, string propertyName)
        {
            if (entity == null)
                return null;
            return entity.GetType().GetProperty(propertyName).GetValue(entity, null);
        }

        /// <summary>
        /// Obtains the entity string value from the given property name
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns>string value</returns>
        static public string GetString(object entity, string propertyName)
        {
            object value = GetObject(entity, propertyName);
            if (value == null)
                return string.Empty;
            return value.ToString();
        }

        /// <summary>
        /// Obtains the entity integer value from the given property name
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns>integer value</returns>
        static public int GetInteger(object entity, string propertyName)
        {
            object value = GetObject(entity, propertyName);
            if (value == null)
                return 0;
            return (int)value;
        }
    
        /// <summary>
        /// Obtains the entity decimal value from the given property name
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns>decimal value</returns>
        static public decimal GetDecimal(object entity, string propertyName)
        {
            object value = GetObject(entity, propertyName);
            if (value == null)
                return 0;
            return (decimal)value;
        }
    
        /// <summary>
        /// Obtains the entity boolean value from the given property name
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns>boolean value</returns>
        static public bool GetBoolean(object entity, string propertyName)
        {
            object value = GetObject(entity, propertyName);
            if (value == null)
                return false;
            return (bool)value;
        }

        /// <summary>
        /// Obtains the entity date time value from the given property name
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <returns>date time value</returns>
        static public DateTime GetDateTime(object entity, string propertyName)
        {
            object value = GetObject(entity, propertyName);
            if (value == null)
                return new DateTime(0, 0, 0);
            return (DateTime)value;
        }    
    }
}