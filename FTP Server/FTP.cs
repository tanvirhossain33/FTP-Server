using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTP_Server
{
    class FTP
    {
        private string host = null;
        private string user = null;
        private string pass = null;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;
        private int bufferSize = 2048;


        public FTP(string host, string user, string pass)
        {
            this.host = host;
            this.user = user;
            this.pass = pass;
        }


       

        public List<string> GetServerFiles()
        {
            
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            
            request.Credentials = new NetworkCredential(user, pass);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string names = reader.ReadToEnd();

            reader.Close();
            response.Close();

            return names.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            
        }



        /* Download File */
        public string download(string remoteFile, string localFile)
        {
            string message = "";

            try
            {

                ftpRequest = (FtpWebRequest) FtpWebRequest.Create(host + remoteFile);

                ftpRequest.Credentials = new NetworkCredential(user, pass);

                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;

                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();

                ftpStream = ftpResponse.GetResponseStream();

                FileStream localFileStream = new FileStream(localFile, FileMode.Create);

                byte[] byteBuffer = new byte[bufferSize];
                int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);

                try
                {
                    while (bytesRead > 0)
                    {
                        localFileStream.Write(byteBuffer, 0, bytesRead);
                        bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                    }

                    message = "Download Compelte .";
                }
                catch (Exception ex)
                {
                    message = "Download Error !!";
                }

                localFileStream.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                message = "Download Error";
            }
            return message;
        }


        /* Upload File */
        public string upload(string remoteFile, string localFile)
        {
            string message = "";
            try
            {

                ftpRequest = (FtpWebRequest) FtpWebRequest.Create(host + remoteFile);

                ftpRequest.Credentials = new NetworkCredential(user, pass);

                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;

                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                ftpStream = ftpRequest.GetRequestStream();

                FileStream localFileStream = new FileStream(localFile, FileMode.Create);

                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);

                try
                {
                    while (bytesSent != 0)
                    {
                        ftpStream.Write(byteBuffer, 0, bytesSent);
                        bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                    }
                    message = "Upload Compelte .";
                }
                catch (Exception ex)
                {
                    message = "Upload Error";
                }

                localFileStream.Close();
                ftpStream.Close();
                ftpRequest = null;
            }
            catch (Exception ex)
            {
                message = "Upload Error";
            }
            return message;
        }


        /* Delete File */
        public string delete(string deleteFile)
        {
            string message = " ";
            try
            {

                ftpRequest = (FtpWebRequest) WebRequest.Create(host + deleteFile);

                ftpRequest.Credentials = new NetworkCredential(user, pass);

                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;

                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();

                ftpResponse.Close();
                ftpRequest = null;

                message = " File Delete Successfull";
            }
            catch (Exception ex)
            {
                message = " Delete Error !! Try Again ";
            }
            return message;
        }



        /* Rename File */
        public string rename(string currentFileNameAndPath, string newFileName)
        {
            string message = " ";
            try
            {

                ftpRequest = (FtpWebRequest) WebRequest.Create(host + currentFileNameAndPath);

                ftpRequest.Credentials = new NetworkCredential(user, pass);

                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;

                ftpRequest.Method = WebRequestMethods.Ftp.Rename;

                ftpRequest.RenameTo = newFileName;

                ftpResponse = (FtpWebResponse) ftpRequest.GetResponse();

                ftpResponse.Close();
                ftpRequest = null;

                message = " Rename Successfull";
            }
            catch (Exception ex)
            {
                message = "Rename Error !!";
            }
            return message;
        }



    }
}
