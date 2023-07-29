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
        public PictureButton()
        {
            InitializeComponent();
        }

        private void PictureButton_Load(object sender, EventArgs e)
        {
            //
        }

        /* Main Button Click */
        private void MainButton_Click(object sender, EventArgs e)
        {
            //
        }

        /* Set Button Image */
        public void SetImage(Image image)
        {
            MainButton.Image = image;
        }
    }
}
