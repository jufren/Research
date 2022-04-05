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

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult r=openFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                textBox1.Text=openFileDialog1.FileName;
                pictureBox1.Image = new System.Drawing.Bitmap(textBox1.Text);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FileInfo fi = new FileInfo(@textBox1.Text);
           
            string oriFileName = Path.GetFileNameWithoutExtension(textBox1.Text);
            string newFileName = oriFileName + "a" + fi.Extension;
            string newFilePath = @textBox1.Text.Replace(oriFileName + fi.Extension, newFileName);
            bmp.Save(newFilePath);
            MessageBox.Show("New Image saved " + newFilePath);
        }
        
        private void btnAutoRotate_Click(object sender, EventArgs e)
        {

            
            bmp= new System.Drawing.Bitmap(textBox1.Text);
            pictureBox1.Image = FixImageOrientation(@textBox1.Text, bmp);            
            
        }
        public Bitmap FixImageOrientation(string filename, Bitmap srce)
        {
            const int ExifOrientationId = 0x112;
            // Read orientation tag
            if (!srce.PropertyIdList.Contains(ExifOrientationId)) return srce;
            var prop = srce.GetPropertyItem(ExifOrientationId);
            var orient = BitConverter.ToInt16(prop.Value, 0);
            // Force value to 1
            prop.Value = BitConverter.GetBytes((short)1);
            srce.SetPropertyItem(prop);
            
            // Rotate/flip image according to <orient>
            switch (orient)
            {
                case 1:
                    srce.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    return srce;


                case 2:
                    srce.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    return srce;

                case 3:
                    srce.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    return srce;

                case 4:
                    srce.RotateFlip(RotateFlipType.Rotate180FlipX);
                    return srce;

                case 5:
                    srce.RotateFlip(RotateFlipType.Rotate90FlipX);
                    return srce;

                case 6:
                    srce.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return srce;

                case 7:
                    srce.RotateFlip(RotateFlipType.Rotate270FlipX);
                    return srce;

                case 8:
                    srce.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    return srce;

                default:
                    srce.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                    return srce;
            }
        }
    }
}
