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
    public partial class Form3 : Form
    {
        MySqlConnection conn = null;
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

        public Form3()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim().Equals(""))
            {
                MessageBox.Show("Bạn chưa nhập vào câu hỏi","Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (textBox2.Text.Trim().Equals(""))
            {
                MessageBox.Show("Bạn chưa nhập vào câu trả lời","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;
            }

            if (connectDatabase())
            {
                try {
                    string stm = "UPDATE `folderlock`.`recovery` SET `question`='" + textBox1.Text + "', `answer`='" + textBox2.Text + "' WHERE `idch`='1'";
                    MySqlCommand cmd = new MySqlCommand(stm, conn);
                    cmd.ExecuteNonQuery();
                    this.Close();
                    MessageBox.Show("Đã cập nhật câu hỏi thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Không thể thay đổi nội dung câu hỏi trong Cơ sở Dữ liệu", "Không thành công", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
