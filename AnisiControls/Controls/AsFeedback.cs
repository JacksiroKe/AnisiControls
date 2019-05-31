using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnisiControls
{
    public class AsFeedback : Control
    {
        public PictureBox act = new PictureBox();
        public Label msg = new Label();
        public AsButton clr = new AsButton();
        public Timer timer = new Timer();

        private float interval;
        private bool isPositive, isTimed;
        Color background;
        int intx = 35, inty = 0;

        public AsFeedback()
        {
            background = Color.LightGreen;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            
            this.act.Parent = this;
            this.msg.Parent = this;
            this.clr.Parent = this;
            this.Controls.Add(this.act);
            this.Controls.Add(this.msg);
            this.Controls.Add(this.clr);

            this.act.Size = new Size(22, 22);
            this.act.BackColor = Color.Transparent;
            this.act.Image = AnisiControls.Properties.Resources.info;
            this.act.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            this.msg.AutoSize = true;
            this.msg.BorderStyle = BorderStyle.None;
            this.msg.Font = this.Font;
            this.msg.ForeColor = Color.Black;
            this.msg.Size = new Size(35, 13);
            this.msg.TabIndex = 1;
            this.msg.Text = this.Text;

            this.clr.Text = "X";
            this.clr.Radius = 12;
            this.clr.AutoSize = true;
            this.clr.Font = new Font("Arial Rounded MT", 10, FontStyle.Bold);
            this.clr.Size = new Size(24, 24);
            this.clr.Active1 = Color.White;
            this.clr.Active2 = Color.Gray;
            this.clr.Inactive1 = Color.White;
            this.clr.Inactive2 = Color.LightGray;

            this.timer.Enabled = this.Visible;
            this.Text = null;
            this.Font = new Font("Trebuchet MS", 12);
            this.ForeColor = Color.Black;
            this.Size = new Size(300, 50);

            DoubleBuffered = true;
            IsPositive = true;
            IsTimed = false;

            this.timer.Interval = 2500;
            this.timer.Tick += timer_Tick;

            this.msg.TextChanged += msg_TextChanged;
            this.clr.Click += clr_onClick;
        }
        void timer_Tick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            this.Visible = false;
            Invalidate();
        }
        protected override Size DefaultSize
        {
            get
            {
                return new Size(400, 40);
            }
        }

        public bool IsPositive
        {
            get
            {
                return isPositive;
            }
            set
            {
                isPositive = value;
                if (isPositive)
                {
                    this.BackColor = Color.LightGreen;
                    background = Color.LightGreen;
                }
                else
                {
                    this.BackColor = Color.LightSalmon;
                    background = Color.LightSalmon;
                }
                Invalidate();
            }
        }
        public bool IsTimed
        {
            get
            {
                return isTimed;
            }
            set
            {
                isTimed = value;
                if (isTimed) timer.Enabled = true;
                else this.timer.Enabled = false;
                Invalidate();
            }
        }

        public float Interval
        {
            get
            {
                return this.timer.Interval;
            }
            set
            {
                interval = value;
                Invalidate();
            }
        }
        private void clr_onClick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            this.Visible = false;
        }


        void msg_TextChanged(object sender, EventArgs e)
        {
            this.Text = msg.Text;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.msg.Text = this.Text;
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            this.msg.Font = this.Font;
            Invalidate();
        }
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            this.msg.ForeColor = this.ForeColor;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            inty = (Height - this.msg.Font.Height) / 2;

            this.act.Location = new Point(5, Height / 4);
            this.msg.Location = new Point(intx, inty - 1);
            this.msg.Width = Width - (intx + 40);
            this.clr.Location = new Point(Width - 30, Height / 4);

            e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
            using (SolidBrush brush = new SolidBrush(background))
                e.Graphics.FillRectangle(brush, 1, 1, Width - 2, Height - 2);
            base.OnPaint(e);
        }
       
    }
}
