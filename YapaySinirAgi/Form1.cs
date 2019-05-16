using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YapaySinirAgi
{
    public partial class Form1 : Form
    {
        private Color bColor = Color.Black;
        private Color bColor2 = Color.White;
        private YSACore ysaCore = new YSACore(new[] {35, 100, 5});

        private Button tempBtn;

        public Form1()
        {
            InitializeComponent();
        }

        void InitializeYSA(int[] katmanlar)
        {
            ysaCore = new YSACore(katmanlar);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void ClickToggle(Control c)
        {
            if (c.BackColor == bColor2)
                c.BackColor = bColor;
            else
                c.BackColor = bColor2;
        }

        private void ClickToggle(Control c, bool isSelected)
        {
            if (isSelected)
                c.BackColor = bColor;
            else
                c.BackColor = bColor2;
        }

        private void ClickClear()
        {
            foreach (Button btn in flowLayoutPanel1.Controls.OfType<Button>())
            {
                ClickToggle(btn, false);
            }
        }

        private void FlowLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            var buttons = flowLayoutPanel1.Controls.OfType<Button>();
            var points = buttons.Select(a => a.PointToClient(Cursor.Position)).ToList();
            var rectanges = buttons.Select(a => a.ClientRectangle).ToList();

            if (rectanges.Any(a => points.Any(b => a.Contains(b))))
            {
                var btn = buttons.Where(a => a.ClientRectangle.Contains(a.PointToClient(Cursor.Position)))
                    .FirstOrDefault();
                if (btn != null && btn != tempBtn)
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

        private void Button38_Click(object sender, EventArgs e)
        {
            float[] cıktı = HarfToFloat(textBox1.Text.ToUpper());
            ysaCore.BackPropagation(cıktı);
        }

        public float[] TabloToFloat()
        {
            float[] f = flowLayoutPanel1.Controls
                .OfType<Button>()
                .Select(a => a.BackColor == Color.Black ? 1f : 0f)
                .ToArray();
            return f;
        }

        public float[] HarfToFloat(string harf)
        {
            float[] yeniOutput = {0, 0, 0, 0, 0};
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

        public void HarfToPattern(string harf)
        {
            string[] HarfPatterns =
            {
                "0,0,1,0,0,0,1,0,1,0,1,0,0,0,1,1,0,0,0,1,1,1,1,1,1,1,0,0,0,1,1,0,0,0,1", //  A
                "1,1,1,1,0,1,0,0,0,1,1,0,0,0,1,1,1,1,1,0,1,0,0,0,1,1,0,0,0,1,1,1,1,1,0", //  B
                "0,1,1,1,1,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,0,1,1,1,1", //  C
                "1,1,1,0,0,1,0,0,1,0,1,0,0,0,1,1,0,0,0,1,1,0,0,0,1,1,0,0,1,0,1,1,1,0,0", //  D
                "1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,1,1,1,1,1" //   E
            };

            int letterNo = 0;
            switch (harf.ToUpper())
            {
                case "A":
                    letterNo = 0;
                    break;
                case "B":
                    letterNo = 1;
                    break;
                case "C":
                    letterNo = 2;
                    break;
                case "D":

                    letterNo = 3;
                    break;
                case "E":
                    letterNo = 4;
                    break;
            }

            var buttons = flowLayoutPanel1.Controls.OfType<Button>().ToArray();
            Color[] colorStatus = HarfPatterns[letterNo].Split(',').Select(a => a == "1" ? Color.Black : Color.White)
                .ToArray();

            for (int i = 0; i < buttons.Count(); i++)
                buttons[i].BackColor = colorStatus[i];
        }

        public void SonucPrint(float[] vars)
        {
            int i = 1;

            foreach (float f in vars)
            {
                Label lbl = (Label) groupBox2.Controls["label" + i];
                lbl.Text = f.ToString();

                float oran = f < 0 ? -(1f - f) : 1;
                double yüzde = f * 100f / oran;
                chart1.Series[0].Points[i - 1].YValues = new[] {yüzde};
                i++;
            }
        }

        private void Button37_Click(object sender, EventArgs e)
        {
            float[] sonuc = ysaCore.ForwardPropagation(TabloToFloat());
            SonucPrint(sonuc);
        }

        private async void Button39_Click(object sender, EventArgs e)
        {
            float[] tabloArray = TabloToFloat();
            float[] cıktı = HarfToFloat(textBox1.Text.ToUpper());
            for (int i = 0; i < (int) numericUpDown1.Value; i++)
            {
                float[] sonuc = ysaCore.ForwardPropagation(tabloArray);
                SonucPrint(sonuc);
                ysaCore.BackPropagation(cıktı);
                await Task.Delay(1);
            }
        }

        private void NumericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            YSACore.hataKatsayı = (float) numericUpDown2.Value;
        }

       

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.ToUpper();
            if (!string.IsNullOrEmpty(textBox1.Text) &&
                new[] {"A", "B", "C", "D", "E"}.Any(a => a == textBox1.Text.Substring(0, 1)))
            {
                textBox1.Text = textBox1.Text.Substring(0, 1);
            }
            else
            {
                textBox1.Text = "";
            }
        }

        private void SetPattern_MouseClick(object sender, EventArgs e)
        {
            string letter = ((Button) sender).Text;
            HarfToPattern(letter);
            textBox1.Text = letter;
        }


        private void GetIntArrayFromPattern()
        {
            int[] array = flowLayoutPanel1.Controls.OfType<Button>().Select(a =>
            {
                return a.BackColor == Color.Black ? 1 : 0;
            }).ToArray();
            string output = string.Join(",", array);
        }

      

        private void Button44_Click(object sender, EventArgs e)
        {
            KatmanEkle(numericUpDown3.Text);
            int[] katmanlar = KatmanToArray();
            InitializeYSA(katmanlar);

        }

        int[] KatmanToArray()
        {
            return flowLayoutPanel2.Controls
                .OfType<Button>()
                .Where(a => a.Text != "→")
                .Select(a => int.Parse(a.Text))
                .ToArray();
        }
        void KatmanEkle(string miktar)
        {
            
            Button btn = buttonBase.Clone();
            btn.FlatAppearance.BorderColor = Color.DeepSkyBlue;
            btn.FlatAppearance.BorderSize = 2;
            btn.Text = miktar;
            Button arrowButton=buttonArrow.Clone();
            arrowButton.FlatAppearance.BorderSize = 0;
            arrowButton.Text = "→";
            flowLayoutPanel2.Controls.Add(arrowButton);
            flowLayoutPanel2.Controls.Add(btn);
            int total = flowLayoutPanel2.Controls.Count-1;
            flowLayoutPanel2.Controls.SetChildIndex(arrowButton,total-2);
            flowLayoutPanel2.Controls.SetChildIndex(btn,total-2);

        }

        void SonKatmanıSil()
        {
            var katmanlar =flowLayoutPanel2.Controls.OfType<Button>().Where(a => a.FlatAppearance.BorderColor != Color.DarkOrange);
            katmanlar.Reverse().Take(2).ToList().ForEach( a=>flowLayoutPanel2.Controls.Remove(a));
            katmanlar =flowLayoutPanel2.Controls.OfType<Button>().Where(a => a.Text=="→");
            if (katmanlar.Count()==2 && flowLayoutPanel2.Controls.Count==4)
            {
                var btn =katmanlar.First();
                flowLayoutPanel2.Controls.Remove(btn);
            }
        }
        private void Button45_Click(object sender, EventArgs e)
        {
            SonKatmanıSil();
        }

        private void Button46_Click(object sender, EventArgs e)
        {
            int[] katmanlar = KatmanToArray();
            InitializeYSA(katmanlar);
        }
    }

    public static class ControlExtensions
    {
        public static T Clone<T>(this T controlToClone) 
            where T : Control
        {
            PropertyInfo[] controlProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            T instance = Activator.CreateInstance<T>();

            foreach (PropertyInfo propInfo in controlProperties)
            {
                if (propInfo.CanWrite)
                {
                    if(propInfo.Name != "WindowTarget")
                        propInfo.SetValue(instance, propInfo.GetValue(controlToClone, null), null);
                }
            }

            return instance;
        }
    }
}