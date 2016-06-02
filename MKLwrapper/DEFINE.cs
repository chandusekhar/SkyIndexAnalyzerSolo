using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKLwrapper
{
    public static class DEFINE
    {
        public static int TR_SUCCESS = 1501;
        public static int TR_INVALID_OPTION = 1502;
        public static int TR_OUT_OF_MEMORY = 1503;



        public static int VSL_SS_MATRIX_STORAGE_ROWS        = 0x00010000;
        public static int VSL_SS_MATRIX_STORAGE_COLS        = 0x00020000;
        public const int VSL_SS_ED_2R_MOM                   = 8;
        public const int VSL_SS_ED_3R_MOM                   = 9;
        public const int VSL_SS_ED_4R_MOM                   = 10;
        public const int VSL_SS_ED_2C_MOM                   = 11;
        public const int VSL_SS_ED_3C_MOM                   = 12;
        public const int VSL_SS_ED_4C_MOM                   = 13;
        public const int VSL_SS_ED_MIN                      = 16;
        public const int VSL_SS_ED_MAX                      = 17;
        public const int VSL_SS_METHOD_FAST                 = 0x00000001;
        public const int VSL_SS_ED_KURTOSIS                 = 14;
        public const int VSL_SS_ED_SKEWNESS                 = 15;



        public const long VSL_SS_ED_MEAN = 7;
        public const long VSL_SS_MEAN = 0x0000000000000001;
        public const long VSL_SS_2R_MOM = 0x0000000000000002;
        public const long VSL_SS_3R_MOM = 0x0000000000000004;
        public const long VSL_SS_4R_MOM = 0x0000000000000008;
        public const long VSL_SS_2C_MOM = 0x0000000000000010;
        public const long VSL_SS_3C_MOM = 0x0000000000000020;
        public const long VSL_SS_4C_MOM = 0x0000000000000040;
        public const long VSL_SS_SUM = 0x0000000002000000;
        public const long VSL_SS_2R_SUM = 0x0000000004000000;
        public const long VSL_SS_3R_SUM = 0x0000000008000000;
        public const long VSL_SS_4R_SUM = 0x0000000010000000;
        public const long VSL_SS_2C_SUM = 0x0000000020000000;
        public const long VSL_SS_3C_SUM = 0x0000000040000000;
        public const long VSL_SS_4C_SUM = 0x0000000080000000;
        public const long VSL_SS_KURTOSIS = 0x0000000000000080;
        public const long VSL_SS_SKEWNESS = 0x0000000000000100;
        public const long VSL_SS_VARIATION = 0x0000000000000200;
        public const long VSL_SS_MIN = 0x0000000000000400;
        public const long VSL_SS_MAX = 0x0000000000000800;
        public const long VSL_SS_COV = 0x0000000000001000;
        public const long VSL_SS_COR = 0x0000000000002000;
        public const long VSL_SS_CP = 0x0000000100000000;
        public const long VSL_SS_POOLED_COV = 0x0000000000004000;
        public const long VSL_SS_GROUP_COV = 0x0000000000008000;
        public const long VSL_SS_POOLED_MEAN = 0x0000000800000000;
        public const long VSL_SS_GROUP_MEAN = 0x0000001000000000;
        public const long VSL_SS_QUANTS = 0x0000000000010000;
        public const long VSL_SS_ORDER_STATS = 0x0000000000020000;
        public const long VSL_SS_SORTED_OBSERV = 0x0000008000000000;
        public const long VSL_SS_ROBUST_COV = 0x0000000000040000;
        public const long VSL_SS_OUTLIERS = 0x0000000000080000;
        public const long VSL_SS_PARTIAL_COV = 0x0000000000100000;
        public const long VSL_SS_PARTIAL_COR = 0x0000000000200000;
        public const long VSL_SS_MISSING_VALS = 0x0000000000400000;
        public const long VSL_SS_PARAMTR_COR = 0x0000000000800000;
        public const long VSL_SS_STREAM_QUANTS = 0x0000000001000000;
        public const long VSL_SS_MDAD = 0x0000000200000000;
        public const long VSL_SS_MNAD = 0x0000000400000000;




        public const int VSL_STATUS_OK = 0;
    }
}
