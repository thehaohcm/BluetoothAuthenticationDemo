using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BluetoothAuthenticationDemo
{
    public delegate void ChangeSizeMainFormDelegate(int width, int height);

    public partial class Form2 : Form
    {
        public event ChangeSizeMainFormDelegate ChangeSizeMainFormEvent = null;
        public event getMacdelegate getMacEvent = null;
        public static bool AnswerSuccess = false;
        private MySqlConnection conn = null;

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(string title)
        {
            InitializeComponent();
            this.Text = title;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                connectDatabase();
                string stm = "SELECT * FROM folderlock.recovery";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                MySqlDataReader rdr = null;
                rdr = cmd.ExecuteReader();
                if(rdr.Read())
                {
                    textBox2.Text=rdr.GetString(1);
                }
                textBox1.Focus();
            }
            catch
            {

            }
        }

        private bool connectDatabase()
        {
            string cs = @"server=localhost;userid=root;password=root;database=folderlock";
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Text.Equals("Thay đổi Câu hỏi Bí mật"))
            {
                using (Form3 frm3 = new Form3())
                {
                    frm3.ShowDialog();
                }
                Close();
                return;
            }

            if (this.Text.Equals("Thay đổi thiết bị"))
            {
                InTheHand.Windows.Forms.SelectBluetoothDeviceDialog dialog = new InTheHand.Windows.Forms.SelectBluetoothDeviceDialog();
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    BluetoothDeviceInfo selected = dialog.SelectedDevice;
                    BluetoothClient bc = new BluetoothClient();
                    if (selected.Authenticated == false)
                    {
                        bool paired = BluetoothSecurity.PairRequest(selected.DeviceAddress, null);
                        if (!paired)
                        {
                            //Không thể repair
                            MessageBox.Show("Không thể Pair thiết bị", "Pair không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    if (bc.Connected == false)
                    {
                        try
                        {
                            bc.Connect(new BluetoothEndPoint(selected.DeviceAddress, InTheHand.Net.Bluetooth.BluetoothService.Handsfree)); //.BluetoothService.SerialPort
                            selected.SetServiceState(InTheHand.Net.Bluetooth.BluetoothService.Handsfree, true); //.BluetoothService.SerialPort
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Thiết bị Bluetooth không hỗ trợ dịch vụ Hands-Free", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                            return;
                        }
                    }
                    selected.Refresh();
                    if (selected.Connected)
                    {
                        try
                        {
                            connectDatabase();
                            string stm1 = "UPDATE `folderlock`.`device` SET `MacAddress`='" + selected.DeviceAddress.ToString() + "' WHERE `id`='1'";
                            MySqlCommand cmd1 = new MySqlCommand(stm1, conn);
                            cmd1.ExecuteNonQuery();

                            AnswerSuccess = true;
                            Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Không thể thay đổi thiết bị trong cơ sở dữ liệu", "Không thể thay đổi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        //Không thể connect
                        MessageBox.Show("Không thể kết nối với thiết bị", "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                }
                return;
            }

            if (textBox2.Text.Trim().Equals(""))
            {
                MessageBox.Show("Bạn vui lòng chọn câu hỏi", "Chọn câu hỏi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox1.Text.Trim().Equals(""))
            {
                MessageBox.Show("Bạn vui lòng nhập vào câu trả lời", "Trả lời câu hỏi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            connectDatabase();
            string stm = "SELECT answer FROM folderlock.recovery where question like '" + textBox2.Text + "'";
            MySqlCommand cmd = new MySqlCommand(stm, conn);
            MySqlDataReader rdr = null;
            rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                if (rdr.GetString(0).Equals(textBox1.Text))
                {
                    //trả lời đúng
                    DialogResult mess = MessageBox.Show("Đã trả lời đúng câu hỏi. Bạn có muốn chọn thiết bị điện thoại khác để đăng nhập?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (mess == DialogResult.Yes)
                    {
                        InTheHand.Windows.Forms.SelectBluetoothDeviceDialog dialog = new InTheHand.Windows.Forms.SelectBluetoothDeviceDialog();
                        DialogResult result = dialog.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            BluetoothDeviceInfo selected = dialog.SelectedDevice;
                            //BluetoothDeviceInfo bd;
                            BluetoothClient bc = new BluetoothClient();
                            if (selected.Authenticated == false)
                            {
                                bool paired = BluetoothSecurity.PairRequest(selected.DeviceAddress, null);
                                if (!paired)
                                {
                                    //Không thể repair
                                    MessageBox.Show("Không thể Pair thiết bị", "Pair không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            if (bc.Connected == false)
                            {
                                try
                                {
                                    bc.Connect(new BluetoothEndPoint(selected.DeviceAddress, InTheHand.Net.Bluetooth.BluetoothService.Handsfree)); //.BluetoothService.SerialPort
                                    selected.SetServiceState(InTheHand.Net.Bluetooth.BluetoothService.Handsfree, true); //.BluetoothService.SerialPort
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Thiết bị Bluetooth không hỗ trợ dịch vụ Hands-Free", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Close();
                                    return;
                                }
                            }
                            selected.Refresh();
                            if (selected.Connected)
                            {
                                try
                                {
                                    connectDatabase();
                                    string stm1 = "UPDATE `folderlock`.`device` SET `MacAddress`='" + selected.DeviceAddress.ToString() + "' WHERE `id`='1'";
                                    MySqlCommand cmd1 = new MySqlCommand(stm1, conn);
                                    cmd1.ExecuteNonQuery();
                                    MessageBox.Show("Đã thay đổi thành công thiết bị đăng truy cập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    AnswerSuccess = false;
                                    getMacEvent();
                                    Close();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Không thể thay đổi thiết bị trong cơ sở dữ liệu", "Không thể thay đổi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                //Không thể connect
                                MessageBox.Show("Không thể kết nối với thiết bị", "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            return;
                        }
                        return;
                    }
                    else
                    {
                        if (ChangeSizeMainFormEvent != null)
                        {
                            ChangeSizeMainFormEvent(463, 383);
                        }
                        AnswerSuccess = true;
                        this.Close();
                        return;
                    }
                    //Close();
                }
                else
                {
                    //trả lời sai
                    DialogResult mess1 = MessageBox.Show("Bạn đã trả lời sai câu hỏi. Bạn có muốn trả lời lại câu hỏi không?", "Trả lời sai câu hỏi", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (mess1 == DialogResult.No)
                        this.Close();

                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy câu hỏi trong Cơ Sở Dữ Liệu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }


        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn != null)
            {
                conn.Close();
            }
            //Form1.ActiveForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
