Public Class Datos_Refuerzo

    Public Nombre_muro As String
    Public Stories As List(Of String)
    Public Alzado_i As List(Of List(Of String))
    Public Coordenadas As List(Of Double())
    Public P_Texto As List(Of Double())
    Public Pc1 As List(Of Double())
    Public Pc2 As List(Of Double())
    Public Longitud_barra As Single
    Public Lista_P_Texto As List(Of List(Of Double()))
    Public Lista_Coordenadas As List(Of List(Of Double()))
    Public Lista_Pc1 As List(Of List(Of Double()))
    Public Lista_Pc2 As List(Of List(Of Double()))
    Public Barra As List(Of String)
    Public Lista_Barras As List(Of List(Of String))


    Public Lista_Longitudes As New List(Of List(Of Single))
    Public Lista_Figura As New List(Of List(Of String))
    Public ListaDiametros As New List(Of List(Of String))
    Public ListaCantidadBarras As New List(Of List(Of Integer))
    Public ListaBarrasConcatenadas As New List(Of List(Of String))
    Public ListaTotalRefuerzo_PorMuro As New List(Of String)
    Public ListaTotalRefuerzo_PorMuro_Cantidad As New List(Of Integer)
    Public ListaTotalRefuerzo_EspecificadoConCantidad As New List(Of String)
    Public ListaRefuerzo_DllNet As New List(Of String)


    Public Sub Load_Coordinates(ByVal Muro_i As String, ByRef pos_x As Double)

        Dim Lista_i As List(Of alzado_muro)
        Dim indice As Integer
        Dim Pos1, Pos2 As Double
        Dim Altura, delta As Double
        Dim Coor_Aux As Double()
        Dim prueba As Muros_Consolidados

        Dim texto1, texto2 As String
        Dim Pos_T1, Pos_T2, Diametro As Integer

        Stories = New List(Of String)
        Alzado_i = New List(Of List(Of String))
        Barra = New List(Of String)

        Lista_Coordenadas = New List(Of List(Of Double()))
        Lista_P_Texto = New List(Of List(Of Double()))
        Lista_Pc1 = New List(Of List(Of Double()))
        Lista_Pc2 = New List(Of List(Of Double()))

        Lista_Barras = New List(Of List(Of String))
        Lista_i = alzado_lista.FindAll(Function(x) x.pier = Muro_i).ToList.OrderBy(Function(x) Convert.ToInt16(x.story.Substring(5))).ToList

        Stories.AddRange(Lista_i.Select(Function(x) x.story).ToList)
        Alzado_i.AddRange(Lista_i.Select(Function(x) x.alzado).ToList)

        prueba = Muros_lista_2.Find(Function(x) x.Pier_name = Nombre_muro)

        For i = 0 To Alzado_i(0).Count - 1

            Altura = 0
            Coordenadas = New List(Of Double())
            Barra = New List(Of String)
            P_Texto = New List(Of Double())
            Pc1 = New List(Of Double())
            Pc2 = New List(Of Double())

            For j = 0 To Alzado_i.Count - 1

                indice = prueba.Stories.FindIndex(Function(x1) x1 = Stories(j))

                If Alzado_i(j)(i) = Nothing Then
                    Alzado_i(j)(i) = ""
                End If

                If Alzado_i(j)(i).Contains("T") = True Then

                    Pos_T1 = Alzado_i(j)(i).IndexOf("T")
                    Pos_T2 = Alzado_i(j)(i).IndexOf("#")
                    Diametro = Alzado_i(j)(i).Substring(Pos_T2 + 1, Pos_T1 - Pos_T2 - 1)

                    If j = 0 Then
                        If Alzado_i(j)(i).Contains("T1") = True Then

                            Caso_T1(Pos1, Pos2, pos_x, delta, Coor_Aux, Diametro, i, j)
                            If delta = 0 Then
                                delta = 0.05
                            Else
                                delta = 0
                            End If
                        End If
                    Else
                        If Alzado_i(j)(i).Contains("T1") = True And Alzado_i(j - 1)(i).Contains("T2") = False Then
                            Caso_T1(Pos1, Pos2, pos_x, delta, Coor_Aux, Diametro, i, j)
                            If delta = 0 Then
                                delta = 0.05
                            Else
                                delta = 0
                            End If
                        End If

                    End If

                    If Alzado_i(j)(i).Contains("T2") = True Then

                        Find_pos_final(i, j, Pos1, Pos2, Alzado_i.Count - 1, 1, pos_x + delta)

                        If Math.Round(Pos2 + Hviga, 2) = Math.Round(prueba.H_acumulado(0) / 100, 2) Then
                            Coor_Aux = {pos_x + delta, Pos1, pos_x + delta, Pos2 + Hviga - 0.05, pos_x + delta + ganchos_90(Diametro), Pos2 + Hviga - 0.05}
                        Else
                            Coor_Aux = {pos_x + delta, Pos1, pos_x + delta, Pos2}
                        End If

                        Barra.Add(Alzado_i(j)(i))
                        Coordenadas.Add(Coor_Aux)

                        If delta = 0 Then
                            delta = 0.05
                        Else
                            delta = 0
                        End If
                    End If

                    If Alzado_i(j)(i).Contains("T3") = True Then
                        Find_pos_T3(i, j, Pos1, Pos2, 0, -1, pos_x + delta, Coor_Aux)
                        Barra.Add(Alzado_i(j)(i))
                        Coordenadas.Add(Coor_Aux)

                    End If

                ElseIf Alzado_i(j)(i) <> "" Then

                    If j < Alzado_i.Count - 1 Then
                        texto1 = Alzado_i(j)(i)
                        texto2 = Alzado_i(j + 1)(i)

                        If texto2 = "" Then
                            If j > 0 Then

                                If texto2 = "" And Alzado_i(j - 1)(i).Contains("T1") = True OrElse texto2 = "" And Alzado_i(j - 1)(i) = "" Then
                                    Find_pos_final(i, j, Pos1, Pos2, 0, -1, pos_x + delta)
                                    Coor_Aux = {pos_x + delta, Pos1, pos_x + delta, Pos2}

                                    Barra.Add(Alzado_i(j)(i))
                                    Coordenadas.Add(Coor_Aux)

                                    If delta = 0 Then
                                        delta = 0.05
                                    Else
                                        delta = 0
                                    End If


                                End If
                            End If

                        ElseIf texto2.Contains("T2") = True And Coordenadas.Count = 0 Then

                            Pos_T2 = Alzado_i(j)(i).IndexOf("#")
                            Diametro = Alzado_i(j)(i).Substring(Pos_T2 + 1)
                            Find_Pos_2(i, j, Pos1, Pos2, 0, -1, pos_x + delta)

                            If Pos1 = 0 Then
                                Coor_Aux = {pos_x + delta + ganchos_90(Diametro), Pos1 - prof, pos_x + delta, Pos1 - prof, pos_x + delta, Pos2 - Hviga}
                            Else
                                Coor_Aux = {pos_x + delta + ganchos_90(Diametro), Pos1 - 0.05, pos_x + delta, Pos1 - 0.05, pos_x + delta, Pos2 - Hviga}
                            End If

                            Barra.Add(Alzado_i(j)(i))
                            Coordenadas.Add(Coor_Aux)

                            If delta = 0 Then
                                delta = 0.05
                            Else
                                delta = 0
                            End If

                        ElseIf texto2 <> "" And texto2.Contains("T") = False And texto1 = texto2 And Coordenadas.Count > 0 Then

                            Find_Pos_2(i, j, Pos1, Pos2, Alzado_i.Count - 1, 1, pos_x + delta)

                            If j + 1 = Alzado_i.Count - 1 Then
                                Coor_Aux = {pos_x + delta, Pos1, pos_x + delta, Pos2 - 0.05, pos_x + delta + ganchos_90(Diametro), Pos2 - 0.05}
                            Else
                                Coor_Aux = {pos_x + delta, Pos1, pos_x + delta, Pos2}
                            End If

                            Barra.Add(Alzado_i(j)(i))
                            Coordenadas.Add(Coor_Aux)

                            If delta = 0 Then
                                delta = 0.05
                            Else
                                delta = 0
                            End If
                        End If

                    Else

                        texto1 = Alzado_i(j)(i)
                        texto2 = Alzado_i(j - 1)(i)

                        If texto2.Contains("T1") = True Then
                            Find_Pos_2(i, j, Pos1, Pos2, Alzado_i.Count - 1, 1, pos_x + delta)
                            Coor_Aux = {pos_x + delta, Pos1, pos_x + delta, Pos2 - 0.05, pos_x + delta + ganchos_90(Diametro), Pos2 - 0.05}

                            Barra.Add(Alzado_i(j)(i))
                            Coordenadas.Add(Coor_Aux)

                            If delta = 0 Then
                                delta = 0.05
                            Else
                                delta = 0
                            End If
                        End If


                    End If

                End If

                Altura += prueba.Hw(indice) / 100
            Next

            delta = 0
            pos_x += 0.55

            Lista_P_Texto.Add(P_Texto)
            Lista_Barras.Add(Barra)
            Lista_Coordenadas.Add(Coordenadas)
            Lista_Pc1.Add(Pc1)
            Lista_Pc2.Add(Pc2)
        Next

        pos_x += 5.1
    End Sub

    Private Sub Add_Text_Point(ByVal Pos_x As Single, ByVal Pos_y As Single)

        P_Texto.Add({Pos_x - 0.2, Pos_y, 0})

    End Sub

    Private Sub Caso_T1(ByRef Pos1 As Double, ByRef Pos2 As Double, Pos_X As Double, delta As Double, ByRef Coor_Aux() As Double, Diametro As Integer, i As Integer, j As Integer)
        Find_pos_final(i, j, Pos1, Pos2, 0, -1, Pos_X + delta)

        If Pos1 = 0 Then
            Coor_Aux = {Pos_X + delta + ganchos_90(Diametro), Pos1 - prof, Pos_X + delta, Pos1 - prof, Pos_X + delta, Pos2}
        Else
            Coor_Aux = {Pos_X + delta, Pos1, Pos_X + delta, Pos2}
        End If

        Barra.Add(Alzado_i(j)(i))
        Coordenadas.Add(Coor_Aux)
    End Sub

    Private Sub Find_pos_T3(ByVal col As Integer, ByVal fila As Integer, ByRef Pos1 As Double, ByRef Pos2 As Double, ByVal Fin As Integer, ByVal Paso As Integer, ByVal Posx As Double, ByRef Coor_Aux() As Double)

        Dim Valor_inicial, Valor_Final As String
        Dim Pos_T1, Pos_T2, Caso As Integer
        Dim indice1, indice2, x1 As Integer
        Dim suma As Double
        Dim Texto_1, Texto_2 As String
        Dim Diametro1 As Integer

        Valor_inicial = Alzado_i(fila)(col)
        Pos_T1 = Alzado_i(fila)(col).IndexOf("T")
        Pos_T2 = Alzado_i(fila)(col).IndexOf("#")
        x1 = 0
        Caso = 1
        Texto_1 = Alzado_i(fila)(col).Substring(Pos_T1)

        For i = fila To Fin Step Paso

            indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(i))
            If Alzado_i(i)(col).Contains("T") = True And i <> fila OrElse Alzado_i(i)(col) = "" And i <> fila Then

                Valor_Final = Alzado_i(i)(col)
                If Valor_Final <> "" Then
                    Pos_T1 = Valor_Final.IndexOf("T")
                    Texto_2 = Valor_Final.Substring(Pos_T1)
                    If Texto_1 <> Texto_2 Then
                        suma += Muros_lista_2(indice1).Hw(indice2) / 100
                        Caso = 2
                    Else
                        If Texto_2.Contains("T1") Then
                            Caso = 1
                        Else
                            Caso = 3
                        End If

                    End If
                Else
                    Caso = 2
                End If
                x1 = i
                Exit For
            Else
                suma += Muros_lista_2(indice1).Hw(indice2) / 100
            End If
        Next

        If x1 = 0 Then
            Caso = 3
        End If


        If Paso < 0 And Caso = 1 Then

            indice1 = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Nombre_muro)
            indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(fila))

            Pos_T1 = Alzado_i(fila)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila)(col).IndexOf("#")
            Diametro1 = Find_Diametro(col, fila, Pos_T1, Pos_T2)

            Pos2 = (Muros_lista_2(indice1).H_acumulado(indice2) / 100) - 0.5
            Pos1 = Pos2 - suma + 0.5

            Coor_Aux = {Posx, Pos1, Posx, Pos2, Posx + ganchos_90(Diametro1), Pos2}
            Add_Text_Point(Posx, Pos1 + 0.9)

        ElseIf Paso < 0 And Caso = 3 Then

            indice1 = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Nombre_muro)
            indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(fila))

            Pos_T1 = Alzado_i(fila)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila)(col).IndexOf("#")
            Diametro1 = Find_Diametro(col, fila, Pos_T1, Pos_T2)

            Pos2 = (Muros_lista_2(indice1).H_acumulado(indice2) / 100) - 0.5
            If Pos2 - suma + 0.5 > 0 Then
                Pos1 = Pos2 - suma + 0.45
            Else
                Pos1 = Pos2 - suma + 0.5 - prof
            End If

            Coor_Aux = {Posx + ganchos_90(Diametro1), Pos1, Posx, Pos1, Posx, Pos2, Posx + ganchos_90(Diametro1), Pos2}
            Add_Text_Point(Posx, Pos1 + 0.9)
        End If

    End Sub

    Private Sub Find_pos_final(ByVal col As Integer, ByVal fila As Integer, ByRef Pos1 As Double, ByRef Pos2 As Double, ByVal Fin As Integer, ByVal Paso As Integer, ByVal Posx As Double)

        Dim suma As Double
        Dim indice1, indice2, x1 As Integer
        Dim Valor_inicial As String
        Dim Pos_T1, Pos_T2 As Integer
        Dim Texto_1, Texto_2 As String
        Dim traslapo_i As Double
        Dim Fc_aux As Double
        Dim Diametro1, Diametro2 As Integer
        Dim Caso As Integer
        Dim Valor_Final As String
        Dim Pc1_aux As Double()
        Dim Pc2_aux As Double()
        Dim Traslapo1, Traslapo2, Traslapo1_1, Traslapo2_2 As Double
        Dim Fc1, Fc2 As Double

        Valor_inicial = Alzado_i(fila)(col)
        Pos_T1 = Alzado_i(fila)(col).IndexOf("T")
        Pos_T2 = Alzado_i(fila)(col).IndexOf("#")
        Caso = 1
        Valor_Final = ""

        If Pos_T1 < 0 Then
            Texto_1 = "T1"
            Diametro1 = Alzado_i(fila)(col).Substring(Pos_T2 + 1)
        Else
            Texto_1 = Alzado_i(fila)(col).Substring(Pos_T1)
            Diametro1 = Alzado_i(fila)(col).Substring(Pos_T2 + 1, Pos_T1 - Pos_T2 - 1)
        End If

        For i = fila To Fin Step Paso

            indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(i))
            If Alzado_i(i)(col).Contains("T") = True And i <> fila OrElse Alzado_i(i)(col) = "" And i <> fila Then
                Valor_Final = Alzado_i(i)(col)
                If Valor_Final <> "" Then
                    Pos_T1 = Valor_Final.IndexOf("T")
                    Texto_2 = Valor_Final.Substring(Pos_T1)
                    If Texto_1 <> Texto_2 Then
                        suma += Muros_lista_2(indice1).Hw(indice2) / 100
                        Caso = 2
                    Else
                        Caso = 1
                    End If
                Else
                    Caso = 2
                End If

                x1 = i
                Exit For
            Else
                suma += Muros_lista_2(indice1).Hw(indice2) / 100
            End If
            x1 = i
        Next

        If Paso > 0 And Caso = 1 Then

            indice1 = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Nombre_muro)
            indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(fila))

            Fc_aux = Muros_lista_2(indice1).fc(indice2 + 1)

            Pos_T1 = Alzado_i(fila)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila)(col).IndexOf("#")

            Diametro1 = Find_Diametro(col, fila, Pos_T1, Pos_T2)

            Pos_T1 = Alzado_i(fila - 1)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila - 1)(col).IndexOf("#")
            Diametro2 = Find_Diametro(col, fila - 1, Pos_T1, Pos_T2)

            Traslapo1 = Traslapo(Fc_aux, Diametro1)
            Traslapo2 = Traslapo(Fc_aux, Diametro2)

            If Traslapo1 > Traslapo2 Then
                traslapo_i = Traslapo1
            Else
                traslapo_i = Traslapo2
            End If

            Pos1 = ((Muros_lista_2(indice1).H_acumulado(indice2) / 100) - Muros_lista_2(indice1).Hw(indice2) / 100) - traslapo_i - Hviga
            Pos2 = Pos1 + suma + traslapo_i

            If traslapo_i > 0 Then
                Pc1_aux = {Posx, Pos1, 0}
                Pc2_aux = {Posx, Pos1 + traslapo_i, 0}
                Pc1.Add(Pc1_aux)
                Pc2.Add(Pc2_aux)
            End If

            Add_Text_Point(Posx, Pos1 + 0.9)

        ElseIf Paso > 0 And Caso = 2 Then

            indice1 = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Nombre_muro)
            indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(fila))

            Fc1 = Muros_lista_2(indice1).fc(indice2 + 1)

            Pos_T1 = Alzado_i(fila)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila)(col).IndexOf("#")
            Diametro1 = Find_Diametro(col, fila, Pos_T1, Pos_T2)

            Pos_T1 = Alzado_i(fila - 1)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila - 1)(col).IndexOf("#")
            Diametro2 = Find_Diametro(col, fila - 1, Pos_T1, Pos_T2)

            Traslapo1 = Traslapo(Fc1, Diametro1)
            Traslapo2 = Traslapo(Fc1, Diametro2)

            If Traslapo1 > Traslapo2 Then
                Traslapo1_1 = Traslapo1
            Else
                Traslapo1_1 = Traslapo2
            End If


            Fc2 = Muros_lista_2(indice1).fc(indice2 - 1)

            Pos_T1 = Alzado_i(fila)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila)(col).IndexOf("#")
            Diametro1 = Find_Diametro(col, fila, Pos_T1, Pos_T2)

            Pos_T1 = Alzado_i(fila + 1)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila + 1)(col).IndexOf("#")
            Diametro2 = Find_Diametro(col, fila + 1, Pos_T1, Pos_T2)

            Traslapo1 = Traslapo(Fc2, Diametro1)
            Traslapo2 = Traslapo(Fc2, Diametro2)

            If Traslapo1 > Traslapo2 Then
                Traslapo2_2 = Traslapo1
            Else
                Traslapo2_2 = Traslapo2
            End If

            Pos1 = ((Muros_lista_2(indice1).H_acumulado(indice2) / 100) - Muros_lista_2(indice1).Hw(indice2) / 100) - Traslapo1_1 - Hviga
            Pos2 = Pos1 + suma + Traslapo1_1 + Hviga + Traslapo2_2
            Add_Text_Point(Posx, Pos1 + 0.9)

            Pc1_aux = {Posx, Pos1, 0}
            Pc2_aux = {Posx, Pos1 + Traslapo1_1, 0}

            Pc1.Add(Pc1_aux)
            Pc2.Add(Pc2_aux)

            Pc1_aux = {Posx, Pos2 - Traslapo2_2, 0}
            Pc2_aux = {Posx, Pos2, 0}

            Pc1.Add(Pc1_aux)
            Pc2.Add(Pc2_aux)


        Else

            indice1 = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Nombre_muro)
            indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(fila))

            Fc_aux = Muros_lista_2(indice1).fc(indice2 - 1)

            Pos_T1 = Alzado_i(fila)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila)(col).IndexOf("#")
            Diametro1 = Find_Diametro(col, fila, Pos_T1, Pos_T2)

            Pos_T1 = Alzado_i(fila + 1)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila + 1)(col).IndexOf("#")
            Diametro2 = Find_Diametro(col, fila + 1, Pos_T1, Pos_T2)

            Traslapo1 = Traslapo(Fc_aux, Diametro1)
            Traslapo2 = Traslapo(Fc_aux, Diametro2)

            If Traslapo1 > Traslapo2 Then
                traslapo_i = Traslapo1
            Else
                traslapo_i = Traslapo2
            End If

            Pos2 = (Muros_lista_2(indice1).H_acumulado(indice2) / 100) + traslapo_i
            Pos1 = Pos2 - suma - traslapo_i

            If traslapo_i > 0 Then
                Pc1_aux = {Posx, Muros_lista_2(indice1).H_acumulado(indice2) / 100, 0}
                Pc2_aux = {Posx, Pc1_aux(1) + traslapo_i, 0}
                Pc1.Add(Pc1_aux)
                Pc2.Add(Pc2_aux)
            End If

            Add_Text_Point(Posx, Pos1 + 0.9)

        End If

    End Sub

    Private Function Find_Diametro(col As Integer, i As Integer, Pos_T1 As Integer, Pos_T2 As Integer) As Integer

        Dim Diametro_i As Integer

        If Alzado_i(i)(col) <> "" Then
            If Pos_T1 < 0 Then
                Diametro_i = Alzado_i(i)(col).Substring(Pos_T2 + 1)
            Else
                Diametro_i = Alzado_i(i)(col).Substring(Pos_T2 + 1, Pos_T1 - Pos_T2 - 1)
            End If
        Else
            Diametro_i = 0
        End If

        Return Diametro_i
    End Function

    Private Sub Find_Pos_2(ByVal col As Integer, ByVal fila As Integer, ByRef Pos1 As Double, ByRef Pos2 As Double, ByVal Fin As Integer, ByVal Paso As Integer, ByVal Posx As Double)
        Dim suma As Double
        Dim indice1, indice2, Diametro, Diametro1, Diametro2, i As Integer
        Dim Valor_inicial As String
        Dim Caso As Integer
        Dim Fc_Aux, Traslapo_i, Traslapo1, Traslapo2 As Single
        Dim Pos_T1, Pos_T2 As Integer
        Dim Pc1_aux As Double()
        Dim Pc2_aux As Double()
        Dim prueba As String

        Valor_inicial = Alzado_i(fila)(col)
        Pos_T1 = Alzado_i(fila)(col).IndexOf("T")
        Pos_T2 = Alzado_i(fila)(col).IndexOf("#")

        If Pos_T1 < 0 Then
            Diametro = Alzado_i(fila)(col).Substring(Pos_T2 + 1)
        Else
            Diametro = Alzado_i(fila)(col).Substring(Pos_T2 + 1, Pos_T1 - Pos_T2 - 1)
        End If

        For i = fila To Fin Step Paso

            If Alzado_i(i)(col) <> Alzado_i(fila)(col) Then
                prueba = Alzado_i(i)(col)
                If prueba = "" Then
                    Caso = 1
                Else
                    Caso = 2
                End If

                Exit For
            Else
                indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(i))
                suma += Muros_lista_2(indice1).Hw(indice2) / 100
            End If

        Next

        indice1 = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Nombre_muro)
        indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(fila))

        If i - 1 = Fin Then
            Caso = 3
        End If

        If Caso = 1 Then

            Fc_Aux = Muros_lista_2(indice1).fc(indice2 - 1)

            Pos_T1 = Alzado_i(fila)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila)(col).IndexOf("#")

            Diametro1 = Find_Diametro(col, fila, Pos_T1, Pos_T2)

            Pos_T1 = Alzado_i(fila - 1)(col).IndexOf("T")
            Pos_T2 = Alzado_i(fila - 1)(col).IndexOf("#")
            Diametro2 = Find_Diametro(col, fila - 1, Pos_T1, Pos_T2)

            Traslapo1 = Traslapo(Fc_Aux, Diametro1)
            Traslapo2 = Traslapo(Fc_Aux, Diametro2)

            If Traslapo1 > Traslapo2 Then
                Traslapo_i = Traslapo1
            Else
                Traslapo_i = Traslapo2
            End If

            Traslapo_i = Traslapo(Fc_Aux, Diametro)

            Pos1 = ((Muros_lista_2(indice1).H_acumulado(indice2) / 100) - Muros_lista_2(indice1).Hw(indice2) / 100)
            Pos2 = Pos1 + suma + Traslapo_i

            Pc1_aux = {Posx, Pos2 - Traslapo_i, 0}
            Pc2_aux = {Posx, Pc1_aux(1) + Traslapo_i, 0}
            Pc1.Add(Pc1_aux)
            Pc2.Add(Pc2_aux)

            Add_Text_Point(Posx, Pos1 + 0.9)
        ElseIf Caso = 2 Then

            Pos1 = ((Muros_lista_2(indice1).H_acumulado(indice2) / 100) - Muros_lista_2(indice1).Hw(indice2) / 100)
            Pos2 = Pos1 + suma - Hviga

            Add_Text_Point(Posx, Pos1 + 0.9)

        ElseIf Caso = 3 Then

            Pos1 = ((Muros_lista_2(indice1).H_acumulado(indice2) / 100) - Muros_lista_2(indice1).Hw(indice2) / 100)
            Pos2 = Pos1 + suma
            Add_Text_Point(Posx, Pos1 + 0.9)
        ElseIf Caso = 0 Then

            Pos2 = (Muros_lista_2(indice1).H_acumulado(indice2) / 100)
            Pos1 = Pos2 - suma
            Add_Text_Point(Posx, Pos1 + 0.9)
        End If

    End Sub



    '*******NUEVAS FUNCIONES *******


    Sub ActivarNuevasFunciones()
        CalcularLongitudesBarras()
        Barra_Recta_L_C()
        DiametroBarras_CantidadBarras()
        ConcatenarBarras()
        RefuerzoTotal()
        RefuerzoFinal()
        RefuerzoDllNet()
    End Sub


    Sub RefuerzoDllNet()

        For i = 0 To ListaTotalRefuerzo_EspecificadoConCantidad.Count - 1

            Dim Nomenclatura As String = ListaTotalRefuerzo_EspecificadoConCantidad(i)
            Dim NomenclaturaFinal As String = ""

            Dim FiguraBarras As String = Nomenclatura.Chars(Len(Nomenclatura) - 1)
            Dim PosicionFinalCantidad As Integer : Dim CantidadBarras As Integer
            Dim PosicionDiametro, DiametroA As Integer
            Dim PosicionInicalLongitud As Integer : Dim Longitud As Single
            Dim Gancho As Single

            For n = 0 To Len(Nomenclatura) - 1
                If Nomenclatura.Chars(n) = "#" Then
                    PosicionFinalCantidad = n
                    PosicionDiametro = n + 1
                End If
                If Nomenclatura.Chars(n) = "=" Then
                    PosicionInicalLongitud = n + 2
                End If
            Next
            CantidadBarras = Val(Nomenclatura.Substring(0, PosicionFinalCantidad))
            Longitud = Val(Nomenclatura.Substring(PosicionInicalLongitud, 3))


            Try
                If Nomenclatura.Chars(PosicionDiametro + 1) = "-" Then
                    DiametroA = Val(Nomenclatura.Chars(PosicionDiametro))
                Else
                    DiametroA = Val(Nomenclatura.Chars(PosicionDiametro) & Nomenclatura.Chars(PosicionDiametro + 1))
                End If
            Catch
                DiametroA = Val(Nomenclatura.Chars(PosicionDiametro))
            End Try

            Gancho = ganchos_90(DiametroA)


            If FiguraBarras = "R" Then
                NomenclaturaFinal = CantidadBarras & " " & NoBarraADiametro(DiametroA) & " " & Format(Longitud, "0.00")
            End If

            If FiguraBarras = "L" Then
                Longitud = Longitud - Gancho
                NomenclaturaFinal = CantidadBarras & " " & NoBarraADiametro(DiametroA) & " " & Format(Longitud, "0.00") & " " & "L" & Format(Gancho, "0.000")
            End If
            If FiguraBarras = "C" Then
                Longitud = Longitud - 2 * Gancho
                NomenclaturaFinal = CantidadBarras & " " & NoBarraADiametro(DiametroA) & " " & Format(Longitud, "0.00") & " " & "L" & Format(Gancho, "0.000") & " " & "L" & Format(Gancho, "0.000")
            End If
            ListaRefuerzo_DllNet.Add(NomenclaturaFinal)


        Next




    End Sub



    Sub RefuerzoFinal()

        Dim VectoIndices As New List(Of Integer)


        For i = 0 To ListaTotalRefuerzo_PorMuro.Count - 1
            Dim Cantidad As Integer = ListaTotalRefuerzo_PorMuro_Cantidad(i)


            If VectoIndices.Exists(Function(x) x = i) = False Then

                For j = i + 1 To ListaTotalRefuerzo_PorMuro.Count - 1

                    If ListaTotalRefuerzo_PorMuro(i) = ListaTotalRefuerzo_PorMuro(j) Then
                        Cantidad = Cantidad + ListaTotalRefuerzo_PorMuro_Cantidad(j)
                        VectoIndices.Add(j)
                    End If
                Next
                ListaTotalRefuerzo_EspecificadoConCantidad.Add(Cantidad & ListaTotalRefuerzo_PorMuro(i))
            End If

        Next






    End Sub




    Sub RefuerzoTotal()

        For i = 0 To ListaBarrasConcatenadas.Count - 1
            For j = 0 To ListaBarrasConcatenadas(i).Count - 1
                ListaTotalRefuerzo_PorMuro.Add(ListaBarrasConcatenadas(i)(j))
                ListaTotalRefuerzo_PorMuro_Cantidad.Add(ListaCantidadBarras(i)(j))
            Next
        Next


    End Sub


    Sub ConcatenarBarras()
        For i = 0 To ListaDiametros.Count - 1
            Dim ListaConcate As New List(Of String)
            For j = 0 To ListaDiametros(i).Count - 1
                Dim TextConca As String
                TextConca = "#" & ListaDiametros(i)(j) & "-" & "L=" & Str(Lista_Longitudes(i)(j)) & "-" & Lista_Figura(i)(j)
                ListaConcate.Add(TextConca)
            Next
            ListaBarrasConcatenadas.Add(ListaConcate)
        Next


    End Sub



    Sub DiametroBarras_CantidadBarras()

        For j = 0 To Lista_Barras.Count - 1
            Dim ListaDiametros1 As New List(Of String)
            Dim ListaCantidadBarras1 As New List(Of Integer)
            For s = 0 To Lista_Barras(j).Count - 1
                Dim PosicionDiametro, PosicionFinalCantidad As Integer
                Dim DiametroAux As String : Dim CantidadBarras As Integer
                For n = 0 To Len(Lista_Barras(j)(s)) - 1

                    If Lista_Barras(j)(s).Chars(n) = "#" Then
                        PosicionFinalCantidad = n
                        PosicionDiametro = n + 1
                    End If
                Next
                CantidadBarras = Val(Lista_Barras(j)(s).Substring(0, PosicionFinalCantidad))

                Try
                    If Lista_Barras(j)(s).Chars(PosicionDiametro + 1) = "T" Then
                        DiametroAux = Lista_Barras(j)(s).Chars(PosicionDiametro)
                    Else
                        DiametroAux = Lista_Barras(j)(s).Chars(PosicionDiametro) & Lista_Barras(j)(s).Chars(PosicionDiametro + 1)
                    End If
                Catch
                    DiametroAux = Lista_Barras(j)(s).Chars(PosicionDiametro)
                End Try
                ListaCantidadBarras1.Add(CantidadBarras)
                ListaDiametros1.Add(DiametroAux)
            Next
            ListaDiametros.Add(ListaDiametros1)
            ListaCantidadBarras.Add(ListaCantidadBarras1)
        Next

    End Sub





    Sub Barra_Recta_L_C()


        For j = 0 To Lista_Coordenadas.Count - 1
            Dim ListaBarras As New List(Of String)
            For s = 0 To Lista_Coordenadas(j).Count - 1

                If Lista_Coordenadas(j)(s).Count = 4 Then
                    ListaBarras.Add("R")
                End If
                If Lista_Coordenadas(j)(s).Count = 6 Then
                    ListaBarras.Add("L")
                End If
                If Lista_Coordenadas(j)(s).Count = 8 Then
                    ListaBarras.Add("C")
                End If
            Next
            Lista_Figura.Add(ListaBarras)
        Next

    End Sub






    Sub CalcularLongitudesBarras()
        For i = 0 To Lista_Coordenadas.Count - 1
            Dim ListaLong As New List(Of Single)
            For j = 0 To Lista_Coordenadas(i).Count - 1
                Dim Longitu = Calcular_Longitud(Lista_Coordenadas(i)(j))
                ListaLong.Add(Longitu)
            Next
            Lista_Longitudes.Add(ListaLong)
        Next


    End Sub

    Private Function Calcular_Longitud(ByVal Coordenadas As Double()) As Single

        Dim Longitud As Single
        Dim Distancia_i As Single
        Dim X_barra, Y_barra As Single

        For i = 0 To Coordenadas.Count - 3 Step 2

            X_barra = Math.Abs(Coordenadas(i) - Coordenadas(i + 2))
            Y_barra = Math.Abs(Coordenadas(i + 1) - Coordenadas(i + 3))
            Distancia_i = X_barra + Y_barra
            Longitud += Distancia_i

        Next
        Return Longitud
    End Function


    '*******FIN - NUEVAS FUNCIONES *******










    Private Sub Find_Pos_3(ByVal col As Integer, ByVal fila As Integer, ByRef Pos1 As Double, ByRef Pos2 As Double, ByVal Fin As Integer, ByVal Paso As Integer, ByVal Posx As Double)



    End Sub

    Private Function Find_Fc(ByVal Pos1 As Integer, ByVal Piso As String) As Single

        Dim Fc As Single
        Dim indice1, indice2 As Integer
        Dim prueba As String

        indice1 = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Nombre_muro)
        indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(Pos1))
        prueba = Muros_lista_2(indice1).Stories(indice2)

        If Piso = "Arriba" Then
            indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(Pos1 + 1))
            Fc = Muros_lista_2(indice1).fc(indice2)
        Else
            indice2 = Muros_lista_2(indice1).Stories.FindIndex(Function(x) x = Stories(Pos1 - 1))
            Fc = Muros_lista_2(indice1).fc(indice2)
        End If

        Return Fc
    End Function

    Private Function Traslapo(fc As Double, diametro As Integer) As Single
        Traslapo = 0
        Try
            If fc = 560 Then
                Traslapo = traslapo_560(diametro)
            End If

            If fc = 490 Then
                Traslapo = traslapo_490(diametro)
            End If

            If fc = 420 Then
                Traslapo = traslapo_420(diametro)
            End If

            If fc = 350 Then
                Traslapo = traslapo_350(diametro)
            End If

            If fc = 280 Then
                Traslapo = traslapo_280(diametro)
            End If

            If fc = 210 Then
                Traslapo = traslapo_210(diametro)
            End If
        Catch ex As Exception
            Traslapo = 0
        End Try


    End Function



End Class
