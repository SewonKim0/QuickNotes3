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
using System.Runtime.InteropServices;
using System.Threading;

namespace QuickNotes3
{
    public partial class Form : System.Windows.Forms.Form
    {
        // Doc text colors
        private Color color1 = Color.White;
        private Color color2 = Color.FromArgb(190, 190, 190);
        private Color color3 = Color.FromArgb(140, 140, 140);
        // Current doc path
        private string docPath = "";

        // Find: Start Indices
        private List<int> findIndices = new List<int>();
        // Find: Curr Index
        private int findIndex = 0;
        // Find: Curr Keyword
        private string findKeyword = "";

        // Doc scroll down control
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int EM_SCROLL = 0x00B5;
        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // hide find-related controls
            FindInput.Visible = false;
            FindUpButton.Visible = false;
            FindDownButton.Visible = false;

            // closing: save to data
            this.FormClosing += SetData;
            // closing: save document
            this.FormClosing += SaveDoc;

            // doc position change: set new color
            Doc.SelectionChanged += SetColor;
            // doc link click: go to link
            Doc.LinkClicked += Doc_LinkClicked;
            // doc shortcut keys & find shortcuts
            Doc.KeyDown += Doc_KeyDown;

            // find input enter: set find data
            FindInput.KeyPress += FindInput_KeyPress;

            // get from data: read file
            string[] data = File.ReadAllText("data.txt").Split(' ');
            // set position by data
            this.Location = new Point(
                int.Parse(data[0]), 
                int.Parse(data[1])
            );
            // set size by data
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
            // get selection index
            int selectionIndex = Doc.SelectionStart;

            // get all lines
            string[] lines = Doc.Text.Split('\n');
            // clear doc
            Doc.Text = "";

            // write lines: colors
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                // 2 tabs: color3
                if (line.Length >= 2 && line[0] == '\t' && line[1] == '\t')
                {
                    Doc.SelectionColor = color3;
                }
                // 1 tab: color2
                else if (line.Length >= 1 && line[0] == '\t')
                {
                    Doc.SelectionColor = color2;
                }
                // default: color1
                else
                {
                    Doc.SelectionColor = color1;
                }

                // add to doc
                if (i == lines.Length - 1)
                {
                    Doc.AppendText(line);
                }
                else
                {
                    Doc.AppendText(line + "\n");
                }
            }

            // scroll to caret by selectionIndex
            Doc.SelectionStart = selectionIndex;
            Doc.ScrollToCaret();

            // get number of visible/total lines
            int numVisibleLines = Doc.ClientSize.Height / Doc.Font.Height;
            int numTotalLines = Doc.GetLineFromCharIndex(Doc.Text.Length - 1) + 1;
            // get current line index
            int currLine = Doc.GetLineFromCharIndex(Doc.SelectionStart);

            // if not bottom: scroll up halfway
            if (currLine < numTotalLines - numVisibleLines)
            {
                int scrollUp = numVisibleLines / 2;
                for (int x = 1; x <= scrollUp; x++)
                {
                    SendMessage(Doc.Handle, EM_SCROLL, (IntPtr)SB_LINEUP, IntPtr.Zero);
                }
            }
            // if bottom: scroll up by (remaining - (visible / 2))
            else
            {
                int remainingLines = numTotalLines - currLine;
                int scrollUp = remainingLines - (numVisibleLines / 2);
                for (int x = 1; x <= scrollUp; x++)
                {
                    SendMessage(Doc.Handle, EM_SCROLL, (IntPtr)SB_LINEUP, IntPtr.Zero);
                }
            }
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

        /* Save Button: Full saving procedure */
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
            // open file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            openFileDialog.Title = "Load File";
            // load from file
            DialogResult res = openFileDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                // load to docPath
                docPath = openFileDialog.FileName;
                // display doc name
                DocPath.Text = docPath.Substring(docPath.LastIndexOf('\\') + 1);

                // load to doc
                Doc.Text = File.ReadAllText(openFileDialog.FileName);
                // reload
                Reload();

