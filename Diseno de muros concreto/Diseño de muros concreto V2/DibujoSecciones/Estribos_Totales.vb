Imports Autodesk.AutoCAD.Interop.Common
Imports MathNet.Numerics.LinearAlgebra

Public Class Estribos_Totales

    Public Bloque_Estribo As AcadBlockReference
    Public Bloque_Gancho As AcadBlockReference
    Public Tabla_Autocad As AcadTable
    Public Pisos_Estribos As List(Of String)
    Public Diametro_Estribo As Integer
    Public Separacion_Estribo As Single
    Public Coord_Tabla As Double()
    Public Rectangulo As AcadLWPolyline
    Public Lista_Rect As New List(Of AcadLWPolyline)
    Public Lista_Cantidades_i As Lista_Cantidades

    Sub Estribos_Pisos(ByRef Delta_X As Double, ByRef DeltaY As Single, ByVal Pos_Y As Single, ByRef Lista_Cantidades As Lista_Cantidades)

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
        Dim Pattern_name, Layer_Hatch As String
        Dim Pc As Double()
        Dim Coordenadas_Texto As Double()
        Dim Lista_ganchos As List(Of Boolean)
        Dim Lista_sep_Ganchos As List(Of Double)
        Dim Diametro_gancho As List(Of Integer)

        Lista_Cantidades_i = Lista_Cantidades

        Distancia_Maxima = 2

        For i = 0 To ListaOrdenada.Count - 1

            Muro_i = Muros_lista_2.Find(Function(x) x.Pier_name = ListaOrdenada(i).NombreMuro)

            If Lista_Cantidades_i.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = Muro_i.Pier_name).Lista_Estribos Is Nothing = True Then
                Lista_Cantidades_i.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = Muro_i.Pier_name).Lista_Estribos = New List(Of Seccion_Estribos)
            Else
                Lista_Cantidades_i.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = Muro_i.Pier_name).Lista_Estribos.RemoveAll(Function(x) x.Pier = Muro_i.Pier_name)
            End If

            If Lista_Cantidades_i.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = Muro_i.Pier_name).Lista_Ganchos Is Nothing = True Then
                Lista_Cantidades_i.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = Muro_i.Pier_name).Lista_Ganchos = New List(Of Seccion_Ganchos)
            Else
                Lista_Cantidades_i.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = Muro_i.Pier_name).Lista_Ganchos.RemoveAll(Function(x) x.Pier = Muro_i.Pier_name)
            End If

            If Muro_i.Lebe_Izq.FindAll(Function(x) x > 0).Count > 0 Or Muro_i.Lebe_Der.FindAll(Function(x) x > 0).Count > 0 Or Muro_i.Zc_Izq.FindAll(Function(x) x > 0).Count > 0 Or Muro_i.Zc_Der.FindAll(Function(x) x > 0).Count > 0 Then

                DeltaY = Pos_Y

                Dim Muro_vecino_izquierda As Muros_Consolidados = New Muros_Consolidados
                Dim Muro_Vecino_derecha As Muros_Consolidados = New Muros_Consolidados
                Dim Muro_Vecino_Arriba As Muros_Consolidados = New Muros_Consolidados
                Dim Muro_Vecino_Abajo As Muros_Consolidados = New Muros_Consolidados
                Dim Longitud_Real As Double

                Vecino_Derecha = False
                Vecino_izquierda = False
                Vecino_Arriba = False
                Vecino_Abajo = False

                Pisos_Estribos = New List(Of String)
                Lista_Rect = New List(Of AcadLWPolyline)

                If ListaOrdenada(i).DireccionMuro = "Horizontal" Then

                    ''Primer paso para el inicio del dibujo
                    Determinacion_Vecinos(Vecino_izquierda, Vecino_Derecha, i, Muro_vecino_izquierda, Muro_Vecino_derecha, ListaOrdenada(i).DireccionMuro)
                    Ordenar_Refuerzo_H(ListaOrdenada(i), Delta_X, DeltaY)
                    Longitud_Real = ListaOrdenada(i).XmaxE - ListaOrdenada(i).XminE

                    For j = Muro_i.Stories.Count - 1 To 0 Step -1

                        Delta_reduccion = 0
                        Pos = 0
                        Suma_Long = 0
                        Lista_ganchos = New List(Of Boolean)
                        Lista_sep_Ganchos = New List(Of Double)
                        Diametro_gancho = New List(Of Integer)

                        For k = 0 To ListaOrdenada(i).Lista_Refuerzos_Original.Count - 1
                            Lista_ganchos.Add(False)
                            Lista_sep_Ganchos.Add(0)
                            Diametro_gancho.Add(0)
                        Next

                        If Muro_i.Lebe_Izq(j) > 0 Or Muro_i.Lebe_Der(j) > 0 Or Muro_i.Zc_Izq(j) > 0 Or Muro_i.Zc_Der(j) > 0 Then
                            Pisos_Estribos.Add(Muro_i.Stories(j))
                        End If

                        ''Caso en el cual el muro va totalmente confinado
                        If Muro_i.Rho_l(j) >= 0.01 Or Muro_i.Lebe_Izq(j) >= Muro_i.lw(i) Then

                            Determinacion_Punto_Arranque_Horizontal(Punto_inicial, Muro_i, Vecino_izquierda, Delta_reduccion, i, Muro_vecino_izquierda, j, DeltaY, Delta_X)
                            Determinacion_Punto_Final_Horizontal(Punto_final, Muro_i, ListaOrdenada(i), Vecino_Derecha, Delta_reduccion, i, Muro_vecino_izquierda, j, DeltaY, Delta_X)

                            Distancia_Confinada = Math.Abs(Punto_final(0) - Punto_inicial(0))

                            If Distancia_Confinada > Distancia_Maxima Then
                                Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                            Else
                                Num_Estribos = 1
                                Distancia_Limite = Distancia_Confinada - 0.04
                            End If

                            Diametro_Estribo = Muro_i.Est_ebe(j)
                            Separacion_Estribo = Muro_i.Sep_ebe(j) / 100

                            If Vecino_Derecha = True Then
                                Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo, Lista_ganchos, Vecino_izquierda, Vecino_Derecha, Muro_Vecino_derecha.Bw(j) / 100, Num_Estribos, Lista_sep_Ganchos, Diametro_gancho)
                            Else
                                Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo, Lista_ganchos, Vecino_izquierda, Vecino_Derecha, 0, Num_Estribos, Lista_sep_Ganchos, Diametro_gancho)
                            End If

                            ''Agregar texto
                            Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")

                            Pc = {Delta_X, Punto_inicial(1) - 0.3, 0}
                            Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 0, AcAttachmentPoint.acAttachmentPointTopLeft)
                            Add_Rectangulo(Rectangulo, {Delta_X - 0.2, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) + 0.25, Delta_X - 0.2, Punto_inicial(1) + 0.25}, "FC_BORDES", True, Lista_Rect)

                            ''Agregar Hatch
                            Pattern_name = "SOLID"
                            Layer_Hatch = "FC_HATCH MUROS"
                            Add_Hatch(Lista_Rect.Last, Pattern_name, Layer_Hatch, 0.9)

                            ''Agregrar Cuadro piso y texto

                            Coordenadas_Texto = {Lista_Rect.Last.Coordinates(0) - 0.75, Lista_Rect.Last.Coordinates(1), Lista_Rect.Last.Coordinates(0), Lista_Rect.Last.Coordinates(1), Lista_Rect.Last.Coordinates(0), Lista_Rect.Last.Coordinates(7), Lista_Rect.First.Coordinates(0) - 0.75, Lista_Rect.Last.Coordinates(7)}
                            Add_Rectangulo(Rectangulo, Coordenadas_Texto, "FC_BORDES", True, Lista_Rect)

                            Coordenadas_Texto = {Lista_Rect.Last.Coordinates(0) + (Lista_Rect.Last.Coordinates(2) - Lista_Rect.Last.Coordinates(0)) / 2, Lista_Rect.Last.Coordinates(1) + (Lista_Rect.Last.Coordinates(7) - Lista_Rect.Last.Coordinates(1)) / 2, 0}
                            Texto_Estribos = Muro_i.Stories(j)
                            Add_Texto(Texto_Estribos, Coordenadas_Texto, "FC_R-80", "FC_TEXT1", 0, 0, AcAttachmentPoint.acAttachmentPointMiddleCenter)
                        Else

                            If Muro_i.Lebe_Izq(j) > 0 Or Muro_i.Zc_Izq(j) > 0 Then

                                Determinacion_Punto_Arranque_Horizontal(Punto_inicial, Muro_i, Vecino_izquierda, Delta_reduccion, i, Muro_vecino_izquierda, j, DeltaY, Delta_X)
                                Distancia_Confinada = Determinacion_Confinamiento_LI(Muro_i, Vecino_izquierda, Muro_vecino_izquierda, j, Diametro_Estribo, Separacion_Estribo, Pattern_name, Layer_Hatch)

                                If Distancia_Confinada > Distancia_Maxima Then
                                    Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                    Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                                Else
                                    Num_Estribos = 1
                                    Distancia_Limite = Distancia_Confinada - 0.04
                                End If

                                Punto_final = {Punto_inicial(0) + Distancia_Confinada, Punto_inicial(1), 0}

                                If Vecino_Derecha = True Then
                                    Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo, Lista_ganchos, Vecino_izquierda, Vecino_Derecha, Muro_Vecino_derecha.Bw(j) / 100, Num_Estribos, Lista_sep_Ganchos, Diametro_gancho)
                                Else
                                    Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo, Lista_ganchos, Vecino_izquierda, Vecino_Derecha, 0, Num_Estribos, Lista_sep_Ganchos, Diametro_gancho)
                                End If

                                ''Agregar texto
                                Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                                Pc = {Delta_X, Punto_inicial(1) - 0.2, 0}
                                Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 1.2, AcAttachmentPoint.acAttachmentPointTopLeft)
                                Add_Rectangulo(Rectangulo, {Delta_X - 0.2, Punto_inicial(1) - 0.4, Delta_X + Distancia_Confinada + 0.25, Punto_inicial(1) - 0.4, Delta_X + Distancia_Confinada + 0.25, Punto_inicial(1) + 0.25, Delta_X - 0.2, Punto_inicial(1) + 0.25}, "FC_BORDES", True, Lista_Rect)

                                ''Agregar Hatch
                                Add_Hatch(Lista_Rect.Last, Pattern_name, Layer_Hatch, 0.9)

                                ''Agregrar Cuadro piso y texto
                                Coordenadas_Texto = {Lista_Rect.Last.Coordinates(0) - 0.75, Lista_Rect.Last.Coordinates(1), Lista_Rect.Last.Coordinates(0), Lista_Rect.Last.Coordinates(1), Lista_Rect.Last.Coordinates(0), Lista_Rect.Last.Coordinates(7), Lista_Rect.First.Coordinates(0) - 0.75, Lista_Rect.Last.Coordinates(7)}
                                Add_Rectangulo(Rectangulo, Coordenadas_Texto, "FC_BORDES", True, Lista_Rect)

                                Coordenadas_Texto = {Lista_Rect.Last.Coordinates(0) + (Lista_Rect.Last.Coordinates(2) - Lista_Rect.Last.Coordinates(0)) / 2, Lista_Rect.Last.Coordinates(1) + (Lista_Rect.Last.Coordinates(7) - Lista_Rect.Last.Coordinates(1)) / 2, 0}
                                Texto_Estribos = Muro_i.Stories(j)
                                Add_Texto(Texto_Estribos, Coordenadas_Texto, "FC_R-80", "FC_TEXT1", 0, 0, AcAttachmentPoint.acAttachmentPointMiddleCenter)

                            End If

                            If Muro_i.Lebe_Der(j) > 0 Or Muro_i.Zc_Der(j) > 0 Then

                                Pos = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1

                                Determinacion_Punto_Final_Horizontal(Punto_final, Muro_i, ListaOrdenada(i), Vecino_Derecha, Delta_reduccion, i, Muro_Vecino_derecha, j, DeltaY, Delta_X)
                                Distancia_Confinada = Determinacion_Confinamiento_Ld(Muro_i, Vecino_Derecha, Muro_Vecino_derecha, j, Diametro_Estribo, Separacion_Estribo, Pattern_name, Layer_Hatch)

                                If Distancia_Confinada > Distancia_Maxima Then
                                    Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                    Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                                Else
                                    Num_Estribos = 1
                                    Distancia_Limite = Distancia_Confinada - 0.04
                                End If

                                Punto_inicial = {Punto_final(0) - Distancia_Confinada, Punto_final(1), 0}
                                Estribos_Derecha(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo, Lista_ganchos, Vecino_Derecha, Num_Estribos, Lista_sep_Ganchos, Diametro_gancho)

                                ''Agregar texto
                                Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                                Pc = {Punto_inicial(0), Punto_inicial(1) - 0.2, 0}
                                Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 1.2, AcAttachmentPoint.acAttachmentPointTopLeft)
                                Add_Rectangulo(Rectangulo, {Punto_inicial(0) - 0.2, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) + 0.25, Punto_inicial(0) - 0.2, Punto_inicial(1) + 0.25}, "FC_BORDES", True, Lista_Rect)

                                ''Agregar Hatch
                                Add_Hatch(Lista_Rect.Last, Pattern_name, Layer_Hatch, 0.9)

                                If Muro_i.Lebe_Izq(j) = 0 And Muro_i.Zc_Izq(j) = 0 Then

                                    ''Agregrar Cuadro piso y texto
                                    Coordenadas_Texto = {Delta_X - 0.95, Lista_Rect.Last.Coordinates(1), Delta_X - 0.2, Lista_Rect.Last.Coordinates(1), Delta_X - 0.2, Lista_Rect.Last.Coordinates(7), Delta_X - 0.95, Lista_Rect.Last.Coordinates(7)}
                                    Add_Rectangulo(Rectangulo, Coordenadas_Texto, "FC_BORDES", True, Lista_Rect)

                                    Coordenadas_Texto = {Lista_Rect.Last.Coordinates(0) + (Lista_Rect.Last.Coordinates(2) - Lista_Rect.Last.Coordinates(0)) / 2, Lista_Rect.Last.Coordinates(1) + (Lista_Rect.Last.Coordinates(7) - Lista_Rect.Last.Coordinates(1)) / 2, 0}
                                    Texto_Estribos = Muro_i.Stories(j)
                                    Add_Texto(Texto_Estribos, Coordenadas_Texto, "FC_R-80", "FC_TEXT1", 0, 0, AcAttachmentPoint.acAttachmentPointMiddleCenter)
                                End If

                            End If

                        End If

                        Add_Rectangulo(Rectangulo, {Delta_X - 0.2, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) + 0.25, Delta_X - 0.2, Punto_inicial(1) + 0.25}, "FC_BORDES", True, Lista_Rect)
                        Add_Ganchos(Lista_ganchos, Lista_sep_Ganchos, ListaOrdenada(i).Lista_Refuerzos_Original, DeltaY, Delta_X + Longitud_Real + 0.25, Muro_i.Pier_name, Muro_i.Stories(j), Muro_i.Bw(j), Diametro_gancho, Muro_i.Hw(i))

                        Dim prueba = Lista_Cantidades_i.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = Muro_i.Pier_name).Lista_Ganchos.FindAll(Function(x) x.Pier = Muro_i.Pier_name And x.Story = Muro_i.Stories(j)).ToList()
                        Dim prueba2 = prueba.Select(Function(x) x.Separacion).Distinct().ToList

                        ''Dibujo de numero de ganchos totales en el piso por separacion
                        Ganchos_Totales_piso(prueba, prueba2, Delta_X + Longitud_Real + 0.25, DeltaY)
                        DeltaY += 0.65
                    Next

                End If

                If ListaOrdenada(i).DireccionMuro = "Vertical" Then

                    Ordenar_Refuerzo(ListaOrdenada(i), DeltaY, Delta_X)
                    Longitud_Real = ListaOrdenada(i).YmaxE - ListaOrdenada(i).YminE
                    Determinacion_Vecinos(Vecino_Abajo, Vecino_Arriba, i, Muro_Vecino_Abajo, Muro_Vecino_Arriba, ListaOrdenada(i).DireccionMuro)

                    For j = Muro_i.Stories.Count - 1 To 0 Step -1

                        Delta_reduccion = 0
                        Pos = 0
                        Suma_Long = 0
                        Lista_ganchos = New List(Of Boolean)
                        Lista_sep_Ganchos = New List(Of Double)
                        Diametro_gancho = New List(Of Integer)

                        For k = 0 To ListaOrdenada(i).Lista_Refuerzos_Original.Count - 1
                            Lista_ganchos.Add(False)
                            Lista_sep_Ganchos.Add(0)
                            Diametro_gancho.Add(0)
                        Next

                        If Muro_i.Lebe_Izq(j) > 0 Or Muro_i.Lebe_Der(j) > 0 Or Muro_i.Zc_Izq(j) > 0 Or Muro_i.Zc_Der(j) > 0 Then
                            Pisos_Estribos.Add(Muro_i.Stories(j))
                        End If

                        If Muro_i.Rho_l(j) >= 0.01 Or Muro_i.Lebe_Izq(j) >= Muro_i.lw(i) Then

                            Determinacion_Punto_Arranque_Vertical(Punto_inicial, Muro_i, Vecino_Abajo, Delta_reduccion, i, Muro_Vecino_Abajo, j, DeltaY, Delta_X)
                            Determinacion_Punto_Final_Vertical(Punto_final, Muro_i, ListaOrdenada(i), Vecino_Arriba, Delta_reduccion, i, Muro_Vecino_Arriba, j, DeltaY, Delta_X)
                            Distancia_Confinada = Math.Abs(Punto_final(0) - Punto_inicial(0))

                            If Distancia_Confinada > Distancia_Maxima Then
                                Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                            Else
                                Num_Estribos = 1
                                Distancia_Limite = Distancia_Confinada - 0.04
                            End If

                            Diametro_Estribo = Muro_i.Est_ebe(j)
                            Separacion_Estribo = Muro_i.Sep_ebe(j) / 100

                            If Vecino_Arriba = True Then
                                Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo, Lista_ganchos, Vecino_Abajo, Vecino_Arriba, Muro_Vecino_Arriba.Bw(j) / 100, Num_Estribos, Lista_sep_Ganchos, Diametro_gancho)
                            Else
                                Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo, Lista_ganchos, Vecino_Abajo, Vecino_Arriba, 0, Num_Estribos, Lista_sep_Ganchos, Diametro_gancho)
                            End If

                            ''Agregar texto
                            Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                            Pc = {Delta_X, Punto_inicial(1) - 0.3, 0}
                            Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 0, AcAttachmentPoint.acAttachmentPointTopLeft)
                            Add_Rectangulo(Rectangulo, {Delta_X - 0.2, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) + 0.25, Delta_X - 0.2, Punto_inicial(1) + 0.25}, "FC_BORDES", True, Lista_Rect)

                            ''Agregar Hatch
                            Pattern_name = "SOLID"
                            Layer_Hatch = "FC_HATCH MUROS"
                            Add_Hatch(Lista_Rect.Last, Pattern_name, Layer_Hatch, 0.9)

                            ''Agregrar Cuadro piso y texto

                            Coordenadas_Texto = {Lista_Rect.Last.Coordinates(0) - 0.75, Lista_Rect.Last.Coordinates(1), Lista_Rect.Last.Coordinates(0), Lista_Rect.Last.Coordinates(1), Lista_Rect.Last.Coordinates(0), Lista_Rect.Last.Coordinates(7), Lista_Rect.First.Coordinates(0) - 0.75, Lista_Rect.Last.Coordinates(7)}
                            Add_Rectangulo(Rectangulo, Coordenadas_Texto, "FC_BORDES", True, Lista_Rect)

                            Coordenadas_Texto = {Lista_Rect.Last.Coordinates(0) + (Lista_Rect.Last.Coordinates(2) - Lista_Rect.Last.Coordinates(0)) / 2, Lista_Rect.Last.Coordinates(1) + (Lista_Rect.Last.Coordinates(7) - Lista_Rect.Last.Coordinates(1)) / 2, 0}
                            Texto_Estribos = Muro_i.Stories(j)
                            Add_Texto(Texto_Estribos, Coordenadas_Texto, "FC_R-80", "FC_TEXT1", 0, 0, AcAttachmentPoint.acAttachmentPointMiddleCenter)
                        Else
                            If Muro_i.Lebe_Izq(j) > 0 Or Muro_i.Zc_Izq(j) > 0 Then

                                Determinacion_Punto_Arranque_Vertical(Punto_inicial, Muro_i, Vecino_Abajo, Delta_reduccion, i, Muro_Vecino_Abajo, j, DeltaY, Delta_X)
                                Distancia_Confinada = Determinacion_Confinamiento_LI(Muro_i, Vecino_Abajo, Muro_Vecino_Abajo, j, Diametro_Estribo, Separacion_Estribo, Pattern_name, Layer_Hatch)

                                If Distancia_Confinada > Distancia_Maxima Then
                                    Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                    Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                                Else
                                    Num_Estribos = 1
                                    Distancia_Limite = Distancia_Confinada - 0.04
                                End If

                                Punto_final = {Punto_inicial(0) + Distancia_Confinada, Punto_inicial(1), 0}

                                If Vecino_Arriba = True Then
                                    Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo, Lista_ganchos, Vecino_Abajo, Vecino_Arriba, Muro_Vecino_Arriba.Bw(j) / 100, Num_Estribos, Lista_sep_Ganchos, Diametro_gancho)
                                Else
                                    Estribos_Izquierda(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo, Lista_ganchos, Vecino_Abajo, Vecino_Arriba, 0, Num_Estribos, Lista_sep_Ganchos, Diametro_gancho)
                                End If

                                ''Agregar texto
                                Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                                Pc = {Delta_X, Punto_inicial(1) - 0.2, 0}
                                Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 1.2, AcAttachmentPoint.acAttachmentPointTopLeft)
                                Add_Rectangulo(Rectangulo, {Delta_X - 0.2, Punto_inicial(1) - 0.4, Delta_X + Distancia_Confinada + 0.25, Punto_inicial(1) - 0.4, Delta_X + Distancia_Confinada + 0.25, Punto_inicial(1) + 0.25, Delta_X - 0.2, Punto_inicial(1) + 0.25}, "FC_BORDES", True, Lista_Rect)

                                ''Agregar Hatch
                                Add_Hatch(Lista_Rect.Last, Pattern_name, Layer_Hatch, 0.9)

                                ''Agregrar Cuadro piso y texto

                                Coordenadas_Texto = {Lista_Rect.Last.Coordinates(0) - 0.75, Lista_Rect.Last.Coordinates(1), Lista_Rect.Last.Coordinates(0), Lista_Rect.Last.Coordinates(1), Lista_Rect.Last.Coordinates(0), Lista_Rect.Last.Coordinates(7), Lista_Rect.First.Coordinates(0) - 0.75, Lista_Rect.Last.Coordinates(7)}
                                Add_Rectangulo(Rectangulo, Coordenadas_Texto, "FC_BORDES", True, Lista_Rect)

                                Coordenadas_Texto = {Lista_Rect.Last.Coordinates(0) + (Lista_Rect.Last.Coordinates(2) - Lista_Rect.Last.Coordinates(0)) / 2, Lista_Rect.Last.Coordinates(1) + (Lista_Rect.Last.Coordinates(7) - Lista_Rect.Last.Coordinates(1)) / 2, 0}
                                Texto_Estribos = Muro_i.Stories(j)
                                Add_Texto(Texto_Estribos, Coordenadas_Texto, "FC_R-80", "FC_TEXT1", 0, 0, AcAttachmentPoint.acAttachmentPointMiddleCenter)

                            End If

                            If Muro_i.Lebe_Der(j) > 0 Or Muro_i.Zc_Der(j) > 0 Then

                                Determinacion_Punto_Final_Vertical(Punto_final, Muro_i, ListaOrdenada(i), Vecino_Arriba, Delta_reduccion, i, Muro_Vecino_Arriba, j, DeltaY, Delta_X)
                                Distancia_Confinada = Determinacion_Confinamiento_Ld(Muro_i, Vecino_Arriba, Muro_Vecino_Arriba, j, Diametro_Estribo, Separacion_Estribo, Pattern_name, Layer_Hatch)

                                If Distancia_Confinada > Distancia_Maxima Then
                                    Num_Estribos = (Distancia_Confinada / Distancia_Maxima) + 1
                                    Distancia_Limite = (Distancia_Confinada / Num_Estribos) + 0.2
                                Else
                                    Num_Estribos = 1
                                    Distancia_Limite = Distancia_Confinada - 0.04
                                End If

                                Punto_inicial = {Punto_final(0) - Distancia_Confinada, Punto_final(1), 0}
                                Pos = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Count - 1
                                Estribos_Derecha(Suma_Long, delta, Punto_inicial, Punto_final, Muro_i, Pos, Distancia_Limite, i, j, 0, DeltaY, Diametro_Estribo, Lista_ganchos, Vecino_Arriba, Num_Estribos, Lista_sep_Ganchos, Diametro_gancho)

                                ''Agregar texto
                                Texto_Estribos = "Ganchos y estribos suplementarios #" & Diametro_Estribo & " a " & Format(Separacion_Estribo, "##,0.000")
                                Pc = {Punto_inicial(0), Punto_inicial(1) - 0.2, 0}
                                Add_Texto(Texto_Estribos, Pc, "FC_R-80", "FC_TEXT1", 0, 1.2, AcAttachmentPoint.acAttachmentPointTopLeft)
                                Add_Rectangulo(Rectangulo, {Punto_inicial(0) - 0.2, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) + 0.25, Punto_inicial(0) - 0.2, Punto_inicial(1) + 0.25}, "FC_BORDES", True, Lista_Rect)

                                ''Agregar Hatch
                                Add_Hatch(Lista_Rect.Last, Pattern_name, Layer_Hatch, 0.9)

                                If Muro_i.Lebe_Izq(j) = 0 And Muro_i.Zc_Izq(j) = 0 Then

                                    ''Agregrar Cuadro piso y texto
                                    Coordenadas_Texto = {Delta_X - 0.95, Lista_Rect.Last.Coordinates(1), Delta_X - 0.2, Lista_Rect.Last.Coordinates(1), Delta_X - 0.2, Lista_Rect.Last.Coordinates(7), Delta_X - 0.95, Lista_Rect.Last.Coordinates(7)}
                                    Add_Rectangulo(Rectangulo, Coordenadas_Texto, "FC_BORDES", True, Lista_Rect)

                                    Coordenadas_Texto = {Lista_Rect.Last.Coordinates(0) + (Lista_Rect.Last.Coordinates(2) - Lista_Rect.Last.Coordinates(0)) / 2, Lista_Rect.Last.Coordinates(1) + (Lista_Rect.Last.Coordinates(7) - Lista_Rect.Last.Coordinates(1)) / 2, 0}
                                    Texto_Estribos = Muro_i.Stories(j)
                                    Add_Texto(Texto_Estribos, Coordenadas_Texto, "FC_R-80", "FC_TEXT1", 0, 0, AcAttachmentPoint.acAttachmentPointMiddleCenter)
                                End If

                            End If

                        End If

                        Add_Rectangulo(Rectangulo, {Delta_X - 0.2, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) - 0.4, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) + 0.25, Delta_X - 0.2, Punto_inicial(1) + 0.25}, "FC_BORDES", True, Lista_Rect)
                        Add_Ganchos(Lista_ganchos, Lista_sep_Ganchos, ListaOrdenada(i).Lista_Refuerzos_Original, DeltaY, Delta_X + Longitud_Real + 0.25, Muro_i.Pier_name, Muro_i.Stories(j), Muro_i.Bw(i), Diametro_gancho, Muro_i.Hw(i))

                        Dim prueba = Lista_Cantidades_i.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = Muro_i.Pier_name).Lista_Ganchos.FindAll(Function(x) x.Pier = Muro_i.Pier_name And x.Story = Muro_i.Stories(j)).ToList()
                        Dim prueba2 = prueba.Select(Function(x) x.Separacion).Distinct().ToList

                        ''Dibujo de numero de ganchos totales en el piso por separacion
                        Ganchos_Totales_piso(prueba, prueba2, Delta_X + Longitud_Real + 0.25, DeltaY)

                        DeltaY += 0.65
                    Next

                End If

                Add_Rectangulo(Rectangulo, {Delta_X - 0.2, Punto_inicial(1) + 0.25, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) + 0.25, Delta_X + Longitud_Real + 0.25, Punto_inicial(1) + 0.45, Delta_X - 0.2, Punto_inicial(1) + 0.45}, "FC_BORDES", True, Lista_Rect)

                Texto_Estribos = "CUADRO DE ESTRIBOS MURO " & Muro_i.Pier_name
                Coordenadas_Texto = {Lista_Rect.First.Coordinates(0) + (Lista_Rect.Last.Coordinates(2) - Lista_Rect.Last.Coordinates(0)) / 2, Lista_Rect.Last.Coordinates(7) - 0.05, 0}
                Add_Texto(Texto_Estribos, Coordenadas_Texto, "FC_R-80", "FC_TEXT1", 0, 0, AcAttachmentPoint.acAttachmentPointTopCenter)

                Delta_X += Longitud_Real + 3
            End If

        Next
        AcadDoc.Regen(AcRegenType.acActiveViewport)
    End Sub

    Private Sub Ganchos_Totales_piso(prueba As List(Of Seccion_Ganchos), prueba2 As List(Of Double), ByVal Pos_X As Double, ByVal Pos_Y As Double)
        Dim Totales As Integer
        Dim Texto As String
        Dim delta_x As Double
        Dim Coord_Gancho As Double()
        Dim Pc As Double()
        Dim Pr As Double()
        Dim dynamic_property1 As Object
        Dim editar_property1 As AcadDynamicBlockReferenceProperty
        Dim Long_Gancho As Double

        delta_x = Pos_X
        Long_Gancho = 0.174
        If prueba.Count > 0 Then
            Pr = {Pos_X, Pos_Y - 0.4, Pos_X + 1.1, Pos_Y - 0.4, Pos_X + 1.1, Pos_Y + 0.25, Pos_X, Pos_Y + 0.25}
            Add_Rectangulo(Rectangulo, Pr, "FC_BORDES", True, Lista_Rect)
        End If

        For k = 0 To prueba2.Count - 1
            Totales = prueba.FindAll(Function(X) X.Separacion = prueba2(k)).Count
            Texto = Totales & " Ganchos #" & prueba.Find(Function(X) X.Separacion = prueba2(k)).Diametro & " a " & prueba2(k)

