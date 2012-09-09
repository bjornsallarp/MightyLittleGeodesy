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

namespace MightyLittleGeodesy.Classes
{
    /*
     * .NET-implementation of "Gauss Conformal Projection 
     * (Transverse Mercator), Krügers Formulas".
     * - Parameters for SWEREF99 lat-long to/from RT90 and SWEREF99 
     * coordinates (RT90 and SWEREF99 are used in Swedish maps).
     * 
     * The calculations are based entirely on the excellent
     * javscript library by Arnold Andreassons.
     * Source: http://www.lantmateriet.se/geodesi/
     * Source: Arnold Andreasson, 2007. http://mellifica.se/konsult
     * Author: Björn Sållarp. 2009. http://blog.sallarp.com
     */
    public class GaussKreuger
    {
        double axis; // Semi-major axis of the ellipsoid.
        double flattening; // Flattening of the ellipsoid.
        double central_meridian; // Central meridian for the projection.    
        double scale; // Scale on central meridian.
        double false_northing; // Offset for origo.
        double false_easting; // Offset for origo.

        // Parameters for RT90 and SWEREF99TM.
        // Note: Parameters for RT90 are choosen to eliminate the 
        // differences between Bessel and GRS80-ellipsoides.
        // Bessel-variants should only be used if lat/long are given as
        // RT90-lat/long based on the Bessel ellipsoide (from old maps).
        // Parameter: projection (string). Must match if-statement.
        public void swedish_params(string projection)
        {
            // RT90 parameters, GRS 80 ellipsoid.
            if (projection == "rt90_7.5_gon_v")
            {
                grs80_params();
                central_meridian = 11.0 + 18.375 / 60.0;
                scale = 1.000006000000;
                false_northing = -667.282;
                false_easting = 1500025.141;
            }
            else if (projection == "rt90_5.0_gon_v")
            {
                grs80_params();
                central_meridian = 13.0 + 33.376 / 60.0;
                scale = 1.000005800000;
                false_northing = -667.130;
                false_easting = 1500044.695;
            }
            else if (projection == "rt90_2.5_gon_v")
            {
                grs80_params();
                central_meridian = 15.0 + 48.0 / 60.0 + 22.624306 / 3600.0;
                scale = 1.00000561024;
                false_northing = -667.711;
                false_easting = 1500064.274;
            }
            else if (projection == "rt90_0.0_gon_v")
            {
                grs80_params();
                central_meridian = 18.0 + 3.378 / 60.0;
                scale = 1.000005400000;
                false_northing = -668.844;
                false_easting = 1500083.521;
            }
            else if (projection == "rt90_2.5_gon_o")
            {
                grs80_params();
                central_meridian = 20.0 + 18.379 / 60.0;
                scale = 1.000005200000;
                false_northing = -670.706;
                false_easting = 1500102.765;
            }
            else if (projection == "rt90_5.0_gon_o")
            {
                grs80_params();
                central_meridian = 22.0 + 33.380 / 60.0;
                scale = 1.000004900000;
                false_northing = -672.557;
                false_easting = 1500121.846;
            }

            // RT90 parameters, Bessel 1841 ellipsoid.
            else if (projection == "bessel_rt90_7.5_gon_v")
            {
                bessel_params();
                central_meridian = 11.0 + 18.0 / 60.0 + 29.8 / 3600.0;
            }
            else if (projection == "bessel_rt90_5.0_gon_v")
            {
                bessel_params();
                central_meridian = 13.0 + 33.0 / 60.0 + 29.8 / 3600.0;
            }
            else if (projection == "bessel_rt90_2.5_gon_v")
            {
                bessel_params();
                central_meridian = 15.0 + 48.0 / 60.0 + 29.8 / 3600.0;
            }
            else if (projection == "bessel_rt90_0.0_gon_v")
            {
                bessel_params();
                central_meridian = 18.0 + 3.0 / 60.0 + 29.8 / 3600.0;
            }
            else if (projection == "bessel_rt90_2.5_gon_o")
            {
                bessel_params();
                central_meridian = 20.0 + 18.0 / 60.0 + 29.8 / 3600.0;
            }
            else if (projection == "bessel_rt90_5.0_gon_o")
            {
                bessel_params();
                central_meridian = 22.0 + 33.0 / 60.0 + 29.8 / 3600.0;
            }

            // SWEREF99TM and SWEREF99ddmm  parameters.
            else if (projection == "sweref_99_tm")
            {
                sweref99_params();
                central_meridian = 15.00;
                scale = 0.9996;
                false_northing = 0.0;
                false_easting = 500000.0;
            }
            else if (projection == "sweref_99_1200")
            {
                sweref99_params();
                central_meridian = 12.00;
            }
            else if (projection == "sweref_99_1330")
            {
                sweref99_params();
                central_meridian = 13.50;
            }
            else if (projection == "sweref_99_1500")
            {
                sweref99_params();
                central_meridian = 15.00;
            }
            else if (projection == "sweref_99_1630")
            {
                sweref99_params();
                central_meridian = 16.50;
            }
            else if (projection == "sweref_99_1800")
            {
                sweref99_params();
                central_meridian = 18.00;
            }
            else if (projection == "sweref_99_1415")
            {
                sweref99_params();
                central_meridian = 14.25;
            }
            else if (projection == "sweref_99_1545")
            {
                sweref99_params();
                central_meridian = 15.75;
            }
            else if (projection == "sweref_99_1715")
            {
                sweref99_params();
                central_meridian = 17.25;
            }
            else if (projection == "sweref_99_1845")
            {
                sweref99_params();
                central_meridian = 18.75;
            }
            else if (projection == "sweref_99_2015")
            {
                sweref99_params();
                central_meridian = 20.25;
            }
            else if (projection == "sweref_99_2145")
            {
                sweref99_params();
                central_meridian = 21.75;
            }
            else if (projection == "sweref_99_2315")
            {
                sweref99_params();
                central_meridian = 23.25;
            }
            else
            {
                central_meridian = double.MinValue;
            }
        }

