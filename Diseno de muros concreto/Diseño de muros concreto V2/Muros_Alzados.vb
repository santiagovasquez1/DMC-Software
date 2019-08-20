Imports System.Runtime.InteropServices
Imports Diseño_de_muros_concreto_V2

Public Class Muros_Alzados
    Public AtivarEvento As Boolean

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

        'Muros_lista_2 = CType(ListaMuros, List(Of Muros_Consolidados))

    End Sub

    Private Sub Muros_Alzados_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AtivarEvento = True
        CrearDataGrid(DataGrid_Muros, Muros_lista_2)
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

    Public Sub CrearDataGrid(ByVal DataGrid As DataGridView, ByVal ListaMuros As List(Of Muros_Consolidados))

        Dim Estilo As New DataGridViewCellStyle
        Dim Muros As List(Of String)
        Dim Alzado_i As Muros_alzados_C
        Dim indice As Integer

        Estilo.Alignment = DataGridViewContentAlignment.MiddleCenter
        Estilo.Font = New Font("Verdana", 8)
        Estilo.BackColor = Color.White

        DataGrid.Columns(0).HeaderCell.Style = Estilo
        DataGrid.Columns(1).HeaderCell.Style = Estilo

        Muros = ListaMuros.Select(Function(x) x.Pier_name).Distinct.ToList()

        For i = 0 To Muros.Count - 1

            indice = Lista_graficar.FindIndex(Function(x) x.Nombre = Muros(i))
            If Lista_graficar.Count = 0 Or indice < 0 Then
                Alzado_i = New Muros_alzados_C With
                {
                    .Nombre = Muros(i),
                    .Graficar = False
                }
                indice = 0
                Lista_graficar.Add(Alzado_i)
            Else
                Alzado_i = Lista_graficar(indice)
            End If

            DataGrid.Rows.Add()

            With DataGrid.Rows(i)

                .Cells(0).Value = Alzado_i.Nombre
                .Cells(0).ReadOnly = True
                .Cells(1).Value = Alzado_i.Graficar

            End With

        Next

        DataGrid.RowsDefaultCellStyle = Estilo

    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Close()
    End Sub

    Private Sub PictureBox2_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox2.MouseMove
        PictureBox2.BackColor = Color.White
    End Sub

    Private Sub PictureBox2_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox2.MouseLeave
        PictureBox2.BackColor = Color.Transparent
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        For i = 0 To Me.DataGrid_Muros.Rows.Count - 1

            DataGrid_Muros.Rows(i).Cells(1).Value = True

        Next

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim indice As Integer
        Dim Muro_i As String

        For i = 0 To Me.DataGrid_Muros.Rows.Count - 1

            Muro_i = DataGrid_Muros.Rows(i).Cells(0).Value
            indice = Lista_graficar.FindIndex(Function(x) x.Nombre = Muro_i)

            If indice >= 0 Then
                Lista_graficar(indice).Graficar = DataGrid_Muros.Rows(i).Cells(1).Value
            End If

        Next

        Me.Close()
    End Sub
End Class