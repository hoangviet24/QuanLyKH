using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyKhachHang
{
    public partial class Form1 : Form
    {
        int i = 1;
        SqlConnection connection;
        SqlCommand command;
        public Form1()
        {
            InitializeComponent();
        }
        string strConn = "Data Source=HOANGVIET\\SQLEXPRESS;User ID=sa;Password=1111;Database=QLKH";

        private void Form1_Load(object sender, EventArgs e)
        {
            HienThiDanhMuc();
        }
        private void ConnecSQL()
        {
            if (connection == null)
            {
                connection = new SqlConnection(strConn);
            }
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        private void HienThiDanhMuc()
        {
            ConnecSQL();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from LoaiKH";
            cmd.Connection = connection;

            SqlDataReader dr = cmd.ExecuteReader();

            //Hien thi du lieu
            cmbLoaiKH.Items.Clear();
            while (dr.Read())
            {
                int MaLoaiKH = dr.GetInt32(0);
                string TenLoai = dr.GetString(1);
                cmbLoaiKH.Items.Add(MaLoaiKH + "-" + TenLoai);
            }
            dr.Close();
        }
        int MaKH = -1;
        private void cmbLoaiKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLoaiKH.SelectedIndex < 0)
                return;
            string dong = cmbLoaiKH.SelectedItem.ToString();
            string[] arr = dong.Split('-');
            MaKH = int.Parse(arr[0]);
            HienThiSanPhamTheoDM(MaKH);
        }
        private void HienThiSanPhamTheoDM(int MaKH)
        {
            ConnecSQL();
            string query = "select * from KhachHang where MaLoaiKH = " + MaKH;
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            SqlCommandBuilder builder = new SqlCommandBuilder();
            DataSet ds = new DataSet();
            adapter.Fill(ds, "KhachHang");
            //Hien thi le luoi
            LuoiSP.DataSource = ds.Tables[0];
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtMa.Text = i.ToString();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from KhachHang", connection);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            DataSet ds = new DataSet();
            adapter.Fill(ds, "KhachHang");
            DataRow row = ds.Tables["KhachHang"].NewRow();
            row["MaKH"] = txtMa.Text;
            row["Ten"] = txtTen.Text;
            row["Gender"] = null;
            if (rdoNam.Checked)
            {
                row["Gender"] = "Nam";
            }
            else row["Gender"] = "Nữ"; 
            
            row["Phone"] = Convert.ToInt32(txtPhone.Text);
            if (cmbLoaiKH.SelectedIndex == -1)
                return;
            string dong = cmbLoaiKH.SelectedItem.ToString();
            string[] arr = dong.Split('-');
            row["MaLoaiKH"] = int.Parse(arr[0]);

            //Luu vao datatbase
            ds.Tables["KhachHang"].Rows.Add(row);

            int kq = adapter.Update(ds.Tables["KhachHang"]);
            if (kq > 0)
            {
                MessageBox.Show("Them SP thanh cong");
                i++;
                txtMa.Text = i.ToString();
            }
            else
            {
                MessageBox.Show("Them SP that bai");
            }
            HienThiSanPhamTheoDM(MaKH);
            
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
           
            if (!string.IsNullOrEmpty(txtMa.Text))
            {
                command = connection.CreateCommand();
                command.CommandText = "delete from KhachHang where MaKH =  " + txtMa.Text;
                command.ExecuteNonQuery();
                int result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Xóa dữ liệu không thành công");
                    
                }
                else
                {
                    MessageBox.Show("Xóa dữ liệu thành công");
                    txtMa.Text = i.ToString();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.");
            }
            HienThiSanPhamTheoDM(MaKH);
        }

        private void LuoiSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = LuoiSP.CurrentRow.Index;
            txtMa.Text = LuoiSP.Rows[i].Cells[0].Value.ToString();
            txtTen.Text = LuoiSP.Rows[i].Cells[1].Value.ToString();
            txtPhone.Text = LuoiSP.Rows[i].Cells[3].Value.ToString();
            cmbLoaiKH.Text = LuoiSP.Rows[i].Cells[4].Value.ToString();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string dong = cmbLoaiKH.SelectedItem.ToString();
            string[] arr = dong.Split('-');
            MaKH = int.Parse(arr[0]);
            command = connection.CreateCommand();
            command.CommandText = "update KhachHang set Ten = '" + txtTen.Text + "', Phone = '" + txtPhone.Text + "', MaLoaiKH = '" + MaKH + "' where MaKH = '" + txtMa.Text + "'";
            command.ExecuteNonQuery();
            int result = command.ExecuteNonQuery();

            if (result > 0)
            {
                MessageBox.Show("Cập nhật liệu thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật liệu không thành công");
            }
            HienThiSanPhamTheoDM(MaKH);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ConnecSQL();
            string query = "select * from KhachHang";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            SqlCommandBuilder builder = new SqlCommandBuilder();
            DataSet ds = new DataSet();
            adapter.Fill(ds, "KhachHang");
            //Hien thi le luoi
            LuoiSP.DataSource = ds.Tables[0];
        }

        private void btnDelAll_Click(object sender, EventArgs e)
        {
            i = 1;
            if (!string.IsNullOrEmpty(txtMa.Text))
            {
                command = connection.CreateCommand();
                command.CommandText = "delete from KhachHang";
                command.ExecuteNonQuery();
                int result = command.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Xóa dữ liệu không thành công");

                }
                else
                {
                    MessageBox.Show("Xóa dữ liệu thành công");
                    txtMa.Text = i.ToString();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xóa.");
            }
            HienThiSanPhamTheoDM(MaKH);
        }
    }
}
