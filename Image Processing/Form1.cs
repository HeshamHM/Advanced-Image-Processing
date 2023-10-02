using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Image_Processing
{


    public partial class Form1 : Form
    {
        bool load = false;
        bool show = false;
        bool error = false;
        bool start = false;
        int acc;
        string type="";
        public Form1()
        {
            InitializeComponent();
        }

       
  
       
       
      
        private void guna2Button6_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void splitter2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            panel_order.Hide();
            panel_mean.Hide();
            panel_low.Hide();
            panel_higth.Show();
            panelFilters.Hide();
            panel_Edge.Hide();

        }

        private void sharpening_Frequency1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loading.Hide();
            label2.Text = "";
            panel_mean.Parent = panel1;
            panel_order.Parent = panel1;
            panel_low.Parent = panel1;
            panel_higth.Parent = panel1;
            panelFilters.Parent = panel1;
            panel_Edge.Parent = panel1;
            panel_higth.Hide();
            panel_mean.Hide();
            panel_order.Hide();
            panelFilters.Hide();
            panel_Edge.Hide();
           
        }
        public  void api(String st)
        {
            try
            {
                WebRequest request = WebRequest.Create(st);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string data = "hh";
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    data = reader.ReadToEnd();

                    reader.Close();
                }
                pic.BackgroundImage = (Bitmap)new ImageConverter().ConvertFrom(Convert.FromBase64String(data));
                pic.BackgroundImageLayout = ImageLayout.Zoom;
                //MessageBox.Show(data, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = false;
            }
            catch (Exception ex)
            {
               //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }
        }
        public void ApiClass(String st)
        {
            try
            {
                WebRequest request = WebRequest.Create(st);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string data = "hh";
                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    data = reader.ReadToEnd();

                    reader.Close();
                }
                //type=data.Split(',')[0];
                type = data;
                acc =96;
                error = false;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                error = true;
            }
        }
        public void apis(String st)
        {
            try
            {
                WebRequest request = WebRequest.Create(st);
                request.Method = "POST";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                error = false;
            }
            catch(Exception ex)
            {
                error = true;
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            panel_higth.Hide();
            panel_low.Show();
            panel_mean.Hide();
            panel_order.Hide();
            panelFilters.Hide();
            panel_Edge.Hide();


        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            panel_higth.Hide();
            panel_low.Hide();
            panel_mean.Show();
            panel_order.Hide();
            panelFilters.Hide();
            panel_Edge.Hide();


        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            panelFilters.Hide();
            panel_higth.Hide();
            panel_low.Hide();
            panel_order.Show();
            panel_mean.Hide();
            panel_Edge.Hide();

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            panelFilters.Show();
            panel_higth.Hide();
            panel_low.Hide();
            panel_mean.Hide();
            panel_order.Hide();
            panel_Edge.Hide();

        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Choose Image | *.jpg; *.jpeg; *.png; ";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                 api(" http://127.0.0.1:5000/file/" + fileDialog.FileName.Replace('/', '_').Replace('\\', '-') + "");
                    load = true;
                    if (error == false)
                            MessageBox.Show("Loaded", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    api(" http://127.0.0.1:5000/file/" + fileDialog.FileName.Replace('/', '_').Replace('\\', '-') + "");
                    load = true;
                    if (error == false)
                        MessageBox.Show("Loaded", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
            }

        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            Docs docs = new Docs();
            docs.Show();

        }

        private void order_Statistic1_Load(object sender, EventArgs e)
        {

        }

    
        private void btn_median_KeyDown(object sender, KeyEventArgs e)
        {
            //loading.Show();
        }

        private void btn_median_KeyUp(object sender, KeyEventArgs e)
        {
            //loading.Hide();
        }

        private void btn_median_MouseDown(object sender, MouseEventArgs e)
        {
         
        }

        private void btn_median_MouseUp(object sender, MouseEventArgs e)
        {
         
        }

        private void smoothing_Frequency1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void btn_median_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                loading.Show();
                show = true;
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Median");
                   
                });
                loading.Hide();
                show = false;
                if(error ==false)
                 MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                //MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void btn_max_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Max");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

              

            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void btn_min_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Min");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void btn_midpoint_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Mid");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void btn_alpha_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Alpha");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button8_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {

                    api("http://127.0.0.1:5000/Undo");

                });
                loading.Hide();
                show = false;

            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void btn_artith_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Mean");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void btn_geo_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Geo");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void btn_harmonic_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Har");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void btn_poscontra_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/PContrahar");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void btn_negtiveContra_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/NContrahar");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

      

        private async void guna2Button20_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Connected");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button23_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/MeanShift");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button19_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Kmeans");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button21_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Watershed");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button22_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Levelset");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        private async void guna2Button14_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() => { api("http://127.0.0.1:5000/snakeACWE"); });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void guna2Button26_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Ideallow");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button25_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Butterlow");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button24_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Gaussianlow");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button17_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/IdealH");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button16_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/butterH");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private async void guna2Button15_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/GaussianH");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btn_EdgeDetection_Click(object sender, EventArgs e)
        {
            panelFilters.Hide();
            panel_higth.Hide();
            panel_low.Hide();
            panel_mean.Hide();
            panel_order.Hide();
            panel_Edge.Show();


        }

        private async void guna2Button13_Click_1(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Sobel");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button28_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Canny");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button9_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Clear");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button29_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/Global");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button31_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/ostu");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button30_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() =>
                {
                    api("http://127.0.0.1:5000/adpaptive");

                });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private async void guna2Button11_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                string st = @"D:\dataset";
                show = true;
                loading.Show();
                await Task.Run(() => { apis("http://127.0.0.1:5000/files/"+st.Replace('/', '_').Replace('\\', '-') + ""); });
                loading.Hide();
                show = false;
            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
         }

        private async void guna2Button18_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() => { api("http://127.0.0.1:5000/Region"); });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void guna2Button12_Click(object sender, EventArgs e)
        {
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() => { api("http://127.0.0.1:5000/Chaincode"); });
                loading.Hide();
                show = false;
                if (error == false)
                    MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void guna2Button27_Click(object sender, EventArgs e)
        {
            guna2CircleProgressBar1.Value = 0;
            label2.Text = "";
            if (show == true)
            {
                MessageBox.Show("Wait for processing", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (load)
            {
                show = true;
                loading.Show();
                await Task.Run(() => { ApiClass("http://127.0.0.1:5000/Classification"); });
                loading.Hide();
                show = false;
                start = true;
                //if (type == "0")
                //    label2.Text = "Has brain tumor";
                //else
                //    label2.Text = "Hasnot brain tumor";
                label2.Text = type;
             }
            else
            {
                MessageBox.Show("Browes for an image", "Erorr", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(start)
                guna2CircleProgressBar1.Increment(1);
            if (guna2CircleProgressBar1.Value == acc )
            {   
                start = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }


}
