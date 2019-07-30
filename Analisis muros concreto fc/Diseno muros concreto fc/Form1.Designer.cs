namespace Diseno_muros_concreto_fc
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.B_Report = new System.Windows.Forms.Button();
            this.B_Flexural = new System.Windows.Forms.Button();
            this.B_Shear = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarComoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarMemoriasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analisisEstructuralToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoGeneralToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alzadoRefuerzoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dibujoSeccionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.variablesDeDibujoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.murosSimilaresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Panel6 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_cuantiavol = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LMuros = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Radio_Des = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.Radio_Dmo = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.Panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // B_Report
            // 
            this.B_Report.BackColor = System.Drawing.Color.Transparent;
            this.B_Report.Enabled = false;
            this.B_Report.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.B_Report.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.B_Report.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_Report.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.B_Report.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.B_Report.Location = new System.Drawing.Point(-1, 386);
            this.B_Report.Name = "B_Report";
            this.B_Report.Size = new System.Drawing.Size(193, 45);
            this.B_Report.TabIndex = 68;
            this.B_Report.Text = "Write Report";
            this.B_Report.UseVisualStyleBackColor = false;
            this.B_Report.Click += new System.EventHandler(this.B_Report_Click);
            // 
            // B_Flexural
            // 
            this.B_Flexural.BackColor = System.Drawing.Color.Transparent;
            this.B_Flexural.Enabled = false;
            this.B_Flexural.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.B_Flexural.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.B_Flexural.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_Flexural.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.B_Flexural.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.B_Flexural.Location = new System.Drawing.Point(-1, 344);
            this.B_Flexural.Name = "B_Flexural";
            this.B_Flexural.Size = new System.Drawing.Size(193, 45);
            this.B_Flexural.TabIndex = 66;
            this.B_Flexural.Text = "Calculate Flexure";
            this.B_Flexural.UseVisualStyleBackColor = false;
            this.B_Flexural.Click += new System.EventHandler(this.B_Flexural_Click);
            // 
            // B_Shear
            // 
            this.B_Shear.BackColor = System.Drawing.Color.Transparent;
            this.B_Shear.Enabled = false;
            this.B_Shear.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
            this.B_Shear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.B_Shear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.B_Shear.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.B_Shear.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.B_Shear.Location = new System.Drawing.Point(-1, 302);
            this.B_Shear.Name = "B_Shear";
            this.B_Shear.Size = new System.Drawing.Size(193, 45);
            this.B_Shear.TabIndex = 65;
            this.B_Shear.Text = "Calculate Shear";
            this.B_Shear.UseVisualStyleBackColor = false;
            this.B_Shear.Click += new System.EventHandler(this.button3_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.editarToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(237, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoToolStripMenuItem,
            this.abrirToolStripMenuItem,
            this.guardarToolStripMenuItem,
            this.guardarComoToolStripMenuItem,
            this.exportarMemoriasToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.archivoToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // nuevoToolStripMenuItem
            // 
            this.nuevoToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.nuevoToolStripMenuItem.Name = "nuevoToolStripMenuItem";
            this.nuevoToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.nuevoToolStripMenuItem.Text = "Nuevo";
            this.nuevoToolStripMenuItem.Click += new System.EventHandler(this.nuevoToolStripMenuItem_Click);
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.abrirToolStripMenuItem.Text = "Abrir";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.abrirToolStripMenuItem_Click);
            // 
            // guardarToolStripMenuItem
            // 
            this.guardarToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.guardarToolStripMenuItem.Name = "guardarToolStripMenuItem";
            this.guardarToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.guardarToolStripMenuItem.Text = "Guardar";
            this.guardarToolStripMenuItem.Click += new System.EventHandler(this.guardarToolStripMenuItem_Click);
            // 
            // guardarComoToolStripMenuItem
            // 
            this.guardarComoToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.guardarComoToolStripMenuItem.Name = "guardarComoToolStripMenuItem";
            this.guardarComoToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.guardarComoToolStripMenuItem.Text = "Guardar como";
            this.guardarComoToolStripMenuItem.Click += new System.EventHandler(this.guardarComoToolStripMenuItem_Click);
            // 
            // exportarMemoriasToolStripMenuItem
            // 
            this.exportarMemoriasToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.exportarMemoriasToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.exportarMemoriasToolStripMenuItem.Name = "exportarMemoriasToolStripMenuItem";
            this.exportarMemoriasToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.exportarMemoriasToolStripMenuItem.Text = "Exportar memorias";
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // editarToolStripMenuItem
            // 
            this.editarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.analisisEstructuralToolStripMenuItem,
            this.infoGeneralToolStripMenuItem,
            this.alzadoRefuerzoToolStripMenuItem,
            this.dibujoSeccionToolStripMenuItem,
            this.variablesDeDibujoToolStripMenuItem,
            this.murosSimilaresToolStripMenuItem});
            this.editarToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.editarToolStripMenuItem.Name = "editarToolStripMenuItem";
            this.editarToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.editarToolStripMenuItem.Text = "Editar";
            // 
            // analisisEstructuralToolStripMenuItem
            // 
            this.analisisEstructuralToolStripMenuItem.Name = "analisisEstructuralToolStripMenuItem";
            this.analisisEstructuralToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.analisisEstructuralToolStripMenuItem.Text = "Analisis Estructural";
            this.analisisEstructuralToolStripMenuItem.Click += new System.EventHandler(this.analisisEstructuralToolStripMenuItem_Click);
            // 
            // infoGeneralToolStripMenuItem
            // 
            this.infoGeneralToolStripMenuItem.Name = "infoGeneralToolStripMenuItem";
            this.infoGeneralToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.infoGeneralToolStripMenuItem.Text = "Info General";
            this.infoGeneralToolStripMenuItem.Click += new System.EventHandler(this.InfoGeneralToolStripMenuItem_Click);
            // 
            // alzadoRefuerzoToolStripMenuItem
            // 
            this.alzadoRefuerzoToolStripMenuItem.Name = "alzadoRefuerzoToolStripMenuItem";
            this.alzadoRefuerzoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.alzadoRefuerzoToolStripMenuItem.Text = "Alzado refuerzo";
            this.alzadoRefuerzoToolStripMenuItem.Click += new System.EventHandler(this.AlzadoRefuerzoToolStripMenuItem_Click);
            // 
            // dibujoSeccionToolStripMenuItem
            // 
            this.dibujoSeccionToolStripMenuItem.Name = "dibujoSeccionToolStripMenuItem";
            this.dibujoSeccionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dibujoSeccionToolStripMenuItem.Text = "Dibujo Seccion";
            this.dibujoSeccionToolStripMenuItem.Click += new System.EventHandler(this.DibujoSeccionToolStripMenuItem_Click);
            // 
            // variablesDeDibujoToolStripMenuItem
            // 
            this.variablesDeDibujoToolStripMenuItem.Name = "variablesDeDibujoToolStripMenuItem";
            this.variablesDeDibujoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.variablesDeDibujoToolStripMenuItem.Text = "Variables de Dibujo";
            this.variablesDeDibujoToolStripMenuItem.Click += new System.EventHandler(this.VariablesDeDibujoToolStripMenuItem_Click);
            // 
            // murosSimilaresToolStripMenuItem
            // 
            this.murosSimilaresToolStripMenuItem.Enabled = false;
            this.murosSimilaresToolStripMenuItem.Name = "murosSimilaresToolStripMenuItem";
            this.murosSimilaresToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.murosSimilaresToolStripMenuItem.Text = "Muros Similares";
            this.murosSimilaresToolStripMenuItem.Click += new System.EventHandler(this.MurosSimilaresToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Controls.Add(this.Panel6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(193, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(802, 649);
            this.panel1.TabIndex = 4;
            // 
            // Panel6
            // 
            this.Panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.Panel6.Controls.Add(this.label3);
            this.Panel6.Controls.Add(this.cb_cuantiavol);
            this.Panel6.Controls.Add(this.label6);
            this.Panel6.Controls.Add(this.label8);
            this.Panel6.Controls.Add(this.label7);
            this.Panel6.Controls.Add(this.LMuros);
            this.Panel6.Controls.Add(this.label10);
            this.Panel6.Controls.Add(this.button6);
            this.Panel6.Controls.Add(this.label11);
            this.Panel6.Controls.Add(this.button5);
            this.Panel6.Controls.Add(this.button4);
            this.Panel6.Controls.Add(this.button1);
            this.Panel6.Location = new System.Drawing.Point(0, 0);
            this.Panel6.Name = "Panel6";
            this.Panel6.Size = new System.Drawing.Size(802, 28);
            this.Panel6.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(586, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 19);
            this.label3.TabIndex = 15;
            this.label3.Text = "|";
            // 
            // cb_cuantiavol
            // 
            this.cb_cuantiavol.Enabled = false;
            this.cb_cuantiavol.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.cb_cuantiavol.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(247)))));
            this.cb_cuantiavol.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(222)))), ((int)(((byte)(245)))));
            this.cb_cuantiavol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cb_cuantiavol.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_cuantiavol.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.cb_cuantiavol.Image = global::Diseno_muros_concreto_fc.Properties.Resources.image;
            this.cb_cuantiavol.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cb_cuantiavol.Location = new System.Drawing.Point(604, 1);
            this.cb_cuantiavol.Name = "cb_cuantiavol";
            this.cb_cuantiavol.Size = new System.Drawing.Size(152, 26);
            this.cb_cuantiavol.TabIndex = 14;
            this.cb_cuantiavol.Text = "       Cuantia Volumetrica";
            this.cb_cuantiavol.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(133, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 19);
            this.label6.TabIndex = 10;
            this.label6.Text = "|";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label8.Location = new System.Drawing.Point(233, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 19);
            this.label8.TabIndex = 9;
            this.label8.Text = "|";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Enabled = false;
            this.label7.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(12, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 15);
            this.label7.TabIndex = 7;
            this.label7.Text = "Muro:";
            // 
            // LMuros
            // 
            this.LMuros.Enabled = false;
            this.LMuros.FormattingEnabled = true;
            this.LMuros.Location = new System.Drawing.Point(59, 4);
            this.LMuros.Name = "LMuros";
            this.LMuros.Size = new System.Drawing.Size(68, 21);
            this.LMuros.TabIndex = 6;
            this.LMuros.Tag = "1";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label10.Location = new System.Drawing.Point(483, 4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 19);
            this.label10.TabIndex = 8;
            this.label10.Text = "|";
            // 
            // button6
            // 
            this.button6.Enabled = false;
            this.button6.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(247)))));
            this.button6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(222)))), ((int)(((byte)(245)))));
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button6.Image = global::Diseno_muros_concreto_fc.Properties.Resources.Crearx16;
            this.button6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button6.Location = new System.Drawing.Point(501, 2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(82, 24);
            this.button6.TabIndex = 7;
            this.button6.Text = "     Generar";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label11.Location = new System.Drawing.Point(328, 5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 19);
            this.label11.TabIndex = 6;
            this.label11.Text = "|";
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(247)))));
            this.button5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(222)))), ((int)(((byte)(245)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button5.Image = global::Diseno_muros_concreto_fc.Properties.Resources.yellow_information_icon_icons_com_59572x161;
            this.button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button5.Location = new System.Drawing.Point(345, 1);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(136, 25);
            this.button5.TabIndex = 5;
            this.button5.Text = "     Procesar Información";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(247)))));
            this.button4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(222)))), ((int)(((byte)(245)))));
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button4.Image = global::Diseno_muros_concreto_fc.Properties.Resources.Autocad_23637x16;
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point(247, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(82, 24);
            this.button4.TabIndex = 4;
            this.button4.Text = "     AutoCAD";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(247)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(222)))), ((int)(((byte)(245)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Image = global::Diseno_muros_concreto_fc.Properties.Resources.vcsadded_93506x16;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(148, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "      Agregar";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 26);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(193, 649);
            this.panel2.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.Radio_Des);
            this.panel3.Controls.Add(this.B_Report);
            this.panel3.Controls.Add(this.B_Flexural);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.B_Shear);
            this.panel3.Controls.Add(this.Radio_Dmo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel3.Location = new System.Drawing.Point(0, 28);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(193, 621);
            this.panel3.TabIndex = 69;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Diseno_muros_concreto_fc.Properties.Resources.efe_Prima_Ce_Pixelado;
            this.pictureBox1.Location = new System.Drawing.Point(5, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(187, 179);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 69;
            this.pictureBox1.TabStop = false;
            // 
            // Radio_Des
            // 
            this.Radio_Des.AutoSize = true;
            this.Radio_Des.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.Radio_Des.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Radio_Des.Location = new System.Drawing.Point(20, 266);
            this.Radio_Des.Name = "Radio_Des";
            this.Radio_Des.Size = new System.Drawing.Size(45, 19);
            this.Radio_Des.TabIndex = 1;
            this.Radio_Des.TabStop = true;
            this.Radio_Des.Text = "DES";
            this.Radio_Des.UseVisualStyleBackColor = true;
            this.Radio_Des.CheckedChanged += new System.EventHandler(this.Radio_Des_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(17, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 15);
            this.label2.TabIndex = 23;
            this.label2.Text = "Grado de disipación:";
            // 
            // Radio_Dmo
            // 
            this.Radio_Dmo.AutoSize = true;
            this.Radio_Dmo.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.Radio_Dmo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Radio_Dmo.Location = new System.Drawing.Point(20, 237);
            this.Radio_Dmo.Name = "Radio_Dmo";
            this.Radio_Dmo.Size = new System.Drawing.Size(53, 19);
            this.Radio_Dmo.TabIndex = 0;
            this.Radio_Dmo.TabStop = true;
            this.Radio_Dmo.Text = "DMO";
            this.Radio_Dmo.UseVisualStyleBackColor = true;
            this.Radio_Dmo.CheckedChanged += new System.EventHandler(this.Radio_Dmo_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button2);
            this.panel4.Controls.Add(this.button12);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.button3);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.button9);
            this.panel4.Controls.Add(this.button7);
            this.panel4.Controls.Add(this.button8);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(193, 28);
            this.panel4.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(247)))));
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(222)))), ((int)(((byte)(245)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Image = global::Diseno_muros_concreto_fc.Properties.Resources.Newx16;
            this.button2.Location = new System.Drawing.Point(20, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 23);
            this.button2.TabIndex = 1;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button12
            // 
            this.button12.Enabled = false;
            this.button12.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button12.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button12.Image = global::Diseno_muros_concreto_fc.Properties.Resources.Puntosx16;
            this.button12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button12.Location = new System.Drawing.Point(-2, 0);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(31, 26);
            this.button12.TabIndex = 16;
            this.button12.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label9.Location = new System.Drawing.Point(185, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 19);
            this.label9.TabIndex = 13;
            this.label9.Text = "|";
            // 
            // button3
            // 
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(247)))));
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(222)))), ((int)(((byte)(245)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Image = global::Diseno_muros_concreto_fc.Properties.Resources.Openx16;
            this.button3.Location = new System.Drawing.Point(50, 1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(24, 24);
            this.button3.TabIndex = 2;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(77, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "|";
            // 
            // button9
            // 
            this.button9.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button9.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(247)))));
            this.button9.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(222)))), ((int)(((byte)(245)))));
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.ForeColor = System.Drawing.Color.Black;
            this.button9.Image = global::Diseno_muros_concreto_fc.Properties.Resources.icons8_ms_excel_48X18;
            this.button9.Location = new System.Drawing.Point(155, 2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(24, 22);
            this.button9.TabIndex = 12;
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(247)))));
            this.button7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(222)))), ((int)(((byte)(245)))));
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.Color.Black;
            this.button7.Image = global::Diseno_muros_concreto_fc.Properties.Resources.SaveAllx13;
            this.button7.Location = new System.Drawing.Point(126, 1);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(24, 24);
            this.button7.TabIndex = 10;
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // button8
            // 
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(224)))), ((int)(((byte)(247)))));
            this.button8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(222)))), ((int)(((byte)(245)))));
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.ForeColor = System.Drawing.Color.Black;
            this.button8.Image = global::Diseno_muros_concreto_fc.Properties.Resources.SaveX13;
            this.button8.Location = new System.Drawing.Point(95, 1);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(24, 24);
            this.button8.TabIndex = 11;
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.Button8_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.panel5.Controls.Add(this.button11);
            this.panel5.Controls.Add(this.button10);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(995, 26);
            this.panel5.TabIndex = 23;
            this.panel5.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel5_Paint);
            this.panel5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel5_MouseDown);
            // 
            // button11
            // 
            this.button11.Dock = System.Windows.Forms.DockStyle.Right;
            this.button11.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button11.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.button11.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button11.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button11.Image = global::Diseno_muros_concreto_fc.Properties.Resources.Minimizex16;
            this.button11.Location = new System.Drawing.Point(915, 0);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(40, 26);
            this.button11.TabIndex = 24;
            this.button11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.Button11_Click);
            // 
            // button10
            // 
            this.button10.Dock = System.Windows.Forms.DockStyle.Right;
            this.button10.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.button10.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(101)))), ((int)(((byte)(113)))));
            this.button10.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(17)))), ((int)(((byte)(35)))));
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button10.Image = global::Diseno_muros_concreto_fc.Properties.Resources.x16Blanca;
            this.button10.Location = new System.Drawing.Point(955, 0);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(40, 26);
            this.button10.TabIndex = 23;
            this.button10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.Button10_Click);
            this.button10.MouseLeave += new System.EventHandler(this.Button10_MouseLeave);
            this.button10.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Button10_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(112)))), ((int)(((byte)(113)))));
            this.ClientSize = new System.Drawing.Size(995, 675);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Diseno muros de concreto";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.Panel6.ResumeLayout(false);
            this.Panel6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.Button B_Shear;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarComoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        internal System.Windows.Forms.Button B_Flexural;
        private System.Windows.Forms.ToolStripMenuItem exportarMemoriasToolStripMenuItem;
        internal System.Windows.Forms.Button B_Report;
        private System.Windows.Forms.ToolStripMenuItem editarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoGeneralToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alzadoRefuerzoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dibujoSeccionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem analisisEstructuralToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem variablesDeDibujoToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel Panel6;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.ComboBox LMuros;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem murosSimilaresToolStripMenuItem;
        private System.Windows.Forms.RadioButton Radio_Des;
        private System.Windows.Forms.RadioButton Radio_Dmo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Button cb_cuantiavol;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

