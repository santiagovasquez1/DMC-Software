Public Class f_alzado

    Public Class refuerzo_piso
        Public cantidad As Integer
        Public diametro As Integer
    End Class

    Public Class refuerzo_comparar
        Public cantidad As New List(Of Integer)
        Public diametro As New List(Of Integer)
    End Class

    Public lista_refpiso As New List(Of refuerzo_piso)

    Public Function color_lapiz(ByVal pos As Integer, valor As String) As Pen
        Dim diametro As Integer
        Dim lapiz As New Pen(Color.Black)
        If pos <> 0 Then
            diametro = Val(Mid(valor, pos + 1))
            Try
                lapiz.Color = colores_refuerzo(diametro)
            Catch ex As Exception
                lapiz.Color = Color.Black
            End Try

            lapiz.Width = 2
        End If

        Return lapiz
    End Function

    Public ActivarTablas As Boolean

    Public Sub f_alzado_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim Tooltip As New ToolTip
        Dim Lista_i As New Listas_serializadas

        Tooltip.SetToolTip(Button3, "Agregar columna de alzado (Ctrl + Q)")
        Tooltip.SetToolTip(button4, "Dibujar Alzado de Muros en AutoCAD (Ctrl + W)")

        Dim Muros_Distintos As New List(Of String)
        Cargar_areas_refuerzo()

        If Muros_lista_2 Is Nothing Then
            Muros_lista_2 = New List(Of Muros_Consolidados)
            Serializador.Deserializar(Ruta_1, Lista_i)
            ActivarTablas = False
        End If

        Muros_Distintos = Muros_lista_2.Select(Function(X) X.Pier_name).Distinct().OrderBy(Function(x2) x2).ToList()

        If Muros_lista_2.Count > 0 Then
            LMuros.Enabled = True
            LMuros.Items.AddRange(Muros_Distintos.ToArray)
            LMuros.Text = Muros_lista_2(0).Pier_name
        End If

        Muros_lista_2 = Muros_lista_2.OrderBy(Function(x) x.Pier_name).ToList()
        Listas_Vacias()

        Me.AutoScroll = False
        Me.DoubleBuffered = True
        button5.Enabled = True
    End Sub

    Public Sub LMuros_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LMuros.SelectedIndexChanged

        If ActivarTablas = True Then
            CargarTablas(LMuros.Text)
        End If
        ActivarTablas = True

        For i = 0 To Data_ayuda.Rows.Count - 1
            descontar_ref(i)
        Next

        pb_Alzado.CreateGraphics.Clear(Color.White)
        pb_Alzado.Invalidate()

    End Sub

    Sub CargarTablas(ByVal Nombre_Muro As String)

        Data_Alzado.Rows.Clear()
        Data_Alzado.ColumnCount = 2

        Dim diametros As New List(Of Integer)

        Data_info.Rows.Clear()
        Tabla_info_gnl(Nombre_Muro, Data_info)
        data_info_f3 = Data_info

        Data_ayuda.Rows.Clear()
        Dim Muroi As Muros_Consolidados
        Muroi = Muros_lista_2.Find(Function(x1) x1.Pier_name = Nombre_Muro)
        For i = 0 To Muroi.Stories.Count - 1
            Tabla_Data_Ayuda(Nombre_Muro, Data_ayuda, i, True)
        Next
        Alzado_info(Nombre_Muro, Data_Alzado)

    End Sub

    Public Sub descontar_ref(ByVal indice)
        Dim vector_texto() As String
        Dim menc As Integer = 0
        Dim menc1 As Integer = 0
        Dim pos1 As Integer
        Dim refuerzo_comparari As New refuerzo_comparar
        Dim lista_prueba As New List(Of refuerzo_piso)
        Dim suma_cantidad As Integer
        Dim lista_acumulado As New List(Of refuerzo_piso)

        lista_refpiso.Clear()

        For i = 2 To Data_Alzado.Columns.Count - 1
            Dim refuerzo As New refuerzo_piso
            If InStr(Data_Alzado.Rows(indice).Cells(i).Value, "#") <> 0 Then
                vector_texto = Split(Data_Alzado.Rows(indice).Cells(i).Value, "#")
                pos1 = InStr(vector_texto(1), "T")
                If vector_texto(0) <> "" Then
                    refuerzo.cantidad = vector_texto(0)
                End If

                If pos1 <> 0 Then
                    Try
                        refuerzo.diametro = Mid(vector_texto(1), 1, pos1 - 1)
                    Catch ex As Exception
                        refuerzo.diametro = 0
                    End Try
                Else
                    Try
                        refuerzo.diametro = Mid(vector_texto(1), pos1 + 1)
                    Catch ex As Exception
                        refuerzo.diametro = 0
                    End Try

                End If
            ElseIf Data_Alzado.Rows(indice).Cells(i).Value <> Nothing Then
                Data_Alzado.Rows(indice).Cells(i).Value = "Error"
            End If
            lista_refpiso.Add(refuerzo)
        Next

        Dim refuerzos_distindos = From c In lista_refpiso Select New With {Key c.diametro} Distinct.ToList

        'Contador de refuerzo acumulado
        For i = 0 To refuerzos_distindos.Count - 1
            suma_cantidad = 0
            For Each elemento As refuerzo_piso In lista_refpiso
                If elemento.diametro = refuerzos_distindos(i).diametro Then
                    suma_cantidad = suma_cantidad + elemento.cantidad
                End If
            Next
            Dim ref_acumulado As New refuerzo_piso
            ref_acumulado.diametro = refuerzos_distindos(i).diametro
            ref_acumulado.cantidad = suma_cantidad
            lista_acumulado.Add(ref_acumulado)
        Next

        If Data_ayuda.Visible = True Then
            For i = 5 To Data_ayuda.ColumnCount - 1
                menc = 0
                For Each elemento As refuerzo_piso In lista_acumulado
                    If elemento.diametro = Val(Mid(Data_ayuda.Columns(i).HeaderText, 2)) Then
                        Data_ayuda.Rows(indice).Cells(i).Value = Data_info.Rows(indice).Cells(i + 4).Value - elemento.cantidad
                        If Data_ayuda.Rows(indice).Cells(i).Value = 0 And Data_info.Rows(indice).Cells(i + 4).Value <> 0 Then
                            Data_ayuda.Rows(indice).Cells(i).Style.BackColor = Color.Green
                            Data_ayuda.Rows(indice).Cells(i).Style.ForeColor = Color.Black
                        ElseIf Data_ayuda.Rows(indice).Cells(i).Value < 0 Then
                            Data_ayuda.Rows(indice).Cells(i).Style.BackColor = Color.LightSalmon
                            Data_ayuda.Rows(indice).Cells(i).Style.ForeColor = Color.Black
                        ElseIf Data_ayuda.Rows(indice).Cells(i).Value <> 0 And Data_info.Rows(indice).Cells(i + 4).Value <> 0 Then
                            Data_ayuda.Rows(indice).Cells(i).Style.BackColor = Color.White
                            Data_ayuda.Rows(indice).Cells(i).Style.ForeColor = Color.Black
                        End If
                    End If
                Next

            Next
        End If
    End Sub

    Public Class objeto_alzado
        Public diametro As Integer
        Public traslapo As String
    End Class

    Public Sub Pb_Alzado_Paint(sender As Object, e As PaintEventArgs) Handles pb_Alzado.Paint

        Dim alzado As Graphics = e.Graphics
        Dim lapiz1 As New Pen(Color.LightGray)
        Dim lapiz2 As New Pen(Color.Black)
        Dim texto() As String
        Dim fuente As New Font("Calibri", 8, FontStyle.Bold)
        Dim x2, y2, x3 As Single
        Dim y1, k As Single
        Dim x1 As Single
        Dim dato_alzado As New List(Of String)
        Dim menc As Integer
        Dim pos As Integer
        Dim texto2 As String
        Dim constante As Single

        With lapiz1
            .Width = 1.0
            .StartCap = Drawing2D.LineCap.Triangle
            .Alignment = Drawing2D.PenAlignment.Center
        End With

        With lapiz2
            .Width = 2
            .LineJoin = Drawing2D.LineJoin.Round
        End With

        x1 = 0
        x2 = 50
        x3 = x2
        Dim EscalaX As Single = (pb_Alzado.Height / (Data_Alzado.Rows.Count + 1)) / 25
        y1 = pb_Alzado.Height / (Data_Alzado.Rows.Count + 1)
        k = 0

        ''Crear divisiones de pisos
        If Data_Alzado.Rows.Count <> 0 Then
            alzado.DrawLine(lapiz1, x1, y1 - 20 * EscalaX, 5000, y1 - 20 * EscalaX)
            For i = 0 To Data_Alzado.Rows.Count - 1
                ReDim Preserve texto(k)
                alzado.DrawLine(lapiz1, x1, y1, 5000, y1)
                texto(k) = Data_Alzado.Rows(i).Cells(1).Value
                alzado.DrawString(texto(k), fuente, Brushes.Black, 0, y1 - 10 * EscalaX)
                y1 = y1 + 25 * EscalaX
                k = k + 1
            Next
            alzado.DrawLine(lapiz1, x1, y1 - 10 * EscalaX, 5000, y1 - 10 * EscalaX)
            alzado.DrawString("Fund", fuente, Brushes.Black, 0, y1 - 20 * EscalaX)

        End If
        constante = y1 - 10 * EscalaX
        y1 = y1 - 25 * EscalaX

        'Crear alzados
        For i = 2 To Data_Alzado.Columns.Count - 1
            dato_alzado.Clear()
            y2 = y1
            For j = 0 To Data_Alzado.Rows.Count - 1
                dato_alzado.Add(Data_Alzado.Rows(j).Cells(i).Value)
            Next

            If dato_alzado(dato_alzado.Count - 1) <> Nothing Then
                pos = InStr(dato_alzado(dato_alzado.Count - 1), "#")
                lapiz2 = color_lapiz(pos, dato_alzado(dato_alzado.Count - 1))
                alzado.DrawLine(lapiz2, x3, y2 + 10 * EscalaX, x3, y2)
                alzado.DrawLine(lapiz2, x3, y2 + 10 * EscalaX, x3 + 10, y2 + 10 * EscalaX)
            End If
            menc = 0
            'CAMBIO 11111--------------
            'Dibujo de barras de refuerzo
            For j = dato_alzado.Count - 1 To 0 Step -1
                If j < dato_alzado.Count - 1 Then
                    If dato_alzado(j) <> Nothing And dato_alzado(j) <> "Error" Then
                        If InStr(dato_alzado(j), "T1") = 0 And InStr(dato_alzado(j), "T2") = 0 And InStr(dato_alzado(j + 1), "T1") = 0 And InStr(dato_alzado(j), "T3") = 0 Then
                            pos = InStr(dato_alzado(j), "#")
                            lapiz2 = color_lapiz(pos, dato_alzado(j))
                            alzado.DrawLine(lapiz2, x3, y2, x3, y2 - 25 * EscalaX)
                            menc = 1
                        End If

                        If InStr(dato_alzado(j), "T1") <> 0 And InStr(dato_alzado(j), "T2") = 0 And InStr(dato_alzado(j + 1), "T1") = 0 And InStr(dato_alzado(j), "T3") = 0 Then
                            pos = InStr(dato_alzado(j), "#")
                            lapiz2 = color_lapiz(pos, dato_alzado(j))
                            alzado.DrawLine(lapiz2, x3, y2, x3, y2 - 35 * EscalaX)
                            menc = 1
                        End If

                        If InStr(dato_alzado(j), "T3") <> 0 Then
                            If j < dato_alzado.Count - 1 Then
                                If dato_alzado(j + 1) = Nothing Or dato_alzado(j + 1) = "Error" Or InStr(dato_alzado(j + 1), "T3") Then
                                    pos = InStr(dato_alzado(j), "#")
                                    lapiz2 = color_lapiz(pos, dato_alzado(j))
                                    alzado.DrawLine(lapiz2, x3, y2 + 6 * EscalaX, x3, y2 - 12 * EscalaX)
                                    alzado.DrawLine(lapiz2, x3, y2 - 12 * EscalaX, x3 + 10, y2 - 12 * EscalaX)
                                    alzado.DrawLine(lapiz2, x3, y2 + 6 * EscalaX, x3 + 10, y2 + 6 * EscalaX)
                                ElseIf InStr(dato_alzado(j + 1), "T1") Then
                                    If x3 = x2 Then
                                        x3 = x2 + 5
                                    Else
                                        x3 = x2
                                    End If

                                    pos = InStr(dato_alzado(j), "#")
                                    lapiz2 = color_lapiz(pos, dato_alzado(j))
                                    alzado.DrawLine(lapiz2, x3, y2, x3, y2 - 16 * EscalaX)
                                    alzado.DrawLine(lapiz2, x3, y2 - 16 * EscalaX, x3 + 10, y2 - 16 * EscalaX)
                                ElseIf InStr(dato_alzado(j + 1), "T2") Or InStr(dato_alzado(j + 1), "T") = 0 Then
                                    If x3 = x2 Then
                                        x3 = x2 + 5
                                    Else
                                        x3 = x2
                                    End If

                                    pos = InStr(dato_alzado(j), "#")
                                    lapiz2 = color_lapiz(pos, dato_alzado(j))
                                    alzado.DrawLine(lapiz2, x3, y2 + 10 * EscalaX, x3, y2 - 12 * EscalaX)
                                    alzado.DrawLine(lapiz2, x3, y2 - 12 * EscalaX, x3 + 10, y2 - 12 * EscalaX)
                                End If
                            End If
                        End If

                        If InStr(dato_alzado(j + 1), "T1") <> 0 And InStr(dato_alzado(j), "T2") = 0 And InStr(dato_alzado(j), "T3") = 0 Then
                            If x3 = x2 Then
                                x3 = x2 + 5
                            Else
                                x3 = x2
                            End If

                            If InStr(dato_alzado(j), "T1") <> 0 Then
                                pos = InStr(dato_alzado(j), "#")
                                lapiz2 = color_lapiz(pos, dato_alzado(j))
                                alzado.DrawLine(lapiz2, x3, y2, x3, y2 - 35 * EscalaX)
                                menc = 1
                            End If
                            If InStr(dato_alzado(j), "T1") = 0 Then
                                pos = InStr(dato_alzado(j), "#")
                                lapiz2 = color_lapiz(pos, dato_alzado(j))
                                alzado.DrawLine(lapiz2, x3, y2, x3, y2 - 25 * EscalaX)
                                menc = 1
                            End If

                        End If

                        If InStr(dato_alzado(j), "T2") <> 0 And InStr(dato_alzado(j + 1), "T1") = 0 And InStr(dato_alzado(j), "T3") = 0 Then
                            If x3 = x2 Then
                                x3 = x2 + 5
                            Else
                                x3 = x2
                            End If

                            If InStr(dato_alzado(j), "T2") <> 0 Then
                                pos = InStr(dato_alzado(j), "#")
                                lapiz2 = color_lapiz(pos, dato_alzado(j))
                                alzado.DrawLine(lapiz2, x3, y2 + 10 * EscalaX, x3, y2 - 25 * EscalaX)
                                menc = 1
                            End If
                            If InStr(dato_alzado(j), "T2") = 0 Then
                                pos = InStr(dato_alzado(j), "#")
                                lapiz2 = color_lapiz(pos, dato_alzado(j))
                                alzado.DrawLine(lapiz2, x3, y2, x3, y2 - 25 * EscalaX)
                                menc = 1
                            End If

                        End If
                    End If

                ElseIf j = dato_alzado.Count - 1 Then

                    If dato_alzado(j) <> Nothing And dato_alzado(j) <> "Error" Then
                        If InStr(dato_alzado(j), "T1") = 0 And InStr(dato_alzado(j), "T2") = 0 Then
                            pos = InStr(dato_alzado(j), "#")
                            lapiz2 = color_lapiz(pos, dato_alzado(j))
                            alzado.DrawLine(lapiz2, x3, y2, x3, y2 - 25 * EscalaX)
                            menc = 1
                        End If

                        If InStr(dato_alzado(j), "T1") <> 0 And InStr(dato_alzado(j), "T2") = 0 Then
                            pos = InStr(dato_alzado(j), "#")
                            lapiz2 = color_lapiz(pos, dato_alzado(j))
                            alzado.DrawLine(lapiz2, x3, y2, x3, y2 - 35 * EscalaX)
                            menc = 1
                        End If

                        If InStr(dato_alzado(j), "T2") <> 0 And InStr(dato_alzado(j), "T1") = 0 Then
                            pos = InStr(dato_alzado(j), "#")
                            lapiz2 = color_lapiz(pos, dato_alzado(j))
                            alzado.DrawLine(lapiz2, x3, y2 + 10 * EscalaX, x3, y2 - 35 * EscalaX)
                            menc = 1
                        End If

                    End If
                End If
                y2 = y2 - 25 * EscalaX
            Next
            If menc <> 0 Then

                texto2 = Mid(Data_Alzado.Columns(i).HeaderText, 7)
                alzado.DrawString(texto2, fuente, Brushes.Black, x2, constante)

            End If
            x2 = x2 + 30
            x3 = x2
        Next

        pb_Alzado.SizeMode = PictureBoxSizeMode.AutoSize

    End Sub

    Public Sub cb_graficar_Click(sender As Object, e As EventArgs)

        pb_Alzado.CreateGraphics.Clear(Color.White)
        pb_Alzado.Invalidate()

    End Sub

    Public Sub CopyToClipboard(datos As DataGridView)
        Dim dataObj As DataObject = datos.GetClipboardContent
        If Not IsNothing(dataObj) Then
            Clipboard.SetDataObject(dataObj)

        End If
    End Sub

    Public Sub CopiarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopiarToolStripMenuItem.Click
        CopyToClipboard(data_grid)
    End Sub

    Private Sub Cortar()
        CopyToClipboard(data_grid)
        For counter As Integer = 0 To data_grid.SelectedCells.Count - 1
            data_grid.SelectedCells(counter).Value = String.Empty
        Next
    End Sub

    Public Sub CortarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CortarToolStripMenuItem.Click
        Cortar()
    End Sub

    Private Sub Pegar()
        PasteClipboard(data_grid)

        If data_grid.Name = Data_Alzado.Name Then
            Dim Story As String

            For i = 0 To data_grid.Rows.Count - 1
                Story = Data_Alzado.Rows(i).Cells(1).Value
                validar_info2(LMuros.Text, Story, i, data_grid.Columns.Count, data_grid)
                descontar_ref(i)
            Next

        End If
    End Sub

    Public Sub PegarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PegarToolStripMenuItem.Click
        Pegar()
    End Sub

    Public Sub PasteClipboard(datos As DataGridView)

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

    Public Sub f_alzado_Click(sender As Object, e As EventArgs) Handles MyBase.Click

        Data_info.ClearSelection()
        Data_ayuda.ClearSelection()
        Data_Alzado.ClearSelection()
    End Sub

    Public Sub Button2_Click(sender As Object, e As EventArgs)
        Procesar_info(datos_alzado)
        Procesar_Info_2()
        Generar_informe()
    End Sub

    Public Sub Data_Alzado_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles Data_Alzado.CellEndEdit
        Dim indice, Columna As Integer
        indice = Data_Alzado.CurrentCell.RowIndex
        Columna = Data_Alzado.ColumnCount

        validar_info2(LMuros.Text, Data_Alzado.Rows(indice).Cells(1).Value, indice, Columna, Data_Alzado)
        descontar_ref(indice)
        pb_Alzado.CreateGraphics.Clear(Color.White)
        pb_Alzado.Invalidate()
    End Sub

    Public Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Public Sub Pb_Alzado_Click(sender As Object, e As EventArgs) Handles pb_Alzado.Click

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles button6.Click
        Procesar_info(datos_alzado)
        Procesar_Info_2()
        Generar_informe()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles button4.Click
        DibujarAlzadoAutoCAD()
    End Sub

    Private Sub DibujarAlzadoAutoCAD()

        Dim Auxiliar As Datos_Refuerzo
        Dim alzado_lista_aux As List(Of alzado_muro) = New List(Of alzado_muro)
        Dim Num_cols As Integer
        Dim indice As Integer
        Dim prueba As List(Of String) = New List(Of String)
        Dim Lista_i As New Listas_serializadas

        Guardar_Archivo.Actualizar_Resumen()
        'Guardar_Archivo.Add_Refuerzoi(data_info_f3)

        If Hviga = 0 Or prof = 0 Or Hfunda = 0 Then
            f_variables.Show()
        Else

            For i = 0 To Lista_graficar.Count - 1

                If Lista_graficar(i).Graficar = True Then

                    dibujar_alzado(Lista_graficar(i).Nombre)

                    If alzado_lista.Count > 0 And alzado_lista.Exists(Function(x) x.pier = Lista_graficar(i).Nombre) = True Then

                        alzado_lista_aux = alzado_lista.FindAll(Function(x) x.pier = Lista_graficar(i).Nombre)
                        Num_cols = alzado_lista_aux.Select(Function(x) x.alzado.Count).ToList().Max

                        If Num_cols > 0 Then

                            For j = 0 To alzado_lista_aux.Count - 1

                                For k = 0 To alzado_lista_aux(j).alzado.Count - 1
                                    If alzado_lista_aux(j).alzado(k) = Nothing Then
                                        alzado_lista_aux(j).alzado(k) = ""
                                    End If
                                Next

                                If alzado_lista_aux(j).alzado.Count < Num_cols Then
                                    For k = alzado_lista_aux(j).alzado.Count To Num_cols - 1
                                        indice = alzado_lista.FindIndex(Function(x) x.pier = alzado_lista_aux(j).pier And x.story = alzado_lista_aux(j).story)
                                        alzado_lista(indice).alzado.Add("")
                                    Next
                                End If

                            Next

                            Auxiliar = New Datos_Refuerzo
                            Auxiliar.Nombre_muro = Lista_graficar(i).Nombre
                            Auxiliar.Load_Coordinates(Lista_graficar(i).Nombre, coordX)
                            If Auxiliar.Lista_Coordenadas.Count > 0 Then
                                Dibujar_Refuerzo(Auxiliar)
                            End If
                        End If
                    End If
                End If
            Next

        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Agregar()
    End Sub

    Private Sub Agregar()
        Dim columnas As New DataGridViewTextBoxColumn

        If Data_Alzado.Columns.Count <= 2 Then
            contador = 1
            columnas.Name = "col" & contador
            columnas.HeaderText = "Alzado" & contador
            columnas.SortMode = DataGridViewColumnSortMode.NotSortable
            columnas.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            Data_Alzado.Columns.Add(columnas)
            contador = contador + 1
        Else
            contador = Mid(Data_Alzado.Columns(Data_Alzado.Columns.Count - 1).HeaderText, 7) + 1
            columnas.Name = "col" & contador
            columnas.HeaderText = "Alzado" & contador
            columnas.SortMode = DataGridViewColumnSortMode.NotSortable
            columnas.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            Data_Alzado.Columns.Add(columnas)
        End If
    End Sub

    Private Sub Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Panel3.Paint

    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PictureBox2_MouseMove(sender As Object, e As MouseEventArgs)
        PictureBox1.BackColor = Color.White
    End Sub

    Private Sub PictureBox2_MouseLeave(sender As Object, e As EventArgs)
        PictureBox1.BackColor = Color.Transparent
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Data_info_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles Data_info.CellEndEdit

        Dim indice, Indice2 As Integer
        Dim datos_refuerzo As New Refuerzo_muros
        Dim prueba As String = Mid(Data_info.Columns(5).HeaderText, 2)
        Dim suma As Single

        indice = e.RowIndex
        datos_refuerzo.piername = Data_info.Rows(indice).Cells(0).Value
        datos_refuerzo.pierstory = Data_info.Rows(indice).Cells(1).Value
        datos_refuerzo.bw = Data_info.Rows(indice).Cells(2).Value

        datos_refuerzo.as_req = Data_info.Rows(indice).Cells(6).Value

        For i = 9 To Data_info.ColumnCount - 1
            datos_refuerzo.diametro.Add(Int(Mid(Data_info.Columns(i).HeaderText, 2)))
            datos_refuerzo.cantidad.Add(Data_info.Rows(indice).Cells(i).Value)
        Next

        suma = 0
        For i = 0 To datos_refuerzo.diametro.Count - 1
            suma = suma + areas_refuerzo(datos_refuerzo.diametro(i)) * datos_refuerzo.cantidad(i)
        Next

        datos_refuerzo.total = suma
        datos_refuerzo.porcentaje = datos_refuerzo.total / datos_refuerzo.as_req

        If datos_refuerzo.total > 0 Then
            Data_info.Rows(indice).Cells(7).Value = Format(datos_refuerzo.total, "##,0.00")
            Data_info.Rows(indice).Cells(8).Value = Math.Round(datos_refuerzo.porcentaje * 100, 1) & "%"
        Else
            Data_info.Rows(indice).Cells(7).Value = Format(0.00, "##,0.00")
            Data_info.Rows(indice).Cells(8).Value = Math.Round(0.00, 1) & "%"
        End If

        If datos_refuerzo.porcentaje < 0.95 Or datos_refuerzo.porcentaje > 1.05 Then
            Data_info.Rows(indice).Cells(8).Style.ForeColor = Color.Red
        Else
            Data_info.Rows(indice).Cells(8).Style.ForeColor = Color.Black
        End If

        Indice2 = refuerzo_lista.FindIndex(Function(x) x.piername = datos_refuerzo.piername And x.pierstory = datos_refuerzo.pierstory)

        If Indice2 >= 0 Then
            refuerzo_lista(Indice2) = datos_refuerzo
        Else
            refuerzo_lista.Add(datos_refuerzo)
        End If

        Tabla_Data_Ayuda(datos_refuerzo.piername, Data_ayuda, indice, False)

    End Sub

    Private Sub Panel11_Paint(sender As Object, e As PaintEventArgs) Handles Panel11.Paint

    End Sub

    Private Sub Data_Alzado_CellMouseClick_1(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Data_Alzado.CellMouseClick
        data_grid = Data_Alzado

        If Data_Alzado.SelectedCells.Count > 0 Then
            Data_Alzado.ContextMenuStrip = ContextMenuStrip1
            If Data_Alzado.CurrentCell.ReadOnly = True Then
                ContextMenuStrip1.Enabled = False
            Else
                ContextMenuStrip1.Enabled = True
                CortarToolStripMenuItem.Enabled = True
            End If

        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        'Recurso_Excel(alzado_lista)
    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        Form_DireccionCambiodeEspesor.Show()
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening

    End Sub



    Private Sub F_alzado_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyData = Keys.Control + Keys.Q Then
            Agregar()
        End If

        If e.KeyData = Keys.Control + Keys.W Then
            DibujarAlzadoAutoCAD()
        End If

        If e.KeyData = Keys.Control + Keys.C Then
            CopyToClipboard(data_grid)
        End If

        If e.KeyData = Keys.Control + Keys.V Then
            Pegar()
        End If
        If e.KeyData = Keys.Control + Keys.X Then
            Cortar()
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles button5.Click

        If Hviga <> 0 And prof <> 0 Then
            GenerarCantidades(alzado_lista, Muros_lista_2)
        Else
            f_variables.Show()
        End If
    End Sub
End Class