/*
 * MightyLittleGeodesy 
 * RT90, SWEREF99 and WGS84 coordinate transformation library
 * 
 * Read my blog @ http://blog.sallarp.com
 * 
 * 
 * Copyright (C) 2009 Björn Sållarp
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this 
 * software and associated documentation files (the "Software"), to deal in the Software 
 * without restriction, including without limitation the rights to use, copy, modify, 
 * merge, publish, distribute, sublicense, and/or sell copies of the Software, and to 
 * permit persons to whom the Software is furnished to do so, subject to the following 
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or 
 * substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING 
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace MightyLittleGeodesy.Positions
{
    using System;
    using System.Globalization;

    using MightyLittleGeodesy.Classes;

    public class WGS84Position : Position
    {
        public enum WGS84Format
        {
            Degrees = 0,
            DegreesMinutes = 1,
            DegreesMinutesSeconds = 2
        }

        /// <summary>
        /// Create a new WGS84 position with empty coordinates
        /// </summary>
        public WGS84Position() : base(Grid.WGS84)
        {
        }

        /// <summary>
        /// Create a new WGS84 position with latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public WGS84Position(double latitude, double longitude)
            : base(Grid.WGS84)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Create a new WGS84 position from a string containing both
        /// latitude and longitude. The string is parsed based on the 
        /// supplied format.
        /// </summary>
        /// <param name="positionString"></param>
        /// <param name="format"></param>
        public WGS84Position(string positionString, WGS84Format format)
            : base(Grid.WGS84)
        {
            if (format == WGS84Format.Degrees)
            {
                positionString = positionString.Trim();
                string[] latLon = positionString.Split(' ');
                if (latLon.Length == 2)
                {
                    Latitude = double.Parse(latLon[0], CultureInfo.InvariantCulture);
                    Longitude = double.Parse(latLon[1], CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new FormatException("The position string is invalid");
                }
            }
            else if (format == WGS84Format.DegreesMinutes || format == WGS84Format.DegreesMinutesSeconds)
            {
                int firstValueEndPos = 0;
                
                switch (format)
                {
                    case WGS84Format.DegreesMinutes:
                        firstValueEndPos = positionString.IndexOf("'", StringComparison.Ordinal);
                        break;
                    case WGS84Format.DegreesMinutesSeconds:
                        firstValueEndPos = positionString.IndexOf("\"", StringComparison.Ordinal);
                        break;
                }

                string lat = positionString.Substring(0, firstValueEndPos + 1).Trim();
                string lon = positionString.Substring(firstValueEndPos + 1).Trim();

                SetLatitudeFromString(lat, format);
                SetLongitudeFromString(lon, format);
            }
        }

        /// <summary>
        /// Set the latitude value from a string. The string is
        /// parsed based on given format
        /// </summary>
        /// <param name="value">String represenation of a latitude value</param>
        /// <param name="format">Coordinate format in the string</param>
        public void SetLatitudeFromString(string value, WGS84Format format)
        {
            value = value.Trim();

            switch (format)
            {
                case WGS84Format.DegreesMinutes:
                    this.Latitude = this.ParseValueFromDmString(value, 'S');
                    break;
                case WGS84Format.DegreesMinutesSeconds:
                    this.Latitude = this.ParseValueFromDmsString(value, 'S');
                    break;
                case WGS84Format.Degrees:
                    this.Latitude = double.Parse(value, CultureInfo.InvariantCulture);
                    break;
            }
        }

        /// <summary>
        /// Set the longitude value from a string. The string is
        /// parsed based on given format
        /// </summary>
        /// <param name="value">String represenation of a longitude value</param>
        /// <param name="format">Coordinate format in the string</param>
        public void SetLongitudeFromString(string value, WGS84Format format)
        {
            switch (format)
            {
                case WGS84Format.DegreesMinutes:
                    this.Longitude = this.ParseValueFromDmString(value, 'W');
                    break;
                case WGS84Format.DegreesMinutesSeconds:
                    this.Longitude = this.ParseValueFromDmsString(value, 'W');
                    break;
                case WGS84Format.Degrees:
                    this.Longitude = double.Parse(value, CultureInfo.InvariantCulture);
                    break;
            }
        }

        /// <summary>
        /// Returns a string representation in the given format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string LatitudeToString(WGS84Format format)
        {
            switch (format)
            {
                case WGS84Format.DegreesMinutes:
                    return this.ConvToDmString(this.Latitude, 'N', 'S');
                case WGS84Format.DegreesMinutesSeconds:
                    return this.ConvToDmsString(this.Latitude, 'N', 'S');
                default:
                    return this.Latitude.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Returns a string representation in the given format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string LongitudeToString(WGS84Format format)
        {
            switch (format)
            {
                case WGS84Format.DegreesMinutes:
                    return this.ConvToDmString(this.Longitude, 'E', 'W');
                case WGS84Format.DegreesMinutesSeconds:
                    return this.ConvToDmsString(this.Longitude, 'E', 'W');
                default:
                    return this.Longitude.ToString(CultureInfo.InvariantCulture);
            }
        }

        private string ConvToDmString(double value, Char positiveValue, Char negativeValue)
        {
            if (value == double.MinValue)
            {
                return "";
            }

            var degrees = Math.Floor(Math.Abs(value));
            var minutes = (Math.Abs(value) - degrees) * 60;

            return string.Format(
                "{0} {1}º {2}'",
                value >= 0 ? positiveValue : negativeValue,
                degrees,
                (Math.Floor(minutes * 10000) / 10000).ToString(CultureInfo.InvariantCulture));
        }

        private string ConvToDmsString(double value, Char positiveValue, Char negativeValue)
        {
            if (value == double.MinValue)
            {
                return string.Empty;
            }

            var degrees = Math.Floor(Math.Abs(value));
            var minutes = Math.Floor((Math.Abs(value) - degrees) * 60);
            var seconds = (Math.Abs(value) - degrees - minutes / 60) * 3600;

            return string.Format(
                "{0} {1}º {2}' {3}\"",
                value >= 0 ? positiveValue : negativeValue,
                degrees,
                minutes,
                Math.Round(seconds, 5).ToString(CultureInfo.InvariantCulture));
        }


        private double ParseValueFromDmString(string value, char positiveChar)
        {
            if (string.IsNullOrEmpty(value))
            {
                return double.MinValue;
            }

            var direction = value[0];
            value = value.Substring(1).Trim();

            var index = value.IndexOf("º", StringComparison.Ordinal);
            var degree = value.Substring(0, index);
            value = value.Substring(index + 1).Trim();

            var minutes = value.Substring(0, value.IndexOf("'", StringComparison.Ordinal));

            double retVal = double.Parse(degree);
            retVal += double.Parse(minutes, CultureInfo.InvariantCulture) / 60;

            if (retVal > 90)
            {
               return double.MinValue;
            }

            if (direction == positiveChar || direction == '-')
            {
                retVal *= -1;
            }

            return retVal;
        }

        private double ParseValueFromDmsString(string value, char positiveChar)
        {
            if (string.IsNullOrEmpty(value))
            {
                return double.MinValue;
            }
            
            double retVal;

            var direction = value[0];
            value = value.Substring(1).Trim();

            var index = value.IndexOf("º", StringComparison.Ordinal);
            var degree = value.Substring(0, index);
            value = value.Substring(index + 1).Trim();

            index = value.IndexOf("'", StringComparison.Ordinal);
            var minutes = value.Substring(0, index);
            value = value.Substring(index + 1).Trim();

            var seconds = value.Substring(0, value.IndexOf("\"", StringComparison.Ordinal));

            retVal = double.Parse(degree);
            retVal += double.Parse(minutes) / 60;
            retVal += double.Parse(seconds, CultureInfo.InvariantCulture) / 3600;

            if (retVal > 90)
            {
                return double.MinValue;
            }

            if (direction == positiveChar || direction == '-')
            {
                retVal *= -1;
            }
            
            return retVal;
        }

        public override string ToString()
        {
            return string.Format(
                "Latitude: {0}  Longitude: {1}",
                LatitudeToString(WGS84Format.DegreesMinutesSeconds),
                LongitudeToString(WGS84Format.DegreesMinutesSeconds));
        }
    }
}