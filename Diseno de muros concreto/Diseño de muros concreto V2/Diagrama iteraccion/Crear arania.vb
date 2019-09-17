Imports System.Runtime.InteropServices

Public Class Crear_arania

    Public Shared Muros_Borrar As String

    'CREAR SOMBRA EN EL FORMULARIO

    Private m_hOriginalParent As Integer
    Private Const GWL_HWNDPARENT As Integer = -8
    Private Declare Function GetClassLong Lib "user32" Alias "GetClassLongA" (lngHandler As IntPtr, lngIndex As Integer) As Integer
    Private Declare Function GetDesktopWindow Lib "user32" () As Integer
    Private Declare Function SetClassLong Lib "user32" Alias "SetClassLongA" (lngHandler As IntPtr, lngIndex As Integer, lngNewClassLong As Integer) As Integer
    Private Declare Function SetWindowLong Lib "user32" Alias "SetWindowLongA" (hWnd As IntPtr, nIndex As Integer, dwNewLong As Integer) As Integer

    Public Sub New()

        ' Esta llamada es exigida por el diseñador.
        InitializeComponent()

        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        '
        Me.SuspendLayout()

        ' NO EJECUTAR LO SIGUIENTE EN EL EVENTO LOAD DEL FORMULARIO

        Const CS_DROPSHADOW As Integer = &H20000
        Const GCL_STYLE As Integer = -26

        m_hOriginalParent = SetWindowLong(Me.Handle, GWL_HWNDPARENT, GetDesktopWindow())
        SetClassLong(Me.Handle, GCL_STYLE, GetClassLong(Me.Handle, GCL_STYLE) Or CS_DROPSHADOW)
        Me.ResumeLayout(False)

    End Sub
    'Mover Pestaña
    <DllImport("user32.DLL", EntryPoint:="ReleaseCapture")>
    Private Shared Sub ReleaseCapture()
    End Sub

    <DllImport("user32.DLL", EntryPoint:="SendMessage")>
    Private Shared Sub SendMessage(ByVal hWnd As System.IntPtr, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer)
    End Sub

    Private Sub Panel1_MouseMove(sender As Object, e As MouseEventArgs) Handles Panel1.MouseMove
        ReleaseCapture()
        SendMessage(Me.Handle, &H112&, &HF012&, 0)
    End Sub

    Private Sub Label9_MouseMove(sender As Object, e As MouseEventArgs) Handles Label9.MouseMove
        ReleaseCapture()
        SendMessage(Me.Handle, &H112&, &HF012&, 0)
    End Sub




    Private Sub Crear_arania_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim Estilo As New DataGridViewCellStyle
        Estilo.Alignment = DataGridViewContentAlignment.MiddleCenter
        Estilo.Font = New Font("Verdana", 8)
        Estilo.BackColor = Color.White
        Data_arania.DefaultCellStyle = Estilo
        Cargar_Tabla()

    End Sub

    Private Sub Data_arania_RowLeave(sender As Object, e As DataGridViewCellEventArgs) Handles Data_arania.RowLeave

        Dim index As Integer = e.RowIndex
        Dim arania_i As Arania = New Arania With
        {
            .Label = Data_arania.Rows(index).Cells(0).Value,
            .Muros_arania = New List(Of Muros)
        }

    End Sub

    Public Shared MuroAranaSelecc As Arania
    Public Shared ItemMuroArana As Integer
    Private Sub Data_arania_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles Data_arania.CellContentClick

        Dim senderGrid = DirectCast(sender, DataGridView)
        Dim label As String

        If TypeOf senderGrid.Columns(e.ColumnIndex) Is DataGridViewButtonColumn AndAlso e.RowIndex >= 0 Then
            label = Data_arania.Rows(e.RowIndex).Cells(0).Value
            MuroAranaSelecc = Lista_aranias.Find(Function(x) x.Label = label)
            ItemMuroArana = e.RowIndex
            Muros_Seccion.ShowDialog()
        End If


    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Close()
    End Sub



    Private Sub B_addrows_Click(sender As Object, e As EventArgs) Handles B_addrows.Click

        Dim Label As String = InputBox("Ingrese el nombre de la araña", "efe Prima ce", $"Araña{Data_arania.Rows.Count + 1}")

        If Label <> "" Then
            Data_arania.Rows.Add()
            Data_arania.Rows(Data_arania.Rows.Count - 1).Cells(0).Value = Label
            Data_arania.Rows(Data_arania.Rows.Count - 1).Cells(2).Value = "Agregar Muros"
            Dim Arania As Arania = New Arania
            Arania.Label = Data_arania.Rows(Data_arania.Rows.Count - 1).Cells(0).Value
            Arania.Muros_arania = New List(Of Muros)
            Lista_aranias.Add(Arania)
        End If


    End Sub


    Public Sub Cargar_Tabla()

        Dim Texto As String
        For i = 0 To Lista_aranias.Count - 1
            Data_arania.Rows.Add()
            Data_arania.Rows(i).Cells(0).Value = Lista_aranias(i).Label
            Texto = $"{Lista_aranias(i).Muros_arania.Select(Function(x) x.NombreMuro).ToArray}"
            Data_arania.Rows(i).Cells(2).Value = "Agregar Muros"
        Next

    End Sub

    Public Sub Eliminar_fila(ByRef Tabla As DataGridView)

        Dim indice As Integer

        For i = 0 To Tabla.Rows.Count - 1

            If Tabla.Rows(i).Cells(0).Value = Muros_Borrar Then
                Tabla.Rows.RemoveAt(i)
                indice = Lista_aranias.FindIndex(Function(x) x.Label = Muros_Borrar)
                Lista_aranias.RemoveAt(indice)
                Exit For
            End If

        Next

    End Sub

    Private Sub EliminarArañaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EliminarArañaToolStripMenuItem.Click
        Eliminar_fila(Data_arania)
    End Sub

    Private Sub Data_arania_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Data_arania.CellMouseDown
        If e.Button = MouseButtons.Right Then
            Data_arania.ContextMenuStrip = ContextMenuStrip1
            Muros_Borrar = Data_arania.Rows(e.RowIndex).Cells(0).Value
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Close()
    End Sub

    Private NombreAnterior As String
    Private Sub Data_arania_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles Data_arania.CellValueChanged

        If e.ColumnIndex = 0 Then

            If NombreAnterior <> Nothing Then
                Try
                    If Lista_aranias.Exists(Function(x) x.Label = Data_arania.Rows(e.RowIndex).Cells(0).Value) Then
                        MsgBox("La araña a definir ya existe.", MsgBoxStyle.Exclamation, "efe Prima Ce")
                        Data_arania.Rows(e.RowIndex).Cells(0).Value = NombreAnterior

                    Else Lista_aranias.Find(Function(x) x.Label = NombreAnterior).Label = Data_arania.Rows(e.RowIndex).Cells(0).Value

                    End If
                Catch
                End Try
            End If

        End If
    End Sub


    Private Sub Crear_arania_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        If Data_arania.Rows.Count > 0 Then
            Data_arania.Rows(ItemMuroArana).Cells(1).Value = Muros_Seccion.MurosPertencientes
        End If
    End Sub

    Private Sub Data_arania_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles Data_arania.CellBeginEdit
        NombreAnterior = Data_arania.Rows(e.RowIndex).Cells(0).Value
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim GenerarArchivo As New AutoCADEtabs(Lista_aranias.ToList)


    End Sub
End Class