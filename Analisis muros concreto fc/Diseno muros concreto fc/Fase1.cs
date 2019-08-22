using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Diseno_muros_concreto_fc
{
    public partial class Fase1 : Form
    {
        private DataGridView D_Wall_Forces = new DataGridView();
        private DataGridView D_Geometria = new DataGridView();
        private DataGridView D_Shear = new DataGridView();
        private DataGridView D_Flexion = new DataGridView();
        private DataGridView D_Resumen = new DataGridView();


        public void Cargar_Lista()
        {
            if (Listas_Programa.Lista_Muros != null)
            {
                if (Listas_Programa.Lista_Muros.Count > 0)
                {
                    List<Muro> Lista_ordenada = Listas_Programa.Lista_Muros.OrderBy(x1 => x1.Pier).ToList();
                    List<string> Muros_distintos = Lista_ordenada.Select(x => x.Pier).Distinct().ToList();
                    Lista_ordenada.Clear();

                    if (comboBox1.Items.Count > 0) comboBox1.Items.Clear();

                    comboBox1.Items.AddRange(Muros_distintos.ToArray());
                    comboBox1.Text = Convert.ToString(comboBox1.Items[0]);
                    Listas_Programa.Texto_combo = comboBox1.Text;
                }
            }

        }

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
            Formulario.BackgroundColor = Color.White;
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

        public Fase1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Listas_Programa.Texto_combo = comboBox1.Text;
            DataGridView Tabla = new DataGridView();
            object Prueba1;
            DataTable Prueba2;
            int pos;

            foreach (Control Controles in panel1.Controls)

            {
                Prueba1 = Controles.GetType();

                if (Prueba1.ToString().Contains("DataGrid"))
                {
                    Tabla = Convert.ChangeType(Controles, typeof(DataGridView)) as DataGridView;
                    break;
                }

            }
            try

            {
                Prueba2 = Convert.ChangeType(Tabla.DataSource, typeof(DataTable)) as DataTable;

                if (Prueba2.TableName == "Fuerzas")
                {
                    Prueba2.Clear();
                    Bases_de_datos.Datos_Fuerzas();
                    pos = Bases_de_datos.Ds_Shear.Tables.IndexOf("Fuerzas");
                    D_Wall_Forces.Visible = false;
                    SetupDataGridView("Fuerzas", D_Shear, Bases_de_datos.Ds_Shear.Tables[pos]);
                    D_Wall_Forces.Visible = true;
                }

                if (Prueba2.TableName == "Geometria")
                {
                    Prueba2.Clear();
                    Bases_de_datos.Datos_Geometria();
                    pos = Bases_de_datos.Ds_Shear.Tables.IndexOf("Geometria");
                    D_Geometria.Visible = false;
                    SetupDataGridView("Geometria", D_Shear, Bases_de_datos.Ds_Shear.Tables[pos]);
                    D_Geometria.Visible = true;
                }

                if (Prueba2.TableName == "Cortante")
                {
                    Prueba2.Clear();
                    Bases_de_datos.Datos_Cortante();
                    pos = Bases_de_datos.Ds_Shear.Tables.IndexOf("Cortante");
                    D_Shear.Visible = false;
                    SetupDataGridView("Cortante", D_Shear, Bases_de_datos.Ds_Shear.Tables[pos]);
                    D_Shear.Visible = true;
                }

                if (Prueba2.TableName == "Flexion")
                {
                    Prueba2.Clear();
                    Bases_de_datos.Datos_Flexion();
                    pos = Bases_de_datos.Ds_Shear.Tables.IndexOf("Flexion");
                    D_Flexion.Visible = false;
                    SetupDataGridView("Analisis Flexion", D_Shear, Bases_de_datos.Ds_Shear.Tables[pos]);
                    D_Flexion.Visible = true;
                }

                if (Prueba2.TableName == "Resumen")
                {
                    Bases_de_datos.Datos_resumen();
                    pos = Bases_de_datos.Ds_Shear.Tables.IndexOf("Resumen");
                    D_Shear.Visible = false;
                    SetupDataGridView("Resumen muros", D_Resumen, Bases_de_datos.Ds_Shear.Tables[pos]);
                    D_Shear.Visible = true;
                }

            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int pos;
            Bases_de_datos.Datos_Fuerzas();
            pos = Bases_de_datos.Ds_Shear.Tables.IndexOf("Fuerzas");
            D_Wall_Forces.Visible = false;
            SetupDataGridView("Fuerzas", D_Shear, Bases_de_datos.Ds_Shear.Tables[pos]);
            D_Wall_Forces.Visible = true;
            LabelCambio.Text = "Wall Forces";
            panel2.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int pos;
            Bases_de_datos.Datos_Geometria();
            pos = Bases_de_datos.Ds_Shear.Tables.IndexOf("Geometria");
            D_Geometria.Visible = false;
            SetupDataGridView("Geometria", D_Shear, Bases_de_datos.Ds_Shear.Tables[pos]);
            D_Geometria.Visible = true;
            LabelCambio.Text = "Wall Geometry";
            panel2.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int pos;
            Bases_de_datos.Datos_Cortante();
            pos = Bases_de_datos.Ds_Shear.Tables.IndexOf("Cortante");
            D_Shear.Visible = false;
            SetupDataGridView("Diseno a cortante", D_Shear, Bases_de_datos.Ds_Shear.Tables[pos]);
            D_Shear.Visible = true;
            LabelCambio.Text = "Shear Desing";
            panel2.Visible = true;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int pos;
            Bases_de_datos.Datos_Flexion();
            pos = Bases_de_datos.Ds_Shear.Tables.IndexOf("Flexion");
            D_Flexion.Visible = false;
            SetupDataGridView("Analisis Flexion", D_Shear, Bases_de_datos.Ds_Shear.Tables[pos]);
            D_Flexion.Visible = true;
            LabelCambio.Text = "Flexural Stress";
            panel2.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int pos;
            Bases_de_datos.Datos_resumen();
            pos = Bases_de_datos.Ds_Shear.Tables.IndexOf("Resumen");
            D_Shear.Visible = false;
            SetupDataGridView("Resumen muros", D_Resumen, Bases_de_datos.Ds_Shear.Tables[pos]);
            D_Shear.Visible = true;
            LabelCambio.Text = "Report";
            panel2.Visible = true;
        }

        private void Fase1_Load(object sender, EventArgs e)
        {
            if (Listas_Programa.Muros_Consolidados_Listos!=null)
            {
                button10.Enabled = true;
            }
        }

        private void Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {           

             foreach (Muros_Consolidados muro_i in Listas_Programa.Muros_Consolidados_Listos)
             {
                muro_i.Peso_Long.Clear();
                muro_i.Peso_malla.Clear();
                muro_i.Peso_Transv.Clear();
                muro_i.Volumen.Clear();
                muro_i.Calculo_Peso_Aprox();
             }      

            CantidadesAproximadas formCantidades = new CantidadesAproximadas();
            formCantidades.ShowDialog();

        }
    }
}
