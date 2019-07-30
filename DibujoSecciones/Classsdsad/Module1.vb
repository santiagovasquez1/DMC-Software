Imports Autodesk.AutoCAD.Interop
Imports Autodesk.AutoCAD.Interop.Common


Module Module1

    Public AcadApp As Autodesk.AutoCAD.Interop.AcadApplication
    Public AcadDoc As Autodesk.AutoCAD.Interop.AcadDocument
    Public Selecccionar As AcadSelectionSet
    Public Polyline As AcadLWPolyline
    Public Muros_V As New List(Of Muros)
    Public CircunferenciasBloques As New List(Of CircunferenciaBloque)
    Public MurosAranas As New List(Of MuroArana)

    Sub IniciarAplicacion()

        Dim rnd As New Random

        Try
            AcadApp = GetObject(, "Autocad.Application")
        Catch ex As Exception
            Dim ruta As New Windows.Forms.OpenFileDialog()
            AcadApp = New AcadApplication
            AcadApp.Visible = True
            With ruta
                .Multiselect = True
                .Filter = "Elegir Planta Estructural|*.dwg"
                .Title = "Archivo dwg"
                .ShowDialog()
            End With
            AcadDoc = AcadApp.Documents.Add(ruta.FileName)
        End Try

        Dim i As Double = rnd.Next(0, 100000)

        AcadDoc = AcadApp.ActiveDocument
        ' AcadDoc.Save()
        'AcadDoc.PurgeAll()
        Selecccionar = AcadDoc.SelectionSets.Add(i.ToString)
        MsgBox("Seleccionar Conjunto de Muros", vbInformation, "eFe prima Ce")
        Selecccionar.SelectOnScreen()   'Todos los objectos seleccionaes estan en la variable Seleccionar
        Dim num As Integer = 1
        For Each Objeto In Selecccionar
            Dim Muro As New Muros
            Dim Circu As New CircunferenciaBloque

            Dim Clasificar As String = ClasificarObjectos(Objeto, num)
            If Clasificar <> "Sin Definir" And Clasificar = "M" & num And Objeto.ObjectName = "AcDbPolyline" Then
                Muro.NombreMuro = Clasificar
                Muro.CoordenadasX = Coordenadas(Objeto)(0)
                Muro.CoordenadasY = Coordenadas(Objeto)(1)
                Muros_V.Add(Muro)
                num = num + 1
            End If

            If Clasificar <> "M" & num And Objeto.ObjectName = "AcDbBlockReference" Then
                Circu.Nombre = Clasificar
                Circu.CoordenadasXyY = (PropiedadesdelBloque(Objeto)(1))
                Circu.Radio = PropiedadesdelBloque(Objeto)(0)(0)
                CircunferenciasBloques.Add(Circu)
            End If

        Next


        'GUARDAR DATOS
        For i = 0 To Muros_V.Count - 1
            Dim X_min, X_max, Y_min, Y_max As Double
            X_min = Math.Round(Muros_V(i).CoordenadasX.Min, 2)
            Y_min = Math.Round(Muros_V(i).CoordenadasY.Min, 2)
            X_max = Math.Round(Muros_V(i).CoordenadasX.Max, 2)
            Y_max = Math.Round(Muros_V(i).CoordenadasY.Max, 2)


            Muros_V(i).EspesorReal = EspesorMuro(X_max, X_min, Y_max, Y_min)(2)
            Muros_V(i).DireccionMuro = EspesorMuro(X_max, X_min, Y_max, Y_min)(3)
            Muros_V(i).Longitud = EspesorMuro(X_max, X_min, Y_max, Y_min)(4)

            If Muros_V(i).EspesorReal > 0.2 Then
                Muros_V(i).EspesorEscalado = 0.4
            Else

                Muros_V(i).EspesorEscalado = Muros_V(i).EspesorReal * 2
            End If

            Dim CentroideX As Double = (X_max - X_min) / 2 : Dim CentroideY As Double = (Y_max - Y_min) / 2

            'Guardar Valores Maximo y Minimos de X y Y
            Muros_V(i).Xmax = X_max
            Muros_V(i).Xmin = X_min
            Muros_V(i).Ymax = Y_max
            Muros_V(i).Ymin = Y_min

            Muros_V(i).XmaxE = Muros_V(i).CoordenadasX.Max
            Muros_V(i).XminE = Muros_V(i).CoordenadasX.Min
            Muros_V(i).YmaxE = Muros_V(i).CoordenadasY.Max
            Muros_V(i).YminE = Muros_V(i).CoordenadasY.Min


            'Centroide de Cada Muro
            Muros_V(i).CentroideX = CentroideX + X_min
            Muros_V(i).CentroideY = CentroideY + Y_min


            'Espesor

        Next


        Dim Distancia_ As Double

        For i = 0 To Muros_V.Count - 1
            Dim Dmin As Double = 99999999
            For j = 0 To CircunferenciasBloques.Count - 1

                Distancia_ = Math.Sqrt((Muros_V(i).CentroideX - CircunferenciasBloques(j).CoordenadasXyY(0)) ^ 2 + (Muros_V(i).CentroideY - CircunferenciasBloques(j).CoordenadasXyY(1)) ^ 2)

                If Dmin > Distancia_ Then
                    Dmin = Distancia_
                    Muros_V(i).NombreMuro = CircunferenciasBloques(j).Nombre
                End If
            Next

        Next



        'Muros Vecinos

        For i = 0 To Muros_V.Count - 1
            Dim X_min, X_max, Y_min, Y_max As Double
            X_min = Muros_V(i).Xmin
            Y_min = Muros_V(i).Ymin
            X_max = Muros_V(i).Xmax
            Y_max = Muros_V(i).Ymax


            For j = 0 To Muros_V.Count - 1

                If i <> j Then
                    Dim X_min1, X_max1, Y_min1, Y_max1 As Double
                    X_min1 = Muros_V(j).Xmin
                    Y_min1 = Muros_V(j).Ymin
                    X_max1 = Muros_V(j).Xmax
                    Y_max1 = Muros_V(j).Ymax

                    If X_min1 = X_min Or X_min1 = X_max Or X_max1 = X_min Or X_max1 = X_max Then
                        If Y_max1 >= Y_max And Y_max >= Y_min1 OrElse Y_max >= Y_max1 And Y_max1 >= Y_min Then
                            Muros_V(i).MurosVecinos.Add(Muros_V(j).NombreMuro)
                        End If

                    ElseIf Y_min1 = Y_min Or Y_min1 = Y_max Or Y_max1 = Y_min Or Y_max1 = Y_max Then
                        If X_max1 >= X_max And X_max >= X_min1 OrElse X_max >= X_max1 And X_max1 >= X_min Then
                            Muros_V(i).MurosVecinos.Add(Muros_V(j).NombreMuro)
                        End If

                    End If

                End If

            Next

        Next


        Dim ValorMenorX As Double = 999999
        ' Dim ValorMayorY, ValorMenorY As Double


        For i = 0 To Muros_V.Count - 1
            For k = 0 To Muros_V(i).MurosVecinos.Count - 1

                For j = 0 To Muros_V.Count - 1
                    If i <> j Then
                        If Muros_V(i).MurosVecinos(k) = Muros_V(j).NombreMuro Then
                            Muros_V(i).MurosVecinosP.Add(j)
                        End If
                    End If
                Next

            Next
        Next

        Dim ListaOrdenada, ListaOrdenadaY As List(Of Muros)

        ListaOrdenada = Muros_V.OrderBy((Function(x) x.Xmin)).ToList()
        ListaOrdenadaY = Muros_V.OrderBy((Function(x) x.Xmin)).ToList()


        For i = 0 To ListaOrdenada.Count - 1
            ListaOrdenada(i).MurosVecinosP.Clear()
        Next


        For i = 0 To ListaOrdenada.Count - 1
            For k = 0 To ListaOrdenada(i).MurosVecinos.Count - 1

                For j = 0 To ListaOrdenada.Count - 1
                    If i <> j Then
                        If ListaOrdenada(i).MurosVecinos(k) = ListaOrdenada(j).NombreMuro Then
                            ListaOrdenada(i).MurosVecinosP.Add(j)
                        End If
                    End If
                Next

            Next
        Next

        For i = 0 To ListaOrdenadaY.Count - 1
            For k = 0 To ListaOrdenadaY(i).MurosVecinos.Count - 1

                For j = 0 To ListaOrdenada.Count - 1
                    If i <> j Then
                        If ListaOrdenada(i).MurosVecinos(k) = ListaOrdenadaY(j).NombreMuro Then
                            ListaOrdenada(i).MurosVecinosPY.Add(j)
                        End If
                    End If
                Next

            Next
        Next




        For i = 0 To ListaOrdenada.Count - 1
            ListaOrdenada(i).MurosVecinosP = ListaOrdenada(i).MurosVecinosP.OrderBy(Function(x) x).ToList

        Next





        For i = 0 To ListaOrdenada.Count - 1
            For j = 0 To ListaOrdenada(i).MurosVecinos.Count - 1
                ListaOrdenada(i).MurosVeciosYmin.Add(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymin)

            Next
        Next



        Dim PosicionY As Double

        Dim AuxY As Integer

        For i = 0 To ListaOrdenada.Count - 1

            For j = 0 To ListaOrdenada(i).MurosVeciosYmin.Count - 1
                For m = 0 To (ListaOrdenada(i).MurosVeciosYmin.Count - 1) - j - 1

                    If ListaOrdenada(i).MurosVeciosYmin(m) > ListaOrdenada(i).MurosVeciosYmin(m + 1) Then
                        PosicionY = ListaOrdenada(i).MurosVeciosYmin(m)
                        AuxY = ListaOrdenada(i).MurosVecinosPY(m)

                        ListaOrdenada(i).MurosVeciosYmin(m) = ListaOrdenada(i).MurosVeciosYmin(m + 1)
                        ListaOrdenada(i).MurosVecinosPY(m) = ListaOrdenada(i).MurosVecinosPY(m + 1)

                        ListaOrdenada(i).MurosVeciosYmin(m + 1) = PosicionY
                        ListaOrdenada(i).MurosVecinosPY(m + 1) = AuxY


                    End If
                Next
            Next

        Next


        Dim Long_aux1, Long_aux2


        For i = 0 To ListaOrdenada.Count - 1
            Dim Delta As Double = 0
            If ListaOrdenada(i).DireccionMuro = "Vertical" Then

                ''Escalado del objeto
                ListaOrdenada(i).Xmax = ListaOrdenada(i).Xmax + (ListaOrdenada(i).EspesorEscalado - ListaOrdenada(i).EspesorReal) ''Expansion del espesor

                For j = 0 To ListaOrdenada(i).MurosVecinos.Count - 1

                    'If ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).NombreMuro = "I" And ListaOrdenada(i).NombreMuro = "11" Then
                    '    Stop
                    'End If

                    If Math.Round(ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).XminE, 2) = Math.Round(ListaOrdenada(i).XmaxE, 2) Then  ''Evaluacion en los primer extremo
                        Long_aux1 = Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).XminE - ListaOrdenada(i).XminE)
                        Long_aux2 = Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).XmaxE - ListaOrdenada(i).XmaxE)
                        Delta = Long_aux2 - Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).Xmax - ListaOrdenada(i).Xmax)
                        ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).Xmin = ListaOrdenada(i).Xmax
                        ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).Xmax = ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).Xmax + Delta

                    ElseIf Math.Round(ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).XmaxE, 2) = Math.Round(ListaOrdenada(i).XminE, 2) Then 'Evaluación en el segundo extremo


                        ListaOrdenada(i).Xmin = ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).Xmax
                        ListaOrdenada(i).Xmax = ListaOrdenada(i).Xmin + ListaOrdenada(i).EspesorEscalado


                    Else
                        Long_aux1 = Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).XminE - ListaOrdenada(i).XminE)
                        Long_aux2 = Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).XmaxE - ListaOrdenada(i).XmaxE)
                        If Math.Round(ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).XminE, 2) <= Math.Round(ListaOrdenada(i).XminE, 2) Then
                            ListaOrdenada(i).Xmin = ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).Xmin + Long_aux1
                            ListaOrdenada(i).Xmax = ListaOrdenada(i).Xmin + ListaOrdenada(i).EspesorEscalado
                            Delta = Long_aux2 - Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).Xmax - ListaOrdenada(i).Xmax)
                            ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).Xmax = ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).Xmax + Delta

                        End If

                    End If
                Next


            Else
                ListaOrdenada(i).Ymax = ListaOrdenada(i).Ymax + (ListaOrdenada(i).EspesorEscalado - ListaOrdenada(i).EspesorReal) ''Expansion del espesor

                For j = 0 To ListaOrdenada(i).MurosVecinos.Count - 1

                    If Math.Round(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).YminE, 2) = Math.Round(ListaOrdenada(i).YmaxE, 2) Then  ''Evaluacion en los primer extremo
                        Long_aux1 = Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).YminE - ListaOrdenada(i).YminE)
                        Long_aux2 = Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).YmaxE - ListaOrdenada(i).YmaxE)
                        Delta = Long_aux2 - Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymax - ListaOrdenada(i).Ymax)
                        ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymin = ListaOrdenada(i).Ymax
                        ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymax = ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymax + Delta

                    ElseIf Math.Round(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).YmaxE, 2) = Math.Round(ListaOrdenada(i).YminE, 2) Then 'Evaluación en el segundo extremo


                        ListaOrdenada(i).Ymin = ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymax
                        ListaOrdenada(i).Ymax = ListaOrdenada(i).Ymin + ListaOrdenada(i).EspesorEscalado


                    Else
                        Long_aux1 = Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).YminE - ListaOrdenada(i).YminE)
                        Long_aux2 = Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).YmaxE - ListaOrdenada(i).YmaxE)
                        If Math.Round(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).YminE, 2) <= Math.Round(ListaOrdenada(i).YminE, 2) Then
                            ListaOrdenada(i).Ymin = ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymin + Long_aux1
                            ListaOrdenada(i).Ymax = ListaOrdenada(i).Ymin + ListaOrdenada(i).EspesorEscalado
                            Delta = Long_aux2 - Math.Abs(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymax - ListaOrdenada(i).Ymax)
                            ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymax = ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymax + Delta

                        End If

                    End If
                Next


            End If
        Next



        'Dim A = AcadDoc.Utility.GetPoint(, "Posicionar")

        Dim VALORINICIALX As Double = 3



        For i = 0 To Muros_V.Count - 1

            For j = 0 To Muros_V(i).CoordenadasX.Count - 1
                Muros_V(i).Xmax = Muros_V(i).Xmax + VALORINICIALX
                Muros_V(i).Xmin = Muros_V(i).Xmin + VALORINICIALX


            Next


            Muros_V(i).VerticesAGraficar()
        Next


        For i = 0 To ListaOrdenada.Count - 1

            Polyline = AcadDoc.ModelSpace.AddLightWeightPolyline(ListaOrdenada(i).CoordenadasaGraficas.ToArray)
            Polyline.Layer = "FC_MURO CONCRETO"
            Polyline.Closed = True
        Next


        ListaOrdenada.Clear()
        Selecccionar.Clear()
        Muros_V.Clear()


        MsgBox("FIN")

    End Sub














    Public Function ClasificarObjectos(Objeto As AcadObject, Numero As Integer) As String
        Dim NombreObjecto As String = "Sin Definir"

        If Objeto.ObjectName = "AcDbPolyline" Then
            Dim Polyline2 As AcadLWPolyline = Objeto
            If Polyline2.Layer = "FC_MURO CONCRETO" Then
                NombreObjecto = "M" & Numero

            End If
        End If

        If Objeto.ObjectName = "AcDbBlockReference" Then
            Dim BloqueReferencia As AcadBlockReference = Objeto
            If BloqueReferencia.Layer = "FC_NUMERO" Then
                Dim NombreBloque As String = BloqueReferencia.GetAttributes(0).textstring
                NombreObjecto = NombreBloque
            End If
        End If

        Return NombreObjecto

    End Function


    Public Function EspesorMuro(MaximoX, MinimoX, MaximoY, MinimoY) As List(Of Object)

        Dim EspesorX As Double = MaximoX - MinimoX
        Dim EspesorY As Double = MaximoY - MinimoY
        Dim Espesor As Double
        Dim Longitud As Double
        Dim Direccion As String
        Dim EspesorMuro1 As New List(Of Object)
        If EspesorX < EspesorY Then
            Espesor = EspesorX
            Longitud = EspesorY
            Direccion = "Vertical"
        Else
            Espesor = EspesorY
            Longitud = EspesorX
            Direccion = "Horizontal"
        End If

        EspesorMuro1.Add(EspesorX)
        EspesorMuro1.Add(EspesorY)
        EspesorMuro1.Add(Espesor)
        EspesorMuro1.Add(Direccion)
        EspesorMuro1.Add(Longitud)
        EspesorMuro = EspesorMuro1

    End Function



    Public Function Coordenadas(Objeto As AcadObject) As List(Of List(Of Double))

        Dim Coord As New List(Of List(Of Double))
        Dim Coordenadas_X As New List(Of Double)
        Dim Coordenadas_Y As New List(Of Double)

        If Objeto.ObjectName = "AcDbPolyline" Then
            Dim Polyline2 As AcadLWPolyline = Objeto
            If Polyline2.Layer = "FC_MURO CONCRETO" Then
                For i = 0 To 3
                    Coordenadas_X.Add(Polyline2.Coordinates(i * 2))
                    Coordenadas_Y.Add(Polyline2.Coordinates(i * 2 + 1))
                Next
            End If
        End If

        Coord.Add(Coordenadas_X)
        Coord.Add(Coordenadas_Y)
        Coordenadas = Coord


    End Function


    Function PropiedadesdelBloque(Objeto As AcadObject) As List(Of Double())

        Dim PropiedadesdelBloque2 As New List(Of Double())
        Dim MinPunto(2) As Double : Dim MaxPunto(2) As Double
        Dim Radio(0) As Double


        If Objeto.ObjectName = "AcDbBlockReference" Then
            Dim BloqueReferencia As AcadBlockReference = Objeto
            Dim Propiedades_Dinamicas As Object = BloqueReferencia.GetDynamicBlockProperties
            Dim Editar_Propiedades2 As AcadDynamicBlockReferenceProperty
            If BloqueReferencia.Layer = "FC_NUMERO" Then
                Editar_Propiedades2 = Propiedades_Dinamicas(0)
                Radio(0) = Editar_Propiedades2.Value
                BloqueReferencia.GetBoundingBox(MinPunto, MaxPunto)
            End If
        End If

        PropiedadesdelBloque2.Add(Radio)
        PropiedadesdelBloque2.Add(MinPunto)

        PropiedadesdelBloque = PropiedadesdelBloque2

    End Function







End Module
