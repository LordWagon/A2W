using System;
using System.IO;
using System.Net;
using System.Text;

namespace LoginManager.Shared
{
    public class FtpUtil
    {
        private string _ftpUrl;
        private string _userName;
        private string _password;

        public FtpUtil(string ftpUrl, string userName, string password)
        {
            _ftpUrl = ftpUrl;
            _userName = userName;
            _password = password;
        }

        public void UploadFile(string filePath, string targetFileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpUrl + "/" + targetFileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(_userName, _password);

            byte[] fileContents;
            
            using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fileContents = new byte[sourceStream.Length];
                sourceStream.Read(fileContents, 0, fileContents.Length);
            }

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }
        }

        public void DownloadFile(string sourceFileName, string targetFilePath)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpUrl + "/" + sourceFileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(_userName, _password);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream responseStream = response.GetResponseStream())
            using (FileStream writer = new FileStream(targetFilePath, FileMode.Create))
            {
                responseStream.CopyTo(writer);
                Console.WriteLine($"Download Completed, status {response.StatusDescription}");
            }
        }

        public DateTime GetLastModifiedTimestamp(string filePathOnServer) {           
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_ftpUrl + "//" + filePathOnServer);
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            request.Credentials = new NetworkCredential(_userName, _password);
        
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.LastModified;
            }
        }
    }
}
