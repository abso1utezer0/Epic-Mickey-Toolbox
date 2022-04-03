using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Epic_Mickey_Level_Loader
{
    public partial class Form3 : Form
    {
        public static Form3 instance;

        //todo: get rid of functions that arent being used since they look like cr*p
        public Form3()
        {
            InitializeComponent();
            instance = this;
            button1.Enabled = Form2.GameInstalled;
            label1.Text = "PS. The mod installer is in very early development so for each new\nmod you install you may need to reinstall epic mickey\nin the main window unless you want the mods merged.";
            label2.Text = "Remember to enable custom textures in dolphin by\nclicking on Graphics then going to the Advanced tab and then\nclicking Load Custom Textures";
            Form2.onChange += Init;
            Form5.ChangeTheme(this.Controls, this, Settings1.Default.DarkMode);
        }

        public void UpdateButton(bool b)
        {
            button1.Enabled = b;
        }
        void Format()
        {
            WebClient client = new WebClient();
            string DownloadInfo = client.DownloadString("https://memerdev.com/EM/downloadinfo.txt");
            string[] info = DownloadInfo.Split("\n");

            label3.Text = info.Length.ToString();
            foreach (string i in info)
            {
                string output = i;
                string[] all = output.Split("`");
                DownloadTemplate cont = new DownloadTemplate();
                cont.downloadLink = all[2];
                cont.iconLink = all[1];
                cont.modName = all[0];
                cont.Initialize();
                flowLayoutPanel1.Controls.Add(cont);
            }
        }
        void Init(object sender, EventArgs e)
        {
            Form5.ChangeTheme(this.Controls, this, Settings1.Default.DarkMode);
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            Format();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
        private void MoveFiles(string sourcePath, string targetPath, bool storeFiles = false)
        {
            Directory.CreateDirectory("OG_FILE");
            Directory.CreateDirectory("OG_FILE/DATA");
            Directory.CreateDirectory("OG_FILE/DATA/files");
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                if (storeFiles)
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, "OG_FILE/DATA"));
                }

                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                if (storeFiles && File.Exists(newPath.Replace("tempTexture/files/DATA", targetPath)))
                {
                    File.Copy(newPath.Replace("tempTexture/files/DATA", targetPath), newPath.Replace("tempTexture/files/", "OG_FILE/"), true);
                }

                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (Directory.Exists("OG_FILE/DATA"))
            {
                MoveFiles("OG_FILE/DATA", Settings1.Default.EmDirectory + "/DATA");
                Directory.Delete("OG_FILE", true);
            }
            else
            {
                Form4 f = new Form4();
                f.Show();
            }
         

            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Dolphin Emulator/Load/Textures/SEME4Q";
            if(Directory.Exists(path))
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
        }
    }
}
