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
    public partial class Form2 : Form
    {
        private string kstr = "data source=TW1CUSER;database=DC_Envi;User ID=sa;Password=weanwean24";
        private SqlConnection koneksi;
        private DataGridView dgv;
        public Form2()
        {
            InitializeComponent();
            koneksi = new SqlConnection(kstr);
            refreshform();
        }

        private void refreshform()
        {
            nmp.Text = "";
            nmp.Enabled = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
        }

        private void dgview()
        {
            koneksi.Open();
            string str = "select nama_prodi from dbo.Prodi";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dgv.DataSource = ds.Tables[0];
            koneksi.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dgview();
            btnOpen.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            nmp.Enabled = true;
            btnSave.Enabled = true;
            btnClear.Enabled = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string nmProdi = nmp.Text;

            if (nmProdi == "")
            {
                MessageBox.Show("Masukkan Nama Prodi",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                koneksi.Open();
                string str = "insert into dbo.Prodi (nama_Prodi)"
                    + "values(@id)";
                SqlCommand cmd = new SqlCommand(str, koneksi);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("id", nmProdi));
                cmd.ExecuteNonQuery();

                koneksi.Close();
                MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                dgview();
                refreshform();
            }
        }
        private void DataProdi_FormCLosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm = new Form1();
            fm.Show();
            this.Hide();
        }
    }
}
