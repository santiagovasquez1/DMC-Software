<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Seccion
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Piso_Box = New System.Windows.Forms.TextBox()
        Me.BarraPersonalizada = New System.Windows.Forms.Panel()
        Me.Label_BarraProgreso = New System.Windows.Forms.Label()
        Me.BarraPersonalizada2 = New System.Windows.Forms.Panel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Panel2.SuspendLayout()
        Me.BarraPersonalizada.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.CheckBox1)
        Me.Panel2.Controls.Add(Me.Button2)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Piso_Box)
        Me.Panel2.Controls.Add(Me.BarraPersonalizada)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 23)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(306, 161)
        Me.Panel2.TabIndex = 25
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.CheckBox1.Location = New System.Drawing.Point(94, 62)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(121, 19)
        Me.CheckBox1.TabIndex = 26
        Me.CheckBox1.Text = "Tablas de Estribos"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.White
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(94, 92)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(121, 33)
        Me.Button2.TabIndex = 23
        Me.Button2.Text = "Seleccionar Sección"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(55, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 15)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Numero de Piso:"
        '
        'Piso_Box
        '
        Me.Piso_Box.Font = New System.Drawing.Font("Calibri", 9.75!)
        Me.Piso_Box.Location = New System.Drawing.Point(160, 26)
        Me.Piso_Box.Name = "Piso_Box"
        Me.Piso_Box.Size = New System.Drawing.Size(69, 23)
        Me.Piso_Box.TabIndex = 2
        '
        'BarraPersonalizada
        '
        Me.BarraPersonalizada.BackColor = System.Drawing.Color.Transparent
        Me.BarraPersonalizada.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.BarraPersonalizada.Controls.Add(Me.Label_BarraProgreso)
        Me.BarraPersonalizada.Controls.Add(Me.BarraPersonalizada2)
        Me.BarraPersonalizada.Location = New System.Drawing.Point(3, 135)
        Me.BarraPersonalizada.Name = "BarraPersonalizada"
        Me.BarraPersonalizada.Size = New System.Drawing.Size(299, 14)
        Me.BarraPersonalizada.TabIndex = 25
        Me.BarraPersonalizada.Visible = False
        '
        'Label_BarraProgreso
        '
        Me.Label_BarraProgreso.AutoSize = True
        Me.Label_BarraProgreso.BackColor = System.Drawing.Color.Transparent
        Me.Label_BarraProgreso.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_BarraProgreso.ForeColor = System.Drawing.Color.Black
        Me.Label_BarraProgreso.Location = New System.Drawing.Point(138, 0)
        Me.Label_BarraProgreso.Name = "Label_BarraProgreso"
        Me.Label_BarraProgreso.Size = New System.Drawing.Size(21, 13)
        Me.Label_BarraProgreso.TabIndex = 26
        Me.Label_BarraProgreso.Text = "0%"
        Me.Label_BarraProgreso.Visible = False
        '
        'BarraPersonalizada2
        '
        Me.BarraPersonalizada2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(21, Byte), Integer))
        Me.BarraPersonalizada2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.BarraPersonalizada2.Location = New System.Drawing.Point(0, 0)
        Me.BarraPersonalizada2.Name = "BarraPersonalizada2"
        Me.BarraPersonalizada2.Size = New System.Drawing.Size(0, 12)
        Me.BarraPersonalizada2.TabIndex = 24
        Me.BarraPersonalizada2.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(306, 23)
        Me.Panel1.TabIndex = 24
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox1.Image = Global.Diseño_de_muros_concreto_V2.My.Resources.Resources.close_button
        Me.PictureBox1.Location = New System.Drawing.Point(291, 6)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(10, 10)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 11
        Me.PictureBox1.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(3, 4)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(89, 15)
        Me.Label6.TabIndex = 21
        Me.Label6.Text = "Dibujar Sección"
        '
        'Seccion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(306, 184)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Seccion"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Seccion"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.BarraPersonalizada.ResumeLayout(False)
        Me.BarraPersonalizada.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents Piso_Box As TextBox
    Friend WithEvents BarraPersonalizada As Panel
    Friend WithEvents Label_BarraProgreso As Label
    Friend WithEvents BarraPersonalizada2 As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Label6 As Label
End Class
