using System;
using System.Collections.Generic;
using System.Text;

namespace OnSale.Common.Business
{
    public class Constants
    {
        public static string URL_BASE => "http://10.1.114.74:84";

        public static string SERVICE_PREFIX => "/api";

        public class Path
        {
            public static string PathNoImage => URL_BASE + "/images/noimage.png";
        }

        public class TextString
        {
            public static string MessageContains => "duplicate";
            public static string MessageErrorDuplicate = "Ya existe un registro con el mismo nombre.";
        }
    }
}
