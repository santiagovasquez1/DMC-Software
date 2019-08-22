Module Organizar_alzados

    Public Sub organizar_Alzados_1()

        Dim Alzado_aux As List(Of alzado_muro)
        Dim Maximo_Cols As Integer
        Dim Indice As Integer
        Maximo_Cols = 0

        For i = 0 To Muros_lista_2.Count - 1

            Maximo_Cols = 0
            Alzado_aux = alzado_lista.FindAll(Function(x) x.pier = Muros_lista_2(i).Pier_name).ToList()

            For j = 0 To Alzado_aux.Count - 1

                If Alzado_aux(j).alzado.Count >= Maximo_Cols Then
                    Maximo_Cols = Alzado_aux(j).alzado.Count
                End If

            Next

            For j = 0 To Alzado_aux.Count - 1

                If Alzado_aux(j).alzado.Count < Maximo_Cols Then

                    Indice = alzado_lista.FindIndex(Function(x) x.pier = Alzado_aux(j).pier And x.story = Alzado_aux(j).story)

                    For k = Alzado_aux(j).alzado.Count To Maximo_Cols - 1

                        alzado_lista(Indice).alzado.Add("")

                    Next

                End If

            Next

        Next

    End Sub

End Module