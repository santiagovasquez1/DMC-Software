Module Modulo1

    Private Lista_texto As List(Of String)

    Sub cargar_lista_muros()
        Try
            Dim muros_distintos As IEnumerable(Of String) = piername.Distinct
            Dim j As Integer = 0
            For Each elemento As String In muros_distintos
                ReDim Preserve list_muros(j)
                list_muros(j) = elemento
                j = j + 1
            Next
        Catch ex As Exception

        End Try
    End Sub

    Sub Validar_info1(ByVal Nombre_Muro As String, ByVal i As Integer, ByVal Data_ganeral As DataGridView)


        Dim Indice As Integer
        Dim j As Integer = 0

        Indice = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Nombre_Muro)

        With Muros_lista_2(Indice)

            .Bw(i) = Data_ganeral.Rows(i).Cells(2).Value
            .lw(i) = Data_ganeral.Rows(i).Cells(3).Value
            .Hw(i) = Data_ganeral.Rows(i).Cells(4).Value
            .fc(i) = Data_ganeral.Rows(i).Cells(5).Value
            .Rho_l(i) = Data_ganeral.Rows(i).Cells(6).Value
            .Rho_T(i) = Data_ganeral.Rows(i).Cells(7).Value
            .Malla(i) = Data_ganeral.Rows(i).Cells(8).Value
            .As_Long(i) = Data_ganeral.Rows(i).Cells(9).Value

            .Lebe_Izq(i) = Data_ganeral.Rows(i).Cells(10).Value
            .Lebe_Der(i) = Data_ganeral.Rows(i).Cells(11).Value
            .Lebe_Centro(i) = Data_ganeral.Rows(i).Cells(12).Value

            .Est_ebe(i) = Data_ganeral.Rows(i).Cells(13).Value
            .Sep_ebe(i) = Data_ganeral.Rows(i).Cells(14).Value
            .ramas_izq(i) = Data_ganeral.Rows(i).Cells(15).Value
            .ramas_der(i) = Data_ganeral.Rows(i).Cells(16).Value
            .ramas_centro(i) = Data_ganeral.Rows(i).Cells(17).Value

            .Zc_Izq(i) = Data_ganeral.Rows(i).Cells(18).Value
            .Zc_Der(i) = Data_ganeral.Rows(i).Cells(19).Value
            .Est_Zc(i) = Data_ganeral.Rows(i).Cells(20).Value
            .Sep_Zc(i) = Data_ganeral.Rows(i).Cells(21).Value

            .As_htal(i) = Data_ganeral.Rows(i).Cells(22).Value
            .Ref_htal(i) = Data_ganeral.Rows(i).Cells(23).Value

            Try
                .Capas_htal(i) = Data_ganeral.Rows(i).Cells(24).Value
                .sep_htal(i) = Data_ganeral.Rows(i).Cells(25).Value
                .As_Htal_Total(i) = Data_ganeral.Rows(i).Cells(26).Value
            Catch ex As Exception
                .Capas_htal(i) = 0
                .sep_htal(i) = 0
                .As_Htal_Total(i) = 0
            End Try

        End With


    End Sub
    Sub FuncionAgregar(ByRef DataAlzado1 As DataGridView)

        Dim columnas As New DataGridViewTextBoxColumn

        If DataAlzado1.Columns.Count <= 2 Then
            contador = 1
            columnas.Name = "col" & contador
            columnas.HeaderText = "Alzado" & contador
            columnas.SortMode = DataGridViewColumnSortMode.NotSortable
            columnas.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            DataAlzado1.Columns.Add(columnas)
            contador = contador + 1
        Else
            contador = Mid(DataAlzado1.Columns(DataAlzado1.Columns.Count - 1).HeaderText, 7) + 1
            columnas.Name = "col" & contador
            columnas.HeaderText = "Alzado" & contador
            columnas.SortMode = DataGridViewColumnSortMode.NotSortable
            columnas.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            DataAlzado1.Columns.Add(columnas)
        End If

    End Sub

    Sub validar_info2(ByVal Nombre_Muro As String, ByVal Story As String, ByVal i As Integer, ByVal C As Integer, ByVal Data_alzado As DataGridView) ''Actualiza los datos de la informacion del alzado del muro de concreto

        Dim Muro_i As Muros_Consolidados
        Dim Muros_hijos As List(Of Muros_Consolidados) = New List(Of Muros_Consolidados)
        Dim Indice As Integer
        Dim Alzado_i As alzado_muro

        If alzado_lista.Count = 0 Or alzado_lista.Exists(Function(x) x.pier = Nombre_Muro And x.story = Story) = False Then
            Alzado_i = New alzado_muro
            Alzado_i.pier = Nombre_Muro
            Alzado_i.story = Story
            Agregar_Columnas(i, C, Data_alzado, Alzado_i)
            alzado_lista.Add(Alzado_i)
        Else
            Indice = alzado_lista.FindIndex(Function(x) x.pier = Nombre_Muro And x.story = Story)
            Agregar_Columnas(i, C, Data_alzado, alzado_lista(Indice))
        End If

        'Muro_i = Muros_lista_2.Find(Function(x) x.Pier_name = Nombre_Muro)

        'If Muro_i.isMuroMaestro = True Then

        '    Find_Muros_Hijos(Muro_i, Muros_hijos)

        '    If Muros_hijos.Count > 0 Then

        '        For j = 0 To Muros_hijos.Count - 1

        '            If alzado_lista.Exists(Function(x) x.pier = Muros_hijos(j).Pier_name And x.story = Story) = False Then
        '                Alzado_i = New alzado_muro
        '                Alzado_i.pier = Muros_hijos(j).Pier_name
        '                Alzado_i.story = Story
        '                Agregar_Columnas(i, C, Data_alzado, Alzado_i)
        '                alzado_lista.Add(Alzado_i)
        '            Else
        '                Indice = alzado_lista.FindIndex(Function(x) x.pier = Muros_hijos(i).Pier_name And x.story = Story)
        '                Agregar_Columnas(i, C, Data_alzado, alzado_lista(Indice))
        '            End If

        '        Next

        '    End If

        'End If

    End Sub

    Public Sub Validar_info_3(indice As Integer, ByRef Indice2 As Integer, datos_refuerzo As Refuerzo_muros, ByRef suma As Single, ByRef Muro_i As Muros_Consolidados, Muros_Hijos As List(Of Muros_Consolidados), ByVal Data_info As DataGridView)

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

        Muro_i = Muros_lista_2.Find(Function(x) x.Pier_name = datos_refuerzo.piername)
        Find_Muros_Hijos(Muro_i, Muros_Hijos)

        If Muros_Hijos.Count > 0 Then
            For j = 0 To Muros_Hijos.Count - 1

                datos_refuerzo.piername = Muros_Hijos(j).Pier_name
                Indice2 = refuerzo_lista.FindIndex(Function(x) x.piername = datos_refuerzo.piername And x.pierstory = datos_refuerzo.pierstory)

                If Indice2 >= 0 Then
                    refuerzo_lista(Indice2) = datos_refuerzo
                Else
                    refuerzo_lista.Add(datos_refuerzo)
                End If
            Next
        End If
    End Sub

    Public Sub Find_Muros_Hijos(Muro_i As Muros_Consolidados, Muros_hijos As List(Of Muros_Consolidados))

        For j = 0 To Muros_lista_2.Count - 1

            If Muros_lista_2(j).MuroSimilar IsNot Nothing = True Then
                If Muros_lista_2(j).MuroSimilar.Pier_name = Muro_i.Pier_name Then
                    Muros_hijos.Add(Muros_lista_2(j))
                End If
            End If
        Next
    End Sub

    Private Sub Agregar_Columnas(ByVal i As Integer, ByVal C As Integer, ByVal Data_alzado As DataGridView, ByVal Alzado_i As alzado_muro)

        For j = 0 To C - 3
            Try
                Alzado_i.alzado(j) = Data_alzado.Rows(i).Cells(j + 2).Value
            Catch ex As Exception
                Try
                    If Data_alzado.Rows(i).Cells(j + 2).Value = Nothing Then
                        Alzado_i.alzado.Add("")
                    Else
                        Alzado_i.alzado.Add(Data_alzado.Rows(i).Cells(j + 2).Value)
                    End If

                Catch ex2 As Exception
                    Alzado_i.alzado.Add("")
                End Try
            End Try
        Next
    End Sub
End Module
