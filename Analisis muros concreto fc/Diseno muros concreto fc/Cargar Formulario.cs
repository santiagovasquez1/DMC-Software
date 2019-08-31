using System.Windows.Forms;

namespace Diseno_muros_concreto_fc
{
    internal class Cargar_Formulario
    {
        public static void Open_From_Panel(Panel Formulario_Madre, Form Formulario,Listas_Serializadas_i Lista_i)
        {
            Panel FM = Formulario_Madre;
            Form FH = Formulario;

            if (FM.Controls.Count > 0)
            {
                FM.Controls.Clear();
            }

            FH.TopLevel = false;
            FH.FormBorderStyle = FormBorderStyle.None;
            FH.Dock = DockStyle.Fill;

            FM.Controls.Add(FH);
            FM.Tag = FH;
            FH.Show();
        }

        public static void Open_From_Panel2(Panel Formulario_Madre, Panel Formulario)
        {
            Panel FM = Formulario_Madre;
            Panel FH = Formulario;

            if (FM.Controls.Count > 0)
            {
                FM.Controls.Clear();
            }

            FM.Controls.Add(FH);
            FM.Tag = FH;
            FH.Show();
        }
    }
}