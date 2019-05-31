using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AnisiControls
{
    #region AsTextbox

    public class AsTextbox : Control
    {
        int radius = 3, irad = 12, intx = 35, inty = 0;

        public TextBox box = new TextBox();
        public AsButton clr = new AsButton();
        public PictureBox act = new PictureBox();
        GraphicsPath shape, innerRect;
        Color background;
        RoundedRectangleF roundedRect;

        private bool isSearch, showIcon;
        private string placeholder = string.Empty;
                
        public AsTextbox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            act.Parent = this;
            box.Parent = this;
            clr.Parent = this;
            Controls.Add(act);  
            Controls.Add(box);
            Controls.Add(clr);

            box.BorderStyle = BorderStyle.None;
            box.TextAlign = HorizontalAlignment.Left;
            box.Font = Font;
           
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            background = Color.Black;
            Text = null;
            Font = new Font("Trebuchet MS", 18);
            Size = new Size(220, 40);

            DoubleBuffered = true;
            IsSearch = false;
            ShowIcon = true;
            
            box.BackColor = Color.Black;
            box.ForeColor = Color.White;
            roundedRect = new RoundedRectangleF(Width, Height, irad);

            act.Size = new Size(22, 22);
            act.BackColor = Color.Transparent;
            act.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                        
            clr.Text = "X";
            clr.Radius = 12;
            clr.Font = new Font("Arial Rounded MT", 10, FontStyle.Bold);
            clr.Visible = false;
            clr.Size = new Size(24, 24);
            clr.Active1 = Color.White;
            clr.Active2 = Color.Gray;
            clr.Inactive1 = Color.White;
            clr.Inactive2 = Color.LightGray;

            box.TextChanged += box_TextChanged;
            box.MouseDoubleClick += box_MouseDoubleClick;
            clr.Click += clr_onClick;

            box.GotFocus += box_GotFocus;
            box.LostFocus += box_LostFocus;
            
        }

        private void Clear()
        {
            box.Clear();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    pressedEnter();
                    break;

                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }
        private void pressedEnter()
        {
            
        }


        protected override Size DefaultSize
        {
            get
            {
                return new Size(220, 40);
            }
        }

        public bool ShowIcon
        {
            get
            {
                return showIcon;
            }
            set
            {
                showIcon = value;
                if (showIcon)
                {
                    intx = 35;
                    act.Visible = true;
                }
                else
                {
                    intx = 5;
                    act.Visible = false;
                }
                Invalidate();
            }
        }
        public bool IsSearch
        {
            get
            {
                return isSearch;
            }
            set
            {
                isSearch = value;
                if (ShowIcon)
                {
                    if (isSearch) act.Image = AnisiControls.Properties.Resources.search;
                    else act.Image = AnisiControls.Properties.Resources.info;
                }               
                Invalidate();
            }
        }
        
        public string Placeholder
        {
            get
            {
                return placeholder;
            }
            set
            {
                placeholder = value;
                DefaultText();
            }
        }

        private void DefaultText()
        {
            if (this.Text.Trim().Length == 0) this.Text = Placeholder;
        }

        private void clr_onClick(object sender, EventArgs e)
        {
            box.Text = "";
        }

        private void box_GotFocus(object sender, EventArgs e)
        {
            if (box.Text.Equals(Placeholder)) box.Text = string.Empty;
        }
        private void box_LostFocus(object sender, EventArgs e)
        {
            DefaultText();
        }

        void box_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;
            box.SelectAll();
        }

        void box_TextChanged(object sender, EventArgs e)
        {
            Text = box.Text;
            if (Text == Placeholder) clr.Visible = false;
            else if (box.Text.Length !=0) clr.Visible = true;
            else clr.Visible = false;
        }
        public void SelectAll()
        {
            box.SelectAll();
        }
        
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            box.Text = Text;
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            box.Font = Font;
            Invalidate();
        }
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            box.ForeColor = ForeColor;
            Invalidate();
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
             
            shape = new RoundedRectangleF(Width, Height, radius).Path;
            innerRect = new RoundedRectangleF(Width - 0.5f, Height - 0.5f, radius, 0.5f, 0.5f).Path;
            if (box.Height >= Height - 5) Height = box.Height + 5;
            inty = (Height - box.Font.Height) / 2;
            act.Location = new Point(5, Height /4);

            box.Location = new Point(intx, inty - 1);
            box.Width = Width - (intx + 40);
            
            //clr.Location = new Point(intx + box.Width + 5, Height / 4);
            clr.Location = new Point(Width - 30, Height / 4);
                        
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            Bitmap bmp = new Bitmap(Width, Height);
            Graphics grp = Graphics.FromImage(bmp);
            e.Graphics.DrawPath(Pens.Gray, shape);
            using (SolidBrush brush = new SolidBrush(background)) e.Graphics.FillPath(brush, innerRect);
            Transparenter.MakeTransparent(this, e.Graphics);
            base.OnPaint(e);
        }
        public Color Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
                if (background != Color.Transparent) box.BackColor = background;
                Invalidate();
            }
        }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = Color.Transparent;
            }
        }
    }

    #endregion
}
