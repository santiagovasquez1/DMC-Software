Imports Autodesk.AutoCAD.Interop
Imports Autodesk.AutoCAD.Interop.Common

Public Class Estribos_Totales

    Public Bloque_Estribo As AcadBlockReference
    Public Bloque_Gancho As AcadBlockReference

    Sub Estribos_Pisos(ByRef DeltaY As Single)

        Dim Suma_Long, delta As Single
        Dim Punto_inicial As Double()
        Dim Punto_final As Double()
        Dim Muro_i As Muros_Consolidados
        Dim Distancia_Limite, Distancia_Maxima As Double
        Dim Vecino_izquierda, Vecino_Derecha, Vecino_Arriba, Vecino_Abajo As Boolean
        Dim Distancia_Confinada As Double
        Dim Delta_reduccion As Double
        Dim Pos, Num_Estribos As Integer
        Dim Delta_X As Double

        Distancia_Maxima = 2

        For i = 0 To ListaOrdenada.Count - 1

            Muro_i = Muros_lista_2.Find(Function(x) x.Pier_name = ListaOrdenada(i).NombreMuro)

            Dim Muro_vecino_izquierda As Muros_Consolidados = New Muros_Consolidados
            Dim Muro_Vecino_derecha As Muros_Consolidados = New Muros_Consolidados
            Dim Muro_Vecino_Arriba As Muros_Consolidados = New Muros_Consolidados
            Dim Muro_Vecino_Abajo As Muros_Consolidados = New Muros_Consolidados

            Vecino_Derecha = False
            Vecino_izquierda = False
            Vecino_Arriba = False
            Vecino_Abajo = False

            If ListaOrdenada(i).DireccionMuro = "Horizontal" Then

                ''Primer paso para el inicio del dibujo 
                Determinacion_Vecinos(Vecino_izquierda, Vecino_Derecha, i, Muro_vecino_izquierda, Muro_Vecino_derecha, ListaOrdenada(i).DireccionMuro)
                DeltaY = 0

                For j = Muro_i.Stories.Count - 1 To 0 Step -1

                    Delta_reduccion = 0
                    Pos = 0
                    Suma_Long = 0

                    ''Caso en el cual el muro va totalmente confinado
                    If Muro_i.Rho_l(j) >= 0.01 Then

                        Determinacion_Punto_Arranque_Horizontal(Punto_inicial, Muro_i, Vecino_izquierda, Delta_reduccion, i, Muro_vecino_izquierda, j, DeltaY, Delta_X)
                        Determinacion_Punto_Final_Horizontal(Punto_final, Muro_i, Vecino_Derecha, Delta_reduccion, i, Muro_vecino_izquierda, j, DeltaY, Delta_X)

                        Distancia_Confinada = Math.Abs(Punto_final(0) - Punto_inicial(0))

                        If Distancia_Confinada > Distancia_Maxima Then
                            Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                            Distancia_Limite = Distancia_Confinada / Num_Estribos
                        Else
                            Distancia_Limite = Distancia_Confinada - 0.04
                        End If
                        Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY)

                    Else

                        If Muro_i.Lebe_Izq(j) > 0 Or Muro_i.Zc_Izq(j) > 0 Then

                            Determinacion_Punto_Arranque_Horizontal(Punto_inicial, Muro_i, Vecino_izquierda, Delta_reduccion, i, Muro_vecino_izquierda, j, DeltaY, Delta_X)
                            Distancia_Confinada = Determinacion_Confinamiento_LI(Muro_i, Vecino_izquierda, Muro_vecino_izquierda, j)

                            If Distancia_Confinada > Distancia_Maxima Then
                                Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                Distancia_Limite = Distancia_Confinada / Num_Estribos
                            Else
                                Distancia_Limite = Distancia_Confinada - 0.04
                            End If

                            Punto_final = {Punto_inicial(0) + Distancia_Confinada, Punto_inicial(1), 0}
                            Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY)

                        End If

                        If Muro_i.Lebe_Der(j) > 0 Or Muro_i.Zc_Der(j) > 0 Then

                            Determinacion_Punto_Final_Horizontal(Punto_final, Muro_i, Vecino_Derecha, Delta_reduccion, i, Muro_Vecino_derecha, j, DeltaY, Delta_X)
                            Distancia_Confinada = Determinacion_Confinamiento_Ld(Muro_i, Vecino_Derecha, Muro_Vecino_derecha, j)

                            If Distancia_Confinada > Distancia_Maxima Then
                                Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                Distancia_Limite = Distancia_Confinada / Num_Estribos
                            Else
                                Distancia_Limite = Distancia_Confinada - 0.04
                            End If

                            Punto_inicial = {Punto_final(0) - Distancia_Confinada, Punto_final(1), 0}
                            Estribos_Derecha(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY)

                        End If

                    End If

                    DeltaY += 0.65
                Next

            End If

            If ListaOrdenada(i).DireccionMuro = "Vertical" Then

                Determinacion_Vecinos(Vecino_Abajo, Vecino_Arriba, i, Muro_Vecino_Abajo, Muro_Vecino_Arriba, ListaOrdenada(i).DireccionMuro)
                DeltaY = 0

                For j = Muro_i.Stories.Count - 1 To 0 Step -1

                    Delta_reduccion = 0
                    Pos = 0
                    Suma_Long = 0

                    If Muro_i.Lebe_Izq(j) > 0 Or Muro_i.Zc_Izq(j) > 0 Then

                        Determinacion_Punto_Arranque_Vertical(Punto_inicial, Muro_i, Vecino_Abajo, Delta_reduccion, i, Muro_Vecino_Abajo, j, DeltaY, Delta_X)
                        Distancia_Confinada = Determinacion_Confinamiento_LI(Muro_i, Vecino_Abajo, Muro_Vecino_Abajo, j)

                        If Distancia_Confinada > Distancia_Maxima Then
                            Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                            Distancia_Limite = Distancia_Confinada / Num_Estribos
                        Else
                            Distancia_Limite = Distancia_Confinada - 0.04
                        End If

                        Punto_final = {Punto_inicial(0) + Distancia_Confinada, Punto_inicial(1), 0}
                        Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY)

                    End If

                    If Muro_i.Lebe_Der(j) > 0 Or Muro_i.Zc_Der(j) > 0 Then

                        Determinacion_Punto_Final_Vertical(Punto_final, Muro_i, Vecino_Arriba, Delta_reduccion, i, Muro_Vecino_Arriba, j, DeltaY, Delta_X)
                        Distancia_Confinada = Determinacion_Confinamiento_Ld(Muro_i, Vecino_Arriba, Muro_Vecino_Arriba, j)

                        If Distancia_Confinada > Distancia_Maxima Then
                            Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                            Distancia_Limite = Distancia_Confinada / Num_Estribos
                        Else
                            Distancia_Limite = Distancia_Confinada - 0.04
                        End If

                        Punto_inicial = {Punto_final(0) - Distancia_Confinada, Punto_final(1), 0}
                        Estribos_Derecha(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY)

                    End If

                    DeltaY += 0.65
                Next

            End If

        Next

    End Sub

    Private Shared Function Determinacion_Confinamiento_Ld(Muro_i As Muros_Consolidados, Vecino_dir As Boolean, Muro_Vecino_dir As Muros_Consolidados, j As Integer) As Double
        Dim Distancia_Confinada As Double

        If Muro_i.Lebe_Der(j) > 0 Then
            If Vecino_dir = True Then
                Distancia_Confinada = (Muro_Vecino_dir.Bw(j) + Muro_i.Lebe_Der(j)) / 100
            Else
                Distancia_Confinada = Muro_i.Lebe_Der(j) / 100
            End If
        Else
            If Vecino_dir = True Then
                Distancia_Confinada = (Muro_Vecino_dir.Bw(j) + Muro_i.Zc_Der(j)) / 100
            Else
                Distancia_Confinada = Muro_i.Zc_Der(j) / 100
            End If
        End If

        Return Distancia_Confinada
    End Function

    Private Shared Function Determinacion_Confinamiento_LI(Muro_i As Muros_Consolidados, Vecino_dir As Boolean, Muro_Vecino_dir As Muros_Consolidados, j As Integer) As Double

        Dim Distancia_Confinada As Double

        If Muro_i.Lebe_Izq(j) > 0 Then

            If Vecino_dir = True Then
                Distancia_Confinada = (Muro_Vecino_dir.Bw(j) + Muro_i.Lebe_Izq(j)) / 100
            Else
                Distancia_Confinada = Muro_i.Lebe_Izq(j) / 100
            End If
        Else
            If Vecino_dir = True Then
                Distancia_Confinada = (Muro_Vecino_dir.Bw(j) + Muro_i.Zc_Izq(j)) / 100
            Else
                Distancia_Confinada = Muro_i.Zc_Izq(j) / 100
            End If

        End If

        Return Distancia_Confinada
    End Function

    Private Shared Sub Determinacion_Vecinos(ByRef Vecino_dir1 As Boolean, ByRef Vecino_dir2 As Boolean, i As Integer, ByRef Muro_vecino_1 As Muros_Consolidados, ByRef Muro_Vecino_2 As Muros_Consolidados, ByVal Direccion As String)
        If Direccion = "Horizontal" Then

            If ListaOrdenada(i).MurosVecinosIzquierda.Count > 0 Then
                Vecino_dir1 = True
                Muro_vecino_1 = Muros_lista_2.Find(Function(x) x.Pier_name = ListaOrdenada(i).MurosVecinosIzquierda(0).NombreMuro)
            End If

            If ListaOrdenada(i).MurosVecinosDerecha.Count > 0 Then
                Vecino_dir2 = True
                Muro_Vecino_2 = Muros_lista_2.Find(Function(x) x.Pier_name = ListaOrdenada(i).MurosVecinosDerecha(0).NombreMuro)
            End If
        Else

            If ListaOrdenada(i).MurosVecinosAbajo.Count > 0 Then
                Vecino_dir1 = True
                Muro_vecino_1 = Muros_lista_2.Find(Function(x) x.Pier_name = ListaOrdenada(i).MurosVecinosAbajo(0).NombreMuro)
            End If

            If ListaOrdenada(i).MurosVecinosArriba.Count > 0 Then
                Vecino_dir2 = True
                Muro_Vecino_2 = Muros_lista_2.Find(Function(x) x.Pier_name = ListaOrdenada(i).MurosVecinosArriba(0).NombreMuro)
            End If

        End If

    End Sub

    Private Shared Sub Determinacion_Punto_Arranque_Horizontal(ByRef Punto_inicial() As Double, Muro_i As Muros_Consolidados, Vecino_izquierda As Boolean, ByRef Delta_reduccion As Double, i As Integer, Muro_vecino_izquierda As Muros_Consolidados, j As Integer, ByVal Delta_Y As Double, ByVal Delta_X As Double)

        If Vecino_izquierda = True Then
            If Muro_vecino_izquierda.Reduccion = "Der" Or Muro_vecino_izquierda.Reduccion = "Sin Reducc" Then
                Delta_reduccion = 0
            End If

            If Muro_vecino_izquierda.Reduccion = "Izq" Then

                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino_izquierda.Bw(j) - Muro_vecino_izquierda.Bw(j + 1)) / 100
                End If
            End If

            If Muro_vecino_izquierda.Reduccion = "Centro" Then
                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino_izquierda.Bw(j) - Muro_vecino_izquierda.Bw(j + 1)) / 200
                End If
            End If
            Punto_inicial = {Delta_X + Delta_reduccion + 0.038 - Muro_vecino_izquierda.Bw(j) / 100, Delta_Y, 0}
        Else
            Punto_inicial = {Delta_X + Delta_reduccion + 0.038, Delta_Y, 0}
        End If

    End Sub

    Private Shared Sub Determinacion_Punto_Arranque_Vertical(ByRef Punto_inicial() As Double, Muro_i As Muros_Consolidados, Vecino_Abajo As Boolean, ByRef Delta_reduccion As Double, i As Integer, Muro_vecino_Abajo As Muros_Consolidados, j As Integer, ByVal Delta_Y As Double, ByVal Delta_X As Double)

        If Vecino_Abajo = True Then

            If Muro_vecino_Abajo.Reduccion = "Abajo" Or Muro_vecino_Abajo.Reduccion = "Sin Reducc" Then
                Delta_reduccion = 0
            End If

            If Muro_vecino_Abajo.Reduccion = "Arriba" Then

                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino_Abajo.Bw(j) - Muro_vecino_Abajo.Bw(j + 1)) / 100
                End If

            End If

            If Muro_vecino_Abajo.Reduccion = "Centro" Then
                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino_Abajo.Bw(j) - Muro_vecino_Abajo.Bw(j + 1)) / 200
                End If
            End If
            Punto_inicial = {Delta_X + Delta_reduccion + 0.038 - Muro_vecino_Abajo.Bw(j) / 100, Delta_Y, 0}
        Else
            Punto_inicial = {Delta_X + Delta_reduccion + 0.038, Delta_Y, 0}
        End If

    End Sub

    Private Shared Sub Determinacion_Punto_Final_Horizontal(ByRef Punto_final() As Double, Muro_i As Muros_Consolidados, Vecino_derecha As Boolean, ByRef Delta_reduccion As Double, i As Integer, Muro_vecino As Muros_Consolidados, j As Integer, ByVal DeltaY As Double, ByVal Delta_X As Double)

        If Vecino_derecha = True Then

            If Muro_vecino.Reduccion = "Izq" Or Muro_vecino.Reduccion = "Sin Reducc" Then
                Delta_reduccion = 0
            End If

            If Muro_vecino.Reduccion = "Der" Then

                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino.Bw(j) - Muro_vecino.Bw(j + 1)) / 100
                End If

            End If

            If Muro_vecino.Reduccion = "Centro" Then
                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino.Bw(j) - Muro_vecino.Bw(j + 1)) / 200
                End If
            End If

            Punto_final = {Delta_X + (Muro_i.lw(j) + Muro_vecino.Bw(j)) / 100 - Delta_reduccion - 0.038, DeltaY, 0}
        Else
            Punto_final = {Delta_X + Muro_i.lw(j) / 100 - Delta_reduccion - 0.038, DeltaY, 0}
        End If

    End Sub

    Private Shared Sub Determinacion_Punto_Final_Vertical(ByRef Punto_final() As Double, Muro_i As Muros_Consolidados, Vecino_arriba As Boolean, ByRef Delta_reduccion As Double, i As Integer, Muro_vecino As Muros_Consolidados, j As Integer, ByVal DeltaY As Double, ByVal Delta_X As Double)

        If Vecino_arriba = True Then

            If Muro_vecino.Reduccion = "Arriba" Or Muro_vecino.Reduccion = "Sin Reducc" Then
                Delta_reduccion = 0
            End If

            If Muro_vecino.Reduccion = "Abajo" Then

                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino.Bw(j) - Muro_vecino.Bw(j + 1)) / 100
                End If

            End If

            If Muro_vecino.Reduccion = "Centro" Then
                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino.Bw(j) - Muro_vecino.Bw(j + 1)) / 200
                End If
            End If
            Punto_final = {Delta_X + (Muro_i.lw(j) + Muro_vecino.Bw(j)) / 100 - Delta_reduccion - 0.038, DeltaY, 0}
        Else
            Punto_final = {Delta_X + Muro_i.lw(j) / 100 - Delta_reduccion - 0.038, DeltaY, 0}
        End If

    End Sub

    Private Sub Estribos_Izquierda(ByRef Suma_Long As Single, ByRef delta As Single, ByRef Punto_inicial() As Double, ByVal Punto_final() As Double, Muro_i As Muros_Consolidados, ByRef Pos As Integer, Distancia_Limite As Double, i As Integer, k As Integer, ByVal Direccion As Integer, DeltaY As Double)

        For j = Pos To ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 2

            If ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).CoordenadasXyY(Direccion) <= Punto_final(Direccion) Then

                delta = ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).CoordenadasXyY(Direccion) - ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(Direccion)
                Suma_Long += delta

                'Determinar si la barra lleva gancho o no

                If j <> Pos Or Pos = 0 Then
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = True
                End If

                If Suma_Long + (0.02 * 2) >= Distancia_Limite Then

                    Add_Estribos("FC_ESTRIBOS", Math.PI / 2, Punto_inicial, Suma_Long, Muro_i.Bw(k) / 100, False)

                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = False 'Determina si la barra lleva gancho o no
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    Pos = j
                    j = Pos - 1

                    Punto_inicial = {ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).CoordenadasXyY(Direccion), DeltaY, 0}
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos + 1).Gancho = False
                    Suma_Long = 0

                End If

                If j + 1 = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1 And Suma_Long + (0.02 * 2) < Distancia_Limite Then

                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = False 'Determina si la barra lleva gancho o no

                    Add_Estribos("FC_ESTRIBOS", Math.PI / 2, Punto_inicial, Suma_Long, Muro_i.Bw(k) / 100, False)
                    Suma_Long = 0
                    Exit For
                End If
            Else
                ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).Gancho = True
                Exit For
            End If

        Next

    End Sub

    Private Sub Estribos_Derecha(ByRef Suma_Long As Single, ByRef delta As Single, ByRef Punto_inicial() As Double, ByRef Punto_final() As Double, Muro_i As Muros_Consolidados, ByRef Pos As Integer, Distancia_Limite As Double, i As Integer, k As Integer, ByVal Direccion As Integer, ByVal DeltaY As Double)

        For j = Pos To 1 Step -1

            If ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).CoordenadasXyY(Direccion) >= Punto_inicial(Direccion) Then

                delta = Math.Abs(ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).CoordenadasXyY(Direccion) - ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(Direccion))
                Suma_Long += delta

                'Determinar si la barra lleva gancho o no

                If j <> Pos Or Pos = 0 Then
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = True
                End If

                If Suma_Long + (0.038 * 2) >= Distancia_Limite Then


                    Add_Estribos("FC_ESTRIBOS", Math.PI / 2, Punto_final, Suma_Long, Muro_i.Bw(j) / 100, True)

                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).Gancho = False 'Determina si la barra lleva gancho o no
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False

                    Pos = j
                    j = Pos + 1

                    Punto_final = {ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).CoordenadasXyY(Direccion), DeltaY, 0}
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos - 1).Gancho = False
                    Suma_Long = 0

                End If

                If j - 1 = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1 And Suma_Long + (0.02 * 2) < Distancia_Limite Then

                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).Gancho = False 'Determina si la barra lleva gancho o no
                    Add_Estribos("FC_ESTRIBOS", Math.PI / 2, Punto_final, Suma_Long, Muro_i.Bw(k) / 100, False)
                    Suma_Long = 0
                    Exit For
                End If

            Else
                ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = True
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
    End Sub

End Class
