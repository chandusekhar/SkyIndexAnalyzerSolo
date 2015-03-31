using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming

namespace SolarPositioning
{
    public class SPA
    {
        public SPAData spa = new SPAData();

        private const double PI = 3.1415926535897932384626433832795028841971;
        private const double SUN_RADIUS = 0.26667;
        private const int L_COUNT = 6;
        private const int B_COUNT = 2;
        private const int R_COUNT = 5;
        private const int Y_COUNT = 63;

        private const int L_MAX_SUBCOUNT = 64;
        private const int B_MAX_SUBCOUNT = 5;
        private const int R_MAX_SUBCOUNT = 40;

        private const int TERM_A = 0;
        private const int TERM_B = 1;
        private const int TERM_C = 2;
        private const int TERM_COUNT = 3;

        private const int TERM_X0 = 0;
        private const int TERM_X1 = 1;
        private const int TERM_X2 = 2;
        private const int TERM_X3 = 3;
        private const int TERM_X4 = 4;
        private const int TERM_X_COUNT = 5;

        private const int TERM_PSI_A = 0;
        private const int TERM_PSI_B = 1;
        private const int TERM_EPS_C = 2;
        private const int TERM_EPS_D = 3;
        private const int TERM_PE_COUNT = 3;

        private const int JD_MINUS = 0;
        private const int JD_ZERO = 1;
        private const int JD_PLUS = 2;
        private const int JD_COUNT = 3;

        private const int SUN_TRANSIT = 0;
        private const int SUN_RISE = 1;
        private const int SUN_SET = 2;
        private const int SUN_COUNT = 3;

        private static int TERM_Y_COUNT = TERM_X_COUNT;

        private static int[] l_subcount = { 64, 34, 20, 7, 3, 1 };
        private static int[] b_subcount = { 5, 2 };
        private static int[] r_subcount = { 40, 10, 6, 2, 1 };

        public static double[][][] L_TERMS = SPAConst.TERMS_L;
        public static double[][][] B_TERMS = SPAConst.TERMS_B;
        public static double[][][] R_TERMS = SPAConst.TERMS_R;
        public static double[][] Y_TERMS = SPAConst.TERMS_Y;
        public static double[][] PE_TERMS = SPAConst.TERMS_PE;


        public SPA(
            int spa_year,
            int spa_month,
            int spa_day,
            int spa_hour,
            int spa_minute,
            int spa_second,
            float longitude,
            float latitude,
            float delta_t = 68.0f,
            float timezone = 0.0f,
            float elevation = 0.0f,
            float pressure = 1010.0f,
            float temperature = 11.0f,
            float slope = 0.0f,
            float azm_rotation = 0.0f,
            float atmos_refract = 0.5667f,
            SPAFunctionType spa_function = SPAFunctionType.SPA_ALL)
        {
            spa.year = spa_year;            // 4-digit year,    valid range: -2000 to 6000, error code: 1
            spa.month = spa_month;           // 2-digit month,         valid range: 1 to 12, error code: 2
            spa.day = spa_day;             // 2-digit day,           valid range: 1 to 31, error code: 3
            spa.hour = spa_hour;            // Observer local hour,   valid range: 0 to 24, error code: 4
            spa.minute = spa_minute;          // Observer local minute, valid range: 0 to 59, error code: 5
            spa.second = spa_second;          // Observer local second, valid range: 0 to 59, error code: 6

            spa.delta_t = delta_t;       // Difference between earth rotation time and terrestrial time
            //     (from observation)
            // valid range: -8000 to 8000 seconds, error code: 7

            spa.timezone = timezone;      // Observer time zone (negative west of Greenwich)
            // valid range: -12   to   12 hours,   error code: 8

            spa.longitude = longitude;     // Observer longitude (negative west of Greenwich)
            // valid range: -180  to  180 degrees, error code: 9

            spa.latitude = latitude;      // Observer latitude (negative south of equator)
            // valid range: -90   to   90 degrees, error code: 10

            spa.elevation = elevation;     // Observer elevation [meters]
            // valid range: -6500000 or higher meters,    error code: 11

            spa.pressure = pressure;      // Annual average local pressure [millibars]
            // valid range:    0 to 5000 millibars,       error code: 12

            spa.temperature = temperature;   // Annual average local temperature [degrees Celsius]
            // valid range: -273 to 6000 degrees Celsius, error code; 13

            spa.slope = slope;         // Surface slope (measured from the horizontal plane)
            // valid range: -360 to 360 degrees, error code: 14

            spa.azm_rotation = azm_rotation;  // Surface azimuth rotation (measured from south to projection of
            //     surface normal on horizontal plane, negative west)
            // valid range: -360 to 360 degrees, error code: 15

            spa.atmos_refract = atmos_refract; // Atmospheric refraction at sunrise and sunset (0.5667 deg is typical)
            // valid range: -5   to   5 degrees, error code: 16

            spa.function = spa_function;        // Switch to choose functions for desired output (from enumeration)
        }




