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
using System.Net;
using System.Net.Mail;
using System.Management;

namespace PUNTO_DE_VENTA_CSHARP.MODULOS
{
    public partial class LOGIN : Form
    {
        int contador;
        int contadorCajas;
        public LOGIN()
        {
            InitializeComponent();
        }

        public void DIBUJARusuarios()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd = new SqlCommand("select * from USUARIO2 where Estado='ACTIVO'", con);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Label b = new Label();
                Panel p1 = new Panel();
                PictureBox I1 = new PictureBox();

                b.Text = rdr["Login"].ToString();
                b.Name = rdr["idUsuario"].ToString();
                b.Size = new System.Drawing.Size(175, 25);
                b.Font = new System.Drawing.Font("Microsoft Sans Serif", 13);
                b.FlatStyle = FlatStyle.Flat;
                b.BackColor = Color.FromArgb(20,20,20);
                b.ForeColor = Color.White;
                b.Dock = DockStyle.Bottom;
                b.TextAlign = ContentAlignment.MiddleCenter;
                b.Cursor = Cursors.Hand;

                p1.Size = new System.Drawing.Size(155, 167);
                p1.BorderStyle = BorderStyle.None;
                p1.BackColor = Color.FromArgb(20, 20, 20);

                I1.Size = new System.Drawing.Size(172, 132);
                I1.Dock = DockStyle.Top;
                I1.BackgroundImage = null;
                byte[] bi = (Byte[])rdr["Icono"];
                MemoryStream ms = new MemoryStream(bi);
                I1.Image = Image.FromStream(ms);
                I1.SizeMode = PictureBoxSizeMode.Zoom;
                I1.Tag = rdr["Login"].ToString();
                I1.Cursor = Cursors.Hand;

                p1.Controls.Add(b);
                p1.Controls.Add(I1);
                b.BringToFront();
                flowLayoutPanel1.Controls.Add(p1);

