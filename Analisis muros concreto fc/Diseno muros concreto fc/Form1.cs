using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Prueba2;

namespace Diseno_muros_concreto_fc
{
    public partial class Form1 : Form
    {
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]

        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);




        private bool Drag;
        private int MouseX;
        private int MouseY;
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        private bool m_aeroEnabled;
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;
        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
         );

        public Form1()
        {
            m_aeroEnabled = false;
            InitializeComponent();

        }
        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)     // drag the form
                m.Result = (IntPtr)HTCAPTION;

        }

        private DataGridView D_Wall_Forces = new DataGridView();
        private DataGridView D_Geometria = new DataGridView();
        private DataGridView D_Shear = new DataGridView();
        private DataGridView D_Flexion = new DataGridView();
        private DataGridView D_Resumen = new DataGridView();



        private void SetupDataGridView(string Nombre_Data, DataGridView Formulario, DataTable Origen_datos)
        {
            //Formulario.Dispose();
            foreach (Control Control in panel1.Controls)
            {
                panel1.Controls.Remove(Control);
            }

            panel1.Controls.Add(Formulario);

            Formulario.VirtualMode = true;
            Formulario.DataSource = Origen_datos;

            Formulario.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            Formulario.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            Formulario.ColumnHeadersDefaultCellStyle.Font = new Font("Verdana", 8f, FontStyle.Bold);
            Formulario.RowsDefaultCellStyle.Font = new Font("Verdana", 8f, FontStyle.Regular);
            Formulario.BackgroundColor = Color.FromName("Control");
            Formulario.BorderStyle = BorderStyle.None;

            Formulario.Dock = DockStyle.Fill;
            Formulario.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            Formulario.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            Formulario.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            Formulario.RowHeadersVisible = true;
            Formulario.ColumnHeadersHeight = 40;
            Formulario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            Formulario.ReadOnly = true;


        }



        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lectura_E2k.Cargar_E2k();
            Generador_de_piers.Lector_Geometria();
            Generador_de_piers.Lector_Fuerza();
            Generador_de_piers.Lectura_Diseno();
            Generador_de_piers.Recopilar_info();

            List<Muro> Lista_ordenada = Listas_Programa.Lista_Muros.OrderBy(x1 => x1.Pier).ToList();
            List<string> Muros_distintos = Lista_ordenada.Select(x => x.Pier).Distinct().ToList();
            Lista_ordenada.Clear();

            Fase1 Formulario1 = new Fase1();
            Formulario1.Cargar_Lista();

            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario1);

            B_Shear.Enabled = true;
            B_Flexural.Enabled = true;

        }


        private void Form1_Load(object sender, EventArgs e)
        {



            this.MaximizeBox = false;
            Bases_de_datos.Ds_Shear = new DataSet();
            Radio_Dmo.Select();
            Panel Panel_i = new Panel();

            Fase1 Formulario1 = new Fase1();
            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario1);

            //Diseño_de_muros_concreto_V2.f_alzado Formulario3 = new Diseño_de_muros_concreto_V2.f_alzado();

            //foreach(Control Control_i in Formulario3.Controls)
            //{
            //    if (Control_i.Name == "Panel2")
            //    {
            //        Panel_i =(Panel) Control_i;
            //        Cargar_Formulario.Open_From_Panel2(this.panel3, Panel_i);
            //        break;
            //    }
            //}

            // Cargar_Formulario.Open_From_Panel(this.panel3, Formulario3);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            string Mensaje;

            try
            {
                Listas_Programa.Muros_insuficientes = new List<Muro>();

                for (int i = 0; i < Listas_Programa.Lista_Muros.Count; i++)
                {
                    Listas_Programa.Lista_Muros[i].Diseno_Cortante();
                    if (Listas_Programa.Lista_Muros[i].Error_Cortante.Exists(x1 => x1 == "V2 mayor que Phi Vn max") == true ^ Listas_Programa.Lista_Muros[i].Error_Cortante.Exists(x1 => x1 == "Phi Vs mayor a Phi Vs max") == true) Listas_Programa.Muros_insuficientes.Add(Listas_Programa.Lista_Muros[i]);

                }

                if (Listas_Programa.Muros_insuficientes.Count > 0)
                {
                    Mensaje = "Los muros :";

                    for (int i = 0; i < Listas_Programa.Muros_insuficientes.Count; i++)
                    {
                        if (i < Listas_Programa.Muros_insuficientes.Count - 1)
                        {
                            Mensaje = Mensaje + Listas_Programa.Muros_insuficientes[i].Pier + ",";
                        }
                        else
                        {
                            Mensaje = Mensaje + Listas_Programa.Muros_insuficientes[i].Pier;
                        }
                    }

                    Mensaje = Mensaje + " Presentan deficiencias en el diseño a cortante";
                    result = MessageBox.Show(Mensaje, "Efe prima Ce", buttons);
                }
            }
            catch
            {

            }
        }

        private void B_Report_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            string Mensaje;

            Procesar_info.Compilar_Datos();

            if (Listas_Programa.Ruta_archivo is null == true)
            {
                Guardar_archivo.Crear_Archivo_Texto();
            }

            Guardar_archivo.Generar_texto();
            Mensaje = "Listo";
            result = MessageBox.Show(Mensaje, "Efe prima Ce", buttons);

        }

        private void Radio_Dmo_CheckedChanged(object sender, EventArgs e)
        {
            Listas_Programa.Capacidad = "DMO";
        }

        private void Radio_Des_CheckedChanged(object sender, EventArgs e)
        {
            Listas_Programa.Capacidad = "DES";
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Listas_Programa.Ruta_archivo is null == true)
            {
                Guardar_archivo.Crear_Archivo_Texto();
            }

            Guardar_archivo.Generar_texto();
            Diseño_de_muros_concreto_V2.Guardar_Archivo Guardado_Archivo = new Diseño_de_muros_concreto_V2.Guardar_Archivo(Listas_Programa.Ruta_archivo);
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cargar_archivo.Cargar_Lista_Texto();

            B_Flexural.Enabled = false;
            B_Report.Enabled = false;
            B_Shear.Enabled = false;

            Fase1 fase1 = new Fase1();
            fase1.Cargar_Lista();
            Cargar_Formulario.Open_From_Panel(this.panel1, fase1);

            if (Listas_Programa.Capacidad == "DMO")
            {
                Radio_Dmo.Checked = true;
            }
            else
            {
                Radio_Des.Checked = true;
            }

        }

        private void InfoGeneralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 1553 + 200;
            this.Height = 760 + 35;
            if (Listas_Programa.Muros_Consolidados_Listos != null)
            {
                Diseño_de_muros_concreto_V2.Objetos_Compartidos Prueba = new Diseño_de_muros_concreto_V2.Objetos_Compartidos(Listas_Programa.Ruta_archivo, Listas_Programa.Capacidad);
            }

            Diseño_de_muros_concreto_V2.Form1 Formulario1 = new Diseño_de_muros_concreto_V2.Form1();
            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario1);
            murosSimilaresToolStripMenuItem.Enabled = true;
        }

        private void analisisEstructuralToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.Width = 995;
            this.Height = 675;
            Fase1 Formulario1 = new Fase1();
            Formulario1.Cargar_Lista();
            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario1);
            murosSimilaresToolStripMenuItem.Enabled = false;

        }

        private void B_Flexural_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            string Mensaje;

            try
            {
                Listas_Programa.Muros_insuficientes = new List<Muro>();

                for (int i = 0; i < Listas_Programa.Lista_Muros.Count; i++)
                {
                    Listas_Programa.Lista_Muros[i].Flexural_Analisis();

                }


                if (Listas_Programa.Muros_insuficientes.Count > 0)
                {
                    Mensaje = "Los muros :";

                    for (int i = 0; i < Listas_Programa.Muros_insuficientes.Count; i++)
                    {
                        if (i < Listas_Programa.Muros_insuficientes.Count - 1)
                        {
                            Mensaje = Mensaje + Listas_Programa.Muros_insuficientes[i].Pier + ",";
                        }
                        else
                        {
                            Mensaje = Mensaje + Listas_Programa.Muros_insuficientes[i].Pier;
                        }
                    }

                    Mensaje = Mensaje + " Presentan deficiencias en el diseño a cortante";
                    result = MessageBox.Show(Mensaje, "Efe prima Ce", buttons);
                }
                B_Report.Enabled = true;
            }
            catch
            {

            }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Guardar_archivo.Crear_Archivo_Texto();
            Guardar_archivo.Generar_texto();
            Diseño_de_muros_concreto_V2.Guardar_Archivo Guardado_Archivo = new Diseño_de_muros_concreto_V2.Guardar_Archivo(Listas_Programa.Ruta_archivo);
        }

        private void AlzadoRefuerzoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Listas_Programa.Muros_Consolidados_Listos != null)
            {
                Diseño_de_muros_concreto_V2.Objetos_Compartidos Prueba = new Diseño_de_muros_concreto_V2.Objetos_Compartidos(Listas_Programa.Ruta_archivo, Listas_Programa.Capacidad);
            }
            panel1.Visible = true;
            Diseño_de_muros_concreto_V2.f_alzado Formulario3 = new Diseño_de_muros_concreto_V2.f_alzado();
            this.Width = 1553 + 200;
            this.Height = 760 + 35;




            // this.Location = new Point(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y);

            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario3);

            murosSimilaresToolStripMenuItem.Enabled = true;
        }


        private void Button2_Click(object sender, EventArgs e)
        {
            Lectura_E2k.Cargar_E2k();
            Generador_de_piers.Lector_Geometria();
            Generador_de_piers.Lector_Fuerza();
            Generador_de_piers.Lectura_Diseno();
            Generador_de_piers.Recopilar_info();

            List<Muro> Lista_ordenada = Listas_Programa.Lista_Muros.OrderBy(x1 => x1.Pier).ToList();
            List<string> Muros_distintos = Lista_ordenada.Select(x => x.Pier).Distinct().ToList();
            Lista_ordenada.Clear();

            Fase1 Formulario1 = new Fase1();
            Formulario1.Cargar_Lista();
            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario1);

            B_Shear.Enabled = true;
            B_Flexural.Enabled = true;
        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            Cargar_archivo.Cargar_Lista_Texto();

            B_Flexural.Enabled = false;
            B_Report.Enabled = false;
            B_Shear.Enabled = false;

            Fase1 fase1 = new Fase1();
            fase1.Cargar_Lista();
            Cargar_Formulario.Open_From_Panel(this.panel1, fase1);

            if (Listas_Programa.Capacidad == "DMO")
            {
                Radio_Dmo.Checked = true;
            }
            else
            {
                Radio_Des.Checked = true;
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            if (Listas_Programa.Ruta_archivo is null == true)
            {
                Guardar_archivo.Crear_Archivo_Texto();
            }

            Guardar_archivo.Generar_texto();
            Diseño_de_muros_concreto_V2.Guardar_Archivo Guardado_Archivo = new Diseño_de_muros_concreto_V2.Guardar_Archivo(Listas_Programa.Ruta_archivo);

        }

        private void Button7_Click(object sender, EventArgs e)
        {
            Guardar_archivo.Crear_Archivo_Texto();
            Guardar_archivo.Generar_texto();
            Diseño_de_muros_concreto_V2.Guardar_Archivo Guardado_Archivo = new Diseño_de_muros_concreto_V2.Guardar_Archivo(Listas_Programa.Ruta_archivo);

        }

        private void MurosSimilaresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Diseño_de_muros_concreto_V2.Similar VentanaSimilares = new Diseño_de_muros_concreto_V2.Similar();
            VentanaSimilares.Show();

        }


        private void Form1_Activated(object sender, EventArgs e)
        {

        }

        private void MenuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button10_MouseMove(object sender, MouseEventArgs e)
        {
            button10.FlatAppearance.BorderColor = Color.FromArgb(232, 17, 35);
        }

        private void Button10_MouseLeave(object sender, EventArgs e)
        {
            button10.FlatAppearance.BorderColor = Color.FromArgb(114, 112, 113);
        }



        private void Button11_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Panel5_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void DibujoSeccionToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Prueba2.Form1 Formulario = new Prueba2.Form1();
            Formulario.Show();


        }

        private void Panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void VariablesDeDibujoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Diseño_de_muros_concreto_V2.f_variables Formulario = new Diseño_de_muros_concreto_V2.f_variables();
            Formulario.Show();


        }



      
   










































        }
}




 