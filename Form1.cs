using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormsApp9
{
    public partial class Form1 : Form
    {
        private string currentFilePath = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InstalledFontCollection installedFonts = new InstalledFontCollection();
            foreach (FontFamily font in installedFonts.Families)
            {
                cmbFonts.Items.Add(font.Name);
            }

            int[] sizes = { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            foreach (int size in sizes)
            {
                cmbSizes.Items.Add(size);
            }

            ResetDefaultSettings();
        }

        private void ResetDefaultSettings()
        {
            cmbFonts.SelectedItem = "Tahoma";
            cmbSizes.SelectedItem = 14;
            richText.Font = new Font("Tahoma", 14, FontStyle.Regular);
            currentFilePath = "";
            UpdateWordCount();
        }

        private void CreateNewDocument()
        {
            richText.Clear();
            ResetDefaultSettings();
        }
        private void btnNew_Click(object sender, EventArgs e) => CreateNewDocument();

        private void OpenDocument()
        {
            OpenFileDialog oFile = new OpenFileDialog();
            oFile.Filter = "Rich Text Format (*.rtf)|*.rtf|Text files (*.txt)|*.txt";

            if (oFile.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = oFile.FileName;
                string ext = Path.GetExtension(currentFilePath).ToLower();

                if (ext == ".rtf")
                {
                    richText.LoadFile(currentFilePath, RichTextBoxStreamType.RichText);
                }
                else
                {
                    richText.LoadFile(currentFilePath, RichTextBoxStreamType.PlainText);
                }

                if (richText.Font != null)
                {
                    cmbFonts.SelectedItem = richText.Font.FontFamily.Name;
                    cmbSizes.SelectedItem = (int)richText.Font.Size;
                }
                UpdateWordCount();
            }
        }

        private void SaveDocument()
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveFileDialog sFile = new SaveFileDialog();
                sFile.Filter = "Rich Text Format (*.rtf)|*.rtf";
                sFile.DefaultExt = "rtf";

                if (sFile.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = sFile.FileName;
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                    MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                string ext = Path.GetExtension(currentFilePath).ToLower();
                if (ext == ".rtf")
                {
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.RichText);
                }
                else
                {
                    richText.SaveFile(currentFilePath, RichTextBoxStreamType.PlainText);
                }
                MessageBox.Show("Lưu văn bản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnSave_Click(object sender, EventArgs e) => SaveDocument();

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuFormat_Click(object sender, EventArgs e)
        {
            FontDialog fontDlg = new FontDialog();
            fontDlg.ShowColor = true;
            fontDlg.ShowApply = true;
            fontDlg.ShowEffects = true;
            fontDlg.ShowHelp = true;

            if (fontDlg.ShowDialog() != DialogResult.Cancel)
            {
                richText.ForeColor = fontDlg.Color;
                richText.Font = fontDlg.Font;

                cmbFonts.SelectedItem = fontDlg.Font.FontFamily.Name;
                cmbSizes.SelectedItem = (int)fontDlg.Font.Size;
            }
        }

        private void cmbFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFonts.SelectedItem is string fontName && richText.SelectionFont != null)
            {
                float fontSize = richText.SelectionFont.Size;
                FontStyle fontStyle = richText.SelectionFont.Style;

                richText.SelectionFont = new Font(fontName, fontSize, fontStyle);
            }
        }

        private void cmbSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSizes.SelectedItem != null && richText.SelectionFont != null)
            {
                if (float.TryParse(cmbSizes.SelectedItem.ToString(), out float fontSize))
                {
                    string fontName = richText.SelectionFont.FontFamily.Name;
                    FontStyle fontStyle = richText.SelectionFont.Style;

                    richText.SelectionFont = new Font(fontName, fontSize, fontStyle);
                }
            }
        }

        private void btnBold_Click(object sender, EventArgs e)
        {
            if (richText.SelectionFont != null)
            {
                Font currentFont = richText.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ FontStyle.Bold;
                richText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newStyle);
            }
        }

        private void btnItalic_Click(object sender, EventArgs e)
        {
            if (richText.SelectionFont != null)
            {
                Font currentFont = richText.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ FontStyle.Italic;
                richText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newStyle);
            }
        }

        private void btnUnderline_Click(object sender, EventArgs e)
        {
            if (richText.SelectionFont != null)
            {
                Font currentFont = richText.SelectionFont;
                FontStyle newStyle = currentFont.Style ^ FontStyle.Underline;
                richText.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newStyle);
            }
        }

        private void richText_TextChanged(object sender, EventArgs e)
        {
            UpdateWordCount();
        }

        private void UpdateWordCount()
        {
            string content = richText.Text.Trim();
            if (string.IsNullOrEmpty(content))
            {
                lblWordCount.Text = "Tổng số từ: 0";
            }
            else
            {
                string[] words = Regex.Split(content, @"\s+");
                lblWordCount.Text = $"Tổng số từ: {words.Length}";
            }
        }

        

        private void mnuNew_Click(object sender, EventArgs e) => CreateNewDocument();
        private void mnuOpen_Click(object sender, EventArgs e) => OpenDocument();
        private void mnuSave_Click(object sender, EventArgs e) => SaveDocument();
    }
}