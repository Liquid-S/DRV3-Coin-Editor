using System;
using System.IO;
using System.Windows.Forms;

namespace DRV3_Coin_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        short Monocoins = 0;
        int CasinoCoins = 0, GDeathCardMachine = 0;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Liquid-S/DRV3-Coin-Editor/releases");
        }

        private void button1_Click(object sender, EventArgs e) //Open Savedata
        {
            using (OpenFileDialog SavedataFile = new OpenFileDialog())
            {
                SavedataFile.Title = "Select the savedata you want to edit.";
                SavedataFile.Filter = ".dat|*.dat|All Files (*.*)|*.*";
                SavedataFile.FileName = "save-dataXX.dat";
                SavedataFile.Multiselect = false;

                if (SavedataFile.ShowDialog() == DialogResult.OK)
                {
                    using (FileStream SaveFile = new FileStream(SavedataFile.FileName, FileMode.Open, FileAccess.Read))
                    using (BinaryReader SaveR = new BinaryReader(SaveFile))
                    {
                        SaveFile.Seek(0x0C, SeekOrigin.Begin);
                        if (SaveR.ReadInt32() == 0x203A3356)
                        {
                            textBox1.Text = SavedataFile.FileName; // Write the Path inside the textbox.
                            ReadSavedata(SavedataFile.FileName);
                        }
                        else
                            MessageBox.Show("File corrupted or unreadable!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ReadSavedata(string Savedata)
        {
            using (FileStream SaveFile = new FileStream(Savedata, FileMode.Open, FileAccess.Read))
            using (BinaryReader SaveR = new BinaryReader(SaveFile))
            {
                SaveFile.Seek(0x590, SeekOrigin.Begin);
                numericUpDown1.Value = Monocoins = SaveR.ReadInt16();

                SaveFile.Seek(0xB458, SeekOrigin.Begin);
                numericUpDown2.Value = CasinoCoins = SaveR.ReadInt32();

                SaveFile.Seek(0xB53C, SeekOrigin.Begin);
                numericUpDown3.Value = GDeathCardMachine = SaveR.ReadInt32();

                if (numericUpDown1.Enabled == false)
                    numericUpDown1.Enabled = true;

                if (numericUpDown2.Enabled == false)
                    numericUpDown2.Enabled = true;

                if (numericUpDown3.Enabled == false)
                    numericUpDown3.Enabled = true;

                if (button2.Enabled == false)
                    button2.Enabled = true;

                if (button3.Enabled == false)
                    button3.Enabled = true;

                if (button4.Enabled == false)
                    button4.Enabled = true;

                if (button5.Enabled == false)
                    button5.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e) //Save changes
        {
            if (File.Exists(textBox1.Text))
                WriteSavedata();
            else
                MessageBox.Show("Savedata not found.", "Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void WriteSavedata()
        {
            if ((numericUpDown1.Value != Monocoins) || (numericUpDown2.Value != CasinoCoins) || (numericUpDown3.Value != GDeathCardMachine))
            {
                // Make a backup.
                if (checkBox1.Checked == true)
                    File.Copy(textBox1.Text, textBox1.Text + ".backup", true);

                // Edit Savedata file.
                using (FileStream SaveFile = new FileStream(textBox1.Text, FileMode.Open, FileAccess.Write))
                using (BinaryWriter SaveW = new BinaryWriter(SaveFile))
                {
                    if (numericUpDown1.Value != Monocoins)
                    {
                        SaveFile.Seek(0x590, SeekOrigin.Begin);
                        SaveW.Write((short)numericUpDown1.Value);

                        SaveFile.Seek(0x2BAE, SeekOrigin.Begin);
                        SaveW.Write((short)numericUpDown1.Value);
                    }

                    if (numericUpDown2.Value != CasinoCoins)
                    {
                        SaveFile.Seek(0xB458, SeekOrigin.Begin);
                        SaveW.Write((uint)numericUpDown2.Value);
                    }

                    if (numericUpDown3.Value != GDeathCardMachine)
                    {
                        SaveFile.Seek(0xB53C, SeekOrigin.Begin);
                        SaveW.Write((uint)numericUpDown3.Value);
                    }
                }
                MessageBox.Show("Done!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Save failed!\nIt looks like you haven't made any changes.", "Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void button3_Click(object sender, EventArgs e) //Monocoins
        {
            numericUpDown1.Value = 999;
        }

        private void button4_Click(object sender, EventArgs e) //Casino coins
        {
            numericUpDown2.Value = 999999999;
        }

        private void button5_Click(object sender, EventArgs e) //G
        {
            numericUpDown3.Value = 999999;
        }
    }
}