                // select first index
                Doc.Select();
                Doc.SelectionStart = 0;
            }
            else
            {
                // stop
                return;
            }
        }

        private void DocPath_Click(object sender, EventArgs e)
        {
            //
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            // get file path to delete
            OpenFileDialog deleteFileDialog = new OpenFileDialog();
            deleteFileDialog.Filter = "Text Files (*.txt)|*.txt";
            deleteFileDialog.Title = "Delete File";

            string deletePath = "";

            // if result is ok, set file path
            if (deleteFileDialog.ShowDialog() == DialogResult.OK)
            {
                deletePath = deleteFileDialog.FileName;
            }
            else
            {
                return;
            }

            // if file path == current path: clear (doc, docPath, DocPath)
            if (deletePath.Equals(docPath))
            {
                Doc.Text = "";
                docPath = "";
                DocPath.Text = "";
            }

            // delete
            File.Delete(deletePath);
        }

        private void Doc_TextChanged(object sender, EventArgs e)
        {
            //
        }

        /* When link clicked in doc */
        private void Doc_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        /* Toggle visibility of find interface */
        private void FindButton_Click(object sender, EventArgs e)
        {
            // if not visible: show
            if (FindInput.Visible == false)
            {
                FindInput.Visible = true;
                FindUpButton.Visible = true;
                FindDownButton.Visible = true;
            }
            // if visible: hide and reset find data
            else
            {
                FindInput.Visible = false;
                FindUpButton.Visible = false;
                FindDownButton.Visible = false;
                findKeyword = "";
                findIndices.Clear();
                findIndex = 0;
            }
        }

        /* Find keyword entered: create list of results */
        private void FindInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ensure enter was pressed
            if (e.KeyChar != (char) Keys.Return)
            {
                return;
            }
            // edge case: input is blank
            if (FindInput.Text.Equals(""))
            {
                return;
            }

            // reset findIndex & findIndices
            findIndex = 0;
            findIndices.Clear();

            // find all start indices of keyword
            findKeyword = FindInput.Text;
            String text = Doc.Text;
            int currIndex = 0;
            while (text.IndexOf(findKeyword, currIndex, StringComparison.OrdinalIgnoreCase) != -1)
            {
                findIndices.Add(text.IndexOf(findKeyword, currIndex, StringComparison.OrdinalIgnoreCase));
                currIndex = text.IndexOf(findKeyword, currIndex, StringComparison.OrdinalIgnoreCase);
                currIndex += findKeyword.Length;
            }
            
            // if no indices: stop
            if (findIndices.Count == 0)
            {
                return;
            }
            // select first index
            else
            {
                Doc.Select();
                Doc.SelectionStart = findIndices[findIndex];
                Doc.SelectionLength = findKeyword.Length;
            }
        }

        /* Go up results of Find */
        private void FindUpButton_Click(object sender, EventArgs e)
        {
            // if find list empty: stop
            if (findIndices.Count == 0)
            {
                return;
            }

            // decrement index
            if (findIndex == 0)
            {
                findIndex = findIndices.Count - 1;
            }
            else
            {
                findIndex--;
            }

            // select and go to position
            Doc.Select();
            Doc.SelectionStart = findIndices[findIndex];
            Doc.SelectionLength = findKeyword.Length;
        }

        /* Go down results of Find */
        private void FindDownButton_Click(object sender, EventArgs e)
        {
            // if find list empty: stop
            if (findIndices.Count == 0)
            {
                return;
            }

            // increment index
            findIndex = (findIndex + 1) % findIndices.Count;

            // select and go to position
            Doc.Select();
            Doc.SelectionStart = findIndices[findIndex];
            Doc.SelectionLength = findKeyword.Length;
        }

        /* Handling for Doc keyboard & find shortcuts */
        private void Doc_KeyDown(object sender, KeyEventArgs e)
        {
            // ctrl + s: save doc
            if (e.Control && char.ToLower((char)e.KeyCode) == 's')
            {
                SaveButton_Click(null, null);
            }

            // ctrl + r: reload doc
            if (e.Control && char.ToLower((char)e.KeyCode) == 'r')
            {
                Reload();
                e.Handled = true;
            }

            // ctrl + f: find in doc
            if (e.Control && char.ToLower((char)e.KeyCode) == 'f')
            {
                // make find interface visible
                FindInput.Visible = true;
                FindDownButton.Visible = true;
                FindUpButton.Visible = true;

                // focus to FindInput
                FindInput.Select();
            }

            // up arrow: move up in find
            if (e.KeyCode == Keys.Up)
            {
                // validate find mode
                string selected = Doc.Text.Substring(Doc.SelectionStart, Doc.SelectionLength);
                if (!(selected.Equals(findKeyword, StringComparison.OrdinalIgnoreCase)) || findKeyword.Equals(""))
                {
                    return;
                }

                // go up
                FindUpButton_Click(null, null);
                e.Handled = true;
            }

            // down arrow: move down in find
            if (e.KeyCode == Keys.Down)
            {
                // validate find mode
                string selected = Doc.Text.Substring(Doc.SelectionStart, Doc.SelectionLength);
                if (!(selected.Equals(findKeyword, StringComparison.OrdinalIgnoreCase)) || findKeyword.Equals(""))
                {
                    return;
                }

                // go down
                FindDownButton_Click(null, null);
                e.Handled = true;
            }
        }
    }
}