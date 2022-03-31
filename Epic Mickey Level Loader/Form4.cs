using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Epic_Mickey_Level_Loader
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            label1.Text = "";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            WebClient c = new WebClient();
            button1.Enabled = false;
            button1.Text = "Downloading";
            //yes i use onedrive suck my dick
            c.DownloadFileAsync(new Uri("https://onedrive.live.com/download?cid=05EDAE1F7908A437&resid=5EDAE1F7908A437%213561868&authkey=ACM6bF8BwAssHes"), "EM.zip");
            c.DownloadProgressChanged += C_DownloadProgressChanged;
            c.DownloadFileCompleted += C_DownloadFileCompleted;
        }

        private async void C_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            label1.Text = "Extracting...";
            await Task.Factory.StartNew(() =>
            {
                ZipFile.ExtractToDirectory("EM.zip", "Game", true);
            });
            File.Delete("EM.zip");
            Settings1.Default.EmDirectory = "Game/Epic Mickey";
            Settings1.Default.EMPath = "Game/Epic Mickey/DATA/sys/main.dol";
            Settings1.Default.cmdline = "Game/Epic Mickey/DATA/files/cmdline.txt";
            Settings1.Default.Save();
            label1.Text = "Downloaded!";
            MessageBox.Show("Download Finished! This window will now close.");
            this.Hide();
        }

        private void C_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            label1.Text = e.ProgressPercentage + "% Downloaded";
            progressBar1.Value = e.ProgressPercentage;
        }
    }
}
