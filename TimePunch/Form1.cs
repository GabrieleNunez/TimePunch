using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace TimePunch
{
    public partial class Form1 : Form
    {
        private DateTime firstPunch;
        private DateTime lastPunch;
     
        public Form1()
        {
            firstPunch = DateTime.MinValue;
            lastPunch = DateTime.MinValue;
            InitializeComponent();
            
        }
        private void Calculate()
        {
            TimeSpan diff = lastPunch - firstPunch;
            howLongLabel.Text = string.Format("How Long? {0} Days, {1} Hours , {2} Minutes, {3} Seconds",
                diff.Days, diff.Hours, diff.Minutes, diff.Seconds);
            decimal rate = rateNumeric.Value;
            decimal totalHours = new decimal(diff.TotalHours);
            decimal amountEarned = rate * totalHours;
            earnedLabel.Text = string.Format("You earned: {0}", Math.Round(amountEarned,2));

        }
        private void punchButton_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (firstPunch == DateTime.MinValue)
            {
                firstPunch = DateTime.Now;
                startLabel.Text = string.Format("Start: {0}", firstPunch);
            }
            else
            {
                lastPunch = DateTime.Now;
                endLabel.Text = string.Format("End: {0}",lastPunch);
                punchButton.Enabled = false;
                Calculate();

            }
            this.Cursor = Cursors.Arrow;
        }
        
        private void SavePunchFile()
        {
            this.Cursor = Cursors.WaitCursor;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Time Sheet (*.tsheet) |*.tsheet|All Files|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter writer = new StreamWriter(dialog.FileName))
                    {
                        writer.WriteLine(firstPunch.ToString());
                        writer.WriteLine(lastPunch.ToString());
                        writer.Close();
                    }
                }
            }
            this.Cursor = Cursors.Arrow;
        }
        private void LoadPunchFile()
        {
            this.Cursor = Cursors.WaitCursor;
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Time Sheet (*.tsheet) |*.tsheet|All Files|*.*";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader reader = new StreamReader(dialog.FileName))
                    {
                        while (!reader.EndOfStream)
                        {
                            try
                            {
                                if (firstPunch == DateTime.MinValue)
                                    firstPunch = DateTime.Parse(reader.ReadLine());
                                else
                                    lastPunch = DateTime.Parse(reader.ReadLine());
                            }
                            catch { }
                        }
                    }
                    if (firstPunch != DateTime.MinValue)
                        startLabel.Text = string.Format("Start: {0}", firstPunch);
                    if (lastPunch != DateTime.MinValue)
                        endLabel.Text = string.Format("End: {0}", lastPunch);
                }
            }
            this.Cursor = Cursors.Arrow;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SavePunchFile();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            LoadPunchFile();
        }

        private void recalcButton_Click(object sender, EventArgs e)
        {
            Calculate();
        }
    }
}
