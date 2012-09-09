MightyLittleGeodesy
===================
Translate coordinates between RT90, WGS84 and SWEREF99 using .NET

The calculations in this library is based on the [excellent javascript library by Arnold Andreasson](http://latlong.mellifica.se/ "javascript library by Arnold Andreasson") which is published under the [Creative Commons license](http://creativecommons.org/licenses/by-nc-sa/3.0/). However, as agreed with mr Andreasson, MightyLittleGeodesy is now licensed under the MIT license.

Using the library
=================

**Here’s an example of how to translate from RT90 to WGS84**

    RT90Position position = new RT90Position(6583052, 1627548);
    WGS84Position wgsPos = position.ToWGS84();

**From WGS84 to SWEREF99**

    WGS84Position wgsPos = new WGS84Position();
    wgsPos.SetLatitudeFromString("N 59º 58' 55.23\"", WGS84Position.WGS84Format.DegreesMinutesSeconds);
    wgsPos.SetLongitudeFromString("E 017º 50' 06.12\"", WGS84Position.WGS84Format.DegreesMinutesSeconds);

    SWEREF99Position rtPos = new SWEREF99Position(wgsPos, SWEREF99Position.SWEREFProjection.sweref_99_tm);

**From SWEREF99 to WGS84**

    SWEREF99Position swePos = new SWEREF99Position(6652797.165, 658185.201);
    WGS84Position wgsPos = swePos.ToWGS84();

**From WGS84 to RT90**

    WGS84Position wgsPos = new WGS84Position("N 59º 58' 55.23\" E 017º 50' 06.12\"", WGS84Position.WGS84Format.DegreesMinutesSeconds);
    RT90Position rtPos = new RT90Position(wgsPos, RT90Position.RT90Projection.rt90_2_5_gon_v);

**From RT90 to SWEREF99**

    RT90Position rt90Pos = new RT90Position(6583052, 1627548);
    SWEREF99Position sweRef = new SWEREF99Position(rt90Pos.ToWGS84(), SWEREF99Position.SWEREFProjection.sweref_99_tm);

You can translate both ways between all three coordinate systems as both RT90 and SWEREF99 can be translated to WGS84 and WGS84 can be translated into both RT90 and SWEREF99. The WGS84 object can parse out positions from strings in DMS (degrees, minutes seconds, and DM (degrees, minutes) format. All in all it’s a pretty easy to use and powerful library. I’ve added some unit-tests to the project which verifies the calculations against other sources (Lantmäteriet, Hitta and Eniro).


MightyLittleGeodesy is licensed under the MIT License
=====================================================

Copyright (C) 2009 Björn Sållarp

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