        private double rad2deg(double radians)
        {
            return (180.0 / PI) * radians;
        }

        private double deg2rad(double degrees)
        {
            return (PI / 180.0) * degrees;
        }


        private double limit_degrees(double degrees)
        {
            double limited;

            degrees /= 360.0;
            limited = 360.0 * (degrees - Math.Floor(degrees));
            if (limited < 0) limited += 360.0;

            return limited;
        }


        private double limit_degrees180pm(double degrees)
        {
            double limited;

            degrees /= 360.0;
            limited = 360.0 * (degrees - Math.Floor(degrees));
            if (limited < -180.0) limited += 360.0;
            else if (limited > 180.0) limited -= 360.0;

            return limited;
        }


        private double limit_degrees180(double degrees)
        {
            double limited;

            degrees /= 180.0;
            limited = 180.0 * (degrees - Math.Floor(degrees));
            if (limited < 0) limited += 180.0;

            return limited;
        }


        private double limit_zero2one(double value)
        {
            double limited;

            limited = value - Math.Floor(value);
            if (limited < 0) limited += 1.0;

            return limited;
        }


        private double limit_minutes(double minutes)
        {
            double limited = minutes;

            if (limited < -20.0) limited += 1440.0;
            else if (limited > 20.0) limited -= 1440.0;

            return limited;
        }


        private double dayfrac_to_local_hr(double dayfrac, float timezone)
        {
            return 24.0 * limit_zero2one(dayfrac + timezone / 24.0);
        }

        private double third_order_polynomial(double a, double b, double c, double d, double x)
        {
            return ((a * x + b) * x + c) * x + d;
        }


        private float fabs(float inVal)
        {
            return Math.Abs(inVal);
        }


        private float fabs(double inVal)
        {
            return Math.Abs((float)inVal);
        }



        private int validate_inputs(SPAData spa)
        {
            if ((spa.year < -2000) || (spa.year > 6000)) return 1;
            if ((spa.month < 1) || (spa.month > 12)) return 2;
            if ((spa.day < 1) || (spa.day > 31)) return 3;
            if ((spa.hour < 0) || (spa.hour > 24)) return 4;
            if ((spa.minute < 0) || (spa.minute > 59)) return 5;
            if ((spa.second < 0) || (spa.second > 59)) return 6;
            if ((spa.pressure < 0) || (spa.pressure > 5000)) return 12;
            if ((spa.temperature <= -273) || (spa.temperature > 6000)) return 13;
            if ((spa.hour == 24) && (spa.minute > 0)) return 5;
            if ((spa.hour == 24) && (spa.second > 0)) return 6;

            if (fabs(spa.delta_t) > 8000) return 7;
            if (fabs(spa.timezone) > 12) return 8;
            if (fabs(spa.longitude) > 180) return 9;
            if (fabs(spa.latitude) > 90) return 10;
            if (fabs(spa.atmos_refract) > 5) return 16;
            if (spa.elevation < -6500000) return 11;

            if ((spa.function == SPAFunctionType.SPA_ZA_INC) || (spa.function == SPAFunctionType.SPA_ALL))
            {
                if (fabs(spa.slope) > 360) return 14;
                if (fabs(spa.azm_rotation) > 360) return 15;
            }

            return 0;
        }





        private double julian_day(int year, int month, int day, int hour, int minute, int second, float tz)
        {
            double day_decimal, julian_day, a;

            day_decimal = day + (hour - tz + (minute + second / 60.0) / 60.0) / 24.0;

            if (month < 3)
            {
                month += 12;
                year--;
            }

            julian_day = Math.Floor(365.25 * (year + 4716.0)) + Math.Floor(30.6001 * (month + 1)) + day_decimal - 1524.5;

            if (julian_day > 2299160.0)
            {
                a = Math.Floor(year / 100.0);
                julian_day += (2 - a + Math.Floor(a / 4.0));
            }

            return julian_day;
        }




