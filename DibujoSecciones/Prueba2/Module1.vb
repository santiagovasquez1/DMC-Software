Imports Autodesk.AutoCAD.Interop
Imports Autodesk.AutoCAD.Interop.Common
Imports MathNet.Numerics.LinearAlgebra
Imports Diseño_de_muros_concreto_V2

Module Module1

    Public AcadApp As Autodesk.AutoCAD.Interop.AcadApplication
    Public AcadDoc As Autodesk.AutoCAD.Interop.AcadDocument
    Public Selecccionar As AcadSelectionSet
    Public Polyline, Polyline2 As AcadLWPolyline
    Public Muros_V As New List(Of Muros)
    Public CircunferenciasBloques As New List(Of CircunferenciaBloque)
    Public MurosAranas As New List(Of MuroArana)
    Public Hacth As AcadHatch
    Public Refuerzo_Circulo As AcadBlockReference
    Public RefuerzoHorizontal, RefuerzoHorizontal2, BloqueNombreMuro As AcadBlockReference
    Public Lista_TextosRefuerzos As New List(Of TextoRefuerzo)
    Public Lista_CirculoRefuerzos As New List(Of RefuerzoCirculo)
    Public Cota As AcadDimRotated
    Public Texto As AcadText
    Public TablaAutocad As AcadTable
    Public NombreMuro As AcadBlockReference


    Public Linea As AcadLine
    Public ListaOrdenada As List(Of Muros)


    Sub IniciarAplicacion(ByVal Formulario As Form1)


        Dim rnd As New Random

        Try
            AcadApp = GetObject(, "Autocad.Application")
            AcadDoc = AcadApp.ActiveDocument
        Catch ex As Exception
            Dim ruta As New OpenFileDialog()
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

        Dim Seleccion_name As String = "Mi seleccion"

        For Each seleccion_i As AcadSelectionSet In AcadDoc.SelectionSets

            If seleccion_i.Name = Seleccion_name Then
                seleccion_i.Delete()
                Exit For
            End If
        Next



        AcadDoc = AcadApp.ActiveDocument
        AcadDoc.PurgeAll()
        Selecccionar = AcadDoc.SelectionSets.Add(Seleccion_name)

        MsgBox("Seleccionar un Conjunto de Muros", vbInformation, "efe Prima Ce")
        Selecccionar.SelectOnScreen()   'Todos los objectos seleccionaes estan en la variable Seleccionar

        Formulario.BarraPersonalizada.Visible = True
        Dim ProgresoBarra As Integer
        Dim ProgresoLabel As Double
        If Selecccionar.Count > 0 Then
            ProgresoBarra = Formulario.BarraPersonalizada.Width / Selecccionar.Count
            Formulario.BarraPersonalizada2.Visible = True
            Formulario.BarraPersonalizada2.Width = 0
        End If
        Dim ProgresoIn As Double = 0

        Dim num As Integer = 1
        For Each Objeto In Selecccionar
            Dim Muro As New Muros
            Dim Circu As New CircunferenciaBloque
            Dim LabelN As New TextoRefuerzo
            Dim CirculoRefuerzo As New RefuerzoCirculo

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

            If Clasificar <> "Sin Definir" And Objeto.ObjectName = "AcDbText" Then
                LabelN.NombreTexto = Clasificar
                LabelN.CoordenasdasXyY = (PropiedadesdelBloque(Objeto)(1))
                Lista_TextosRefuerzos.Add(LabelN)
            End If

            If Clasificar <> "Sin Definir" And Objeto.ObjectName = "AcDbCircle" Then
                CirculoRefuerzo.Label = Clasificar
                CirculoRefuerzo.CoordenadasXyY = (PropiedadesdelBloque(Objeto)(1))
                Lista_CirculoRefuerzos.Add(CirculoRefuerzo)
            End If

            'CARGANDO....
            ProgresoIn = ProgresoIn + ProgresoLabel
            Formulario.BarraPersonalizada2.Width = Formulario.BarraPersonalizada2.Width + ProgresoBarra
        Next

        'GUARDAR DATOS
        For i = 0 To Muros_V.Count - 1
            Dim X_min, X_max, Y_min, Y_max As Double
            X_min = Muros_V(i).CoordenadasX.Min
            Y_min = Muros_V(i).CoordenadasY.Min
            X_max = Muros_V(i).CoordenadasX.Max
            Y_max = Muros_V(i).CoordenadasY.Max


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
            X_min = Math.Round(Muros_V(i).Xmin, 2)
            Y_min = Math.Round(Muros_V(i).Ymin, 2)
            X_max = Math.Round(Muros_V(i).Xmax, 2)
            Y_max = Math.Round(Muros_V(i).Ymax, 2)


            For j = 0 To Muros_V.Count - 1

                If i <> j Then
                    Dim X_min1, X_max1, Y_min1, Y_max1 As Double
                    X_min1 = Math.Round(Muros_V(j).Xmin, 2)
                    Y_min1 = Math.Round(Muros_V(j).Ymin, 2)
                    X_max1 = Math.Round(Muros_V(j).Xmax, 2)
                    Y_max1 = Math.Round(Muros_V(j).Ymax, 2)

                    If X_min1 = X_min Or X_min1 = X_max Or X_max1 = X_min Or X_max1 = X_max Then
                        If Y_max1 >= Y_max And Y_max >= Y_min1 OrElse Y_max >= Y_max1 And Y_max1 >= Y_min Then
                            Muros_V(i).MurosVecinos.Add(Muros_V(j).NombreMuro)
                            Muros_V(i).MurosVecinosClase.Add(Muros_V(j))
                        End If

                    ElseIf Y_min1 = Y_min Or Y_min1 = Y_max Or Y_max1 = Y_min Or Y_max1 = Y_max Then
                        If X_max1 >= X_max And X_max >= X_min1 OrElse X_max >= X_max1 And X_max1 >= X_min Then
                            Muros_V(i).MurosVecinos.Add(Muros_V(j).NombreMuro)
                            Muros_V(i).MurosVecinosClase.Add(Muros_V(j))
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

        ListaOrdenada = Muros_V.OrderBy((Function(x) x.Xmin)).ToList()

        For i = 0 To ListaOrdenada.Count - 1
            ListaOrdenada(i).MurosVecinosP.Clear()
        Next

        For i = 0 To ListaOrdenada.Count - 1
            For k = 0 To ListaOrdenada(i).MurosVecinos.Count - 1

                For j = 0 To ListaOrdenada.Count - 1
                    If i <> j Then
                        If ListaOrdenada(i).MurosVecinos(k) = ListaOrdenada(j).NombreMuro Then
                            ListaOrdenada(i).MurosVecinosP.Add(j)
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
                ListaOrdenada(i).MurosVeciosXmin.Add(ListaOrdenada(ListaOrdenada(i).MurosVecinosP(j)).Xmin)

            Next
        Next


        For i = 0 To ListaOrdenada.Count - 1
            For j = 0 To ListaOrdenada(i).MurosVecinos.Count - 1
                ListaOrdenada(i).MurosVeciosYmin.Add(ListaOrdenada(ListaOrdenada(i).MurosVecinosPY(j)).Ymin)

            Next
        Next


        Dim PosicionX As Double

        Dim AuxX As Integer


        For i = 0 To ListaOrdenada.Count - 1

            For j = 0 To ListaOrdenada(i).MurosVeciosXmin.Count - 1
                For m = 0 To (ListaOrdenada(i).MurosVeciosXmin.Count - 1) - j - 1

                    If ListaOrdenada(i).MurosVeciosXmin(m) > ListaOrdenada(i).MurosVeciosXmin(m + 1) Then
                        PosicionX = ListaOrdenada(i).MurosVeciosXmin(m)
                        AuxX = ListaOrdenada(i).MurosVecinosP(m)

                        ListaOrdenada(i).MurosVeciosXmin(m) = ListaOrdenada(i).MurosVeciosXmin(m + 1)
                        ListaOrdenada(i).MurosVecinosP(m) = ListaOrdenada(i).MurosVecinosP(m + 1)

                        ListaOrdenada(i).MurosVeciosXmin(m + 1) = PosicionX
                        ListaOrdenada(i).MurosVecinosP(m + 1) = AuxX


                    End If
                Next
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

        '-------------------------------INICIO ESCALA


        For i = 0 To ListaOrdenada.Count - 1
            ListaOrdenada(i).ClasificacionMuros()
        Next
        ESCALA()

        '-----------------------------------------------------FIN DE ESCALA


        'CARGAR DATOS ----> Elemento de Borde, Numero de Barras, Longitud, Malla

        Dim No_Piso As Integer = Val(Formulario.Piso_Box.Text)

        For i = 0 To ListaOrdenada.Count - 1
            Try


                For j = 0 To Muros_lista_2.Count - 1

                    If ListaOrdenada(i).NombreMuro = Muros_lista_2(j).Pier_name Then

                        Dim Indice = Muros_lista_2(j).Stories.FindIndex(Function(x) x = ("Story" & No_Piso))


                        ListaOrdenada(i).LEB_Iz = Muros_lista_2(j).Lebe_Izq(Indice) / 100
                        ListaOrdenada(i).LEB_Dr = Muros_lista_2(j).Lebe_Der(Indice) / 100

                        ListaOrdenada(i).Hatch_pattern_Izq = "SOLID"
                        ListaOrdenada(i).Hatch_pattern_Der = "SOLID"
                        ListaOrdenada(i).Hatch_Layer_Izq = "FC_HATCH MUROS"
                        ListaOrdenada(i).Hatch_Layer_Der = "FC_HATCH MUROS"


                        If Muros_lista_2(j).Lebe_Der(Indice) = 0 Then
                            ListaOrdenada(i).LEB_Dr = Muros_lista_2(j).Zc_Der(Indice) / 100
                            ListaOrdenada(i).Hatch_pattern_Der = "DOTS"
                            ListaOrdenada(i).Hatch_Layer_Der = "FC_HATCH 252"
                        End If

                        If Muros_lista_2(j).Lebe_Izq(Indice) = 0 Then
                            ListaOrdenada(i).LEB_Iz = Muros_lista_2(j).Zc_Izq(Indice) / 100
                            ListaOrdenada(i).Hatch_pattern_Izq = "DOTS"
                            ListaOrdenada(i).Hatch_Layer_Izq = "FC_HATCH 252"
                        End If


                        ListaOrdenada(i).Malla = Muros_lista_2(j).Malla(Indice)
                        ListaOrdenada(i).Capas_RefuerzoHorizontal = Muros_lista_2(j).Capas_htal(Indice)
                        ListaOrdenada(i).RefuerzoHorizontalLabel = Muros_lista_2(j).Ref_htal(Indice)

                        Try
                            ListaOrdenada(i).Lista_NoBarras = Muros_lista_2(j).NombreBarras(Indice)
                            ListaOrdenada(i).Lista_LongitudBarras = Muros_lista_2(j).LongitudBarras(Indice)
                        Catch EX As Exception

                        End Try




                    End If
                Next
            Catch ex As Exception


            End Try
        Next

        ''Definir las escalas del dibujo

        'Dim Escala_dibujo As New Commands_Autocad
        'Escala_dibujo.add_scale(AcadDoc)


        Formulario.BarraPersonalizada.Visible = False
        Formulario.BarraPersonalizada2.Visible = False
        Formulario.Label_BarraProgreso.Visible = False

        For i = 0 To ListaOrdenada.Count - 1

            For j = 0 To Lista_CirculoRefuerzos.Count - 1 Step 2

                If Lista_CirculoRefuerzos(j).CoordenadasXyY(0) >= ListaOrdenada(i).XminE And Lista_CirculoRefuerzos(j).CoordenadasXyY(0) <= ListaOrdenada(i).XmaxE AndAlso Lista_CirculoRefuerzos(j).CoordenadasXyY(1) >= ListaOrdenada(i).YminE And Lista_CirculoRefuerzos(j).CoordenadasXyY(1) <= ListaOrdenada(i).YmaxE Then
                    ListaOrdenada(i).Lista_Refuerzos_Original.Add(Lista_CirculoRefuerzos(j).CoordenadasXyY.ToArray)
                End If

            Next

            If ListaOrdenada(i).DireccionMuro = "Horizontal" Then
                ListaOrdenada(i).Lista_Refuerzos_Original = ListaOrdenada(i).Lista_Refuerzos_Original.OrderBy(Function(x) x(0)).ToList
            Else
                ListaOrdenada(i).Lista_Refuerzos_Original = ListaOrdenada(i).Lista_Refuerzos_Original.OrderBy(Function(x) x(1)).ToList
            End If

        Next

        Dim A As Object
        Try
            A = AcadDoc.Utility.GetPoint(, "Posicionar")
        Catch

            Muros_V.Clear()
            ListaOrdenada.Clear()
            Selecccionar.Clear()
            Lista_CirculoRefuerzos.Clear()
            Lista_TextosRefuerzos.Clear()
            Exit Sub
        End Try

        Dim Ymin As Double = 99999
        Dim Xmin As Double = 99999

        For i = 0 To ListaOrdenada.Count - 1

            If ListaOrdenada(i).Ymin < Ymin Then
                Ymin = ListaOrdenada(i).Ymin
            End If
            If ListaOrdenada(i).Xmin < Xmin Then
                Xmin = ListaOrdenada(i).Xmin
            End If

        Next


        For i = 0 To ListaOrdenada.Count - 1

            ListaOrdenada(i).CoordenadasaGraficas = Traslacion_Coordenas(A(0) - Xmin, A(1) - Ymin, ListaOrdenada(i).Xmin, ListaOrdenada(i).Xmax, ListaOrdenada(i).Ymin, ListaOrdenada(i).Ymax)

        Next
        'Actualizar Coordenadas 
        For i = 0 To ListaOrdenada.Count - 1
            ListaOrdenada(i).Xmin = ListaOrdenada(i).CoordenadasaGraficas(0)
            ListaOrdenada(i).Xmax = ListaOrdenada(i).CoordenadasaGraficas(2)
            ListaOrdenada(i).Ymin = ListaOrdenada(i).CoordenadasaGraficas(1)
            ListaOrdenada(i).Ymax = ListaOrdenada(i).CoordenadasaGraficas(5)

        Next


        Dim Ymax As Double = -99999
        Dim Xmax As Double = -99999

        For i = 0 To ListaOrdenada.Count - 1

            If ListaOrdenada(i).Ymax > Ymax Then
                Ymax = ListaOrdenada(i).Ymax
            End If
            If ListaOrdenada(i).Xmax > Xmax Then
                Xmax = ListaOrdenada(i).Xmax
            End If

        Next


        'AsignarBloques----------> NOMBRES DE MUROS


        For i = 0 To ListaOrdenada.Count - 1

            AddBloqueNombresMuros(ListaOrdenada(i))

        Next






        'Asignar cada Circulo Refuerzo con Su Respectivo Label
        For s = 0 To ListaOrdenada.Count - 1
            For i = 0 To Lista_CirculoRefuerzos.Count - 1
                Dim Dmin As Double = 99999999
                For j = 0 To Lista_TextosRefuerzos.Count - 1


                    Distancia_ = Math.Sqrt((Lista_CirculoRefuerzos(i).CoordenadasXyY(0) - Lista_TextosRefuerzos(j).CoordenasdasXyY(0)) ^ 2 + (Lista_CirculoRefuerzos(i).CoordenadasXyY(1) - Lista_TextosRefuerzos(j).CoordenasdasXyY(1)) ^ 2)




                    If Dmin > Distancia_ Then
                        Dmin = Distancia_
                        Lista_CirculoRefuerzos(i).Label = Lista_TextosRefuerzos(j).NombreTexto
                    End If
                Next

            Next
        Next

        'Asignar Cada Muro a CirculoRefuerzo

        For i = 0 To ListaOrdenada.Count - 1
            For j = 0 To Lista_CirculoRefuerzos.Count - 1

                If Lista_CirculoRefuerzos(j).CoordenadasXyY(0) > ListaOrdenada(i).XminE And Lista_CirculoRefuerzos(j).CoordenadasXyY(0) < ListaOrdenada(i).XmaxE And Lista_CirculoRefuerzos(j).CoordenadasXyY(1) > ListaOrdenada(i).YminE And Lista_CirculoRefuerzos(j).CoordenadasXyY(1) < ListaOrdenada(i).YmaxE Then

                    Lista_CirculoRefuerzos(j).MuroPerteneciente = ListaOrdenada(i).NombreMuro
                    Lista_CirculoRefuerzos(j).IndiceMuroPerteneciente = i
                    ListaOrdenada(i).Lista_Refuerzos.Add(Lista_CirculoRefuerzos(j))
                End If
            Next
        Next




        For i = 0 To Lista_CirculoRefuerzos.Count - 1

            Dim YBarra = (Lista_CirculoRefuerzos(i).CoordenadasXyY(1)) - ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).YminE
            Dim Y = ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).YmaxE - ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).YminE
            Dim YE = ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).Ymax - ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).Ymin

            Dim YBarraE = (YBarra * YE) / Y

            Lista_CirculoRefuerzos(i).CoordenadasXyY(1) = ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).Ymin + YBarraE

            Dim XBarra = (Lista_CirculoRefuerzos(i).CoordenadasXyY(0)) - ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).XminE
            Dim X = ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).XmaxE - ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).XminE
            Dim XE = ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).Xmax - ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).Xmin

            Dim XBarraE = (XBarra * XE) / X

            Lista_CirculoRefuerzos(i).CoordenadasXyY(0) = ListaOrdenada(Lista_CirculoRefuerzos(i).IndiceMuroPerteneciente).Xmin + XBarraE


        Next

        '-------------------------------ASIGNAR FILAS DE RECUBRIMIENTO A CADA MURO 

        For i = 0 To ListaOrdenada.Count - 1
            Dim MenorRecubrimiento As Double = 9999999
            If ListaOrdenada(i).DireccionMuro = "Vertical" Then


                For j = 0 To ListaOrdenada(i).Lista_Refuerzos.Count - 1

                    If MenorRecubrimiento > ListaOrdenada(i).Lista_Refuerzos(j).CoordenadasXyY(0) Then
                        MenorRecubrimiento = ListaOrdenada(i).Lista_Refuerzos(j).CoordenadasXyY(0)
                    End If

                Next
                ListaOrdenada(i).RecubrimientoRefuerzo = MenorRecubrimiento


            Else

                For j = 0 To ListaOrdenada(i).Lista_Refuerzos.Count - 1
                    If MenorRecubrimiento > ListaOrdenada(i).Lista_Refuerzos(j).CoordenadasXyY(1) Then
                        MenorRecubrimiento = ListaOrdenada(i).Lista_Refuerzos(j).CoordenadasXyY(1)
                    End If

                Next
                ListaOrdenada(i).RecubrimientoRefuerzo = MenorRecubrimiento

            End If


        Next


        For i = 0 To ListaOrdenada.Count - 1

            For j = 0 To ListaOrdenada(i).Lista_Refuerzos.Count - 1
                If ListaOrdenada(i).DireccionMuro = "Vertical" Then
                    If Math.Round(ListaOrdenada(i).Lista_Refuerzos(j).CoordenadasXyY(0), 3) = Math.Round(ListaOrdenada(i).RecubrimientoRefuerzo, 3) Then
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Add(ListaOrdenada(i).Lista_Refuerzos(j))
                    Else
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Max.Add(ListaOrdenada(i).Lista_Refuerzos(j))
                    End If
                Else
                    If Math.Round(ListaOrdenada(i).Lista_Refuerzos(j).CoordenadasXyY(1), 3) = Math.Round(ListaOrdenada(i).RecubrimientoRefuerzo, 3) Then
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Min.Add(ListaOrdenada(i).Lista_Refuerzos(j))
                    Else
                        ListaOrdenada(i).Lista_Refuerzos_Fila_Max.Add(ListaOrdenada(i).Lista_Refuerzos(j))
                    End If

                End If
            Next


        Next

        'ORDENAR SEGUN COORDENADAS
        For i = 0 To ListaOrdenada.Count - 1
            If ListaOrdenada(i).DireccionMuro = "Vertical" Then
                Dim ListaNuevaOrdenar As List(Of RefuerzoCirculo)
                ListaNuevaOrdenar = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.OrderBy(Function(x) x.CoordenadasXyY(1)).ToList
                ListaOrdenada(i).Lista_Refuerzos_Fila_Min = ListaNuevaOrdenar


                ListaNuevaOrdenar = ListaOrdenada(i).Lista_Refuerzos_Fila_Max.OrderBy(Function(x) x.CoordenadasXyY(1)).ToList
                ListaOrdenada(i).Lista_Refuerzos_Fila_Max = ListaNuevaOrdenar


            Else
                Dim ListaNuevaOrdenar As List(Of RefuerzoCirculo)

                ListaNuevaOrdenar = ListaOrdenada(i).Lista_Refuerzos_Fila_Min.OrderBy(Function(x) x.CoordenadasXyY(0)).ToList
                ListaOrdenada(i).Lista_Refuerzos_Fila_Min = ListaNuevaOrdenar

                ListaNuevaOrdenar = ListaOrdenada(i).Lista_Refuerzos_Fila_Max.OrderBy(Function(x) x.CoordenadasXyY(0)).ToList
                ListaOrdenada(i).Lista_Refuerzos_Fila_Max = ListaNuevaOrdenar
            End If

        Next



        'Cambio de Recubrimiento a 0.038

        For i = 0 To ListaOrdenada.Count - 1

            With ListaOrdenada(i)

                If .DireccionMuro = "Horizontal" Then

                    For j = 0 To .Lista_Refuerzos_Fila_Min.Count - 1
                        .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(1) = .Ymin + 0.038
                    Next
                    For j = 0 To .Lista_Refuerzos_Fila_Max.Count - 1
                        .Lista_Refuerzos_Fila_Max(j).CoordenadasXyY(1) = .Ymax - 0.038
                    Next

                Else
                    For j = 0 To .Lista_Refuerzos_Fila_Min.Count - 1
                        .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(0) = .Xmin + 0.038
                    Next
                    For j = 0 To .Lista_Refuerzos_Fila_Max.Count - 1
                        .Lista_Refuerzos_Fila_Max(j).CoordenadasXyY(0) = .Xmax - 0.038
                    Next
                End If

            End With


        Next

        For i = 0 To ListaOrdenada.Count - 1

            With ListaOrdenada(i)
                If .DireccionMuro = "Horizontal" Then

                    .RecubrimientoRefuerzo = .Ymin + 0.038
                Else
                    .RecubrimientoRefuerzo = .Xmin + 0.038
                End If

            End With
        Next




        '-------------------------------ASIGNAR FILAS DE RECUBRIMIENTO A CADA MURO -------------- FIN

        For i = 0 To ListaOrdenada.Count - 1

            ListaOrdenada(i).PutosHatchFuc()

        Next

        For i = 0 To ListaOrdenada.Count - 1


            If ListaOrdenada(i).LEB_Iz <> 0 Then

                Add_LW_PL(ListaOrdenada(i).PuntosHatchIz, "FC_BORDES")
                Add_Hatch(Polyline2, ListaOrdenada(i).Hatch_pattern_Izq, ListaOrdenada(i).Hatch_Layer_Izq, 0.9)
            End If

            If ListaOrdenada(i).LEB_Dr <> 0 Then

                Add_LW_PL(ListaOrdenada(i).PuntosHatchDer, "FC_BORDES")
                Add_Hatch(Polyline2, ListaOrdenada(i).Hatch_pattern_Der, ListaOrdenada(i).Hatch_Layer_Der, 0.9)
            End If

        Next

        For i = 0 To ListaOrdenada.Count - 1

            Polyline = AcadDoc.ModelSpace.AddLightWeightPolyline(ListaOrdenada(i).CoordenadasaGraficas.ToArray)
            Polyline.Layer = "FC_BORDES"
            Polyline.Closed = True
            Polyline.Update()
        Next


        'DIBUJO DE REFUERZO
        For i = 0 To ListaOrdenada.Count - 1
            Dim LabelAux As Integer
            LabelAux = Str(1 + i)
            With ListaOrdenada(i)
                For j = 0 To .Lista_Refuerzos_Fila_Min.Count - 1
                    AddRefuerzo(.Lista_Refuerzos_Fila_Min(j).CoordenadasXyY, LabelAux, 1, "FC_REFUERZO 2")
                Next
                For j = 0 To .Lista_Refuerzos_Fila_Max.Count - 1


                    AddRefuerzo(.Lista_Refuerzos_Fila_Max(j).CoordenadasXyY, LabelAux, 1, "FC_REFUERZO 2")
                Next

            End With
        Next

        'COTAS Y NUMERO DE REFUERZO
        For i = 0 To ListaOrdenada.Count - 1
            With ListaOrdenada(i)
                If .DireccionMuro = "Vertical" Then
                    Dim RangoEspesorLista As New List(Of Double())
                    Dim RangoEspesorListaX As New List(Of Double())
                    If .MurosVecinosIzquierda.Count <> 0 Then

                        For m = 0 To .MurosVecinosIzquierda.Count - 1
                            Dim RangoEspesor(1) As Double

                            RangoEspesor(0) = .MurosVecinosIzquierda(m).Ymax + 0.45
                            RangoEspesor(1) = .MurosVecinosIzquierda(m).Ymin - 0.3
                            RangoEspesorLista.Add(RangoEspesor)

                            Dim RangoEspesorX(0) As Double
                            RangoEspesorX(0) = .MurosVecinosIzquierda(m).Xmax
                            RangoEspesorListaX.Add(RangoEspesorX)
                        Next

                    End If


                    If .MurosVecinosDerecha.Count <> 0 Then
                        For m = 0 To .MurosVecinosDerecha.Count - 1
                            Dim RangoEspesor(1) As Double
                            RangoEspesor(0) = .MurosVecinosDerecha(m).Ymax + 0.45
                            RangoEspesor(1) = .MurosVecinosDerecha(m).Ymin - 0.3
                            RangoEspesorLista.Add(RangoEspesor)

                            Dim RangoEspesorX(0) As Double
                            RangoEspesorX(0) = .MurosVecinosDerecha(m).Xmax
                            RangoEspesorListaX.Add(RangoEspesorX)
                        Next

                    End If

                    'Desplazamiento adicional para Numero de Refuerzo
                    If .MurosVecinosAbajo.Count <> 0 Then

                        For m = 0 To .MurosVecinosAbajo.Count - 1
                            Dim RangoEspesor(1) As Double
                            RangoEspesor(0) = .MurosVecinosAbajo(m).Ymax + 0.3
                            RangoEspesor(1) = .MurosVecinosAbajo(m).Ymax
                            RangoEspesorLista.Add(RangoEspesor)
                            Dim RangoEspesorX(0) As Double
                            RangoEspesorX(0) = 99999999
                            RangoEspesorListaX.Add(RangoEspesorX)
                        Next


                    End If



                    For j = 0 To .Lista_Refuerzos_Fila_Min.Count - 1
                        Dim Alto = 0.0375
                        Dim FactorSubir = 1.35
                        Dim DesplazaCotaDerecha As Double = 0.2
                        Dim FactoAdicionalTexto As Double = 1
                        Dim FactorAdicionalTextoIzquierda1 As Double = 1
                        Dim FactorAdicionalTextoIzquierda2 As Double = 1


                        For m = 0 To RangoEspesorLista.Count - 1
                            Dim RangoEspesorAux = RangoEspesorLista(m)
                            If RangoEspesorAux.Count > 1 Then
                                If RangoEspesorListaX(m)(0) < .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(0) Then

                                    If RangoEspesorAux(0) >= .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(1) And RangoEspesorAux(1) <= .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(1) Then
                                        DesplazaCotaDerecha = -0.1
                                        FactoAdicionalTexto = -0.05
                                    End If
                                End If
                            End If
                        Next



                        For m = 0 To RangoEspesorLista.Count - 1
                            Dim RangoEspesorAux = RangoEspesorLista(m)
                            If RangoEspesorAux.Count > 1 Then
                                If RangoEspesorListaX(m)(0) > .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(0) Then
                                    If RangoEspesorAux(0) >= .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(1) And RangoEspesorAux(1) <= .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(1) Then
                                        FactorAdicionalTextoIzquierda1 = -1.03
                                        FactorAdicionalTextoIzquierda2 = -0.6
                                    End If
                                End If
                            End If
                        Next


                        For m = 0 To RangoEspesorLista.Count - 1
                            Dim RangoEspesorAux = RangoEspesorLista(m)

                            If RangoEspesorAux(0) >= .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(1) And RangoEspesorAux(1) <= .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(1) Then
                                FactorSubir = 0.9
                                FactoAdicionalTexto = -0.05
                                FactorAdicionalTextoIzquierda1 = -1.03
                                FactorAdicionalTextoIzquierda2 = -0.6
                            End If

                        Next



                        'COTAS
                        If j < .Lista_Refuerzos_Fila_Min.Count - 1 Then


                            AddCota(.Lista_Refuerzos_Fila_Min(j).CoordenadasXyY, .Lista_Refuerzos_Fila_Min(j + 1).CoordenadasXyY, 90, "", False, DesplazaCotaDerecha, 0.15)

                        End If

                        'TEXTOS DE BARRAS
                        Try
                            Dim TextoString = ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).Label
                            Dim FactorDesplazado = If(Len(TextoString) = 1, 0.12 * FactoAdicionalTexto, 0.15 * FactoAdicionalTexto)
                            Dim Ubicacion = {ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(0) - FactorDesplazado, ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(1) - FactorSubir * Alto, 0}
                            AddTexto(TextoString, Ubicacion, Alto, "FC_R-100", "FC_TEXT")
                        Catch ex As Exception

                        End Try
                        Try
                            Dim TextoString2 = ListaOrdenada(i).Lista_Refuerzos_Fila_Max(j).Label
                            Dim FactorDesplazado2 = If(Len(TextoString2) = 1, 0.1 * FactorAdicionalTextoIzquierda2, 0.08 * FactorAdicionalTextoIzquierda1)
                            Dim Ubicacion2 = {ListaOrdenada(i).Lista_Refuerzos_Fila_Max(j).CoordenadasXyY(0) + FactorDesplazado2, ListaOrdenada(i).Lista_Refuerzos_Fila_Max(j).CoordenadasXyY(1) - FactorSubir * Alto, 0}
                            AddTexto(TextoString2, Ubicacion2, Alto, "FC_R-100", "FC_TEXT")
                        Catch ex As Exception

                        End Try


                    Next
                Else

                    'FALTA MODIFICAR COTAS DE ARRIBA!!!!!!
                    'MUROS HORIZONTALES
                    Dim RangoEspesorLista As New List(Of Double())
                    If .MurosVecinosDerecha.Count <> 0 Then

                        For m = 0 To .MurosVecinosDerecha.Count - 1
                            Dim RangoEspesor(0) As Double
                            RangoEspesor(0) = .MurosVecinosDerecha(m).Xmin - 0.3
                            RangoEspesorLista.Add(RangoEspesor)
                        Next

                    End If

                    If .MurosVecinosArriba.Count <> 0 Then
                        For m = 0 To .MurosVecinosArriba.Count - 1
                            Dim RangoEspesor(1) As Double
                            RangoEspesor(0) = .MurosVecinosArriba(m).Xmax + 0.4
                            RangoEspesor(1) = .MurosVecinosArriba(m).Xmin - 0.4
                            RangoEspesorLista.Add(RangoEspesor)
                        Next

                    End If

                    If .MurosVecinosAbajo.Count <> 0 Then
                        For m = 0 To .MurosVecinosAbajo.Count - 1
                            Dim RangoEspesor(1) As Double
                            RangoEspesor(0) = .MurosVecinosAbajo(m).Xmax + 0.4
                            RangoEspesor(1) = .MurosVecinosAbajo(m).Xmin - 0.4
                            RangoEspesorLista.Add(RangoEspesor)
                        Next

                    End If

                    For j = 0 To ListaOrdenada(i).Lista_Refuerzos_Fila_Max.Count - 1
                            Dim Alto = 0.0375

                        Dim FactorDesplazado1 = 0.12 : Dim FactorDesplazado2 = 0.12 : Dim DesplazamientoX As Double = 0.02
                        Dim DesplazamientoCota = 0.15

                        'Desplazamiento adicional para Numero de Refuerzo

                        For m = 0 To RangoEspesorLista.Count - 1
                            Dim RangoEspesorAux = RangoEspesorLista(m)
                            If RangoEspesorAux(0) <= .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(0) Then
                                DesplazamientoX = -0.05
                                FactorDesplazado1 = -0.12
                                FactorDesplazado2 = -0.12
                            End If
                        Next


                        'DesplazamientoCotas Por MurosArriba y MurosAbajo



                        For m = 0 To RangoEspesorLista.Count - 1
                            Dim RangoEspesorAux = RangoEspesorLista(m)
                            If RangoEspesorAux.Count > 1 Then
                                If RangoEspesorAux(0) >= .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(0) And RangoEspesorAux(1) <= .Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(0) Then
                                    DesplazamientoCota = -0.09
                                    DesplazamientoX = -0.05
                                    FactorDesplazado1 = -0.12
                                    FactorDesplazado2 = -0.12
                                End If
                            End If
                        Next









                        If j < ListaOrdenada(i).Lista_Refuerzos_Fila_Max.Count - 1 Then
                            AddCota(ListaOrdenada(i).Lista_Refuerzos_Fila_Max(j).CoordenadasXyY, ListaOrdenada(i).Lista_Refuerzos_Fila_Max(j + 1).CoordenadasXyY, 0, "", False, 0.2, DesplazamientoCota)
                        End If





                        Try
                            Dim TextoString = ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).Label

                            Dim Ubicacion = {ListaOrdenada(i).Lista_Refuerzos_Fila_Min(j).CoordenadasXyY(0) + DesplazamientoX, ListaOrdenada(i).Ymin - FactorDesplazado1 / 2, 0}

                            AddTexto(TextoString, Ubicacion, Alto, "FC_R-100", "FC_TEXT")
                        Catch ex As Exception

                        End Try
                        Try
                            Dim TextoString2 = ListaOrdenada(i).Lista_Refuerzos_Fila_Max(j).Label

                            Dim Ubicacion2 = {ListaOrdenada(i).Lista_Refuerzos_Fila_Max(j).CoordenadasXyY(0) + DesplazamientoX, ListaOrdenada(i).Ymax + FactorDesplazado2 / 3, 0}

                            AddTexto(TextoString2, Ubicacion2, Alto, "FC_R-100", "FC_TEXT")
                        Catch ex As Exception

                        End Try



                    Next

                End If
            End With
        Next


        'Arreglar Numero de Barras

        For i = 0 To ListaOrdenada.Count - 1
            For j = 0 To ListaOrdenada(i).Lista_NoBarras.Count - 1
                Dim Texto As String = ListaOrdenada(i).Lista_NoBarras(j)
                Dim Inicio As Integer
                For Inicio = 0 To Len(Texto) - 1
                    If Texto.Chars(Inicio) = "T" Then
                        Exit For
                    End If
                Next
                Texto = Texto.Substring(0, Inicio)
                ListaOrdenada(i).Lista_NoBarras(j) = Texto
            Next
        Next

        If Lista_CirculoRefuerzos.Count <> 0 Then

            Dim CoordTabla(2) As Double
            Dim NombresMuros As New List(Of String)
            NombresMuros = ListaOrdenada.Select(Function(x) x.NombreMuro).ToList()
            CoordTabla(0) = Xmax + 2 : CoordTabla(1) = Ymax


            CrearTabla(CoordTabla, NombresMuros)

        End If

        ''LABEL ----> BARRAS Y LONGITUDES 

        'For i = 0 To ListaOrdenada.Count - 1
        '    Dim Alto = 0.05
        '    Dim Ubicacion1 As Double()


        '    For j = 0 To ListaOrdenada(i).Lista_NoBarras.Count - 1
        '        Dim TextoString = ListaOrdenada(i).Lista_NoBarras(j) & " L=" & ListaOrdenada(i).Lista_LongitudBarras(j) & " (" & j + 1 & ")"
        '        Dim Ubicacion = {Xmax + 2 + i * 2, Ymax - j * 0.09, 0}

        '        AddTexto(TextoString, Ubicacion, Alto, "FC_R-80", "FC_TEXT1")
        '        Dim NumeroBloque = j + 1
        '        If NumeroBloque > 33 Then
        '            NumeroBloque = j - 32
        '        End If
        '        Ubicacion(0) = Xmax + 1.9 + i * 2
        '        Ubicacion(1) = Ymax - j * 0.09 + Alto / 2
        '        AddRefuerzo(Ubicacion, NumeroBloque, 1, "FC_REFUERZO 2")
        '    Next

        '    If ListaOrdenada(i).Lista_NoBarras.Count <> 0 Then
        '        Ubicacion1 = {Xmax + 2 + i * 2 + (Len("X#X L=X.XX (XX)") * Alto) / 3, Ymax + 0.12, 0}
        '        AddNombreMuro(Ubicacion1, "Muro " & ListaOrdenada(i).NombreMuro)
        '        'UbicacionAuxiliar = UbicacionAuxiliar + 1
        '    End If
        'Next

        ' COTAS------------------------------------------------------------->
        For i = 0 To ListaOrdenada.Count - 1
            With ListaOrdenada(i)
            Dim TextoAdicional As String = Format(.EspesorReal, "##,0.00")
            Dim PTO1(2) As Double
            Dim PTO2(2) As Double
                If .DireccionMuro = "Vertical" Then

                    'Cotas de Espesores
                    If .MurosVecinosArriba.Count = 0 Then

                        PTO1(0) = .Xmin : PTO1(1) = .Ymax : PTO1(2) = 0
                        PTO2(0) = .Xmax : PTO2(1) = .Ymax : PTO2(2) = 0
                        AddCota(PTO1, PTO2, 0, TextoAdicional, True, 0.2, 0.15)

                    End If


                    If .MurosVecinosAbajo.Count = 0 Then
                        PTO1(0) = .Xmin : PTO1(1) = .Ymin : PTO1(2) = 0
                        PTO2(0) = .Xmax : PTO2(1) = .Ymin : PTO2(2) = 0
                        AddCota(PTO1, PTO2, 0, TextoAdicional, True, 0.2, -0.15)
                    End If




                    'Cotas de Elementos de Borde y Longitud del Elemento
                    If .LEB_Dr < .Longitud And .LEB_Iz < .Longitud And .LEB_Dr <> 0 And .LEB_Iz <> 0 Then
                        If .LEB_Dr <> 0 Then

                            PTO1(0) = .Xmin : PTO1(1) = .Ymax
                            PTO2(0) = .Xmin : PTO2(1) = .Ymax - .LEB_Dr
                            AddCota(PTO1, PTO2, 90, "", False, 0.35, 0.15)


                        End If


                        If .LEB_Iz <> 0 Then
                            PTO1(0) = .Xmin : PTO1(1) = .Ymin
                            PTO2(0) = .Xmin : PTO2(1) = .Ymin + .LEB_Iz
                            AddCota(PTO1, PTO2, 90, "", False, 0.35, 0.15)

                        End If

                        PTO1(0) = .Xmin : PTO1(1) = .Ymin + .LEB_Iz

                        If .MurosVecinosIzquierda.Count = 0 Then
                            PTO1(0) = .Xmin : PTO1(1) = .Ymax - .LEB_Dr : PTO1(2) = 0
                            PTO2(0) = .Xmin : PTO2(1) = .Ymin + .LEB_Iz
                            AddCota(PTO1, PTO2, 90, Format(.Longitud - .LEB_Dr - .LEB_Iz, "##,0.00"), True, 0.35, 0.15)

                        Else


                            Dim ListaDeMurosVecinosIzquierdaOrdenados As New List(Of Muros)

                            ListaDeMurosVecinosIzquierdaOrdenados = .MurosVecinosIzquierda.OrderBy(Function(x) x.Ymin).ToList()

                            PTO2(0) = .Xmin
                            PTO2(1) = ListaDeMurosVecinosIzquierdaOrdenados(0).Ymin

                            Dim LongitudReal As Double = ListaDeMurosVecinosIzquierdaOrdenados(0).YminE - (.YminE + .LEB_Iz)

                            AddCota(PTO1, PTO2, 90, Format(LongitudReal, "##,0.00"), True, 0.35, 0.15)


                            For V = 0 To ListaDeMurosVecinosIzquierdaOrdenados.Count - 1
                                PTO1(1) = ListaDeMurosVecinosIzquierdaOrdenados(V).Ymax
                                Try
                                    PTO2(1) = ListaDeMurosVecinosIzquierdaOrdenados(V + 1).Ymin
                                    LongitudReal = ListaDeMurosVecinosIzquierdaOrdenados(V + 1).YminE - ListaDeMurosVecinosIzquierdaOrdenados(V).YmaxE
                                    AddCota(PTO1, PTO2, 90, Format(LongitudReal, "##,0.00"), True, 0.35, 0.15)

                                Catch
                                    PTO2(1) = .Ymax - .LEB_Dr
                                    LongitudReal = (.YmaxE - .LEB_Dr) - ListaDeMurosVecinosIzquierdaOrdenados(V).YmaxE
                                    AddCota(PTO1, PTO2, 90, Format(LongitudReal, "##,0.00"), True, 0.35, 0.15)
                                    Exit For
                                End Try
                            Next


                        End If


                    ElseIf .LEB_Dr > .Longitud Or .LEB_Iz > .Longitud OrElse .LEB_Dr = 0 And .LEB_Iz = 0 Then
                        PTO1(0) = .Xmin : PTO1(1) = .Ymin

                        If .MurosVecinosIzquierda.Count = 0 Then
                            PTO2(0) = .Xmin : PTO2(1) = .Ymax

                            AddCota(PTO1, PTO2, 90, Format(.Longitud, "##,0.00"), True, 0.35, 0.15)
                        Else
                            Dim ListaDeMurosVecinosIzquierdaOrdenados As New List(Of Muros)

                            ListaDeMurosVecinosIzquierdaOrdenados = .MurosVecinosIzquierda.OrderBy(Function(x) x.Ymin).ToList()

                            PTO2(0) = .Xmin
                            PTO2(1) = ListaDeMurosVecinosIzquierdaOrdenados(0).Ymin

                            Dim LongitudReal As Double = ListaDeMurosVecinosIzquierdaOrdenados(0).YminE - .YminE

                            AddCota(PTO1, PTO2, 90, Format(LongitudReal, "##,0.00"), True, 0.35, 0.15)


                            For V = 0 To ListaDeMurosVecinosIzquierdaOrdenados.Count - 1
                                PTO1(1) = ListaDeMurosVecinosIzquierdaOrdenados(V).Ymax
                                Try
                                    PTO2(1) = ListaDeMurosVecinosIzquierdaOrdenados(V + 1).Ymin
                                    LongitudReal = ListaDeMurosVecinosIzquierdaOrdenados(V + 1).YminE - ListaDeMurosVecinosIzquierdaOrdenados(V).YmaxE
                                    AddCota(PTO1, PTO2, 90, Format(LongitudReal, "##,0.00"), True, 0.35, 0.15)

                                Catch
                                    PTO2(1) = .Ymax
                                    LongitudReal = .YmaxE - ListaDeMurosVecinosIzquierdaOrdenados(V).YmaxE
                                    AddCota(PTO1, PTO2, 90, Format(LongitudReal, "##,0.00"), True, 0.35, 0.15)
                                    Exit For
                                End Try
                            Next


                        End If



                    End If

                Else



                    'Cotas de Espesores


                    If .MurosVecinosIzquierda.Count = 0 Then
                        PTO1(0) = .Xmin : PTO1(1) = .Ymin : PTO1(2) = 0
                        PTO2(0) = .Xmin : PTO2(1) = .Ymax : PTO2(2) = 0
                        AddCota(PTO1, PTO2, 90, TextoAdicional, True, 0.2, 0.15)
                    End If

                    If .MurosVecinosDerecha.Count = 0 Then
                        PTO1(0) = .Xmax : PTO1(1) = .Ymin : PTO1(2) = 0
                        PTO2(0) = .Xmax : PTO2(1) = .Ymax : PTO2(2) = 0
                        AddCota(PTO1, PTO2, 90, TextoAdicional, True, -0.2, 0.15)
                    End If


                    'Cotas de Elementos de Borde y Longitud del Elemento
                    If .LEB_Dr < .Longitud And .LEB_Iz < .Longitud And .LEB_Dr <> 0 And .LEB_Iz <> 0 Then

                        'Cota Derecha

                        PTO1(0) = .Xmax : PTO1(1) = .Ymax : PTO1(2) = 0
                        PTO2(0) = .Xmax - .LEB_Dr : PTO2(1) = .Ymax
                        AddCota(PTO1, PTO2, 0, "", False, 0.2, 0.25)


                        'Cota Izquierda

                        PTO1(0) = .Xmin : PTO1(1) = .Ymax : PTO1(2) = 0
                        PTO2(0) = .Xmin + .LEB_Iz : PTO2(1) = .Ymax
                        AddCota(PTO1, PTO2, 0, "", False, 0.2, 0.25)


                        PTO1(0) = .Xmin + .LEB_Iz
                        PTO1(1) = .Ymax
                        'Cota Medio

                        If .MurosVecinosArriba.Count = 0 Then

                            PTO1(0) = .Xmin + .LEB_Iz : PTO1(1) = .Ymax : PTO1(2) = 0
                            PTO2(0) = .Xmax - .LEB_Dr : PTO2(1) = .Ymax
                            AddCota(PTO1, PTO2, 0, Format(.Longitud - .LEB_Dr - .LEB_Iz, "##,0.00"), True, 0.2, 0.25)

                        Else


                            Dim ListaDeMurosVecinosArribaOrdenados As New List(Of Muros)

                            ListaDeMurosVecinosArribaOrdenados = .MurosVecinosArriba.OrderBy(Function(x) x.Xmin).ToList()

                            PTO2(0) = ListaDeMurosVecinosArribaOrdenados(0).Xmin

                            Dim LongitudReal As Double = ListaDeMurosVecinosArribaOrdenados(0).XminE - (.XminE + .LEB_Iz)

                            AddCota(PTO1, PTO2, 0, Format(LongitudReal, "##,0.00"), True, 0.2, 0.25)


                            For H = 0 To ListaDeMurosVecinosArribaOrdenados.Count - 1
                                PTO1(0) = ListaDeMurosVecinosArribaOrdenados(H).Xmax
                                Try
                                    PTO2(0) = ListaDeMurosVecinosArribaOrdenados(H + 1).Xmin
                                    LongitudReal = ListaDeMurosVecinosArribaOrdenados(H + 1).XminE - ListaDeMurosVecinosArribaOrdenados(H).XmaxE
                                    AddCota(PTO1, PTO2, 0, Format(LongitudReal, "##,0.00"), True, 0.2, 0.25)
                                Catch
                                    PTO2(0) = .Xmax - .LEB_Dr
                                    LongitudReal = (.XmaxE - .LEB_Dr) - ListaDeMurosVecinosArribaOrdenados(H).XmaxE
                                    AddCota(PTO1, PTO2, 0, Format(LongitudReal, "##,0.00"), True, 0.2, 0.25)
                                    Exit For
                                End Try
                            Next


                        End If


                    ElseIf .LEB_Dr > .Longitud Or .LEB_Iz > .Longitud OrElse .LEB_Dr = 0 And .LEB_Iz = 0 Then



                        PTO1(0) = .Xmin : PTO1(1) = .Ymax : PTO1(2) = 0
                        PTO2(0) = .Xmax : PTO2(1) = .Ymax

                        If .MurosVecinosArriba.Count = 0 Then
                            AddCota(PTO1, PTO2, 0, Format(.Longitud, "##,0.00"), True, 0.2, 0.25)
                        Else

                            Dim ListaDeMurosVecinosArribaOrdenados As New List(Of Muros)

                            ListaDeMurosVecinosArribaOrdenados = .MurosVecinosArriba.OrderBy(Function(x) x.Xmin).ToList()

                            PTO2(0) = ListaDeMurosVecinosArribaOrdenados(0).Xmin

                            Dim LongitudReal As Double = ListaDeMurosVecinosArribaOrdenados(0).XminE - (.XminE + .LEB_Iz)

                            AddCota(PTO1, PTO2, 0, Format(LongitudReal, "##,0.00"), True, 0.2, 0.25)


                            For H = 0 To ListaDeMurosVecinosArribaOrdenados.Count - 1
                                PTO1(0) = ListaDeMurosVecinosArribaOrdenados(H).Xmax
                                Try
                                    PTO2(0) = ListaDeMurosVecinosArribaOrdenados(H + 1).Xmin
                                    LongitudReal = ListaDeMurosVecinosArribaOrdenados(H + 1).XminE - ListaDeMurosVecinosArribaOrdenados(H).XmaxE
                                    AddCota(PTO1, PTO2, 0, Format(LongitudReal, "##,0.00"), True, 0.2, 0.25)
                                Catch
                                    PTO2(0) = .Xmax
                                    LongitudReal = (.XmaxE) - ListaDeMurosVecinosArribaOrdenados(H).XmaxE
                                    AddCota(PTO1, PTO2, 0, Format(LongitudReal, "##,0.00"), True, 0.2, 0.25)
                                    Exit For
                                End Try
                            Next


                        End If



                    End If

                    End If

            End With
        Next





        'MALLA
        For i = 0 To ListaOrdenada.Count - 1
            With ListaOrdenada(i)
                Dim Coord1(2) As Double
                Dim NoMallas As Integer = 0
                Dim NumeroFilas As Integer = 1 : Dim NumeroColumnas As Integer = 1
                If .Malla <> "Sin Malla" Or .Malla <> "" Then
                    For letras = 0 To Len(.Malla) - 1 : If .Malla.Chars(letras) = "D" Then : NoMallas = NoMallas + 1 : End If : Next
                    If .DireccionMuro = "Vertical" Then
                        'Caso 1 

                        If .LEB_Dr = 0 And .LEB_Iz = 0 Then
                            Dim FilasSinRedon = (Math.Abs(.Ymax - .Ymin) - (2 * 0.04)) / 0.1
                            NumeroFilas = Math.Ceiling(FilasSinRedon)
                            If NoMallas = 2 Then
                                Coord1(0) = .Xmin + 0.04 : Coord1(1) = .Ymax - 0.04
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)
                                Coord1(0) = .Xmax - 0.04 : Coord1(1) = .Ymax - 0.04
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)

                            ElseIf NoMallas = 1 Then
                                Coord1(0) = .Xmin + (.Xmax - .Xmin) / 2 : Coord1(1) = .Ymax - 0.04
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)
                            End If

                        End If

                        If .LEB_Dr <> 0 And .LEB_Iz <> 0 Then
                            Dim FilasSinRedon = (Math.Abs(.Ymax - .Ymin) - (.LEB_Dr + .LEB_Iz) - (2 * 0.04)) / 0.1
                            NumeroFilas = Math.Ceiling(FilasSinRedon)
                            If NoMallas = 2 Then
                                Coord1(0) = .Xmin + 0.04 : Coord1(1) = .Ymax - .LEB_Dr - 0.04
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)
                                Coord1(0) = .Xmax - 0.04 : Coord1(1) = .Ymax - .LEB_Dr - 0.04
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)

                            ElseIf NoMallas = 1 Then
                                Coord1(0) = .Xmin + (.Xmax - .Xmin) / 2 : Coord1(1) = .Ymax - .LEB_Dr - 0.04
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)
                            End If

                        End If

                    Else
                        If .LEB_Dr = 0 And .LEB_Iz = 0 Then
                            Dim ColumnasRedon = (Math.Abs(.Xmax - .Xmin) - (2 * 0.04)) / 0.1
                            NumeroColumnas = Math.Ceiling(ColumnasRedon)
                            If NoMallas = 2 Then
                                Coord1(0) = .Xmin + 0.04 : Coord1(1) = .Ymax - 0.04
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)
                                Coord1(0) = .Xmin + 0.04 : Coord1(1) = .Ymin + 0.04
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)

                            ElseIf NoMallas = 1 Then
                                Coord1(0) = .Xmin + 0.04 : Coord1(1) = .Ymax - (.Ymax - .Ymin) / 2
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)
                            End If
                        End If

                        If .LEB_Dr <> 0 And .LEB_Iz <> 0 Then
                            Dim ColumnasRedon = (Math.Abs(.Xmax - .Xmin) - (.LEB_Dr + .LEB_Iz) - (2 * 0.04)) / 0.1
                            NumeroColumnas = Math.Ceiling(ColumnasRedon)
                            If NoMallas = 2 Then
                                Coord1(0) = .Xmin + .LEB_Iz + 0.04 : Coord1(1) = .Ymax - 0.04
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)
                                Coord1(0) = .Xmin + .LEB_Iz + 0.04 : Coord1(1) = .Ymin + 0.04
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)

                            ElseIf NoMallas = 1 Then
                                Coord1(0) = .Xmin + .LEB_Iz + 0.04 : Coord1(1) = .Ymax - (.Ymax - .Ymin) / 2
                                AddMalla(Coord1, NumeroFilas, -0.1, 0.1, NumeroColumnas)
                            End If

                        End If

                    End If

                    MALLAOREFUERZOHORIZONTAL("FC_MALLA", ListaOrdenada(i), NoMallas, .Xmin + 0.04, 0.04, 0.04, .Ymin + 0.04)
                End If
            End With

        Next

        'Refuerzo Horizontal
        For Each Muro In ListaOrdenada
            If Muro.RefuerzoHorizontalLabel <> "" Then
                MALLAOREFUERZOHORIZONTAL("FC_REFUERZO HORIZONTAL 2", Muro, Muro.Capas_RefuerzoHorizontal, Muro.RecubrimientoRefuerzo, (Muro.RecubrimientoRefuerzo - Muro.Xmin), (Muro.RecubrimientoRefuerzo - Muro.Ymin), Muro.RecubrimientoRefuerzo)
            End If
        Next

        Dim Estribos As New Crear_Estribos
        Estribos.Determinar_Estribos(Formulario)

        Dim Aux As New Estribos_Totales
        Dim Delta_X, Delta_Y As Double

        Delta_X = Xmax + 4.6
        Delta_Y = A(1) + 1

        Aux.Estribos_Pisos(Delta_X, 0, A(1))

        Muros_V.Clear()
        ListaOrdenada.Clear()
        Selecccionar.Clear()
        Lista_CirculoRefuerzos.Clear()
        Lista_TextosRefuerzos.Clear()


    End Sub



    Sub AddBloqueNombresMuros(ByVal MuroI As Muros)



        With MuroI


            If .DireccionMuro = "Vertical" Then

                Dim Coord(2) As Double
                Coord(0) = .Xmax + 0.1577
                Coord(1) = .Ymax - (.Ymax - .Ymin) * 0.12

                BloqueNombreMuro = AcadDoc.ModelSpace.InsertBlock(Coord, "FC_B_Leader nombre de muro en seccion", 25, 25, 25, 0)
                BloqueNombreMuro.Layer = "FC_COTAS"
                Dim Propiedades_Dinamicas As Object = BloqueNombreMuro.GetDynamicBlockProperties
                Dim AtibutosBloque As Object = BloqueNombreMuro.GetAttributes()


                Dim Editar_Propiedades2 As AcadDynamicBlockReferenceProperty
                Dim Editar_Atributos As Object = AtibutosBloque(1)

                Editar_Propiedades2 = Propiedades_Dinamicas(4) ''''Visibility1
                Editar_Propiedades2.Value = "Vertical"

                Editar_Propiedades2 = Propiedades_Dinamicas(5) ''''Distance1
                Editar_Propiedades2.Value = 0.1577

                Editar_Propiedades2 = Propiedades_Dinamicas(7) ''''FlipState1     
                Editar_Propiedades2.Value = Convert.ToInt16(0)

                Editar_Propiedades2 = Propiedades_Dinamicas(12) ''''Distance3     
                Editar_Propiedades2.Value = 0.4018

                Editar_Atributos.TextString = "Muro " & .NombreMuro


                '//////(0)---->Arriba-Abajo    //////(1)---->Leader   /////////(3) --------->  Derecha-Izquierda    ////(8)------>Distance2   ////(10) -----Distance4 /////(12)---- Distance3   

            Else


                Dim Coord(2) As Double
                Coord(0) = .Xmin + (.Xmax - .Xmin) * 0.2
                Coord(1) = .Ymin - 0.2249

                BloqueNombreMuro = AcadDoc.ModelSpace.InsertBlock(Coord, "FC_B_Leader nombre de muro en seccion", 25, 25, 25, 0)
                BloqueNombreMuro.Layer = "FC_COTAS"
                Dim Propiedades_Dinamicas As Object = BloqueNombreMuro.GetDynamicBlockProperties
                Dim AtibutosBloque As Object = BloqueNombreMuro.GetAttributes()


                Dim Editar_Propiedades2 As AcadDynamicBlockReferenceProperty
                Dim Editar_Atributos As Object = AtibutosBloque(0)

                Editar_Propiedades2 = Propiedades_Dinamicas(0) ''''Arriba-Abajo
                Editar_Propiedades2.Value = Convert.ToInt16(1)


                Editar_Propiedades2 = Propiedades_Dinamicas(4) ''''Visibility1
                Editar_Propiedades2.Value = "Horizontal"

                Editar_Propiedades2 = Propiedades_Dinamicas(5) ''''Distance1
                Editar_Propiedades2.Value = 0.1577

                Editar_Propiedades2 = Propiedades_Dinamicas(3) ''''Derecha-Izquierda      
                Editar_Propiedades2.Value = Convert.ToInt16(0)

                Editar_Propiedades2 = Propiedades_Dinamicas(10) ''''Distance4     
                Editar_Propiedades2.Value = 0.4774

                Editar_Atributos.TextString = "Muro " & .NombreMuro







            End If


        End With

        BloqueNombreMuro.Update()

    End Sub








    Sub ESCALA()
        For i = 0 To ListaOrdenada.Count - 1
            With ListaOrdenada(i)

                If .DireccionMuro = "Horizontal" Then
                    Dim Delta = (.EspesorEscalado - .EspesorReal) / 2
                    .Ymin = .Ymin - Delta
                    .Ymax = .Ymax + Delta


                    For j = 0 To .MurosVecinosIzquierda.Count - 1

                        .MurosVecinosIzquierda(j).Ymax = .MurosVecinosIzquierda(j).Ymax + Delta
                        .MurosVecinosIzquierda(j).Ymin = .MurosVecinosIzquierda(j).Ymin - Delta
                        If .MurosVecinosIzquierda(j).MurosVecinosArriba.Count > 0 Then
                            .MurosVecinosIzquierda(j).MurosVecinosArriba(0).Ymax = .MurosVecinosIzquierda(j).MurosVecinosArriba(0).Ymax + Delta
                            .MurosVecinosIzquierda(j).MurosVecinosArriba(0).Ymin = .MurosVecinosIzquierda(j).MurosVecinosArriba(0).Ymin + Delta
                            DesplazaminetoaArriba(.MurosVecinosIzquierda(j), .MurosVecinosIzquierda(j).MurosVecinosArriba(0), Delta)

                        End If
                        If .MurosVecinosIzquierda(j).MurosVecinosAbajo.Count > 0 Then
                            .MurosVecinosIzquierda(j).MurosVecinosAbajo(0).Ymax = .MurosVecinosIzquierda(j).MurosVecinosAbajo(0).Ymax - Delta
                            .MurosVecinosIzquierda(j).MurosVecinosAbajo(0).Ymin = .MurosVecinosIzquierda(j).MurosVecinosAbajo(0).Ymin - Delta
                            DesplazaminetoaAbajo(.MurosVecinosIzquierda(j), .MurosVecinosIzquierda(j).MurosVecinosAbajo(0), Delta)
                        End If


                        For s = 0 To .MurosVecinosIzquierda(j).MurosVecinosIzquierda.Count - 1
                            If Math.Round(.CentroideY, 2) < Math.Round(.MurosVecinosIzquierda(j).MurosVecinosIzquierda(s).CentroideY, 2) Then
                                .MurosVecinosIzquierda(j).MurosVecinosIzquierda(s).Ymax = .MurosVecinosIzquierda(j).MurosVecinosIzquierda(s).Ymax + Delta
                                .MurosVecinosIzquierda(j).MurosVecinosIzquierda(s).Ymin = .MurosVecinosIzquierda(j).MurosVecinosIzquierda(s).Ymin + Delta
                                DesplazaminetoaArriba(.MurosVecinosIzquierda(j), .MurosVecinosIzquierda(j).MurosVecinosIzquierda(s), Delta)
                            ElseIf Math.Round(.CentroideY, 2) > Math.Round(.MurosVecinosIzquierda(j).MurosVecinosIzquierda(s).CentroideY, 2) Then
                                .MurosVecinosIzquierda(j).MurosVecinosIzquierda(s).Ymax = .MurosVecinosIzquierda(j).MurosVecinosIzquierda(s).Ymax - Delta
                                .MurosVecinosIzquierda(j).MurosVecinosIzquierda(s).Ymin = .MurosVecinosIzquierda(j).MurosVecinosIzquierda(s).Ymin - Delta
                                DesplazaminetoaAbajo(.MurosVecinosIzquierda(j), .MurosVecinosIzquierda(j).MurosVecinosIzquierda(s), Delta)
                            End If
                        Next


                        For s = 0 To .MurosVecinosIzquierda(j).MurosVecinosDerecha.Count - 1
                            If Math.Round(.CentroideY, 2) < Math.Round(.MurosVecinosIzquierda(j).MurosVecinosDerecha(s).CentroideY, 2) Then
                                .MurosVecinosIzquierda(j).MurosVecinosDerecha(s).Ymax = .MurosVecinosIzquierda(j).MurosVecinosDerecha(s).Ymax + Delta
                                .MurosVecinosIzquierda(j).MurosVecinosDerecha(s).Ymin = .MurosVecinosIzquierda(j).MurosVecinosDerecha(s).Ymin + Delta
                                DesplazaminetoaArriba(.MurosVecinosIzquierda(j), .MurosVecinosIzquierda(j).MurosVecinosDerecha(s), Delta)
                            ElseIf Math.Round(.CentroideY, 2) > Math.Round(.MurosVecinosIzquierda(j).MurosVecinosDerecha(s).CentroideY, 2) Then
                                .MurosVecinosIzquierda(j).MurosVecinosDerecha(s).Ymax = .MurosVecinosIzquierda(j).MurosVecinosDerecha(s).Ymax - Delta
                                .MurosVecinosIzquierda(j).MurosVecinosDerecha(s).Ymin = .MurosVecinosIzquierda(j).MurosVecinosDerecha(s).Ymin - Delta
                                DesplazaminetoaAbajo(.MurosVecinosIzquierda(j), .MurosVecinosIzquierda(j).MurosVecinosDerecha(s), Delta)
                            End If
                        Next
                    Next

                    For j = 0 To .MurosVecinosAbajo.Count - 1

                        .MurosVecinosAbajo(j).Ymax = .MurosVecinosAbajo(j).Ymax - Delta
                        .MurosVecinosAbajo(j).Ymin = .MurosVecinosAbajo(j).Ymin - Delta
                        DesplazaminetoaAbajo(ListaOrdenada(i), .MurosVecinosAbajo(j), Delta)
                    Next

                    For j = 0 To .MurosVecinosArriba.Count - 1

                        .MurosVecinosArriba(j).Ymax = .MurosVecinosArriba(j).Ymax + Delta
                        .MurosVecinosArriba(j).Ymin = .MurosVecinosArriba(j).Ymin + Delta
                        DesplazaminetoaArriba(ListaOrdenada(i), .MurosVecinosArriba(j), Delta)
                    Next

                    For j = 0 To .MurosVecinosDerecha.Count - 1
                        Dim ActivarExpansion As Boolean = True

                        For s = 0 To .MurosVecinosDerecha(j).MurosVecinosDerecha.Count - 1
                            If Math.Round(.MurosVecinosDerecha(j).MurosVecinosDerecha(s).CentroideY, 2) = Math.Round(.CentroideY, 2) Then
                                ActivarExpansion = False
                            End If
                        Next

                        If ActivarExpansion Then

                            .MurosVecinosDerecha(j).Ymax = .MurosVecinosDerecha(j).Ymax + Delta
                            .MurosVecinosDerecha(j).Ymin = .MurosVecinosDerecha(j).Ymin - Delta

                            If .MurosVecinosDerecha(j).MurosVecinosArriba.Count > 0 Then
                                .MurosVecinosDerecha(j).MurosVecinosArriba(0).Ymax = .MurosVecinosDerecha(j).MurosVecinosArriba(0).Ymax + Delta
                                .MurosVecinosDerecha(j).MurosVecinosArriba(0).Ymin = .MurosVecinosDerecha(j).MurosVecinosArriba(0).Ymin + Delta
                                DesplazaminetoaArriba(.MurosVecinosDerecha(j), .MurosVecinosDerecha(j).MurosVecinosArriba(0), Delta)

                            End If
                            If .MurosVecinosDerecha(j).MurosVecinosAbajo.Count > 0 Then
                                .MurosVecinosDerecha(j).MurosVecinosAbajo(0).Ymax = .MurosVecinosDerecha(j).MurosVecinosAbajo(0).Ymax - Delta
                                .MurosVecinosDerecha(j).MurosVecinosAbajo(0).Ymin = .MurosVecinosDerecha(j).MurosVecinosAbajo(0).Ymin - Delta
                                DesplazaminetoaAbajo(.MurosVecinosDerecha(j), .MurosVecinosDerecha(j).MurosVecinosAbajo(0), Delta)
                            End If



                            For s = 0 To .MurosVecinosDerecha(j).MurosVecinosDerecha.Count - 1
                                If Math.Round(.CentroideY, 2) < Math.Round(.MurosVecinosDerecha(j).MurosVecinosDerecha(s).CentroideY, 2) Then
                                    .MurosVecinosDerecha(j).MurosVecinosDerecha(s).Ymax = .MurosVecinosDerecha(j).MurosVecinosDerecha(s).Ymax + Delta
                                    .MurosVecinosDerecha(j).MurosVecinosDerecha(s).Ymin = .MurosVecinosDerecha(j).MurosVecinosDerecha(s).Ymin + Delta
                                    DesplazaminetoaArriba(.MurosVecinosDerecha(j), .MurosVecinosDerecha(j).MurosVecinosDerecha(s), Delta)
                                ElseIf Math.Round(.CentroideY, 2) > Math.Round(.MurosVecinosDerecha(j).MurosVecinosDerecha(s).CentroideY, 2) Then
                                    .MurosVecinosDerecha(j).MurosVecinosDerecha(s).Ymax = .MurosVecinosDerecha(j).MurosVecinosDerecha(s).Ymax - Delta
                                    .MurosVecinosDerecha(j).MurosVecinosDerecha(s).Ymin = .MurosVecinosDerecha(j).MurosVecinosDerecha(s).Ymin - Delta
                                    DesplazaminetoaAbajo(.MurosVecinosDerecha(j), .MurosVecinosDerecha(j).MurosVecinosDerecha(s), Delta)
                                End If
                            Next


                            For s = 0 To .MurosVecinosDerecha(j).MurosVecinosIzquierda.Count - 1
                                If Math.Round(.CentroideY, 2) < Math.Round(.MurosVecinosDerecha(j).MurosVecinosIzquierda(s).CentroideY, 2) Then
                                    .MurosVecinosDerecha(j).MurosVecinosIzquierda(s).Ymax = .MurosVecinosDerecha(j).MurosVecinosIzquierda(s).Ymax + Delta
                                    .MurosVecinosDerecha(j).MurosVecinosIzquierda(s).Ymin = .MurosVecinosDerecha(j).MurosVecinosIzquierda(s).Ymin + Delta
                                    '  Try
                                    '  DesplazaminetoaArriba(.MurosVecinosIzquierda(j), .MurosVecinosDerecha(j).MurosVecinosIzquierda(s), Delta)
                                    ' Catch ex As Exception
                                    DesplazaminetoaArriba(.MurosVecinosDerecha(j), .MurosVecinosDerecha(j).MurosVecinosIzquierda(s), Delta)

                                    '  End Try
                                ElseIf Math.Round(.CentroideY, 2) > Math.Round(.MurosVecinosDerecha(j).MurosVecinosIzquierda(s).CentroideY, 2) Then
                                    .MurosVecinosDerecha(j).MurosVecinosIzquierda(s).Ymax = .MurosVecinosDerecha(j).MurosVecinosIzquierda(s).Ymax - Delta
                                    .MurosVecinosDerecha(j).MurosVecinosIzquierda(s).Ymin = .MurosVecinosDerecha(j).MurosVecinosIzquierda(s).Ymin - Delta
                                    DesplazaminetoaAbajo(.MurosVecinosDerecha(j), .MurosVecinosDerecha(j).MurosVecinosIzquierda(s), Delta)
                                End If
                            Next
                        End If
                    Next
                End If
            End With
        Next

        '''''''------------------------VERTICALES-------------------------------.


        For i = 0 To ListaOrdenada.Count - 1
            With ListaOrdenada(i)

                If .DireccionMuro = "Vertical" Then
                    Dim Delta = (.EspesorEscalado - .EspesorReal) / 2
                    .Xmin = .Xmin - Delta
                    .Xmax = .Xmax + Delta


                    For j = 0 To .MurosVecinosAbajo.Count - 1

                        .MurosVecinosAbajo(j).Xmax = .MurosVecinosAbajo(j).Xmax + Delta
                        .MurosVecinosAbajo(j).Xmin = .MurosVecinosAbajo(j).Xmin - Delta

                        If .MurosVecinosAbajo(j).MurosVecinosDerecha.Count > 0 Then
                            .MurosVecinosAbajo(j).MurosVecinosDerecha(0).Xmax = .MurosVecinosAbajo(j).MurosVecinosDerecha(0).Xmax + Delta
                            .MurosVecinosAbajo(j).MurosVecinosDerecha(0).Xmin = .MurosVecinosAbajo(j).MurosVecinosDerecha(0).Xmin + Delta
                            DesplazaminetoaDerecha(.MurosVecinosAbajo(j), .MurosVecinosAbajo(j).MurosVecinosDerecha(0), Delta)

                        End If
                        If .MurosVecinosAbajo(j).MurosVecinosIzquierda.Count > 0 Then
                            .MurosVecinosAbajo(j).MurosVecinosIzquierda(0).Xmax = .MurosVecinosAbajo(j).MurosVecinosIzquierda(0).Xmax - Delta
                            .MurosVecinosAbajo(j).MurosVecinosIzquierda(0).Xmin = .MurosVecinosAbajo(j).MurosVecinosIzquierda(0).Xmin - Delta
                            DesplazaminetoaIzquierda(.MurosVecinosAbajo(j), .MurosVecinosAbajo(j).MurosVecinosIzquierda(0), Delta)
                        End If


                        For s = 0 To .MurosVecinosAbajo(j).MurosVecinosAbajo.Count - 1
                            If Math.Round(.CentroideX, 2) < Math.Round(.MurosVecinosAbajo(j).MurosVecinosAbajo(s).CentroideX, 2) Then
                                .MurosVecinosAbajo(j).MurosVecinosAbajo(s).Xmax = .MurosVecinosAbajo(j).MurosVecinosAbajo(s).Xmax + Delta
                                .MurosVecinosAbajo(j).MurosVecinosAbajo(s).Xmin = .MurosVecinosAbajo(j).MurosVecinosAbajo(s).Xmin + Delta
                                DesplazaminetoaDerecha(.MurosVecinosAbajo(j), .MurosVecinosAbajo(j).MurosVecinosAbajo(s), Delta)
                            ElseIf Math.Round(.CentroideX, 2) > Math.Round(.MurosVecinosAbajo(j).MurosVecinosAbajo(s).CentroideX, 2) Then
                                .MurosVecinosAbajo(j).MurosVecinosAbajo(s).Xmax = .MurosVecinosAbajo(j).MurosVecinosAbajo(s).Xmax - Delta
                                .MurosVecinosAbajo(j).MurosVecinosAbajo(s).Xmin = .MurosVecinosAbajo(j).MurosVecinosAbajo(s).Xmin - Delta
                                DesplazaminetoaIzquierda(.MurosVecinosAbajo(j), .MurosVecinosAbajo(j).MurosVecinosAbajo(s), Delta)
                            End If
                        Next

                        For s = 0 To .MurosVecinosAbajo(j).MurosVecinosArriba.Count - 1
                            If Math.Round(.CentroideX, 2) < Math.Round(.MurosVecinosAbajo(j).MurosVecinosArriba(s).CentroideX, 2) Then
                                .MurosVecinosAbajo(j).MurosVecinosArriba(s).Xmax = .MurosVecinosAbajo(j).MurosVecinosArriba(s).Xmax + Delta
                                .MurosVecinosAbajo(j).MurosVecinosArriba(s).Xmin = .MurosVecinosAbajo(j).MurosVecinosArriba(s).Xmin + Delta
                                DesplazaminetoaDerecha(.MurosVecinosAbajo(j), .MurosVecinosAbajo(j).MurosVecinosArriba(s), Delta)
                            ElseIf Math.Round(.CentroideX, 2) > Math.Round(.MurosVecinosAbajo(j).MurosVecinosArriba(s).CentroideX, 2) Then
                                .MurosVecinosAbajo(j).MurosVecinosArriba(s).Xmax = .MurosVecinosAbajo(j).MurosVecinosArriba(s).Xmax - Delta
                                .MurosVecinosAbajo(j).MurosVecinosArriba(s).Xmin = .MurosVecinosAbajo(j).MurosVecinosArriba(s).Xmin - Delta
                                DesplazaminetoaIzquierda(.MurosVecinosAbajo(j), .MurosVecinosAbajo(j).MurosVecinosArriba(s), Delta)
                            End If
                        Next
                        'Cambio 1-----OK!
                    Next

                    For j = 0 To .MurosVecinosIzquierda.Count - 1

                        .MurosVecinosIzquierda(j).Xmax = .MurosVecinosIzquierda(j).Xmax - Delta
                        .MurosVecinosIzquierda(j).Xmin = .MurosVecinosIzquierda(j).Xmin - Delta
                        DesplazaminetoaIzquierda(ListaOrdenada(i), .MurosVecinosIzquierda(j), Delta)
                    Next

                    For j = 0 To .MurosVecinosDerecha.Count - 1

                        .MurosVecinosDerecha(j).Xmax = .MurosVecinosDerecha(j).Xmax + Delta
                        .MurosVecinosDerecha(j).Xmin = .MurosVecinosDerecha(j).Xmin + Delta
                        DesplazaminetoaDerecha(ListaOrdenada(i), .MurosVecinosDerecha(j), Delta)
                    Next

                    'CAMBIO 2 ----- OK!

                    For j = 0 To .MurosVecinosArriba.Count - 1
                        Dim ActivarExpansion As Boolean = True

                        For s = 0 To .MurosVecinosArriba(j).MurosVecinosArriba.Count - 1
                            If Math.Round(.MurosVecinosArriba(j).MurosVecinosArriba(s).CentroideX, 2) = Math.Round(.CentroideX, 2) Then
                                ActivarExpansion = False
                            End If
                        Next

                        If ActivarExpansion Then

                            .MurosVecinosArriba(j).Xmax = .MurosVecinosArriba(j).Xmax + Delta
                            .MurosVecinosArriba(j).Xmin = .MurosVecinosArriba(j).Xmin - Delta

                            If .MurosVecinosArriba(j).MurosVecinosDerecha.Count > 0 Then
                                .MurosVecinosArriba(j).MurosVecinosDerecha(0).Xmax = .MurosVecinosArriba(j).MurosVecinosDerecha(0).Xmax + Delta
                                .MurosVecinosArriba(j).MurosVecinosDerecha(0).Xmin = .MurosVecinosArriba(j).MurosVecinosDerecha(0).Xmin + Delta
                                DesplazaminetoaDerecha(.MurosVecinosArriba(j), .MurosVecinosArriba(j).MurosVecinosDerecha(0), Delta)

                            End If

                            'CAMBIO 3 --- OK!


                            If .MurosVecinosArriba(j).MurosVecinosIzquierda.Count > 0 Then
                                .MurosVecinosArriba(j).MurosVecinosIzquierda(0).Xmax = .MurosVecinosArriba(j).MurosVecinosIzquierda(0).Xmax - Delta
                                .MurosVecinosArriba(j).MurosVecinosIzquierda(0).Xmin = .MurosVecinosArriba(j).MurosVecinosIzquierda(0).Xmin - Delta
                                DesplazaminetoaIzquierda(.MurosVecinosArriba(j), .MurosVecinosArriba(j).MurosVecinosIzquierda(0), Delta)
                            End If

                            'CAMBIO 4----- OK!

                            For s = 0 To .MurosVecinosArriba(j).MurosVecinosArriba.Count - 1
                                If Math.Round(.CentroideX, 2) < Math.Round(.MurosVecinosArriba(j).MurosVecinosArriba(s).CentroideX, 2) Then
                                    .MurosVecinosArriba(j).MurosVecinosArriba(s).Xmax = .MurosVecinosArriba(j).MurosVecinosArriba(s).Xmax + Delta
                                    .MurosVecinosArriba(j).MurosVecinosArriba(s).Xmin = .MurosVecinosArriba(j).MurosVecinosArriba(s).Xmin + Delta
                                    DesplazaminetoaDerecha(.MurosVecinosArriba(j), .MurosVecinosArriba(j).MurosVecinosArriba(s), Delta)
                                ElseIf Math.Round(.CentroideX, 2) > Math.Round(.MurosVecinosArriba(j).MurosVecinosArriba(s).CentroideX, 2) Then
                                    .MurosVecinosArriba(j).MurosVecinosArriba(s).Xmax = .MurosVecinosArriba(j).MurosVecinosArriba(s).Xmax - Delta
                                    .MurosVecinosArriba(j).MurosVecinosArriba(s).Xmin = .MurosVecinosArriba(j).MurosVecinosArriba(s).Xmin - Delta
                                    DesplazaminetoaIzquierda(.MurosVecinosArriba(j), .MurosVecinosArriba(j).MurosVecinosArriba(s), Delta)
                                End If
                            Next

                            'CAMBIO 5----- OK!

                            For s = 0 To .MurosVecinosArriba(j).MurosVecinosAbajo.Count - 1
                                If Math.Round(.CentroideX, 2) < Math.Round(.MurosVecinosArriba(j).MurosVecinosAbajo(s).CentroideX, 2) Then
                                    .MurosVecinosArriba(j).MurosVecinosAbajo(s).Xmax = .MurosVecinosArriba(j).MurosVecinosAbajo(s).Xmax + Delta
                                    .MurosVecinosArriba(j).MurosVecinosAbajo(s).Xmin = .MurosVecinosArriba(j).MurosVecinosAbajo(s).Xmin + Delta
                                    DesplazaminetoaDerecha(.MurosVecinosArriba(j), .MurosVecinosArriba(j).MurosVecinosAbajo(s), Delta)
                                ElseIf Math.Round(.CentroideX, 2) > Math.Round(.MurosVecinosArriba(j).MurosVecinosAbajo(s).CentroideX, 2) Then
                                    .MurosVecinosArriba(j).MurosVecinosAbajo(s).Xmax = .MurosVecinosArriba(j).MurosVecinosAbajo(s).Xmax - Delta
                                    .MurosVecinosArriba(j).MurosVecinosAbajo(s).Xmin = .MurosVecinosArriba(j).MurosVecinosAbajo(s).Xmin - Delta
                                    DesplazaminetoaIzquierda(.MurosVecinosArriba(j), .MurosVecinosArriba(j).MurosVecinosAbajo(s), Delta)
                                End If
                            Next

                            'CAMBIO 6----- OK!
                        End If


                    Next

                End If

            End With
        Next
    End Sub

    Sub DesplazaminetoaAbajo(ByVal MuroMadre As Muros, ByVal MuroHijo As Muros, ByVal Delta As Double)

        For i = 0 To MuroHijo.MurosVecinosClase.Count - 1
            If MuroMadre.NombreMuro <> MuroHijo.MurosVecinosClase(i).NombreMuro Then
                If MuroMadre.Ymax <> MuroHijo.MurosVecinosClase(i).Ymax And MuroMadre.Ymin <> MuroHijo.MurosVecinosClase(i).Ymin Then
                    MuroHijo.MurosVecinosClase(i).Ymax = MuroHijo.MurosVecinosClase(i).Ymax - Delta
                    MuroHijo.MurosVecinosClase(i).Ymin = MuroHijo.MurosVecinosClase(i).Ymin - Delta
                    DesplazaminetoaAbajo(MuroHijo, MuroHijo.MurosVecinosClase(i), Delta)
                End If
            End If
        Next

    End Sub

    Sub DesplazaminetoaArriba(ByVal MuroMadre As Muros, ByVal MuroHijo As Muros, ByVal Delta As Double)

        For i = 0 To MuroHijo.MurosVecinosClase.Count - 1
            If MuroMadre.NombreMuro <> MuroHijo.MurosVecinosClase(i).NombreMuro Then
                If MuroMadre.Ymax <> MuroHijo.MurosVecinosClase(i).Ymax And MuroMadre.Ymin <> MuroHijo.MurosVecinosClase(i).Ymin Then
                    MuroHijo.MurosVecinosClase(i).Ymax = MuroHijo.MurosVecinosClase(i).Ymax + Delta
                    MuroHijo.MurosVecinosClase(i).Ymin = MuroHijo.MurosVecinosClase(i).Ymin + Delta
                    DesplazaminetoaArriba(MuroHijo, MuroHijo.MurosVecinosClase(i), Delta)
                End If
            End If
        Next

    End Sub


    Sub DesplazaminetoaIzquierda(ByVal MuroMadre As Muros, ByVal MuroHijo As Muros, ByVal Delta As Double)

        For i = 0 To MuroHijo.MurosVecinosClase.Count - 1
            If MuroMadre.NombreMuro <> MuroHijo.MurosVecinosClase(i).NombreMuro Then
                If MuroMadre.Xmax <> MuroHijo.MurosVecinosClase(i).Xmax And MuroMadre.Xmin <> MuroHijo.MurosVecinosClase(i).Xmin Then
                    MuroHijo.MurosVecinosClase(i).Xmax = MuroHijo.MurosVecinosClase(i).Xmax - Delta
                    MuroHijo.MurosVecinosClase(i).Xmin = MuroHijo.MurosVecinosClase(i).Xmin - Delta
                    DesplazaminetoaIzquierda(MuroHijo, MuroHijo.MurosVecinosClase(i), Delta)
                End If
            End If
        Next

    End Sub



    Sub DesplazaminetoaDerecha(ByVal MuroMadre As Muros, ByVal MuroHijo As Muros, ByVal Delta As Double)

        For i = 0 To MuroHijo.MurosVecinosClase.Count - 1
            If MuroMadre.NombreMuro <> MuroHijo.MurosVecinosClase(i).NombreMuro Then
                If MuroMadre.Xmax <> MuroHijo.MurosVecinosClase(i).Xmax And MuroMadre.Xmin <> MuroHijo.MurosVecinosClase(i).Xmin Then
                    MuroHijo.MurosVecinosClase(i).Xmax = MuroHijo.MurosVecinosClase(i).Xmax + Delta
                    MuroHijo.MurosVecinosClase(i).Xmin = MuroHijo.MurosVecinosClase(i).Xmin + Delta
                    DesplazaminetoaDerecha(MuroHijo, MuroHijo.MurosVecinosClase(i), Delta)
                End If
            End If
        Next

    End Sub

    Private pattername As String
    Private outerLoop(0 To 0) As AcadEntity

    Private Sub Add_Hatch(ByVal Acad_Ent As AcadEntity, ByVal Pattern As String, ByVal Layer As String, ByVal Escala As Double)

        pattername = Pattern
        Hacth = AcadDoc.ModelSpace.AddHatch(0, pattername, True)
        outerLoop(0) = Acad_Ent
        With Hacth
            .AppendOuterLoop(outerLoop)
            .Layer = Layer
            .LinetypeScale = Escala
            .PatternAngle = 45
            .PatternScale = 0.009
            .PatternSpace = 0.009
        End With
    End Sub

    Private Sub Add_LW_PL(ByVal coord As List(Of Double), ByVal Layer As String)
        Polyline2 = AcadDoc.ModelSpace.AddLightWeightPolyline(coord.ToArray)

        With Polyline2
            .Closed = True
            .Layer = Layer
        End With
        Polyline2.Update()

    End Sub



    Sub CrearTabla(ByVal Coord() As Double, ByVal NombresMuros As List(Of String))

        Dim No_Rows As Integer = NombresMuros.Count

        TablaAutocad = AcadDoc.ModelSpace.AddTable(Coord, No_Rows + 2, 2, 1, 1)
        TablaAutocad.Width = 29
        ' TablaAutocad.Height = 20.2739

        TablaAutocad.StyleName = "FC_TABLA"
        'TablaAutocad.MinimumTableHeight = (0.5 + 0.1786 + 0.15 * NombresMuros.Count)
        TablaAutocad.VertCellMargin = 0.15
        TablaAutocad.HorzCellMargin = 0.15

        TablaAutocad.SetCellValue(0, 0, "CONVECIÓN DE COLOCACIÓN DE REFUERZO")
        TablaAutocad.SetCellTextStyle(0, 0, "FC_TEXT1")
        TablaAutocad.SetCellTextHeight(0, 0, 1.604)
        TablaAutocad.SetRowHeight(0, 11.7427)
        TablaAutocad.SetCellValue(1, 0, "CONVECIÓN") : TablaAutocad.SetCellValue(1, 1, "MURO") : TablaAutocad.SetCellTextStyle(1, 0, "FC_TEXT1")
        TablaAutocad.SetCellTextStyle(1, 1, "FC_TEXT1") : TablaAutocad.SetCellTextHeight(1, 0, 1.3366) : TablaAutocad.SetCellTextHeight(1, 1, 1.3366)
        TablaAutocad.SetRowHeight(1, 5.5015)
        TablaAutocad.Layer = "FC_BORDES"

        For i = 0 To NombresMuros.Count - 1

            TablaAutocad.SetCellValue(2 + i, 1, "Muro " & NombresMuros(i))
            TablaAutocad.SetCellTextStyle(2 + i, 1, "FC_TEXT1")
            TablaAutocad.SetCellTextHeight(2 + i, 1, 1.0693)
            TablaAutocad.SetRowHeight(2 + i, 3.0297)
            TablaAutocad.SetCellAlignment(2 + i, 1, AcCellAlignment.acMiddleCenter)

        Next


        TablaAutocad.ScaleEntity(Coord, 0.04)
        For i = 0 To NombresMuros.Count - 1
            Dim Coord2 = {Coord(0) + 0.315, Coord(1) - 0.75 - i * 0.12, 0}
            AddRefuerzo(Coord2, i + 1, 1, "FC_REFUERZO 2")

        Next

        TablaAutocad.Update()



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

        If Objeto.ObjectName = "AcDbText" Then
            Dim Texto As AcadText = Objeto
            If Texto.Layer = "FC_R-80" Then
                NombreObjecto = Texto.TextString
            End If
        End If

        If Objeto.ObjectName = "AcDbCircle" Then
            Dim Circle As AcadCircle = Objeto
            If Circle.Layer = "FC_REFUERZO 2" Then
                NombreObjecto = "C" & Numero
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
            Longitud= EspesorX
            Direccion="Horizontal"
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

        If Objeto.ObjectName = "AcDbText" Then
            Dim Texto As AcadText = Objeto
            If Texto.Layer = "FC_R-80" Then
                Texto.GetBoundingBox(MinPunto, MaxPunto)
                MinPunto(0) = MinPunto(0) + (Len(Texto.TextString) * Texto.Height) / 2
                MinPunto(1) = MinPunto(1) + Texto.Height / 2
            End If
        End If

        If Objeto.ObjectName = "AcDbCircle" Then
            Dim Circle As AcadCircle = Objeto
            If Circle.Layer = "FC_REFUERZO 2" Then

                Circle.GetBoundingBox(MinPunto, MaxPunto)
                MinPunto(0) = MinPunto(0) + Circle.Radius
                MinPunto(1) = MinPunto(1) + Circle.Radius

            End If
        End If



        PropiedadesdelBloque2.Add(Radio)
        PropiedadesdelBloque2.Add(MinPunto)

        PropiedadesdelBloque = PropiedadesdelBloque2

    End Function

    'Translacion de Coordenadas
    Function Traslacion_Coordenas(ByVal X As Double, ByVal Y As Double, Xmin As Double, Xmax As Double, Ymin As Double, Ymax As Double) As List(Of Double)


        Dim O As Matrix(Of Double)
        Dim P As Matrix(Of Double)
        Dim T_aux As Matrix(Of Double)
        Dim Traslacion As New List(Of Double)

        P = Matrix(Of Double).Build.DenseOfArray({{1, 0, X}, {0, 1, Y}, {0, 0, 1}})
        O = Matrix(Of Double).Build.DenseOfArray({{Xmin, Xmax, Xmax, Xmin}, {Ymin, Ymin, Ymax, Ymax}, {1, 1, 1, 1}})

        T_aux = P * O


        For j = 0 To T_aux.ColumnCount - 1
            Traslacion.Add(T_aux(0, j))
            Traslacion.Add(T_aux(1, j))

        Next


        Traslacion_Coordenas = Traslacion


    End Function


    Sub AddRefuerzo(ByVal Coord() As Double, ByVal NumeroBloque As String, ByVal Xscale As Double, ByVal Layer As String)

        Dim Bloque As String = "FC_B_Convenciones refuerzoV2"


        Refuerzo_Circulo = AcadDoc.ModelSpace.InsertBlock(Coord, Bloque, 1, 1, 1, 0)

        Dim Propiedades_Dinamicas As Object = Refuerzo_Circulo.GetDynamicBlockProperties
        Dim Editar_Propiedades2 As AcadDynamicBlockReferenceProperty
        Editar_Propiedades2 = Propiedades_Dinamicas(0)
        Editar_Propiedades2.Value = NumeroBloque

        With Refuerzo_Circulo
            .Layer = Layer
            .XScaleFactor = Xscale

        End With
        Refuerzo_Circulo.Update()



    End Sub



    Sub AddCota(ByVal PTO1() As Double, ByVal PTO2() As Double, ByVal Rotation As Double, ByVal TextoAdicional As String, ByVal EditarTexto As Boolean, ByVal DesplazX As Double, ByVal DesplazY As Double)

        Dim Location() As Double

        If Rotation = 90 Then
            Rotation = Math.PI / 2
            Location = {PTO1(0) - DesplazX, ((PTO2(1) - PTO1(1)) / 2) + PTO1(1), 0}
        Else
            Rotation = 0
            Location = {((PTO2(0) - PTO1(0)) / 2) + PTO1(0), PTO1(1) + DesplazY, 0}
        End If


        Cota = AcadDoc.ModelSpace.AddDimRotated(PTO1, PTO2, Location, Rotation)

        With Cota

            .Layer = "FC_COTAS"
            .StyleName = "FC_COTAS"
            .TextHeight = 0.0015
            ' .TextPosition = Location
            .Arrowhead1Type = AcDimArrowheadType.acArrowDot
            .Arrowhead2Type = AcDimArrowheadType.acArrowDot
            .ArrowheadSize = 0.001


            If EditarTexto Then
                .TextOverride = TextoAdicional

            End If


        End With

        Cota.Update()
    End Sub



    Sub AddTexto(ByVal TextoString As String, Ubicacion() As Double, ByVal Alto As Double, ByVal LayerText As String, ByVal StyleText As String)


        Texto = AcadDoc.ModelSpace.AddText(TextoString, Ubicacion, Alto)

        With Texto
            .Layer = LayerText
            .StyleName = StyleText

        End With
        Texto.Update()

    End Sub

    Sub AddNombreMuro(ByVal Coord() As Double, ByVal LabelNa As String)

        Dim Bloque As String = "FC_B_Nombre de muro en seccion"


        NombreMuro = AcadDoc.ModelSpace.InsertBlock(Coord, Bloque, 1, 1, 1, 0)

        Dim Propiedades_Dinamicas As Object = NombreMuro.GetDynamicBlockProperties
        Dim Editar_Propiedades2 As AcadDynamicBlockReferenceProperty
        Editar_Propiedades2 = Propiedades_Dinamicas(1)

        NombreMuro.GetAttributes()(0).textstring = LabelNa

        With NombreMuro
            .Layer = "FC_R-80"
            .XScaleFactor = 25

        End With
        NombreMuro.Update()
    End Sub



    Sub AddRefuerzoHorizo_LineaMalla(ByVal Coord() As Double, ByVal NombreBloque As String, ByVal Rotacion As Double, ByVal Layer As String, ByVal LongituddeMalla As Double, ByVal DireccGancho1 As Integer, ByVal DireccGancho2 As Integer, ByVal ScaleX As Double)


        Rotacion = (Rotacion * Math.PI) / 180

        RefuerzoHorizontal = AcadDoc.ModelSpace.InsertBlock(Coord, NombreBloque, 1, 1, 1, Rotacion)
        Dim Propiedades_Dinamicas As Object = RefuerzoHorizontal.GetDynamicBlockProperties
        Dim Editar_Propiedades2 As AcadDynamicBlockReferenceProperty

        If NombreBloque = "FC_B_Malla-refuerzo_2" Then
            Editar_Propiedades2 = Propiedades_Dinamicas(0)
            Editar_Propiedades2.Value = LongituddeMalla
        ElseIf NombreBloque = "FC_B_Malla-refuerzo_3" Then

            Editar_Propiedades2 = Propiedades_Dinamicas(0)
            Editar_Propiedades2.Value = Convert.ToInt16(DireccGancho1)

            Editar_Propiedades2 = Propiedades_Dinamicas(1)
            Editar_Propiedades2.Value = LongituddeMalla

        ElseIf NombreBloque = "FC_B_Malla-refuerzo_1" Then
            Editar_Propiedades2 = Propiedades_Dinamicas(0)
            Editar_Propiedades2.Value = LongituddeMalla



        Else


            Editar_Propiedades2 = Propiedades_Dinamicas(2)
            Editar_Propiedades2.Value = LongituddeMalla
            'Direccion Gancho 1
            Editar_Propiedades2 = Propiedades_Dinamicas(0)
            Editar_Propiedades2.Value = Convert.ToInt16(DireccGancho1)
            'Direccion Gancho 2
            Editar_Propiedades2 = Propiedades_Dinamicas(1)
            Editar_Propiedades2.Value = Convert.ToInt16(DireccGancho2)

        End If




        With RefuerzoHorizontal
            .Layer = Layer


        End With
        RefuerzoHorizontal.Update()

    End Sub

    Sub AddRefuerzoHorizo_LineaMalla_ElementosDeBordeMayores045(ByVal Coord0() As Double, ByVal Coord1() As Double, ByVal Layer As String)

        Linea = AcadDoc.ModelSpace.AddLine(Coord0, Coord1)

        With Linea
            .Layer = Layer

        End With



    End Sub







    Sub AddMalla(ByVal CoordCircle1() As Double, ByVal NumeroFilas As Integer, ByVal DistanciaFilas As Double, ByVal DistanciaColumas As Double, ByVal NumeroColumnas As Integer)


        AddRefuerzo(CoordCircle1, 1, 0.7, "FC_HATCH 251")
        Refuerzo_Circulo.ArrayRectangular(NumeroFilas, NumeroColumnas, 1, DistanciaFilas, DistanciaColumas, 0.1)
        Refuerzo_Circulo.color = 251

        Refuerzo_Circulo.Update()


    End Sub

    Public Sub MALLAOREFUERZOHORIZONTAL(ByVal Layer As String, ByVal MuroaAnalizar As Muros, ByVal Capas As Integer, ByVal UbicacionRefuerzoMV As Double, ByVal UbicacionRefuerzoMV2 As Double, ByVal UbicacionRefuerzoMH As Double, ByVal UbicacionRefuerzoMH0 As Double)


        With MuroaAnalizar
            Dim FactorAd
            Dim Coord(2) As Double
            Dim LongitudRefuerzo As Double
            Dim Coord0(2) As Double
            Dim Coord1(2) As Double
            If Layer = "FC_MALLA" Then
                FactorAd = 0.01
            Else
                Dim NoMallas As Integer
                For letras = 0 To Len(.Malla) - 1 : If .Malla.Chars(letras) = "D" Then : NoMallas = NoMallas + 1 : End If : Next
                If NoMallas = 1 Then : FactorAd = -0.03 : Else : FactorAd = 0 : End If

            End If

            If Capas <> 0 Then


                If .DireccionMuro = "Horizontal" Then
                    '----------------------MUROS HORIZONTALES ---------------------------------
                    Dim MurosaExtenderMalla As Integer = 0
                    Dim XaExtender1 As Double = 0 : Dim XaExtender2 As Double = 0 : Dim R1 As Double = 0 : Dim R2 As Double = 0
                    Dim MuroL1 As String = "" : Dim MuroL2 As String = "" : Dim DireccGancho1 As Integer = 0 : Dim DireccGancho2 As Integer = 0

                    For j = 0 To .MurosVecinosP.Count - 1
                        If Math.Round(ListaOrdenada(.MurosVecinosP(j)).XmaxE, 2) = Math.Round(.XminE, 2) Or Math.Round(ListaOrdenada(.MurosVecinosP(j)).XminE, 2) = Math.Round(.XminE, 2) Then
                            MurosaExtenderMalla = MurosaExtenderMalla + 1
                            XaExtender1 = ListaOrdenada(.MurosVecinosP(j)).Xmin
                            R1 = ListaOrdenada(.MurosVecinosP(j)).RecubrimientoRefuerzo - ListaOrdenada(.MurosVecinosP(j)).Xmin
                            If Math.Round(ListaOrdenada(.MurosVecinosP(j)).YmaxE, 2) = Math.Round(.YmaxE, 2) Or Math.Round(ListaOrdenada(.MurosVecinosP(j)).YmaxE, 2) = Math.Round(.YminE, 2) Then
                                MuroL1 = "MuroLArriba"
                            ElseIf Math.Round(ListaOrdenada(.MurosVecinosP(j)).YminE, 2) = Math.Round(.YmaxE, 2) Or Math.Round(ListaOrdenada(.MurosVecinosP(j)).YminE, 2) = Math.Round(.YminE, 2) Then
                                MuroL1 = "MuroLAbajo"
                            End If

                        End If


                        If Math.Round(.XmaxE, 2) = Math.Round(ListaOrdenada(.MurosVecinosP(j)).XminE, 2) Or Math.Round(.XmaxE, 2) = Math.Round(ListaOrdenada(.MurosVecinosP(j)).XmaxE, 2) Then
                            MurosaExtenderMalla = MurosaExtenderMalla + 1
                            XaExtender2 = ListaOrdenada(.MurosVecinosP(j)).Xmax
                            R2 = ListaOrdenada(.MurosVecinosP(j)).RecubrimientoRefuerzo - ListaOrdenada(.MurosVecinosP(j)).Xmin
                            If Math.Round(ListaOrdenada(.MurosVecinosP(j)).YmaxE, 2) = Math.Round(.YmaxE, 2) Or Math.Round(ListaOrdenada(.MurosVecinosP(j)).YmaxE, 2) = Math.Round(.YminE, 2) Then
                                MuroL2 = "MuroLArriba"
                            ElseIf Math.Round(ListaOrdenada(.MurosVecinosP(j)).YminE, 2) = Math.Round(.YmaxE, 2) Or Math.Round(ListaOrdenada(.MurosVecinosP(j)).YminE, 2) = Math.Round(.YminE, 2) Then
                                MuroL2 = "MuroLAbajo"
                            End If
                        End If
                    Next


                    'CASO MAYOR 1: CASO 1,2,3 Cuando Los Elementos de Borde son menores a 0.45

                    'CASO MAYOR 1


                    If .LEB_Dr < 0.45 And .LEB_Iz < 0.45 Then
                        'Caso1
                        If MurosaExtenderMalla = 2 Then 'Para dos Capas, con extension en sus dos vecinos

                            If Capas = 2 Then
                                Coord(0) = XaExtender2 - R2 - 0.42 - 0.01
                                Coord(1) = (.Ymax - UbicacionRefuerzoMH) + 0.01
                                LongitudRefuerzo = (Coord(0) + 0.42) - (XaExtender1 + R1 + 0.01)

                                'Refuerzo Arriba
                                DireccGancho1 = 0 : DireccGancho2 = 0


                                If MuroL1 = "MuroLArriba" Then : DireccGancho2 = 1 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho2 = 0 : End If
                                If MuroL2 = "MuroLArriba" Then : DireccGancho1 = 1 : End If
                                If MuroL2 = "MuroLAbajo" Then : DireccGancho1 = 0 : End If


                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_4", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Abajo

                                DireccGancho1 = 1 : DireccGancho2 = 1

                                If MuroL1 = "MuroLArriba" Then : DireccGancho2 = 1 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho2 = 0 : End If
                                If MuroL2 = "MuroLArriba" Then : DireccGancho1 = 1 : End If
                                If MuroL2 = "MuroLAbajo" Then : DireccGancho1 = 0 : End If
                                Coord(0) = XaExtender2 - R2 - 0.42 - 0.01
                                Coord(1) = UbicacionRefuerzoMH0 - 0.01
                                LongitudRefuerzo = (Coord(0) + 0.42) - (XaExtender1 + R1 + 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_4", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            ElseIf Capas = 1 Then
                                DireccGancho1 = 1 : DireccGancho2 = 1

                                If MuroL1 = "MuroLArriba" Then : DireccGancho2 = 1 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho2 = 0 : End If
                                If MuroL2 = "MuroLArriba" Then : DireccGancho1 = 1 : End If
                                If MuroL2 = "MuroLAbajo" Then : DireccGancho1 = 0 : End If
                                Coord(0) = XaExtender2 - R2 - 0.42 - 0.01
                                Coord(1) = .Ymin + (.Ymax - .Ymin) / 2 + FactorAd
                                LongitudRefuerzo = (Coord(0) + 0.42) - (XaExtender1 + R1 + 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_4", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            End If



                        End If




                        'Caso2.1 ----> LadoDerecho 



                        If MurosaExtenderMalla = 1 And XaExtender1 <> 0 Then 'Para dos Capas, con extension en un vecino izquierdo

                            If Capas = 2 Then
                                'Refuerzo Arriba
                                Coord(0) = XaExtender1 + R1 - 0.01 + 0.42
                                Coord(1) = (.Ymax - UbicacionRefuerzoMH) + 0.01
                                LongitudRefuerzo = (.Xmax - (.RecubrimientoRefuerzo - .Ymin) / 2 + 0.01) - (Coord(0) - 0.42)

                                DireccGancho1 = 1 : DireccGancho2 = 1



                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 1 : End If



                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 90, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Abajo

                                DireccGancho1 = 0 : DireccGancho2 = 0

                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 1 : End If

                                Coord(0) = XaExtender1 + R1 - 0.01 + 0.42
                                Coord(1) = UbicacionRefuerzoMH0 - 0.01
                                LongitudRefuerzo = (.Xmax - (.RecubrimientoRefuerzo - .Ymin) / 2 + 0.01) - (Coord(0) - 0.42)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 90, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)


                            ElseIf Capas = 1 Then

                                DireccGancho1 = 0 : DireccGancho2 = 0

                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 1 : End If

                                Coord(0) = XaExtender1 + R1 - 0.01 + 0.42
                                Coord(1) = .Ymin + (.Ymax - .Ymin) / 2 + FactorAd
                                LongitudRefuerzo = (.Xmax - (.RecubrimientoRefuerzo - .Ymin) / 2 + 0.01) - (Coord(0) - 0.42)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 90, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                            End If



                        End If

                        'Caso2.2 ----> LadoIzquierdo

                        If MurosaExtenderMalla = 1 And XaExtender2 <> 0 Then 'Para dos Capas, con extension en un vecino derecho

                            If Capas = 2 Then
                                'Refuerzo Arriba
                                Coord(0) = XaExtender2 - R2 - 0.01 - 0.42
                                Coord(1) = (.Ymax - UbicacionRefuerzoMH) + 0.01
                                LongitudRefuerzo = (Coord(0) + 0.42) - (.Xmin + (.RecubrimientoRefuerzo - .Ymin) / 2 - 0.01)

                                DireccGancho1 = 0 : DireccGancho2 = 0


                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 0 : End If


                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Abajo

                                DireccGancho1 = 1 : DireccGancho2 = 1

                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 0 : End If

                                Coord(0) = XaExtender2 - R2 - 0.01 - 0.42
                                Coord(1) = UbicacionRefuerzoMH0 - 0.01
                                LongitudRefuerzo = (Coord(0) + 0.42) - (.Xmin + (.RecubrimientoRefuerzo - .Ymin) / 2 - 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                            ElseIf Capas = 1 Then

                                DireccGancho1 = 1 : DireccGancho2 = 1

                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 0 : End If

                                Coord(0) = XaExtender2 - R2 - 0.01 - 0.42
                                Coord(1) = .Ymin + (.Ymax - .Ymin) / 2 + FactorAd
                                LongitudRefuerzo = (Coord(0) + 0.42) - (.Xmin + (.RecubrimientoRefuerzo - .Ymin) / 2 - 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            End If




                        End If

                        'Caso 3 -----> Sin Vecinos


                        If MurosaExtenderMalla = 0 Then

                            If Capas = 2 Then
                                'Refuerzo Arriba

                                Coord(0) = .Xmax - (.RecubrimientoRefuerzo - .Ymin) / 2 + 0.01 - 0.02
                                Coord(1) = (.Ymax - UbicacionRefuerzoMH) + 0.01
                                LongitudRefuerzo = Coord(0) + 0.02 - (.Xmin + (.RecubrimientoRefuerzo - .Ymin) / 2 - 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_2", 90, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Abajo

                                Coord(0) = .Xmin + (.RecubrimientoRefuerzo - .Ymin) / 2 - 0.01 + 0.02
                                Coord(1) = UbicacionRefuerzoMH0 - 0.01
                                LongitudRefuerzo = ((.Xmax - (.RecubrimientoRefuerzo - .Ymin) / 2) + 0.01) - (Coord(0) - 0.02)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_2", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            ElseIf Capas = 1 Then
                                Coord(0) = .Xmin + (.RecubrimientoRefuerzo - .Ymin) / 2 - 0.01 + 0.02
                                Coord(1) = .Ymin + (.Ymax - .Ymin) / 2 + FactorAd
                                LongitudRefuerzo = ((.Xmax - (.RecubrimientoRefuerzo - .Ymin) / 2) + 0.01) - (Coord(0) - 0.02)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_2", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)


                            End If
                        End If


                    End If



                    'CASO MAYOR 2
                    'CASO MAYOR 2: CASO 1,2,3 Cuando Elemento de Borde Derecho es Mayor a 0.45 y Elemento De Borde Izquierdo Menor 0.45 pero mayor a cero


                    If .LEB_Dr >= 0.45 And .LEB_Iz < 0.45 And .LEB_Iz <> 0 Then


                        'Caso 1 ---> Muros Vecinos
                        If MurosaExtenderMalla = 2 OrElse XaExtender1 <> 0 Then
                            If Capas = 2 Then

                                'Refuerzo Arriba
                                Coord(0) = .Xmax - 0.15
                                Coord(1) = .Ymax - UbicacionRefuerzoMH + 0.01
                                DireccGancho1 = 1

                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 1 : End If

                                LongitudRefuerzo = (Coord(0)) - (XaExtender1 + R1 + 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 90, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)


                                'Refuerzo Abajo
                                Coord(0) = .Xmax - 0.15
                                Coord(1) = UbicacionRefuerzoMH0 - 0.01
                                DireccGancho1 = 0


                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 1 : End If

                                LongitudRefuerzo = (Coord(0)) - (XaExtender1 + R1 + 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 90, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            ElseIf Capas = 1 Then
                                Coord(0) = .Xmax - 0.15
                                Coord(1) = .Ymin + (.Ymax - .Ymin) / 2 + FactorAd
                                DireccGancho1 = 0

                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 1 : End If

                                LongitudRefuerzo = (Coord(0)) - (XaExtender1 + R1 + 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 90, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)


                            End If
                        End If


                        'Caso 2

                        If MurosaExtenderMalla = 1 And XaExtender2 <> 0 OrElse MurosaExtenderMalla = 0 Then
                            If Capas = 2 Then
                                'Refuerzo Arriba
                                Coord(0) = .Xmax - 0.15 : Coord(1) = .Ymax - UbicacionRefuerzoMH + 0.01
                                LongitudRefuerzo = Coord(0) - (.Xmin + (.RecubrimientoRefuerzo - .Ymin) / 2 - 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 90, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Abajo
                                Coord(0) = .Xmax - 0.15 : Coord(1) = UbicacionRefuerzoMH0 - 0.01

                                LongitudRefuerzo = Coord(0) - (.Xmin + (.RecubrimientoRefuerzo - .Ymin) / 2 - 0.01)

                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 90, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                RefuerzoHorizontal2 = RefuerzoHorizontal.Mirror(Coord, {Coord(0) + LongitudRefuerzo, Coord(1), 0})

                                RefuerzoHorizontal.Delete()
                            ElseIf Capas = 1 Then
                                Coord(0) = .Xmax - 0.15 : Coord(1) = .Ymin + (.Ymax - .Ymin) / 2 + FactorAd

                                LongitudRefuerzo = Coord(0) - (.Xmin + (.RecubrimientoRefuerzo - .Ymin) / 2 - 0.01)

                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 90, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                RefuerzoHorizontal2 = RefuerzoHorizontal.Mirror(Coord, {Coord(0) + LongitudRefuerzo, Coord(1), 0})
                                RefuerzoHorizontal.Delete()

                            End If
                        End If




                    End If

                    'CASO MAYOR 3
                    'CASO MAYOR 3: CASO 1,2,3 Cuando Elemento de Borde Izquierdo es Mayor a 0.45 y Elemento De Borde Derecho Menor 0.45 pero mayor a cero


                    If .LEB_Dr < 0.45 And .LEB_Iz >= 0.45 And .LEB_Dr <> 0 Then


                        'Caso 1
                        If MurosaExtenderMalla = 2 OrElse MurosaExtenderMalla = 1 And XaExtender2 <> 0 Then
                            If Capas = 2 Then
                                'Refuerzo Arriba
                                Coord(0) = .Xmin + 0.15
                                Coord(1) = .Ymax - UbicacionRefuerzoMH + 0.01
                                DireccGancho1 = 0

                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 0 : End If

                                LongitudRefuerzo = (XaExtender2 - R2 + 0.01) - (Coord(0))
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Abajo
                                Coord(0) = .Xmin + 0.15
                                Coord(1) = UbicacionRefuerzoMH0 - 0.01
                                DireccGancho1 = 1


                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 0 : End If

                                LongitudRefuerzo = ((XaExtender2 - R2 + 0.01) - Coord(0))
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                            ElseIf Capas = 1 Then
                                Coord(0) = .Xmin + 0.15
                                Coord(1) = .Ymin + (.Ymax - .Ymin) / 2 + FactorAd
                                DireccGancho1 = 1

                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 0 : End If

                                LongitudRefuerzo = ((XaExtender2 - R2 + 0.01) - Coord(0))
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            End If

                        End If

                        'Caso 2

                        If MurosaExtenderMalla = 1 And XaExtender1 <> 0 OrElse MurosaExtenderMalla = 0 Then
                            If Capas = 2 Then
                                'Refuerzo Abajo
                                Coord(0) = .Xmin + 0.15 : Coord(1) = UbicacionRefuerzoMH0 - 0.01

                                LongitudRefuerzo = (.Xmax - (.RecubrimientoRefuerzo - .Ymin) / 2 + 0.01) - Coord(0)

                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)


                                'Refuerzo Arriba
                                Coord(0) = .Xmin + 0.15 : Coord(1) = .Ymax - UbicacionRefuerzoMH + 0.01
                                LongitudRefuerzo = (.Xmax - (.RecubrimientoRefuerzo - .Ymin) / 2 + 0.01) - Coord(0)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                RefuerzoHorizontal2 = RefuerzoHorizontal.Mirror(Coord, {Coord(0) + LongitudRefuerzo, Coord(1), 0})
                                RefuerzoHorizontal.Delete()
                            ElseIf Capas = 1 Then
                                Coord(0) = .Xmin + 0.15 : Coord(1) = .Ymin + (.Ymax - .Ymin) / 2 + FactorAd
                                LongitudRefuerzo = (.Xmax - (.RecubrimientoRefuerzo - .Ymin) / 2 + 0.01) - Coord(0)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 270, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                RefuerzoHorizontal2 = RefuerzoHorizontal.Mirror(Coord, {Coord(0) + LongitudRefuerzo, Coord(1), 0})
                                RefuerzoHorizontal.Delete()

                            End If
                        End If


                    End If



                    'CASO MAYOR 4
                    'CASO MAYOR 4: CASO 1,2,3 Cuando Los Elementos de Borde son mayores a 0.45

                    If .LEB_Dr >= 0.45 And .LEB_Iz >= 0.45 OrElse .LEB_Dr >= .Longitud And .LEB_Iz = 0 OrElse .LEB_Iz >= .Longitud And .LEB_Dr = 0 Then


                        If Capas = 2 Then

                            'Refuerzo Arriba
                            Coord0(0) = .Xmin + 0.15 : Coord0(1) = .Ymax - UbicacionRefuerzoMH + 0.01
                            Coord1(0) = .Xmax - 0.15 : Coord1(1) = .Ymax - UbicacionRefuerzoMH + 0.01
                            AddRefuerzoHorizo_LineaMalla_ElementosDeBordeMayores045(Coord0, Coord1, Layer)

                            'Refuerzo Abajo
                            Coord0(0) = .Xmin + 0.15 : Coord0(1) = UbicacionRefuerzoMH0 - 0.01
                            Coord1(0) = .Xmax - 0.15 : Coord1(1) = UbicacionRefuerzoMH0 - 0.01
                            AddRefuerzoHorizo_LineaMalla_ElementosDeBordeMayores045(Coord0, Coord1, Layer)

                        ElseIf Capas = 2 Then
                            Coord0(0) = .Xmin + 0.15 : Coord0(1) = .Ymin + (.Ymax - .Ymin) / 2 + FactorAd
                            Coord1(0) = .Xmax - 0.15 : Coord1(1) = .Ymin + (.Ymax - .Ymin) / 2 + FactorAd
                            AddRefuerzoHorizo_LineaMalla_ElementosDeBordeMayores045(Coord0, Coord1, Layer)

                        End If


                    End If





                Else


                    '----------------------MUROS VERTICALES ---------------------------------




                    Dim MurosaExtenderMalla As Integer = 0
                    Dim YaExtender1 As Double = 0 : Dim YaExtender2 As Double = 0 : Dim R1 As Double = 0 : Dim R2 As Double = 0
                    Dim MuroL1 As String = "" : Dim MuroL2 As String = "" : Dim DireccGancho1 As Integer = 0 : Dim DireccGancho2 As Integer = 0

                    For j = 0 To .MurosVecinosPY.Count - 1


                        If Math.Round(ListaOrdenada(.MurosVecinosPY(j)).YmaxE, 2) = Math.Round(.YminE, 2) Or Math.Round(ListaOrdenada(.MurosVecinosPY(j)).YminE, 2) = Math.Round(.YminE, 2) Then
                            MurosaExtenderMalla = MurosaExtenderMalla + 1
                            YaExtender1 = ListaOrdenada(.MurosVecinosPY(j)).Ymin
                            R1 = ListaOrdenada(.MurosVecinosPY(j)).RecubrimientoRefuerzo - ListaOrdenada(.MurosVecinosPY(j)).Ymin
                            If Math.Round(ListaOrdenada(.MurosVecinosPY(j)).XmaxE, 2) = Math.Round(.XmaxE, 2) Or Math.Round(ListaOrdenada(.MurosVecinosPY(j)).XmaxE, 2) = Math.Round(.XminE, 2) Then
                                MuroL1 = "MuroLIzquierda"
                            ElseIf Math.Round(ListaOrdenada(.MurosVecinosPY(j)).XminE, 2) = Math.Round(.XmaxE, 2) Or Math.Round(ListaOrdenada(.MurosVecinosPY(j)).XminE, 2) = Math.Round(.XminE, 2) Then
                                MuroL1 = "MuroLDerecha"
                            End If

                        End If




                        If Math.Round(.YmaxE, 2) = Math.Round(ListaOrdenada(.MurosVecinosPY(j)).YminE, 2) Or Math.Round(.YmaxE, 2) = Math.Round(ListaOrdenada(.MurosVecinosPY(j)).YmaxE, 2) Then
                            MurosaExtenderMalla = MurosaExtenderMalla + 1
                            YaExtender2 = ListaOrdenada(.MurosVecinosPY(j)).Ymax
                            R2 = ListaOrdenada(.MurosVecinosPY(j)).RecubrimientoRefuerzo - ListaOrdenada(.MurosVecinosPY(j)).Ymin
                            If Math.Round(ListaOrdenada(.MurosVecinosPY(j)).XmaxE, 2) = Math.Round(.XmaxE, 2) Or Math.Round(ListaOrdenada(.MurosVecinosPY(j)).XmaxE, 2) = Math.Round(.XminE, 2) Then
                                MuroL2 = "MuroLIzquierda"
                            ElseIf Math.Round(ListaOrdenada(.MurosVecinosPY(j)).XminE, 2) = Math.Round(.XmaxE, 2) Or Math.Round(ListaOrdenada(.MurosVecinosPY(j)).XminE, 2) = Math.Round(.XminE, 2) Then
                                MuroL2 = "MuroLDerecha"
                            End If
                        End If



                    Next




                    'CASO MAYOR 1
                    'CASO MAYOR 1: CASO 1,2,3 Cuando Los Elementos de Borde son menores a 0.45



                    If .LEB_Dr < 0.45 And .LEB_Iz < 0.45 Then
                        'Caso1
                        If MurosaExtenderMalla = 2 Then 'Para dos Capas, con extension en sus dos vecinos
                            If Capas = 2 Then
                                Coord(0) = (.Xmax - UbicacionRefuerzoMV2) + 0.01
                                Coord(1) = YaExtender2 - R2 - 0.42 - 0.01
                                LongitudRefuerzo = (Coord(1) + 0.42) - (YaExtender1 + R1 + 0.01)

                                'Refuerzo Derecha
                                DireccGancho1 = 1 : DireccGancho2 = 1


                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho2 = 0 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho2 = 1 : End If

                                If MuroL2 = "MuroLIzquierda" Then : DireccGancho1 = 0 : End If
                                If MuroL2 = "MuroLDerecha" Then : DireccGancho1 = 1 : End If


                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_4", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Izquierda

                                DireccGancho1 = 0 : DireccGancho2 = 0


                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho2 = 0 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho2 = 1 : End If

                                If MuroL2 = "MuroLIzquierda" Then : DireccGancho1 = 0 : End If
                                If MuroL2 = "MuroLDerecha" Then : DireccGancho1 = 1 : End If


                                Coord(0) = UbicacionRefuerzoMV - 0.01
                                Coord(1) = YaExtender2 - R2 - 0.42 - 0.01
                                LongitudRefuerzo = (Coord(1) + 0.42) - (YaExtender1 + R1 + 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_4", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                            ElseIf Capas = 1 Then
                                'Refuerzo Izquierda

                                DireccGancho1 = 0 : DireccGancho2 = 0

                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho2 = 0 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho2 = 1 : End If

                                If MuroL2 = "MuroLIzquierda" Then : DireccGancho1 = 0 : End If
                                If MuroL2 = "MuroLDerecha" Then : DireccGancho1 = 1 : End If

                                Coord(0) = .Xmin + (.Xmax - .Xmin) / 2 + FactorAd
                                Coord(1) = YaExtender2 - R2 - 0.42 - 0.01
                                LongitudRefuerzo = (Coord(1) + 0.42) - (YaExtender1 + R1 + 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_4", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            End If
                        End If



                        'Caso2.1 ----> LadoAbajo



                        If MurosaExtenderMalla = 1 And YaExtender1 <> 0 Then 'Para dos Capas, con extension en un vecino abajo
                            If Capas = 2 Then
                                'Refuerzo Derecha
                                Coord(0) = (.Xmax - UbicacionRefuerzoMV2) + 0.01
                                Coord(1) = YaExtender1 + R1 - 0.01 + 0.42
                                LongitudRefuerzo = (.Ymax - (.RecubrimientoRefuerzo - .Xmin) / 2 + 0.01) - (Coord(1) - 0.42)

                                DireccGancho1 = 0 : DireccGancho2 = 0

                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho1 = 0 : End If


                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Izquierda

                                DireccGancho1 = 1 : DireccGancho2 = 1

                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho1 = 0 : End If

                                Coord(0) = UbicacionRefuerzoMV - 0.01
                                Coord(1) = YaExtender1 + R1 - 0.01 + 0.42
                                LongitudRefuerzo = (.Ymax - (.RecubrimientoRefuerzo - .Xmin) / 2 + 0.01) - (Coord(1) - 0.42)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                            ElseIf Capas = 1 Then
                                DireccGancho1 = 1 : DireccGancho2 = 1

                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho1 = 0 : End If

                                Coord(0) = .Xmin + (.Xmax - .Xmin) / 2 + FactorAd
                                Coord(1) = YaExtender1 + R1 - 0.01 + 0.42
                                LongitudRefuerzo = (.Ymax - (.RecubrimientoRefuerzo - .Xmin) / 2 + 0.01) - (Coord(1) - 0.42)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                            End If


                        End If

                        'Caso2.2 ----> LadoArriba



                        If MurosaExtenderMalla = 1 And YaExtender2 <> 0 Then 'Para dos Capas, con extension en un vecino arriba
                            If Capas = 2 Then
                                'Refuerzo Derecha
                                Coord(0) = (.Xmax - UbicacionRefuerzoMV2) + 0.01
                                Coord(1) = YaExtender2 - R2 - 0.01 - 0.42
                                LongitudRefuerzo = (Coord(1) + 0.42) - (.Ymin + (.RecubrimientoRefuerzo - .Xmin) / 2 - 0.02)

                                DireccGancho1 = 1 : DireccGancho2 = 1


                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho1 = 1 : End If


                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Izquierdo

                                DireccGancho1 = 0 : DireccGancho2 = 0

                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 1 : End If

                                Coord(0) = UbicacionRefuerzoMV - 0.01
                                Coord(1) = YaExtender2 - R2 - 0.01 - 0.42
                                LongitudRefuerzo = (Coord(1) + 0.42) - (.Ymin + (.RecubrimientoRefuerzo - .Xmin) / 2 - 0.02)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                            ElseIf Capas = 1 Then
                                DireccGancho1 = 0 : DireccGancho2 = 0

                                If MuroL1 = "MuroLArriba" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLAbajo" Then : DireccGancho1 = 1 : End If

                                Coord(0) = .Xmin + (.Xmax - .Xmin) / 2 + FactorAd
                                Coord(1) = YaExtender2 - R2 - 0.01 - 0.42
                                LongitudRefuerzo = (Coord(1) + 0.42) - (.Ymin + (.RecubrimientoRefuerzo - .Xmin) / 2 - 0.02)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_5", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            End If
                        End If



                        'Caso 3 ---> Sin Vecinos
                        If MurosaExtenderMalla = 0 Then

                            If Capas = 2 Then

                                'Refuerzo Derecha

                                Coord(0) = .Xmax - UbicacionRefuerzoMV2 + 0.01
                                Coord(1) = (.Ymin + (.RecubrimientoRefuerzo - .Xmin) / 2 + 0.02) - 0.02
                                LongitudRefuerzo = (.Ymax - (.RecubrimientoRefuerzo - .Xmin) / 2 + 0.01) - (Coord(1) - 0.02)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_2", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Izquierda

                                Coord(0) = UbicacionRefuerzoMV - 0.01
                                Coord(1) = .Ymax - ((.RecubrimientoRefuerzo - .Xmin) / 2 - 0.02) - 0.02
                                LongitudRefuerzo = (Coord(1) + 0.02) - ((.Ymin + (.RecubrimientoRefuerzo - .Xmin) / 2) - 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_2", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                            ElseIf Capas = 1 Then
                                Coord(0) = .Xmin + (.Xmax - .Xmin) / 2 + FactorAd
                                Coord(1) = .Ymax - ((.RecubrimientoRefuerzo - .Xmin) / 2 - 0.02) - 0.02
                                LongitudRefuerzo = (Coord(1) + 0.02) - ((.Ymin + (.RecubrimientoRefuerzo - .Xmin) / 2) - 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_2", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                            End If

                        End If

                    End If




                    'CASO MAYOR 2
                    'CASO MAYOR 2: CASO 1,2,3 Cuando Elemento de Borde Derecho es Mayor a 0.45 y Elemento De Borde Izquierdo Menor 0.45 pero mayor a cero


                    If .LEB_Dr >= 0.45 And .LEB_Iz < 0.45 And .LEB_Iz <> 0 Then


                        'Caso 1 ---> Muros Vecinos
                        If MurosaExtenderMalla = 2 OrElse YaExtender1 <> 0 Then
                            If Capas = 2 Then
                                'Refuerzo Derecha
                                Coord(0) = .Xmax - UbicacionRefuerzoMV2 + 0.01
                                Coord(1) = .Ymax - 0.15
                                DireccGancho1 = 0

                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho1 = 0 : End If

                                LongitudRefuerzo = (Coord(1)) - (YaExtender1 + R1 + 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Izquierda
                                Coord(0) = UbicacionRefuerzoMV - 0.01
                                Coord(1) = .Ymax - 0.15
                                DireccGancho1 = 1


                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho1 = 0 : End If

                                LongitudRefuerzo = (Coord(1)) - (YaExtender1 + R1 + 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                            ElseIf Capas = 1 Then
                                Coord(0) = .Xmin + (.Xmax - .Xmin) / 2 + FactorAd
                                Coord(1) = .Ymax - 0.15
                                DireccGancho1 = 1

                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho1 = 1 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho1 = 0 : End If

                                LongitudRefuerzo = (Coord(1)) - (YaExtender1 + R1 + 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)


                            End If

                        End If


                        'Caso 2

                        If MurosaExtenderMalla = 1 And YaExtender2 <> 0 OrElse MurosaExtenderMalla = 0 Then
                            If Capas = 2 Then
                                'Refuerzo Izquierda
                                Coord(0) = UbicacionRefuerzoMV - 0.01
                                Coord(1) = .Ymax - 0.15
                                LongitudRefuerzo = Coord(1) - (.Ymin + (.RecubrimientoRefuerzo - .Xmin) / 2 - 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Derecha
                                Coord(0) = .Xmax - UbicacionRefuerzoMV2 + 0.01
                                Coord(1) = .Ymax - 0.15

                                LongitudRefuerzo = Coord(1) - (.Ymin + (.RecubrimientoRefuerzo - .Xmin) / 2 - 0.01)

                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                Dim RefuerzoHorizontal2 = RefuerzoHorizontal.Mirror(Coord, {Coord(0), Coord(1) + +LongitudRefuerzo, 0})
                                RefuerzoHorizontal.Delete()
                            ElseIf Capas = 1 Then
                                Coord(0) = .Xmin + (.Xmax - .Xmin) / 2 + FactorAd
                                Coord(1) = .Ymax - 0.15
                                LongitudRefuerzo = Coord(1) - (.Ymin + (.RecubrimientoRefuerzo - .Xmin) / 2 - 0.01)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 180, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            End If
                        End If




                    End If



                    'CASO MAYOR 3
                    'CASO MAYOR 3: CASO 1,2,3 Cuando Elemento de Borde Izquierdo es Mayor a 0.45 y Elemento De Borde Derecho Menor 0.45 pero mayor a cero


                    If .LEB_Dr < 0.45 And .LEB_Iz >= 0.45 And .LEB_Dr <> 0 Then


                        'Caso 1
                        If MurosaExtenderMalla = 2 OrElse MurosaExtenderMalla = 1 And YaExtender2 <> 0 Then
                            If Capas = 2 Then
                                'Refuerzo Derecha
                                Coord(0) = .Xmax - UbicacionRefuerzoMV2 + 0.01
                                Coord(1) = .Ymin + 0.15
                                DireccGancho1 = 1

                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho1 = 1 : End If

                                LongitudRefuerzo = (YaExtender2 - R2 + 0.01) - (Coord(1))
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Izquierda
                                Coord(0) = UbicacionRefuerzoMV - 0.01

                                DireccGancho1 = 0

                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho1 = 1 : End If

                                LongitudRefuerzo = ((YaExtender2 - R2 + 0.01) - Coord(1))
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            ElseIf Capas = 1 Then

                                Coord(0) = .Xmin + (.Xmax - .Xmin) / 2 + FactorAd
                                Coord(1) = .Ymin + 0.15
                                DireccGancho1 = 0
                                If MuroL1 = "MuroLIzquierda" Then : DireccGancho1 = 0 : End If
                                If MuroL1 = "MuroLDerecha" Then : DireccGancho1 = 1 : End If
                                LongitudRefuerzo = ((YaExtender2 - R2 + 0.01) - Coord(1))
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_3", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                            End If
                        End If

                        'Caso 2

                        If MurosaExtenderMalla = 1 And YaExtender1 <> 0 OrElse MurosaExtenderMalla = 0 Then
                            If Capas = 2 Then
                                'Refuerzo Derecha
                                Coord(0) = .Xmax - UbicacionRefuerzoMV2 + 0.01
                                Coord(1) = .Ymin + 0.15

                                LongitudRefuerzo = (.Ymax - (.RecubrimientoRefuerzo - .Xmin) / 2 + 0.01) - Coord(1)

                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                'Refuerzo Izquierda
                                Coord(0) = UbicacionRefuerzoMV - 0.01 : Coord(1) = .Ymin + 0.15
                                LongitudRefuerzo = (.Ymax - (.RecubrimientoRefuerzo - .Xmin) / 2 + 0.01) - Coord(1)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)

                                Dim RefuerzoHorizontal2 = RefuerzoHorizontal.Mirror(Coord, {Coord(0), Coord(1) + +LongitudRefuerzo, 0})
                                RefuerzoHorizontal.Delete()
                            ElseIf Capas = 1 Then
                                Coord(0) = .Xmin + (.Xmax - .Xmin) / 2 + FactorAd
                                Coord(1) = .Ymin + 0.15
                                LongitudRefuerzo = (.Ymax - (.RecubrimientoRefuerzo - .Xmin) / 2 + 0.01) - Coord(1)
                                AddRefuerzoHorizo_LineaMalla(Coord, "FC_B_Malla-refuerzo_1", 0, Layer, LongitudRefuerzo, DireccGancho1, DireccGancho2, 1)
                                Dim RefuerzoHorizontal2 = RefuerzoHorizontal.Mirror(Coord, {Coord(0), Coord(1) + +LongitudRefuerzo, 0})
                                RefuerzoHorizontal.Delete()

                            End If
                        End If


                    End If


                    'CASO MAYOR 4
                    'CASO MAYOR 4: CASO 1,2,3 Cuando Los Elementos de Borde son mayores a 0.45

                    If .LEB_Dr >= 0.45 And .LEB_Iz >= 0.45 OrElse .LEB_Dr >= .Longitud And .LEB_Iz = 0 OrElse .LEB_Iz >= .Longitud And .LEB_Dr = 0 Then


                        If Capas = 2 Then

                            'Refuerzo Derecha
                            Coord0(0) = .Xmax - UbicacionRefuerzoMV2 + 0.01 : Coord0(1) = .Ymin + 0.15
                            Coord1(0) = .Xmax - UbicacionRefuerzoMV2 + 0.01 : Coord1(1) = .Ymax - 0.15
                            AddRefuerzoHorizo_LineaMalla_ElementosDeBordeMayores045(Coord0, Coord1, Layer)

                            'Refuerzo Izquierda
                            Coord0(0) = UbicacionRefuerzoMV - 0.01 : Coord0(1) = .Ymin + 0.15
                            Coord1(0) = UbicacionRefuerzoMV - 0.01 : Coord1(1) = .Ymax - 0.15
                            AddRefuerzoHorizo_LineaMalla_ElementosDeBordeMayores045(Coord0, Coord1, Layer)

                        ElseIf Capas = 1 Then

                            Coord0(0) = .Xmin + (.Xmax - .Xmin) / 2 + FactorAd : Coord0(1) = .Ymin + 0.15
                            Coord1(0) = .Xmin + (.Xmax - .Xmin) / 2 + FactorAd : Coord1(1) = .Ymax - 0.15
                            AddRefuerzoHorizo_LineaMalla_ElementosDeBordeMayores045(Coord0, Coord1, Layer)
                        End If


                    End If


                End If




            End If



        End With



    End Sub





End Module



