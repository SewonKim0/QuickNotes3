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
using System.Diagnostics;

namespace QuickNotes3
{
    public partial class Form : System.Windows.Forms.Form
    {
        //Doc text colors
        private Color color1 = Color.White;
        private Color color2 = Color.FromArgb(180, 180, 180);
        private Color color3 = Color.FromArgb(120, 120, 120);
        //Current doc path
        private string docPath = "";

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            //closing: save to data
            this.FormClosing += SetData;
            //closing: save document
            this.FormClosing += SaveDoc;
            //doc position change: set new color
            Doc.SelectionChanged += SetColor;
            //doc link click: go to link
            Doc.LinkClicked += Doc_LinkClicked;

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

        private void SaveDoc(object sender, EventArgs e)
        {
            //if unnamed, dont save
            if (DocPath.Text.Equals(""))
            {
                return;
            }

            //save to current path
            try
            {
                File.WriteAllText(docPath, Doc.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: FILE SAVING FAILED");
                return;
            }
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
            string path = docPath;

            //if no path: prompt save as
            if (path.Equals(""))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                saveFileDialog.Title = "Save As";
                //Dialog: Save file
                DialogResult res = saveFileDialog.ShowDialog();
                if (res == DialogResult.OK)
                {
                    //set path for file
                    path = saveFileDialog.FileName;
                }
                else
                {
                    //if no save, reload and stop
                    Reload();
                    return;
                }
            }

            //file path validation: must end with .txt
            string extension = path.Substring(path.Length - 4);
            if (!extension.Equals(".txt"))
            {
                MessageBox.Show("Error: Must save as a .txt file!");
                Reload();
                return;
            }

            //save to file
            try
            {
                File.WriteAllText(path, Doc.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: FILE SAVING FAILED");
            }
            //set docPath
            docPath = path;
            //set doc name display
            DocPath.Text = path.Substring(path.LastIndexOf('\\') + 1);

            //Reload doc
            Reload();
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            //open file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            openFileDialog.Title = "Load File";
            //load from file
            DialogResult res = openFileDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                //load to docPath
                docPath = openFileDialog.FileName;
                //display doc name
                DocPath.Text = docPath.Substring(docPath.LastIndexOf('\\') + 1);

                //load to doc
                Doc.Text = File.ReadAllText(openFileDialog.FileName);
                //reload
                Reload();
            }
            else
            {
                //stop
                return;
            }
        }

        private void DocPath_Click(object sender, EventArgs e)
        {
            //
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            //get file path to delete
            OpenFileDialog deleteFileDialog = new OpenFileDialog();
            deleteFileDialog.Filter = "Text Files (*.txt)|*.txt";
            deleteFileDialog.Title = "Delete File";

            string deletePath = "";

            //if result is ok, set file path
            if (deleteFileDialog.ShowDialog() == DialogResult.OK)
            {
                deletePath = deleteFileDialog.FileName;
            }
            else
            {
                return;
            }

            //if file path == current path: clear (doc, docPath, DocPath)
            if (deletePath.Equals(docPath))
            {
                Doc.Text = "";
                docPath = "";
                DocPath.Text = "";
            }

            //delete
            File.Delete(deletePath);
        }

        private void Doc_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void Doc_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }
    }
}
