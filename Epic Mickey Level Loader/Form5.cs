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

namespace Epic_Mickey_Level_Loader
{
    public partial class Form5 : Form
    {
        public Form2 MainForm;
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please assign your dump directory");
            FolderBrowserDialog fold = new FolderBrowserDialog();
            if(fold.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(fold.SelectedPath + "/DATA"))
                {
                    string emPath = fold.SelectedPath;
                    MessageBox.Show("Please assign your main.dol");
                    OpenFileDialog f = new OpenFileDialog();
                    if(f.ShowDialog() == DialogResult.OK)
                    {
                        if(f.SafeFileName == "main.dol")
                        {
                            string maindol = f.FileName;
                            MessageBox.Show("Please assign your cmdline.txt");
                            f = new OpenFileDialog();
                            if(f.ShowDialog() == DialogResult.OK)
                            {
                                if(f.SafeFileName == "cmdline.txt")
                                {
                                    string cmdline = f.FileName;

                                    Settings1.Default.EmDirectory = emPath;
                                    Settings1.Default.EMPath = maindol;
                                    Settings1.Default.cmdline = cmdline;
                                    Settings1.Default.Save();

                                    MainForm.onChange.Invoke(this, EventArgs.Empty);
                                    MessageBox.Show("Success!");
                                }
                                else
                                {
                                    MessageBox.Show("File is not called cmdline.txt");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("File is not called main.dol");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The assigned folder does not have the correct contents.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "Dolphin.exe Path";
            if(file.ShowDialog() == DialogResult.OK)
            {
                if(file.SafeFileName == "Dolphin.exe")
                {
                    Settings1.Default.DolphinPath = file.FileName;
                    Settings1.Default.Save();
                    MainForm.onChange.Invoke(this, EventArgs.Empty);
                    MessageBox.Show("Success!");
                }
                else
                {
                    MessageBox.Show("File name is not Dolphin.exe");
                }
               
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Settings1.Default.cmdline = "";
            Settings1.Default.DolphinPath = "";
            Settings1.Default.EmDirectory = "";
            Settings1.Default.EMPath = "";
            Settings1.Default.Favourites = "";
            Settings1.Default.Save();
            MainForm.onChange.Invoke(this, EventArgs.Empty);
        }
    }
}
