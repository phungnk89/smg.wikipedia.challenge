namespace SMG.Wikipedia.Elements
{
    public class YopMailPage
    {
        public static string btnDeleteAll = "#delall";
        public static string btnRefresh = "#refreshbut";
        public static string divResetPasswordMail = "(//*[text()='Account details on Wikipedia'])[1]";
        public static string frameInbox = "#ifinbox";
        public static string frameMail = "#ifmail";
        public static string txtMailContent = "//*[@id='mail']/*";
        public static string txtEmail = "[name='login']";
        public static string messageDeleted = "//*[contains(text(),'deleted')]";
    }
}
