using System;
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
        public static EventHandler onDownloadStart;
        public static EventHandler onDownloadFinish;

        public DownloadTemplate()
        {
            InitializeComponent();
            label3.Text = "";

            button1.Enabled = Form2.GameInstalled;
            
            label4.Text = "";

          
        }

        private void DownloadTemplate_Load(object sender, EventArgs e)
        {
           
        }

        public void Initialize()
        {
            label2.Text = modName;
            pictureBox1.ImageLocation = iconLink;

            if (Directory.Exists(Settings1.Default.EmDirectory))
            {
                if (File.Exists(Settings1.Default.EmDirectory + "/EML.dat"))
                {
                    string get = File.ReadAllText(Settings1.Default.EmDirectory + "/EML.dat");

                    if (get == modName)
                    {
                        label4.Text = "Mod already installed!";
                    }
                }
            }
            if (dirExists("InstalledMods/" + modName))
            {
                button1.Text = "Install";
            }

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

            if(!downloading)
            {
                label4.Text = ""; 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(Directory.Exists("InstalledMods/" + modName))
            {
                Install("InstalledMods/" + modName, true);
            }
            else
            {
                if (!IsDownloading)
                {
                    Download();
                }
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
            if(Directory.Exists("tempTexture"))
            {
                Directory.Delete("tempTexture", true);
            }
            
            if(File.Exists("texturepack.zip"))
            {
                File.Delete("texturepack.zip");
            }
            IsDownloading = false;
            onDownloadFinish.Invoke(this, EventArgs.Empty);
            downloading = false;

            progressBar1.Visible = false;
            label3.Text = "";
        }
        private void MoveFiles(string sourcePath, string targetPath, bool storeFiles = false, string getFilesFrom = "tempTexture/files/DATA", string replace = "tempTexture/files/")
        {
            Directory.CreateDirectory("OG_FILE");
            Directory.CreateDirectory("OG_FILE/DATA");
            Directory.CreateDirectory("OG_FILE/DATA/files");
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                if(storeFiles)
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, "OG_FILE/DATA"));
                }
               
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                if(storeFiles && File.Exists(newPath.Replace(getFilesFrom, targetPath)) && !File.Exists(newPath.Replace(replace, "OG_FILE/")))
                {
                    File.Copy(newPath.Replace(getFilesFrom, targetPath), newPath.Replace(replace, "OG_FILE/"), true);
                }
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Directory.CreateDirectory("tempTexture");
            Install();
        }
        async void Install(string path = "tempTexture", bool forceNoCache = false)
        {
            if (Settings1.Default.OgFileReinstate && File.Exists("OG_FILE/DATA"))
            {
                MoveFiles("OG_FILE/DATA", Settings1.Default.EmDirectory + "/DATA");
            }
            label3.Text = "Installing... This might take a while";
            await Task.Factory.StartNew(() =>
            {
                if(File.Exists("texturepack.zip"))
                {
                    ZipFile.ExtractToDirectory("texturepack.zip", "tempTexture", true);
                }
                
                if(Settings1.Default.CacheMods && !forceNoCache)
                {
                    MessageBox.Show("extract");
                    Directory.CreateDirectory("InstalledMods");
                    Directory.CreateDirectory("InstalledMods/" + modName);
                    ZipFile.ExtractToDirectory("texturepack.zip", "InstalledMods/" + modName, true);
                }
            });
            string getfiles = path + "/files/DATA";
            string replace = path + "/files/";
            MessageBox.Show(path);
            if (dirExists(path))
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
                            return;
                        }
                        else
                        {
                            Settings1.Default.EmDirectory = folder.SelectedPath;
                            Settings1.Default.Save();
                            
                            MoveFiles(path + "/files/DATA", Settings1.Default.EmDirectory + "/DATA", true, getfiles, replace);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Directory was not set. Download Failed.");
                        finish();
                        return;
                    }
                }
                else
                {
                    MoveFiles(path +"/files/DATA", Settings1.Default.EmDirectory + "/DATA", true, getfiles, replace);
                }


            }
            if (dirExists(path + "/custtext"))
            {
                //this is a complete gutter but until i find a better way to do this its gonna stay like this
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator/Load");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator/Load/Textures");
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator/Load/Textures/SEME4Q");
                MoveFiles(path + "/custtext", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator/Load/Textures/SEME4Q");

            }
            Form3.instance.UpdateButton(true);
            File.WriteAllText(Settings1.Default.EmDirectory + "/EML.dat", modName);
            label4.Text = "Mod already installed!";
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
