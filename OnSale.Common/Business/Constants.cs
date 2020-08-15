namespace OnSale.Common.Business
{
    public class Constants
    {
        public static string URL_BASE => "http://10.1.114.74:84";
        //public static string URL_BASE => "http://localhost:51773";
        //public static string URL_BASE => "https://onsalewebdacalo.azurewebsites.net";
        public static string URL_BASE_BLOB => "https://onsaledacalo.blob.core.windows.net";

        public static string SERVICE_PREFIX => "/api";

        public class Path
        {
            public static string PathNoImage => URL_BASE + "/images/noimage.png";
            public static string PathImageEmpty => URL_BASE + "/images/d844c6c4-c929-4518-abeb-e900ac95ac53";
        }

        public class TextString
        {
            public static string MessageContains = "duplicate";
            public static string MessageErrorDuplicate = "Ya existe un registro con el mismo nombre.";
            public static string GuidImageEmpty = "d844c6c4-c929-4518-abeb-e900ac95ac53";
        }

        public class EndPoints
        {
            public static string GetProducts = "/Products";
        }
    }
}
