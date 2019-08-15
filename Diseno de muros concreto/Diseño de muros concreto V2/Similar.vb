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
                                    NuevoMuroRefuerzo.MuroSimilar = refuerzo_lista.Find(Function(x) x.piername = .Cells(2).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m))
                                    NuevoMuroRefuerzo.pierstory = refuerzo_lista.Find(Function(x) x.piername = .Cells(2).Value And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m)).pierstory
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


    Sub AsignarSimilitudDeMuros(DataGrid As DataGridView)


























        For i = 0 To DataGrid.Rows.Count - 1
            With DataGrid.Rows(i)
                If .Cells(2).Value <> "" Then
                    Dim NombreMuroMaestro As String = .Cells(2).Value : Dim NombreMuroSimilar As String = .Cells(0).Value


                    Dim MuroMaestro As Muros_Consolidados
                    Dim MuroSimilarIndice As Integer
                    MuroSimilarIndice = Muros_lista_2.FindIndex(Function(x1) x1.Pier_name = NombreMuroSimilar)
                    MuroMaestro = Muros_lista_2.Find(Function(x1) x1.Pier_name = NombreMuroMaestro)
                    Dim MuroMaestroIndice = Muros_lista_2.FindIndex(Function(x1) x1.Pier_name = NombreMuroMaestro)


                    Muros_lista_2(MuroSimilarIndice).Pier_name = NombreMuroSimilar
                    Muros_lista_2(MuroSimilarIndice).lw = MuroMaestro.lw
                    Muros_lista_2(MuroSimilarIndice).Malla = MuroMaestro.Malla
                    Muros_lista_2(MuroSimilarIndice).Lebe_Izq = MuroMaestro.Lebe_Izq
                    Muros_lista_2(MuroSimilarIndice).Lebe_Der = MuroMaestro.Lebe_Der
                    Muros_lista_2(MuroSimilarIndice).Lebe_Centro = MuroMaestro.Lebe_Centro
                    Muros_lista_2(MuroSimilarIndice).Est_ebe = MuroMaestro.Est_ebe
                    Muros_lista_2(MuroSimilarIndice).Sep_ebe = MuroMaestro.Sep_ebe
                    Muros_lista_2(MuroSimilarIndice).ramas_izq = MuroMaestro.ramas_izq
                    Muros_lista_2(MuroSimilarIndice).ramas_der = MuroMaestro.ramas_der
                    Muros_lista_2(MuroSimilarIndice).ramas_centro = MuroMaestro.ramas_centro
                    Muros_lista_2(MuroSimilarIndice).Zc_Der = MuroMaestro.Zc_Der
                    Muros_lista_2(MuroSimilarIndice).Est_Zc = MuroMaestro.Est_Zc
                    Muros_lista_2(MuroSimilarIndice).Sep_Zc = MuroMaestro.Sep_Zc
                    Muros_lista_2(MuroSimilarIndice).As_htal = MuroMaestro.As_htal
                    Muros_lista_2(MuroSimilarIndice).Ref_htal = MuroMaestro.Ref_htal
                    Muros_lista_2(MuroSimilarIndice).Capas_htal = MuroMaestro.Capas_htal
                    Muros_lista_2(MuroSimilarIndice).sep_htal = MuroMaestro.sep_htal
                    Muros_lista_2(MuroSimilarIndice).As_Htal_Total = MuroMaestro.As_Htal_Total




                    For j = 0 To Muros_lista_2(MuroMaestroIndice).Stories.Count - 1

                        MuroSimilarIndice = alzado_lista.FindIndex(Function(x1) x1.pier = NombreMuroSimilar And x1.story = "Story" & j + 1)


                        Dim MuroMaestro2 As New alzado_muro
                        MuroMaestro2 = alzado_lista.Find(Function(x1) x1.pier = NombreMuroMaestro And x1.story = "Story" & j + 1)
                        'If MuroMaestro2 Is Nothing Then
                        '    'MsgBox("Muro Maestro: " & NombreMuroMaestro & " no encontrado", MsgBoxStyle.Exclamation, "efe Prima Ce")
                        '    ' Exit For
                        'End If

                        If MuroSimilarIndice <> -1 And MuroMaestro2 IsNot Nothing Then
                            alzado_lista(MuroSimilarIndice).pier = NombreMuroSimilar
                            alzado_lista(MuroSimilarIndice).alzado = MuroMaestro2.alzado
                            alzado_lista(MuroSimilarIndice).Alzado_Longitud = MuroMaestro2.Alzado_Longitud
                        ElseIf MuroMaestro2 IsNot Nothing Then
                            Dim NuevoMuro As New alzado_muro
                            NuevoMuro.pier = NombreMuroSimilar
                            NuevoMuro.alzado = MuroMaestro2.alzado
                            NuevoMuro.Alzado_Longitud = MuroMaestro2.Alzado_Longitud
                            NuevoMuro.Bw = MuroMaestro2.Bw
                            NuevoMuro.story = MuroMaestro2.story
                            alzado_lista.Add(NuevoMuro)
                        End If
                    Next



                    For j = 0 To Muros_lista_2(MuroMaestroIndice).Stories.Count - 1

                        MuroSimilarIndice = refuerzo_lista.FindIndex(Function(x1) x1.piername = NombreMuroSimilar And x1.pierstory = "Story" & j + 1)


                        Dim MuroMaestro3 As New Refuerzo_muros
                        MuroMaestro3 = refuerzo_lista.Find(Function(x1) x1.piername = NombreMuroMaestro)


                        If MuroSimilarIndice <> -1 And MuroMaestro3 IsNot Nothing Then
                            refuerzo_lista(MuroSimilarIndice).piername = NombreMuroSimilar
                            refuerzo_lista(MuroSimilarIndice).Ebe_Izq = MuroMaestro3.Ebe_Izq
                            refuerzo_lista(MuroSimilarIndice).Ebe_Der = MuroMaestro3.Ebe_Der
                            refuerzo_lista(MuroSimilarIndice).as_req = MuroMaestro3.as_req
                            refuerzo_lista(MuroSimilarIndice).cantidad = MuroMaestro3.cantidad
                            refuerzo_lista(MuroSimilarIndice).diametro = MuroMaestro3.diametro
                            refuerzo_lista(MuroSimilarIndice).porcentaje = MuroMaestro3.porcentaje
                            refuerzo_lista(MuroSimilarIndice).rho = MuroMaestro3.rho
                            refuerzo_lista(MuroSimilarIndice).Zc_Der = MuroMaestro3.Zc_Der
                            refuerzo_lista(MuroSimilarIndice).Zc_Izq = MuroMaestro3.Zc_Izq
                            refuerzo_lista(MuroSimilarIndice).total = MuroMaestro3.total
                        ElseIf MuroMaestro3 IsNot Nothing Then
                            Dim NuevoMuro As New Refuerzo_muros
                            NuevoMuro.piername = NombreMuroSimilar
                            NuevoMuro.Ebe_Izq = MuroMaestro3.Ebe_Izq
                            NuevoMuro.pierstory = MuroMaestro3.pierstory
                            NuevoMuro.Ebe_Der = MuroMaestro3.Ebe_Der
                            NuevoMuro.as_req = MuroMaestro3.as_req
                            NuevoMuro.cantidad = MuroMaestro3.cantidad
                            NuevoMuro.diametro = MuroMaestro3.diametro
                            NuevoMuro.porcentaje = MuroMaestro3.porcentaje
                            NuevoMuro.rho = MuroMaestro3.rho
                            NuevoMuro.Zc_Der = MuroMaestro3.Zc_Der
                            NuevoMuro.Zc_Izq = MuroMaestro3.Zc_Izq
                            NuevoMuro.total = MuroMaestro3.total
                            refuerzo_lista.Add(NuevoMuro)
                        End If



                    Next




                End If


            End With


        Next


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


