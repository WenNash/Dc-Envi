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
    public partial class DataMhs : Form
    {
        private string kstr = "data source=TW1CUSER;database=DC_Envi;User ID=sa;Password=weanwean24";
        private SqlConnection koneksi;
        private string nim, nama, alamat, jk, prodi;
        private DateTime tgl;
        BindingSource customerBindingSource = new BindingSource();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtNIM.Text = "";
            txtNm.Text = "";
            txtAL.Text = "";
            dtTGL.Value = DateTime.Today;
            txtNIM.Enabled = true;
            txtNm.Enabled = true;
            cbxJK.Enabled = true;
            txtAL.Enabled = true;
            dtTGL.Enabled = true;
            cbxProdi.Enabled = true;
            Prodicbx();
            btnSave.Enabled = true;
            btnClear.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        public DataMhs()
        {
            InitializeComponent();
            koneksi = new SqlConnection(kstr);
            this.bnMhs.BindingSource = this.customerBindingSource;
            refreshform();
        }
        private void refreshform()
        {
            txtNIM.Enabled = false;
            txtNm.Enabled = false;
            cbxJK.Enabled = false;
            txtAL.Enabled = false;
            dtTGL.Enabled = false;
            cbxProdi.Enabled = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnAdd.Enabled = true;
            clearbinding();
            DataMhs_Load();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            nim = txtNIM.Text;
            nama = txtNm.Text;
            jk = cbxJK.Text;
            alamat = txtAL.Text;
            tgl = dtTGL.Value;
            prodi = cbxProdi.Text;
            int hs = 0;
            koneksi.Open();
            string strs = "select id_prodi from dbo.Prodi where nama_prodi = @dd";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@dd", prodi));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                hs = int.Parse(dr["id_prodi"].ToString());
            }
            dr.Close();
            string str = "insert into dbo.mahasiswa (nim, nama_mahasiswa, jenis_kelamin, " +
                "alamat, tgl_lahir, id_prodi)" +
                "values(@NIM, @Nm, @JK, @AL, @TGL, @Idp)";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            cmd.CommandType = CommandType.Text;

            cmd.Parameters.Add(new SqlParameter("NIM", nim));
            cmd.Parameters.Add(new SqlParameter("Nm", nama));
            cmd.Parameters.Add(new SqlParameter("JK", jk));
            cmd.Parameters.Add(new SqlParameter("AL", alamat));
            cmd.Parameters.Add(new SqlParameter("TGL", tgl));
            cmd.Parameters.Add(new SqlParameter("prodi", prodi));
            cmd.ExecuteNonQuery();

            koneksi.Close();

            MessageBox.Show("Data Berhasil DiSimpan", "Sukses", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            refreshform();
        }

        private void DataMhs_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void DataMhs_Load()
        {
            koneksi.Open();
            SqlDataAdapter dataAdapter1 =
                new SqlDataAdapter(new SqlCommand("Select m.nim, m.nama_mahasiswa, " +
                "m.alamat, m.jenis_kelamin, m.tgl_lahir, p.nama_Prodi From dbo.mahasiswa m " +
                "join dbo.Prodi p on m.id_prodi = p.id_prodi", koneksi));
            DataSet ds = new DataSet();
            dataAdapter1.Fill(ds);

            this.customerBindingSource.DataSource = ds.Tables[0];
            this.txtNIM.DataBindings.Add(
                new Binding("Text",
                this.customerBindingSource, "NIM", true));
            this.txtNm.DataBindings.Add(
                new Binding("Text",
                this.customerBindingSource, "nama_mahasiswa", true));
            this.txtAL.DataBindings.Add(
                new Binding("Text",
                this.customerBindingSource, "Alamat", true));
            this.cbxJK.DataBindings.Add(
                new Binding("Text",
                this.customerBindingSource, "Jenis_Kel", true));
            this.dtTGL.DataBindings.Add(
                new Binding("Text",
                this.customerBindingSource, "tgl_Lahir", true));
            this.cbxProdi.DataBindings.Add(
                new Binding("Text",
                this.customerBindingSource, "nama_prodi", true));
            koneksi.Close();
        }
        private void clearbinding()
        {
            this.txtNIM.DataBindings.Clear();
            this.txtNm.DataBindings.Clear();
            this.txtAL.DataBindings.Clear();
            this.cbxJK.DataBindings.Clear();
            this.dtTGL.DataBindings.Clear();
            this.cbxProdi.DataBindings.Clear();
        }
        private void Prodicbx()
        {
            koneksi.Open();
            string str = "selet nama_prodi from bdo.Prodi";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();

            cbxProdi.DisplayMember = "nama_prodi";
            cbxProdi.ValueMember = "id_prodi";
            cbxProdi.DataSource = ds.Tables[0];
        }
    }
}