        private double julian_century(double jd)
        {
            return (jd - 2451545.0) / 36525.0;
        }


        private double julian_ephemeris_day(double jd, float delta_t)
        {
            return jd + delta_t / 86400.0;
        }

        private double julian_ephemeris_century(double jde)
        {
            return (jde - 2451545.0) / 36525.0;
        }

        private double julian_ephemeris_millennium(double jce)
        {
            return (jce / 10.0);
        }

        private double earth_periodic_term_summation(double[][] terms, int count, double jme)
        {
            int i;
            double sum = 0;

            for (i = 0; i < count; i++)
                sum += terms[i][TERM_A] * Math.Cos(terms[i][TERM_B] + terms[i][TERM_C] * jme);

            return sum;
        }

        private double earth_values(double[] term_sum, int count, double jme)
        {
            int i;
            double sum = 0;

            for (i = 0; i < count; i++)
                sum += term_sum[i] * Math.Pow(jme, i);

            sum /= 1.0e8;

            return sum;
        }

        private double earth_heliocentric_longitude(double jme)
        {
            double[] sum = new double[L_COUNT];
            int i;

            for (i = 0; i < L_COUNT; i++)
                sum[i] = earth_periodic_term_summation(L_TERMS[i], l_subcount[i], jme);

            return limit_degrees(rad2deg(earth_values(sum, L_COUNT, jme)));

        }

        private double earth_heliocentric_latitude(double jme)
        {
            double[] sum = new double[B_COUNT];
            int i;

            for (i = 0; i < B_COUNT; i++)
                sum[i] = earth_periodic_term_summation(B_TERMS[i], b_subcount[i], jme);

            return rad2deg(earth_values(sum, B_COUNT, jme));

        }

        private double earth_radius_vector(double jme)
        {
            double[] sum = new double[R_COUNT];
            int i;

            for (i = 0; i < R_COUNT; i++)
                sum[i] = earth_periodic_term_summation(R_TERMS[i], r_subcount[i], jme);

            return earth_values(sum, R_COUNT, jme);

        }

        private double geocentric_longitude(double l)
        {
            double theta = l + 180.0;

            if (theta >= 360.0) theta -= 360.0;

            return theta;
        }

        private double geocentric_latitude(double b)
        {
            return -b;
        }

        private double mean_elongation_moon_sun(double jce)
        {
            return third_order_polynomial(1.0 / 189474.0, -0.0019142, 445267.11148, 297.85036, jce);
        }

        private double mean_anomaly_sun(double jce)
        {
            return third_order_polynomial(-1.0 / 300000.0, -0.0001603, 35999.05034, 357.52772, jce);
        }

        private double mean_anomaly_moon(double jce)
        {
            return third_order_polynomial(1.0 / 56250.0, 0.0086972, 477198.867398, 134.96298, jce);
        }

        private double argument_latitude_moon(double jce)
        {
            return third_order_polynomial(1.0 / 327270.0, -0.0036825, 483202.017538, 93.27191, jce);
        }

        private double ascending_longitude_moon(double jce)
        {
            return third_order_polynomial(1.0 / 450000.0, 0.0020708, -1934.136261, 125.04452, jce);
        }

        private double xy_term_summation(int i, double[] x)
        {
            int j;
            double sum = 0;

            for (j = 0; j < TERM_Y_COUNT; j++)
                sum += x[j] * Y_TERMS[i][j];

            return sum;
        }


        private void nutation_longitude_and_obliquity(double jce, double[] x, double del_psi, double del_epsilon)
        {
            int i;
            double xy_term_sum, sum_psi = 0, sum_epsilon = 0;

            for (i = 0; i < Y_COUNT; i++)
            {
                xy_term_sum = deg2rad(xy_term_summation(i, x));
                sum_psi += (PE_TERMS[i][TERM_PSI_A] + jce * PE_TERMS[i][TERM_PSI_B]) * Math.Sin(xy_term_sum);
                sum_epsilon += (PE_TERMS[i][TERM_EPS_C] + jce * PE_TERMS[i][TERM_EPS_D]) * Math.Cos(xy_term_sum);
            }

            del_psi = sum_psi / 36000000.0;
            del_epsilon = sum_epsilon / 36000000.0;
        }


