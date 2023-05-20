using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QuickNotes3
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Color color1 = Color.White;
        private Color color2 = Color.FromArgb(180, 180, 180);
        private Color color3 = Color.FromArgb(120, 120, 120);

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            //closing: save to data
            this.FormClosing += SetData;
            //doc position change: set new color
            Doc.SelectionChanged += SetColor;

            //get from data: read file
            string[] data = File.ReadAllText("data.txt").Split(' ');
            //set position by data
            this.Location = new Point(
                int.Parse(data[0]), 
                int.Parse(data[1])
            );
            //set size by data
            this.Size = new Size(
                int.Parse(data[2]),
                int.Parse(data[3])
            );
        }

        private void SetData(object sender, EventArgs e)
        {
            //get position
            int posX = this.Location.X;
            int posY = this.Location.Y;
            //get size
            int sizeX = this.Size.Width;
            int sizeY = this.Size.Height;

            //save to file
            File.WriteAllText("data.txt", 
                posX + " " + posY + " " + sizeX + " " + sizeY    
            );
        }

        private void Reload()
        {
            //get selection index
            int selectionIndex = Doc.SelectionStart;

            //get all lines
            string[] lines = Doc.Text.Split('\n');
            //clear doc
            Doc.Text = "";

            //write lines: colors
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                //2 tabs: color3
                if (line.Length >= 2 && line[0] == '\t' && line[1] == '\t')
                {
                    Doc.SelectionColor = color3;
                }
                //1 tab: color2
                else if (line.Length >= 1 && line[0] == '\t')
                {
                    Doc.SelectionColor = color2;
                }
                //default: color1
                else
                {
                    Doc.SelectionColor = color1;
                }

                //add to doc
                if (i == lines.Length - 1)
                {
                    Doc.AppendText(line);
                }
                else
                {
                    Doc.AppendText(line + "\n");
                }
            }

            //scroll to caret by selectionIndex
            Doc.SelectionStart = selectionIndex;
            Doc.ScrollToCaret();
        }

        private void SetColor(object sender, EventArgs e)
        {
            //get lines
            string[] lines = Doc.Text.Split('\n');
            //get pos
            int pos = Doc.SelectionStart;
            //get line index: line iteration
            int lineIndex = -1;
            int currPos = 0;
            while (currPos <= pos)
            {
                //increment lineIndex
                lineIndex++;
                //increment currPos by len + 1
                currPos += lines[lineIndex].Length + 1;
            }

            string line = lines[lineIndex];
            //2 tabs: color3
            if (line.Length >= 2 && line[0] == '\t' && line[1] == '\t')
            {
                Doc.SelectionColor = color3;
            }
            //1 tab: color2
            else if (line.Length >= 1 && line[0] == '\t')
            {
                Doc.SelectionColor = color2;
            }
            //default: color1
            else
            {
                Doc.SelectionColor = color1;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Reload();
        }
    }
}
