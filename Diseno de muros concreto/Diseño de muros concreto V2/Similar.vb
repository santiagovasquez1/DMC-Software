Imports System.Runtime.InteropServices
Public Class Similar
    Public AtivarEvento As Boolean
    Public Sub CrearDataGrid(ByVal DataGrid As DataGridView, ByVal ListaMuros As List(Of Muros_Consolidados))

        Dim Estilo As New DataGridViewCellStyle
        Estilo.Alignment = DataGridViewContentAlignment.MiddleCenter
        Estilo.Font = New Font("Verdana", 8)
        Estilo.BackColor = Color.White

        DataGrid.Columns(0).HeaderCell.Style = Estilo
        DataGrid.Columns(1).HeaderCell.Style = Estilo
        DataGrid.Columns(2).HeaderCell.Style = Estilo


        Dim ListaMurosMaestros As New List(Of Muros_Consolidados)
        Dim ListaMurosMaestrosName As New List(Of String)


        ListaMurosMaestros = ListaMuros.FindAll(Function(x) x.isMuroMaestro = True)
        If ListaMurosMaestros.Count > 0 Then
            ListaMurosMaestrosName = ListaMurosMaestros.Select(Function(x) x.Pier_name).ToList()
        End If



        For i = 0 To ListaMuros.Count - 1
            DataGrid.Rows.Add()
            With DataGrid.Rows(i)
                .Cells(0).Value = ListaMuros(i).Pier_name
                .Cells(0).Style = Estilo
                .Cells(0).ReadOnly = True

                .Cells(1).Value = ListaMuros(i).isMuroMaestro
                .Cells(1).Style = Estilo

                Dim ComboBox As DataGridViewComboBoxCell = .Cells(2)
                ComboBox.Items.AddRange(ListaMurosMaestrosName.ToArray())

                If ListaMuros(i).MuroSimilar IsNot Nothing Then
                    .Cells(2).Value = ListaMuros(i).MuroSimilar.Pier_name
                Else
                    .Cells(2).Value = ""
                End If

                ComboBox.Style.BackColor = Color.White

                If ListaMuros(i).isMuroMaestro = True Then
                    .Cells(2).ReadOnly = True
                Else
                    .Cells(2).ReadOnly = False
                End If

                .Cells(2).Style = Estilo

            End With
        Next


    End Sub

    Private Sub Similar_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Me.StartPosition = FormStartPosition.Manual
        'Me.Location = New Point(f_alzado.Location.X + f_alzado.Width / 2 - Me.Width / 2, f_alzado.Location.Y + f_alzado.Height / 2 - Me.Height / 2)



        CrearDataGrid(DataGrid_Muros, Muros_lista_2)
        AtivarEvento = True
    End Sub

    Sub ConfirmarMaestrosSimilares(ByVal DataGrid As DataGridView)

        Dim Indice As Integer

        For i = 0 To DataGrid.Rows.Count - 1
            With DataGrid.Rows(i)

                'Confirmar Maestros 
                If .Cells(1).Value = True Then
                    Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).isMuroMaestro = True

                    Dim IndiceMuro As Integer = Muros_lista_2.FindIndex(Function(x) x.Pier_name = .Cells(0).Value)

                    For j = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1
                        If refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                            refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)).IsMuroMaestro = True
                        End If
                        If alzado_lista.Find(Function(x) x.pier = .Cells(0).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                            alzado_lista.Find(Function(x) x.pier = .Cells(0).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j)).isMuroMaestro = True
                        End If
                    Next

                Else
                    Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).isMuroMaestro = False

                    Dim IndiceMuro As Integer = Muros_lista_2.FindIndex(Function(x) x.Pier_name = .Cells(0).Value)

                    For j = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1
                        If refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                            refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)).IsMuroMaestro = False
                        End If
                        If alzado_lista.Find(Function(x) x.pier = .Cells(0).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                            alzado_lista.Find(Function(x) x.pier = .Cells(0).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j)).isMuroMaestro = False
                        End If
                    Next
                End If

                'Confirmar Similares

                If .Cells(2).Value <> "" Then

                    '''Asignar propiedades del maestro a reporte

                    'Dim Muro_maestro As New Muros_Consolidados
                    'Muro_maestro = Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(2).Value)

                    'Dim Muro_hijo As New Muros_Consolidados

                    'Muro_hijo = Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value)
                    'Muro_hijo.Malla = Muro_maestro.Malla
                    'Muro_hijo.Lebe_Izq = Muro_maestro.Lebe_Izq
                    'Muro_hijo.Lebe_Der = Muro_maestro.Lebe_Der
                    'Muro_hijo.Lebe_Centro = Muro_maestro.Lebe_Centro
                    'Muro_hijo.Zc_Izq = Muro_maestro.Zc_Izq
                    'Muro_hijo.Zc_Der = Muro_maestro.Zc_Der
                    'Muro_hijo.Est_ebe = Muro_maestro.Est_ebe
                    'Muro_hijo.Sep_ebe = Muro_maestro.Sep_ebe
                    'Muro_hijo.Est_Zc = Muro_maestro.Est_Zc
                    'Muro_hijo.Sep_Zc = Muro_maestro.Sep_Zc
                    'Muro_hijo.As_Long = Muro_maestro.As_Long
                    'Muro_hijo.ramas_der = Muro_maestro.ramas_der
                    'Muro_hijo.ramas_izq = Muro_maestro.ramas_izq
                    'Muro_hijo.ramas_centro = Muro_maestro.ramas_centro
                    'Muro_hijo.Ref_htal = Muro_maestro.Ref_htal
                    'Muro_hijo.sep_htal = Muro_maestro.sep_htal
                    'Muro_hijo.As_Htal_Total = Muro_maestro.As_htal
                    'Muro_hijo.isMuroMaestro = False
                    'Muro_hijo.MuroSimilar = Muro_maestro

                    'Indice = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Muro_hijo.Pier_name)
                    'Muros_lista_2(Indice) = Muro_hijo

                    Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).MuroSimilar = Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(2).Value)

                    Dim IndiceMuro As Integer = Muros_lista_2.FindIndex(Function(x) x.Pier_name = .Cells(0).Value)

                    For j = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1
                        If refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                            refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)).MuroSimilar = refuerzo_lista.Find(Function(x) x.piername = .Cells(2).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j))

                        ElseIf refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value) Is Nothing Then

                            For m = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1
                                If refuerzo_lista.Find(Function(x) x.piername = .Cells(2).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m)) IsNot Nothing Then

                                    Dim NuevoMuroRefuerzo As New Refuerzo_muros

                                    NuevoMuroRefuerzo.MuroCreadoDespues = True
                                    NuevoMuroRefuerzo.piername = .Cells(0).Value
                                    NuevoMuroRefuerzo.pierstory = refuerzo_lista.Find(Function(x) x.piername = .Cells(2).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m)).pierstory
                                    NuevoMuroRefuerzo.bw = refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m)).bw
                                    NuevoMuroRefuerzo.rho = refuerzo_lista.Find(Function(x) x.piername = .Cells(2).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m)).rho
                                    NuevoMuroRefuerzo.as_req = refuerzo_lista.Find(Function(x) x.piername = .Cells(2).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m)).as_req

                                    NuevoMuroRefuerzo.MuroSimilar = refuerzo_lista.Find(Function(x) x.piername = .Cells(2).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m))




                                    refuerzo_lista.Add(NuevoMuroRefuerzo)

                                End If
                            Next

                        End If


                        If alzado_lista.Find(Function(x) x.pier = .Cells(0).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                            alzado_lista.Find(Function(x) x.pier = .Cells(0).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j)).MuroSimilar = alzado_lista.Find(Function(x) x.pier = .Cells(2).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j))


                        ElseIf alzado_lista.Find(Function(x) x.pier = .Cells(0).Value) Is Nothing Then

                            For m = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1
                                If alzado_lista.Find(Function(x) x.pier = .Cells(2).Value And x.story = Muros_lista_2(IndiceMuro).Stories(m)) IsNot Nothing Then

                                    Dim NuevoMuroAlzado As New alzado_muro
                                    Dim MuroSimilar As New alzado_muro
                                    MuroSimilar = alzado_lista.Find(Function(x) x.pier = .Cells(2).Value And x.story = Muros_lista_2(IndiceMuro).Stories(m))
                                    NuevoMuroAlzado.MuroCreadoDespues = True
                                    NuevoMuroAlzado.pier = .Cells(0).Value
                                    MuroSimilar.story = Muros_lista_2(IndiceMuro).Stories(m)
                                    NuevoMuroAlzado.MuroSimilar = MuroSimilar
                                    alzado_lista.Add(NuevoMuroAlzado)

                                End If
                            Next

                        End If




                    Next

                Else
                    Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).MuroSimilar = Nothing

                    Dim IndiceMuro As Integer = Muros_lista_2.FindIndex(Function(x) x.Pier_name = .Cells(0).Value)

                    For j = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1

                        If refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                            If refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)).MuroCreadoDespues = True Then

                                refuerzo_lista.Remove(refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)))
                            Else
                                refuerzo_lista.Find(Function(x) x.piername = .Cells(0).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)).MuroSimilar = Nothing

                            End If
                        End If



                        If alzado_lista.Find(Function(x) x.pier = .Cells(0).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                            If alzado_lista.Find(Function(x) x.pier = .Cells(0).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j)).MuroCreadoDespues = True Then
                                alzado_lista.Remove(alzado_lista.Find(Function(x) x.pier = .Cells(0).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j)))
                            Else
                                alzado_lista.Find(Function(x) x.pier = .Cells(0).Value And x.story = Muros_lista_2(IndiceMuro).Stories(j)).MuroSimilar = Nothing
                            End If
                        End If

                    Next




                End If


            End With


        Next



    End Sub



    Private Sub DataGrid_Muros_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGrid_Muros.CellValueChanged
        If AtivarEvento Then
            If e.ColumnIndex = 1 Then
                With DataGrid_Muros.Rows(e.RowIndex)

                    If .Cells(1).Value = True Then
                        .Cells(2).ReadOnly = True
                        .Cells(2).Value = ""
                        Dim ComboBox1 As DataGridViewComboBoxCell = .Cells(2)
                        ComboBox1.Items.Clear()


                    Else
                        .Cells(2).ReadOnly = False

                    End If


                    For i = 0 To DataGrid_Muros.Rows.Count - 1
                        Dim ListaMurosNombre As New List(Of String)
                        For j = 0 To DataGrid_Muros.Rows.Count - 1
                            If DataGrid_Muros.Rows(j).Cells(1).Value = True Then
                                ListaMurosNombre.Add(DataGrid_Muros.Rows(j).Cells(0).Value)
                            End If
                        Next

                        Dim ComboBox As DataGridViewComboBoxCell = DataGrid_Muros.Rows(i).Cells(2)
                        ComboBox.Items.Clear()
                        ComboBox.Items.AddRange(ListaMurosNombre.ToArray)

                    Next


                End With
            End If
        End If


    End Sub


    Private Sub DataGrid_Muros_CellBeginEdit(sender As Object, e As DataGridViewCellCancelEventArgs) Handles DataGrid_Muros.CellBeginEdit
        If AtivarEvento Then
            If e.ColumnIndex = 1 Then
                With DataGrid_Muros.Rows(e.RowIndex)
                    If .Cells(1).Value = True Then
                        .Cells(2).Value = ""
                    End If
                End With
            End If
        End If

    End Sub


    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

        ConfirmarMaestrosSimilares(DataGrid_Muros)
        ' AsignarSimilitudDeMuros(DataGrid_Muros)

        Me.Close()
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

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
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


