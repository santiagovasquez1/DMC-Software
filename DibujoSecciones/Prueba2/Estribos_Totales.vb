Imports Autodesk.AutoCAD.Interop
Imports Autodesk.AutoCAD.Interop.Common
Imports MathNet.Numerics.LinearAlgebra

Public Class Estribos_Totales

    Public Bloque_Estribo As AcadBlockReference
    Public Bloque_Gancho As AcadBlockReference
    Public Diametro_Estribo As Integer
    Public Separacion_Estribo As Single

    Sub Estribos_Pisos(ByRef Delta_X As Double, ByRef DeltaY As Single, ByVal Pos_Y As Single)

        Dim Suma_Long, delta As Single
        Dim Punto_inicial As Double()
        Dim Punto_final As Double()
        Dim Muro_i As Muros_Consolidados
        Dim Distancia_Limite, Distancia_Maxima As Double
        Dim Vecino_izquierda, Vecino_Derecha, Vecino_Arriba, Vecino_Abajo As Boolean
        Dim Distancia_Confinada As Double
        Dim Delta_reduccion As Double
        Dim Pos, Num_Estribos As Integer
        Dim Texto_Estribos As String
        Dim Pc As Double()
        Distancia_Maxima = 2

        For i = 0 To ListaOrdenada.Count - 1

            Muro_i = Muros_lista_2.Find(Function(x) x.Pier_name = ListaOrdenada(i).NombreMuro)

            If Muro_i.Lebe_Izq.FindAll(Function(x) x > 0).Count > 0 Or Muro_i.Lebe_Der.FindAll(Function(x) x > 0).Count > 0 Or Muro_i.Zc_Izq.FindAll(Function(x) x > 0).Count > 0 Or Muro_i.Zc_Der.FindAll(Function(x) x > 0).Count > 0 Then

                DeltaY = Pos_Y

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
                    Ordenar_Refuerzo_H(ListaOrdenada(i), Delta_X, DeltaY)

                    For j = Muro_i.Stories.Count - 1 To 0 Step -1

                        ''Caso en el cual el muro va totalmente confinado
                        If Muro_i.Rho_l(j) >= 0.01 Then

                            Delta_reduccion = 0
                            Pos = 0
                            Suma_Long = 0

                            Determinacion_Punto_Arranque_Horizontal(Punto_inicial, Muro_i, Vecino_izquierda, Delta_reduccion, i, Muro_vecino_izquierda, j, DeltaY, Delta_X)
                            Determinacion_Punto_Final_Horizontal(Punto_final, Muro_i, ListaOrdenada(i), Vecino_Derecha, Delta_reduccion, i, Muro_vecino_izquierda, j, DeltaY, Delta_X)

                            Distancia_Confinada = Math.Abs(Punto_final(0) - Punto_inicial(0))

                            If Distancia_Confinada > Distancia_Maxima Then
                                Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                            Else
                                Distancia_Limite = Distancia_Confinada - 0.04
                            End If

                            Diametro_Estribo = Muro_i.Est_ebe(j)
                            Separacion_Estribo = Muro_i.Sep_ebe(j) / 100
                            Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo)

                            ''Agregar texto
                            Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                            Pc = {Delta_X, Punto_inicial(1) - 0.3, 0}
                            Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 0)

                        Else

                            If Muro_i.Lebe_Izq(j) > 0 Or Muro_i.Zc_Izq(j) > 0 Then

                                Delta_reduccion = 0
                                Pos = 0
                                Suma_Long = 0

                                Determinacion_Punto_Arranque_Horizontal(Punto_inicial, Muro_i, Vecino_izquierda, Delta_reduccion, i, Muro_vecino_izquierda, j, DeltaY, Delta_X)
                                Distancia_Confinada = Determinacion_Confinamiento_LI(Muro_i, Vecino_izquierda, Muro_vecino_izquierda, j, Diametro_Estribo, Separacion_Estribo)

                                If Distancia_Confinada > Distancia_Maxima Then
                                    Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                    Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                                Else
                                    Distancia_Limite = Distancia_Confinada - 0.04
                                End If

                                Punto_final = {Punto_inicial(0) + Distancia_Confinada, Punto_inicial(1), 0}
                                Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo)

                                ''Agregar texto
                                Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                                Pc = {Delta_X, Punto_inicial(1) - 0.2, 0}
                                Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 1.0)

                            End If

                            If Muro_i.Lebe_Der(j) > 0 Or Muro_i.Zc_Der(j) > 0 Then

                                Delta_reduccion = 0
                                Pos = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1
                                Suma_Long = 0

                                Determinacion_Punto_Final_Horizontal(Punto_final, Muro_i, ListaOrdenada(i), Vecino_Derecha, Delta_reduccion, i, Muro_Vecino_derecha, j, DeltaY, Delta_X)
                                Distancia_Confinada = Determinacion_Confinamiento_Ld(Muro_i, Vecino_Derecha, Muro_Vecino_derecha, j, Diametro_Estribo, Separacion_Estribo)

                                If Distancia_Confinada > Distancia_Maxima Then
                                    Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                    Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                                Else
                                    Distancia_Limite = Distancia_Confinada - 0.04
                                End If

                                Punto_inicial = {Punto_final(0) - Distancia_Confinada, Punto_final(1), 0}
                                Estribos_Derecha(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo)

                                ''Agregar texto
                                Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                                Pc = {Punto_inicial(0), Punto_inicial(1) - 0.2, 0}
                                Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 1.0)

                            End If

                        End If

                        DeltaY += 0.65
                    Next

                End If

                If ListaOrdenada(i).DireccionMuro = "Vertical" Then

                    Ordenar_Refuerzo(ListaOrdenada(i), DeltaY, Delta_X)

                    Determinacion_Vecinos(Vecino_Abajo, Vecino_Arriba, i, Muro_Vecino_Abajo, Muro_Vecino_Arriba, ListaOrdenada(i).DireccionMuro)

                    For j = Muro_i.Stories.Count - 1 To 0 Step -1

                        Delta_reduccion = 0
                        Pos = 0
                        Suma_Long = 0

                        If Muro_i.Rho_l(j) > 0.01 Then

                            Determinacion_Punto_Arranque_Vertical(Punto_inicial, Muro_i, Vecino_Abajo, Delta_reduccion, i, Muro_Vecino_Abajo, j, DeltaY, Delta_X)
                            Determinacion_Punto_Final_Vertical(Punto_final, Muro_i, ListaOrdenada(i), Vecino_Arriba, Delta_reduccion, i, Muro_Vecino_Arriba, j, DeltaY, Delta_X)
                            Distancia_Confinada = Math.Abs(Punto_final(0) - Punto_inicial(0))

                            If Distancia_Confinada > Distancia_Maxima Then
                                Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                            Else
                                Distancia_Limite = Distancia_Confinada - 0.04
                            End If

                            Diametro_Estribo = Muro_i.Est_ebe(j)
                            Separacion_Estribo = Muro_i.Sep_ebe(j)
                            Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo)

                            ''Agregar texto
                            Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                            Pc = {Delta_X, Punto_inicial(1) - 0.3, 0}
                            Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 0)

                        Else
                            If Muro_i.Lebe_Izq(j) > 0 Or Muro_i.Zc_Izq(j) > 0 Then

                                Determinacion_Punto_Arranque_Vertical(Punto_inicial, Muro_i, Vecino_Abajo, Delta_reduccion, i, Muro_Vecino_Abajo, j, DeltaY, Delta_X)
                                Distancia_Confinada = Determinacion_Confinamiento_LI(Muro_i, Vecino_Abajo, Muro_Vecino_Abajo, j, Diametro_Estribo, Separacion_Estribo)

                                If Distancia_Confinada > Distancia_Maxima Then
                                    Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                    Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                                Else
                                    Distancia_Limite = Distancia_Confinada - 0.04
                                End If

                                Punto_final = {Punto_inicial(0) + Distancia_Confinada, Punto_inicial(1), 0}
                                Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo)

                                ''Agregar texto
                                Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                                Pc = {Delta_X, Punto_inicial(1) - 0.2, 0}
                                Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 1.0)
                            End If

                            If Muro_i.Lebe_Der(j) > 0 Or Muro_i.Zc_Der(j) > 0 Then

                                Determinacion_Punto_Final_Vertical(Punto_final, Muro_i, ListaOrdenada(i), Vecino_Arriba, Delta_reduccion, i, Muro_Vecino_Arriba, j, DeltaY, Delta_X)
                                Distancia_Confinada = Determinacion_Confinamiento_Ld(Muro_i, Vecino_Arriba, Muro_Vecino_Arriba, j, Diametro_Estribo, Separacion_Estribo)

                                If Distancia_Confinada > Distancia_Maxima Then
                                    Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                    Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                                Else
                                    Distancia_Limite = Distancia_Confinada - 0.04
                                End If

                                Punto_inicial = {Punto_final(0) - Distancia_Confinada, Punto_final(1), 0}
                                Pos = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1
                                Estribos_Derecha(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo)

                                ''Agregar texto
                                Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                                Pc = {Punto_inicial(0), Punto_inicial(1) - 0.2, 0}
                                Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 1.0)

                            End If

                        End If

                        DeltaY += 0.65
                    Next

                End If

                Delta_X += Punto_final(0) - Delta_X + 2
            End If


        Next
        AcadDoc.Regen(AcRegenType.acActiveViewport)
    End Sub

    Private Shared Function Determinacion_Confinamiento_Ld(Muro_i As Muros_Consolidados, Vecino_dir As Boolean, Muro_Vecino_dir As Muros_Consolidados, j As Integer, ByRef Diametro_Estribo As Integer, ByRef Sep As Single) As Double
        Dim Distancia_Confinada As Double

        If Muro_i.Lebe_Der(j) > 0 Then
            If Vecino_dir = True Then
                Distancia_Confinada = (Muro_Vecino_dir.Bw(j) + Muro_i.Lebe_Der(j)) / 100
            Else
                Distancia_Confinada = Muro_i.Lebe_Der(j) / 100
            End If
            Diametro_Estribo = Muro_i.Est_ebe(j)
            Sep = Muro_i.Sep_ebe(j) / 100
        Else
            If Vecino_dir = True Then
                Distancia_Confinada = (Muro_Vecino_dir.Bw(j) + Muro_i.Zc_Der(j)) / 100
            Else
                Distancia_Confinada = Muro_i.Zc_Der(j) / 100
            End If
            Diametro_Estribo = Muro_i.Est_Zc(j)
            Sep = Muro_i.Sep_Zc(j) / 100
        End If

        Return Distancia_Confinada
    End Function

    Private Shared Function Determinacion_Confinamiento_LI(Muro_i As Muros_Consolidados, Vecino_dir As Boolean, Muro_Vecino_dir As Muros_Consolidados, j As Integer, ByRef Diametro_Estribo As Integer, ByRef Sep As Single) As Double

        Dim Distancia_Confinada As Double

        If Muro_i.Lebe_Izq(j) > 0 Then

            If Vecino_dir = True Then
                Distancia_Confinada = (Muro_Vecino_dir.Bw(j) + Muro_i.Lebe_Izq(j)) / 100
            Else
                Distancia_Confinada = Muro_i.Lebe_Izq(j) / 100
            End If

            Diametro_Estribo = Muro_i.Est_ebe(j)
            Sep = Muro_i.Sep_ebe(j) / 100
        Else
            If Vecino_dir = True Then
                Distancia_Confinada = (Muro_Vecino_dir.Bw(j) + Muro_i.Zc_Izq(j)) / 100
            Else
                Distancia_Confinada = Muro_i.Zc_Izq(j) / 100
            End If
            Diametro_Estribo = Muro_i.Est_Zc(j)
            Sep = Muro_i.Sep_Zc(j) / 100
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

    Private Shared Sub Determinacion_Punto_Final_Horizontal(ByRef Punto_final() As Double, Muro_i As Muros_Consolidados, Muro_D As Muros, Vecino_derecha As Boolean, ByRef Delta_reduccion As Double, i As Integer, Muro_vecino As Muros_Consolidados, j As Integer, ByVal DeltaY As Double, ByVal Delta_X As Double)

        Dim Longitud_Muro As Double


        Longitud_Muro = Muro_D.XmaxE - Muro_D.XminE


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

            Punto_final = {Delta_X + Longitud_Muro - Delta_reduccion - 0.038, DeltaY, 0}
        Else
            Punto_final = {Delta_X + Longitud_Muro - Delta_reduccion - 0.038, DeltaY, 0}
        End If

    End Sub

    Private Shared Sub Determinacion_Punto_Final_Vertical(ByRef Punto_final() As Double, Muro_i As Muros_Consolidados, ByVal Muro_D As Muros, Vecino_arriba As Boolean, ByRef Delta_reduccion As Double, i As Integer, Muro_vecino As Muros_Consolidados, j As Integer, ByVal DeltaY As Double, ByVal Delta_X As Double)

        Dim Longitud_Muro As Double

        Longitud_Muro = Muro_D.YmaxE - Muro_D.YminE

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
            Punto_final = {Delta_X + Longitud_Muro - Delta_reduccion - 0.038, DeltaY, 0}
        Else
            Punto_final = {Delta_X + Longitud_Muro - Delta_reduccion - 0.038, DeltaY, 0}
        End If

    End Sub

    Private Sub Estribos_Izquierda(ByRef Suma_Long As Single, ByRef delta As Single, ByRef Punto_inicial() As Double, ByVal Punto_final() As Double, Muro_i As Muros_Consolidados, ByRef Pos As Integer, Distancia_Limite As Double, i As Integer, k As Integer, ByVal Direccion As Integer, DeltaY As Double, ByVal Diametro As Integer)

        For j = Pos To ListaOrdenada(i).Lista_Refuerzos_Original.Count - 2

            If Math.Round(ListaOrdenada(i).Lista_Refuerzos_Original(j + 1)(Direccion), 2) <= Math.Round(Punto_final(0), 2) Then

                delta = ListaOrdenada(i).Lista_Refuerzos_Original(j + 1)(Direccion) - ListaOrdenada(i).Lista_Refuerzos_Original(j)(Direccion)
                Suma_Long += delta

                'Determinar si la barra lleva gancho o no

                If j <> Pos Or Pos = 0 Then
                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = True
                End If

                If Suma_Long + (0.038 * 2) >= Distancia_Limite Then

                    Add_Estribos("FC_ESTRIBOS", 0, Punto_inicial, Suma_Long, Muro_i.Bw(k) / 100, Diametro, False)

                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = False 'Determina si la barra lleva gancho o no
                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    Pos = j
                    j = Pos - 1

                    Punto_inicial = {ListaOrdenada(i).Lista_Refuerzos_Original(Pos)(Direccion), DeltaY, 0}
                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos + 1).Gancho = False
                    Suma_Long = 0

                End If

                If j + 1 = ListaOrdenada(i).Lista_Refuerzos_Original.Count - 1 And Suma_Long + (0.02 * 2) < Distancia_Limite Then

                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = False 'Determina si la barra lleva gancho o no

                    Add_Estribos("FC_ESTRIBOS", 0, Punto_inicial, Suma_Long, Muro_i.Bw(k) / 100, Diametro, False)
                    Suma_Long = 0
                    Exit For
                End If
            Else
                'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).Gancho = True
                Exit For
            End If

        Next

    End Sub

    Private Sub Estribos_Derecha(ByRef Suma_Long As Single, ByRef delta As Single, ByRef Punto_inicial() As Double, ByRef Punto_final() As Double, Muro_i As Muros_Consolidados, ByRef Pos As Integer, Distancia_Limite As Double, i As Integer, k As Integer, ByVal Direccion As Integer, ByVal DeltaY As Double, ByVal Diametro As Integer)

        For j = Pos To 1 Step -1

            If Math.Round(ListaOrdenada(i).Lista_Refuerzos_Original(j - 1)(Direccion), 2) >= Math.Round(Punto_inicial(Direccion), 2) Then

                delta = Math.Abs(ListaOrdenada(i).Lista_Refuerzos_Original(j - 1)(Direccion) - ListaOrdenada(i).Lista_Refuerzos_Original(j)(Direccion))
                Suma_Long += delta

                'Determinar si la barra lleva gancho o no

                If j <> Pos Or Pos = 0 Then
                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = True
                End If

                If Suma_Long + (0.038 * 2) >= Distancia_Limite Then


                    Add_Estribos("FC_ESTRIBOS", 0, Punto_final, Suma_Long, Muro_i.Bw(k) / 100, Diametro, True)

                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).Gancho = False 'Determina si la barra lleva gancho o no
                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False

                    Pos = j
                    j = Pos + 1

                    Punto_final = {ListaOrdenada(i).Lista_Refuerzos_Original(Pos)(Direccion), DeltaY, 0}
                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos - 1).Gancho = False
                    Suma_Long = 0

                End If

                If j - 1 = ListaOrdenada(i).Lista_Refuerzos_Original.Count - 1 And Suma_Long + (0.02 * 2) < Distancia_Limite Then

                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(Pos).Gancho = False
                    'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j - 1).Gancho = False 'Determina si la barra lleva gancho o no
                    Add_Estribos("FC_ESTRIBOS", 0, Punto_final, Suma_Long, Muro_i.Bw(k) / 100, Diametro, False)
                    Suma_Long = 0
                    Exit For
                End If

            Else
                'ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j + 1).Gancho = True
                Exit For
            End If

        Next
    End Sub

    Private Sub Add_Estribos(ByVal Layer As String, ByVal Angulo As Double, ByVal Ip As Double(), ByVal Distancia As Double, ByVal Espesor_Doble As Double, ByVal Diametro As Integer, ByVal Mover As Boolean)

        Dim Nombre_Bloque As String
        Dim dynamic_property1 As Object
        Dim editar_property1 As AcadDynamicBlockReferenceProperty
        Dim Point_aux As Double()

        Nombre_Bloque = "FC_B_Estribo tipo 6"

        If Mover = False Then
            Bloque_Estribo = AcadDoc.ModelSpace.InsertBlock(Ip, Nombre_Bloque, 1, 1, 1, Angulo)
        Else
            Point_aux = {Ip(0) - Distancia, Ip(1), 0}
            Bloque_Estribo = AcadDoc.ModelSpace.InsertBlock(Point_aux, Nombre_Bloque, 1, 1, 1, Angulo)
        End If

        dynamic_property1 = Bloque_Estribo.GetDynamicBlockProperties

        editar_property1 = dynamic_property1(0)
        editar_property1.Value = Distancia + 2 * 0.038

        editar_property1 = dynamic_property1(2)
        editar_property1.Value = 0.25

        editar_property1 = dynamic_property1(4)
        editar_property1.Value = Espesor_Doble

        editar_property1 = dynamic_property1(6)
        editar_property1.Value = Find_Long_Ganchos(Diametro)

        Bloque_Estribo.Layer = Layer
        Bloque_Estribo.Update()
    End Sub

    Public Function Find_Long_Ganchos(ByVal Diametro As Integer) As Double
        Dim Longitud As Double

        Select Case Diametro
            Case 3
                Longitud = 0.094
            Case 4
                Longitud = 0.125
            Case 5
                Longitud = 0.157
        End Select

        Return Longitud
    End Function

    Private Sub Ordenar_Refuerzo_H(ByRef Muro_D As Muros, ByVal Delta_X As Double, ByVal Delta_Y As Double)

        Dim Dix, Diy As Double
        Dim Vector_Traslacion As List(Of Double)
        Dim Xmin1, Ymin1 As Double

        Xmin1 = Muro_D.Lista_Refuerzos_Original.Select(Function(x) x(0)).Min
        Ymin1 = Muro_D.Lista_Refuerzos_Original.Select(Function(x) x(1)).Min

        Dix = (Delta_X + 0.038) - Xmin1
        Diy = Delta_Y - Ymin1

        For i = 0 To Muro_D.Lista_Refuerzos_Original.Count - 1

            Vector_Traslacion = Traslacion(Dix, Diy, Muro_D.Lista_Refuerzos_Original(i)(0), Muro_D.Lista_Refuerzos_Original(i)(1))
            Muro_D.Lista_Refuerzos_Original(i) = Vector_Traslacion.ToArray
            'AcadDoc.ModelSpace.AddCircle(Muro_D.Lista_Refuerzos_Original(i), 0.01)
        Next

    End Sub

    Private Sub Ordenar_Refuerzo(ByRef Muro_D As Muros, ByVal Delta_y As Double, ByVal Delta_X As Double)

        Dim Dix, Diy As Double
        Dim Rotacion As Double()
        Dim Vector_Origen As Double()
        Dim Vector_Traslacion As List(Of Double)
        Dim Lista_aux As New List(Of RefuerzoCirculo)
        Dim Xmin1, Ymin1 As Double

        Vector_Origen = {Muro_D.XminE, Muro_D.YminE}

        For i = 0 To Muro_D.Lista_Refuerzos_Original.Count - 1


            Rotacion = Rotar_Refuerzo(Muro_D.Lista_Refuerzos_Original(i)(0), Muro_D.Lista_Refuerzos_Original(i)(1), Math.PI / 2).ToArray
            Muro_D.Lista_Refuerzos_Original(i) = Rotacion
        Next

        Xmin1 = Muro_D.Lista_Refuerzos_Original.Select(Function(x) x(0)).Min
        Ymin1 = Muro_D.Lista_Refuerzos_Original.Select(Function(x) x(1)).Min

        Dix = (Delta_X + 0.038) - Xmin1
        Diy = Delta_y - Ymin1

        For i = 0 To Muro_D.Lista_Refuerzos_Original.Count - 1

            Vector_Traslacion = Traslacion(Dix, Diy, Muro_D.Lista_Refuerzos_Original(i)(0), Muro_D.Lista_Refuerzos_Original(i)(1))
            Muro_D.Lista_Refuerzos_Original(i) = Vector_Traslacion.ToArray

        Next

    End Sub

    Public Function Traslacion(ByVal Xc As Double, ByVal Yc As Double, ByVal Xi As Double, ByVal Yi As Double) As List(Of Double)

        Dim O As Matrix(Of Double)
        Dim P As Matrix(Of Double)
        Dim T As Matrix(Of Double)
        Dim T_aux As List(Of Double) = New List(Of Double)

        P = Matrix(Of Double).Build.DenseOfArray({{1, 0, Xc}, {0, 1, Yc}, {0, 0, 1}})
        O = Matrix(Of Double).Build.DenseOfArray({{Xi}, {Yi}, {1}})
        T = P * O

        For i = 0 To T.RowCount - 1
            T_aux.Add(T(i, 0))
        Next

        Return T_aux

    End Function

    Private Function Rotar_Refuerzo(ByVal Dix As Double, Diy As Double, ByVal theta As Double) As List(Of Double)

        Dim Original As Matrix(Of Double)
        Dim Rotacion As Matrix(Of Double)
        Dim Producto_Punto As Matrix(Of Double)
        Dim M_aux As New List(Of Double)

        Original = Matrix(Of Double).Build.DenseOfArray({{Dix, Diy}})
        Rotacion = Matrix(Of Double).Build.DenseOfArray({{Math.Cos(theta), -Math.Sin(theta)}, {Math.Sin(theta), Math.Cos(theta)}})

        Producto_Punto = Original * Rotacion

        For i = 0 To Producto_Punto.ColumnCount - 1
            M_aux.Add(Producto_Punto(0, i))
        Next

        M_aux.Add(0)
        Return M_aux

    End Function

    Private Shared Sub Add_Texto(ByVal Texto_1 As String, ByVal P_texto As Double(), ByVal Layer As String, ByVal Style As String, ByVal Angulo As Double, ByVal Ancho As Single)

        Dim texto_Estribo As AcadMText
        texto_Estribo = AcadDoc.ModelSpace.AddMText(P_texto, 0.75, Texto_1)

        With texto_Estribo
            .Layer = Layer
            .StyleName = Style
            .Height = 0.05
            .Rotation = Angulo
            .Width = Ancho
        End With

    End Sub

End Class
