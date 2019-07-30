Imports System.Runtime.InteropServices

Public Class f_variables



    Private Sub f_variables_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        T_Hviga.TextAlign = HorizontalAlignment.Center
        T_prof.TextAlign = HorizontalAlignment.Center
        T_Vf.TextAlign = HorizontalAlignment.Center
        T_piso.TextAlign = HorizontalAlignment.Center
        T_arranque.TextAlign = HorizontalAlignment.Center

        Dim aviso As New ToolTip
        Dim texto As String

        'texto = "Ingrese el nivel de fundacíon ej: -2.40 ó 0.00"
        'aviso.SetToolTip(Label7, texto)
        'Me.Controls.Add(Label7)

        'texto = "Ingrese el nombre de piso ej: Losa, Piso, Story"
        'aviso.SetToolTip(Label3, texto)
        'Me.Controls.Add(Label3)

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

    Private Sub PictureBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseMove
        PictureBox1.BackColor = Color.White
    End Sub


    Private Sub PictureBox1_MouseLeave(sender As Object, e As EventArgs) Handles PictureBox1.MouseLeave
        PictureBox1.BackColor = Color.Transparent
    End Sub








    Private Sub Cb_Aceptar_Click_1(sender As Object, e As EventArgs) Handles cb_Aceptar.Click

        Dim Aranque1 As Single = 0
        If T_Hviga.Text = Nothing Or T_prof.Text = Nothing Or T_Vf.Text = Nothing Or T_piso.Text = Nothing Or T_arranque.Text = Nothing Then
            MsgBox("Llene todos los campos", vbInformation, "Efe prima Ce")
        ElseIf IsNumeric(T_Hviga.Text) And IsNumeric(T_prof.Text) And IsNumeric(T_Vf.Text) And IsNumeric(T_arranque.Text) Then
            Hviga = T_Hviga.Text
            prof = T_prof.Text
            Hfunda = T_Vf.Text
            Nombre_Nivel = T_piso.Text
            Arranque = T_arranque.Text
            Me.Hide()
        Else
            MsgBox("Ingrese los datos correctamente", vbInformation, "Efe prima Ce")
        End If

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