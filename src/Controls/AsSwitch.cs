using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JacksiroKe.AnisiCtrls
{
    public class AsSwitch : Control
    {
        public delegate void SliderChangedEventHandler(object sender, EventArgs e);
        public event SliderChangedEventHandler SliderValueChanged;

        #region Variables
        float diameter;
        RoundedRectangleF rect;
        RectangleF circle;
        private bool isOn;
        float artis;
        private Color borderColor;
        private bool textEnabled;
        System.Windows.Forms.Timer paintTicker = new System.Windows.Forms.Timer();
        #endregion

        #region Properties
        public bool TextEnabled
        {
            get { return textEnabled; }
            set
            {
                textEnabled = value;
                Invalidate();
            }
        }
        public bool IsOn
        {
            get { return isOn; }
            set
            {
                paintTicker.Stop();
                isOn = value;
                paintTicker.Start();
                if (SliderValueChanged != null)
                    SliderValueChanged(this, EventArgs.Empty);
            }
        }
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                Invalidate();
            }
        }

        #endregion

        public AsSwitch()
        {

            Cursor = Cursors.Hand;
            DoubleBuffered = true;

            artis = 4; //increment for sliding animation
            diameter = 30;
            textEnabled = true;
            rect = new RoundedRectangleF(2 * diameter, diameter + 2, diameter / 2, 1, 1);
            circle = new RectangleF(1, 1, diameter, diameter);
            isOn = true;
            borderColor = Color.Black;

            paintTicker.Tick += paintTicker_Tick;
            paintTicker.Interval = 1;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }
        protected override void OnResize(EventArgs e)
        {
            Width = (Height - 2) * 2;
            diameter = Width / 2;
            artis = 4 * diameter / 30;
            rect = new RoundedRectangleF(2 * diameter, diameter + 2, diameter / 2, 1, 1);
            circle = new RectangleF(!isOn ? 1 : Width - diameter - 1, 1, diameter, diameter);
            base.OnResize(e);
        }
        //creates slide animation
        void paintTicker_Tick(object sender, EventArgs e)
        {
            float x = circle.X;

            if (isOn)           //switch the circle to the left
            {
                if (x + artis <= Width - diameter - 1)
                {
                    x += artis;
                    circle = new RectangleF(x, 1, diameter, diameter);

                    Invalidate();
                }
                else
                {
                    x = Width - diameter - 1;
                    circle = new RectangleF(x, 1, diameter, diameter);

                    Invalidate();
                    paintTicker.Stop();
                }

            }
            else //switch the circle to the left with animation
            {
                if (x - artis >= 1)
                {
                    x -= artis;
                    circle = new RectangleF(x, 1, diameter, diameter);

                    Invalidate();
                }
                else
                {
                    x = 1;
                    circle = new RectangleF(x, 1, diameter, diameter);

                    Invalidate();
                    paintTicker.Stop();

                }
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(60, 35);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            if (Enabled)
            {
                using (SolidBrush brush = new SolidBrush(isOn ? Color.Red : Color.Green))
                    e.Graphics.FillPath(brush, rect.Path);

                using (Pen pen = new Pen(borderColor, 1f))
                    e.Graphics.DrawPath(pen, rect.Path);

                string on = "ON";
                string off = "OFF";
                if (textEnabled)
                    using (Font font = new Font("Trebuchet MS", 9f * diameter / 30, FontStyle.Bold))
                    {
                        int height = TextRenderer.MeasureText(on, font).Height;
                        float y = (diameter - height) / 2f;
                        e.Graphics.DrawString(on, font, Brushes.White, 5, y + 1);

                        height = TextRenderer.MeasureText(off, font).Height;
                        y = (diameter - height) / 2f;
                        e.Graphics.DrawString(off, font, Brushes.White, diameter + 2, y + 1);
                    }

                using (SolidBrush circleBrush = new SolidBrush("#ffffff".FromHex()))
                    e.Graphics.FillEllipse(circleBrush, circle);

                using (Pen pen = new Pen(Color.Black, 1f))
                    e.Graphics.DrawEllipse(pen, circle);

            }
            else
            {
                using (SolidBrush disableBrush = new SolidBrush("#CFCFCF".FromHex()))
                using (SolidBrush ellBrush = new SolidBrush("#B3B3B3".FromHex()))
                {
                    e.Graphics.FillPath(disableBrush, rect.Path);
                    e.Graphics.FillEllipse(ellBrush, circle);
                    e.Graphics.DrawEllipse(Pens.DarkGray, circle);
                }
            }

            base.OnPaint(e);

        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;
            isOn = !isOn;
            IsOn = isOn;

            base.OnMouseClick(e);
        }
    }

}
