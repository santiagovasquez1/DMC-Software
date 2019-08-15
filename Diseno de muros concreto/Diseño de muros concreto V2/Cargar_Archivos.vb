Imports System.IO
Module Cargar_Archivos

    Private Lista_texto As List(Of String)
    Private Lista_Similares As New List(Of String)

    Sub Cargar_Lista_Texto()

        '   Try

        Dim Carpeta As DirectoryInfo
            Dim sline, sline2 As String
            Lista_texto = New List(Of String)

            Dim Lector As New StreamReader(Ruta_1)
            Dim Ruta_Similares As String = ""

            Carpeta = New DirectoryInfo(Ruta_Carpeta)



            For Each Archivo In Carpeta.GetFiles()
                If Archivo.Name = "thesames.SDMC" Then
                    Ruta_Similares = Archivo.FullName
                End If
            Next


            If Ruta_Similares <> "" Then
                Dim Lector2 As New StreamReader(Ruta_Similares)
                Do
                    sline2 = Lector2.ReadLine()
                    Lista_Similares.Add(sline2)
                Loop Until sline2 Is Nothing

                Lector2.Close()
            Else
                MsgBox("El archivo 'thesames.SDMC' no fue encontrado.", MsgBoxStyle.Exclamation, "efe Prima Ce")
            End If










            Do
                sline = Lector.ReadLine()
                Lista_texto.Add(sline)

            Loop Until sline Is Nothing

            Lector.Close()
            Cargar_Resumen()
            Cargar_Alzado()
            Cargar_Long_Alzado()
            Cargar_Similares()
            '  Catch

        '   MsgBox("MSG1", MsgBoxStyle.Exclamation, "efe Prima Ce")

        '  End Try


    End Sub


    Sub Cargar_Similares()
        Dim ListaMuros As New List(Of String())

        For i = 0 To Lista_Similares.Count - 1
            Try
                ListaMuros.Add(Lista_Similares(i).Split(vbTab))

            Catch

            End Try
        Next

        ConfirmarMaestrosSimilares2(ListaMuros)
    End Sub








    Sub Cargar_Resumen()

        Dim Inicio, Fin As Integer
        Dim Vector_Texto As String()
        Dim Auxiliar As List(Of List(Of String)) = New List(Of List(Of String))
        Dim Muros_distintos As List(Of String)
        Dim Muro_i As Muros_Consolidados
        Dim Muro_r As Refuerzo_muros

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
            Muro_i.Calculo_H_acumulado()
            Muros_lista_2.Add(Muro_i)
        Next

        Try
            Inicio = Lista_texto.FindIndex(Function(x) x.Contains("6.Datos de Refuerzo Adicional")) + 2
            Fin = Lista_texto.FindIndex(Function(x) x.Contains("7.Datos de alzado refuerzo longitudinal")) - 2

            Dim prueba As Double = Fin - Inicio

            If prueba > 0 Then

                Dim Indice2, Indice3, indice4 As Integer
                For i = Inicio To Fin
                    Vector_Texto = Lista_texto(i).Split(vbTab)
                    Muro_r = New Refuerzo_muros
                    With Muro_r

                        .piername = Vector_Texto(0)
                        .pierstory = Vector_Texto(1)
                        .bw = Vector_Texto(2)
                        .rho = Vector_Texto(3)
                        Indice2 = Muros_lista_2.FindIndex(Function(x) x.Pier_name = .piername)
                        Indice3 = Muros_lista_2(Indice2).Stories.FindIndex(Function(x2) x2 = .pierstory)
                        .as_req = Muros_lista_2(Indice2).As_Long(Indice3)
                        .Ebe_Izq = Muros_lista_2(Indice2).Lebe_Izq(Indice3)
                        .Ebe_Der = Muros_lista_2(Indice2).Lebe_Der(Indice3)
                        .Zc_Izq = Muros_lista_2(Indice2).Zc_Izq(Indice3)
                        .Zc_Der = Muros_lista_2(Indice2).Zc_Der(Indice3)

                        For j = 5 To Vector_Texto.Count - 2 Step 2
                            .diametro.Add(Vector_Texto(j))
                            .cantidad.Add(Vector_Texto(j + 1))
                        Next

                        .Calculo_Porcentaje()

                        If refuerzo_lista.Count = 0 Or refuerzo_lista.Exists(Function(x) x.piername = .piername And x.pierstory = .pierstory) = False Then
                            refuerzo_lista.Add(Muro_r)
                        Else
                            indice4 = refuerzo_lista.FindIndex(Function(x) x.piername = .piername And x.pierstory = .pierstory)
                            refuerzo_lista(indice4) = Muro_r
                        End If

                    End With

                Next
            End If
        Catch ex As Exception

        End Try

    End Sub

    Sub Cargar_Alzado()

        Dim Inicio, Fin As Integer
        Dim Vector_Texto As String()
        Dim Auxiliar As List(Of List(Of String)) = New List(Of List(Of String))
        Dim Muro_Alzado_i As alzado_muro
        Dim Vector_auxliar As List(Of String)

        Inicio = Lista_texto.FindIndex(Function(x) x.Contains("7.Datos de alzado refuerzo longitudinal")) + 2
        Fin = Lista_texto.FindIndex(Function(x) x.Contains("8.Datos de alzado refuerzo longitudinal - Longitud")) - 1

        Dim prueba As Double = Fin - Inicio

        If prueba > 0 Then
            Vector_auxliar = Lista_texto.GetRange(Inicio, Fin - Inicio)

            For i = 0 To Vector_auxliar.Count - 1

                Vector_Texto = Vector_auxliar(i).Split(vbTab)
                Muro_Alzado_i = New alzado_muro
                With Muro_Alzado_i
                    .pier = Vector_Texto(0)
                    .story = Vector_Texto(1)
                    For j = 2 To Vector_Texto.Count - 1
                        .alzado.Add(Vector_Texto(j))
                    Next
                End With

                If alzado_lista.Count = 0 Or alzado_lista.Exists(Function(x) x.pier = Muro_Alzado_i.pier And x.story = Muro_Alzado_i.story) = False Then
                    alzado_lista.Add(Muro_Alzado_i)
                Else
                    alzado_lista(alzado_lista.FindIndex(Function(x) x.pier = Muro_Alzado_i.pier And x.story = Muro_Alzado_i.story)) = Muro_Alzado_i
                End If

            Next
        End If


    End Sub

    Sub Cargar_Long_Alzado()

        Dim Inicio, Fin As Integer
        Dim Vector_Texto As String()
        Dim Auxiliar As List(Of List(Of String)) = New List(Of List(Of String))
        Dim Vector_auxliar As List(Of String)

        Inicio = Lista_texto.FindIndex(Function(x) x.Contains("8.Datos de alzado refuerzo longitudinal - Longitud")) + 2
        Fin = Lista_texto.FindIndex(Function(x) x.Contains("Fin")) - 1

        Dim prueba As Double = Fin - Inicio

        If prueba > 0 Then

            Vector_auxliar = Lista_texto.GetRange(Inicio, Fin - Inicio)
            For i = 0 To Vector_auxliar.Count - 1

                Vector_Texto = Vector_auxliar(i).Split(vbTab)
                With alzado_lista(alzado_lista.FindIndex(Function(x) x.pier = Vector_Texto(0) And x.story = Vector_Texto(1)))
                    For j = 2 To Vector_Texto.Count - 1
                        .Alzado_Longitud.Add(Vector_Texto(j))
                    Next
                End With
            Next

        End If

    End Sub

    Public Sub ConfirmarMaestrosSimilares2(ByVal ListaMaestroSimilares As List(Of String()))




        For i = 0 To ListaMaestroSimilares.Count - 1
            If ListaMaestroSimilares(i)(3) = "True" Then
                If alzado_lista.Exists(Function(x) x.pier = ListaMaestroSimilares(i)(0)) Then
                    alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0)).MuroCreadoDespues = True

                End If
            End If
        Next






        For i = 0 To ListaMaestroSimilares.Count - 1


            'Confirmar Maestros 
            If ListaMaestroSimilares(i)(1) = "True" Then
                Muros_lista_2.Find(Function(x) x.Pier_name = ListaMaestroSimilares(i)(0)).isMuroMaestro = True

                Dim IndiceMuro As Integer = Muros_lista_2.FindIndex(Function(x) x.Pier_name = ListaMaestroSimilares(i)(0))

                For j = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1
                    If refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                        refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)).IsMuroMaestro = True
                    End If
                    If alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0) And x.story = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                        alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0) And x.story = Muros_lista_2(IndiceMuro).Stories(j)).isMuroMaestro = True
                    End If
                Next

            Else
                Muros_lista_2.Find(Function(x) x.Pier_name = ListaMaestroSimilares(i)(0)).isMuroMaestro = False

                Dim IndiceMuro As Integer = Muros_lista_2.FindIndex(Function(x) x.Pier_name = ListaMaestroSimilares(i)(0))

                For j = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1
                    If refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                        refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)).IsMuroMaestro = False
                    End If
                    If alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0) And x.story = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                        alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0) And x.story = Muros_lista_2(IndiceMuro).Stories(j)).isMuroMaestro = False
                    End If

                Next


            End If

            'Confirmar Similares

            If ListaMaestroSimilares(i)(2) <> "SinSimilar" Then

                Muros_lista_2.Find(Function(x) x.Pier_name = ListaMaestroSimilares(i)(0)).MuroSimilar = Muros_lista_2.Find(Function(x) x.Pier_name = ListaMaestroSimilares(i)(2))


                Dim IndiceMuro As Integer = Muros_lista_2.FindIndex(Function(x) x.Pier_name = ListaMaestroSimilares(i)(0))

                For j = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1
                    If refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                        refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)).MuroSimilar = refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(2) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j))


                    ElseIf refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0)) Is Nothing Then


                        For m = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1
                            If refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(2) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m)) IsNot Nothing Then
                                Dim NuevoMuroRefuerzo As New Refuerzo_muros

                                NuevoMuroRefuerzo.MuroCreadoDespues = True
                                NuevoMuroRefuerzo.piername = ListaMaestroSimilares(i)(0)
                                NuevoMuroRefuerzo.MuroSimilar = refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(2) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m))
                                NuevoMuroRefuerzo.pierstory = refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(2) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(m)).pierstory
                                refuerzo_lista.Add(NuevoMuroRefuerzo)

                            End If
                        Next

                    End If


                    If alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0) And x.story = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                        alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0) And x.story = Muros_lista_2(IndiceMuro).Stories(j)).MuroSimilar = alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(2) And x.story = Muros_lista_2(IndiceMuro).Stories(j))


                    ElseIf alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0)) Is Nothing Then


                        For m = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1
                            If alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(2) And x.story = Muros_lista_2(IndiceMuro).Stories(m)) IsNot Nothing Then

                                Dim NuevoMuroAlzado As New alzado_muro
                                Dim MuroSimilar As New alzado_muro
                                MuroSimilar = alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(2) And x.story = Muros_lista_2(IndiceMuro).Stories(m))
                                NuevoMuroAlzado.MuroCreadoDespues = True
                                NuevoMuroAlzado.pier = ListaMaestroSimilares(i)(0)
                                MuroSimilar.story = Muros_lista_2(IndiceMuro).Stories(m)
                                NuevoMuroAlzado.MuroSimilar = MuroSimilar
                                alzado_lista.Add(NuevoMuroAlzado)

                            End If
                        Next

                    End If




                Next

            Else
                Muros_lista_2.Find(Function(x) x.Pier_name = ListaMaestroSimilares(i)(0)).MuroSimilar = Nothing

                Dim IndiceMuro As Integer = Muros_lista_2.FindIndex(Function(x) x.Pier_name = ListaMaestroSimilares(i)(0))

                For j = 0 To Muros_lista_2(IndiceMuro).Stories.Count - 1

                    If refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                        If refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)).MuroCreadoDespues = True Then

                            refuerzo_lista.Remove(refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)))
                        Else
                            refuerzo_lista.Find(Function(x) x.piername = ListaMaestroSimilares(i)(0) And x.pierstory = Muros_lista_2(IndiceMuro).Stories(j)).MuroSimilar = Nothing

                        End If
                    End If



                    If alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0) And x.story = Muros_lista_2(IndiceMuro).Stories(j)) IsNot Nothing Then
                        If alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0) And x.story = Muros_lista_2(IndiceMuro).Stories(j)).MuroCreadoDespues = True Then
                            alzado_lista.Remove(alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0) And x.story = Muros_lista_2(IndiceMuro).Stories(j)))
                        Else
                            alzado_lista.Find(Function(x) x.pier = ListaMaestroSimilares(i)(0) And x.story = Muros_lista_2(IndiceMuro).Stories(j)).MuroSimilar = Nothing
                        End If
                    End If

                Next




            End If




        Next




    End Sub






End Module