        private double ecliptic_mean_obliquity(double jme)
        {
            double u = jme / 10.0;

            return 84381.448 + u * (-4680.96 + u * (-1.55 + u * (1999.25 + u * (-51.38 + u * (-249.67 +
                               u * (-39.05 + u * (7.12 + u * (27.87 + u * (5.79 + u * 2.45)))))))));
        }


        private double ecliptic_true_obliquity(double delta_epsilon, double epsilon0)
        {
            return delta_epsilon + epsilon0 / 3600.0;
        }

        private double aberration_correction(double r)
        {
            return -20.4898 / (3600.0 * r);
        }

        private double apparent_sun_longitude(double theta, double delta_psi, double delta_tau)
        {
            return theta + delta_psi + delta_tau;
        }


        private double greenwich_mean_sidereal_time(double jd, double jc)
        {
            return limit_degrees(280.46061837 + 360.98564736629 * (jd - 2451545.0) +
                                               jc * jc * (0.000387933 - jc / 38710000.0));
        }

        private double greenwich_sidereal_time(double nu0, double delta_psi, double epsilon)
        {
            return nu0 + delta_psi * Math.Cos(deg2rad(epsilon));
        }

        private double geocentric_sun_right_ascension(double lamda, double epsilon, double beta)
        {
            double lamda_rad = deg2rad(lamda);
            double epsilon_rad = deg2rad(epsilon);

            return limit_degrees(rad2deg(Math.Atan2(Math.Sin(lamda_rad) * Math.Cos(epsilon_rad) -
                                               Math.Tan(deg2rad(beta)) * Math.Sin(epsilon_rad), Math.Cos(lamda_rad))));
        }


        private double geocentric_sun_declination(double beta, double epsilon, double lamda)
        {
            double beta_rad = deg2rad(beta);
            double epsilon_rad = deg2rad(epsilon);

            return rad2deg(Math.Asin(Math.Sin(beta_rad) * Math.Cos(epsilon_rad) +
                                Math.Cos(beta_rad) * Math.Sin(epsilon_rad) * Math.Sin(deg2rad(lamda))));
        }

        private double observer_hour_angle(double nu, float longitude, double alpha_deg)
        {
            return nu + longitude - alpha_deg;
        }

        private double sun_equatorial_horizontal_parallax(double r)
        {
            return 8.794 / (3600.0 * r);
        }



        private void sun_right_ascension_parallax_and_topocentric_dec(float latitude, float elevation, double xi,
                                double h, double delta, double delta_alpha, double delta_prime)
        {
            double lat_rad = deg2rad(latitude);
            double xi_rad = deg2rad(xi);
            double h_rad = deg2rad(h);
            double delta_rad = deg2rad(delta);
            double u = Math.Atan(0.99664719 * Math.Tan(lat_rad));
            double y = 0.99664717 * Math.Sin(u) + elevation * Math.Sin(lat_rad) / 6378140.0;
            double x = Math.Cos(u) + elevation * Math.Cos(lat_rad) / 6378140.0;

            delta_alpha = rad2deg(Math.Atan2(-x * Math.Sin(xi_rad) * Math.Sin(h_rad), Math.Cos(delta_rad) -
                                          x * Math.Sin(xi_rad) * Math.Cos(h_rad)));

            delta_prime = rad2deg(Math.Atan2((Math.Sin(delta_rad) - y * Math.Sin(xi_rad)) * Math.Cos(delta_alpha),
                                          Math.Cos(delta_rad) - x * Math.Sin(xi_rad) * Math.Cos(h_rad)));
        }




        private double topocentric_sun_right_ascension(double alpha_deg, double delta_alpha)
        {
            return alpha_deg + delta_alpha;
        }

        private double topocentric_local_hour_angle(double h, double delta_alpha)
        {
            return h - delta_alpha;
        }

        private double topocentric_elevation_angle(float latitude, double delta_prime, double h_prime)
        {
            double lat_rad = deg2rad(latitude);
            double delta_prime_rad = deg2rad(delta_prime);

            return rad2deg(Math.Asin(Math.Sin(lat_rad) * Math.Sin(delta_prime_rad) +
                                Math.Cos(lat_rad) * Math.Cos(delta_prime_rad) * Math.Cos(deg2rad(h_prime))));
        }



