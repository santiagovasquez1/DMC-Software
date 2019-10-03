﻿Imports System.Runtime.InteropServices

Public Class Seccion



    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim Lista_i As New Listas_serializadas
        If Muros_lista_2 Is Nothing Then
            Muros_lista_2 = New List(Of Muros_Consolidados)
            Serializador.Deserializar(Ruta_1, Lista_i)
        End If

        Muros_lista_2 = Muros_lista_2.OrderBy(Function(x) x.Pier_name).ToList()

        If Lista_Cantidades1 Is Nothing Then
            Lista_Cantidades1 = New Lista_Cantidades
            Lista_Cantidades1.ListaRefuerzoHorzontal = New List(Of RefuerzoHorizontal)
        End If
        Cargar_areas_refuerzo()

        If Ruta_1 = "" Then
            Me.Close()
            MsgBox("Sin Información", MsgBoxStyle.Exclamation, "efe Prima Ce")
        End If



    End Sub


    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        If Hviga <> 0 Then
            IniciarAplicacion(Me)
        Else
            f_variables.Show()
        End If
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        'If ListaOrdenada.Count > 0 Then
        '    Dim Diagrama_i As New Diagrama(ListaOrdenada)
        'End If

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
