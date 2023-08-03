using power;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibreHardwareMonitor.Hardware;

namespace powerr
{
    public partial class Form1 : Form
    {
        Power myPower = new Power();
        Computer computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,/*
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsNetworkEnabled = true,
                IsStorageEnabled = true*/
        };
        
        public Form1()
        {
            InitializeComponent();
            Thread t = new Thread(new ThreadStart(ChangeLabel));
            t.IsBackground = true;
            t.Start();
        }
        private void ChangeLabel()
        {
            computer.Open();
            while (true)
            {
                myPower = new Power();
                SetLabelText(myPower.Monitor(computer));
                Thread.Sleep(500);
            } 
        }
        private delegate void SetLabelTextDelegate(string sensorInfo);
        private void SetLabelText(string sensorInfo)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new SetLabelTextDelegate(SetLabelText), new object[] { sensorInfo });
                return;
            }
            richTextBox1.Text = sensorInfo.ToString();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Minimized!";
            notifyIcon1.BalloonTipText = "You can expand it anytime you want.";
            notifyIcon1.Text = "Powerr";
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }
        private void Form1_Close(object sender, EventArgs e)
        {
            computer.Close();
            this.Close();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(200);
            }
            else if (FormWindowState.Normal == this.WindowState)
            { notifyIcon1.Visible = false; }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            computer.Close();
            this.Close();   
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                float HourVal = float.Parse(textBox1.Text);
                float AverageVal = float.Parse(textBox2.Text);
                float kw_h_val = float.Parse(textBox3.Text);
                float totalvalue = HourVal * AverageVal * kw_h_val / 1000;
                label3.Text = "Total cost is: " + String.Format("{0:0.00}", totalvalue);
            }
            catch (FormatException fe)
            {
                label3.Text = "Error:" + fe.Message;
            }
            
        }
    }
}
