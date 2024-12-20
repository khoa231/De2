using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using De2.model;

namespace De2
{
    public partial class frmQuanLySanPham : Form
    {
        public frmQuanLySanPham()
        {
            InitializeComponent();
        }



        private void frmSanPham_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadLoaiSP();
            SetButtonState(true); 
        }

        private void LoadData()
        {
            using (var context = new productContextDB())
            {
                dgvSanPham.DataSource = context.Sanphams.ToList();
            }
        }
        private void SetButtonState(bool isInitialLoad)
        {
            btnThem.Enabled = isInitialLoad;
            btnXoa.Enabled = btnSua.Enabled = !isInitialLoad && dgvSanPham.SelectedRows.Count > 0;
            btnLuu.Enabled = btnKluu.Enabled = !isInitialLoad;
        }
        private void LoadLoaiSP()
        {
            using (var context = new productContextDB())
            {
                cboLoaiSP.DataSource = context.Sanphams.Select(sp => sp.LoaiSP).Distinct().ToList();
            }
        }

        private void dgvSanPham_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSanPham.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvSanPham.SelectedRows[0];
                txtMaSP.Text = row.Cells["MSP"].Value.ToString();
                txtTenSP.Text = row.Cells["TenSP"].Value.ToString();
                dtNgaynhap.Value = (DateTime)row.Cells["Ngaynhap"].Value;
                cboLoaiSP.SelectedItem = row.Cells["LoaiSP"].Value;
            }
            SetButtonState(false);
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearInputFields();
            SetButtonState(false);
            txtMaSP.Focus(); 
        }
        private void ClearInputFields()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            dtNgaynhap.Value = DateTime.Now;
            cboLoaiSP.SelectedIndex = -1;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            
            string maSP = dgvSanPham.CurrentRow.Cells["MaSP"].Value.ToString();
            string tenSP = txtTenSP.Text;
            DateTime ngayNhap = dtNgaynhap.Value;
            string maLoai = cboLoaiSP.SelectedValue.ToString();          
            //SuaSanPham(maSP, tenSP, ngayNhap, maLoai);          
            //TaiDanhSachSanPham();
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var context = new productContextDB())
                {
                    var sanpham = context.Sanphams.Find(txtMaSP.Text);
                    if (sanpham != null)
                    {
                        context.Sanphams.Remove(sanpham);
                        context.SaveChanges();
                        LoadData();
                        SetButtonState(true);
                        ClearInputFields();
                    }
                }
            }
        }


        private void btnTim_Click(object sender, EventArgs e)
        {
            using (var context = new productContextDB())
            {
                dgvSanPham.DataSource = context.Sanphams.Where(sp => sp.TenSP.Contains(txtTim.Text)).ToList();
            }
        }


        private void btnKLuu_Click(object sender, EventArgs e)
        {
            LoadData();
            SetButtonState(true);
            ClearInputFields();
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        //private void btnLuu_Click(object sender, EventArgs e)
        //{
        //    using (var context = new productContextDB())
        //    {
        //        if (string.IsNullOrEmpty(txtMaSP.Text))
        //        {
        //            MessageBox.Show("Mã sản phẩm không được để trống!");
        //            return;
        //        }
        //        var sanpham = context.Sanphams.Find(txtMaSP.Text);
        //        if (sanpham == null)
        //        {
        //            sanpham = new SanPham
        //            {
        //                MSP = txtMaSP.Text,
        //                TenSP = txtTenSP.Text,
        //                Ngaynhap = dtNgaynhap.Value,
        //                LoaiSP = cboLoaiSP.SelectedItem.ToString()
        //            };
        //            context.Sanphams.Add(sanpham);
        //        }
        //        else // Cập nhật
        //        {
        //            sanpham.TenSP = txtTenSP.Text;
        //            sanpham.Ngaynhap = dtNgaynhap.Value;
        //            sanpham.LoaiSP = cboLoaiSP.SelectedItem.ToString();
        //        }
        //        context.SaveChanges();
        //        LoadData();
        //        SetButtonState(true);
        //    }
        //}
    }
}
