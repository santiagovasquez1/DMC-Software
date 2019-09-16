Imports System.Runtime.InteropServices

Public Class Form22

    <DllImport("user32.DLL", EntryPoint:="ReleaseCapture")>
    Private Shared Sub ReleaseCapture()
    End Sub

    <DllImport("user32.DLL", EntryPoint:="SendMessage")>
    Private Shared Sub SendMessage()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DoubleBuffered = True
        Dim prueba As String
        Dim Muros_Distintos As New List(Of String)
        Dim Lista_i As New Listas_serializadas

        prueba = Ruta_1
        Cargar_areas_refuerzo()

        Dim style, style2 As New DataGridViewCellStyle

        style.Font = New Font("Calibri", 8.5, FontStyle.Bold)
        style2.Font = New Font("Calibri", 8)
        style.BackColor = Color.Blue

        ' Data_muros.AutoSize = True
        Data_muros.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Data_muros.ColumnHeadersDefaultCellStyle.Font = style.Font
        Data_muros.EnableHeadersVisualStyles = False

        Data_muros.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Data_muros.DefaultCellStyle.Font = style2.Font

        If Muros_lista_2 Is Nothing Then
            Muros_lista_2 = New List(Of Muros_Consolidados)
            Serializador.Deserializar(Ruta_1, Lista_i)
            cb_cuantiavol.Enabled = True
        End If

        Muros_Distintos = Muros_lista_2.Select(Function(X) X.Pier_name).Distinct().OrderBy(Function(x2) x2).ToList()

        If Muros_lista_2.Count > 0 Then
            LMuros.Enabled = True
            LMuros.Items.Clear()
            LMuros.Items.AddRange(Muros_Distintos.ToArray)
            LMuros.Text = LMuros.Items(0)
        End If

        ''Crear lista vacia de alzado y refuerzos
        Muros_lista_2 = Muros_lista_2.OrderBy(Function(x) x.Pier_name).ToList()
        Listas_Vacias()

    End Sub

    Private Sub PasteClipboard(datos As DataGridView)

        Dim s As String
        Try
            s = Clipboard.GetText()
            Dim i, ii As Integer
            Dim tArr() As String = s.Split(ControlChars.NewLine)
            Dim arT() As String
            Dim cc, iRow, iCol As Integer

            iRow = datos.SelectedCells(0).RowIndex
            iCol = datos.SelectedCells(0).ColumnIndex

            For i = 0 To tArr.Length - 1
                If tArr(i) <> "" Then
                    arT = tArr(i).Split(vbTab)
                    cc = iCol
                    For ii = 0 To arT.Length - 1
                        If cc > datos.ColumnCount - 1 Then Exit For
                        If iRow > datos.Rows.Count - 1 Then Exit Sub
                        With datos.Item(cc, iRow)
                            .Value = arT(ii).TrimStart
                        End With
                        cc = cc + 1
                    Next
                    iRow = iRow + 1
                End If
            Next
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CopyToClipboard(datos As DataGridView)
        Dim dataObj As DataObject = datos.GetClipboardContent
        If Not IsNothing(dataObj) Then
            Clipboard.SetDataObject(dataObj)
        End If
    End Sub

    Private Sub cbCargar_Click(sender As Object, e As EventArgs)

        Dim muros_distintos As IEnumerable(Of String) = piername.Distinct()
        Dim j As Integer = 0

        For Each elemento As String In muros_distintos
            ReDim Preserve list_muros(j)
            list_muros(j) = elemento
            j = j + 1
        Next

        LMuros.Enabled = True
        LMuros.Items.AddRange(list_muros)
    End Sub

    Private Sub LMuros_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LMuros.SelectedIndexChanged

        Dim Indice As Integer
        Data_muros.Rows.Clear()
        Data_muros.Invalidate()
        Dim Muro As New Muros_Consolidados
        Dim Rho_l, Rho_t As New List(Of Double)
        Dim Fc As New List(Of Single)
        Dim Pier As String
        Dim Hw, Bw As New List(Of Single) : Dim Stories As New List(Of String) : Dim As_Long As New List(Of Double)

        Indice = Muros_lista_2.FindIndex(Function(x) x.Pier_name = LMuros.Text)
        Muro = Muros_lista_2(Indice) : Pier = Muros_lista_2(Indice).Pier_name
        Rho_l = Muros_lista_2(Indice).Rho_l : Rho_t = Muros_lista_2(Indice).Rho_T : Fc = Muros_lista_2(Indice).fc : Hw = Muros_lista_2(Indice).Hw
        Stories = Muros_lista_2(Indice).Stories : Bw = Muros_lista_2(Indice).Bw : As_Long = Muros_lista_2(Indice).As_Long

        If Muros_lista_2(Indice).MuroSimilar IsNot Nothing Then
            Muro = Muros_lista_2(Indice).MuroSimilar
        End If

        Data_muros.Rows().Add(Muros_lista_2(Indice).Stories.Count)

        For i = 0 To Muros_lista_2(Indice).Stories.Count - 1
            Data_muros.Rows(i).Cells(0).Value = Pier
            Data_muros.Rows(i).Cells(1).Value = Stories(i)
            Data_muros.Rows(i).Cells(2).Value = Format(Bw(i), "##,0.00")
            Data_muros.Rows(i).Cells(3).Value = Format(Muro.lw(i), "##,0.00")
            Data_muros.Rows(i).Cells(4).Value = Format(Hw(i), "##,0.00")
            Data_muros.Rows(i).Cells(5).Value = Fc(i)
            Data_muros.Rows(i).Cells(6).Value = Rho_l(i)
            Data_muros.Rows(i).Cells(7).Value = Rho_t(i)
            Data_muros.Rows(i).Cells(8).Value = Muro.Malla(i)

            If Muro.Malla(i) = "Sin Malla" And Rho_l(i) >= 0.01 OrElse Bw(i) >= 25 Then
                Data_muros.Rows(i).Cells(8).ReadOnly = True
            End If

            Data_muros.Rows(i).Cells(9).Value = Format(As_Long(i), "##,0.00")
            Data_muros.Rows(i).Cells(10).Value = Format(Muro.Lebe_Izq(i), "##,0.00")
            Data_muros.Rows(i).Cells(11).Value = Format(Muro.Lebe_Der(i), "##,0.00")
            Data_muros.Rows(i).Cells(12).Value = Format(Muro.Lebe_Centro(i), "##,0.00")
            Data_muros.Rows(i).Cells(13).Value = Format(Muro.Est_ebe(i), "##,0")
            Data_muros.Rows(i).Cells(14).Value = Format(Muro.Sep_ebe(i), "##,0.00")
            Data_muros.Rows(i).Cells(15).Value = Format(Muro.ramas_izq(i), "##,0")
            Data_muros.Rows(i).Cells(16).Value = Format(Muro.ramas_der(i), "##,0")
            Data_muros.Rows(i).Cells(17).Value = Format(Muro.ramas_centro(i), "##,0")
            Data_muros.Rows(i).Cells(18).Value = Format(Muro.Zc_Izq(i), "##,0.00")
            Data_muros.Rows(i).Cells(19).Value = Format(Muro.Zc_Der(i), "##,0.00")
            Data_muros.Rows(i).Cells(20).Value = Format(Muro.Est_Zc(i), "##,0")
            Data_muros.Rows(i).Cells(21).Value = Format(Muro.Sep_Zc(i), "##,0.00")
            Data_muros.Rows(i).Cells(22).Value = Format(Muro.As_htal(i), "##,0.00")

            If Muro.As_htal(i) <> 0 Then

                Data_muros.Rows(i).Cells(23).ReadOnly = False
                Data_muros.Rows(i).Cells(24).ReadOnly = False
                Data_muros.Rows(i).Cells(25).ReadOnly = False
            Else
                Data_muros.Rows(i).Cells(23).ReadOnly = True
                Data_muros.Rows(i).Cells(24).ReadOnly = True
                Data_muros.Rows(i).Cells(25).ReadOnly = True
            End If

            If Muro.Ref_htal(i) <> "" Then
                Data_muros.Rows(i).Cells(23).Value = Muro.Ref_htal(i)
            Else
                Data_muros.Rows(i).Cells(23).Value = ""
            End If

            If Muro.Capas_htal(i) <> 0 Then
                Data_muros.Rows(i).Cells(24).Value = Muro.Capas_htal(i).ToString
            Else
                Data_muros.Rows(i).Cells(24).Value = ""
            End If
            Data_muros.Rows(i).Cells(25).Value = Format(Muro.sep_htal(i), "##,0.00")
            Data_muros.Rows(i).Cells(26).Value = Format(Muro.As_Htal_Total(i), "##,0.00")
        Next

        '  Data_muros.AutoSize = True
        Data_muros.Update()

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs)

        Try
            LMuros.Enabled = True
            LMuros.Items.AddRange(list_muros)
        Catch ex As Exception

        End Try

        cb_cuantiavol.Enabled = True

    End Sub

    Private Sub Ts_guardar_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub cargar_proyecto_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub cb_Validar_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub cb_next_Click(sender As Object, e As EventArgs)
        f_alzado.Show()
        f_alzado.Text = "Diseño Muros de Concreto :" & Ruta_archivo
    End Sub

    Private Sub cb_cuantiavol_Click(sender As Object, e As EventArgs) Handles cb_cuantiavol.Click

        Calculo_cuantia_volumetrica()
        For i = 0 To Data_muros.Rows.Count - 1

            Validar_info1(LMuros.Text, i, Data_muros)

        Next

    End Sub

    Private Sub Calculo_cuantia_volumetrica()
        Dim Diametro As Integer
        Dim sep As Single
        Dim Ash() As Single
        Dim cantx, canty As Integer
        Dim bw, lebe As Single
        Dim fc As Single
        Dim fy As Single = 4220 'Kgf/cm²
        Dim r As Single = 2.5 'cm
        Dim texto As String = ""
        Dim Espesor_minimo As Single

        Espesor_minimo = 0
        For i = 0 To Data_muros.Rows.Count - 1

            If Data_muros.Rows(i).Cells(13).Value <> 0 Then

                Diametro = Data_muros.Rows(i).Cells(13).Value
                sep = Data_muros.Rows(i).Cells(14).Value
                bw = Data_muros.Rows(i).Cells(2).Value - 2 * r
                fc = Data_muros.Rows(i).Cells(5).Value

                'If CheckBox1.Checked = True Then
                '    Espesor_minimo = e_EBE.Text
                '    If bw <= Espesor_minimo Then
                '        bw = Espesor_minimo
                '    End If
                'End If

                'Calculo Cuantia Volumetrica EBE Izq
                If Data_muros.Rows(i).Cells(10).Value > 0 Then
                    lebe = Data_muros.Rows(i).Cells(10).Value - r
                    Ash = Cuantia_volumetrica(sep, fc, fy, lebe, bw, Capacidad)

                    cantx = Math.Ceiling(Ash(0) / (areas_refuerzo(Diametro)))
                    canty = Math.Ceiling(Ash(1) / (areas_refuerzo(Diametro)))
                    If canty > 2 Then
                        Data_muros.Rows(i).Cells(15).Value = cantx & " 🔺"
                    Else
                        Data_muros.Rows(i).Cells(15).Value = cantx
                    End If

                    texto = "AshX =" & Format(Ash(0) / (cantx), "##,0.00") & vbNewLine & "Ramas Y =" & canty

                        Data_muros.Rows(i).Cells(15).ToolTipText = texto
                    Else
                        Data_muros.Rows(i).Cells(15).Value = 0
                End If

                'Calculo Cuantia Volumetrica EBE der
                If Data_muros.Rows(i).Cells(11).Value > 0 Then
                    lebe = Data_muros.Rows(i).Cells(11).Value - r
                    Ash = Cuantia_volumetrica(sep, fc, fy, lebe, bw, Capacidad)

                    cantx = Math.Ceiling(Ash(0) / (areas_refuerzo(Diametro)))
                    canty = Math.Ceiling(Ash(1) / (areas_refuerzo(Diametro)))
                    If canty > 2 Then
                        Data_muros.Rows(i).Cells(16).Value = cantx & " 🔺"
                    Else
                        Data_muros.Rows(i).Cells(16).Value = cantx
                    End If

                    texto = "AshX =" & Format(Ash(0) / (cantx), "##,0.00") & vbNewLine & "Ramas Y =" & canty
                    Data_muros.Rows(i).Cells(16).ToolTipText = texto
                Else
                    Data_muros.Rows(i).Cells(16).Value = 0
                End If

                'Calculo Cuantia Volumetrica EBE centro
                If Data_muros.Rows(i).Cells(12).Value > 0 Then
                    lebe = Data_muros.Rows(i).Cells(12).Value
                    Ash = Cuantia_volumetrica(sep, fc, fy, lebe, bw, Capacidad)

                    cantx = Math.Ceiling(Ash(0) / (areas_refuerzo(Diametro)))
                    canty = Math.Ceiling(Ash(1) / (areas_refuerzo(Diametro)))

                    Data_muros.Rows(i).Cells(17).Value = cantx
                    texto = "AshX =" & Format(Ash(0) / (cantx), "##,0.00") & vbNewLine & "Ramas Y =" & canty
                    Data_muros.Rows(i).Cells(17).ToolTipText = texto
                Else
                    Data_muros.Rows(i).Cells(17).Value = 0
                End If
            End If

        Next
    End Sub

    Private Sub Calculo_cuantia_volumetrica2(ByVal indice As Integer)
        Dim Diametro As Integer
        Dim sep As Single
        Dim Ash() As Single
        Dim cantx, canty As Integer
        Dim bw, lebe As Single
        Dim fc As Single
        Dim fy As Single = 4220 'Kgf/cm²
        Dim r As Single = 2.5 'cm
        Dim texto As String = ""
        Dim Espesor_minimo As Single

        Espesor_minimo = 0
        If Data_muros.Rows(indice).Cells(13).Value <> 0 Then
            Diametro = Data_muros.Rows(indice).Cells(13).Value
            sep = Data_muros.Rows(indice).Cells(14).Value
            bw = Data_muros.Rows(indice).Cells(2).Value - 2 * r
            fc = Data_muros.Rows(indice).Cells(5).Value

            'If CheckBox1.Checked = True Then
            '    Espesor_minimo = e_EBE.Text
            '    If bw <= Espesor_minimo Then
            '        bw = Espesor_minimo - 2 * r
            '    End If
            'End If

            'Calculo Cuantia Volumetrica EBE Izq
            If Data_muros.Rows(indice).Cells(10).Value > 0 Then

                lebe = Data_muros.Rows(indice).Cells(10).Value - r

                ''''''Revisar
                Ash = Cuantia_volumetrica(sep, fc, fy, lebe, bw, Capacidad)

                cantx = Val(Data_muros.Rows(indice).Cells(15).Value)
                canty = Math.Ceiling(Ash(1) / (areas_refuerzo(Diametro)))

                If Double.IsInfinity(Ash(0) / (cantx)) = False And Double.IsNaN(Ash(0) / (cantx)) = False Then
                    texto = "AshX =" & Format(Ash(0) / (cantx), "##,0.00") & vbNewLine & "Ramas Y =" & canty
                    texto = "AshX =" & Format(0, "##,0.00") & vbNewLine & "Ashy =" & Format(0, "##,0.00")
                End If
                Data_muros.Rows(indice).Cells(15).ToolTipText = texto
            End If

            'Calculo Cuantia Volumetrica EBE der
            If Data_muros.Rows(indice).Cells(11).Value > 0 Then
                lebe = Data_muros.Rows(indice).Cells(11).Value - r
                Ash = Cuantia_volumetrica(sep, fc, fy, lebe, bw, Capacidad)

                cantx = Val(Data_muros.Rows(indice).Cells(16).Value)
                canty = Math.Ceiling(Ash(1) / (areas_refuerzo(Diametro)))

                If Double.IsInfinity(Ash(0) / (cantx)) = False And Double.IsNaN(Ash(0) / (cantx)) = False Then
                    texto = "AshX =" & Format(Ash(0) / (cantx), "##,0.00") & vbNewLine & "Ramas Y =" & canty
                Else
                    texto = "AshX =" & Format(0, "##,0.00") & vbNewLine & "Ashy =" & Format(0, "##,0.00")
                End If
                Data_muros.Rows(indice).Cells(16).ToolTipText = texto
            End If

            'Calculo Cuantia Volumetrica EBE centro

            If Data_muros.Rows(indice).Cells(12).Value > 0 Then

                lebe = Data_muros.Rows(indice).Cells(12).Value - r
                Ash = Cuantia_volumetrica(sep, fc, fy, lebe, bw, Capacidad)

                cantx = Data_muros.Rows(indice).Cells(17).Value
                canty = Math.Ceiling(Ash(1) / (areas_refuerzo(Diametro)))
                If Double.IsInfinity(Ash(0) / (cantx)) = False And Double.IsNaN(Ash(0) / (cantx)) = False Then
                    texto = "AshX =" & Format(Ash(0) / (cantx), "##,0.00") & vbNewLine & "Ramas Y =" & canty
                Else
                    texto = "AshX =" & Format(0, "##,0.00") & vbNewLine & "Ashy =" & Format(0, "##,0.00")
                End If
                Data_muros.Rows(indice).Cells(17).ToolTipText = texto
            End If
        End If
    End Sub

    Private Sub Data_muros_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles Data_muros.CellEndEdit

        Dim nombre_Muro As String
        Dim malla As String
        Dim lw_eff, bw, lw, pt As Double
        Dim As_long, rho_l, As_def As Double
        Dim indice As Integer
        Dim l_z, l_d, l_c As Double
        Dim As_htal As Double
        Dim As_htal_total As Double
        Dim Diametro As String
        Dim capas As Integer
        Dim sep As Single

        nombre_Muro = LMuros.Text
        indice = Data_muros.CurrentCell.RowIndex
        bw = Data_muros.Rows(indice).Cells(2).Value
        lw = Data_muros.Rows(indice).Cells(3).Value
        malla = Data_muros.Rows(indice).Cells(8).Value
        rho_l = Data_muros.Rows(indice).Cells(6).Value

        If Data_muros.Rows(indice).Cells(10).Value <> 0 And malla <> "Sin Malla" Then
            l_z = Data_muros.Rows(indice).Cells(10).Value
        ElseIf malla <> "Sin Malla" Then
            l_z = Data_muros.Rows(indice).Cells(18).Value
        ElseIf malla = "Sin Malla" Then
            l_z = 0
        End If

        If Data_muros.Rows(indice).Cells(11).Value <> 0 And malla <> "Sin Malla" Then
            l_d = Data_muros.Rows(indice).Cells(11).Value
        ElseIf malla <> "Sin Malla" Then
            l_d = Data_muros.Rows(indice).Cells(19).Value
        ElseIf malla = "Sin Malla" Then
            l_d = 0
        End If

        If malla <> "Sin Malla" Then
            l_c = Data_muros.Rows(indice).Cells(12).Value
        Else
            l_c = 0
        End If
        lw_eff = (lw - l_z - l_d - l_c) / 100
        As_long = lw * bw * rho_l
        As_def = As_long - lw_eff * As_malla(malla)

        Data_muros.Rows(indice).Cells(9).Value = Math.Round(As_def, 2)

        If Data_muros.Rows(indice).Cells(9).Value < 0 Then
            Data_muros.Rows(indice).Cells(9).Value = 0
        End If

        'calculo As Horizontal Adicional
        pt = Data_muros.Rows(indice).Cells(7).Value
        Diametro = Data_muros.Rows(indice).Cells(23).Value

        Try
            capas = Data_muros.Rows(indice).Cells(24).Value
            sep = Data_muros.Rows(indice).Cells(25).Value
        Catch ex As Exception
            capas = 0
            sep = 0
        End Try

        As_htal = (pt * bw * 100) - As_malla(malla)

        If As_htal > 0 And rho_l < 0.01 Then
            Data_muros.Rows(indice).Cells(22).Value = Format(As_htal, "##0.00")
            Data_muros.Rows(indice).Cells(23).ReadOnly = False
            Data_muros.Rows(indice).Cells(24).ReadOnly = False
            Data_muros.Rows(indice).Cells(25).ReadOnly = False

            If InStr(Diametro, "mm") Then
                Dim pos As Integer = InStr(Diametro, "mm")
                Try
                    As_htal_total = (areas_refuerzo(Mid(Diametro, 1, pos - 1)) / (sep / 100)) * capas
                Catch ex As Exception
                    As_htal_total = 0
                End Try
            Else
                Try
                    As_htal_total = (areas_refuerzo(Diametro) / (sep / 100)) * capas
                Catch ex As Exception
                    As_htal_total = 0
                End Try

            End If

            If Double.IsInfinity(As_htal_total) Or Double.IsNaN(As_htal_total) Or As_htal_total = 0 Then
                Data_muros.Rows(indice).Cells(26).Value = 0
            Else
                Data_muros.Rows(indice).Cells(26).Value = Format(As_htal_total, "##0.00")
            End If

        ElseIf rho_l >= 0.01 Then
            Data_muros.Rows(indice).Cells(22).Value = Format(0, "##0.00")
            Data_muros.Rows(indice).Cells(23).Value = ""
            Data_muros.Rows(indice).Cells(24).Value = ""
            Data_muros.Rows(indice).Cells(25).Value = 0
            Data_muros.Rows(indice).Cells(26).Value = 0

            Data_muros.Rows(indice).Cells(23).ReadOnly = True
            Data_muros.Rows(indice).Cells(24).ReadOnly = True
            Data_muros.Rows(indice).Cells(25).ReadOnly = True

        ElseIf As_htal <= 0 Then
            As_htal = 0
            Data_muros.Rows(indice).Cells(22).Value = Format(As_htal, "##0.00")
        End If

        Calculo_cuantia_volumetrica2(indice)
        Validar_info1(nombre_Muro, indice, Data_muros)

    End Sub

    Private Sub CopiarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopiarToolStripMenuItem.Click
        CopyToClipboard(data_grid)
    End Sub

    Private Sub PegarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PegarToolStripMenuItem.Click
        PasteClipboard(data_grid)
    End Sub

    Private Sub CortarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CortarToolStripMenuItem.Click
        CopyToClipboard(data_grid)
        For counter As Integer = 0 To data_grid.SelectedCells.Count - 1
            data_grid.SelectedCells(counter).Value = String.Empty
        Next
    End Sub

    Private Sub Data_muros_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Data_muros.CellMouseClick

        Dim celda As DataGridViewCell
        Dim validar As Boolean = True

        data_grid = Data_muros
        data_grid.ContextMenuStrip = ContextMenuStrip1
        ContextMenuStrip1.Enabled = False

        If data_grid.SelectedCells.Count > 0 Then

            For Each celda In data_grid.SelectedCells
                If celda.ReadOnly = True Then
                    validar = False
                End If
            Next

            If validar = False Then
                ContextMenuStrip1.Enabled = False
            Else
                ContextMenuStrip1.Enabled = True
                CortarToolStripMenuItem.Enabled = False
            End If

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs)

        Dim Listas_serializar As New Listas_serializadas

        Listas_serializar.Lista_Muros = Muros_lista_2
        Listas_serializar.Lista_Alzados = alzado_lista
        Listas_serializar.lista_refuerzo = refuerzo_lista

        Serializador.Serializar(Ruta_archivo, Listas_serializar)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

        Dim Ruta As String = ""
        Dim Lista_serializada As New Listas_serializadas

        Serializador.Deserializar(Ruta, Lista_serializada)

        Muros_lista_2 = Lista_serializada.Lista_Muros
        alzado_lista = Lista_serializada.Lista_Alzados
        refuerzo_lista = Lista_serializada.lista_refuerzo

    End Sub

End Class