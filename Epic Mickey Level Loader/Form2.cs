using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Epic_Mickey_Level_Loader
{
    public partial class Form2 : Form
    {
        //random green quotes dedicated to mr slaycap (mr green)
        string[] s = { "'The grass is always greener on the other side'", "'It's that not easy being green'", "'Greeny-Game walked so Bluey-Game could run'" };

        public static Form2 instance;

        public static EventHandler onChange;

        public static bool GameInstalled;
        public static string version = "v.1.4";
        public Form2()
        {
            InitializeComponent();
            instance = this;
            onChange += OnChange;
         
          

        }

        bool isReady;

        private void Form2_Load(object sender, EventArgs e)
        {
            Init();
        }
       async void Init()
        {
            button2.Enabled = false;
            GameInstalled = false;
            Form5.ChangeTheme(this.Controls, this, Settings1.Default.DarkMode);
            if (Settings1.Default.DolphinPath == "")
            {
                    textBox1.Text = "Please define your dolphin.exe path in Settings";   
            }
            else
            {
                if(EpicMickeyLauncher.CheckForDeletedFile(Settings1.Default.DolphinPath))
                {
                    textBox1.Text = "Dolphin path does not exist.";
                }
                if (Settings1.Default.EMPath != "")
                {
                    if (EpicMickeyLauncher.CheckForDeletedFile(Settings1.Default.EMPath))
                    {
                        textBox1.Text = "Main.dol no longer exists. Please reinstall the game";
                    }
                    else
                    {
                        textBox1.Text = "Ready to play";
                        button2.Enabled = true;
                        isReady = true;
                        GameInstalled = true;
                    }

                }
                else
                {
                    textBox1.Text = "Main.dol path has not been assigned";
                }
            }
            bool updateAvailable = false;
            await Task.Run(() =>
            {
                WebClient web = new WebClient();
                string v = web.DownloadString("https://memerdev.com/eml/ver.txt");
                updateAvailable = v != version;
            });
            button6.Enabled = updateAvailable;

        }
        void OnChange(object sender, EventArgs e)
        {
            Init();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4 f = new Form4();
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(isReady)
            {
                if(checkBox1.Checked)
                {
                    foreach (Process p in Process.GetProcessesByName("Dolphin"))
                    {
                        p.Kill();
                    }
                }
               
                System.Diagnostics.Process.Start(Settings1.Default.DolphinPath, '"' + Settings1.Default.EMPath + '"');
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //mod installer
            Form f = new Form3();
            f.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //level loader

            Form f = new Form1();
            f.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://memerdev.com",
                UseShellExecute = true
            });
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
           
        }

        private void Form2_Enter(object sender, EventArgs e)
        {
            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form5 f = new Form5();
            f.MainForm = this;
            f.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if(File.Exists("emlupdate.exe"))
            {
                Process.Start("emlupdate.exe");
            }
            else
            {
                MessageBox.Show("Can't update - Update client not found");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form6 f = new Form6();
            f.Show();
        }
    }
}
