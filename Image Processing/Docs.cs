using System;
using System.Windows.Forms;

namespace Image_Processing
{
    public partial class Docs : Form
    {
        public Docs()
        {
            InitializeComponent();
        }

        private void Docs_Load(object sender, EventArgs e)
        {
           
          
          axAcroPDF1.LoadFile(@"f:\C#Ado\Image Processing\Image Processing\Resources\4_5942682987454794126.pdf");

            
           
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void segmintationToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
                  axAcroPDF1.LoadFile(@"f:\C#Ado\Image Processing\Image Processing\Resources\4_5949707380697598575.pdf");
            MessageBox.Show("Segmentation Docs", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lowPasAndHigthPassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axAcroPDF1.LoadFile(@"D:\C#Ado\Image Processing\Image Processing\Resources\4_5942682987454794126.pdf");
            MessageBox.Show("Low pass and High pass Docs", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void meanFiltersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void orderStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
