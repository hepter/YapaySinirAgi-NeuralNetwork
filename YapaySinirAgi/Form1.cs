using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YapaySinirAgi
{

   
    public partial class Form1 : Form
    {
        Color bColor=Color.Black;
        Color bColor2 = Color.White;
   
        public Form1()
        {
            InitializeComponent();
        }

 

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }

        

        void ClickToggle(Control c)
        {
            if (c.BackColor==bColor2)
                c.BackColor = bColor;
            else
                c.BackColor = bColor2;
        }
        void ClickToggle(Control c,bool isSelected)
        {
            if (isSelected)
                c.BackColor = bColor;
            else
                c.BackColor = bColor2;
        }

        void ClickClear()
        {
            foreach (Button btn in flowLayoutPanel1.Controls.OfType<Button>())
            {
                ClickToggle(btn,false);
            }
        }

        private Button tempBtn;
        private void FlowLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
          
            var buttons =flowLayoutPanel1.Controls.OfType<Button>(); 
            var points =buttons.Select(a => a.PointToClient(Cursor.Position)).ToList();
            var rectanges = buttons.Select(a => a.ClientRectangle).ToList();

            if (rectanges.Any(a => points.Any(b => a.Contains(b))))
            {
                var btn= buttons.Where(a => a.ClientRectangle.Contains(a.PointToClient(Cursor.Position))).FirstOrDefault();
                if (btn!=null && btn!=tempBtn)
                {
                    if (e.Button == MouseButtons.Left)
                        ClickToggle(btn, true);
                    else if (e.Button == MouseButtons.Right)
                        ClickToggle(btn, false);
                    
                    //ClickToggle(btn);
                }
                tempBtn = btn;
            }

        }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void FlowLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
             if (e.Button == MouseButtons.Middle)
                ClickClear();
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

      

        private void Button38_Click(object sender, EventArgs e)
        {
            float[] cıktı= HarfToFloat(textBox1.Text.ToUpper());
            drv.BackPropagation(cıktı);
        }
        YSACore drv= new YSACore(new int[]{ 35, 35,35, 5  });
        public float[] TabloToFloat()
        {
            float[] f=flowLayoutPanel1.Controls
                .OfType<Button>()
                .Select(a=> a.BackColor == Color.Black ? 1f : 0f)
                .ToArray();
            return f;
        }

        public float[] HarfToFloat(string harf)
        {
            float[] yeniOutput = new float[] {0,0,0,0,0};
            switch (harf.ToUpper())
            {
                case "A":
                    yeniOutput[0] = 1;
                    break;
                case "B":
                    yeniOutput[1] = 1;
                    break;
                case "C":
                    yeniOutput[2] = 1;
                    break;
                case "D":

                    yeniOutput[3] = 1;
                    break;
                case "E":
                    yeniOutput[4] = 1;
                    break;
            }

            return yeniOutput;
        }

        public void SonucPrint(float[] vars)
        {
            int i = 1;

            foreach (float f in vars)
            {
                Label lbl = (Label) groupBox2.Controls["label" + i];
                lbl.Text = f.ToString();

                float oran = f < 0 ? -(1f - f): 1;
                double yüzde = (double)((f * 100f) / oran);
                chart1.Series[0].Points[i-1].YValues=new []{yüzde};
                i++;
            }

        }
        private void Button37_Click(object sender, EventArgs e)
        {
           float[] sonuc = drv.ForwardPropagation(TabloToFloat());
           SonucPrint(sonuc);

        }

        private async void Button39_Click(object sender, EventArgs e)
        {
            float[] tabloArray = TabloToFloat();
            float[] cıktı= HarfToFloat(textBox1.Text.ToUpper());
            for (int i = 0; i < (int)numericUpDown1.Value; i++)
            {
                float[] sonuc = drv.ForwardPropagation(tabloArray);
                SonucPrint(sonuc);
                drv.BackPropagation(cıktı);
                await Task.Delay(1);
            }
            
           
           
        }

        private void NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            YSACore.hataKatsayı = (float)numericUpDown2.Value;
        }

        private void Label13_Click(object sender, EventArgs e)
        {

        }

        private void Label12_Click(object sender, EventArgs e)
        {

        }

        private void Label11_Click(object sender, EventArgs e)
        {

        }

        private void Label10_Click(object sender, EventArgs e)
        {

        }

        private void Label14_Click(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.ToUpper();
            if (!string.IsNullOrEmpty(textBox1.Text) && new String[]{"A","B","C","D","E"}.Any(a=>a==textBox1.Text.Substring(0,1)))
            {
                textBox1.Text = textBox1.Text.Substring(0, 1);
                
            }
            else
            {
                textBox1.Text = "";
            }
        }

        private void Label15_Click(object sender, EventArgs e)
        {

        }
    }
}