        private double atmospheric_refraction_correction(float pressure, float temperature, float atmos_refract,
                                                                            double e0)
        {
            double del_e = 0;

            if (e0 >= -1 * atmos_refract)
                del_e = (pressure / 1010.0) * (283.0 / (273.0 + temperature)) *
                         1.02 / (60.0 * Math.Tan(deg2rad(e0 + 10.3 / (e0 + 5.11))));

            return del_e;
        }

        private double topocentric_elevation_angle_corrected(double e0, double delta_e)
        {
            return e0 + delta_e;
        }

        private double topocentric_zenith_angle(double e)
        {
            return 90.0 - e;
        }


        private double topocentric_azimuth_angle_neg180_180(double h_prime, float latitude, double delta_prime)
        {
            double h_prime_rad = deg2rad(h_prime);
            double lat_rad = deg2rad(latitude);

            return rad2deg(Math.Atan2(Math.Sin(h_prime_rad),
                                 Math.Cos(h_prime_rad) * Math.Sin(lat_rad) - Math.Tan(deg2rad(delta_prime)) * Math.Cos(lat_rad)));
        }

        private double topocentric_azimuth_angle_zero_360(double azimuth180)
        {
            return azimuth180 + 180.0;
        }





        private double surface_incidence_angle(double zenith, double azimuth180, float azm_rotation, float slope)
        {
            double zenith_rad = deg2rad(zenith);
            double slope_rad = deg2rad(slope);

            return rad2deg(Math.Acos(Math.Cos(zenith_rad) * Math.Cos(slope_rad) +
                                Math.Sin(slope_rad) * Math.Sin(zenith_rad) * Math.Cos(deg2rad(azimuth180 - azm_rotation))));
        }

        private double sun_mean_longitude(double jme)
        {
            return limit_degrees(280.4664567 + jme * (360007.6982779 + jme * (0.03032028 +
                            jme * (1 / 49931.0 + jme * (-1 / 15300.0 + jme * (-1 / 2000000.0))))));
        }

        private double eot(double m, double alpha, double del_psi, double epsilon)
        {
            return limit_minutes(4.0 * (m - 0.0057183 - alpha + del_psi * Math.Cos(deg2rad(epsilon))));
        }

        private double approx_sun_transit_time(double alpha_zero, float longitude, double nu)
        {
            return (alpha_zero - longitude - nu) / 360.0;
        }

        private double sun_hour_angle_at_rise_set(float latitude, double delta_zero, double h0_prime)
        {
            double h0 = -99999;
            double latitude_rad = deg2rad(latitude);
            double delta_zero_rad = deg2rad(delta_zero);
            double argument = (Math.Sin(deg2rad(h0_prime)) - Math.Sin(latitude_rad) * Math.Sin(delta_zero_rad)) /
                                                             (Math.Cos(latitude_rad) * Math.Cos(delta_zero_rad));

            if (fabs(argument) <= 1) h0 = limit_degrees180(rad2deg(Math.Acos(argument)));

            return h0;
        }

        private void approx_sun_rise_and_set(double[] m_rts, double h0)
        {
            double h0_dfrac = h0 / 360.0;

            m_rts[SUN_RISE] = limit_zero2one(m_rts[SUN_TRANSIT] - h0_dfrac);
            m_rts[SUN_SET] = limit_zero2one(m_rts[SUN_TRANSIT] + h0_dfrac);
            m_rts[SUN_TRANSIT] = limit_zero2one(m_rts[SUN_TRANSIT]);
        }

        private double rts_alpha_delta_prime(double[] ad, double n)
        {
            double a = ad[JD_ZERO] - ad[JD_MINUS];
            double b = ad[JD_PLUS] - ad[JD_ZERO];

            if (fabs(a) >= 2.0) a = limit_zero2one(a);
            if (fabs(b) >= 2.0) b = limit_zero2one(b);

            return ad[JD_ZERO] + n * (a + b + (b - a) * n) / 2.0;
        }

        private double rts_sun_altitude(double latitude, double delta_prime, double h_prime)
        {
            double latitude_rad = deg2rad(latitude);
            double delta_prime_rad = deg2rad(delta_prime);

            return rad2deg(Math.Asin(Math.Sin(latitude_rad) * Math.Sin(delta_prime_rad) +
                                Math.Cos(latitude_rad) * Math.Cos(delta_prime_rad) * Math.Cos(deg2rad(h_prime))));
        }

