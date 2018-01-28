using InTheHand.Net.Bluetooth;
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

namespace BluetoothAuthenticationDemo
{
    public partial class WaitingBluetooth : Form
    {
        public WaitingBluetooth()
        {
            InitializeComponent();
        }

        private void CheckBluetooth()
        {
            BluetoothRadio myRadio;
            while(true){
                myRadio = BluetoothRadio.PrimaryRadio;
                if (myRadio != null)
                {
                    break;
                    
                }
            }
            Thread.CurrentThread.Interrupt();
        }

        private void WaitingBluetooth_Load(object sender, EventArgs e)
        {

            Thread a = new Thread(CheckBluetooth);
            a.Start();
            this.Close();
        }
    }
}
