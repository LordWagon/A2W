namespace LoginManager.Shared
{
    public class FtpSetting
    {
        public string Folder { get; set; }
        public string Hostname { get; set; }
        public string Username { get; set; }
        public string CryptedPassword { get; set; }
        public string Password { get; set; }
        
        public bool DecryptPassword()
        {
            try
            {
                Password = Crypto.Decrypt(CryptedPassword);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool VerifyFolder(string encodedFolder)
        {
            try 
            {
                string folder;
                string crypt;
                int index = encodedFolder.IndexOf('-');
                if (index >= 0)
                {
                    folder = encodedFolder.Substring(0, index);
                    crypt = encodedFolder.Substring(index + 1);
                }
                else
                    return false;

                string decrypt = Crypto.Decrypt(crypt);
                if (decrypt != folder)
                    return false;
                
                Folder = decrypt;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