        private double sun_rise_and_set(double[] m_rts, double[] h_rts, double[] delta_prime, double latitude,
                                double[] h_prime, double h0_prime, int sun)
        {
            return m_rts[sun] + (h_rts[sun] - h0_prime) /
                  (360.0 * Math.Cos(deg2rad(delta_prime[sun])) * Math.Cos(deg2rad(latitude)) * Math.Sin(deg2rad(h_prime[sun])));
        }



        private void calculate_geocentric_sun_right_ascension_and_declination(SPAData spa)
        {
            double[] x = new double[TERM_X_COUNT];

            spa.jc = julian_century(spa.jd);

            spa.jde = julian_ephemeris_day(spa.jd, spa.delta_t);
            spa.jce = julian_ephemeris_century(spa.jde);
            spa.jme = julian_ephemeris_millennium(spa.jce);

            spa.l = earth_heliocentric_longitude(spa.jme);
            spa.b = earth_heliocentric_latitude(spa.jme);
            spa.r = earth_radius_vector(spa.jme);

            spa.theta = geocentric_longitude(spa.l);
            spa.beta = geocentric_latitude(spa.b);

            x[TERM_X0] = spa.x0 = mean_elongation_moon_sun(spa.jce);
            x[TERM_X1] = spa.x1 = mean_anomaly_sun(spa.jce);
            x[TERM_X2] = spa.x2 = mean_anomaly_moon(spa.jce);
            x[TERM_X3] = spa.x3 = argument_latitude_moon(spa.jce);
            x[TERM_X4] = spa.x4 = ascending_longitude_moon(spa.jce);

            nutation_longitude_and_obliquity(spa.jce, x, spa.del_psi, spa.del_epsilon);

            spa.epsilon0 = ecliptic_mean_obliquity(spa.jme);
            spa.epsilon = ecliptic_true_obliquity(spa.del_epsilon, spa.epsilon0);

            spa.del_tau = aberration_correction(spa.r);
            spa.lamda = apparent_sun_longitude(spa.theta, spa.del_psi, spa.del_tau);
            spa.nu0 = greenwich_mean_sidereal_time(spa.jd, spa.jc);
            spa.nu = greenwich_sidereal_time(spa.nu0, spa.del_psi, spa.epsilon);

            spa.alpha = geocentric_sun_right_ascension(spa.lamda, spa.epsilon, spa.beta);
            spa.delta = geocentric_sun_declination(spa.beta, spa.epsilon, spa.lamda);
        }




        private void calculate_eot_and_sun_rise_transit_set(SPAData spa)
        {
            SPAData sun_rts = spa;
            double nu, m, h0, n;
            double[] alpha = new double[JD_COUNT];
            double[] delta = new double[JD_COUNT];
            double[] m_rts = new double[SUN_COUNT];
            double[] nu_rts = new double[SUN_COUNT];
            double[] h_rts = new double[SUN_COUNT];
            double[] alpha_prime = new double[SUN_COUNT];
            double[] delta_prime = new double[SUN_COUNT];
            double[] h_prime = new double[SUN_COUNT];
            double h0_prime = -1 * (SUN_RADIUS + spa.atmos_refract);
            int i;


            m = sun_mean_longitude(spa.jme);
            spa.eot = eot(m, spa.alpha, spa.del_psi, spa.epsilon);

            sun_rts.hour = sun_rts.minute = sun_rts.second = 0;

            sun_rts.timezone = 0.0f;

            sun_rts.jd = julian_day(sun_rts.year, sun_rts.month, sun_rts.day,
                                     sun_rts.hour, sun_rts.minute, sun_rts.second, sun_rts.timezone);

            calculate_geocentric_sun_right_ascension_and_declination(sun_rts);
            nu = sun_rts.nu;

            sun_rts.delta_t = 0;
            sun_rts.jd--;
            for (i = 0; i < JD_COUNT; i++)
            {
                calculate_geocentric_sun_right_ascension_and_declination(sun_rts);
                alpha[i] = sun_rts.alpha;
                delta[i] = sun_rts.delta;
                sun_rts.jd++;
            }

            m_rts[SUN_TRANSIT] = approx_sun_transit_time(alpha[JD_ZERO], spa.longitude, nu);
            h0 = sun_hour_angle_at_rise_set(spa.latitude, delta[JD_ZERO], h0_prime);

            if (h0 >= 0)
            {

                approx_sun_rise_and_set(m_rts, h0);

                for (i = 0; i < SUN_COUNT; i++)
                {

                    nu_rts[i] = nu + 360.985647 * m_rts[i];

                    n = m_rts[i] + spa.delta_t / 86400.0;
                    alpha_prime[i] = rts_alpha_delta_prime(alpha, n);
                    delta_prime[i] = rts_alpha_delta_prime(delta, n);

                    h_prime[i] = limit_degrees180pm(nu_rts[i] + spa.longitude - alpha_prime[i]);

                    h_rts[i] = rts_sun_altitude(spa.latitude, delta_prime[i], h_prime[i]);
                }

                spa.srha = h_prime[SUN_RISE];
                spa.ssha = h_prime[SUN_SET];
                spa.sta = h_rts[SUN_TRANSIT];

                spa.suntransit = dayfrac_to_local_hr(m_rts[SUN_TRANSIT] - h_prime[SUN_TRANSIT] / 360.0,
                                                      spa.timezone);

                spa.sunrise = dayfrac_to_local_hr(sun_rise_and_set(m_rts, h_rts, delta_prime,
                                  spa.latitude, h_prime, h0_prime, SUN_RISE), spa.timezone);

                spa.sunset = dayfrac_to_local_hr(sun_rise_and_set(m_rts, h_rts, delta_prime,
                                  spa.latitude, h_prime, h0_prime, SUN_SET), spa.timezone);

            }
            else spa.srha = spa.ssha = spa.sta = spa.suntransit = spa.sunrise = spa.sunset = -99999;

        }



