<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_DireccionCambiodeEspesor
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DataGrid_Muros = New System.Windows.Forms.DataGridView()
        Me.Muros = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ListadeMuros = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel2.SuspendLayout()
        CType(Me.DataGrid_Muros, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Transparent
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Button2)
        Me.Panel2.Controls.Add(Me.Button1)
        Me.Panel2.Controls.Add(Me.DataGrid_Muros)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 23)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(291, 421)
        Me.Panel2.TabIndex = 15
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(161, 374)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(65, 23)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Cancelar"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Calibri", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(72, 375)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(59, 23)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Ok"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DataGrid_Muros
        '
        Me.DataGrid_Muros.AllowUserToAddRows = False
        Me.DataGrid_Muros.AllowUserToDeleteRows = False
        Me.DataGrid_Muros.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight
        Me.DataGrid_Muros.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGrid_Muros.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Muros, Me.ListadeMuros})
        Me.DataGrid_Muros.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.DataGrid_Muros.GridColor = System.Drawing.SystemColors.Control
        Me.DataGrid_Muros.Location = New System.Drawing.Point(11, 17)
        Me.DataGrid_Muros.Name = "DataGrid_Muros"
        Me.DataGrid_Muros.Size = New System.Drawing.Size(266, 343)
        Me.DataGrid_Muros.TabIndex = 0
        '
        'Muros
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        Me.Muros.DefaultCellStyle = DataGridViewCellStyle1
        Me.Muros.HeaderText = "Muros"
        Me.Muros.Name = "Muros"
        '
        'ListadeMuros
        '
        Me.ListadeMuros.HeaderText = "Dirección:"
        Me.ListadeMuros.Items.AddRange(New Object() {"Derecha", "Izquierda", "Arriba", "Abajo", "Centro", "No Aplica"})
        Me.ListadeMuros.Name = "ListadeMuros"
        Me.ListadeMuros.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ListadeMuros.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(291, 23)
        Me.Panel1.TabIndex = 14
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Label9.Location = New System.Drawing.Point(8, 3)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(163, 15)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "Dirección Cambio de Espesor"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox1.Image = Global.Diseño_de_muros_concreto_V2.My.Resources.Resources.close_button
        Me.PictureBox1.Location = New System.Drawing.Point(267, 5)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(10, 10)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 11
        Me.PictureBox1.TabStop = False
        '
        'Form_DireccionCambiodeEspesor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(291, 444)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Form_DireccionCambiodeEspesor"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form_DireccionCambiodeEspesor"
        Me.Panel2.ResumeLayout(False)
        CType(Me.DataGrid_Muros, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents Button2 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents DataGrid_Muros As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label9 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Muros As DataGridViewTextBoxColumn
    Friend WithEvents ListadeMuros As DataGridViewComboBoxColumn
End Class
