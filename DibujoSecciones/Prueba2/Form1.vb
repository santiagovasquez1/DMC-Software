Imports System.Runtime.InteropServices

Public Class Form1

    Public RutaArchivo As String
    Public Lista_Cantidades As New Lista_Cantidades

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Try
        Dim Lista_i As New Listas_Serializadas
        Ganchos_180.Clear()
        Serializador.Deserializar(RutaArchivo, Lista_i)

        'ArchivoTexto(Me)

        'Cargar_Resumen()
        'CargarRefuerzo()

        Ganchos_180.Add("4.5mm", 0.116)
        Ganchos_180.Add("2", 0.116)
        Ganchos_180.Add("3", 0.14)
        Ganchos_180.Add("4", 0.167)
        Ganchos_180.Add("5", 0.192)
        Ganchos_180.Add("6", 0.228)
        Ganchos_180.Add("7", 0.266)
        Ganchos_180.Add("8", 0.305)
        Ganchos_180.Add("10", 0.457)
        ' Catch

        'MsgBox("Sin Información", MsgBoxStyle.Exclamation, "efe Prima Ce")
        '    Me.Close()
        ' End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Piso_Box_TextChanged(sender As Object, e As EventArgs) Handles Piso_Box.TextChanged

    End Sub

    Private Sub CargarDatos_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        IniciarAplicacion(Me, Lista_Cantidades)
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

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Me.Close()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs)
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        PictureBox1.BackColor = Color.White
    End Sub

    Private Sub PictureBox1_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox1.MouseLeave
        PictureBox1.BackColor = Color.Transparent
    End Sub

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

End Class