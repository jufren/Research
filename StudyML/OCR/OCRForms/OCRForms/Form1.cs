using Tesseract;

namespace OCRForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            txtFile.Text = openFileDialog1.FileName;
            Pix p = Pix.LoadFromFile(txtFile.Text);
            //sBitmap image = new Bitmap(txtFile.Text);
            TesseractEngine ocr = new TesseractEngine("./tessdata-main", "chi_sim", EngineMode.Default);
            Page page = ocr.Process(p);
            string result = page.GetText();
            textBox2.Text = result;
        }
    }
}