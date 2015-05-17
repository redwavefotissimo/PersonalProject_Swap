using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Swap
{
    public partial class Form1 : Form
    {
        private gamemechanics gm; // call the gamemechanics class
        private Panel pnl; // call the panel class
        private ComboBox dropdown; // call the combobox class
        private Button[] btn = new Button[2]; // create array of button
        private Label[] lbl = new Label[2]; // create array of label
        private Timer tmr = new Timer(); // call the timer class
        private int size = 0; // the number of tiles (n x n)
        private bool hasstarted; // to know if the game has started
        private int x1 = -1, y1 = -1, x2 = -1, y2 = -1; // variables for mouse click
        private int X1 = -1, Y1 = -1; // variables for mouse down
        private int timecnt = 0, minute = 0, seconds = 0; // variables to time/timer

        public Form1()
        {
            InitializeComponent();
            crtcntrl();
        }

        private void crtcntrl() // create controls and the class gamemechanics
        {
            #region create panel
            pnl = new Panel();
            pnl.Size = new Size(300, 300);
            pnl.Location = new Point(10,10);
            pnl.BorderStyle = BorderStyle.FixedSingle;
            pnl.Paint += new PaintEventHandler(pnl_Paint);
            pnl.MouseClick += new MouseEventHandler(pnl_MouseClick);
            pnl.MouseUp += new MouseEventHandler(pnl_MouseUp);
            pnl.MouseDown += new MouseEventHandler(pnl_MouseDown);
            this.Controls.Add(pnl);
            #endregion

            #region create combobox
            dropdown = new ComboBox(); // call combobox
            dropdown.FormattingEnabled = true;
            dropdown.Size = new Size(121, 21);
            dropdown.Location = new Point(315, 133);
            dropdown.Font = new Font("Times New Roman", 15F, FontStyle.Regular);
            this.Controls.Add(dropdown);

            dropdown.Items.AddRange(new object[] {
                2,
                3,
                4,
                5,
                6,
                10
            });

            dropdown.SelectedItem = 3;
            #endregion

            #region create button
            for (int i = 0; i < btn.Length; i++)
            {
                btn[i] = new Button(); // call button class
                btn[i].Size = new Size(114, 45);
                btn[i].Font = new Font("Times New Roman",15F , FontStyle.Regular);
                switch (i)
                {
                    case 0: btn[i].Text = "Start";
                            btn[i].Location = new Point(318, 170);
                            btn[i].Click +=new EventHandler(start_click);
                        break;
                    case 1: btn[i].Text = "Quit";
                            btn[i].Location = new Point(318, 230);
                            btn[i].Click += new EventHandler(quit_click);
                        break;
                }
                this.Controls.Add(btn[i]);
            }
            #endregion

            #region create label
            for (int i = 0; i < lbl.Length; i++)
            {
                lbl[i] = new Label(); // call the label class
                lbl[i].Size = new Size(120, 25);
                switch (i)
                {
                    case 0: lbl[i].Text = timecnt.ToString();
                            lbl[i].Location = new Point(380, 35);
                            lbl[i].Font = new Font("Times New Roman", 20F, FontStyle.Regular);
                        break;
                    case 1: lbl[i].Text = "Please select field size";
                            lbl[i].Location = new Point(315, 100);
                            lbl[i].Font = new Font("Times New Roman", 10F, FontStyle.Bold);
                        break;
                }
                this.Controls.Add(lbl[i]);
            }
            #endregion

            #region create timer
            tmr.Interval = 1000;
            tmr.Enabled = true;
            tmr.Stop();
            tmr.Tick +=new EventHandler(tmr_Tick);
            #endregion
        }

        private void drawonpanel(Graphics g)
        {
            if (size != 0)
            {
                if (hasstarted != true)
                { gm.drawimage(size, g, false); hasstarted = true; }
                else
                { gm.drawimage(size, g, true); }
            }
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            timecnt++;
            lbl[0].Text = timecnt.ToString();
        }

        private void start_click(object sender, EventArgs e)
        {
            size = (int) dropdown.SelectedItem;
            gm = new gamemechanics(size);
            hasstarted = false;
            timecnt = 0;
            tmr.Start();
            pnl.Refresh();
            btn[0].Enabled = false;
            dropdown.Enabled = false;
        }

        private void quit_click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pnl_Paint(object sender, PaintEventArgs e)
        {
            drawonpanel(e.Graphics);
        }

        private void pnl_MouseDown(object sender, MouseEventArgs e)
        {
            if (X1 == -1 && Y1 == -1)
            {
                X1 = e.X / (300 / size); Y1 = e.Y / (300 / size); 
                gm.imgclicked(X1, Y1, true);
            }
            else
            {
                gm.imgclicked(X1, Y1, false);
                X1 = -1; Y1 = -1;
            }
        }

        private void pnl_MouseClick(object sender, MouseEventArgs e)
        {
            if (x1 == -1 && y1 == -1)
            { 
                x1 = e.X / (300 / size); y1 = e.Y / (300 / size); 
            }
            else
            {
                x2 = e.X / (300 / size); y2 = e.Y / (300 / size);
                gm.swap(x1, y1, x2, y2, size);
                x1 = -1; y1 = -1; x2 = -1; y2 = -1;
            }
            pnl.Refresh();
        }

        private void pnl_MouseUp(object sender, MouseEventArgs e)
        {
            if (gm.check(size) == true)
            {
                tmr.Stop();
                minute = timecnt / 60;
                seconds = timecnt % 60;
                if (MessageBox.Show("Puzzle Solved! at: " + minute + ":" + seconds, "Play again?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    btn[0].Enabled = true;
                    dropdown.Enabled = true;
                }
                else
                { this.Close(); }
            }
        }
    }
}
