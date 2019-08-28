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

                .Cells(1).Value = ListaMuros(i).DireccionCambioEspesor
                .Cells(1).Style = Estilo

            End With
        Next

    End Sub

    Sub AceptarDireccion(ByVal DataGrid As DataGridView)

        For i = 0 To DataGrid.Rows.Count - 1
            With DataGrid.Rows(i)
                Muros_lista_2.Find(Function(x) x.Pier_name = .Cells(0).Value).DireccionCambioEspesor = .Cells(1).Value
            End With
        Next

    End Sub

End Module