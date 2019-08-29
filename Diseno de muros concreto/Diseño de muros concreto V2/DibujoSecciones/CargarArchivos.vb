Module CargarArchivos
    Private Lista_texto As New List(Of String)

    Sub Cargar_Resumen()

        Dim Inicio, Fin As Integer
        Dim Vector_Texto As String()
        Dim Auxiliar As List(Of List(Of String)) = New List(Of List(Of String))
        Dim Muros_distintos As List(Of String)
        Dim Muro_i As Muros_Consolidados

        Inicio = Lista_texto.FindIndex(Function(x) x.Contains("5.Reporte")) + 2
        Try
            Fin = Lista_texto.FindIndex(Function(x) x.Contains("6.Datos de Refuerzo Adicional")) - 2
        Catch ex As Exception
            Fin = Lista_texto.FindIndex(Function(x) x.Contains("Fin")) - 2
        End Try

        If Muros_lista_2 IsNot Nothing Then
            Muros_lista_2.Clear()
        Else
            Muros_lista_2 = New List(Of Muros_Consolidados)
        End If

        For i As Integer = Inicio To Fin
            Vector_Texto = Lista_texto(i).Split(vbTab)
            Auxiliar.Add(Vector_Texto.ToList())
        Next

        Muros_distintos = Auxiliar.Select(Function(x) x(1)).Distinct().OrderBy(Function(x2) x2).ToList()

        For i As Integer = 0 To Muros_distintos.Count - 1

            Dim Auxiliar_2 As List(Of List(Of String)) = Auxiliar.FindAll(Function(x) x(1) = Muros_distintos(i)).ToList()
            Muro_i = New Muros_Consolidados()
            Muro_i.Pier_name = Muros_distintos(i)
            Muro_i.Stories.AddRange(Auxiliar_2.Select(Function(x) x(0)))
            Muro_i.lw.AddRange(Auxiliar_2.Select(Function(x) Convert.ToSingle(x(2))))
            Muro_i.Bw.AddRange(Auxiliar_2.Select(Function(x) Convert.ToSingle(x(3))))
            Muro_i.fc.AddRange(Auxiliar_2.Select(Function(x) Convert.ToSingle(x(4))))
            Muro_i.Rho_T.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(5))))
            Muro_i.Rho_l.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(6))))
            Muro_i.Malla.AddRange(Auxiliar_2.Select(Function(x) x(7)))
            Muro_i.C_Def.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(8))))
            Muro_i.Lebe_Izq.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(9))))
            Muro_i.Lebe_Der.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(10))))
            Muro_i.Lebe_Centro.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(11))))
            Muro_i.Zc_Izq.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(12))))
            Muro_i.Zc_Der.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(13))))
            Muro_i.Hw.AddRange(Auxiliar_2.Select(Function(x) Convert.ToSingle(x(14))))
            Muro_i.Est_ebe.AddRange(Auxiliar_2.Select(Function(x) Convert.ToInt32(x(15))))
            Muro_i.Sep_ebe.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(16))))
            Muro_i.Est_Zc.AddRange(Auxiliar_2.Select(Function(x) Convert.ToInt32(x(17))))
            Muro_i.Sep_Zc.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(18))))
            Muro_i.As_Long.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(19))))
            Muro_i.ramas_izq.AddRange(Auxiliar_2.Select(Function(x) Convert.ToInt32(x(20))))
            Muro_i.ramas_der.AddRange(Auxiliar_2.Select(Function(x) Convert.ToInt32(x(21))))
            Muro_i.ramas_centro.AddRange(Auxiliar_2.Select(Function(x) Convert.ToInt32(x(22))))
            Muro_i.As_htal.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(23))))
            Muro_i.Ref_htal.AddRange(Auxiliar_2.Select(Function(x) x(24)))
            Muro_i.Capas_htal.AddRange(Auxiliar_2.Select(Function(x) Convert.ToInt32(x(25))))
            Muro_i.sep_htal.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(26))))
            Muro_i.As_Htal_Total.AddRange(Auxiliar_2.Select(Function(x) Convert.ToDouble(x(27))))
            Muros_lista_2.Add(Muro_i)
        Next

    End Sub

    Sub CargarRefuerzo()

        Dim Inicio, Fin As Integer
        Dim Auxiliar As List(Of List(Of String)) = New List(Of List(Of String))
        Dim Vector_Texto As String()
        Dim Auxiliar2 As List(Of List(Of String)) = New List(Of List(Of String))
        Dim Auxiliar3 As List(Of List(Of String)) = New List(Of List(Of String))

        Try
            Inicio = Lista_texto.FindIndex(Function(x) x.Contains("6.Datos de Refuerzo Adicional")) + 2
            Fin = Lista_texto.FindIndex(Function(x) x.Contains("7.Datos de alzado refuerzo longitudinal")) - 2
        Catch ex As Exception
            Fin = Lista_texto.FindIndex(Function(x) x.Contains("Fin")) - 2
        End Try

        For i As Integer = Inicio To Fin
            Vector_Texto = Lista_texto(i).Split(vbTab)
            Auxiliar.Add(Vector_Texto.ToList())
        Next

        For i = 0 To Muros_lista_2.Count - 1
            For j = 0 To Auxiliar.Count - 1

                If Muros_lista_2(i).Pier_name = Auxiliar(j)(0) Then

                    Dim SumaDeBarras As Double = 0

                    For s = 6 To 26 Step 2
                        SumaDeBarras = SumaDeBarras + Val(Auxiliar(j)(s))

                    Next

                    Muros_lista_2(i).NoBarras.Add(SumaDeBarras)

                End If

            Next
        Next

        'Nombre de Barras

        Try
            Inicio = Lista_texto.FindIndex(Function(x) x.Contains("7.Datos de alzado refuerzo longitudinal")) + 2
            Fin = Lista_texto.FindIndex(Function(x) x.Contains("8.Datos de alzado refuerzo longitudinal - Longitud")) - 2
        Catch ex As Exception
            Inicio = 0
            Fin = 0
        End Try

        For i As Integer = Inicio To Fin
            Vector_Texto = Lista_texto(i).Split(vbTab)
            Auxiliar2.Add(Vector_Texto.ToList())
        Next
        Dim IndiceStory As Integer

        For i = 0 To Muros_lista_2.Count - 1

            For j = 0 To Auxiliar2.Count - 1
                Dim NombreBarras As New List(Of String)
                If Muros_lista_2(i).Pier_name = Auxiliar2(j)(0) Then
                    IndiceStory = Auxiliar2(j).FindIndex(Function(x) x.Contains("Story")) + 1

                    If Auxiliar2(j).Count - 1 > IndiceStory Then

                        For m = IndiceStory To Auxiliar2(j).Count - 1
                            NombreBarras.Add(Auxiliar2(j)(m))
                        Next
                    Else
                        NombreBarras.Add("")

                    End If

                    Muros_lista_2(i).NombreBarras.Add(NombreBarras)

                End If

            Next
        Next

        'Longitud de Barras

        Try
            Inicio = Lista_texto.FindIndex(Function(x) x.Contains("8.Datos de alzado refuerzo longitudinal - Longitud")) + 2
            Fin = Lista_texto.FindIndex(Function(x) x.Contains("Fin")) - 2
        Catch ex As Exception
            Inicio = 0
            Fin = 0
        End Try

        For i As Integer = Inicio To Fin
            Vector_Texto = Lista_texto(i).Split(vbTab)
            Auxiliar3.Add(Vector_Texto.ToList())
        Next

        For i = 0 To Muros_lista_2.Count - 1

            For j = 0 To Auxiliar3.Count - 1
                Dim L_Barras As New List(Of Double)
                If Muros_lista_2(i).Pier_name = Auxiliar3(j)(0) Then
                    IndiceStory = Auxiliar3(j).FindIndex(Function(x) x.Contains("Story")) + 1

                    If Auxiliar3(j).Count - 1 > IndiceStory Then

                        For m = IndiceStory To Auxiliar3(j).Count - 1
                            L_Barras.Add(Val(Auxiliar3(j)(m)))
                        Next
                    Else
                        L_Barras.Add(0)

                    End If

                    Muros_lista_2(i).LongitudBarras.Add(L_Barras)

                End If

            Next
        Next

    End Sub

End Module
