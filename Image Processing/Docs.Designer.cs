
namespace Image_Processing
{
    partial class Docs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Docs));
            this.axAcroPDF1 = new AxAcroPDFLib.AxAcroPDF();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.docsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowPasAndHigthPassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.meanFiltersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orderStatisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.segmintationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // axAcroPDF1
            // 
            this.axAcroPDF1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axAcroPDF1.Enabled = true;
            this.axAcroPDF1.Location = new System.Drawing.Point(0, 24);
            this.axAcroPDF1.Name = "axAcroPDF1";
            this.axAcroPDF1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axAcroPDF1.OcxState")));
            this.axAcroPDF1.Size = new System.Drawing.Size(784, 596);
            this.axAcroPDF1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.docsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // docsToolStripMenuItem
            // 
            this.docsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lowPasAndHigthPassToolStripMenuItem,
            this.meanFiltersToolStripMenuItem,
            this.orderStatisticsToolStripMenuItem,
            this.segmintationToolStripMenuItem});
            this.docsToolStripMenuItem.Name = "docsToolStripMenuItem";
            this.docsToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.docsToolStripMenuItem.Text = "Docs";
            // 
            // lowPasAndHigthPassToolStripMenuItem
            // 
            this.lowPasAndHigthPassToolStripMenuItem.Name = "lowPasAndHigthPassToolStripMenuItem";
            this.lowPasAndHigthPassToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.lowPasAndHigthPassToolStripMenuItem.Text = "Low pas and Higth Pass";
            this.lowPasAndHigthPassToolStripMenuItem.Click += new System.EventHandler(this.lowPasAndHigthPassToolStripMenuItem_Click);
            // 
            // meanFiltersToolStripMenuItem
            // 
            this.meanFiltersToolStripMenuItem.Name = "meanFiltersToolStripMenuItem";
            this.meanFiltersToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.meanFiltersToolStripMenuItem.Text = "Mean Filters";
            this.meanFiltersToolStripMenuItem.Click += new System.EventHandler(this.meanFiltersToolStripMenuItem_Click);
            // 
            // orderStatisticsToolStripMenuItem
            // 
            this.orderStatisticsToolStripMenuItem.Name = "orderStatisticsToolStripMenuItem";
            this.orderStatisticsToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.orderStatisticsToolStripMenuItem.Text = "Order-Statistics";
            this.orderStatisticsToolStripMenuItem.Click += new System.EventHandler(this.orderStatisticsToolStripMenuItem_Click);
            // 
            // segmintationToolStripMenuItem
            // 
            this.segmintationToolStripMenuItem.Name = "segmintationToolStripMenuItem";
            this.segmintationToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.segmintationToolStripMenuItem.Text = "Segmintation";
            this.segmintationToolStripMenuItem.Click += new System.EventHandler(this.segmintationToolStripMenuItem_Click);
            // 
            // Docs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 620);
            this.Controls.Add(this.axAcroPDF1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Docs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Docs";
            this.Load += new System.EventHandler(this.Docs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axAcroPDF1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private AxAcroPDFLib.AxAcroPDF axAcroPDF1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem docsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowPasAndHigthPassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem meanFiltersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orderStatisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem segmintationToolStripMenuItem;
    }
}