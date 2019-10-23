using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Diseno_muros_concreto_fc
{
    public partial class Form1 : Form
    {
        private Listas_Serializadas_i Listas = new Listas_Serializadas_i();

        private Diseño_de_muros_concreto_V2.Lista_Cantidades ListaSer = new Diseño_de_muros_concreto_V2.Lista_Cantidades();

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

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

        private void SalirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void NuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NuevoProyecto();
        }

        private void NuevoProyecto()
        {
            Label_Inicial.Visible = true;
            Label_Inicial.Text = "Cargando modelo del edificio";
            Lectura_E2k.Cargar_E2k();
            Label_Inicial.Text = "Listo";
            Label_Inicial.Update();
            Label_Inicial.Visible = false;

            Generador_de_piers.Lector_Geometria();
            Generador_de_piers.Lector_Fuerza();
            Generador_de_piers.Lectura_Diseno();
            Generador_de_piers.Recopilar_info();

            List<Muro> Lista_ordenada = Listas_Programa.Lista_Muros.OrderBy(x1 => x1.Pier).ToList();
            List<string> Muros_distintos = Lista_ordenada.Select(x => x.Pier).Distinct().ToList();
            Lista_ordenada.Clear();

            Fase1 Formulario1 = new Fase1();
            Formulario1.Cargar_Lista();

            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario1, Listas);

            B_Shear.Enabled = true;
            B_Flexural.Enabled = true;
        }

        private void RegistrarDMC()
        {
            RegistryKey clave1 = Registry.CurrentUser.OpenSubKey("Software", true);
            clave1.CreateSubKey("Classes");
            clave1 = clave1.OpenSubKey("Classes", true);

            clave1.CreateSubKey(".dmc");
            clave1 = clave1.OpenSubKey(".dmc", true);
            clave1.SetValue("", "archivo.dmc");

            clave1.Close();
            ////////////////////////////////////////
            RegistryKey clave2 = Registry.CurrentUser.OpenSubKey("Software", true);
            clave2.CreateSubKey("Classes");
            clave2 = clave2.OpenSubKey("Classes", true);

            clave2.CreateSubKey("archivo.dmc");
            clave2 = clave2.OpenSubKey("archivo.dmc", true);
            clave2.SetValue("", "File DMC");

            clave2.CreateSubKey("DefaultIcon");
            clave2 = clave2.OpenSubKey("DefaultIcon", true);
            clave2.SetValue("", Application.StartupPath + "\\Icono\\icono.ico");

            clave2.Close();
            ////////////////////////////////////////
            RegistryKey clave3 = Registry.CurrentUser.OpenSubKey("Software", true);
            clave3.CreateSubKey("Classes");
            clave3 = clave3.OpenSubKey("Classes", true);

            clave3.CreateSubKey("archivo.dmc");
            clave3 = clave3.OpenSubKey("archivo.dmc", true);
            clave3.SetValue("", "File DMC");

            clave3.CreateSubKey("shell");
            clave3 = clave3.OpenSubKey("shell", true);

            clave3.CreateSubKey("open");
            clave3 = clave3.OpenSubKey("open", true);

            clave3.CreateSubKey("command");
            clave3 = clave3.OpenSubKey("command", true);
            clave3.SetValue("", "\"" + Application.StartupPath + "\\Diseno muros concreto fc.exe\" \"%1\"");

            clave3.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegistrarDMC();

            this.MaximizeBox = false;
            Bases_de_datos.Ds_Shear = new DataSet();
            List<string> Lista_ToolTip = new List<string>();
            Lista_ToolTip.Add("Nuevo Proyecto (Ctrl + N)");
            Lista_ToolTip.Add("Abrir (Ctrl + O)");
            Lista_ToolTip.Add("Guardar (Ctrl + S)");
            Lista_ToolTip.Add("Guardar como (Ctrl + Mayús + S");
            Lista_ToolTip.Add("Exportar Memorias (Ctrl + E)");
            Lista_ToolTip.Add("Cerrar");
            Lista_ToolTip.Add("Minimizar");

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(button2, Lista_ToolTip[0]);
            toolTip.SetToolTip(button3, Lista_ToolTip[1]);
            toolTip.SetToolTip(button8, Lista_ToolTip[2]);
            toolTip.SetToolTip(button7, Lista_ToolTip[3]);
            toolTip.SetToolTip(button9, Lista_ToolTip[4]);
            toolTip.SetToolTip(button10, Lista_ToolTip[5]);
            toolTip.SetToolTip(button11, Lista_ToolTip[6]);

            this.MaximizeBox = false;
            Bases_de_datos.Ds_Shear = new DataSet();

            Panel Panel_i = new Panel();
            Fase1 Formulario1 = new Fase1();
            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario1, Listas);

            if (Environment.CommandLine.Contains("\" \""))
            {
                string ArchivoRuta = Environment.CommandLine.Split(new char[] { '"' })[3];
                Listas_Programa.Ruta_archivo = ArchivoRuta;
                AbrirProyecto(ref Listas, false);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            string Mensaje;

            if (Radio_Dmo.Checked == true | Radio_Des.Checked == true)
            {
                try
                {
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
                        Listas_Programa.Error_Cortante = "¡CORTANTE!";
                        Mensaje = Mensaje + " Presentan deficiencias en el diseño a cortante";
                        result = MessageBox.Show(Mensaje, "Efe prima Ce", buttons);
                    }
                }
                catch
                {
                }
            }
            else
            {
                Mensaje = "Seleccione la capacidad de dicipación de la estructura";
                result = MessageBox.Show(Mensaje, "efe Prima Ce", buttons);
            }
        }

        private void B_Report_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            string Mensaje;

            Procesar_info.Compilar_Datos();
            Listas_Programa.Muros_errores = Procesar_info.Muros_errores;

            Listas = new Listas_Serializadas_i
            {
                Capacidad_proyecto = Listas_Programa.Capacidad,
                Lista_Alzados = new List<Alzado_muro>(),
                lista_refuerzo = new List<Refuerzo_muros>(),
                Lista_Muros = Listas_Programa.Muros_Consolidados_Listos,
                Muros_generales = Listas_Programa.Lista_Muros,
            };

            if (Listas_Programa.Ruta_archivo is null == true)
            {
                Listas_Programa.Ruta_archivo = "";
            }

            Serializador.Serializar(ref Listas_Programa.Ruta_archivo, Listas);
            Mensaje = "Listo";
            result = MessageBox.Show(Mensaje, "Efe prima Ce", buttons);

            this.Width = 995;
            this.Height = 675;
            Fase1 Formulario1 = new Fase1();
            Formulario1.Cargar_Lista();

            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario1, Listas);
            murosSimilaresToolStripMenuItem.Enabled = false;
            direcciónDeCambioDeEspesorToolStripMenuItem.Enabled = false;
        }

        private void Radio_Dmo_CheckedChanged(object sender, EventArgs e)
        {
            Listas_Programa.Capacidad = "DMO";
        }

        private void Radio_Des_CheckedChanged(object sender, EventArgs e)
        {
            Listas_Programa.Capacidad = "DES";
        }

        private void SaveFile()
        {
            if (Listas_Programa.Ruta_archivo is null == true | Listas_Programa.Ruta_archivo == "")
            {
                //Serializador.Serializar(ref Listas_Programa.Ruta_archivo, Listas);
                Diseño_de_muros_concreto_V2.Serializador Serializar = new Diseño_de_muros_concreto_V2.Serializador(Listas_Programa.Ruta_archivo, true, Listas);
                if (Listas_Programa.Ruta_archivo != null | Listas_Programa.Ruta_archivo == "")
                {
                    DeterminarRutaCarpeta_NombreProyecto(Listas_Programa.Ruta_archivo);
                    Diseño_de_muros_concreto_V2.Serializador2 serializador2 = new Diseño_de_muros_concreto_V2.Serializador2(Listas_Programa.Ruta_Carpeta, Listas_Programa.Name_Proyecto, true);
                }
                //Guardar_archivo.Crear_Archivo_Texto();
                //Guardar_archivo.Generar_texto();
                //Diseño_de_muros_concreto_V2.Guardar_Archivo Guardado_Archivo = new Diseño_de_muros_concreto_V2.Guardar_Archivo(Listas_Programa.Ruta_archivo, false);
            }
            else
            {
                //Serializador.Serializar(ref Listas_Programa.Ruta_archivo, Listas);
                Diseño_de_muros_concreto_V2.Serializador Serializar = new Diseño_de_muros_concreto_V2.Serializador(Listas_Programa.Ruta_archivo, true, Listas);
                DeterminarRutaCarpeta_NombreProyecto(Listas_Programa.Ruta_archivo);
                Diseño_de_muros_concreto_V2.Serializador2 serializador2 = new Diseño_de_muros_concreto_V2.Serializador2(Listas_Programa.Ruta_Carpeta, Listas_Programa.Name_Proyecto, true);
                //Guardar_archivo.Generar_texto();
                //Diseño_de_muros_concreto_V2.Guardar_Archivo Guardado_Archivo = new Diseño_de_muros_concreto_V2.Guardar_Archivo(Listas_Programa.Ruta_archivo, false);
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirProyecto(ref Listas);
        }

        private void InfoGeneralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 1553 + 200;
            this.Height = 760 + 35;

            if (Listas_Programa.Muros_Consolidados_Listos != null)
            {
                Diseño_de_muros_concreto_V2.Objetos_Compartidos Prueba = new Diseño_de_muros_concreto_V2.Objetos_Compartidos(Listas_Programa.Ruta_archivo, Listas_Programa.Capacidad, Listas_Programa.Ruta_Carpeta);
            }

            Diseño_de_muros_concreto_V2.Form22 Formulario1 = new Diseño_de_muros_concreto_V2.Form22();
            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario1, Listas);
            murosSimilaresToolStripMenuItem.Enabled = true;
            direcciónDeCambioDeEspesorToolStripMenuItem.Enabled = true;
            direcciónDeCambioDeEspesorToolStripMenuItem.Enabled = true;
            listasDeMurosAGraficarToolStripMenuItem.Enabled = true;
        }

        private void analisisEstructuralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Width = 995;
            this.Height = 675;
            Fase1 Formulario1 = new Fase1();

            //Serializador.Deserializar(ref Listas_Programa.Ruta_archivo, ref Listas);
            Formulario1.Cargar_Lista();

            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario1, Listas);
            murosSimilaresToolStripMenuItem.Enabled = false;
            direcciónDeCambioDeEspesorToolStripMenuItem.Enabled = false;
        }

        private void B_Flexural_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;
            string Mensaje;
            if (Radio_Dmo.Checked == true | Radio_Des.Checked == true)
            {
                try
                {
                    for (int i = 0; i < Listas_Programa.Lista_Muros.Count; i++)
                    {
                        Listas_Programa.Lista_Muros[i].Flexural_Analisis();
                    }

                    if (Listas_Programa.Muros_insuficientes.Count > 0)
                    {
                        var Flexion = Listas_Programa.Muros_insuficientes.FindAll(x => x.Error_Flexion.Exists(x1 => x1 != "Ok") == true);

                        if (Flexion.Count > 0)
                        {
                            Mensaje = "Los muros :";
                            for (int i = 0; i < Flexion.Count; i++)
                            {
                                if (i < Flexion.Count - 1)
                                {
                                    Mensaje = Mensaje + Flexion[i].Pier + ",";
                                }
                                else
                                {
                                    Mensaje = Mensaje + Flexion[i].Pier;
                                }
                            }

                            Listas_Programa.Error_Flexion = "¡FLEXION!";
                            Mensaje = Mensaje + " Presentan deficiencias en el diseño a flexion";
                            result = MessageBox.Show(Mensaje, "Efe prima Ce", buttons);
                        }
                    }
                    B_Report.Enabled = true;
                }
                catch
                {
                }
            }
            else
            {
                Mensaje = "Seleccione la capacidad de dicipación de la estructura";
                result = MessageBox.Show(Mensaje, "efe Prima Ce", buttons);
            }
        }

        private void guardarComoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuardarComo();
        }

        private void GuardarComo()
        {
            Listas_Programa.Ruta_archivo = "";
            Serializador.Serializar(ref Listas_Programa.Ruta_archivo, Listas);
            Diseño_de_muros_concreto_V2.Serializador Serializar = new Diseño_de_muros_concreto_V2.Serializador(Listas_Programa.Ruta_archivo, true, Listas);
            if (Listas_Programa.Ruta_archivo != null | Listas_Programa.Ruta_archivo == "")
            {
                DeterminarRutaCarpeta_NombreProyecto(Listas_Programa.Ruta_archivo);
                Diseño_de_muros_concreto_V2.Serializador2 serializador2 = new Diseño_de_muros_concreto_V2.Serializador2(Listas_Programa.Ruta_Carpeta, Listas_Programa.Name_Proyecto, true);
            }
            //Guardar_archivo.Crear_Archivo_Texto();
            //Guardar_archivo.Generar_texto();
            //Diseño_de_muros_concreto_V2.Guardar_Archivo Guardado_Archivo = new Diseño_de_muros_concreto_V2.Guardar_Archivo(Listas_Programa.Ruta_archivo, false);
        }

        private void AlzadoRefuerzoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Listas_Programa.Muros_Consolidados_Listos != null)
            {
                Diseño_de_muros_concreto_V2.Objetos_Compartidos Prueba = new Diseño_de_muros_concreto_V2.Objetos_Compartidos(Listas_Programa.Ruta_archivo, Listas_Programa.Capacidad, Listas_Programa.Ruta_Carpeta);
            }
            panel1.Visible = true;
            Diseño_de_muros_concreto_V2.f_alzado Formulario3 = new Diseño_de_muros_concreto_V2.f_alzado();
            this.Width = 1553 + 200;
            this.Height = 760 + 35;

            // this.Location = new Point(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y);

            Cargar_Formulario.Open_From_Panel(this.panel1, Formulario3, Listas);

            murosSimilaresToolStripMenuItem.Enabled = true;
            direcciónDeCambioDeEspesorToolStripMenuItem.Enabled = true;
            listasDeMurosAGraficarToolStripMenuItem.Enabled = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            NuevoProyecto();
        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            AbrirProyecto(ref Listas);
        }

        private void AbrirProyecto(ref Listas_Serializadas_i Lista_i, bool Diferente = true)
        {
            Listas_Programa.Muros_Consolidados_Listos = new List<Muros_Consolidados_1>();
            Listas_Programa.Lista_Muros = new List<Muro>();
            Listas_Programa.Lista_shells = new List<Shells_Prop>();

            Serializador.Deserializar(ref Listas_Programa.Ruta_archivo, ref Lista_i, Diferente);
            if (Listas_Programa.Ruta_archivo != null | Listas_Programa.Ruta_archivo != "")
            {
                DeterminarRutaCarpeta_NombreProyecto(Listas_Programa.Ruta_archivo);
                Diseño_de_muros_concreto_V2.Serializador2 serializador2 = new Diseño_de_muros_concreto_V2.Serializador2(Listas_Programa.Ruta_Carpeta, Listas_Programa.Name_Proyecto, false);
            }
            Listas_Programa.Capacidad = Lista_i.Capacidad_proyecto;

            foreach (Muro muroi in Listas_Programa.Lista_Muros)
            {
                muroi.Calc_pc();
            }

            foreach (Muros_Consolidados_1 muroi in Listas_Programa.Muros_Consolidados_Listos)
            {
                Procesar_info.Errores_muro(muroi);
            }
            Listas_Programa.Muros_errores = Procesar_info.Muros_errores;

            B_Flexural.Enabled = false;
            B_Report.Enabled = false;
            B_Shear.Enabled = false;

            Fase1 fase1 = new Fase1();
            fase1.Cargar_Lista();
            Cargar_Formulario.Open_From_Panel(this.panel1, fase1, Lista_i);

            if (Listas_Programa.Capacidad == "DMO")
            {
                Radio_Dmo.Checked = true;
            }
            else
            {
                Radio_Des.Checked = true;
            }
            Generar.Enabled = true;
        }

        private void DeterminarRutaCarpeta_NombreProyecto(string Ruta)
        {
            try
            {
                int FinPunto = 0;
                int FinSla = 0;
                int FinSla2 = 0;
                string Ruta_Carpeta = "";
                string Nombre_Proyecto = "";

                for (int n = 0; n < (Ruta.Length); n++) { if (Ruta.Substring(n, 1) == ".") { FinPunto = n; } }
                for (int n = FinPunto; n >= 0; n--) { if (Ruta.Substring(n, 1) == @"\") { FinSla = n; break; } }

                for (int n = 0; n < FinSla; n++) { Ruta_Carpeta = Ruta_Carpeta + (Ruta.Substring(n, 1)); }

                for (int n = 0; n < (Ruta.Length); n++) { if (Ruta.Substring(n, 1) == @"\") { FinSla2 = n + 1; } }
                for (int n = FinSla2; n < FinPunto; n++) { Nombre_Proyecto = Nombre_Proyecto + (Ruta.Substring(n, 1)); }

                Listas_Programa.Ruta_Carpeta = Ruta_Carpeta;
                Listas_Programa.Name_Proyecto = Nombre_Proyecto;
            }
            catch
            {
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            GuardarComo();
        }

        private void MurosSimilaresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MurosSimilaresVentana();
        }

        private void MurosSimilaresVentana()
        {
            Diseño_de_muros_concreto_V2.Similar VentanaSimilares = new Diseño_de_muros_concreto_V2.Similar();
            VentanaSimilares.ShowDialog();
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
            Diseño_de_muros_concreto_V2.Seccion Formulario = new Diseño_de_muros_concreto_V2.Seccion();
            if (Listas_Programa.Muros_Consolidados_Listos != null)
            {
                Diseño_de_muros_concreto_V2.Objetos_Compartidos Prueba = new Diseño_de_muros_concreto_V2.Objetos_Compartidos(Listas_Programa.Ruta_archivo, Listas_Programa.Capacidad, Listas_Programa.Ruta_Carpeta);
            }

            Formulario.ShowDialog();
        }

        private void VariablesDeDibujoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Diseño_de_muros_concreto_V2.f_variables Formulario = new Diseño_de_muros_concreto_V2.f_variables();
            Formulario.Show();
        }

        private void DirecciónDeCambioDeEspesorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CambioEspesorVentana();
        }

        private void CambioEspesorVentana()
        {
            Diseño_de_muros_concreto_V2.Form_DireccionCambiodeEspesor Formulario = new Diseño_de_muros_concreto_V2.Form_DireccionCambiodeEspesor();
            Formulario.ShowDialog();
        }

        private void AcercaDeDiseñoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DerechosAutor FormDerechos = new DerechosAutor();
            FormDerechos.Show();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            ExportMemorias();
        }

        private void listasDeMurosAGraficarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Diseño_de_muros_concreto_V2.Muros_Consolidados> L_Muro_aux = new List<Diseño_de_muros_concreto_V2.Muros_Consolidados>();
            Diseño_de_muros_concreto_V2.Muros_Alzados Muros_graficar = new Diseño_de_muros_concreto_V2.Muros_Alzados();
            Muros_graficar.ShowDialog();
        }

        private void ExportMemorias()
        {
            if (Listas_Programa.Muros_Consolidados_Listos != null)
            {
                Diseño_de_muros_concreto_V2.Objetos_Compartidos Prueba = new Diseño_de_muros_concreto_V2.Objetos_Compartidos(Listas_Programa.Ruta_archivo, Listas_Programa.Capacidad, Listas_Programa.Ruta_Carpeta);
            }

            Label_Inicial.Visible = true;
            Label_Inicial.Text = "Exportando...";
            Diseño_de_muros_concreto_V2.ExportExcel exportExcel = new Diseño_de_muros_concreto_V2.ExportExcel();
            exportExcel.Exportar(Listas_Programa.Ruta_archivo);
            Label_Inicial.Visible = false;
        }

        private void ExportarMemoriasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExportMemorias();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void definirArañaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Diseño_de_muros_concreto_V2.Muros_Consolidados> L_Muro_aux = new List<Diseño_de_muros_concreto_V2.Muros_Consolidados>();
            Diseño_de_muros_concreto_V2.Crear_arania Arañas = new Diseño_de_muros_concreto_V2.Crear_arania();
            Arañas.ShowDialog();
        }
    }
}