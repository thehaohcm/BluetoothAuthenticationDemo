using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using MySql.Data.MySqlClient;
using InTheHand.Net;
using System.Security.AccessControl;

namespace BluetoothAuthenticationDemo
{
    public delegate void getMacdelegate();
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

        BluetoothClient bc;
        string deviceMac = null;
        int indexRow = 0;
        MySqlConnection conn = null;
        Thread waitingThread = null;
        ShowDialogDelegate di;
        getMacdelegate gmd;
        private delegate void ObjectDelegate(string name, string type, string status);
        private delegate void ChangeScanDeviceBtnDelegate(bool flag, string str);
        private delegate void UpdateDataGridViewDelegate();
        private delegate void StopThreadDelegate();
        private delegate void EnabledButtonsErrorDelegate();
        private delegate void ShowDialogDelegate(bool type);
        private delegate void UpdateDatabaseDelegate();
        bool flagForgotDevice = false;
        bool flag_connect = false;
        string[][] strArray = new string[100][];
        int i_strArray = 0;


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

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(232, 187);
            gmd = new getMacdelegate(getMacDevice);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string stm = "SELECT * FROM folderlock.folder";
            MySqlCommand cmd = new MySqlCommand(stm, conn);
            MySqlDataReader rdr = null;
            rdr = cmd.ExecuteReader();
        }

        private void HandleRequests(object that, BluetoothWin32AuthenticationEventArgs e)
        {
            e.Confirm = true;
        }

        //private void getMacDevice()
        public void getMacDevice()
        {
            try
            {
                connectDatabase();
                string stm = "SELECT * FROM folderlock.device";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                MySqlDataReader rdr = null;
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    deviceMac = rdr.GetString(1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến Cơ sở dữ liệu", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void nextBtn_Click(object sender, EventArgs e)
        {
            string[][] strArray=new string[100][];
            int i_strArray_t = 0;
            connectDatabase();
            string stm = "SELECT * FROM folderlock.folder";
            MySqlCommand cmd = new MySqlCommand(stm, conn);
            MySqlDataReader rdr = null;
            rdr = cmd.ExecuteReader();
            for(i_strArray_t = 0; rdr.Read(); i_strArray_t++)
            {
                strArray[i_strArray_t] = new string[4];
                for(int j = 0; j <= 3; j++)
                {
                    strArray[i_strArray_t][j] = rdr.GetString(j);
                }
            }

            if (forgetPassBtn.Text == "Forgot / Lost Device")
            {
                this.Hide();
                using (Form2 frm2 = new Form2())
                {
                    frm2.ChangeSizeMainFormEvent += new ChangeSizeMainFormDelegate(ChangeSizeForm);
                    frm2.getMacEvent += new getMacdelegate(getMacDevice);
                    frm2.ShowDialog();
                }
                Show();
                if (Form2.AnswerSuccess == true && connectDatabase())
                {
                    try
                    {
                        bool flagunlock = true;
                        string patherr = "";
                        int i = 0;
                        while (i<i_strArray_t)
                        {
                            if (strArray[i][2].Equals("True") && strArray[i][3].Equals("True"))
                            {
                                if (!unlockFolder(strArray[i][0], strArray[i][1]))
                                {
                                    flagunlock = false;
                                    patherr += strArray[i][1] + "\n";
                                }
                            }
                            i++;
                        }
                        if (flagunlock)
                        {
                            this.Size = new Size(463, 383);
                            MessageBox.Show("Đã mở khóa thành công tất cả thư mục", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                            MessageBox.Show("Không thể mở khóa thành công các thư mục sau: \n" + patherr, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        UpdateDataGridView();
                        this.scandevicebtn.Enabled = false;
                        this.forgetPassBtn.Text = "Lock All Folder";
                        this.typeDevice.Text = "None";
                        this.nameDevice.Text = "None";
                        this.statusDevice.Text = "None";
                        flagForgotDevice = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể kết nối với Database", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Form2.AnswerSuccess = false;
                }
            }
            else
            {
                if (connectDatabase())
                {
                    try
                    {
                        cmd = new MySqlCommand(stm, conn);
                        rdr = null;
                        rdr = cmd.ExecuteReader();
                        for (i_strArray = 0; rdr.Read(); i_strArray++)
                        {
                            strArray[i_strArray] = new string[4];
                            for (int j = 0; j <= 3; j++)
                            {
                                strArray[i_strArray][j] = rdr.GetString(j);
                            }
                        }
                        bool flaglock = true;
                        string patherr = "";
                        int i = 0;
                        while (i < i_strArray)
                        {
                            if (strArray[i][2].Equals("True") && strArray[i][3].Equals("False"))
                            {
                                if (!lockFolder(strArray[i][0], strArray[i][1]))
                                {
                                    flaglock = false;
                                    patherr += strArray[i][1] + "\n";
                                }
                            }
                            i++;
                        }
                        if (flaglock)
                            MessageBox.Show("Đã khóa thành công tất cả thư mục", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Không thể khóa thành công các thư mục sau: \n" + patherr, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        UpdateDataGridView();
                        this.scandevicebtn.Enabled = true;
                        this.forgetPassBtn.Text = "Forgot / Lost Device";
                        flagForgotDevice = false;
                        this.Size = new Size(232, 187);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể kết nối với Database", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Frm2_AnserSucessEvent(int width, int height)
        {
            throw new NotImplementedException();
        }

        private void scandevicebtn_Click(object sender, EventArgs e)
        {
            if (scandevicebtn.Text.Equals("Scan Device..."))
            {
                if (waitingThread != null)
                {
                    waitingThread.Abort();
                    waitingThread = null;
                }
                forgetPassBtn.Enabled = false;
                scandevicebtn.Text = "Scanning Device... Click to Stop";
                waitingThread = new Thread(WaitingBluetooth);
                waitingThread.Start();
                
                return;
            }
            if (scandevicebtn.Text.Contains("Stop"))
            {
                StopThreadDelegate sptth;
                if (flag_connect&&scandevicebtn.Text.Equals("Stop"))
                {
                    DialogResult mess = MessageBox.Show("Phần mềm sẽ khóa các thư mục đã định. Bạn có thật sự muốn dừng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (mess == DialogResult.Yes)
                    {
                        sptth = new StopThreadDelegate(stopThread);
                        sptth.Invoke();
                        scandevicebtn.Text = "Scan Device...";
                        this.Size = new Size(232, 187);

                        UpdateDatabase();
                        bool flag_success = true;
                        string pathError = "";
                        int i = 0;
                        while (i < i_strArray)
                        {
                            if (strArray[i][2].Equals("True") && strArray[i][3].Equals("False"))//True
                            {
                                if (!lockFolder(strArray[i][0], strArray[i][1]))
                                {
                                    flag_success = false;
                                    pathError += strArray[i][1] + "\n";
                                }
                            }
                            i++;
                        }
                        if (!flag_success)
                            MessageBox.Show("Không thể khóa thành công tất cả các thư mục sau:\n" + pathError, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero); //refresh Desktop and Windows Explorer
                        return;
                    }
                }
                sptth = new StopThreadDelegate(stopThread);
                sptth.Invoke();
                scandevicebtn.Text = "Scan Device...";
                this.Size = new Size(232, 187);
                return;
            }
        }
        private void WaitingBluetooth()
        {
            try {
                connectDatabase();
                string stm = "SELECT * FROM folderlock.folder";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                MySqlDataReader rdr = null;
                rdr = cmd.ExecuteReader();
                for (i_strArray = 0; rdr.Read(); i_strArray++)
                {
                    strArray[i_strArray] = new string[4];
                    for (int j = 0; j <= 3; j++)
                    {
                        strArray[i_strArray][j] = rdr.GetString(j);
                    }
                }
                bc = new BluetoothClient();
                BluetoothDeviceInfo[] array = bc.DiscoverDevicesInRange();
                getMacDevice();
                bool flag = false;
                BluetoothDeviceInfo bd;
                ChangeScanDeviceBtnDelegate changeScanbtn = new ChangeScanDeviceBtnDelegate(ChangeEnabledScanDeviceBtn);
                UpdateDataGridViewDelegate updateDataGrid = new UpdateDataGridViewDelegate(UpdateDataGridView);
            chay:
                foreach (BluetoothDeviceInfo bdi in array)
                {
                    try
                    {
                        if (bdi.DeviceAddress.ToString().Equals(deviceMac))
                        {
                            bd = bdi;
                            if (bc.Connected == false)
                            {
                                bc.Connect(new BluetoothEndPoint(bd.DeviceAddress, InTheHand.Net.Bluetooth.BluetoothService.Handsfree)); //.BluetoothService.SerialPort
                                bd.SetServiceState(InTheHand.Net.Bluetooth.BluetoothService.Handsfree, true); //.BluetoothService.SerialPort
                            }
                            bd.Refresh();
                            if (bd.Connected)
                            {
                                flag = true;
                                flag_connect = true;
                                di = new ShowDialogDelegate(ShowDialog);
                                di.Invoke(true);
                                ObjectDelegate del = new ObjectDelegate(UpdateForm);
                                del.Invoke(bd.DeviceName.ToString(), bd.ClassOfDevice.MajorDevice.ToString(), bd.Authenticated.ToString());
                                ChangeSizeMainFormDelegate csd = new ChangeSizeMainFormDelegate(ChangeSizeForm);
                                csd.Invoke(463, 383);
                                string bd_DeviceMac = bd.DeviceAddress.ToString();
                                if (connectDatabase())
                                {
                                    try
                                    {
                                        bool flagunlock = true;
                                        string patherr = "";
                                        int i = 0;
                                        while (i < i_strArray)
                                        {
                                            if (strArray[i][2].Equals("True") && strArray[i][3].Equals("True")) //strArray[i][2]: Thuộc tính Auto Lock, strArray[i][3]: Thuộc tính Locked
                                            {
                                                if (!unlockFolder(strArray[i][0], strArray[i][1]))
                                                {
                                                    flagunlock = false;
                                                    patherr += strArray[i][1] + "\n";
                                                }
                                            }
                                            i++;
                                        }
                                        if (!flagunlock)
                                            MessageBox.Show("Không thể mở khóa thành công các thư mục sau: \n" + patherr, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        updateDataGrid();
                                        SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero); //refresh Desktop and Windows Explorer
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Không thể kết nối với Database4", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    changeScanbtn.Invoke(true, "Stop...");
                                    while (true)
                                    {
                                        //if (flag_connect == true) //flag_connect: kiểm tra nếu là true thì refesh, false thì không refresh
                                        bd.Refresh();
                                        bool bd_Connected = bd.Connected;

                                        if (flag_connect == true && bd_Connected == false) //flag_connect: 
                                        {
                                            //không tìm thấy thiết bị, khóa tất cả các thư mục
                                            //update Form status-name-type
                                            //del.Invoke(bd.DeviceName.ToString(), bd.ClassOfDevice.MajorDevice.ToString(), bd.Authenticated.ToString());
                                            del.Invoke("None", "None", "False");

                                            di = new ShowDialogDelegate(ShowDialog);
                                            di.Invoke(false);
                                            try
                                            {
                                                csd = new ChangeSizeMainFormDelegate(ChangeSizeForm);
                                                csd.Invoke(232, 187);
                                                UpdateDatabase();
                                                bool flag_success = true;
                                                string pathError = "";
                                                int i = 0;
                                                while (i < i_strArray)
                                                {
                                                    if (strArray[i][2].Equals("True") && strArray[i][3].Equals("False"))//True
                                                    {
                                                        if (!lockFolder(strArray[i][0], strArray[i][1]))
                                                        {
                                                            flag_success = false;
                                                            pathError += strArray[i][1] + "\n";
                                                        }
                                                    }
                                                    i++;
                                                }
                                                if (!flag_success)
                                                    MessageBox.Show("Không thể khóa thành công tất cả các thư mục sau:\n" + pathError, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                updateDataGrid();
                                                SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero); //refresh Desktop and Windows Explorer
                                                flag_connect = false;
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("Không thể kết nối với Database3", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        if (flag_connect == false && bd_Connected == true && bd_DeviceMac.Equals(deviceMac))
                                        {
                                            //tìm thấy thiết bị, mở khóa tất cả các thư mục
                                            //update Form status-name-type
                                            del.Invoke(bd.DeviceName.ToString(), bd.ClassOfDevice.MajorDevice.ToString(), bd.Authenticated.ToString());
                                            di = new ShowDialogDelegate(ShowDialog);
                                            di.Invoke(true);
                                            try
                                            {
                                                csd = new ChangeSizeMainFormDelegate(ChangeSizeForm);
                                                csd.Invoke(463, 383);

                                                UpdateDatabase();
                                                bool flag_success = true;
                                                string pathError = "";
                                                int i = 0;
                                                while (i < i_strArray)
                                                {
                                                    if (strArray[i][2].Equals("True") && strArray[i][3].Equals("True"))
                                                    {
                                                        if (!unlockFolder(strArray[i][0], strArray[i][1]))
                                                        {
                                                            flag_success = false;
                                                            pathError += strArray[i][1] + "\n";
                                                        }
                                                    }
                                                    i++;
                                                }
                                                if (!flag_success)
                                                    MessageBox.Show("Không thể khóa thành công tất cả các thư mục sau:\n" + pathError, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                updateDataGrid();
                                                SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero); //Refresh Desktop and Windows Explorer
                                                flag_connect = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("Không thể kết nối với Database2", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }

                                        if (flag_connect == false && bd_Connected == false)
                                        {
                                            bc = null;
                                            bc = new BluetoothClient();
                                            array = bc.DiscoverDevicesInRange();
                                            foreach (BluetoothDeviceInfo bd_t in array)
                                            {
                                                if (bd_t.DeviceAddress.ToString().Equals(deviceMac))
                                                {
                                                    bd = bd_t;
                                                    bc.Connect(new BluetoothEndPoint(bd.DeviceAddress, InTheHand.Net.Bluetooth.BluetoothService.Handsfree)); //.BluetoothService.SerialPort
                                                    bd.SetServiceState(InTheHand.Net.Bluetooth.BluetoothService.Handsfree, true); //.BluetoothService.SerialPort
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Không thể kết nối với Database1", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else {
                                MessageBox.Show("Thiết bị chưa được kết nối", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                bc = null;
                                flag = false;
                            }
                            flag = true;
                            break;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                if (!flag)
                {
                    //MessageBox.Show("Không tìm thấy đúng thiết bị cần kết nối", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //changeScanbtn(true, "Scan Device...");
                    //StopThreadDelegate sthread = new StopThreadDelegate(stopThread);
                    //sthread.Invoke();
                    //return;
                    goto chay;
                }
                //else {
                changeScanbtn.Invoke(true, "stop");
                StopThreadDelegate stpth = new StopThreadDelegate(stopThread);
                stpth.Invoke();
                //}
            }
            catch (Exception ex)
            {
                int i = 0;
                while (i < i_strArray)
                {
                    if (strArray[i][2].Equals("True") && strArray[i][3].Equals("False"))//True
                    {
                        lockFolder(strArray[i][0], strArray[i][1]);
                    }
                    i++;
                }
                
            }
        }

        private bool lockFolder(string stt, string path)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(path);
                if (d.Exists == false)
                    return false;
                connectDatabase();
                string stm = "UPDATE `folderlock`.`folder` SET `Locked`='True' WHERE `stt`='" + stt + "'";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.ExecuteNonQuery();
                DirectoryInfo di = new DirectoryInfo(path);
                if (di.Exists)
                {
                    di.Attributes = FileAttributes.Directory | FileAttributes.Hidden | FileAttributes.System;
                    FileSecurity fs = File.GetAccessControl(path);
                    fs.AddAccessRule(new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl, AccessControlType.Deny));
                    File.SetAccessControl(path, fs);
                }
                else
                {
                    removeElement(stt);
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool unlockFolder(string stt, string path)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(path);
                if (d.Exists == false)
                {
                    removeElement(stt);
                    return false;
                }
                FileSecurity fs = File.GetAccessControl(path);
                fs.RemoveAccessRule(new FileSystemAccessRule(Environment.UserName, FileSystemRights.FullControl, AccessControlType.Deny));
                File.SetAccessControl(path, fs);
                d.Attributes = FileAttributes.Normal;
                connectDatabase();
                string stm = "UPDATE `folderlock`.`folder` SET `Locked`='False' WHERE `stt`='" + stt + "'";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private bool checkFolder(string pathFolder)
        {
            connectDatabase();
            string stm = "SELECT * FROM folderlock.folder";
            MySqlCommand cmd = new MySqlCommand(stm, conn);
            MySqlDataReader rdr = null;
            rdr = cmd.ExecuteReader();
            for (i_strArray = 0; rdr.Read(); i_strArray++)
            {
                if (pathFolder.Equals(rdr.GetString(1)))
                    return true;
            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo d = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                string selectedpath = d.Parent.FullName + "\\" + d.Name;
                //if (folderBrowserDialog1.SelectedPath.LastIndexOf(".{") == -1)
                if(!checkFolder(selectedpath))
                {
                    try
                    {
                        connectDatabase();
                        using (MySqlCommand command = new MySqlCommand("INSERT INTO `folderlock`.`folder` (`pathFolder`, `Auto Lock`, `Locked`) VALUES (@path, @lockac, @locked)", conn))
                        {
                            command.Parameters.Add(new MySqlParameter("path", selectedpath));
                            command.Parameters.Add(new MySqlParameter("lockac", "True"));
                            command.Parameters.Add(new MySqlParameter("locked", "False"));

                            command.ExecuteNonQuery();
                        }

                        this.folderTableAdapter1.Fill(this.folderlockDataSet4.folder);
                        folderBindingSource.ResetAllowNew();

                        dataGridView1.Update();
                        dataGridView1.Refresh();
                        indexRow = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Không thể thêm vào cơ sở dữ liệu", "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Không thể thêm thư mục " + selectedpath + " vào cơ sở dữ liệu", "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //private string getstatus(string stat)
        //{
        //    for (int i = 0; i < 6; i++)
        //        if (stat.LastIndexOf(lockFolderStr) != -1)
        //            stat = stat.Substring(stat.LastIndexOf("."));
        //    return stat;
        //}

        private void Form1_Load(object sender, EventArgs e)
        {
            if (BluetoothRadio.IsSupported == false)
            {
                MessageBox.Show("Máy tính chưa được mở Bluetooth. Xin vui lòng mở kết nối Bluetooth và chạy lại chương trình", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }

            // TODO: This line of code loads data into the 'folderlockDataSet4.folder' table. You can move, or remove it, as needed.
            this.folderTableAdapter1.Fill(this.folderlockDataSet4.folder);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (indexRow > -1)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[indexRow];

                string stt = Convert.ToString(selectedRow.Cells[0].Value);
                string path = Convert.ToString(selectedRow.Cells[1].Value);
                string autoLock = Convert.ToString(selectedRow.Cells[2].Value);
                string locked = Convert.ToString(selectedRow.Cells[3].Value);
                if (autoLock.Equals("True"))
                {
                    if (locked.Equals("True"))
                    {
                        if (unlockFolder(stt, path))
                            MessageBox.Show("Đã mở khóa thành công thư mục " + path, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Không thể mở khóa thành công thư mục " + path, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (lockFolder(stt, path))
                            MessageBox.Show("Đã Khóa thành công thư mục " + path, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Không thể khóa thư mục " + path, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    this.folderTableAdapter1.Fill(this.folderlockDataSet4.folder);
                    dataGridView1.Update();
                    dataGridView1.Refresh();
                    indexRow = 0;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flagForgotDevice == true)
            {
                DialogResult mess = MessageBox.Show("Phần mềm sẽ khóa các thư mục đã định. Bạn có thật sự muốn dừng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (mess == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                connectDatabase();
                string stm = "SELECT * FROM folderlock.folder;";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                MySqlDataReader rdr = null;
                rdr = cmd.ExecuteReader();
                bool flaglock = true;
                string pathlockerr = "";
                while (rdr.Read())
                {
                    if (rdr.GetString(2).Equals("True") && rdr.GetString(3).Equals("False"))
                    {
                        if (!lockFolder(rdr.GetString(0), rdr.GetString(1)))
                        {
                            flaglock = false;
                            pathlockerr += rdr.GetString(1) + "\n";
                        }
                    }
                }
                if (flaglock == false)
                    MessageBox.Show("Không thể khóa thành công các thư mục sau: \n" + pathlockerr, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                flagForgotDevice = false;
                return;
            }


            if (waitingThread != null)
            {
                DialogResult mess = MessageBox.Show("Phần mềm sẽ khóa các thư mục đã định. Bạn có thật sự muốn dừng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (mess == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                if (conn != null)
                    conn.Close();
                waitingThread.Abort();
                waitingThread = null;
                connectDatabase();
                string stm = "SELECT * FROM folderlock.folder;";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                MySqlDataReader rdr = null;
                rdr = cmd.ExecuteReader();
                bool flaglock = true;
                string pathlockerr = "";
                while (rdr.Read())
                {
                    if (rdr.GetString(2).Equals("True") && rdr.GetString(3).Equals("False"))
                    {
                        if (!lockFolder(rdr.GetString(0), rdr.GetString(1)))
                        {
                            flaglock = false;
                            pathlockerr += rdr.GetString(1) + "\n";
                        }
                        //coding...
                    }
                }
                if (flaglock == false)
                    MessageBox.Show("Không thể khóa thành công các thư mục sau: \n" + pathlockerr, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                if (conn != null)
            {
                conn.Close();
                conn = null;
            }
        }

        private void UpdateForm(string name, string type, string status)
        {
            if (InvokeRequired)
            {
                ObjectDelegate method = new ObjectDelegate(UpdateForm);
                Invoke(method, name, type, status);
                return;
            }

            this.nameDevice.Text = name;
            this.typeDevice.Text = type;
            this.statusDevice.Text = status;
            this.forgetPassBtn.Enabled = false;
        }

        private void ChangeSizeForm(int width, int height)
        {
            if (InvokeRequired)
            {
                ChangeSizeMainFormDelegate method = new ChangeSizeMainFormDelegate(ChangeSizeForm);
                Invoke(method, width, height);
                return;
            }
            this.Size = new Size(width, height);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private bool removeElement(string stt)
        {
            try
            {
                connectDatabase();
                string stm = "DELETE FROM `folderlock`.`folder` WHERE `stt`='" + stt + "'";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void removeEleBtn_Click(object sender, EventArgs e)
        {
            if (indexRow > -1)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[indexRow];

                string stt = Convert.ToString(selectedRow.Cells[0].Value);
                string pathFolder = Convert.ToString(selectedRow.Cells[1].Value);
                string locked = Convert.ToString(selectedRow.Cells[3].Value);
                if (locked.Equals("True"))
                {
                    MessageBox.Show("Vui lòng mở khóa thư mục trước khi loại khỏi danh sách thư mục này", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
                DialogResult messRe = MessageBox.Show("Bạn có chắc chắn muốn loại bỏ thư mục ra khỏi danh sách?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (messRe == DialogResult.No || messRe == DialogResult.None)
                    return;
                if (removeElement(stt))
                    MessageBox.Show("Đã loại bỏ thành công thư mục " + pathFolder, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Không thể loại bỏ thành công thư mục " + pathFolder, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.folderTableAdapter1.Fill(this.folderlockDataSet4.folder);
                dataGridView1.Update();
                dataGridView1.Refresh();
                indexRow = 0;
            }
        }

        private bool TurnOnAutoLockFolder(string stt)
        {
            try
            {
                connectDatabase();
                string stm = "UPDATE `folderlock`.`folder` SET `Auto Lock`='True' WHERE `stt`='" + stt + "';";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool TurnOffAutoLockFolder(string stt)
        {
            try
            {
                connectDatabase();
                string stm = "UPDATE `folderlock`.`folder` SET `Auto Lock`='False' WHERE `stt`='" + stt + "';";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void onoffAutoLockBtn_Click(object sender, EventArgs e)
        {
            if (indexRow > -1)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[indexRow];

                string stt = Convert.ToString(selectedRow.Cells[0].Value);
                string path = Convert.ToString(selectedRow.Cells[1].Value);
                string autoLock = Convert.ToString(selectedRow.Cells[2].Value);
                string locked = Convert.ToString(selectedRow.Cells[3].Value);
                if (autoLock.Equals("True"))
                {
                    if (TurnOffAutoLockFolder(stt))
                        MessageBox.Show("Đã tắt tính năng Auto Lock của thư mục " + path, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Không thể tắt tính năng Auto Lock của thư mục " + path, "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (TurnOnAutoLockFolder(stt))
                        MessageBox.Show("Đã bật tính năng Auto Lock của thư mục " + path, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                        MessageBox.Show("Không thể mở tính năng Auto Lock của thư mục " + path, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                this.folderTableAdapter1.Fill(this.folderlockDataSet4.folder);
                dataGridView1.Update();
                dataGridView1.Refresh();
                indexRow = 0;
            }
        }

        private void ChangeEnabledScanDeviceBtn(bool flag, string str)
        {
            if (InvokeRequired)
            {
                ChangeScanDeviceBtnDelegate method = new ChangeScanDeviceBtnDelegate(ChangeEnabledScanDeviceBtn);
                Invoke(method, flag, str);
                return;
            }
            scandevicebtn.Enabled = flag;
            scandevicebtn.Text = str;

        }

        private void UpdateDataGridView()
        {
            if (InvokeRequired)
            {
                UpdateDataGridViewDelegate method = new UpdateDataGridViewDelegate(UpdateDataGridView);
                Invoke(method);
                return;
            }
            this.folderTableAdapter1.Fill(this.folderlockDataSet4.folder);
            dataGridView1.Update();
            dataGridView1.Refresh();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            indexRow = e.RowIndex;
            unlockBtn.Enabled = true;
            onoffAutoLockBtn.Enabled = true;
            removeEleBtn.Enabled = true;
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData & Keys.KeyCode)
            {
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                case Keys.Left:
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    break;
            }
        }

        public void recoveryForm(bool check) //hàm của delegate chạy từ Form2 gửi tín hiệu về Form1 khi kết thúc để kiểm tra việc trả lời câu hỏi là đúng hay sai
        {
            if (check)
            {
                this.Size = new Size(463, 383);
            }
        }

        private void stopThread()
        {
            if (InvokeRequired)
            {
                StopThreadDelegate stpth = new StopThreadDelegate(stopThread);
                Invoke(stpth);
                return;
            }

            if (bc.Connected)
            {
                bc.Close();
                bc = null;
            }
            if (waitingThread != null)
            {
                waitingThread.Abort();
                waitingThread = null;
            }
            this.forgetPassBtn.Enabled = true;
        }

        private void enabledButtonsError()
        {
            if (InvokeRequired)
            {
                EnabledButtonsErrorDelegate method = new EnabledButtonsErrorDelegate(enabledButtonsError);
                Invoke(method);
                return;
            }
            forgetPassBtn.Enabled = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            using (Form2 frm2 = new Form2("Thay đổi thiết bị"))
            {
                frm2.ShowDialog();
            }
            Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (Form2 frm2 = new Form2("Thay đổi Câu hỏi Bí mật"))
            {
                frm2.ShowDialog();
            }
            Show();
        }

        private void ShowDialog(bool type)
        {
            if (InvokeRequired)
            {
                ShowDialogDelegate method = new ShowDialogDelegate(ShowDialog);
                Invoke(method, type);
                return;
            }
            using (DialogForm1 di = new DialogForm1(type))
            {
                di.ShowDialog(this);
            }
        }

        private void UpdateDatabase()
        {
            try
            {
                if (conn != null)
                    conn.Close();
                connectDatabase();
                string stm = "SELECT * FROM folderlock.folder;";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                MySqlDataReader rs = null;
                rs = cmd.ExecuteReader();
                for (i_strArray = 0; rs.Read(); i_strArray++)
                {
                    strArray[i_strArray] = new string[4];
                    for (int j = 0; j <= 3; j++)
                    {
                        strArray[i_strArray][j] = rs.GetString(j);
                    }
                }
                //return true;
            }
            catch (Exception ex)
            {
                //return false;
            }
        }
    }
}
