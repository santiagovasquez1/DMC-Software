Imports Autodesk.AutoCAD.Interop.Common

Public Class Crear_Estribos

    Public Bloque_Estribo As AcadBlockReference
    Public Bloque_Gancho As AcadBlockReference

    Sub Determinar_Estribos(ByVal Formulario As Seccion)

        Dim Suma_Long, delta As Single
        Dim Punto_inicial As Double()
        Dim Punto_final As Double()
        Dim Muro_i As Muros_Consolidados
        Dim indice, indice2, Pos, Num_Estribos As Integer
        Dim Distancia_Limite, Distancia_Maxima As Double
        Dim Delta_Escalado As Double
        Dim Puntos_Limites As List(Of Double())
        Dim Confinamiento_Izq, Confinamiento_Der As Double

        Dim Vecino_Izq, Vecino_Der, Vecino_Abajo, Vecino_Arriba As Boolean
        Dim bw_vecino_izq, bw_Vecino_der As Single

        Distancia_Maxima = 2.0

        For i = 0 To ListaOrdenada.Count - 1
            Puntos_Limites = New List(Of Double())
            bw_vecino_izq = 0
            bw_Vecino_der = 0

            If ListaOrdenada(i).DireccionMuro = "Horizontal" Then

                Muro_i = Muros_lista_2.Find(Function(x) x.Pier_name = ListaOrdenada(i).NombreMuro)
                indice = Muro_i.Stories.FindIndex(Function(x) x = "Story" & Formulario.Piso_Box.Text)

                If ListaOrdenada(i).MurosVecinosIzquierda.Count > 0 Then
                    Punto_inicial = {ListaOrdenada(i).MurosVecinosIzquierda(0).Xmin + 0.038, ListaOrdenada(i).Ymin + (ListaOrdenada(i).Ymax - ListaOrdenada(i).Ymin) / 2, 0}
                    Vecino_Izq = True
                    bw_vecino_izq = ListaOrdenada(i).MurosVecinosIzquierda(0).Xmax - ListaOrdenada(i).MurosVecinosIzquierda(0).Xmin
                Else
                    Punto_inicial = {ListaOrdenada(i).Xmin + 0.038, ListaOrdenada(i).Ymin + (ListaOrdenada(i).Ymax - ListaOrdenada(i).Ymin) / 2, 0}
                    Vecino_Izq = False
                End If

                If ListaOrdenada(i).MurosVecinosDerecha.Count > 0 Then
                    Punto_final = {ListaOrdenada(i).MurosVecinosDerecha(0).Xmax - 0.038, ListaOrdenada(i).Ymin + (ListaOrdenada(i).Ymax - ListaOrdenada(i).Ymin) / 2, 0}
                    Vecino_Der = True
                    bw_Vecino_der = ListaOrdenada(i).MurosVecinosDerecha(0).Xmax - ListaOrdenada(i).MurosVecinosDerecha(0).Xmin
                Else
                    Punto_final = {ListaOrdenada(i).Xmax - 0.038, ListaOrdenada(i).Ymin + (ListaOrdenada(i).Ymax - ListaOrdenada(i).Ymin) / 2, 0}
                    Vecino_Der = False
                End If

                Suma_Long = 0
                Delta_Escalado = 0
                Pos = 0

                ''Caso en el cual el muro no va totalmente confinado

                If Muro_i.Lebe_Izq(indice) > 0 And Muro_i.Lebe_Izq(indice) < Muro_i.lw(indice) OrElse Muro_i.Zc_Izq(indice) > 0 Then

                    Dim prueba1, prueba2 As List(Of Double)
                    Dim prueba3 As List(Of Double) = New List(Of Double)

                    If Muro_i.Lebe_Izq(indice) > 0 Then
                        Confinamiento_Izq = (Muro_i.Lebe_Izq(indice) / 100) + bw_vecino_izq
                    ElseIf Muro_i.Zc_Izq(indice) > 0 Then
                        Confinamiento_Izq = (Muro_i.Zc_Izq(indice) / 100) + bw_vecino_izq
                    End If

                    prueba1 = ListaOrdenada(i).MurosVecinosAbajo.Select(Function(x) Math.Round(x.Xmin, 2)).ToList
                    prueba2 = ListaOrdenada(i).MurosVecinosArriba.Select(Function(x) Math.Round(x.Xmin, 2)).ToList

                    For j = 0 To prueba1.Count - 1
                        prueba3.Add(prueba1(j))
                    Next

                    For j = 0 To prueba2.Count - 1
                        prueba3.Add(prueba2(j))
                    Next

                    prueba3 = prueba3.Distinct().ToList

                    For j = 0 To prueba3.Count - 1

                        If prueba3(j) >= Math.Round(ListaOrdenada(i).Xmin, 2) And prueba3(j) <= Math.Round(ListaOrdenada(i).Xmin + ListaOrdenada(i).LEB_Iz, 2) Then

                            If ListaOrdenada(i).MurosVecinosAbajo.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)) IsNot Nothing = True Then

                                indice2 = ListaOrdenada(i).MurosVecinosAbajo.FindIndex(Function(x) Math.Round(x.Xmin, 2) = prueba3(j))
                                Delta_Escalado += ListaOrdenada(i).MurosVecinosAbajo.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)).EspesorEscalado - ListaOrdenada(i).MurosVecinosAbajo.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)).EspesorReal
                                Puntos_Limites.Add({ListaOrdenada(i).MurosVecinosAbajo(indice2).Xmin, ListaOrdenada(i).MurosVecinosAbajo(indice2).Xmax})

                            ElseIf ListaOrdenada(i).MurosVecinosArriba.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)) IsNot Nothing = True Then

                                indice2 = ListaOrdenada(i).MurosVecinosArriba.FindIndex(Function(x) Math.Round(x.Xmin, 2) = prueba3(j))
                                Delta_Escalado += ListaOrdenada(i).MurosVecinosArriba.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)).EspesorEscalado - ListaOrdenada(i).MurosVecinosArriba.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)).EspesorReal
                                Puntos_Limites.Add({Math.Round(ListaOrdenada(i).MurosVecinosArriba(indice2).Xmin, 2), Math.Round(ListaOrdenada(i).MurosVecinosArriba(indice2).Xmax, 2)})

                            End If

                        End If

                    Next

                    If Confinamiento_Izq > 2 Then
                        Num_Estribos = (Confinamiento_Izq / Distancia_Maxima) + 1
                        Distancia_Limite = (Confinamiento_Izq / Num_Estribos) + 0.2
                    Else
                        Num_Estribos = 1
                        Distancia_Limite = (Confinamiento_Izq + Delta_Escalado / 2) - (2 * 0.02)
                    End If

                    Estribos_Izquierda(Suma_Long, delta, Punto_inicial, {ListaOrdenada(i).Xmin + Delta_Escalado / 2 + Confinamiento_Izq, Punto_inicial(1), 0}, Muro_i, indice, Pos, Distancia_Limite, i, Delta_Escalado, Puntos_Limites, 0, Vecino_Izq, Vecino_Der, bw_Vecino_der, Num_Estribos)

                End If

                ''Caso en el cual el muro va totalmente confinado
                If Muro_i.Rho_l(indice) >= 1 / 100 Then

                    Dim prueba1, prueba2 As List(Of Double)
                    Dim prueba3 As List(Of Double) = New List(Of Double)

                    prueba1 = ListaOrdenada(i).MurosVecinosAbajo.Select(Function(x) Math.Round(x.Xmin, 2)).ToList
                    prueba2 = ListaOrdenada(i).MurosVecinosArriba.Select(Function(x) Math.Round(x.Xmin, 2)).ToList

                    For j = 0 To prueba1.Count - 1
                        prueba3.Add(prueba1(j))
                    Next

                    For j = 0 To prueba2.Count - 1
                        prueba3.Add(prueba2(j))
                    Next

                    prueba3 = prueba3.Distinct().ToList

                    For j = 0 To prueba3.Count - 1

                        If prueba3(j) >= Math.Round(Punto_inicial(0) - 0.038, 2) And prueba3(j) <= Math.Round(Punto_final(0) + 0.038, 2) Then

                            If ListaOrdenada(i).MurosVecinosAbajo.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)) IsNot Nothing = True Then

                                indice2 = ListaOrdenada(i).MurosVecinosAbajo.FindIndex(Function(x) Math.Round(x.Xmin, 2) = prueba3(j))
                                Delta_Escalado += ListaOrdenada(i).MurosVecinosAbajo.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)).EspesorEscalado - ListaOrdenada(i).MurosVecinosAbajo.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)).EspesorReal
                                Puntos_Limites.Add({ListaOrdenada(i).MurosVecinosAbajo(indice2).Xmin, ListaOrdenada(i).MurosVecinosAbajo(indice2).Xmax})

                            ElseIf ListaOrdenada(i).MurosVecinosArriba.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)) IsNot Nothing = True Then

                                indice2 = ListaOrdenada(i).MurosVecinosArriba.FindIndex(Function(x) Math.Round(x.Xmin, 2) = prueba3(j))
                                Delta_Escalado += ListaOrdenada(i).MurosVecinosArriba.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)).EspesorEscalado - ListaOrdenada(i).MurosVecinosArriba.Find(Function(x) Math.Round(x.Xmin, 2) = prueba3(j)).EspesorReal
                                Puntos_Limites.Add({Math.Round(ListaOrdenada(i).MurosVecinosArriba(indice2).Xmin, 2), Math.Round(ListaOrdenada(i).MurosVecinosArriba(indice2).Xmax, 2)})

                            End If

                        End If

                    Next

                    If Punto_final(0) - Punto_inicial(0) > 2 + Delta_Escalado Then
                        Num_Estribos = ((Punto_final(0) - Punto_inicial(0)) / (Distancia_Maxima)) + 1
                        Distancia_Limite = ((Punto_final(0) - Punto_inicial(0)) / (Num_Estribos)) + 0.2
                    Else
                        Num_Estribos = 1
                        Distancia_Limite = (Punto_final(0) - Punto_inicial(0) + Delta_Escalado) - (2 * 0.02)
                    End If

                    Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, indice, Pos, Distancia_Limite, i, Delta_Escalado, Puntos_Limites, 0, Vecino_Izq, Vecino_Der, bw_Vecino_der, Num_Estribos)

                End If

                If Muro_i.Lebe_Der(indice) > 0 And Muro_i.Lebe_Der(indice) <= Muro_i.lw(indice) OrElse Muro_i.Zc_Der(indice) > 0 Then

                    Dim prueba1, prueba2 As List(Of Double)
                    Dim prueba3 As List(Of Double) = New List(Of Double)

                    If Muro_i.Lebe_Der(indice) > 0 Then
                        Confinamiento_Der = (Muro_i.Lebe_Der(indice) / 100) + bw_Vecino_der
                    ElseIf Muro_i.Zc_Der(indice) > 0 Then
                        Confinamiento_Der = (Muro_i.Zc_Der(indice) / 100) + bw_Vecino_der
                    End If

                    prueba1 = ListaOrdenada(i).MurosVecinosAbajo.Select(Function(x) Math.Round(x.Xmax, 2)).ToList
                    prueba2 = ListaOrdenada(i).MurosVecinosArriba.Select(Function(x) Math.Round(x.Xmax, 2)).ToList

                    For j = 0 To prueba1.Count - 1
                        prueba3.Add(prueba1(j))
                    Next

                    For j = 0 To prueba2.Count - 1
                        prueba3.Add(prueba2(j))
                    Next

                    prueba3 = prueba3.Distinct().ToList

                    For j = 0 To prueba3.Count - 1

                        If prueba3(j) <= Math.Round(ListaOrdenada(i).Xmax, 2) And prueba3(j) >= Math.Round(ListaOrdenada(i).Xmax - ListaOrdenada(i).LEB_Dr, 2) Then

                            If ListaOrdenada(i).MurosVecinosAbajo.Find(Function(x) Math.Round(x.Xmax, 2) = prueba3(j)) IsNot Nothing Then

                                Delta_Escalado += ListaOrdenada(i).MurosVecinosAbajo.Find(Function(x) Math.Round(x.Xmax, 2) = prueba3(j)).EspesorEscalado - ListaOrdenada(i).MurosVecinosAbajo.Find(Function(x) Math.Round(x.Xmax, 2) = prueba3(j)).EspesorReal

                            ElseIf ListaOrdenada(i).MurosVecinosArriba.Find(Function(x) Math.Round(x.Xmax, 2) = prueba3(j)) IsNot Nothing Then

                                Delta_Escalado += ListaOrdenada(i).MurosVecinosArriba.Find(Function(x) Math.Round(x.Xmax, 2) = prueba3(j)).EspesorEscalado - ListaOrdenada(i).MurosVecinosArriba.Find(Function(x) Math.Round(x.Xmax, 2) = prueba3(j)).EspesorReal

                            End If

                        End If

                    Next

                    If Confinamiento_Der > 2 Then
                        Num_Estribos = (Confinamiento_Der / Distancia_Maxima) + 1
                        Distancia_Limite = (Confinamiento_Der / Num_Estribos) + 0.2
                    Else
                        Num_Estribos = 1
                        Distancia_Limite = Confinamiento_Der + Delta_Escalado / 2 - 0.04
                    End If

                    Pos = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1
                    Estribos_Derecha(Suma_Long, delta, {(ListaOrdenada(i).Xmax - Confinamiento_Der) - Delta_Escalado / 2, ListaOrdenada(i).Ymin, 0}, Punto_final, Muro_i, indice, Pos, Distancia_Limite, i, 0, Vecino_Der, Num_Estribos)

                End If

                'Agregar ganchos en la seccion
                Dim flip_State As Short = 0
                For j = 0 To ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1

                    If ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).Gancho = True Then
                        Dim Ip As Double()
                        Dim Long_Gancho As Double

                        Ip = ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).CoordenadasXyY
                        Long_Gancho = Math.Abs(ListaOrdenada(i).Lista_Refuerzos_Fila_Max(j).CoordenadasXyY(1) - ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(1))
                        Add_Gancho("FC_GANCHOS", Math.PI / 2, Ip, Long_Gancho, flip_State)

                        If flip_State = 0 Then
                            flip_State = 1
                        Else
                            flip_State = 0
                        End If

                    End If
                Next
            Else

                Muro_i = Muros_lista_2.Find(Function(x) x.Pier_name = ListaOrdenada(i).NombreMuro)
                indice = Muro_i.Stories.FindIndex(Function(x) x = "Story" & Formulario.Piso_Box.Text)

                If ListaOrdenada(i).MurosVecinosAbajo.Count > 0 Then
                    Punto_inicial = {ListaOrdenada(i).Xmin + (ListaOrdenada(i).Xmax - ListaOrdenada(i).Xmin) / 2, ListaOrdenada(i).MurosVecinosAbajo(0).Ymin + 0.038, 0}
                    Vecino_Abajo = True
                    bw_vecino_izq = ListaOrdenada(i).MurosVecinosAbajo(0).Ymax - ListaOrdenada(i).MurosVecinosAbajo(0).Ymin
                Else
                    Punto_inicial = {ListaOrdenada(i).Xmin + (ListaOrdenada(i).Xmax - ListaOrdenada(i).Xmin) / 2, ListaOrdenada(i).Ymin + 0.038, 0}
                    Vecino_Abajo = False
                End If

                If ListaOrdenada(i).MurosVecinosArriba.Count > 0 Then
                    Punto_final = {ListaOrdenada(i).Xmin + (ListaOrdenada(i).Xmax - ListaOrdenada(i).Xmin) / 2, ListaOrdenada(i).MurosVecinosArriba(0).Ymax - 0.038, 0}
                    Vecino_Arriba = True
                    bw_Vecino_der = ListaOrdenada(i).MurosVecinosArriba(0).Ymax - ListaOrdenada(i).MurosVecinosArriba(0).Ymin
                Else
                    Punto_final = {ListaOrdenada(i).Xmin + (ListaOrdenada(i).Xmax - ListaOrdenada(i).Xmin) / 2, ListaOrdenada(i).Ymax - 0.038, 0}
                    Vecino_Arriba = False
                End If

                Suma_Long = 0
                Delta_Escalado = 0
                Pos = 0

                ''Caso en el cual el muro no va totalmente confinado

                If Muro_i.Lebe_Izq(indice) > 0 Or Muro_i.Zc_Izq(indice) > 0 Then

                    Dim prueba1, prueba2 As List(Of Double)
                    Dim prueba3 As List(Of Double) = New List(Of Double)

                    If Muro_i.Lebe_Izq(indice) > 0 Then
                        Confinamiento_Izq = (Muro_i.Lebe_Izq(indice) / 100) + bw_vecino_izq
                    ElseIf Muro_i.Zc_Izq(indice) > 0 Then
                        Confinamiento_Izq = (Muro_i.Zc_Izq(indice) / 100) + bw_vecino_izq
                    End If

                    prueba1 = ListaOrdenada(i).MurosVecinosIzquierda.Select(Function(x) Math.Round(x.Ymin, 2)).ToList
                    prueba2 = ListaOrdenada(i).MurosVecinosDerecha.Select(Function(x) Math.Round(x.Ymin, 2)).ToList

                    For j = 0 To prueba1.Count - 1
                        prueba3.Add(prueba1(j))
                    Next

                    For j = 0 To prueba2.Count - 1
                        prueba3.Add(prueba2(j))
                    Next

                    prueba3 = prueba3.Distinct().ToList

                    If Muro_i.Rho_l(indice) < 1 / 100 Then

                        For j = 0 To prueba3.Count - 1
                            Escalado_Vertical_1(ListaOrdenada(i).Ymin, ListaOrdenada(i).Ymin + Confinamiento_Izq, indice2, Delta_Escalado, Puntos_Limites, i, prueba3, j)
                        Next

                        If Confinamiento_Izq > 2 Then
                            Num_Estribos = (Confinamiento_Izq / Distancia_Maxima) + 1
                            Distancia_Limite = (Confinamiento_Izq / Num_Estribos) + 0.2
                        Else
                            Num_Estribos = 1
                            Distancia_Limite = Confinamiento_Izq + Delta_Escalado / 2 - (2 * 0.02)
                        End If

                        Estribos_Izquierda(Suma_Long, delta, Punto_inicial, {Punto_inicial(0), ListaOrdenada(i).Ymin + Delta_Escalado / 2 + Confinamiento_Izq, 0}, Muro_i, indice, Pos, Distancia_Limite, i, Delta_Escalado, Puntos_Limites, 1, Vecino_Abajo, Vecino_Arriba, bw_Vecino_der, Num_Estribos)

                    End If

                    If Muro_i.Rho_l(indice) >= 1 / 100 Then ''Caso en el cual el muro va totalmente confinado

                        For j = 0 To prueba3.Count - 1
                            Escalado_Vertical_1(Punto_inicial(1) + 0.038, Punto_final(1) - 0.038, indice2, Delta_Escalado, Puntos_Limites, i, prueba3, j)
                        Next

                        Confinamiento_Izq = Punto_final(1) - Punto_inicial(1)

                        If Confinamiento_Izq > 2 + Delta_Escalado Then
                            Num_Estribos = (Confinamiento_Izq / Distancia_Maxima) + 1
                            Distancia_Limite = (Confinamiento_Izq / Num_Estribos) + 0.2
                        Else
                            Num_Estribos = 1
                            Distancia_Limite = Confinamiento_Izq + Delta_Escalado / 2 - (2 * 0.02)
                        End If

                        Estribos_Izquierda(Suma_Long, delta, Punto_inicial, {Punto_inicial(0), ListaOrdenada(i).Ymin + Delta_Escalado / 2 + Confinamiento_Izq, 0}, Muro_i, indice, Pos, Distancia_Limite, i, Delta_Escalado, Puntos_Limites, 1, Vecino_Abajo, Vecino_Arriba, bw_Vecino_der, Num_Estribos)

                    End If

                End If

                If Muro_i.Lebe_Der(indice) > 0 OrElse Muro_i.Zc_Der(indice) > 0 Then

                    Dim prueba1, prueba2 As List(Of Double)
                    Dim prueba3 As List(Of Double) = New List(Of Double)

                    If Muro_i.Lebe_Der(indice) > 0 Then
                        Confinamiento_Der = (Muro_i.Lebe_Der(indice) / 100) + bw_Vecino_der
                    ElseIf Muro_i.Zc_Der(indice) > 0 Then
                        Confinamiento_Der = (Muro_i.Zc_Der(indice) / 100) + bw_Vecino_der
                    End If

                    prueba1 = ListaOrdenada(i).MurosVecinosIzquierda.Select(Function(x) Math.Round(x.Ymin, 2)).ToList
                    prueba2 = ListaOrdenada(i).MurosVecinosDerecha.Select(Function(x) Math.Round(x.Ymin, 2)).ToList

                    For j = 0 To prueba1.Count - 1
                        prueba3.Add(prueba1(j))
                    Next

                    For j = 0 To prueba2.Count - 1
                        prueba3.Add(prueba2(j))
                    Next

                    prueba3 = prueba3.Distinct().ToList

                    For j = 0 To prueba3.Count - 1
                        Escalado_Vertical_2(Punto_final(1), Punto_final(1) - Confinamiento_Der, indice2, Delta_Escalado, Puntos_Limites, i, prueba3, j)
                    Next

                    If Confinamiento_Der > 2 Then
                        Num_Estribos = (Confinamiento_Der / Distancia_Maxima) + 1
                        Distancia_Limite = (Confinamiento_Der / Num_Estribos) + 0.2
                    Else
                        Distancia_Limite = Confinamiento_Der + Delta_Escalado / 2 - (2 * 0.02)
                    End If

                    Pos = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1
                    Estribos_Derecha(Suma_Long, delta, {Punto_final(0), Punto_final(1) - Confinamiento_Der - Delta_Escalado / 2, 0}, Punto_final, Muro_i, indice, Pos, Distancia_Limite, i, 1, Vecino_Arriba, Num_Estribos)
                End If

                'Agregar ganchos en la seccion

                Dim Flip_State As Short = 0
                For j = 0 To ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1
                    If ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).Gancho = True Then
                        Dim Ip As Double()
                        Dim Long_Gancho As Double

                        Ip = ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).CoordenadasXyY
                        Long_Gancho = Math.Abs(ListaOrdenada(i).Lista_Refuerzos_Fila_Max(j).CoordenadasXyY(0) - ListaOrdenada(i).Lista_Refuerzos_Fila_Min(0).CoordenadasXyY(0))
                        Add_Gancho("FC_GANCHOS", 0, Ip, Long_Gancho, Flip_State)

                        If Flip_State = 0 Then
                            Flip_State = 1
                        Else
                            Flip_State = 0
                        End If

                    End If
                Next

            End If
        Next

    End Sub

    Private Shared Sub Escalado_Vertical_1(ByVal Punto_inicial As Double, ByVal Punto_final As Double, ByRef indice2 As Integer, ByRef Delta_Escalado As Double, Puntos_Limites As List(Of Double()), i As Integer, prueba3 As List(Of Double), j As Integer)

        If prueba3(j) >= Math.Round(Punto_inicial, 2) And prueba3(j) <= Math.Round(Punto_final, 2) Then

            If ListaOrdenada(i).MurosVecinosIzquierda.Find(Function(x) Math.Round(x.Ymin, 2) = prueba3(j)) IsNot Nothing = True Then

                'indice2 = ListaOrdenada(i).MurosVecinosIzquierda.FindIndex(Function(x) Math.Round(x.Ymin, 2) = prueba3(j))
                Delta_Escalado += ListaOrdenada(i).MurosVecinosIzquierda.Find(Function(x) Math.Round(x.Ymin, 2) = prueba3(j)).EspesorEscalado - ListaOrdenada(i).MurosVecinosIzquierda.Find(Function(x) Math.Round(x.Ymin, 2) = prueba3(j)).EspesorReal
                'Puntos_Limites.Add({ListaOrdenada(i).MurosVecinosAbajo(indice2).Ymin, ListaOrdenada(i).MurosVecinosAbajo(indice2).Ymax})

            ElseIf ListaOrdenada(i).MurosVecinosDerecha.Find(Function(x) Math.Round(x.Ymin, 2) = prueba3(j)) IsNot Nothing = True Then

                'indice2 = ListaOrdenada(i).MurosVecinosDerecha.FindIndex(Function(x) Math.Round(x.Ymin, 2) = prueba3(j))
                Delta_Escalado += ListaOrdenada(i).MurosVecinosDerecha.Find(Function(x) Math.Round(x.Ymin, 2) = prueba3(j)).EspesorEscalado - ListaOrdenada(i).MurosVecinosDerecha.Find(Function(x) Math.Round(x.Ymin, 2) = prueba3(j)).EspesorReal
                'Puntos_Limites.Add({Math.Round(ListaOrdenada(i).MurosVecinosArriba(indice2).Ymin, 2), Math.Round(ListaOrdenada(i).MurosVecinosArriba(indice2).Ymax, 2)})

            End If

        End If

    End Sub

    Private Shared Sub Escalado_Vertical_2(ByVal Punto_inicial As Double, ByVal Punto_final As Double, ByRef indice2 As Integer, ByRef Delta_Escalado As Double, Puntos_Limites As List(Of Double()), i As Integer, prueba3 As List(Of Double), j As Integer)

        If prueba3(j) >= Math.Round(Punto_inicial, 2) And prueba3(j) <= Math.Round(Punto_final, 2) Then

            If ListaOrdenada(i).MurosVecinosIzquierda.Find(Function(x) Math.Round(x.Ymax, 2) = prueba3(j)) IsNot Nothing = True Then

                Delta_Escalado += ListaOrdenada(i).MurosVecinosIzquierda.Find(Function(x) Math.Round(x.Ymax, 2) = prueba3(j)).EspesorEscalado - ListaOrdenada(i).MurosVecinosIzquierda.Find(Function(x) Math.Round(x.Ymax, 2) = prueba3(j)).EspesorReal

            ElseIf ListaOrdenada(i).MurosVecinosDerecha.Find(Function(x) Math.Round(x.Ymax, 2) = prueba3(j)) IsNot Nothing = True Then

                Delta_Escalado += ListaOrdenada(i).MurosVecinosDerecha.Find(Function(x) Math.Round(x.Ymin, 2) = prueba3(j)).EspesorEscalado - ListaOrdenada(i).MurosVecinosDerecha.Find(Function(x) Math.Round(x.Ymin, 2) = prueba3(j)).EspesorReal

            End If

        End If

    End Sub

    Private Sub Estribos_Derecha(ByRef Suma_Long As Single, ByRef delta As Single, ByRef Punto_inicial() As Double, ByRef Punto_final() As Double, Muro_i As Muros_Consolidados, indice As Integer, ByRef Pos As Integer, Distancia_Limite As Double, i As Integer, ByVal Direccion As Integer, ByVal Vecino_Der As Boolean, ByVal Num_estribos As Integer)

        Dim condicion, condicion2 As Boolean
        condicion = True
        condicion2 = False
        Suma_Long = 0
        delta = 0

        For j = Pos To 1 Step -1

            If Math.Round(ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).CoordenadasXyY(Direccion), 2) >= Math.Round(Punto_inicial(Direccion), 2) Then

                If j = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1 And Vecino_Der = True Then
                    delta = Math.Abs(Punto_final(Direccion) - ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).CoordenadasXyY(Direccion))
                    condicion2 = True
                Else
                    delta = Math.Abs(ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).CoordenadasXyY(Direccion) - ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(Direccion))
                End If

                Suma_Long += delta

                'Determinar si la barra lleva gancho o no

                If j <> Pos Or Pos = 0 Then
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = True
                End If

                If Suma_Long + (0.038 * 2) >= Distancia_Limite Then

                    If Direccion = 0 Then
                        Add_Estribos("FC_ESTRIBOS", Math.PI / 2, Punto_final, Suma_Long, ListaOrdenada(i).EspesorEscalado, True)
                    Else
                        Add_Estribos("FC_ESTRIBOS", 0, Punto_final, Suma_Long, ListaOrdenada(i).EspesorEscalado, False)
                    End If

                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).Gancho = False 'Determina si la barra lleva gancho o no

                    If condicion2 = False Then
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    Else
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = True
                        condicion2 = False
                    End If

                    If j - 1 = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1 Then
                        condicion = False
                    End If

                    Pos = j
                    j = Pos + 1
                    Punto_final = {ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).CoordenadasXyY(Direccion), ListaOrdenada(i).Ymin + (ListaOrdenada(i).Ymax - ListaOrdenada(i).Ymin) / 2, 0}

                    If condicion = False Then
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = True
                    Else
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    End If

                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos - 1).Gancho = False
                    Suma_Long = 0

                    If Num_estribos = 1 Then
                        If ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False Then
                            ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = True
                        End If
                        Exit For
                    End If

                End If

                If j - 1 = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1 And Suma_Long + (0.038 * 2) < Distancia_Limite And condicion = True Then
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 2).Gancho = False 'Determina si la barra lleva gancho o no
                    Suma_Long = 0
                    Exit For
                End If
            Else
                ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = True
                Exit For
            End If

        Next
    End Sub

    Private Sub Estribos_Izquierda(ByRef Suma_Long As Single, ByRef delta As Single, ByRef Punto_inicial() As Double, ByVal Punto_final() As Double, Muro_i As Muros_Consolidados, indice As Integer, ByRef Pos As Integer, Distancia_Limite As Double, i As Integer, ByVal delta_escala As Double, ByVal Puntos_Limites As List(Of Double()), ByVal Direccion As Integer, ByVal Vecino_izq As Boolean, ByVal Vecino_Der As Boolean, ByVal Bw_Der As Single, ByVal Num_estribos As Integer)

        Dim condicion, condicion2 As Boolean
        condicion = True
        condicion2 = False
        Suma_Long = 0
        delta = 0

        For j = Pos To ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 2

            If Math.Round(ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).CoordenadasXyY(Direccion), 2) <= Math.Round(Punto_final(Direccion), 2) Then

                If j = 0 And Vecino_izq = True Then
                    delta = ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).CoordenadasXyY(Direccion) - Punto_inicial(Direccion)
                    condicion2 = True
                Else
                    delta = ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).CoordenadasXyY(Direccion) - ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(Direccion)
                End If

                Suma_Long += delta

                'Determinar si la barra lleva gancho o no

                If j <> Pos Or Pos = 0 Then
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = True
                End If

                If Suma_Long + (0.038 * 2) >= Distancia_Limite Then

                    If j + 1 = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1 Then
                        condicion = False
                        If Vecino_Der = True Then
                            Suma_Long += Bw_Der
                        End If

                    End If

                    If Direccion = 0 Then
                        Add_Estribos("FC_ESTRIBOS", Math.PI / 2, Punto_inicial, Suma_Long, ListaOrdenada(i).EspesorEscalado, False)
                    Else
                        Add_Estribos("FC_ESTRIBOS", 0, {Punto_inicial(0), Punto_inicial(1) + Suma_Long, 0}, Suma_Long, ListaOrdenada(i).EspesorEscalado, False)
                    End If

                    If condicion2 = False Then
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    Else
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = True
                        condicion2 = False
                    End If

                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = False 'Determina si la barra lleva gancho o no
                    Pos = j
                    j = Pos - 1

                    If Direccion = 0 Then
                        Punto_inicial = {ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).CoordenadasXyY(Direccion), ListaOrdenada(i).Ymin + (ListaOrdenada(i).Ymax - ListaOrdenada(i).Ymin) / 2, 0}
                    Else
                        Punto_inicial = {ListaOrdenada(i).Xmin + (ListaOrdenada(i).Xmax - ListaOrdenada(i).Xmin) / 2, ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).CoordenadasXyY(Direccion), 0}
                    End If

                    If condicion = False Then
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = True
                    Else
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    End If

                    If Pos + 1 = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1 And Vecino_Der = True Then
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos + 1).Gancho = True
                    Else
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos + 1).Gancho = False
                    End If

                    Suma_Long = 0

                    If Num_estribos = 1 Then
                        If ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False Then
                            ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = True
                        End If
                        Exit For
                    End If

                End If

                If j + 1 = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1 And Suma_Long + (0.038 * 2) < Distancia_Limite And condicion = True Then

                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = False 'Determina si la barra lleva gancho o no

                    If Direccion = 0 Then
                        Add_Estribos("FC_ESTRIBOS", Math.PI / 2, Punto_inicial, Suma_Long, ListaOrdenada(i).EspesorEscalado, False)
                    Else
                        Add_Estribos("FC_ESTRIBOS", 0, {Punto_inicial(0), Punto_inicial(1) + Suma_Long, 0}, Suma_Long, ListaOrdenada(i).EspesorEscalado, False)
                    End If

                    Suma_Long = 0
                    Exit For
                End If
            Else
                ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).Gancho = True
                Exit For
            End If

        Next
    End Sub

    Private Sub Add_Estribos(ByVal Layer As String, ByVal Angulo As Double, ByVal Ip As Double(), ByVal Distancia As Double, ByVal Espesor_Doble As Double, ByVal Mover As Boolean)

        Dim Nombre_Bloque As String
        Dim dynamic_property1 As Object
        Dim editar_property1 As AcadDynamicBlockReferenceProperty
        Dim Point_aux As Double()

        Nombre_Bloque = "FC_B_Estribo tipo 7"

        If Mover = False Then
            Bloque_Estribo = AcadDoc.ModelSpace.InsertBlock(Ip, Nombre_Bloque, 1, 1, 1, Angulo)
        Else
            Point_aux = {Ip(0) - Distancia, Ip(1), 0}
            Bloque_Estribo = AcadDoc.ModelSpace.InsertBlock(Point_aux, Nombre_Bloque, 1, 1, 1, Angulo)
        End If

        dynamic_property1 = Bloque_Estribo.GetDynamicBlockProperties

        editar_property1 = dynamic_property1(0)
        editar_property1.Value = Espesor_Doble

        editar_property1 = dynamic_property1(2)
        editar_property1.Value = Distancia

        Bloque_Estribo.Layer = Layer
        Bloque_Estribo.Update()

        ''Estribo_Back(Bloque_Estribo)

    End Sub

    Private Sub Add_Gancho(ByVal Layer As String, ByVal Angulo As Double, ByVal Ip As Double(), ByVal Distancia As Double, ByVal Flip_State As Short)

        Dim Nombre_Bloque As String
        Dim dynamic_property1 As Object
        Dim editar_property1 As AcadDynamicBlockReferenceProperty

        Nombre_Bloque = "FC_B_Gancho Tipo 5"

        Bloque_Gancho = AcadDoc.ModelSpace.InsertBlock(Ip, Nombre_Bloque, 1, 1, 1, Angulo)
        dynamic_property1 = Bloque_Gancho.GetDynamicBlockProperties

        editar_property1 = dynamic_property1(0)
        editar_property1.Value = Distancia

        editar_property1 = dynamic_property1(2)
        editar_property1.Value = Flip_State

        Bloque_Gancho.Layer = Layer
        Bloque_Gancho.Update()

    End Sub

    Private Shared Sub Estribo_Back(ByVal Acad_object As AcadObject)

        Dim Diccionario As AcadDictionary
        Dim sentityObj As AcadObject
        Dim arr(0) As AcadObject

        'Mover el hacth hacia atras
        Diccionario = AcadDoc.ModelSpace.GetExtensionDictionary

        sentityObj = Diccionario.GetObject("ACAD_SORTENTS")
        sentityObj = Diccionario.AddObject("ACAD_SORTENTS", "AcDbSortentsTable")

        arr(0) = Acad_object
        sentityObj.MoveToBottom(arr.ToArray)

    End Sub

End Class