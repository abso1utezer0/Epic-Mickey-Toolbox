﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Epic_Mickey_Level_Loader
{
    public partial class Form2 : Form
    {
        //random green quotes dedicated to mr slaycap (mr green)
        string[] s = { "'The grass is always greener on the other side'", "'It's that not easy being green'", "'Greeny-Game walked so Bluey-Game could run'" };

        public static Form2 instance;

        public EventHandler onChange;

        public static bool GameInstalled;
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
        void Init()
        {
            button2.Enabled = false;
            GameInstalled = false;
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
                FileName = "http://www.webpage.com",
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
    }
}
