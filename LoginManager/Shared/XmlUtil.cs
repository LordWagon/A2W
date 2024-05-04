using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LoginManager.Shared
{
    public class XmlUtil
    {
        private static string _xmlFileName = PlatformSpecifics.GetPathToFtpSetting();

        public FtpSetting? GetSetting()
        {
            XDocument doc = XDocument.Load(_xmlFileName);
            FtpSetting ftpSetting = new FtpSetting();

            foreach (XElement element in doc.Root.Elements())
            {
                switch (element.Name.ToString())
                {
                    case "folder":
                        bool result = ftpSetting.VerifyFolder(element.Value);
                        if (!result)
                            return null;
                        break;
                    case "hostname" :
                        ftpSetting.Hostname = element.Value;
                        break;
                    case "username":
                        ftpSetting.Username = element.Value;
                        break;
                    case "password":
                        ftpSetting.CryptedPassword = element.Value;
                        break;
                }
            }
            bool successfullyDecrypted = ftpSetting.DecryptPassword();
            if (successfullyDecrypted)
            {
                return ftpSetting;
            }
            else 
            { 
                Console.WriteLine("Failed to decrypt password");
                return null;
            }
        }
    }
}