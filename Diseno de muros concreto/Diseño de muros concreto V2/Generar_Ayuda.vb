Module Generar_Ayuda
    Public Sub Tabla_Data_Ayuda(ByVal Nombre_muro As String, ByVal Data_ayuda As DataGridView, ByVal i As Integer, ByVal Crear_Fila As Boolean)

        Dim Muroi As Muros_Consolidados
        Dim indice As Integer


        Muroi = Muros_lista_2.Find(Function(x1) x1.Pier_name = Nombre_muro)

        If Muroi.MuroSimilar IsNot Nothing Then

            Muroi = Muroi.MuroSimilar

        End If


        If Crear_Fila = True Then
            Data_ayuda.Rows.Add()
        End If

        With Data_ayuda.Rows(i)
            .Cells(0).Value = Nombre_muro
            .Cells(0).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Cells(0).Style.BackColor = Color.LightGray

            .Cells(1).Value = Muroi.Stories(i)
            .Cells(1).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Cells(1).Style.BackColor = Color.LightGray

            .Cells(2).Value = Format(Muroi.Bw(i), "##,0")
            .Cells(2).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Cells(2).Style.BackColor = Color.LightGray

            If Muroi.Lebe_Izq(i) <> 0 Then
                .Cells(3).Value = Format(Muroi.Lebe_Izq(i), "##,0")
                .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(3).Style.BackColor = Color.DarkGreen
                .Cells(3).ToolTipText = "EBE"
            ElseIf Muroi.Zc_Izq(i) <> 0 And Muroi.Lebe_Izq(i) = 0 Then
                .Cells(3).Value = Format(Muroi.Zc_Izq(i), "##,0")
                .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(3).Style.BackColor = Color.LightGreen
                .Cells(3).ToolTipText = "Zc"
            Else
                .Cells(3).Value = 0
                .Cells(3).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(3).Style.BackColor = Color.LightGray
            End If

            If Muroi.Lebe_Der(i) <> 0 Then
                .Cells(4).Value = Format(Muroi.Lebe_Der(i), "##,0")
                .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(4).Style.BackColor = Color.DarkGreen
                .Cells(4).ToolTipText = "EBE"
            ElseIf Muroi.Zc_Der(i) <> 0 And Muroi.Lebe_Der(i) = 0 Then
                .Cells(4).Value = Format(Muroi.Zc_Der(i), "##,0")
                .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(4).Style.BackColor = Color.LightGreen
                .Cells(4).ToolTipText = "Zc"
            Else
                .Cells(4).Value = 0
                .Cells(4).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Cells(4).Style.BackColor = Color.LightGray
            End If

            If refuerzo_lista.Count = 0 Or refuerzo_lista.Exists(Function(x1) x1.piername = Nombre_muro And x1.pierstory = Muroi.Stories(i)) = False Then
                .Cells(5).Value = 0
                .Cells(6).Value = 0
                .Cells(7).Value = 0
                .Cells(8).Value = 0
                .Cells(9).Value = 0
                .Cells(10).Value = 0
                .Cells(11).Value = 0
                .Cells(12).Value = 0
                .Cells(13).Value = 0
                .Cells(14).Value = 0
                .Cells(15).Value = 0

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

                    Dim contador As Integer
                    contador = 5

                    For j = 0 To MuroAxuiliar.diametro.Count - 1
                        .Cells(contador).Value = MuroAxuiliar.cantidad(j)
                        contador = contador + 1
                    Next

                End If

            End If

        End With
        ' Data_ayuda.Refresh()
        ' Data_ayuda.Update()


    End Sub
End Module
