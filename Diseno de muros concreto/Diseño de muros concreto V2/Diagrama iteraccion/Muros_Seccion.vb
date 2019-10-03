Imports System.Runtime.InteropServices

Public Class Muros_Seccion

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

    Private Sub Muros_Seccion_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        ReleaseCapture()
        SendMessage(Me.Handle, &H112&, &HF012&, 0)
    End Sub

    Private Sub Muros_Seccion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGrid_Muros.Rows.Clear()
        CrearDataGrid(DataGrid_Muros, ListaOrdenada2)

    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Close()
    End Sub

    Public Sub CrearDataGrid(ByVal DataGrid As DataGridView, ByVal ListaMuros As List(Of Muros))

        Dim Estilo As New DataGridViewCellStyle
        Dim Muros As List(Of String)

        Estilo.Alignment = DataGridViewContentAlignment.MiddleCenter
        Estilo.Font = New Font("Verdana", 8)
        Estilo.BackColor = Color.White

        DataGrid.Columns(0).HeaderCell.Style = Estilo
        DataGrid.Columns(1).HeaderCell.Style = Estilo

        Muros = ListaMuros.Select(Function(x) x.NombreMuro).ToList()
        Dim MuroArana = Crear_arania.MuroAranaSelecc

        For i = 0 To Muros.Count - 1

            DataGrid.Rows.Add()

            With DataGrid.Rows(i)
                .Cells(0).Value = Muros(i)
                .Cells(0).ReadOnly = True
                For j = 0 To MuroArana.Muros_arania.Count - 1
                    If Muros(i) = MuroArana.Muros_arania(j).NombreMuro Then
                        .Cells(1).Value = True
                    End If
                Next
            End With
        Next

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub

    Public Shared MurosPertencientes As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click  'Boton Ok

        MurosPertencientes = ""
        Crear_arania.MuroAranaSelecc.Muros_arania.Clear()

        For i = 0 To DataGrid_Muros.Rows.Count - 1
            If DataGrid_Muros.Rows(i).Cells(1).Value = True Then
                Dim MuroPerteneciente As Muros
                MuroPerteneciente = ListaOrdenada2.Find(Function(x) x.NombreMuro = DataGrid_Muros.Rows(i).Cells(0).Value)
                Crear_arania.MuroAranaSelecc.Muros_arania.Add(MuroPerteneciente)

            End If
        Next

        For i = 0 To Crear_arania.MuroAranaSelecc.Muros_arania.Count - 1
            If i < Crear_arania.MuroAranaSelecc.Muros_arania.Count - 1 Then
                MurosPertencientes = MurosPertencientes & Crear_arania.MuroAranaSelecc.Muros_arania(i).NombreMuro & ","
            Else
                MurosPertencientes = MurosPertencientes & Crear_arania.MuroAranaSelecc.Muros_arania(i).NombreMuro
            End If
        Next
        Close()

    End Sub

End Class