#Region "VariablesDibujo"

            If prueba2.Count > 1 Then
                Coord_Gancho = {delta_x + 0.25, Pos_Y - 0.087, 0}
            Else
                Coord_Gancho = {Pos_X + 0.55, Pos_Y - 0.087, 0}
            End If

            Pc = {Coord_Gancho(0), Coord_Gancho(1) - 0.2, 0}
            Dibujar_Gancho(Coord_Gancho, Long_Gancho, "FC_B_Gancho Tipo 5", dynamic_property1, editar_property1, 0)
            Add_Texto(Texto, Pc, "FC_R-80", "FC_TEXT1", 0, 0.45, AcAttachmentPoint.acAttachmentPointMiddleCenter)
            delta_x += 0.5

#End Region

        Next
    End Sub

    Private Shared Function Determinacion_Confinamiento_Ld(Muro_i As Muros_Consolidados, Vecino_dir As Boolean, Muro_Vecino_dir As Muros_Consolidados, j As Integer, ByRef Diametro_Estribo As Integer, ByRef Sep As Single, ByRef Pattern As String, ByRef Layer As String) As Double
        Dim Distancia_Confinada As Double

        If Muro_i.Lebe_Der(j) > 0 Then
            If Vecino_dir = True Then
                Distancia_Confinada = (Muro_Vecino_dir.Bw(j) + Muro_i.Lebe_Der(j)) / 100
            Else
                Distancia_Confinada = Muro_i.Lebe_Der(j) / 100
            End If
            Diametro_Estribo = Muro_i.Est_ebe(j)
            Sep = Muro_i.Sep_ebe(j) / 100
            Pattern = "SOLID"
            Layer = "FC_HATCH MUROS"
        Else
            If Vecino_dir = True Then
                Distancia_Confinada = (Muro_Vecino_dir.Bw(j) + Muro_i.Zc_Der(j)) / 100
            Else
                Distancia_Confinada = Muro_i.Zc_Der(j) / 100
            End If
            Diametro_Estribo = Muro_i.Est_Zc(j)
            Sep = Muro_i.Sep_Zc(j) / 100
            Pattern = "DOTS"
            Layer = "FC_HATCH 252"
        End If

        Return Distancia_Confinada
    End Function

    Private Shared Function Determinacion_Confinamiento_LI(Muro_i As Muros_Consolidados, Vecino_izq As Boolean, Muro_Vecino_izq As Muros_Consolidados, j As Integer, ByRef Diametro_Estribo As Integer, ByRef Sep As Single, ByRef Pattern As String, ByRef Layer As String) As Double

        Dim Distancia_Confinada As Double
        Dim Bw_izq As Double = 0

        If Vecino_izq = True Then
            Bw_izq = Muro_Vecino_izq.Bw(j)
        End If

        If Muro_i.Lebe_Izq(j) > 0 Then

            Distancia_Confinada = (Bw_izq + Muro_i.Lebe_Izq(j)) / 100

            Diametro_Estribo = Muro_i.Est_ebe(j)
            Sep = Muro_i.Sep_ebe(j) / 100

            Pattern = "SOLID"
            Layer = "FC_HATCH MUROS"
        Else

            Distancia_Confinada = (Bw_izq + Muro_i.Zc_Izq(j)) / 100

            Diametro_Estribo = Muro_i.Est_Zc(j)
            Sep = Muro_i.Sep_Zc(j) / 100

            Pattern = "DOTS"
            Layer = "FC_HATCH 252"
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
            If Muro_vecino_izquierda.Reduccion = Reduccion.Derecha Or Muro_vecino_izquierda.Reduccion = Reduccion.NoAplica Then
                Delta_reduccion = 0
            End If

            If Muro_vecino_izquierda.Reduccion = Reduccion.Izquierda Then

                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino_izquierda.Bw(j) - Muro_vecino_izquierda.Bw(j + 1)) / 100
                End If
            End If

            If Muro_vecino_izquierda.Reduccion = Reduccion.Centro Then
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

            If Muro_vecino_Abajo.Reduccion = Reduccion.Abajo Or Muro_vecino_Abajo.Reduccion = Reduccion.NoAplica Then
                Delta_reduccion = 0
            End If

            If Muro_vecino_Abajo.Reduccion = Reduccion.Arriba Then

                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino_Abajo.Bw(j) - Muro_vecino_Abajo.Bw(j + 1)) / 100
                End If

            End If

            If Muro_vecino_Abajo.Reduccion = Reduccion.Centro Then
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

            If Muro_vecino.Reduccion = Reduccion.Izquierda Or Muro_vecino.Reduccion = Reduccion.NoAplica Then
                Delta_reduccion = 0
            End If

            If Muro_vecino.Reduccion = Reduccion.Derecha Then

                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino.Bw(j) - Muro_vecino.Bw(j + 1)) / 100
                End If

            End If

            If Muro_vecino.Reduccion = Reduccion.Centro Then
                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino.Bw(j) - Muro_vecino.Bw(j + 1)) / 200
                End If
            End If

            Punto_final = {Delta_X + Longitud_Muro + Muro_vecino.Bw(j) / 100 - Delta_reduccion - 0.038, DeltaY, 0}
        Else
            Punto_final = {Delta_X + Longitud_Muro - Delta_reduccion - 0.038, DeltaY, 0}
        End If

    End Sub

    Private Shared Sub Determinacion_Punto_Final_Vertical(ByRef Punto_final() As Double, Muro_i As Muros_Consolidados, ByVal Muro_D As Muros, Vecino_arriba As Boolean, ByRef Delta_reduccion As Double, i As Integer, Muro_vecino As Muros_Consolidados, j As Integer, ByVal DeltaY As Double, ByVal Delta_X As Double)

        Dim Longitud_Muro As Double

        Longitud_Muro = Muro_D.YmaxE - Muro_D.YminE

        If Vecino_arriba = True Then

            If Muro_vecino.Reduccion = Reduccion.Arriba Or Muro_vecino.Reduccion = Reduccion.NoAplica Then
                Delta_reduccion = 0
            End If

            If Muro_vecino.Reduccion = Reduccion.Abajo Then

                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino.Bw(j) - Muro_vecino.Bw(j + 1)) / 100
                End If

            End If

            If Muro_vecino.Reduccion = Reduccion.Centro Then
                If j < Muro_i.Stories.Count - 1 Then
                    Delta_reduccion = (Muro_vecino.Bw(j) - Muro_vecino.Bw(j + 1)) / 200
                End If
            End If
            Punto_final = {Delta_X + Longitud_Muro + Muro_vecino.Bw(j) / 100 - Delta_reduccion - 0.038, DeltaY, 0}
        Else
            Punto_final = {Delta_X + Longitud_Muro - Delta_reduccion - 0.038, DeltaY, 0}
        End If

    End Sub

    Private Sub Estribos_Izquierda(ByRef Suma_Long As Single, ByRef delta As Single, ByRef Punto_inicial() As Double, ByVal Punto_final() As Double, Muro_i As Muros_Consolidados, ByRef Pos As Integer, Distancia_Limite As Double, i As Integer, k As Integer, ByVal Direccion As Integer, DeltaY As Double, ByVal Diametro As Integer, ByRef Lista_Ganchos As List(Of Boolean), ByVal Vecino_Izq As Boolean, ByVal Vecino_Der As Boolean, ByVal Bw_Der As Double, ByVal Num_Estribos As Integer, ByRef lista_sep As List(Of Double), ByRef Diametros_gancho As List(Of Integer))

        Dim Condicion, Condicion2 As Boolean
        delta = 0
        Condicion = True
        Condicion2 = False
        Suma_Long = 0

        If Separacion_Estribo = 0 Then
            MsgBox("Estribos en zonas de confinamiento sin separacion definida", MsgBoxStyle.Critical, "efe Prima ce")
            Exit Sub
        End If

        For j = Pos To ListaOrdenada(i).Lista_Refuerzos_Original.Count - 2

            If Math.Round(ListaOrdenada(i).Lista_Refuerzos_Original(j + 1)(Direccion), 2) <= Math.Round(Punto_final(0), 2) Then

                If j = 0 And Vecino_Izq = True Then
                    delta = ListaOrdenada(i).Lista_Refuerzos_Original(j + 1)(Direccion) - Punto_inicial(0)
                    Condicion2 = True
                Else
                    delta = ListaOrdenada(i).Lista_Refuerzos_Original(j + 1)(Direccion) - ListaOrdenada(i).Lista_Refuerzos_Original(j)(Direccion)
                End If

                Suma_Long += delta

                'Determinar si la barra lleva gancho o no

                If j <> Pos Or Pos = 0 Then
                    Lista_Ganchos(j + 1) = True
                    lista_sep(j + 1) = Separacion_Estribo
                    Diametros_gancho(j + 1) = Diametro
                End If

                If Suma_Long + (0.038 * 2) >= Distancia_Limite Then

                    If j + 1 = ListaOrdenada(i).Lista_Refuerzos_Original.Count - 1 Then
                        Condicion = False
                        If Vecino_Der = True Then
                            Suma_Long += Bw_Der
                        End If
                    End If

                    Add_Estribos("FC_ESTRIBOS", 0, Punto_inicial, Suma_Long, Muro_i.Bw(k) / 100, Diametro, False, Muro_i.Stories(k), Muro_i.Hw(k), Muro_i.Pier_name)
                    Lista_Ganchos(j + 1) = False
                    lista_sep(j + 1) = 0
                    Diametros_gancho(j + 1) = 0

                    If Condicion2 = False Then
                        Lista_Ganchos(Pos) = False
                        lista_sep(Pos) = 0
                        Diametros_gancho(Pos) = 0
                    Else
                        Lista_Ganchos(Pos) = True
                        lista_sep(Pos) = Separacion_Estribo
                        Diametros_gancho(Pos) = Diametro
                        Condicion2 = False
                    End If

                    Pos = j
                    j = Pos - 1

                    Punto_inicial = {ListaOrdenada(i).Lista_Refuerzos_Original(Pos)(Direccion), DeltaY, 0}

                    If Condicion = False Then
                        Lista_Ganchos(Pos) = True
                        lista_sep(Pos) = Separacion_Estribo
                        Diametros_gancho(Pos) = Diametro
                    Else
                        Lista_Ganchos(Pos) = False
                        lista_sep(Pos) = 0
                        Diametros_gancho(Pos) = 0
                    End If

                    If Pos + 1 = ListaOrdenada(i).Lista_Refuerzos_Original.Count - 1 And Vecino_Der = True Then
                        Lista_Ganchos(Pos + 1) = True
                        lista_sep(Pos + 1) = Separacion_Estribo
                        Diametros_gancho(Pos + 1) = Diametro
                    Else
                        Lista_Ganchos(Pos + 1) = False
                        lista_sep(Pos + 1) = 0
                        Diametros_gancho(Pos + 1) = 0
                    End If

                    Suma_Long = 0

                    If Num_Estribos = 1 Then
                        If Lista_Ganchos(Pos) = False Then
                            Lista_Ganchos(Pos) = True
                            lista_sep(Pos) = Separacion_Estribo
                            Diametros_gancho(Pos) = Diametro
                        End If
                        Exit For
                    End If

                End If

                If j + 1 = ListaOrdenada(i).Lista_Refuerzos_Original.Count - 1 And Suma_Long + (0.038 * 2) < Distancia_Limite And Condicion = True Then

                    Lista_Ganchos(j + 1) = False
                    lista_sep(j + 1) = 0
                    Diametros_gancho(j + 1) = 0
                    Lista_Ganchos(Pos) = False
                    lista_sep(Pos) = 0
                    Diametros_gancho(Pos) = 0
                    Add_Estribos("FC_ESTRIBOS", 0, Punto_inicial, Suma_Long, Muro_i.Bw(k) / 100, Diametro, False, Muro_i.Stories(k), Muro_i.Hw(k), Muro_i.Pier_name)
                    Suma_Long = 0
                    Exit For
                End If
            Else
                Lista_Ganchos(j - 1) = True
                lista_sep(j - 1) = Separacion_Estribo
                Diametros_gancho(j - 1) = Diametro
                Exit For
            End If

        Next

    End Sub

    Private Sub Estribos_Derecha(ByRef Suma_Long As Single, ByRef delta As Single, ByRef Punto_inicial() As Double, ByRef Punto_final() As Double, Muro_i As Muros_Consolidados, ByRef Pos As Integer, Distancia_Limite As Double, i As Integer, k As Integer, ByVal Direccion As Integer, ByVal DeltaY As Double, ByVal Diametro As Integer, ByRef Lista_Ganchos As List(Of Boolean), ByVal Vecino_Der As Boolean, ByVal Num_Estribos As Integer, ByRef lista_sep_ganchos As List(Of Double), Diametros_gancho As List(Of Integer))

        Dim condicion, condicion2 As Boolean
        condicion = True
        condicion2 = False
        Suma_Long = 0
        delta = 0

        If Separacion_Estribo = 0 Then
            MsgBox("Estribos en zonas de confinamiento sin separacion definida", MsgBoxStyle.Critical, "efe Prima ce")
            Exit Sub
        End If

        For j = Pos To 1 Step -1

            If Math.Round(ListaOrdenada(i).Lista_Refuerzos_Original(j - 1)(Direccion), 2) >= Math.Round(Punto_inicial(Direccion), 2) Then

                If j = Pos And Vecino_Der = True Then
                    delta = Math.Abs(Punto_final(0) - ListaOrdenada(i).Lista_Refuerzos_Original(j - 1)(Direccion))
                    condicion2 = True
                Else
                    delta = Math.Abs(ListaOrdenada(i).Lista_Refuerzos_Original(j - 1)(Direccion) - ListaOrdenada(i).Lista_Refuerzos_Original(j)(Direccion))

                End If
                Suma_Long += delta

                'Determinar si la barra lleva gancho o no

                If j <> Pos Or Pos = 0 Then
                    Lista_Ganchos(j + 1) = True
                    lista_sep_ganchos(j + 1) = Separacion_Estribo
                    Diametros_gancho(j + 1) = Diametro
                End If

                If Suma_Long + (0.038 * 2) >= Distancia_Limite Then

                    Add_Estribos("FC_ESTRIBOS", 0, Punto_final, Suma_Long, Muro_i.Bw(k) / 100, Diametro, True, Muro_i.Stories(k), Muro_i.Hw(k), Muro_i.Pier_name)

                    Lista_Ganchos(j - 1) = False
                    If condicion2 = False Then
                        Lista_Ganchos(Pos) = False
                        lista_sep_ganchos(Pos) = 0
                        Diametros_gancho(Pos) = 0
                    Else
                        Lista_Ganchos(Pos) = True
                        lista_sep_ganchos(Pos) = Separacion_Estribo
                        Diametros_gancho(Pos) = Diametro
                        condicion2 = False
                    End If

                    If j - 1 = ListaOrdenada(i).Lista_Refuerzos_Original.Count - 1 Then
                        condicion = False
                    End If

                    Pos = j
                    j = Pos + 1

                    Punto_final = {ListaOrdenada(i).Lista_Refuerzos_Original(Pos)(Direccion), DeltaY, 0}
                    If condicion = False Then
                        Lista_Ganchos(Pos) = True
                        lista_sep_ganchos(Pos) = Separacion_Estribo
                        Diametros_gancho(Pos) = Diametro
                    Else
                        Lista_Ganchos(Pos) = False
                        lista_sep_ganchos(Pos) = 0
                        Diametros_gancho(Pos) = 0
                    End If
                    Lista_Ganchos(Pos - 1) = False
                    lista_sep_ganchos(Pos - 1) = 0
                    Diametros_gancho(Pos - 1) = 0
                    Suma_Long = 0

                    If Num_Estribos = 1 Then
                        If Lista_Ganchos(Pos) = False Then
                            Lista_Ganchos(Pos) = True
                            lista_sep_ganchos(Pos) = Separacion_Estribo
                            Diametros_gancho(Pos) = Diametro
                        End If
                        Exit For
                    End If

                End If

                If j - 1 = ListaOrdenada(i).Lista_Refuerzos_Original.Count - 1 And Suma_Long + (0.02 * 2) < Distancia_Limite And condicion = True Then
                    Lista_Ganchos(j - 2) = True
                    lista_sep_ganchos(j - 2) = Separacion_Estribo
                    Diametros_gancho(j - 2) = Diametro
                    Exit For
                End If
            Else
                Lista_Ganchos(j + 1) = True
                lista_sep_ganchos(j + 1) = Separacion_Estribo
                Diametros_gancho(j + 1) = Diametro
                Exit For
            End If

        Next
    End Sub

    Private Sub Add_Estribos(ByVal Layer As String, ByVal Angulo As Double, ByVal Ip As Double(), ByVal Distancia As Double, ByVal Espesor_Doble As Double, ByVal Diametro As Integer, ByVal Mover As Boolean, ByVal Story As String, ByVal H_piso As Single, ByVal Pier_name As String)

        Dim Nombre_Bloque As String
        Dim dynamic_property1 As Object
        Dim editar_property1 As AcadDynamicBlockReferenceProperty
        Dim Point_aux As Double()
        Dim Estribo_i As Seccion_Estribos
        Dim Hlibre As Double
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

        Hlibre = (H_piso / 100) - 0.1

        If Separacion_Estribo > 0 Then
            Estribo_i = New Seccion_Estribos With
            {
                .Diametro = Diametro,
                .Separacion = Math.Round(Separacion_Estribo, 3),
                .Longitdud = Math.Round((Espesor_Doble - 0.04) * 2 + (Distancia + 0.038) * 2 + (2 * Find_Long_Ganchos(Diametro)), 2),
                .Story = Story,
                .Pier = Pier_name,
                .Cantidad = Math.Round((Hlibre / Separacion_Estribo) + 1, 0)
            }
            Lista_Cantidades_i.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = Pier_name).Lista_Estribos.Add(Estribo_i)
        Else
            MsgBox("Estribos en zonas de confinamiento sin separacion definida", MsgBoxStyle.Critical, "efe Prima ce")
            Exit Sub
        End If

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

    Private Shared Sub Add_Texto(ByVal Texto_1 As String, ByVal P_texto As Double(), ByVal Layer As String, ByVal Style As String, ByVal Angulo As Double, ByVal Ancho As Single, ByVal Justificacion As AcAttachmentPoint)

        Dim texto_Estribo As AcadMText
        texto_Estribo = AcadDoc.ModelSpace.AddMText(P_texto, 0.75, Texto_1)

        With texto_Estribo
            .Layer = Layer
            .StyleName = Style
            .Height = 0.05
            .Rotation = Angulo
            .Width = Ancho
            .AttachmentPoint = Justificacion
            .InsertionPoint = P_texto
            .Update()
        End With

    End Sub

    Private Shared Sub Add_Rectangulo(ByVal Rectangulo As AcadLWPolyline, ByVal Coord() As Double, ByVal Layer As String, ByVal IsClosed As Boolean, ByRef Lista As List(Of AcadLWPolyline))

        Rectangulo = AcadDoc.ModelSpace.AddLightWeightPolyline(Coord)

        With Rectangulo
            .Layer = Layer
            .Closed = IsClosed
            .Update()
        End With

        Lista.Add(Rectangulo)

    End Sub

    Private Shared Sub Add_Hatch(ByVal Acad_Ent As AcadEntity, ByVal Pattern As String, ByVal Layer As String, ByVal Escala As Double)

        Dim Hatch As AcadHatch
        Dim outerLoop(0 To 0) As AcadEntity

        Hatch = AcadDoc.ModelSpace.AddHatch(0, Pattern, True)
        outerLoop(0) = Acad_Ent

        With Hatch
            .AppendOuterLoop(outerLoop)
            .Layer = Layer
            .LinetypeScale = Escala
            .PatternAngle = 45
            .PatternScale = 0.009
            .PatternSpace = 0.009
            .Update()
        End With

        Hatch_Back(Hatch)

    End Sub

    Private Shared Sub Hatch_Back(ByVal Acad_object As AcadObject)

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

    Private Sub Add_Ganchos(ByVal Lista_Ganchos As List(Of Boolean), ByVal Lista_sep_ganchos As List(Of Double), ByVal Lista_Coord As List(Of Double()), ByVal Delta_Y As Double, ByVal Delta_X As Double, ByVal Pier_name As String, ByVal Story As String, ByVal bw As Double, ByVal diametro As List(Of Integer), ByVal Hw As Double)

        Dim Coord_Gancho As Double()
        Dim Long_Gancho As Double
        Dim Nombre_Bloque As String
        Dim dynamic_property1 As Object
        Dim editar_property1 As AcadDynamicBlockReferenceProperty
        Dim Contador As Integer = 0
        Dim Texto As String = ""
        Dim pos_ini As Short = 0
        Dim gancho_i As Seccion_Ganchos
        Dim Hlibre As Double
        Nombre_Bloque = "FC_B_Gancho Tipo 5"

        Hlibre = (Hw / 100) - 0.1

        For i = 0 To Lista_Ganchos.Count - 1

            If Lista_Ganchos(i) = True Then

                Coord_Gancho = {Lista_Coord(i)(0), Delta_Y - 0.087, 0}
                Long_Gancho = 0.174
                Dibujar_Gancho(Coord_Gancho, Long_Gancho, Nombre_Bloque, dynamic_property1, editar_property1, pos_ini)

                If Lista_sep_ganchos(i) = 0 Then
                    MsgBox("Estribos en zonas de confinamiento sin separacion definida", MsgBoxStyle.Critical, "efe Prima ce")
                    Exit Sub
                End If

                gancho_i = New Seccion_Ganchos With
                {
                    .Longitud = (bw / 100) - 0.04 + 2 * ganchos_180(diametro(i)),
                    .Diametro = diametro(i),
                    .Separacion = Math.Round(Lista_sep_ganchos(i), 3),
                    .Pier = Pier_name,
                    .Story = Story,
                    .Cantidad = Math.Round((Hlibre / Lista_sep_ganchos(i)) + 1, 0)
                }
                Lista_Cantidades_i.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = Pier_name).Lista_Ganchos.Add(gancho_i)

                If pos_ini = 0 Then
                    pos_ini = 1
                Else
                    pos_ini = 0
                End If

                Contador += 1
            End If

        Next

    End Sub

    Private Sub Dibujar_Gancho(Coord_Gancho() As Double, Long_Gancho As Double, Nombre_Bloque As String, ByRef dynamic_property1 As Object, ByRef editar_property1 As AcadDynamicBlockReferenceProperty, ByVal Flip_State As Short)

        Bloque_Gancho = AcadDoc.ModelSpace.InsertBlock(Coord_Gancho, Nombre_Bloque, 1, 1, 1, Math.PI / 2)
        dynamic_property1 = Bloque_Gancho.GetDynamicBlockProperties

        editar_property1 = dynamic_property1(0)
        editar_property1.Value = Long_Gancho

        editar_property1 = dynamic_property1(2)
        editar_property1.Value = Flip_State

        Bloque_Gancho.Layer = "FC_GANCHOS"
        Bloque_Gancho.Update()
    End Sub

End Class