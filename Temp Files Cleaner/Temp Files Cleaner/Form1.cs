using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Temp_Files_Cleaner
{
    public partial class Form1 : Form
    {
        long tempfilesize = 0;
        public Form1()
        {
            
            InitializeComponent();
        }

        private static long DirectorySize(DirectoryInfo dInfo,bool SubDir)
        {
            long totalSize = dInfo.EnumerateFiles().Sum(file => file.Length);

            if(SubDir)
            {
                totalSize += dInfo.EnumerateDirectories().Sum(dir => DirectorySize(dir, true));
            }
            return totalSize;
        }
        static string Bytestostring (long bytecount)
        {
            string[] size = { "B", "KB", "MB", "GB", "TB" };

            if(bytecount == 0)
            {
                return "0" + size[0];
            }

            long bytes = Math.Abs(bytecount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));

            double num = Math.Round(bytes / Math.Pow(1024, place), 1);

            return (Math.Sign(bytecount) * num).ToString() + " " + size[place];
        }
    
        private void button1_Click(object sender, EventArgs e)
        {
            tempfilesize = 0;
            string TempPath = Path.GetTempPath();
            var Dir = new DirectoryInfo(TempPath);

            if(!Dir.Exists)
            {
                MessageBox.Show("Can't Find Temp Folder");
                return;
            }

            foreach (DirectoryInfo dir in Dir.GetDirectories())
            {
                try
                {
                    tempfilesize += DirectorySize(dir, true);
                    dir.Delete(true);
                }
                catch(Exception)
                {

                }
            }

            foreach(FileInfo filez in Dir.GetFiles())
            {
                try
                {
                    tempfilesize += filez.Length;
                    filez.Delete();
                }
                catch(Exception)
                {

                }
            }

            MessageBox.Show("Temporary files have been deleted!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            tempfilesize = 0;
            label1.Text = "Junk: " + tempfilesize + " KB";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tempfilesize = 0;
            string TempPath = Path.GetTempPath();

            var Dir = new DirectoryInfo(TempPath);

            if(!Dir.Exists)
            {
                MessageBox.Show("Can't find Temp folder");
                return;
            }
            foreach (DirectoryInfo dir in Dir.GetDirectories())
            {
                try
                {
                    tempfilesize += DirectorySize(dir, true);
                }

                catch (Exception)
                {
                    // Ignore folders that are locked or being used.
                }
            }
            foreach (FileInfo Filez in Dir.GetFiles())
            {
                try
                {
                    tempfilesize += Filez.Length;
                }

                catch (Exception)
                {
                    // Ignore files that are locked or being used.
                }
            }
            label1.Text = "Junk: " + Bytestostring(tempfilesize);
        }
    }
}
