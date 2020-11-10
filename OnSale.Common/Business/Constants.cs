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
            public static string PathImageUser => URL_BASE + "/users/";
        }

        public class TextString
        {
            public static string MessageBadRequest = "Solicitud incorrecta.";
            public static string MessageInstructionsSend = "Las instrucciones del usuario han sido enviadas al correo.";
            public static string MessageErrorResetting = "Error mientras se reestablecia la contraseña.";
            public static string MessagePasswordReset = "Se reestableció la contraseña satisfactoriamente.";
            public static string MessageEmailAlreadyUsed = "El correo ya está registrado por otro usuario.";
            public static string MessageEmailRegisteredUser = "El correo no corresponde a un usuario registrado.";
            public static string MessageUserNoFound = "Usuario no encontrado.";
            public static string MessageContains = "duplicate";
            public static string MessageErrorDuplicate = "Ya existe un registro con el mismo nombre.";
            public static string GuidImageEmpty = "d844c6c4-c929-4518-abeb-e900ac95ac53";
            public static string SyncfusionLicense = "MzMxOTg1QDMxMzgyZTMzMmUzMEZFczliOC8xRXdSMXJwNWd2dTNONnFYNGtkVmg5eWZmWW91TWVMcnlyLzg9";
            public static string StripeClavePublicable  = "pk_test_51HYHKpDX9rnrWo2nWtx8Gy5Yu3q2cycZE22VDKBqwnrAFy1RrMEsDgvn2wsKWSM3ayVJuQGPSZOXcYZcOzfTxjkS00n8dRuEKr";
            public static string StripeClaveSecreta  = "sk_test_51HYHKpDX9rnrWo2nocNDBm5VQSwMSpODyYeOglO38oX2UBoNoN7mgVCQ6ArV6aKmwetkBhDGT0lRplis5LlHzpMU00xrku9Fbx";

        }

        public class EndPoints
        {
            public static string GetProducts = "/Products";
            public static string PostQualifications = "/Qualifications";
            public static string PostCreateToken = "/Account/CreateToken";
            public static string GetCountries = "/Countries";
            public static string PostRegisterUser = "/Account/Register";
            public static string PostRecoverPassword = "/Account/RecoverPassword";
            public static string GetContries = "/Countries";
            public static string PostModifyUser = "/Account";
            public static string PostChangePassword = "/Account/ChangePassword";
            public static string PostOrders = "/Orders";
            public static string GetLoginFacebook = "/Account/LoginFacebook";
            public static string GetUser = "/Account";
        }
    }
}
