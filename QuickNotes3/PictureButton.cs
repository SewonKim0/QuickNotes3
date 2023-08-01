using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickNotes3
{
    public partial class PictureButton : UserControl
    {
        // colors
        private Color NORMAL_COLOR { get; } = Color.DarkGray;
        private Color HOVER_COLOR { get; } = Color.LightGray;
        private Color ACTIVE_COLOR { get; } = Color.White;

        // states
        private bool hover { get; set; } = false;

        // events
        public event EventHandler<EventArgs> ButtonClick;

        public PictureButton()
        {
            InitializeComponent();
        }

        /* Load */
        private void PictureButton_Load(object sender, EventArgs e)
        {
            // events
            MainButton.MouseDown += MainButton_MouseDown;
            MainButton.MouseUp += MainButton_MouseUp;
            MainButton.MouseHover += MainButton_MouseHover;
            MainButton.MouseLeave += MainButton_MouseLeave;
        }

        /* Mouse Down */
        private void MainButton_MouseDown(object sender, EventArgs e)
        {
            this.BackColor = ACTIVE_COLOR;

            // fire ButtonClicked event
            ButtonClick?.Invoke(this, EventArgs.Empty);
        }

        /* Mouse Up */
        private void MainButton_MouseUp(object sender, EventArgs e)
        {
            if (hover == false)
            {
                this.BackColor = NORMAL_COLOR;
            }
            else
            {
                this.BackColor = HOVER_COLOR;
            }
        }

        /* Hover */
        private void MainButton_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = HOVER_COLOR;
            hover = true;
        }

        /* Unhover */
        private void MainButton_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = NORMAL_COLOR;
            hover = false;
        }

        /* Set Button Image */
        public void SetImage(Image image)
        {
            MainButton.Image = image;
        }
    }
}
