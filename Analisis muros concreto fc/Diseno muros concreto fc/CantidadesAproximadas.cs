using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Diseno_muros_concreto_fc
{
    public partial class CantidadesAproximadas : Form
    {
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

        public CantidadesAproximadas()
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

        private void PictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Panel10_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void CantidadesAproximadas_Load(object sender, EventArgs e)
        {
            CargarDataGrid(this.DataGrid_Muros);
            CargarDataGrid2(this.dataGridView1);
            CargarDataGrid3(this.dataGridViewVols);
            dataGridView1.Rows[0].Cells[3].Value = Listas_Programa.Area_ParaTenorApprox;

            if (Listas_Programa.Area_ParaTenorApprox != "")
            {
                dataGridView1.Rows[0].Cells[4].Value = Math.Round((Convert.ToDouble(dataGridView1.Rows[0].Cells[0].Value) + Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value) + Convert.ToDouble(dataGridView1.Rows[0].Cells[2].Value)) / Convert.ToDouble(Listas_Programa.Area_ParaTenorApprox), 2);
            }
        }

        private void CargarDataGrid(DataGridView DataGrid)
        {
            DataGridViewCellStyle Estilo = new DataGridViewCellStyle();
            Estilo.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Estilo.Font = new Font("Verdana", 8);
            Estilo.BackColor = Color.White;

            int count = 0;
            foreach (Muros_Consolidados_1 muro_i in Listas_Programa.Muros_Consolidados_Listos)
            {
                foreach (DataGridViewColumn column in DataGrid.Columns) { column.HeaderCell.Style = Estilo; }
                DataGrid.Rows.Add();

                DataGrid.Rows[count].ReadOnly = true;
                DataGrid.Rows[count].Cells[0].Value = muro_i.Pier_name;
                DataGrid.Rows[count].Cells[1].Value = Math.Round(muro_i.Peso_Long.Sum(), 2);
                DataGrid.Rows[count].Cells[2].Value = Math.Round(muro_i.Peso_malla.Sum(), 2);
                DataGrid.Rows[count].Cells[3].Value = Math.Round(muro_i.Peso_Transv.Sum(), 2);
                double PesoTotal = Math.Round(muro_i.Peso_Long.Sum() + muro_i.Peso_malla.Sum() + muro_i.Peso_Transv.Sum(), 2);
                DataGrid.Rows[count].Cells[4].Value = PesoTotal;
                count = count + 1;
            }

            DataGrid.DefaultCellStyle = Estilo;
        }

        private void CargarDataGrid2(DataGridView DataGrid)
        {
            DataGridViewCellStyle Estilo = new DataGridViewCellStyle();
            Estilo.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Estilo.Font = new Font("Verdana", 8);
            Estilo.BackColor = Color.White;

            foreach (DataGridViewColumn column in DataGrid.Columns) { column.HeaderCell.Style = Estilo; }

            double PesoLongitudinal = 0; double PesoTransversal = 0; double PesoMalla = 0;
            foreach (Muros_Consolidados_1 muro_i in Listas_Programa.Muros_Consolidados_Listos)
            {
                PesoLongitudinal += muro_i.Peso_Long.Sum();
                PesoTransversal += muro_i.Peso_Transv.Sum();
                PesoMalla += muro_i.Peso_malla.Sum();
            }

            DataGrid.Rows.Add();
            DataGrid.Rows[0].Cells[0].ReadOnly = true;
            DataGrid.Rows[0].Cells[1].ReadOnly = true;
            DataGrid.Rows[0].Cells[2].ReadOnly = true;
            DataGrid.Rows[0].Cells[4].ReadOnly = true;

            DataGrid.Rows[0].Cells[0].Value = Math.Round(PesoLongitudinal, 2);
            DataGrid.Rows[0].Cells[1].Value = Math.Round(PesoMalla, 2);
            DataGrid.Rows[0].Cells[2].Value = Math.Round(PesoTransversal, 2);
            DataGrid.Rows[0].Cells[3].ReadOnly = false;
            DataGrid.DefaultCellStyle = Estilo;
        }

        private void CargarDataGrid3(DataGridView DataGrid)
        {
            List<double> Fc_dif = new List<double>();
            List<double> Volumenes = new List<double>();
            Resumen_Vol(Listas_Programa.Muros_Consolidados_Listos, ref Fc_dif, ref Volumenes);

            DataGridViewCellStyle Estilo = new DataGridViewCellStyle();
            Estilo.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Estilo.Font = new Font("Verdana", 8);
            Estilo.BackColor = Color.White;

            foreach (DataGridViewColumn column in DataGrid.Columns) { column.HeaderCell.Style = Estilo; }

            for (int i = 0; i < Fc_dif.Count; i++)
            {
                DataGrid.Rows.Add();
                DataGrid.Rows[i].Cells[0].Value = string.Format(Fc_dif[i].ToString(), "#0");
                DataGrid.Rows[i].Cells[1].Value = string.Format(Convert.ToString(Math.Round(Volumenes[i], 2)), "##0.00");
            }
            DataGrid.DefaultCellStyle = Estilo;
        }

        private void Resumen_Vol(List<Muros_Consolidados_1> Muros_lista, ref List<double> Fc_dif, ref List<double> Volumenes)
        {
            double suma;

            for (int i = 0; i < Muros_lista.Count; i++)
            {
                for (int j = 0; j < Muros_lista[i].fc.Count; j++)
                {
                    Fc_dif.Add(Muros_lista[i].fc[j]);
                }
            }
            Fc_dif = Fc_dif.Distinct().ToList();

            for (int i = 0; i < Fc_dif.Count; i++)
            {
                suma = 0;
                for (int j = 0; j < Muros_lista.Count; j++)
                {
                    for (int k = 0; k < Muros_lista[j].fc.Count; k++)
                    {
                        if (Muros_lista[i].fc[k] == Fc_dif[i])
                        {
                            suma += Muros_lista[j].Volumen[k];
                        }
                    }
                }
                Volumenes.Add(suma);
            }
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double Area = Convert.ToDouble(dataGridView1.Rows[0].Cells[3].Value);
            Listas_Programa.Area_ParaTenorApprox = Convert.ToString(Area);

            if (Area != 0)
            {
                dataGridView1.Rows[0].Cells[4].Value = Math.Round((Convert.ToDouble(dataGridView1.Rows[0].Cells[0].Value) + Convert.ToDouble(dataGridView1.Rows[0].Cells[1].Value) + Convert.ToDouble(dataGridView1.Rows[0].Cells[2].Value)) / Convert.ToDouble(Listas_Programa.Area_ParaTenorApprox), 2);
            }
        }

        private void label3_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
