using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FTP_Server
{
    public partial class Form1 : Form
    {

        FTP ftp = new FTP(@"ftp://127.0.0.1/", "tanvir", "aspire");

        public Form1()
        {
            InitializeComponent();
        }

        private void startFTPButton_Click(object sender, EventArgs e)
        {
            GetFTPDataList();
        }


        private void downloadButton_Click(object sender, EventArgs e)
        {
            string fileName = downloadTextBox.Text;

            List<string> list = ftp.GetServerFiles();

            bool flag = true;

            foreach (var item in list)
            {
                string[] words = item.Split(' ');
                int length = words.Length;

                if (fileName == words[length - 1])
                {
                    flag = false;
                    break;
                }
            }

            if (flag == true)
            {
                errorLabel.Text = "File Not Found !! Try Again";
                deleteTextBox.Text = " ";
                messageLabel.Text = " ";
            }
            else if (flag == false)
            {

                string message = ftp.download(fileName, @"C:\Users\Tanvir\Desktop\LocalFTP\" + fileName);
                messageLabel.Text = message;
                errorLabel.Text = " ";
                GetFTPDataList();
            }
        }

        private void uploadButton_Click(object sender, EventArgs e)
        {
            string fileName = uploadTextBox.Text;

            List<string> list = ftp.GetServerFiles();
            bool flag = true;


            foreach (var item in list)
            {
                string[] words = item.Split(' ');
                int length = words.Length;

                if (fileName == words[length - 1])
                {
                    flag = false;
                    break;
                }

            }

            if (flag == false)
            {
                errorLabel.Text = "File Already Exist ! try again";
                uploadTextBox.Text = " ";
                messageLabel.Text = " ";
            }
            else if (flag == true)
            {
                string message = ftp.upload(fileName, @"C:\Users\Tanvir\Desktop\LocalFTP\" + fileName );
                messageLabel.Text = message;
                uploadTextBox.Text = " ";
                errorLabel.Text = " ";
                GetFTPDataList();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            string fileName = deleteTextBox.Text;

            List<string> list = ftp.GetServerFiles();
            bool flag = true;


            foreach (var item in list)
            {
                string[] words = item.Split(' ');
                int length = words.Length;

                if (fileName == words[length - 1])
                {
                    flag = false;
                    break;
                }
            }


            if (flag == true)
            {
                errorLabel.Text = "File Not Found !! Try Again";
                deleteTextBox.Text = " ";
                messageLabel.Text = " ";
            }
            else if (flag == false)
            {
                string message = ftp.delete(fileName);
                messageLabel.Text = message;
                deleteTextBox.Text = " ";
                errorLabel.Text = " ";
                GetFTPDataList();
            }
        }

        private void renameButton_Click(object sender, EventArgs e)
        {
            string prevName = prevTextBox.Text;
            string newName = newTextBox.Text;

            List<string> list = ftp.GetServerFiles();
            bool flag1 = true;
            bool flag2 = true;


            foreach (var item in list)
            {
                string[] words = item.Split(' ');
                int length = words.Length;

                if (prevName == words[length - 1])
                {
                    flag1 = false;
                    break;
                }
                if (newName == words[length - 1])
                {
                    flag2 = false;
                    break;
                }
            }

            if (flag1 == true)
            {
                errorLabel.Text = "Previous File Name Not Found !! Try Again";
                prevTextBox.Text = " ";
                messageLabel.Text = " ";
            }
            else if (flag2 == false)
            {
                errorLabel.Text = "New File Name Already Exist !! Try Again";
                prevTextBox.Text = " ";
                messageLabel.Text = " ";
            }
            else if (flag1 == false && flag2 == true)
            {
                string message = ftp.rename(prevName, newName);
                messageLabel.Text = message;
                errorLabel.Text = " ";
                prevTextBox.Text = " ";
                newTextBox.Text = " ";
                GetFTPDataList();
            }
        }





        private void GetFTPDataList()
        {
            List<string> list = ftp.GetServerFiles();
            List<string> fileList = new List<string>();

            foreach (var item in list)
            {
                string[] words = item.Split(' ');
                int length = words.Length;
                fileList.Add(words[length - 1]);
            }
            serverDataGridView.DataSource = fileList.Select(x => new { Files = x }).ToList();





            clientDataGridView.DataSource = new System.IO.DirectoryInfo(@"C:\Users\Tanvir\Desktop\LocalFTP").GetFiles();
        }

        private void stopFTPButton_Click(object sender, EventArgs e)
        {
            serverDataGridView.DataSource = new object();
            clientDataGridView.DataSource = new object();
        }
    }
}
