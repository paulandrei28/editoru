using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editoru
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<TextBox> textBoxList = new List<TextBox>();
        TabControl tab = new TabControl();
        int indexTab=0;


       

        private void iesireToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            indexTab = tabControl1.SelectedIndex;
            textBoxList[indexTab].WordWrap = wordWrapToolStripMenuItem.Checked;
        }

        private void despreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Editoru' de texte - v.1.9", "Despre", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool saveFile(string fName)
        {
            bool res = false;

            try
            {
                StreamWriter sw = new StreamWriter(fName);
                indexTab = tabControl1.SelectedIndex;
                sw.Write(textBoxList[indexTab].Text);
                sw.Close();

                isSaved = true;

                updateInterface();

                res = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la salvare!\n" + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return res;
        }

        private void salveazaCaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveAsDialog.ShowDialog() == DialogResult.OK)
            {
                if (saveFile(saveAsDialog.FileName))
                {
                    fileNameInEditor = saveAsDialog.FileName;
                    int n = fileNameInEditor.Length;
                    int i = n - 1;
                    int indexNume = 0;
                    string copie = fileNameInEditor;
                    while (copie[i].CompareTo('\\') != 0)
                    {
                        indexNume = i;
                        i--;
                    }
                    fileNameInEditor = string.Empty;
                    fileNameInEditor = copie.Substring(indexNume, (n - indexNume));
                    updateInterface();
                }
            }
        }

        private string fileNameInEditor = null;
        private bool isSaved = false;

        private void updateInterface()
        {
            if (!isSaved)
            {
                string s = this.Text;
                if (s[0]!='*')
                {
                    this.Text = "*" + s;
                }
            }
            else
            {
                string s = this.Text;
                if (s[0] == '*')
                {
                    this.Text = s.Replace("*","");
                }
            }

            labelFName.Text = (fileNameInEditor == null) ? "Fisier fara nume" : fileNameInEditor;
            tabControl1.SelectedTab.Text = (fileNameInEditor == null) ? "Fisier fara nume" : fileNameInEditor;
        }

        private void salveazaToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            if (fileNameInEditor == null)
            {
                salveazaCaToolStripMenuItem_Click(sender, null);
            }
            else
            {
                saveFile(fileNameInEditor);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool isExitSure = true;

            if (!isSaved)
            {
                DialogResult resDiag = MessageBox.Show("Aveti modificari nesalvate.\nSalvati modificarile inainte sa parasiti aplicatia?", "Confirmare", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (resDiag == DialogResult.Yes)
                {
                    salveazaToolStripMenuItem_Click(sender, null);
                }

                if (resDiag == DialogResult.Cancel)
                {
                    isExitSure = false;
                }
            }

            if (!isExitSure)
            {
                e.Cancel = true;
            }
        }

        private void nouToolStripMenuItem_Click(object sender, EventArgs e)
        {
                TabPage tab = new TabPage();
                tabControl1.Controls.Add(tab);
                int nr = tabControl1.TabCount - 1;
                tabControl1.SelectTab(nr);
                TextBox text = new TextBox();
                text.Parent = tab;
                text.Multiline = true;
                text.Dock = DockStyle.Fill;
                text.ScrollBars = ScrollBars.Both;
                fileNameInEditor = null;
                isSaved = true;
                textBoxList.Add(text);
                updateInterface();
            
        }

        private void deschideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isDoOpen = true;

            if (isDoOpen && openDialog.ShowDialog()==DialogResult.OK)
            {
                indexTab = tabControl1.SelectedIndex;
                try
                {
                    StreamReader sr = new StreamReader(openDialog.FileName);
                    TabPage tab = new TabPage();
                    tabControl1.Controls.Add(tab);
                    int nr = tabControl1.TabCount - 1;
                    tabControl1.SelectTab(nr);
                    TextBox text = new TextBox();
                    text.Text = sr.ReadToEnd();
                    sr.Close();
                    text.Parent = tab;
                    text.Multiline = true;
                    text.Dock = DockStyle.Fill;
                    text.ScrollBars = ScrollBars.Both;
                    fileNameInEditor = null;
                    isSaved = true;
                    textBoxList.Add(text);
                    fileNameInEditor = openDialog.FileName;
                    int n = fileNameInEditor.Length;
                    int i = n - 1;
                    int indexNume = 0;
                    string copie = fileNameInEditor;
                    while (copie[i].CompareTo('\\') != 0)
                    {
                        indexNume = i;
                        i--;
                    }
                    fileNameInEditor = string.Empty;
                    fileNameInEditor = copie.Substring(indexNume, (n - indexNume));
                    
                    isSaved = true;

                    updateInterface();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Eroare la deschidere!\n" + ex.Message, "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TextBox text = new TextBox();
            text.Parent = tabControl1.SelectedTab;
            text.Multiline = true;
            text.Dock = DockStyle.Fill;
            textBoxList.Add(text);
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            isSaved = false;
            updateInterface();
        }

        private void inchideTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            indexTab = tabControl1.SelectedIndex;
            textBoxList.Remove(textBoxList[indexTab]);
            tabControl1.Controls.Remove(tabControl1.SelectedTab);
        }

       
    }
}
