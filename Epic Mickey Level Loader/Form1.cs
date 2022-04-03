//Created by a complete dumbass who has no idea how levels work in EM
//memer#1024


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Epic_Mickey_Level_Loader
{
    public partial class Form1 : Form
    {
        public string currentLevel = "Levels/Main_Menu.level";
        public string savePath;

        public List<string> favourites = new List<string>();

        public static Form1 instance;
        

        public Form1()
        {
            instance = this;
            InitializeComponent();
            Form5.ChangeTheme(this.Controls, this, Settings1.Default.DarkMode);
            if (Settings1.Default.Favourites != "")
            {
                string all = Settings1.Default.Favourites;

                string[] array = all.Split("\n");

                foreach(string s in array)
                {
                    listBox2.Items.Add(s);
                }
            }
        if(Settings1.Default.cmdline == "")
            {
                OpenFileDialog file = new OpenFileDialog();
                file.ShowDialog();
                savePath = file.FileName;
                Settings1.Default.cmdline = savePath;
                Settings1.Default.Save();
            }
            else
            {
                if(File.Exists(Settings1.Default.cmdline) && Settings1.Default.cmdline.EndsWith("cmdline.txt"))
                {
                    savePath = Settings1.Default.cmdline;
                }
                else
                {
                    OpenFileDialog file = new OpenFileDialog();
                    file.ShowDialog();
                    savePath = file.FileName;
                    Settings1.Default.cmdline = savePath;
                    Settings1.Default.Save();
                }
               
            }
          

            if(!savePath.EndsWith("cmdline.txt"))
            {
                if(MessageBox.Show("Name of file is not cmdline.txt. Proceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    Environment.Exit(0);
                }
            }
        }

        void Save()
        {
            string allContent = File.ReadAllText(savePath);
            try
            {
                string[] toArray = allContent.Split(' ');
                toArray[0] = currentLevel;
                allContent = string.Join(" ", toArray);

                allContent = allContent.Replace("-Set PlayerEnableAllAbilities=false", "-Set PlayerEnableAllAbilities=true");
                File.WriteAllText(savePath, allContent);
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occured while saving cmdline: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void SaveFavourites()
        {
            string save = string.Join("\n", favourites.ToArray());
            Settings1.Default.Favourites = save;
            Settings1.Default.Save(); 
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.Items[listBox1.SelectedIndex].ToString() != null)
            {
                currentLevel = listBox1.Items[listBox1.SelectedIndex].ToString();
                label3.Text = "Current Selected Level: " + currentLevel;
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                return;
            }
            if (listBox1.Items[listBox1.SelectedIndex].ToString() != null)
            {
                if(!listBox2.Items.Contains(listBox1.Items[listBox1.SelectedIndex].ToString()))
                {
                    listBox2.Items.Add(listBox1.Items[listBox1.SelectedIndex].ToString());
                    favourites.Add(listBox1.Items[listBox1.SelectedIndex].ToString());
                }
            }
            SaveFavourites();
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null || listBox2.SelectedIndex == -1) 
            {
                return;
            }
            if (listBox2.Items[listBox2.SelectedIndex].ToString() != null)
            {
                string s = listBox2.Items[listBox2.SelectedIndex].ToString();
                favourites.Remove(s);
                listBox2.Items.Remove(s);  
            }
      
            SaveFavourites();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Title = "Main.dol Path";
            file.ShowDialog();
            if (file.FileName.EndsWith("main.dol"))
            {
                Settings1.Default.EMPath = file.FileName;
                Settings1.Default.Save();
            }
            else
            {
                MessageBox.Show("Selected file is not main.dol", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            file = new OpenFileDialog();
            file.Title = "Dolphin.exe Path";
            file.ShowDialog();
            Settings1.Default.DolphinPath = file.FileName;
            Settings1.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveAndPlay();
        }

        void SaveAndPlay()
        {
            if (Settings1.Default.EMPath == "" || Settings1.Default.DolphinPath == "")
            {
                MessageBox.Show("Main.dol path has not been set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Save();
            if(checkBox2.Checked)
            {
                foreach (Process p in Process.GetProcessesByName("Dolphin"))
                {
                    p.Kill();
                }
            }
            
            Process.Start(Settings1.Default.DolphinPath, '"' + Settings1.Default.EMPath + '"');
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null || listBox2.SelectedIndex == -1)
            {
                return;
            }
            if (listBox2.Items[listBox2.SelectedIndex].ToString() != null)
            {
                currentLevel = listBox2.Items[listBox2.SelectedIndex].ToString();
                label3.Text = "Current Selected Level: " + currentLevel;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            savePath = file.FileName;
            Settings1.Default.cmdline = savePath;
            Settings1.Default.Save();
        }

        void SetLevel(string path)
        {
            currentLevel = path;
            label3.Text = "Current Selected Level: " + currentLevel;

            if(checkBox1.Checked)
            {
                SaveAndPlay();
            }
        }

        //mamamia christ this is not gonna look good

        private void button30_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MJM_ZoneA.Level");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MeanStreet_V1.Level");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MeanStreet_V2.Level");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MeanStreet_V3.Level");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MeanStreet_V4.Level");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MeanStreet_V5.Level");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MeanStreet_V6.Level");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/ToonTown_V1.Level");
        }

        private void button19_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/ToonTown_V2.Level");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/ToonTown_V3.Level");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/DBC_Start_1st.Level");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/DBC_Start.Level");
        }

        private void button25_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/DBC_Start_ZoneC.Level");
        }

        private void button28_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/DBC_Start_ZoneF.Level");
        }

        private void button29_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/Main_Menu.level");
        }

        private void button31_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MJM_ZoneB.Level");
        }

        private void button32_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MJM_ZoneC.Level");
        }

        private void button33_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MJM_ZoneE.Level");
        }

        private void button34_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MJM_ZoneG.Level");
        }

        private void button35_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/MJM_Zonef.Level");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/SW_ClockWorkBoss_static.Level");
        }

        private void button22_Click(object sender, EventArgs e)
        {
              SetLevel("Levels/TL_SpaceMountain_TronPete.Level");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/PotW_JollyRoger_Start.Level");
        }

        private void button24_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/ShadowBlot_Boss.Level");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/Adventureland_v1.Level");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/Adventureland_v2_Pirates.Level");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/Adventureland_v2_NoPirates.Level");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/Adventureland_v3.Level");
        }

        private void button15_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/NewOrleans_V1.Level");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/NewOrleans_V2.Level");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/NewOrleans_V3.Level");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2.onChange += Init;
        }

        void Init(object sender, EventArgs e)
        {
            Form5.ChangeTheme(this.Controls, this, Settings1.Default.DarkMode);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/GV_ZoneA_Start.level");
        }

        private void button39_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/GV_ZoneB_Start.level");
        }

        private void button40_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/GV_ZoneC_Start.level");
        }

        private void button41_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/GV_ZoneD_Start.level");
        }

        private void button42_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/GV_ZoneF_Start.level");
        }

        private void button43_Click(object sender, EventArgs e)
        {
            SetLevel("Levels/GV_ZoneI_Start.level");
        }
    }
}