        // Sets of default parameters.
        private void grs80_params()
        {
            axis = 6378137.0; // GRS 80.
            flattening = 1.0 / 298.257222101; // GRS 80.
            central_meridian = double.MinValue;
        }
        private void bessel_params()
        {
            axis = 6377397.155; // Bessel 1841.
            flattening = 1.0 / 299.1528128; // Bessel 1841.
            central_meridian = double.MinValue;
            scale = 1.0;
            false_northing = 0.0;
            false_easting = 1500000.0;
        }
        private void sweref99_params()
        {
            axis = 6378137.0; // GRS 80.
            flattening = 1.0 / 298.257222101; // GRS 80.
            central_meridian = double.MinValue;
            scale = 1.0;
            false_northing = 0.0;
            false_easting = 150000.0;
        }

        // Conversion from geodetic coordinates to grid coordinates.
        public double[] geodetic_to_grid(double latitude, double longitude)
        {
            double[] x_y = new double[2];

            // Prepare ellipsoid-based stuff.
            double e2 = flattening * (2.0 - flattening);
            double n = flattening / (2.0 - flattening);
            double a_roof = axis / (1.0 + n) * (1.0 + n * n / 4.0 + n * n * n * n / 64.0);
            double A = e2;
            double B = (5.0 * e2 * e2 - e2 * e2 * e2) / 6.0;
            double C = (104.0 * e2 * e2 * e2 - 45.0 * e2 * e2 * e2 * e2) / 120.0;
            double D = (1237.0 * e2 * e2 * e2 * e2) / 1260.0;
            double beta1 = n / 2.0 - 2.0 * n * n / 3.0 + 5.0 * n * n * n / 16.0 + 41.0 * n * n * n * n / 180.0;
            double beta2 = 13.0 * n * n / 48.0 - 3.0 * n * n * n / 5.0 + 557.0 * n * n * n * n / 1440.0;
            double beta3 = 61.0 * n * n * n / 240.0 - 103.0 * n * n * n * n / 140.0;
            double beta4 = 49561.0 * n * n * n * n / 161280.0;

            // Convert.
            double deg_to_rad = Math.PI / 180.0;
            double phi = latitude * deg_to_rad;
            double lambda = longitude * deg_to_rad;
            double lambda_zero = central_meridian * deg_to_rad;

            double phi_star = phi - Math.Sin(phi) * Math.Cos(phi) * (A +
                            B * Math.Pow(Math.Sin(phi), 2) +
                            C * Math.Pow(Math.Sin(phi), 4) +
                            D * Math.Pow(Math.Sin(phi), 6));
            double delta_lambda = lambda - lambda_zero;
            double xi_prim = Math.Atan(Math.Tan(phi_star) / Math.Cos(delta_lambda));
            double eta_prim = math_atanh(Math.Cos(phi_star) * Math.Sin(delta_lambda));
            double x = scale * a_roof * (xi_prim +
                            beta1 * Math.Sin(2.0 * xi_prim) * math_cosh(2.0 * eta_prim) +
                            beta2 * Math.Sin(4.0 * xi_prim) * math_cosh(4.0 * eta_prim) +
                            beta3 * Math.Sin(6.0 * xi_prim) * math_cosh(6.0 * eta_prim) +
                            beta4 * Math.Sin(8.0 * xi_prim) * math_cosh(8.0 * eta_prim)) +
                            false_northing;
            double y = scale * a_roof * (eta_prim +
                            beta1 * Math.Cos(2.0 * xi_prim) * math_sinh(2.0 * eta_prim) +
                            beta2 * Math.Cos(4.0 * xi_prim) * math_sinh(4.0 * eta_prim) +
                            beta3 * Math.Cos(6.0 * xi_prim) * math_sinh(6.0 * eta_prim) +
                            beta4 * Math.Cos(8.0 * xi_prim) * math_sinh(8.0 * eta_prim)) +
                            false_easting;
            x_y[0] = Math.Round(x * 1000.0) / 1000.0;
            x_y[1] = Math.Round(y * 1000.0) / 1000.0;

            return x_y;
        }

