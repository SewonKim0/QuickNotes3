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
using Newtonsoft.Json;

namespace QuickNotes3
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        // Doc text colors
        private Color COLOR1 { get; } = Color.White;
        private Color COLOR2 { get; } = Color.FromArgb(190, 190, 190);
        private Color COLOR3 { get; } = Color.FromArgb(140, 140, 140);
        private Color BACK_COLOR { get; } = Color.FromArgb(40, 40, 40);

        // Paths
        private string docPath { get; set; } = "";
        private string DATA_PATH { get; } = "data.txt";
        private string BACKUP_PATH { get; } = "backup.txt";

        private string FIND_ICON_PATH { get; } = "Images/Icons/find.png";
        private string SECTIONS_ICON_PATH { get; } = "Images/Icons/sections.png";
        private string BACKUP_ICON_PATH { get; } = "Images/Icons/backup.png";

        // Find: Start Indices
        private List<int> findIndices = new List<int>();
        // Find: Curr Index
        private int findIndex = 0;
        // Find: Curr Keyword
        private string findKeyword = "";

        // Sections: Sections Data
        private List<Tuple<string, int>> sectionsData = new List<Tuple<string, int>>();

        // Doc scroll down control
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        private const int EM_SCROLL = 0x00B5;
        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;

        // Backups Data
        private Dictionary<string, string> backupData { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // setup icon buttons
            FindButton.SetImage(Image.FromFile(FIND_ICON_PATH));
            FindButton.ButtonClick += FindButton_ButtonClick;

            SectionButton.SetImage(Image.FromFile(SECTIONS_ICON_PATH));
            SectionButton.ButtonClick += SectionButton_ButtonClick;

            BackupButton.SetImage(Image.FromFile(BACKUP_ICON_PATH));
            BackupButton.ButtonClick += BackupButton_ButtonClick;

            // position hidden controls
            FindInput.Location = new Point(FindInput.Location.X, 0);
            FindUpButton.Location = new Point(FindUpButton.Location.X, 0);
            FindDownButton.Location = new Point(FindDownButton.Location.X, 12);

            Sections.Location = new Point(Sections.Location.X, 0);

            Backups.Location = new Point(Backups.Location.X, 0);

            // closing: save document & data
            this.FormClosing += MainForm_Closing;

            // doc position change: set new color
            Doc.SelectionChanged += SetColor;
            // doc link click: go to link
            Doc.LinkClicked += Doc_LinkClicked;
            // doc shortcut keys & find shortcuts
            Doc.KeyDown += Doc_KeyDown;

            // find input enter: set find data
            FindInput.KeyPress += FindInput_KeyPress;

            // get from data: read file
            string[] data = File.ReadAllText(DATA_PATH).Split(' ');

            // position: out of bounds check
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            int sizeX = int.Parse(data[2]);
            int sizeY = int.Parse(data[3]);

            int locationX = Math.Min(int.Parse(data[0]), screenWidth - sizeX);
            int locationY = Math.Min(int.Parse(data[1]), screenHeight - sizeY);
            locationX = Math.Max(locationX, 0);
            locationY = Math.Max(locationY, 0);

            // set position by data
            this.Location = new Point(
                locationX,
                locationY
            );
            // set size by data
            this.Size = new Size(
                sizeX,
                sizeY
            );

            // get backup data
            if (!(File.Exists(BACKUP_PATH))) {
                File.WriteAllText(BACKUP_PATH, "");
            }
            string backupJson = File.ReadAllText(BACKUP_PATH);
            if (backupJson.Equals(""))
            {
                backupData = new Dictionary<string, string>();
            }
            else
            {
                backupData = JsonConvert.DeserializeObject<Dictionary<string, string>>(backupJson);
            }
            // setup Backups list
            foreach (KeyValuePair<string, string> entry in backupData)
            {
                Backups.Items.Add(entry.Key);
            }
        }

        private void Backup(string backupPath, string backupText)
        {
            string path = backupPath;
            if (path.Equals(""))
            {
                return;
            }

            // get file name
            string fileName = path.Substring(path.LastIndexOf("\\") + 1);
            // get directory name
            path = path.Substring(0, path.LastIndexOf("\\"));
            string directoryName = path.Substring(path.LastIndexOf("\\") + 1);

            // save to BackupData
            string formattedPath = directoryName + "\\" + fileName;
            backupData[formattedPath] = backupText;
        }

        private void MainForm_Closing(object sender, FormClosingEventArgs e)
        {
            // save to current path
            if (!(docPath.Equals("")))
            {
                try
                {
                    File.WriteAllText(docPath, Doc.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR: FILE SAVING FAILED\n" + ex.Message);
                    e.Cancel = true;
                    return;
                }
            }

            // get position
            int posX = this.Location.X;
            int posY = this.Location.Y;
            // get size
            int sizeX = this.Size.Width;
            int sizeY = this.Size.Height;

            // if out of bounds: reset dimensions
            if (posX < 0 || posY < 0)
            {
                posX = 0;
                posY = 0;
                sizeX = 450;
                sizeY = 300;
            }

            // save to file
            try
            {
                File.WriteAllText(DATA_PATH,
                    posX + " " + posY + " " + sizeX + " " + sizeY
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: POS DATA SAVING FAILED\n" + ex.Message);
                e.Cancel = true;
                return; //
            }

            // save backupData as json
            string backupJson = JsonConvert.SerializeObject(backupData);
            try
            {
                File.WriteAllText(BACKUP_PATH, backupJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: BACKUP SAVING FAILED\n" + ex.Message);
                e.Cancel = true;
                return;
            }
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
                    Doc.SelectionColor = COLOR3;
                }
                // 1 tab: color2
                else if (line.Length >= 1 && line[0] == '\t')
                {
                    Doc.SelectionColor = COLOR2;
                }
                // default: color1 & highlight
                else
                {
                    Doc.SelectionColor = COLOR1;
                    Doc.SelectionBackColor = BACK_COLOR;
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
                Doc.SelectionColor = COLOR3;
            }
            //1 tab: color2
            else if (line.Length >= 1 && line[0] == '\t')
            {
                Doc.SelectionColor = COLOR2;
            }
            //default: color1
            else
            {
                Doc.SelectionColor = COLOR1;
            }
        }

        /* Save Button: Full saving procedure */
        private void SaveButton_Click(object sender, EventArgs e)
        {
            // take focus off of Doc
            SaveButton.Focus();

            string path = docPath;

            // if no path: prompt save as
            if (path.Equals(""))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                saveFileDialog.Title = "Save As";
                // Dialog: Save file
                DialogResult res = saveFileDialog.ShowDialog();
                if (res == DialogResult.OK)
                {
                    // set path for file
                    path = saveFileDialog.FileName;
                }
                else
                {
                    // if no save, reload and stop
                    Reload();
                    return;
                }
            }

            // file path validation: must end with .txt
            string extension = path.Substring(path.Length - 4);
            if (!extension.Equals(".txt"))
            {
                MessageBox.Show("Error: Must save as a .txt file!");
                Reload();
                return;
            }

            // save to file
            try
            {
                File.WriteAllText(path, Doc.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: FILE SAVING FAILED\n" + ex.Message);
            }

            // set docPath
            docPath = path;
            // set doc name display
            DocPath.Text = path.Substring(path.LastIndexOf('\\') + 1, path.LastIndexOf('.') - path.LastIndexOf('\\') - 1);
            this.Text = DocPath.Text;

            // Reload doc
            Reload();
            // Backup doc
            Backup(docPath, Doc.Text);
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
                // save to current path
                if (!(docPath.Equals("")))
                {
                    try
                    {
                        File.WriteAllText(docPath, Doc.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR: FILE SAVING FAILED\n" + ex.Message);
                        return;
                    }
                }

                // load to docPath
                docPath = openFileDialog.FileName;
                // display doc name
                DocPath.Text = docPath.Substring(docPath.LastIndexOf('\\') + 1, docPath.LastIndexOf('.') - docPath.LastIndexOf('\\') - 1);
                this.Text = DocPath.Text;

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

            // backup file
            if (deletePath.Equals(docPath))
            {
                Backup(docPath, Doc.Text);
            }
            else
            {
                Backup(deletePath, File.ReadAllText(deletePath));
            }

            // if file path == current path: clear (doc, docPath, DocPath)
            if (deletePath.Equals(docPath))
            {
                Doc.Text = "";
                docPath = "";
                DocPath.Text = "";
                this.Text = "Quicknotes3";
            }

            // delete
            File.Delete(deletePath);
        }

        private void Doc_TextChanged(object sender, EventArgs e)
        {
            // close Sections combobox
            Sections.Visible = false;
        }

        /* When link clicked in doc */
        private void Doc_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        /* Open Find */
        private void OpenFind()
        {
            FindInput.Visible = true;
            FindUpButton.Visible = true;
            FindDownButton.Visible = true;
            FindInput.Select();

            // close others
            CloseSections();
            CloseBackups();
        }

        /* Close Find */
        private void CloseFind()
        {
            FindInput.Visible = false;
            FindInput.Text = "";
            FindUpButton.Visible = false;
            FindDownButton.Visible = false;
            findKeyword = "";
            findIndices.Clear();
            findIndex = 0;
        }

        /* Open Sections */
        private void OpenSections()
        {
            Sections.Visible = true;

            // clear sections data
            sectionsData.Clear();
            // clear Sections combobox
            Sections.Text = "";
            Sections.Items.Clear();

            // store sections data
            string[] lines = Doc.Lines;
            int lineIndex = 0;
            foreach (string line in lines)
            {
                // if no tab: add to sections
                if (line.Length > 0 && line[0] != '\t')
                {
                    sectionsData.Add(new Tuple<string, int>(line, lineIndex));
                }

                // update lineIndex
                lineIndex += line.Length + 1;
            }

            // show sections data
            foreach (Tuple<string, int> section in sectionsData)
            {
                Sections.Items.Add(section.Item1);
            }
            Sections.DroppedDown = true;

            // get closest line in sectionsData and select it
            int currLine = Doc.GetLineFromCharIndex(Doc.SelectionStart);
            int closestIndex = 0;
            for (int sectionsIndex = 0; sectionsIndex < sectionsData.Count; sectionsIndex++)
            {
                int sectionIndex = sectionsData[sectionsIndex].Item2;
                int sectionLine = Doc.GetLineFromCharIndex(sectionIndex);
                if (sectionLine <= currLine)
                {
                    closestIndex = sectionsIndex;
                }
            }
            // Tag = false indicates that selection event should not be triggered
            Sections.Tag = false;
            if (Doc.Lines.Length != 0)
            {
                Sections.SelectedIndex = closestIndex;
            }
            Sections.Tag = true;

            // close others
            CloseFind();
            CloseBackups();
        }

        /* Close Sections */
        private void CloseSections()
        {
            Sections.Visible = false;
        }

        /* Open Backups */
        private void OpenBackups()
        {
            // show backups
            Backups.Visible = true;
            Backups.DroppedDown = true;

            // close others
            CloseSections();
            CloseFind();
        }

        /* Close Backups */
        private void CloseBackups()
        {
            Backups.Visible = false;
        }

        /* Toggle visibility of find interface */
        private void FindButton_ButtonClick(object sender, EventArgs e)
        {
            // if not visible: show
            if (FindInput.Visible == false)
            {
                OpenFind();
            }
            // if visible: hide and reset find data
            else
            {
                CloseFind();
            }
        }

        /* Section Button Click: Toggle Sections Visibility */
        private void SectionButton_ButtonClick(object sender, EventArgs e)
        {
            // if not visible: show
            if (Sections.Visible == false)
            {
                OpenSections();
            }
            // if visible: hide
            else
            {
                CloseSections();
            }
        }

        /* Backup Button: Click */
        private void BackupButton_ButtonClick(object sender, EventArgs e)
        {
            // if not visible: show
            if (Backups.Visible == false)
            {
                OpenBackups();
            }
            // if visible: hide
            else
            {
                CloseBackups();
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
                int selectionStart = Doc.SelectionStart;

                SaveButton_Click(null, null);

                Doc.SelectionStart = selectionStart;
                Doc.SelectionLength = 0;
                Doc.Select();
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
                OpenFind();
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

            // enter: move down in find
            if (e.KeyCode == Keys.Return)
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

        /* Sections Select Section: Go To Section */
        private void Sections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((bool)Sections.Tag == false)
            {
                return;
            }

            // get index
            string sectionText = Sections.SelectedItem.ToString();
            int selectionIndex = -1;
            foreach (Tuple<string, int> section in sectionsData)
            {
                if (section.Item1.Equals(sectionText))
                {
                    selectionIndex = section.Item2;
                    break;
                }
            }

            // go to index
            if (selectionIndex == -1)
            {
                return;
            }
            else
            {
                // go to selection location
                Doc.Select();
                Doc.SelectionStart = selectionIndex;
                Doc.SelectionLength = 0;
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
        }

        /* Backups Index Selected: Load Backup To Doc */
        private void Backups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(backupData.ContainsKey(Backups.Text)))
            {
                MessageBox.Show("Error: Backup path does not exist");
            }
            Doc.Text = backupData[Backups.Text];
            Reload();
        }
    }
}