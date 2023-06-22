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


namespace Disconnect_Enviroment
{
    public partial class Form4 : Form
    {
        private string kstr = "data source=TW1CUSER;database=DC_Envi;User ID=sa;Password=weanwean24";
        private SqlConnection koneksi;
        private DataGridView dgv;
        public Form4()
        {
            InitializeComponent();
            koneksi = new SqlConnection(kstr);
            refreshform();
        }

        private void Dgview()
        {
            koneksi.Open();
            string str = "select * from dbo.status_mahasiswa";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dgv.DataSource = ds.Tables[0];
            koneksi.Close();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Dgview();
            btnOpen.Enabled = false;
        }

        private void refreshform()
        {
            cbxNM.Enabled = false;
            cbxSM.Enabled = false;
            cbxTM.Enabled = false;
            cbxNM.SelectedIndex = -1;
            cbxSM.SelectedIndex = -1;
            cbxTM.SelectedIndex = -1;
            txtNIM.Visible = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnAdd.Enabled = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cbxNM.Enabled = true;
            cbxSM.Enabled = true;
            cbxTM.Enabled = true;
            txtNIM.Visible = true;
            cbTm();
            cbNama();
            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
        }
        private void cbNama()
        {
            koneksi.Open();
            string str = "select nama_mahasiswa from dbo.mahasiswa where " +
                "not EXISTS(select id_status from dbo.status_mahasiswa where " +
                "status_mahasiswa.nim = mahasiswa.nim";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            koneksi.Close();

            cbxNM.DisplayMember = "nama_mahasiswa";
            cbxNM.ValueMember = "NIM";
            cbxNM.DataSource = ds.Tables[0];
            
        }
        private void cbTm()
        {
            int y = DateTime.Now.Year - 2010;
            string[] type = new string[y];
            int i = 0;
            for (i = 0; i < type.Length; i++)
            {
                if (i == 0)
                {
                    cbxTM.Items.Add("2010");
                }
                else
                {
                    int l = 2010 + i;
                    cbxTM.Items.Add(1.ToString());
                }
            }
        }

        private void cbxNM_TextChanged(object sender, EventArgs e)
        {
            koneksi.Open();
            string nim = "";
            string strs = "select NIM from dbo.mahasiswa where nama_mahasiswa = @nm";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@nm", cbxNM.Text));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                nim = dr["NIM"].ToString();
            }
            dr.Close();
            koneksi.Close();

            txtNIM.Text = nim;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nim = txtNIM.Text;
            string Sm = cbxSM.Text;
            string Tm = cbxTM.Text;
            int count = 0;
            string kode = "";
            string kdsm = "";
            koneksi.Open();

            string str = "select count(*) from dbo.status_mahasiswa";
            SqlCommand cm = new SqlCommand(str, koneksi);
            count = (int)cm.ExecuteScalar();

            if (count == 0)
            {
                kdsm = "1";
            }
            else
            {
                string s = "select max(id_status) from dbo.status_mahasiswa";
                SqlCommand cmma = new SqlCommand(s, koneksi);
                int kd = (int)cmma.ExecuteScalar();
                int cn = kd + 1;
                kode = Convert.ToString(cn);
                kdsm = kode;
            }
            string stri = "insert into dbo.status_mahasiswa (id_status, nim, " +
                "status_mahasiswa, tahun_masuk)" +
                "values(@ids, @NIM, @sm, @tm)";
            SqlCommand cmd = new SqlCommand(stri, koneksi);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("ids", kdsm));
            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("sm", Sm));
            cmd.Parameters.Add(new SqlParameter("tm", Tm));
            cmd.ExecuteNonQuery();
            koneksi.Close();

            MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            refreshform();
            Dgview();
        }

        private void DataStMhs_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }
    }
}