        // Conversion from grid coordinates to geodetic coordinates.
        public double[] grid_to_geodetic(double x, double y)
        {
            double[] lat_lon = new double[2];
            if (central_meridian == double.MinValue)
            {
                return lat_lon;
            }
            // Prepare ellipsoid-based stuff.
            double e2 = flattening * (2.0 - flattening);
            double n = flattening / (2.0 - flattening);
            double a_roof = axis / (1.0 + n) * (1.0 + n * n / 4.0 + n * n * n * n / 64.0);
            double delta1 = n / 2.0 - 2.0 * n * n / 3.0 + 37.0 * n * n * n / 96.0 - n * n * n * n / 360.0;
            double delta2 = n * n / 48.0 + n * n * n / 15.0 - 437.0 * n * n * n * n / 1440.0;
            double delta3 = 17.0 * n * n * n / 480.0 - 37 * n * n * n * n / 840.0;
            double delta4 = 4397.0 * n * n * n * n / 161280.0;

            double Astar = e2 + e2 * e2 + e2 * e2 * e2 + e2 * e2 * e2 * e2;
            double Bstar = -(7.0 * e2 * e2 + 17.0 * e2 * e2 * e2 + 30.0 * e2 * e2 * e2 * e2) / 6.0;
            double Cstar = (224.0 * e2 * e2 * e2 + 889.0 * e2 * e2 * e2 * e2) / 120.0;
            double Dstar = -(4279.0 * e2 * e2 * e2 * e2) / 1260.0;

            // Convert.
            double deg_to_rad = Math.PI / 180;
            double lambda_zero = central_meridian * deg_to_rad;
            double xi = (x - false_northing) / (scale * a_roof);
            double eta = (y - false_easting) / (scale * a_roof);
            double xi_prim = xi -
                            delta1 * Math.Sin(2.0 * xi) * math_cosh(2.0 * eta) -
                            delta2 * Math.Sin(4.0 * xi) * math_cosh(4.0 * eta) -
                            delta3 * Math.Sin(6.0 * xi) * math_cosh(6.0 * eta) -
                            delta4 * Math.Sin(8.0 * xi) * math_cosh(8.0 * eta);
            double eta_prim = eta -
                            delta1 * Math.Cos(2.0 * xi) * math_sinh(2.0 * eta) -
                            delta2 * Math.Cos(4.0 * xi) * math_sinh(4.0 * eta) -
                            delta3 * Math.Cos(6.0 * xi) * math_sinh(6.0 * eta) -
                            delta4 * Math.Cos(8.0 * xi) * math_sinh(8.0 * eta);
            double phi_star = Math.Asin(Math.Sin(xi_prim) / math_cosh(eta_prim));
            double delta_lambda = Math.Atan(math_sinh(eta_prim) / Math.Cos(xi_prim));
            double lon_radian = lambda_zero + delta_lambda;
            double lat_radian = phi_star + Math.Sin(phi_star) * Math.Cos(phi_star) *
                            (Astar +
                             Bstar * Math.Pow(Math.Sin(phi_star), 2) +
                             Cstar * Math.Pow(Math.Sin(phi_star), 4) +
                             Dstar * Math.Pow(Math.Sin(phi_star), 6));
            lat_lon[0] = lat_radian * 180.0 / Math.PI;
            lat_lon[1] = lon_radian * 180.0 / Math.PI;
            return lat_lon;
        }


        private double math_sinh(double value) {
            return 0.5 * (Math.Exp(value) - Math.Exp(-value));
        }
        private double math_cosh(double value) {
            return 0.5 * (Math.Exp(value) + Math.Exp(-value));
        }
        private double math_atanh(double value) {
            return 0.5 * Math.Log((1.0 + value) / (1.0 - value));
        }

    }



}
