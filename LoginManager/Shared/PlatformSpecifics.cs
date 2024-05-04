namespace LoginManager.Shared
{
    internal class PlatformSpecifics
    {
        internal static string GetDirectory()
        {
#if ANDROID
            return "/storage/emulated/0/Android/data/com.companyname.age2word/files"; 
#else
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "A2W");
#endif
            //Xiaomi: return "/storage/emulated/0/Download";
            // return System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
        }
        internal static string GetPathToDb() 
        {
            return Path.Combine(GetDirectory(), "A2W.db");
        }
        internal static string GetPathToCryptoKeySetting() 
        {
            return Path.Combine(GetDirectory(), "Cipher.key");
        }
        internal static string GetPathToCryptoIVSetting() 
        { 
            return Path.Combine(GetDirectory(), "Cipher.IV");
        }
        internal static string GetPathToFtpSetting() 
        { 
            return Path.Combine(GetDirectory(), "ftpSetting.xml");
        }
    }
}



