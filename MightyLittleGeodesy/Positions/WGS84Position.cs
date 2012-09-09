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

using System;
using MightyLittleGeodesy.Classes;
using System.Globalization;

namespace MightyLittleGeodesy.Positions
{
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
                string[] lat_lon = positionString.Split(' ');
                if (lat_lon.Length == 2)
                {
                    Latitude = double.Parse(lat_lon[0], CultureInfo.InvariantCulture);
                    Longitude = double.Parse(lat_lon[1], CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new FormatException("The position string is invalid");
                }
            }
            else if (format == WGS84Format.DegreesMinutes || format == WGS84Format.DegreesMinutesSeconds)
            {
                int firstValueEndPos = 0;
                
                if(format== WGS84Format.DegreesMinutes)
                    firstValueEndPos = positionString.IndexOf("'");
                else if(format == WGS84Format.DegreesMinutesSeconds)
                    firstValueEndPos = positionString.IndexOf("\"");

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

            if(format == WGS84Format.DegreesMinutes)
                Latitude = ParseValueFromDmString(value, "S");
            else if (format == WGS84Format.DegreesMinutesSeconds)
                Latitude = ParseValueFromDmsString(value, "S");
            else if(format == WGS84Format.Degrees)
            {
                Latitude = double.Parse(value);
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
            if (format == WGS84Format.DegreesMinutes)
                Longitude = ParseValueFromDmString(value, "W");
            else if (format == WGS84Format.DegreesMinutesSeconds)
                Longitude = ParseValueFromDmsString(value, "W");
            else if (format == WGS84Format.Degrees)
            {
                Longitude = double.Parse(value);
            }
        }

        /// <summary>
        /// Returns a string representation in the given format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string LatitudeToString(WGS84Format format)
        {
            if (format == WGS84Format.DegreesMinutes)
                return ConvToDmString(Latitude, 'N', 'S');
            else if (format == WGS84Format.DegreesMinutesSeconds)
                return ConvToDmsString(Latitude, 'N', 'S');
            else
                return Latitude.ToString();
        }

        /// <summary>
        /// Returns a string representation in the given format
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public string LongitudeToString(WGS84Format format)
        {
            if (format == WGS84Format.DegreesMinutes)
                return ConvToDmString(Longitude, 'E', 'W');
            else if (format == WGS84Format.DegreesMinutesSeconds)
                return ConvToDmsString(Longitude, 'E', 'W');
            else
                return Longitude.ToString();
        }

        private string ConvToDmString(double value, Char positiveValue, Char negativeValue)
        {
            if (value == double.MinValue)
            {
                return "";
            }

            var degrees = Math.Floor(Math.Abs(value));
            var minutes = (Math.Abs(value) - degrees) * 60;

            return string.Format("{0} {1}º {2}'", value >= 0 ? positiveValue : negativeValue, degrees, (Math.Floor(minutes * 10000) / 10000));
        }

        private string ConvToDmsString(double value, Char positiveValue, Char negativeValue)
        {
            if (value == double.MinValue)
            {
                return "";
            }

            var degrees = Math.Floor(Math.Abs(value));
            var minutes = Math.Floor((Math.Abs(value) - degrees) * 60);
            var seconds = (Math.Abs(value) - degrees - minutes / 60) * 3600;

            return string.Format("{0} {1}º {2}' {3}\"", value >= 0 ? positiveValue : negativeValue, degrees, minutes, Math.Round(seconds, 5));

        }


        private double ParseValueFromDmString(string value, string positiveChar)
        {
            double retVal = 0.0;
            if (!string.IsNullOrEmpty(value))
            {
                string direction = value[0].ToString();
                value = value.Substring(1).Trim();

                string degree = value.Substring(0, value.IndexOf("º"));
                value = value.Substring(value.IndexOf("º") + 1).Trim();

                string minutes = value.Substring(0, value.IndexOf("'"));
                value = value.Substring(value.IndexOf("'") + 1).Trim();

                retVal = double.Parse(degree);
                retVal += double.Parse(minutes, CultureInfo.InvariantCulture) / 60;

                if (retVal > 90)
                {
                    retVal = double.MinValue;
                }
                if (direction == positiveChar || direction == "-")
                {
                    retVal *= -1;
                }
            }
            else
            {
                retVal = double.MinValue;
            }
            return retVal;
        }

        private double ParseValueFromDmsString(string value, string positiveChar)
        {
            double retVal = 0.0;
            if (!string.IsNullOrEmpty(value) )
            {
                string direction = value[0].ToString();
                value = value.Substring(1).Trim();

                string degree = value.Substring(0, value.IndexOf("º"));
                value = value.Substring(value.IndexOf("º")+1).Trim();

                string minutes = value.Substring(0, value.IndexOf("'"));
                value = value.Substring(value.IndexOf("'")+1).Trim();

                string seconds = value.Substring(0, value.IndexOf("\""));


                retVal = double.Parse(degree);
                retVal += double.Parse(minutes) / 60;
                retVal += double.Parse(seconds, CultureInfo.InvariantCulture) / 3600;

                if (retVal > 90)
                {
                    retVal = double.MinValue;
                    return retVal;
                }
                if (direction == positiveChar || direction == "-")
                {
                    retVal *= -1;
                }
            }
            else
            {
                retVal = double.MinValue;
            }
            return retVal;
        }


        public override string ToString()
        {
            return string.Format("Latitude: {0}  Longitude: {1}", 
                LatitudeToString(WGS84Format.DegreesMinutesSeconds), LongitudeToString(WGS84Format.DegreesMinutesSeconds));
        }
    }
}