        public int spa_calculate()
        {
            int result;

            result = validate_inputs(spa);

            if (result == 0)
            {
                spa.jd = julian_day(spa.year, spa.month, spa.day,
                                      spa.hour, spa.minute, spa.second, spa.timezone);

                calculate_geocentric_sun_right_ascension_and_declination(spa);

                spa.h = observer_hour_angle(spa.nu, spa.longitude, spa.alpha);
                spa.xi = sun_equatorial_horizontal_parallax(spa.r);

                sun_right_ascension_parallax_and_topocentric_dec(spa.latitude, spa.elevation, spa.xi,
                                            spa.h, spa.delta, spa.del_alpha, spa.delta_prime);

                spa.alpha_prime = topocentric_sun_right_ascension(spa.alpha, spa.del_alpha);
                spa.h_prime = topocentric_local_hour_angle(spa.h, spa.del_alpha);

                spa.e0 = topocentric_elevation_angle(spa.latitude, spa.delta_prime, spa.h_prime);
                spa.del_e = atmospheric_refraction_correction(spa.pressure, spa.temperature,
                                                                 spa.atmos_refract, spa.e0);
                spa.e = topocentric_elevation_angle_corrected(spa.e0, spa.del_e);

                spa.zenith = topocentric_zenith_angle(spa.e);
                spa.azimuth180 = topocentric_azimuth_angle_neg180_180(spa.h_prime, spa.latitude,
                                                                                     spa.delta_prime);
                spa.azimuth = topocentric_azimuth_angle_zero_360(spa.azimuth180);

                if ((spa.function == SPAFunctionType.SPA_ZA_INC) || (spa.function == SPAFunctionType.SPA_ALL))
                    spa.incidence = surface_incidence_angle(spa.zenith, spa.azimuth180,
                                                              spa.azm_rotation, spa.slope);

                if ((spa.function == SPAFunctionType.SPA_ZA_RTS) || (spa.function == SPAFunctionType.SPA_ALL))
                    calculate_eot_and_sun_rise_transit_set(spa);
            }

            return result;
        }

    }




    public enum SPAFunctionType
    {
        SPA_ZA,           //calculate zenith and azimuth
        SPA_ZA_INC,       //calculate zenith, azimuth, and incidence
        SPA_ZA_RTS,       //calculate zenith, azimuth, and sun rise/transit/set values
        SPA_ALL
    }



    public struct SPAData
    {
        //----------------------INPUT VALUES------------------------

        public int year;            // 4-digit year,    valid range: -2000 to 6000, error code: 1
        public int month;           // 2-digit month,         valid range: 1 to 12, error code: 2
        public int day;             // 2-digit day,           valid range: 1 to 31, error code: 3
        public int hour;            // Observer local hour,   valid range: 0 to 24, error code: 4
        public int minute;          // Observer local minute, valid range: 0 to 59, error code: 5
        public int second;          // Observer local second, valid range: 0 to 59, error code: 6

