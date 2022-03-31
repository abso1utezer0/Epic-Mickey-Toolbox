﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.IO;

namespace Epic_Mickey_Level_Loader
{
    public partial class DownloadTemplate : UserControl
    {
        public string modName;
        public string downloadLink;
        public string iconLink;
        public static bool IsDownloading;
        bool downloading;
        public EventHandler onDownloadStart;
        public EventHandler onDownloadFinish;

        public DownloadTemplate()
        {
            InitializeComponent();
            label3.Text = "";
        }

        private void DownloadTemplate_Load(object sender, EventArgs e)
        {
           
        }

        public void Initialize()
        {
            label2.Text = modName;
            pictureBox1.ImageLocation = iconLink;

            onDownloadStart += OnStartDownload;
            onDownloadFinish += OnDownloadFinish;
        }
        void OnStartDownload(object sender, EventArgs e)
        {
            if(downloading)
            {
                button1.Text = "Downloading";
            }
            else
            {
                button1.Text = "Unavailable";
            }

            button1.Enabled = false;
        }
        void OnDownloadFinish(object sender, EventArgs e)
        {
            button1.Text = "Download";
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!IsDownloading)
            {
                Download();
            }
        }
        void Download()
        {
            downloading = true;
            onDownloadStart.Invoke(this, EventArgs.Empty);
            WebClient client = new WebClient();
            label3.Text = "Preparing Download";
            progressBar1.Visible = true;
            client.DownloadFileAsync(new Uri(downloadLink), "texturepack.zip");

            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
        }
        void finish()
        {
            Directory.Delete("tempTexture", true);
            downloading = false;
            IsDownloading = false;
            onDownloadFinish.Invoke(this, EventArgs.Empty);

            progressBar1.Visible = false;
            label3.Text = "";
        }
        private void MoveFiles(string sourcePath, string targetPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        private async void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Directory.CreateDirectory("tempTexture");
            label3.Text = "Installing... This might take a while";
            await Task.Factory.StartNew(() =>
            {
                ZipFile.ExtractToDirectory("texturepack.zip", "tempTexture", true);
            });
            if(dirExists("tempTexture/files"))
            {
                if (Settings1.Default.EmDirectory == "")
                {
                    MessageBox.Show("Your Epic Mickey dump directory has not been set. Please find the path to your dump and set it here");
                    FolderBrowserDialog folder = new FolderBrowserDialog();
                    if (folder.ShowDialog() == DialogResult.OK)
                    {
                        if (!dirExists(folder.SelectedPath + "/DATA"))
                        {
                            MessageBox.Show("Directory does not have DATA directory. Download Failed.");
                            finish();
                        }
                        else
                        {
                            Settings1.Default.EmDirectory = folder.SelectedPath;
                            Settings1.Default.Save();

                            MoveFiles("tempTexture/files/DATA", Settings1.Default.EmDirectory + "/DATA");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Directory was not set. Download Failed.");
                        finish();
                    }
                }
                else
                {
                    MoveFiles("tempTexture/files/DATA", Settings1.Default.EmDirectory + "/DATA");
                }
            
                
            }
            if(dirExists("tempTexture/custtext"))
            {
                //this is a complete gutter but until i find a better way to do this its gonna stay like this
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator");
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator/Load");
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator/Load/Textures");
                    Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator/Load/Textures/SEME4Q");
                MoveFiles("tempTexture/custtext", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator/Load/Textures/SEME4Q");
                label3.Text = "cloudZ!";
            }
      
            MessageBox.Show(modName + " Has been installed!");
            finish();
        }

        
        public bool dirExists(string path = "tempTexture")
        {
           return Directory.Exists(path);
        }
     


        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            label3.Text = e.ProgressPercentage + "%";
            float percentage = e.BytesReceived / e.TotalBytesToReceive * 100;
            progressBar1.Value = e.ProgressPercentage;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}