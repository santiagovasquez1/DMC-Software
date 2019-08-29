Module DireccionCambioMuro

    Sub CrearDataGrid(ByVal DataGrid As DataGridView, ByVal ListaMuros As List(Of Muros_Consolidados))

        Dim Estilo As New DataGridViewCellStyle
        Estilo.Alignment = DataGridViewContentAlignment.MiddleCenter
        Estilo.Font = New Font("Verdana", 8)
        Estilo.BackColor = Color.White

        DataGrid.Columns(0).HeaderCell.Style = Estilo
        DataGrid.Columns(1).HeaderCell.Style = Estilo

        For i = 0 To ListaMuros.Count - 1
            DataGrid.Rows.Add()
            With DataGrid.Rows(i)
                .Cells(0).Value = ListaMuros(i).Pier_name
                .Cells(0).Style = Estilo
                .Cells(0).ReadOnly = True

                Select Case ListaMuros(i).Reduccion
                    Case Reduccion.Derecha
                        .Cells(1).Value = "Derecha"
                    Case Reduccion.Izquierda
                        .Cells(1).Value = "Izquierda"
                    Case Reduccion.Arriba
                        .Cells(1).Value = "Arriba"
                    Case Reduccion.Abajo
                        .Cells(1).Value = "Abajo"
                    Case Reduccion.Centro
                        .Cells(1).Value = "Centro"
                    Case Reduccion.NoAplica
                        .Cells(1).Value = "No Aplica"
                End Select

                .Cells(1).Style = Estilo

            End With
        Next

    End Sub

    Sub AceptarDireccion(ByVal DataGrid As DataGridView)

        Dim Cambio_espesor As String

        For i = 0 To DataGrid.Rows.Count - 1
            With DataGrid.Rows(i)

                Cambio_espesor = .Cells(1).Value
                Select Case Cambio_espesor
                    Case "Derecha"
                        Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).Reduccion = Reduccion.Derecha
                    Case "Izquierda"
                        Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).Reduccion = Reduccion.Izquierda
                    Case "Arriba"
                        Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).Reduccion = Reduccion.Arriba
                    Case "Abajo"
                        Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).Reduccion = Reduccion.Abajo
                    Case "Centro"
                        Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).Reduccion = Reduccion.Centro
                    Case "No Aplica"
                        Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).Reduccion = Reduccion.NoAplica
                End Select

            End With
        Next

    End Sub

End Module