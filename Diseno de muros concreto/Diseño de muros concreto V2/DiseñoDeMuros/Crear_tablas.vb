Module Crear_tablas

    Public Ds_form3 As DataSet

    Sub Tabla_info_gnl(ByVal Nombre_muro As String, ByVal Data_info As DataGridView)

        Dim Muroi As Muros_Consolidados
        Dim indice As Integer

        Muroi = Muros_lista_2.Find(Function(x1) x1.Pier_name = Nombre_muro)

        If Muroi.MuroSimilar IsNot Nothing Then
            Muroi = Muroi.MuroSimilar
        End If

        For i = 0 To Muroi.Stories.Count - 1
            Data_info.Rows.Add()

            With Data_info.Rows(i)

                .Cells(0).Value = Nombre_muro
                .Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(0).Style.BackColor = Color.LightGray

                .Cells(1).Value = Muroi.Stories(i)
                .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(1).Style.BackColor = Color.LightGray

                .Cells(2).Value = Format(Muroi.Bw(i), "##,0")
                .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(2).Style.BackColor = Color.LightGray

                .Cells(3).Value = Format(Muroi.Rho_l(i), "##,0.0000")
                .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(3).Style.BackColor = Color.LightGray

                If Muroi.Lebe_Izq(i) <> 0 Then
                    .Cells(4).Value = Format(Muroi.Lebe_Izq(i), "##,0")
                    .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Cells(4).Style.BackColor = Color.DarkGreen
                    .Cells(4).ToolTipText = "EBE"
                ElseIf Muroi.Zc_Izq(i) <> 0 And Muroi.Lebe_Izq(i) = 0 Then
                    .Cells(4).Value = Format(Muroi.Zc_Izq(i), "##,0")
                    .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Cells(4).Style.BackColor = Color.LightGreen
                    .Cells(4).ToolTipText = "Zc"
                Else
                    .Cells(4).Value = 0
                    .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Cells(4).Style.BackColor = Color.LightGray
                End If

                If Muroi.Lebe_Der(i) <> 0 Then
                    .Cells(5).Value = Format(Muroi.Lebe_Der(i), "##,0")
                    .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Cells(5).Style.BackColor = Color.DarkGreen
                    .Cells(5).ToolTipText = "EBE"
                ElseIf Muroi.Zc_Der(i) <> 0 And Muroi.Lebe_Der(i) = 0 Then
                    .Cells(5).Value = Format(Muroi.Zc_Der(i), "##,0")
                    .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Cells(5).Style.BackColor = Color.LightGreen
                    .Cells(5).ToolTipText = "Zc"
                Else
                    .Cells(5).Value = 0
                    .Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .Cells(5).Style.BackColor = Color.LightGray
                End If

                .Cells(6).Value = Format(Muroi.As_Long(i), "##,0.00")
                .Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(6).Style.ForeColor = Color.Blue
                .Cells(6).Style.BackColor = Color.LightGray

                .Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(7).Style.BackColor = Color.LightGray
                .Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(8).Style.BackColor = Color.LightGray
                .Cells(8).Style.ForeColor = Color.Red

                If refuerzo_lista.Count = 0 Or refuerzo_lista.Exists(Function(x1) x1.piername = Nombre_muro And x1.pierstory = Muroi.Stories(i)) = False Then
                    .Cells(7).Value = 0
                    .Cells(8).Value = 0 & "%"
                    .Cells(9).Value = 0
                    .Cells(10).Value = 0
                    .Cells(11).Value = 0
                    .Cells(12).Value = 0
                    .Cells(13).Value = 0
                    .Cells(14).Value = 0
                    .Cells(15).Value = 0
                    .Cells(16).Value = 0
                    .Cells(17).Value = 0
                    .Cells(18).Value = 0
                    .Cells(19).Value = 0
                Else

                    Dim MuroAxuiliar As New Refuerzo_muros
                    If refuerzo_lista.Exists(Function(x) x.piername = Muroi.Pier_name And x.pierstory = Muroi.Stories(i) And x.MuroSimilar Is Nothing) Then

                        indice = refuerzo_lista.FindIndex(Function(x) x.piername = Muroi.Pier_name And x.pierstory = Muroi.Stories(i) And x.MuroSimilar Is Nothing)
                        MuroAxuiliar = refuerzo_lista(indice)
                    Else
                        indice = refuerzo_lista.FindIndex(Function(x) x.piername = Muroi.Pier_name And x.pierstory = Muroi.Stories(i) And x.MuroSimilar IsNot Nothing)
                        MuroAxuiliar = refuerzo_lista(indice).MuroSimilar
                    End If

                    If indice >= 0 Then
                        .Cells(7).Value = Format(MuroAxuiliar.total, "##,0.00")
                        .Cells(8).Value = Format(MuroAxuiliar.porcentaje * 100, "##,0.0") & "%"

                        If MuroAxuiliar.porcentaje > 1.05 Or MuroAxuiliar.porcentaje < 0.95 Then
                            .Cells(8).Style.ForeColor = Color.Red
                        Else
                            .Cells(8).Style.ForeColor = Color.Black
                        End If
                        Dim contador As Integer
                        contador = 9

                        For j = 0 To MuroAxuiliar.diametro.Count - 1
                            .Cells(contador).Value = MuroAxuiliar.cantidad(j)
                            contador = contador + 1
                        Next

                    End If

                End If
            End With

        Next
        ' Data_info.Refresh()
        '  Data_info.Update()

    End Sub

    Sub Alzado_info(ByVal Nombre_muro As String, ByVal Data_Alzado As DataGridView)

        Dim Muroi As Muros_Consolidados
        Dim indice, contador As Integer
        Dim Columna_i As DataGridViewTextBoxColumn
        Dim Lista_Cols As New List(Of DataGridViewTextBoxColumn)

        Muroi = Muros_lista_2.Find(Function(x1) x1.Pier_name = Nombre_muro)

        contador = 0

        For i = 0 To Muroi.Stories.Count - 1

            Data_Alzado.Rows.Add()

            With Data_Alzado.Rows(i)
                .Cells(0).Value = Muroi.Pier_name
                .Cells(1).Value = Muroi.Stories(i)

                .Cells(0).ReadOnly = True
                .Cells(1).ReadOnly = True
                Dim Lista_auxiliar As New List(Of alzado_muro)
                If alzado_lista.Count > 0 And alzado_lista.Exists(Function(x) x.pier = Muroi.Pier_name) = True Then

                    If alzado_lista.Exists(Function(x) x.pier = Muroi.Pier_name And x.MuroSimilar Is Nothing) = True Then
                        Lista_auxiliar = alzado_lista.FindAll(Function(x) x.pier = Muroi.Pier_name And x.MuroSimilar Is Nothing).ToList
                        indice = Lista_auxiliar.FindIndex(Function(x) x.story = Muroi.Stories(i))
                    Else
                        For j = 0 To alzado_lista.Count - 1
                            If alzado_lista(j).MuroSimilar IsNot Nothing And alzado_lista(j).pier = Muroi.Pier_name Then
                                Lista_auxiliar.Add(alzado_lista(j).MuroSimilar)
                            End If

                        Next

                    End If

                    indice = Lista_auxiliar.FindIndex(Function(x) x.story = Muroi.Stories(i))
                    If indice >= 0 Then

                        For j = 0 To Lista_auxiliar(indice).alzado.Count - 1

                            Try
                                .Cells(j + 2).Value = Lista_auxiliar(indice).alzado(j)
                            Catch ex As Exception
                                Columna_i = New DataGridViewTextBoxColumn

                                If Data_Alzado.ColumnCount = 2 Then
                                    contador = 1
                                Else
                                    contador = contador + 1
                                End If
                                Columna_i.Name = "Alz" & contador
                                Columna_i.HeaderText = "Alzado" & contador
                                Columna_i.SortMode = DataGridViewColumnSortMode.NotSortable
                                Columna_i.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                                Data_Alzado.Columns.Add(Columna_i)
                                .Cells(j + 2).Value = Lista_auxiliar(indice).alzado(j)
                            End Try

                        Next

                    End If
                End If

            End With

        Next
        '  Data_Alzado.Refresh()
        '  Data_Alzado.Update()

    End Sub

    Sub SetupDataGridView(ByVal Nombre_Data As String, ByVal Formulario As DataGridView, ByVal Origen_datos As DataTable)

        For Each control As Control In Formulario.Controls
            Formulario.Controls.Remove(control)
        Next

        Formulario.Controls.Add(Formulario)
        Formulario.VirtualMode = True
        Formulario.DataSource = Origen_datos

        Formulario.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
        Formulario.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        Formulario.ColumnHeadersDefaultCellStyle.Font = New Font("Verdana", 8.0F, FontStyle.Bold)
        Formulario.RowsDefaultCellStyle.Font = New Font("Verdana", 8.0F, FontStyle.Regular)
        Formulario.BackgroundColor = Color.FromName("Control")
        Formulario.BorderStyle = BorderStyle.None

        Formulario.Dock = DockStyle.Fill
        Formulario.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders
        Formulario.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        Formulario.CellBorderStyle = DataGridViewCellBorderStyle.Single
        Formulario.RowHeadersVisible = True
        Formulario.ColumnHeadersHeight = 40
        Formulario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

    End Sub

    Sub Base_datos_info_refuerzo(ByVal Nombre_muro As String)

        Dim Encabezados As New List(Of String) From {"Pier", "Story", "Bw" & vbNewLine & "cm", "rho l", "Li" & vbNewLine & "cm", "LD" & vbNewLine & "cm", "As R" & vbNewLine & "cm²", "As T" & vbNewLine & "cm²", "% Ref"}
        Dim Texto As String
        Dim T_info As New DataTable("Info refuerzo adicional")
        Dim X, Pos, Indice As Integer
        Dim Muroi As Muros_Consolidados

        Muroi = Muros_lista_2.Find(Function(x1) x1.Pier_name = Nombre_muro)
        X = 0
        Pos = -1
        For i = 2 To 14
            Texto = "#" & i
            Encabezados.Add(Texto)
        Next

        For Each Tabla As DataTable In Ds_form3.Tables
            If Tabla.TableName = "Info refuerzo adicional" Then
                Pos = X
                Exit For
            End If
            X += 1
        Next

        If Pos > -1 Then
            Ds_form3.Tables.Clear()
            Ds_form3.Tables.Add(T_info)
        Else
            Ds_form3.Tables.Add(T_info)
        End If

        For i = 0 To Encabezados.Count - 1
            Dim Columna_i As New DataColumn(Encabezados(i))
            T_info.Columns.Add(Columna_i)
        Next

        For i = 0 To Muroi.Stories.Count - 1

            Dim Cells As DataRow = T_info.NewRow()
            Cells(0).Value = Muroi.Pier_name
            Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Cells(0).Style.BackColor = Color.LightGray

            Cells(1).Value = Muroi.Stories(i)
            Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Cells(1).Style.BackColor = Color.LightGray

            Cells(2).Value = Format(Muroi.Bw(i), "##,0")
            Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Cells(2).Style.BackColor = Color.LightGray

            Cells(3).Value = Format(Muroi.Rho_l(i), "##,0.0000")
            Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Cells(3).Style.BackColor = Color.LightGray

            If Muroi.Lebe_Izq(i) <> 0 Then
                Cells(4).Value = Format(Muroi.Lebe_Izq(i), "##,0")
                Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                Cells(4).Style.BackColor = Color.DarkGreen
                Cells(4).ToolTipText = "EBE"
            ElseIf Muroi.Zc_Izq(i) <> 0 And Muroi.Lebe_Izq(i) = 0 Then
                Cells(4).Value = Format(Muroi.Zc_Izq(i), "##,0")
                Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                Cells(4).Style.BackColor = Color.LightGreen
                Cells(4).ToolTipText = "Zc"
            Else
                Cells(4).Value = 0
                Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                Cells(4).Style.BackColor = Color.LightGray
            End If

            If Muroi.Lebe_Der(i) <> 0 Then
                Cells(5).Value = Format(Muroi.Lebe_Der(i), "##,0")
                Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                Cells(5).Style.BackColor = Color.DarkGreen
                Cells(5).ToolTipText = "EBE"
            ElseIf Muroi.Zc_Der(i) <> 0 And Muroi.Lebe_Der(i) = 0 Then
                Cells(5).Value = Format(Muroi.Zc_Der(i), "##,0")
                Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                Cells(5).Style.BackColor = Color.LightGreen
                Cells(5).ToolTipText = "Zc"
            Else
                Cells(5).Value = 0
                Cells(5).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                Cells(5).Style.BackColor = Color.LightGray
            End If

            Cells(6).Value = Format(Muroi.As_Long(i), "##,0.00")
            Cells(6).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Cells(6).Style.ForeColor = Color.Blue
            Cells(6).Style.BackColor = Color.LightGray

            Cells(7).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Cells(7).Style.BackColor = Color.LightGray
            Cells(8).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Cells(8).Style.BackColor = Color.LightGray
            Cells(8).Style.ForeColor = Color.Red

            If refuerzo_lista.Count = 0 Or refuerzo_lista.Exists(Function(x1) x1.piername = Nombre_muro And x1.pierstory = Muroi.Stories(i)) = False Then
                Cells(7).Value = 0
                Cells(8).Value = 0 & "%"
                Cells(9).Value = 0
                Cells(10).Value = 0
                Cells(11).Value = 0
                Cells(12).Value = 0
                Cells(13).Value = 0
                Cells(14).Value = 0
                Cells(15).Value = 0
                Cells(16).Value = 0
                Cells(17).Value = 0
                Cells(18).Value = 0
                Cells(19).Value = 0
            Else ''Llenado de informacion en la lista
                Indice = refuerzo_lista.FindIndex(Function(x1) x1.piername = Muroi.Pier_name And x1.pierstory = Muroi.Stories(i))
                If Indice >= 0 Then
                    Cells(7).Value = Format(refuerzo_lista(Indice).total, "##,0.00")
                    Cells(8).Value = Format(refuerzo_lista(Indice).porcentaje * 100, "##,0.0") & "%"

                    Dim contador As Integer
                    contador = 9

                    For j = 0 To refuerzo_lista(Indice).diametro.Count - 1
                        Cells(contador).Value = refuerzo_lista(Indice).cantidad(j)
                        contador += 1
                    Next

                End If

            End If

            T_info.Rows.Add(Cells)
        Next

    End Sub

End Module