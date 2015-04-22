namespace MightyLittleGeodesyTests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using MightyLittleGeodesy.Positions;

    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void RT90ToWGS84()
        {
            RT90Position position = new RT90Position(6583052, 1627548);
            WGS84Position wgsPos = position.ToWGS84();

            // Values from Hitta.se for the conversion
            double latFromHitta = 59.3489;
            double lonFromHitta = 18.0473;

            double lat = Math.Round(wgsPos.Latitude, 4);
            double lon = Math.Round(wgsPos.Longitude, 4);

            Assert.AreEqual(latFromHitta, lat);
            Assert.AreEqual(lonFromHitta, lon);

            // String values from Lantmateriet.se, they convert DMS only.
            // Reference: http://www.lantmateriet.se/templates/LMV_Enkelkoordinattransformation.aspx?id=11500
            string latDmsStringFromLM = "N 59º 20' 56.09287\"";
            string lonDmsStringFromLM = "E 18º 2' 50.34806\"";

            Assert.AreEqual(latDmsStringFromLM, wgsPos.LatitudeToString(WGS84Position.WGS84Format.DegreesMinutesSeconds));
            Assert.AreEqual(lonDmsStringFromLM, wgsPos.LongitudeToString(WGS84Position.WGS84Format.DegreesMinutesSeconds));
        }

        [TestMethod]
        public void WGS84ToRT90()
        {
            WGS84Position wgsPos = new WGS84Position("N 59º 58' 55.23\" E 017º 50' 06.12\"", WGS84Position.WGS84Format.DegreesMinutesSeconds);
            RT90Position rtPos = new RT90Position(wgsPos, RT90Position.RT90Projection.rt90_2_5_gon_v);

            // Conversion values from Lantmateriet.se, they convert from DMS only.
            // Reference: http://www.lantmateriet.se/templates/LMV_Enkelkoordinattransformation.aspx?id=11500
            double xPosFromLM = 6653174.343;
            double yPosFromLM = 1613318.742;

            Assert.AreEqual(Math.Round(rtPos.Latitude, 3), xPosFromLM);
            Assert.AreEqual(Math.Round(rtPos.Longitude, 3), yPosFromLM);
        }

        [TestMethod]
        public void WGS84ToSweref()
        {
            WGS84Position wgsPos = new WGS84Position();

            wgsPos.SetLatitudeFromString("N 59º 58' 55.23\"", WGS84Position.WGS84Format.DegreesMinutesSeconds);
            wgsPos.SetLongitudeFromString("E 017º 50' 06.12\"", WGS84Position.WGS84Format.DegreesMinutesSeconds);

            SWEREF99Position rtPos = new SWEREF99Position(wgsPos, SWEREF99Position.SWEREFProjection.sweref_99_tm);

            // Conversion values from Lantmateriet.se, they convert from DMS only.
            // Reference: http://www.lantmateriet.se/templates/LMV_Enkelkoordinattransformation.aspx?id=11500
            double xPosFromLM = 6652797.165;
            double yPosFromLM = 658185.201;

            Assert.AreEqual(Math.Round(rtPos.Latitude, 3), xPosFromLM);
            Assert.AreEqual(Math.Round(rtPos.Longitude, 3), yPosFromLM);
        }

        [TestMethod]
        public void SwerefToWGS84()
        {
            SWEREF99Position swePos = new SWEREF99Position(6652797.165, 658185.201);
            WGS84Position wgsPos = swePos.ToWGS84();

            // String values from Lantmateriet.se, they convert DMS only.
            // Reference: http://www.lantmateriet.se/templates/LMV_Enkelkoordinattransformation.aspx?id=11500
            string latDmsStringFromLM = "N 59º 58' 55.23001\"";
            string lonDmsStringFromLM = "E 17º 50' 6.11997\"";

            Assert.AreEqual(latDmsStringFromLM, wgsPos.LatitudeToString(WGS84Position.WGS84Format.DegreesMinutesSeconds));
            Assert.AreEqual(lonDmsStringFromLM, wgsPos.LongitudeToString(WGS84Position.WGS84Format.DegreesMinutesSeconds));
        }

        [TestMethod]
        public void WGS84ParseString()
        {
            // Values from Eniro.se
            WGS84Position wgsPosDM = new WGS84Position("N 62º 10.560' E 015º 54.180'", WGS84Position.WGS84Format.DegreesMinutes);
            WGS84Position wgsPosDMs = new WGS84Position("N 62º 10' 33.60\" E 015º 54' 10.80\"", WGS84Position.WGS84Format.DegreesMinutesSeconds);

            Assert.AreEqual(62.176, Math.Round(wgsPosDM.Latitude, 3));
            Assert.AreEqual(15.903, Math.Round(wgsPosDM.Longitude, 3));

            Assert.AreEqual(62.176, Math.Round(wgsPosDMs.Latitude, 3));
            Assert.AreEqual(15.903, Math.Round(wgsPosDMs.Longitude, 3));
        }
    }
}