        public float delta_t;       // Difference between earth rotation time and terrestrial time
        //     (from observation)
        // valid range: -8000 to 8000 seconds, error code: 7

        public float timezone;      // Observer time zone (negative west of Greenwich)
        // valid range: -12   to   12 hours,   error code: 8

        public float longitude;     // Observer longitude (negative west of Greenwich)
        // valid range: -180  to  180 degrees, error code: 9

        public float latitude;      // Observer latitude (negative south of equator)
        // valid range: -90   to   90 degrees, error code: 10

        public float elevation;     // Observer elevation [meters]
        // valid range: -6500000 or higher meters,    error code: 11

        public float pressure;      // Annual average local pressure [millibars]
        // valid range:    0 to 5000 millibars,       error code: 12

        public float temperature;   // Annual average local temperature [degrees Celsius]
        // valid range: -273 to 6000 degrees Celsius, error code; 13

        public float slope;         // Surface slope (measured from the horizontal plane)
        // valid range: -360 to 360 degrees, error code: 14

        public float azm_rotation;  // Surface azimuth rotation (measured from south to projection of
        //     surface normal on horizontal plane, negative west)
        // valid range: -360 to 360 degrees, error code: 15

        public float atmos_refract; // Atmospheric refraction at sunrise and sunset (0.5667 deg is typical)
        // valid range: -5   to   5 degrees, error code: 16

        public SPAFunctionType function;        // Switch to choose functions for desired output (from enumeration)

        //-----------------Intermediate OUTPUT VALUES--------------------

        public double jd;          //Julian day
        public double jc;          //Julian century

        public double jde;         //Julian ephemeris day
        public double jce;         //Julian ephemeris century
        public double jme;         //Julian ephemeris millennium

        public double l;           //earth heliocentric longitude [degrees]
        public double b;           //earth heliocentric latitude [degrees]
        public double r;           //earth radius vector [Astronomical Units, AU]

        public double theta;       //geocentric longitude [degrees]
        public double beta;        //geocentric latitude [degrees]

        public double x0;          //mean elongation (moon-sun) [degrees]
        public double x1;          //mean anomaly (sun) [degrees]
        public double x2;          //mean anomaly (moon) [degrees]
        public double x3;          //argument latitude (moon) [degrees]
        public double x4;          //ascending longitude (moon) [degrees]

        public double del_psi;     //nutation longitude [degrees]
        public double del_epsilon; //nutation obliquity [degrees]
        public double epsilon0;    //ecliptic mean obliquity [arc seconds]
        public double epsilon;     //ecliptic true obliquity  [degrees]

        public double del_tau;     //aberration correction [degrees]
        public double lamda;       //apparent sun longitude [degrees]
        public double nu0;         //Greenwich mean sidereal time [degrees]
        public double nu;          //Greenwich sidereal time [degrees]

        public double alpha;       //geocentric sun right ascension [degrees]
        public double delta;       //geocentric sun declination [degrees]

        public double h;           //observer hour angle [degrees]
        public double xi;          //sun equatorial horizontal parallax [degrees]
        public double del_alpha;   //sun right ascension parallax [degrees]
        public double delta_prime; //topocentric sun declination [degrees]
        public double alpha_prime; //topocentric sun right ascension [degrees]
        public double h_prime;     //topocentric local hour angle [degrees]

        public double e0;          //topocentric elevation angle (uncorrected) [degrees]
        public double del_e;       //atmospheric refraction correction [degrees]
        public double e;           //topocentric elevation angle (corrected) [degrees]

        public double eot;         //equation of time [minutes]
        public double srha;        //sunrise hour angle [degrees]
        public double ssha;        //sunset hour angle [degrees]
        public double sta;         //sun transit altitude [degrees]

        //---------------------Final OUTPUT VALUES------------------------

        public double zenith;      //topocentric zenith angle [degrees]
        public double azimuth180;  //topocentric azimuth angle (westward from south) [-180 to 180 degrees]
        public double azimuth;     //topocentric azimuth angle (eastward from north) [   0 to 360 degrees]
        public double incidence;   //surface incidence angle [degrees]

        public double suntransit;  //local sun transit time (or solar noon) [fractional hour]
        public double sunrise;     //local sunrise time (+/- 30 seconds) [fractional hour]
        public double sunset;      //local sunset time (+/- 30 seconds) [fractional hour]

    }
}
