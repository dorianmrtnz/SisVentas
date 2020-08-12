using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace PUNTO_DE_VENTA_CSHARP
{
    public partial class usuariosok : Form
    {
        public usuariosok()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Cargar_estado_de_iconos();
            panelICONO.Visible = true;
        }
        //toolStripMenuItem4
        private void Cargar_estado_de_iconos()
        {
            try
            {
                foreach (DataGridViewRow row in datalistado.Rows)
                {
                    try
                    {
                        string Icono = Convert.ToString(row.Cells["Nombre_de_icono"].Value);

                        if (Icono == "1")
                        {
                            picBox1.Visible = false;
                        }
                        else if(Icono == "2")
                        {
                            picBox2.Visible = false;
                        }
                        else if (Icono == "3")
                        {
                            picBox3.Visible = false;
                        }
                        else if (Icono == "4")
                        {
                            picBox4.Visible = false;
                        }
                        else if (Icono == "5")
                        {
                            picBox5.Visible = false;
                        }
                        else if (Icono == "6")
                        {
                            picBox6.Visible = false;
                        }
                        else if (Icono == "7")
                        {
                            picBox7.Visible = false;
                        }
                        else if (Icono == "8")
                        {
                            picBox8.Visible = false;
                        }

                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public bool validar_mail(string sMail)
        {
            return Regex.IsMatch(sMail, @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$");
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

            if (validar_mail(txtCorreo.Text) == false)
            {
                MessageBox.Show("Direccion de correo electrónico no validada, el correo debe tener el formato: nombre@dominio.com, " + "por favor seleccione un correo valido", "Validación de correo electrónico", MessageBoxButtons.OK, MessageBoxIcon.Question);
                txtCorreo.Focus();
                txtCorreo.SelectAll();
            }
            else
            {
                if(txtNombre.Text != "")
                {
                    if(txtrol.Text != "")
                    {
                        if (lblAnuncioIcono.Visible == false)
                        {
                                                 
                            try
                            {
                                SqlConnection con = new SqlConnection();
                                con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                                con.Open();
                                SqlCommand cmd = new SqlCommand();
                                cmd = new SqlCommand("insertar_usuario", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@nombres", txtNombre.Text);
                                cmd.Parameters.AddWithValue("@Login", txtLogin.Text);
                                cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                                cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                                cmd.Parameters.AddWithValue("@Rol", txtrol.Text);

                                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                ICONO.Image.Save(ms, ICONO.Image.RawFormat);

                                cmd.Parameters.AddWithValue("@Icono", ms.GetBuffer());
                                cmd.Parameters.AddWithValue("@Nombre_de_icono", lblnumeroIcono.Text);
                                cmd.Parameters.AddWithValue("@Estado", "ACTIVO");
                                cmd.ExecuteNonQuery();
                                con.Close();
                                mostrar();
                                panel4.Visible = false;
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Elija un icono", "Registro", MessageBoxButtons.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Elija un rol", "Registro", MessageBoxButtons.OK);
                    }

                }
                else
                {
                    MessageBox.Show("Introduzca su nombre", "Registro", MessageBoxButtons.OK);
                }
            }
        }

        private void mostrar()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                con.Open();

                da = new SqlDataAdapter("mostrar_usuario", con);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();

                datalistado.Columns[1].Visible = false;
                datalistado.Columns[5].Visible = false;
                datalistado.Columns[6].Visible = false;
                datalistado.Columns[7].Visible = false;
                datalistado.Columns[8].Visible = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            CONEXION.Tamaño_automatico_de_datatables.Multilinea(ref datalistado);
           
        }
        private void picBox1_Click(object sender, EventArgs e)
        {
            ICONO.Image = picBox1.Image;
            lblnumeroIcono.Text = "1";
            lblAnuncioIcono.Visible = false;
            panelICONO.Visible = false;
        }

        private void lblAnuncioIcono_Click(object sender, EventArgs e)
        {
            Cargar_estado_de_iconos();
            panelICONO.Visible = true;

        }

        private void picBox2_Click(object sender, EventArgs e)
        {
            ICONO.Image = picBox2.Image;
            lblnumeroIcono.Text = "2";
            lblAnuncioIcono.Visible = false;
            panelICONO.Visible = false;
        }

        private void picBox3_Click(object sender, EventArgs e)
        {
            ICONO.Image = picBox3.Image;
            lblnumeroIcono.Text = "3";
            lblAnuncioIcono.Visible = false;
            panelICONO.Visible = false;
        }

        private void picBox4_Click(object sender, EventArgs e)
        {
            ICONO.Image = picBox4.Image;
            lblnumeroIcono.Text = "4";
            lblAnuncioIcono.Visible = false;
            panelICONO.Visible = false;
        }

        private void picBox5_Click(object sender, EventArgs e)
        {
            ICONO.Image = picBox5.Image;
            lblnumeroIcono.Text = "5";
            lblAnuncioIcono.Visible = false;
            panelICONO.Visible = false;
        }

        private void picBox6_Click(object sender, EventArgs e)
        {
            ICONO.Image = picBox6.Image;
            lblnumeroIcono.Text = "6";
            lblAnuncioIcono.Visible = false;
            panelICONO.Visible = false;
        }

        private void picBox7_Click(object sender, EventArgs e)
        {
            ICONO.Image = picBox7.Image;
            lblnumeroIcono.Text = "7";
            lblAnuncioIcono.Visible = false;
            panelICONO.Visible = false;
        }

        private void picBox8_Click(object sender, EventArgs e)
        {
            ICONO.Image = picBox8.Image;
            lblnumeroIcono.Text = "8";
            lblAnuncioIcono.Visible = false;
            panelICONO.Visible = false;
        }

        private void usuariosok_Load(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panelICONO.Visible = false;
            mostrar();
        }

        private void picBoxAgregar_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            lblAnuncioIcono.Visible = true;
            txtNombre.Text = "";
            txtLogin.Text = "";
            txtPassword.Text = "";
            txtCorreo.Text = "";
            btnGuardar.Visible = true;
            btnGuardarCambios.Visible = false;
        }

        private void datalistado_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void datalistado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            lblid_usuario.Text = datalistado.SelectedCells[1].Value.ToString();
            txtNombre.Text = datalistado.SelectedCells[2].Value.ToString();
            txtLogin.Text = datalistado.SelectedCells[3].Value.ToString();

            txtPassword.Text = datalistado.SelectedCells[4].Value.ToString();

            ICONO.BackgroundImage = null;
            byte[] b = (Byte[])datalistado.SelectedCells[5].Value;
            MemoryStream ms = new MemoryStream(b);
            ICONO.Image = Image.FromStream(ms);

            lblAnuncioIcono.Visible = false;

            lblnumeroIcono.Text = datalistado.SelectedCells[6].Value.ToString();
            txtCorreo.Text = datalistado.SelectedCells[7].Value.ToString();
            txtrol.Text = datalistado.SelectedCells[8].Value.ToString();
            panel4.Visible = true;
            btnGuardar.Visible = false;
            btnGuardarCambios.Visible = true;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text != "")
            {
                try
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd = new SqlCommand("editar_usuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@idUsuario", lblid_usuario.Text);
                    cmd.Parameters.AddWithValue("@nombres", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Login", txtLogin.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                    cmd.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                    cmd.Parameters.AddWithValue("@Rol", txtrol.Text);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    ICONO.Image.Save(ms, ICONO.Image.RawFormat);
                    cmd.Parameters.AddWithValue("@Icono", ms.GetBuffer());
                    cmd.Parameters.AddWithValue("@Nombre_de_icono", lblnumeroIcono.Text);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    mostrar();
                    panel4.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void datalistado_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == this.datalistado.Columns["Eli"].Index)
            {
                DialogResult result;
                result = MessageBox.Show("Realmente desea eliminar este Usuario?", "Eliminando Registros", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    SqlCommand cmd;
                    try
                    {
                        foreach (DataGridViewRow row in datalistado.SelectedRows)
                        {
                            int onekey = Convert.ToInt32(row.Cells["idusuario"].Value);
                            string usuario = Convert.ToString(row.Cells["Login"].Value);

                            try
                            {
                                try
                                {
                                    SqlConnection con = new SqlConnection();
                                    con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                                    con.Open();
                                    cmd = new SqlCommand("eliminar_usuario", con);
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("@idUsuario", onekey);
                                    cmd.Parameters.AddWithValue("@Login", usuario);
                                    cmd.ExecuteNonQuery();

                                    con.Close();
                                }
                                catch(Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        mostrar();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                
            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            dlg.InitialDirectory = "";
            dlg.Filter = "Imagenes| *.jpg;*.png";
            dlg.FilterIndex = 2;
            dlg.Title = "Cargador de imagenes";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ICONO.BackgroundImage = null;
                ICONO.Image = new Bitmap(dlg.FileName);
                ICONO.SizeMode = PictureBoxSizeMode.Zoom;
                lblnumeroIcono.Text = Path.GetFileName(dlg.FileName);
                lblAnuncioIcono.Visible = false;
                panelICONO.Visible = false;
            }
        }
        private void buscar_usuario()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                con.Open();

                da = new SqlDataAdapter("buscar_usuario", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@letra", txtBuscar.Text);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();

                datalistado.Columns[1].Visible = false;
                datalistado.Columns[5].Visible = false;
                datalistado.Columns[6].Visible = false;
                datalistado.Columns[7].Visible = false;
                datalistado.Columns[8].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            CONEXION.Tamaño_automatico_de_datatables.Multilinea(ref datalistado);

        }
        public void Numeros(System.Windows.Forms.TextBox CajaTexto, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            buscar_usuario();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            Numeros(txtBuscar, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtCorreo_TextChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
