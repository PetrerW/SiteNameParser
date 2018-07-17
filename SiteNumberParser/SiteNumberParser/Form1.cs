using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// The project to parse the names of the sites to numbered order and then conect them into a book.
/// </summary>
namespace SiteNumberParser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// A button with name "Parse Names"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Parser.ParseFiles(textBox2.Text);
        }

        /// <summary>
        /// A button with name "Connect"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
                Parser.connect(textBox2.Text+"\\parsed", "book");
            else
                Parser.connect(textBox2.Text+"\\parsed", textBox1.Text);
        }

        /// <summary>
        /// A button "Browse..."
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderDlg = new FolderBrowserDialog();
                folderDlg.ShowNewFolderButton = true;

                // Show the FolderBrowserDialog
                DialogResult result = folderDlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    //Set textof the textBox with the selected path
                    textBox2.Text = folderDlg.SelectedPath;
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("An error occured. Make sure you have entered a valid path.");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
