using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using HexMate;
using System.Windows.Forms;

namespace Epic_Mickey_Level_Loader
{
    public partial class Form6 : Form
    {
        public EventHandler finish;
        public Form6()
        {
   
            InitializeComponent();
        }
        public static byte[] StrToByteArray(string str)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
                hexindex.Add(i.ToString("X2"), (byte)i);

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
                hexres.Add(hexindex[str.Substring(i, 2)]);

            return hexres.ToArray();
        }
        private void button1_Click(object sender, EventArgs e)
        {



            if(Directory.Exists("AUDIOMOD"))
            {
                Directory.Delete("AUDIOMOD", true);
            }

            Directory.CreateDirectory("AUDIOMOD");
            Directory.CreateDirectory("AUDIOMOD/even");
            Directory.CreateDirectory("AUDIOMOD/odd");
            Directory.CreateDirectory("AUDIOMOD/files");
            OpenFileDialog f = new OpenFileDialog();
            f.ShowDialog();

            string dsp = f.FileName;

           // f.ShowDialog();

            int originalFilesize; // = BitConverter.ToString(File.ReadAllBytes(f.FileName)).Replace("-", "").Length;

            byte[] replacing = Properties.Resources.header;

            byte[] dspByte = File.ReadAllBytes(dsp);

            byte[] monoData = ReadHex(dspByte, 0x7C, 92 / 2);

            File.WriteAllBytes("AUDIOMOD/monodata.txt", monoData);

            Directory.CreateDirectory("AUDIOMOD/files");
            int count = 1;

            byte[] header = ReadHex(replacing, 0x00, 512/2);

            header = WriteToHex(header, 0xB6, monoData);

            string toHexBytes = BitConverter.ToString(header).Replace("-", "");

            int lastIndex = 0;
            SplitFile(dsp, 16384, "AUDIOMOD/files/", toHexBytes, 0); //originalFileSize;
            lastIndex++;
            count++;
           
        }
        public static bool IsEvenNumber(int value)
        {
            return value % 2 == 0;
        }
        private void Form6_Load(object sender, EventArgs e)
        {
            Form5.ChangeTheme(this.Controls, this, Settings1.Default.DarkMode);
            Form2.onChange += Init;
        }
        void Init(object sender, EventArgs e)
        {
            Form5.ChangeTheme(this.Controls, this, Settings1.Default.DarkMode);
        }

        byte[] WriteToHex(byte[] b, int offset, byte[] towrite)
        {
            File.WriteAllBytes("TEMP", b);

            using (var read = File.Open("TEMP", FileMode.Open))
            {
                using (BinaryWriter bw = new BinaryWriter(read))
                {
                    bw.Seek(offset, SeekOrigin.Begin);

                    bw.Write(towrite);
                }
                byte[] by = File.ReadAllBytes("TEMP");
                File.Delete("TEMP");
                return by;
            }
        }

        byte[] ReadHex(byte[] b, int offset, int length)
        {
            using (MemoryStream s = new MemoryStream(b))
            {
                using (BinaryReader br = new BinaryReader(s))
                {
                    br.BaseStream.Position = offset;
                   
                    return br.ReadBytes(length);
                }
            }
        }
        string StackFiles(string[] files)
        {
            string toReturn = "";
            foreach(string f in files)
            {
                toReturn += File.ReadAllText(f);
            }
            return toReturn;
        }
        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }
        public void SplitFile(string inputFile, int chunkSize, string path, string header, int fileSizeOriginal)
        {
            const int BUFFER_SIZE = 20 * 1024;
            byte[] buffer = new byte[BUFFER_SIZE];
            int index = 1;

            List<string> filOdd = new List<string>();
            List<string> filEven = new List<string>();
            //OpenFileDialog f = new OpenFileDialog();
            //f.Multiselect = true;
            //f.ShowDialog();


            //for (int i = 0; i < f.FileNames.Length; i++)
            //{
            //    File.Copy(f.FileNames[i], "c:/superbaby/files/" + f.SafeFileNames[i], true);
            //}


            File.WriteAllText("READING", BitConverter.ToString(File.ReadAllBytes(inputFile)).Replace("-", ""));
            Thread.Sleep(500);

            using (Stream input = File.OpenRead("READING"))
            {
                while (input.Position < input.Length)
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        int remaining = chunkSize, bytesRead;
                        while (remaining > 0 && (bytesRead = input.Read(buffer, 0,
                                Math.Min(remaining, BUFFER_SIZE))) > 0)
                        {
                            output.Write(buffer, 0, bytesRead);
                            remaining -= bytesRead;
                        }
                        File.WriteAllBytes(path + "/" + index + ".txt", output.ToArray());
                        if(IsEven(index))
                        {
                            filEven.Add("AUDIOMOD/even/" + index + ".txt");

                            File.Move(path + "/" + index + ".txt", "AUDIOMOD/even/" + index + ".txt");

                        }
                        else
                        {
                            filOdd.Add("AUDIOMOD/odd/" + index + ".txt");

                            File.Move(path + "/" + index + ".txt", "AUDIOMOD/odd/" + index + ".txt");
                        }
                    }

        


                    index++;
                }
            }

            //Process.Start("c:/superbaby/movebatch.bat").WaitForExit();

            File.WriteAllText("AUDIOMOD/odd/odd.txt", StackFiles(filOdd.ToArray()));
            File.WriteAllText("AUDIOMOD/even/even.txt", StackFiles(filEven.ToArray()));

            string odd = Regex.Replace(File.ReadAllText("AUDIOMOD/odd/odd.txt"), "(.{" + 16 + "})", "$1" + Environment.NewLine);
            string even = Regex.Replace(File.ReadAllText("AUDIOMOD/even/even.txt"), "(.{" + 16 + "})", "$1" + Environment.NewLine);

            odd = odd.Replace(" ", "");

            even = even.Replace(" ", "");

            File.WriteAllText("AUDIOMOD/odd/odd.txt", odd);
            File.WriteAllText("AUDIOMOD/even/even.txt", even);

            Process.Start("int.bat").WaitForExit();

            string[] odds = odd.Split("\n");
            string[] evens = even.Split("\n");



            string fullDat = "";

            File.WriteAllText("AUDIOMOD/header.txt", header);



            fullDat += header;
            fullDat += File.ReadAllText("AUDIOMOD/interout.txt");

          

            fullDat = fullDat.Replace(Environment.NewLine, "");



            //if (fileSizeOriginal < fullDat.Length)
            //{
            //    fullDat = fullDat.Substring(0, fileSizeOriginal);
            //    MessageBox.Show("Your file is bigger than the one you are replacing! trimming...");
            //}
            //else
            //{
            //    string whitespace = new string('0', fileSizeOriginal - fullDat.Length);
            //    fullDat += whitespace;
            //}
            File.WriteAllText("OUT.txt", fullDat);
            Directory.Delete("AUDIOMOD", true);
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://apps.microsoft.com/store/detail/vgaudio/9NBLGGH4S2WN?hl=en-us&gl=US",
                UseShellExecute = true
            });

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
