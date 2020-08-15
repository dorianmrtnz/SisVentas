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
        }

        private void mieventoImagen(System.Object sender, EventArgs e)
        {
            txtlogin.Text = ((PictureBox)sender).Tag.ToString();
            panel2.Visible = true;
            panel1.Visible = false;
        }

        private void SISVENTAS_Load(object sender, EventArgs e)
        {
            DIBUJARusuarios();
            panel2.Visible = false;
            timer1.Start();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            Iniciar_sesion_correcto();
        }

        private void Iniciar_sesion_correcto()
        {
            cargarusuarios();
            contar();

            if(contador > 0)
            {
                CAJA.APERTURA_DE_CAJA formulario_apertura_de_caja = new CAJA.APERTURA_DE_CAJA();
                this.Hide();
                formulario_apertura_de_caja.ShowDialog();
                this.Close();
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
                datalistado.DataSource = dt;
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
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
