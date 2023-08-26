namespace SMG.Wikipedia.Elements
{
    public class ForgotPasswordPage
    {
        public static string txtUsername = "//*[@name='wpUsername']";
        public static string txtEmail = "//*[@name='wpEmail']";
        public static string btnResetPassword = "//*[@value='Reset password']";
        public static string headerResetPassword = "//h1[text()='Reset password']";
        public static string labelResetInstruction = "//*[contains(text(),'You have requested a password reset.')]";
    }
}
