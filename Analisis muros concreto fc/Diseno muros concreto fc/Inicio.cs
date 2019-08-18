using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;






namespace Diseno_muros_concreto_fc
{




    public partial class Inicio : Form
    {


        public string ComprobarEntrada = "FAIL";

        public Inicio()
        {
            InitializeComponent();

        }

        private void Inicio_Load(object sender, EventArgs e)


        {
            this.Opacity = 0.0;
            timer1.Start();


        }

        int cont = 0;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1) this.Opacity += 0.05;
            cont += 1;
            if (cont == 20)
            {
                Comprobar(this);
                timer1.Stop();

                //timer2.Start();
            }

        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            this.Opacity -= 0.1;
            if (this.Opacity == 0)
            {
                timer2.Stop();

            }
        }

        public static void Comprobar(Inicio Formulario)
        {                      
            
            string ComprobarEntrada = "FAIL";

            String IP_Servidor = "";
            if (NetworkInterface.GetIsNetworkAvailable())
            {

                string hostname = "FCSAS.COM";

                IPAddress[] addresses = Dns.GetHostAddresses(hostname);
                foreach (IPAddress address in addresses)
                {
                    IP_Servidor = address.ToString();

                }


                List<IPAddressCollection> ListaIPS = new List<IPAddressCollection>();



                foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {

                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        IPAddressCollection IP = properties.DnsAddresses;
                        ListaIPS.Add(IP);
                    }
                }


                for (int i = 0; i < ListaIPS.Count; i++)
                {
                    foreach (IPAddress iPAddress in ListaIPS[i])
                    {
                        string IP_CLIENTE = iPAddress.ToString();
                        if (IP_CLIENTE == IP_Servidor)
                        {
                            ComprobarEntrada = "CORRECT";
                        }

                    }

                }

                if (ComprobarEntrada == "CORRECT")
                {
                    //MessageBox.Show("BIENVENIDO", "efe Prima Ce", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Formulario.Visible = false;
                    Form1 Fromulario = new Form1();
                    Fromulario.ShowDialog();
                }
                else
                {

                    //MessageBox.Show("Acceso Denegado ", "efe Prima Ce", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //Formulario.Close();
                    Formulario.Visible = false;
                    Form1 Fromulario = new Form1();
                    Fromulario.ShowDialog();
                }

            }                       
            else
            {
                MessageBox.Show("SIN CONEXIÓN A LA RED", "efe Prima Ce", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Formulario.Close();
            }

        }













































    }
}