                b.Click += new EventHandler(mieventoLabel);
                I1.Click += new EventHandler(mieventoImagen);
            }
            con.Close();
        }
        private void mieventoLabel(System.Object sender, EventArgs e)
        {
            txtlogin.Text = ((Label)sender).Text;
            panel2.Visible = true;
            panel1.Visible = false;
            MOSTRAR_PERMISOS();
        }

        private void mieventoImagen(System.Object sender, EventArgs e)
        {
            txtlogin.Text = ((PictureBox)sender).Tag.ToString();
            panel2.Visible = true;
            panel1.Visible = false;
            MOSTRAR_PERMISOS();
        }

        private void SISVENTAS_Load(object sender, EventArgs e)
        {
            DIBUJARusuarios();
            panel2.Visible = false;
            timer1.Start();
            tocultar.Visible = false;
        }

        private void Listar_APERTURAS_de_detalle_de_cierres_de_caja()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                con.Open();

                da = new SqlDataAdapter("MOSTRAR_MOVIMIENTOS_DE_CAJA_POR_SERIAL", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@serial", lblSerialPc.Text);
                da.Fill(dt);
                datalistado_caja.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            Iniciar_sesion_correcto();
        }
        private void contar_APERTURAS_de_detalle_de_cierres_de_caja()
        {
            int x;
            x = datalistado_detalle_cierre_de_caja.Rows.Count;
            contadorCajas = (x);
        }

        private void aperturar_detalle_de_cierre_caja()
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("insertar_DETALLE_cierre_de_caja", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fechaini", DateTime.Today);
                cmd.Parameters.AddWithValue("@fechafin", DateTime.Today);
                cmd.Parameters.AddWithValue("@fechacierre", DateTime.Today);
                cmd.Parameters.AddWithValue("@ingresos", "0.00");
                cmd.Parameters.AddWithValue("@egresos", "0.00");
                cmd.Parameters.AddWithValue("@saldo", "0.00");
                cmd.Parameters.AddWithValue("@idusuario", IDUSUARIO.Text);
                cmd.Parameters.AddWithValue("@totalcaluclado", "0.00");
                cmd.Parameters.AddWithValue("@totalreal", "0.00");
                cmd.Parameters.AddWithValue("@estado", "CAJA APERTURADA");
                cmd.Parameters.AddWithValue("@diferencia", "0.00");
                cmd.Parameters.AddWithValue("@id_caja", txtidcaja.Text);

                cmd.ExecuteNonQuery();
                con.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Iniciar_sesion_correcto()
        {
            cargarusuarios();
            contar();

            try
            {
                IDUSUARIO.Text = datalistado.SelectedCells[1].Value.ToString();
                txtnombre.Text = datalistado.SelectedCells[2].Value.ToString();
            }
            catch
            {

            }

            if(contador > 0)
            {
                Listar_APERTURAS_de_detalle_de_cierres_de_caja();
                contar_APERTURAS_de_detalle_de_cierres_de_caja();
                if (contadorCajas == 0 & lblRol.Text != "Solo Ventas (no esta autorizado para manejar dinero)")
                {
                    aperturar_detalle_de_cierre_caja();
                    lblApertura_de_caja.Text = "Nuevo*****";
                    timer2.Start();
                }
            }
        }

        private void contar()
        {
            int x;
            x = datalistado.Rows.Count;
            contador = (x);
        }
        private void cargarusuarios()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                con.Open();

                da = new SqlDataAdapter("validar_usuario", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@password", txtPassword.Text);
                da.SelectCommand.Parameters.AddWithValue("@login", txtlogin.Text);
                da.Fill(dt);
                datalistado.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            CONEXION.Tamaño_automatico_de_datatables.Multilinea(ref datalistado);

        }

        private void mostrar_correos()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                con.Open();

                da = new SqlDataAdapter("select Correo from USUARIO2 where Estado='ACTIVO'", con);
                
                da.Fill(dt);
                txt_correo.DisplayMember = "Correo";
                txt_correo.ValueMember = "Correo";
                txt_correo.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            CONEXION.Tamaño_automatico_de_datatables.Multilinea(ref datalistado);

        }
        private void btn_olvide_pass_Click(object sender, EventArgs e)
        {
            PanelRestaurarCuenta.Visible = true;
            mostrar_correos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PanelRestaurarCuenta.Visible = false;
        }

        private void mostrar_usuarios_por_correo()
        {
            try
            {
                string resultado;
                
                SqlConnection con = new SqlConnection();
                con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                SqlCommand da = new SqlCommand("buscar_USUARIO_por_correo", con);
                

                
                da.CommandType = CommandType.StoredProcedure;
                da.Parameters.AddWithValue("@correo", txt_correo.Text);

                con.Open();
                lblResultadoContraseña.Text = Convert.ToString(da.ExecuteScalar());
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }
        private void button1_Click(object sender, EventArgs e)
        {
            //boton enviar correo
            mostrar_usuarios_por_correo();  
            richTextBox1.Text = richTextBox1.Text.Replace("@pass", lblResultadoContraseña.Text);
            enviarCorreo("internetexplorerboy@gmail.com", "201092Reco", richTextBox1.Text, "Solicitud de Contraseña", txt_correo.Text, "");
        }

        internal void enviarCorreo(string emisor, string password, string mensaje, string asunto, string destinatario, string ruta)
        {
            try
            {
                MailMessage correos = new MailMessage();
                SmtpClient envios = new SmtpClient();
                correos.To.Clear();
                correos.Body = "";
                correos.Subject = "";
                correos.Body = mensaje;
                correos.Subject = asunto;
                correos.IsBodyHtml = true;
                correos.To.Add((destinatario));
                correos.From = new MailAddress(emisor);

                envios.Credentials = new NetworkCredential(emisor, password);
                envios.Host = "smtp.gmail.com";
                envios.Port = 587;
                envios.EnableSsl = true;

                envios.Send(correos);
                lblEstado_de_envio.Text = "ENVIADO";
                MessageBox.Show("Contraseña Enviada, revisa tu correo Electrónico", "Success");
                PanelRestaurarCuenta.Visible = false;
            }
            catch(Exception)
            {
                lblEstado_de_envio.Text = "Correo no registrado";
            }
        }

        private void MOSTRAR_CAJA_POR_SERIAL()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;
                con.Open();

                da = new SqlDataAdapter("mostrar_cajas_por_Serial_de_DiscoDuro", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Serial", lblSerialPc.Text);
                da.Fill(dt);
                datalistado_caja.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            try
            {
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("select * from Win32_BaseBoard");
                foreach(ManagementObject getserial in MOS.Get())
                {
                    lblSerialPc.Text = getserial.Properties["SerialNumber"].Value.ToString();
                    MOSTRAR_CAJA_POR_SERIAL();
                    try
                    {
                        txtidcaja.Text = datalistado_caja.SelectedCells[1].Value.ToString();
                        lblcaja.Text = datalistado_caja.SelectedCells[2].Value.ToString();
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

        private void btn0_Click(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text + "0";
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text + "1";
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text + "2";
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text + "3";
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text + "4";
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text + "5";
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text + "6";
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text + "7";
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text + "8";
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtPassword.Text = txtPassword.Text + "9";
        }

        private void btn_borrar_Click(object sender, EventArgs e)
        {
            txtPassword.Clear();
        }

        public static string Mid(string param, int startIndex, int length)
        {
            string result = param.Substring(startIndex);
            return result;
        }

        private void btn_borrarderecha_Click(object sender, EventArgs e)
        {
            try
            {
                int largo;
                if(txtPassword.Text != "")
                {
                    largo = txtPassword.Text.Length;
                    txtPassword.Text =Mid(txtPassword.Text ,1 , largo - 1);
                }
            }
            catch
            {

            }
        }

        private void tver_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '\0';
            tocultar.Visible = true;
            tver.Visible = false;
        }

        private void tocultar_Click(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
            tocultar.Visible = false;
            tver.Visible = true;
        }

        private void btn_insertar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Usuario o contraseña Inconrrectos", "Datos Incorrectos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void MOSTRAR_PERMISOS()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = CONEXION.CONEXIONMAESTRA.conexion;

            SqlCommand com = new SqlCommand("mostrar_permisos_por_usuario_ROL_UNICO", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@LOGIN", txtlogin.Text);
            string importe;

            try
            {
                con.Open();
                importe = Convert.ToString(com.ExecuteScalar());
                con.Close();
                lblRol.Text = importe;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if(progressBar1.Value < 100)
            {
                BackColor = Color.FromArgb(26, 26, 26);
                progressBar1.Value = progressBar1.Value + 10;
                pictureBox3.Visible = true;
            }
            else
            {
                progressBar1.Value = 0;
                timer2.Stop();
                if(lblApertura_de_caja.Text == "Nuevo*****" & lblRol.Text != "Solo Ventas (no esta autorizado para manejar dinero)")
                {
                    this.Hide();
                    Form APERTURA_DE_CAJA = new Form();
                    APERTURA_DE_CAJA.ShowDialog();
                    this.Hide();
                }
                else
                {
                    this.Hide();
                    Form VENTAS_MENU_PRINCIPALOk = new Form();
                    VENTAS_MENU_PRINCIPALOk.ShowDialog();
                    this.Hide();
                }
            }
        }
    }
}
