namespace Diseno_muros_concreto_fc
{
    partial class CantidadesAproximadas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Panel10 = new System.Windows.Forms.Panel();
            this.PictureBox2 = new System.Windows.Forms.PictureBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.LabelCambio = new System.Windows.Forms.Label();
            this.DataGrid_Muros = new System.Windows.Forms.DataGridView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tenor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewVols = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Muros = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Similars = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PesoMalla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PesoTransversal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PesoTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Muros)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVols)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel10
            // 
            this.Panel10.BackColor = System.Drawing.Color.Gainsboro;
            this.Panel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel10.Controls.Add(this.PictureBox2);
            this.Panel10.Controls.Add(this.Label9);
            this.Panel10.Controls.Add(this.LabelCambio);
            this.Panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel10.Location = new System.Drawing.Point(0, 0);
            this.Panel10.Name = "Panel10";
            this.Panel10.Size = new System.Drawing.Size(667, 29);
            this.Panel10.TabIndex = 14;
            this.Panel10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel10_MouseDown);
            // 
            // PictureBox2
            // 
            this.PictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.PictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PictureBox2.Image = global::Diseno_muros_concreto_fc.Properties.Resources.close_button2;
            this.PictureBox2.Location = new System.Drawing.Point(634, 7);
            this.PictureBox2.Name = "PictureBox2";
            this.PictureBox2.Size = new System.Drawing.Size(10, 10);
            this.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox2.TabIndex = 15;
            this.PictureBox2.TabStop = false;
            this.PictureBox2.Click += new System.EventHandler(this.PictureBox2_Click);
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.Label9.Location = new System.Drawing.Point(10, 7);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(141, 15);
            this.Label9.TabIndex = 14;
            this.Label9.Text = "Cantidades Aproximadas";
            // 
            // LabelCambio
            // 
            this.LabelCambio.AutoSize = true;
            this.LabelCambio.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.LabelCambio.Location = new System.Drawing.Point(8, 7);
            this.LabelCambio.Name = "LabelCambio";
            this.LabelCambio.Size = new System.Drawing.Size(0, 15);
            this.LabelCambio.TabIndex = 1;
            // 
            // DataGrid_Muros
            // 
            this.DataGrid_Muros.AllowUserToAddRows = false;
            this.DataGrid_Muros.AllowUserToDeleteRows = false;
            this.DataGrid_Muros.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.DataGrid_Muros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGrid_Muros.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Muros,
            this.Similars,
            this.PesoMalla,
            this.PesoTransversal,
            this.PesoTotal,
            this.Column1});
            this.DataGrid_Muros.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DataGrid_Muros.GridColor = System.Drawing.SystemColors.Control;
            this.DataGrid_Muros.Location = new System.Drawing.Point(11, 35);
            this.DataGrid_Muros.Name = "DataGrid_Muros";
            this.DataGrid_Muros.Size = new System.Drawing.Size(645, 286);
            this.DataGrid_Muros.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.Tenor});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.Location = new System.Drawing.Point(9, 367);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(645, 82);
            this.dataGridView1.TabIndex = 15;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellEndEdit);
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.HeaderText = "Peso Longitudinal (kgf)";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCheckBoxColumn1.Width = 135;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Peso Malla (kgf)";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Peso Transversal (kgf)";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Area (m²)";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 80;
            // 
            // Tenor
            // 
            this.Tenor.HeaderText = "Tenor  (kgf/m²)";
            this.Tenor.Name = "Tenor";
            this.Tenor.Width = 78;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(1, 330);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(666, 29);
            this.panel1.TabIndex = 16;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Panel1_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Resumen Acero";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(8, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 15);
            this.label2.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.dataGridViewVols);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.DataGrid_Muros);
            this.panel2.Controls.Add(this.Panel10);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(669, 650);
            this.panel2.TabIndex = 26;
            // 
            // dataGridViewVols
            // 
            this.dataGridViewVols.AllowUserToAddRows = false;
            this.dataGridViewVols.AllowUserToDeleteRows = false;
            this.dataGridViewVols.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridViewVols.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewVols.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn5});
            this.dataGridViewVols.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dataGridViewVols.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridViewVols.Location = new System.Drawing.Point(6, 490);
            this.dataGridViewVols.Name = "dataGridViewVols";
            this.dataGridViewVols.Size = new System.Drawing.Size(648, 147);
            this.dataGridViewVols.TabIndex = 18;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "F\'c (kgf/cm²)";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.Width = 135;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Volumen Concreto(m³)";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Gainsboro;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Location = new System.Drawing.Point(1, 455);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(666, 29);
            this.panel3.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(1, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Resumen Volumenes Concreto";
            this.label3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label3_MouseDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(8, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 15);
            this.label4.TabIndex = 1;
            // 
            // Muros
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.Muros.DefaultCellStyle = dataGridViewCellStyle4;
            this.Muros.HeaderText = "Muro";
            this.Muros.Name = "Muros";
            this.Muros.ReadOnly = true;
            this.Muros.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Muros.Width = 50;
            // 
            // Similars
            // 
            this.Similars.HeaderText = "Peso Longitudinal (kgf)";
            this.Similars.Name = "Similars";
            this.Similars.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Similars.Width = 130;
            // 
            // PesoMalla
            // 
            this.PesoMalla.HeaderText = "Peso Malla (kgf)";
            this.PesoMalla.Name = "PesoMalla";
            this.PesoMalla.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // PesoTransversal
            // 
            this.PesoTransversal.HeaderText = "Peso Transversal (kgf)";
            this.PesoTransversal.Name = "PesoTransversal";
            this.PesoTransversal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PesoTransversal.Width = 126;
            // 
            // PesoTotal
            // 
            this.PesoTotal.HeaderText = "Peso Total (kgf)";
            this.PesoTotal.Name = "PesoTotal";
            this.PesoTotal.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PesoTotal.Width = 90;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "%Ref";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // CantidadesAproximadas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 650);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CantidadesAproximadas";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "CantidadesAproximadas";
            this.Load += new System.EventHandler(this.CantidadesAproximadas_Load);
            this.Panel10.ResumeLayout(false);
            this.Panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGrid_Muros)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewVols)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Panel Panel10;
        internal System.Windows.Forms.PictureBox PictureBox2;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label LabelCambio;
        internal System.Windows.Forms.DataGridView DataGrid_Muros;
        internal System.Windows.Forms.DataGridView dataGridView1;
        internal System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tenor;
        internal System.Windows.Forms.DataGridView dataGridViewVols;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        internal System.Windows.Forms.Panel panel3;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Muros;
        private System.Windows.Forms.DataGridViewTextBoxColumn Similars;
        private System.Windows.Forms.DataGridViewTextBoxColumn PesoMalla;
        private System.Windows.Forms.DataGridViewTextBoxColumn PesoTransversal;
        private System.Windows.Forms.DataGridViewTextBoxColumn PesoTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}