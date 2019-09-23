Imports System.IO

Public Class AutoCADEtabs


    Public Separaciones_Malla As New Dictionary(Of String, Single)

    Public Sub New(ByVal ListaMurosAranas As List(Of Arania))


        Separaciones_Malla.Add("Sin Malla", 0)


        Separaciones_Malla.Add("D84", 0.38)
        Separaciones_Malla.Add("DD84", 0.38)


        Separaciones_Malla.Add("D106", 0.3)
        Separaciones_Malla.Add("DD106", 0.3)

        Separaciones_Malla.Add("D158", 0.17)
        Separaciones_Malla.Add("DD158", 0.17)


        Separaciones_Malla.Add("D188", 0.15)
        Separaciones_Malla.Add("DD188", 0.15)

        Separaciones_Malla.Add("D221", 0.14)
        Separaciones_Malla.Add("DD221", 0.14)

        Separaciones_Malla.Add("D257", 0.12)
        Separaciones_Malla.Add("DD257", 0.12)


        Dim ArchivoTexto As New List(Of String)
        Dim OpenFile As OpenFileDialog : OpenFile = New OpenFileDialog
        With OpenFile
            .Title = "Abrir Proyecto de ETABS"
            .Filter = "Proyecto ETABS |*.$et"
            .ShowDialog()
        End With

        If OpenFile.FileName <> "" Then
            Dim LectorArchivo As StreamReader
            Dim LineaTexto As String
            LectorArchivo = New StreamReader(OpenFile.FileName)
            Do
                LineaTexto = LectorArchivo.ReadLine()
                ArchivoTexto.Add(LineaTexto)
            Loop Until LineaTexto Is Nothing

            LectorArchivo.Close()
            If ListaMurosAranas.Count <> 0 Then
                Dim Iniciar As Boolean = True

                For Each Arana As Arania In ListaMurosAranas
                    If Arana.Muros_arania.Count = 0 Then
                        Iniciar = False
                    End If
                Next
                If Iniciar Then
                    Inicio(ArchivoTexto, ListaMurosAranas)
                Else
                    MsgBox("Alguna de las arañas no tiene definido muros.", MsgBoxStyle.Exclamation, "efe Prima Ce")
                End If
            Else
                MsgBox("Agregue por lo menos una Araña.", MsgBoxStyle.Information, "efe Prima Ce")
            End If

        Else

        End If

    End Sub
    Public Sub Inicio(ByVal ArchivoET As List(Of String), ByVal ListaMurosAranas As List(Of Arania))



        'OrganizarBarrasPorPiso
        For i = 0 To ListaMurosAranas.Count - 1

            For j = 0 To ListaMurosAranas(i).Muros_arania.Count - 1
                ListaMurosAranas(i).Muros_arania(j).AsignarBarras()
                ListaMurosAranas(i).Muros_arania(j).CalculoCentroide2()

                ListaMurosAranas(i).Muros_arania(j).Fc = Muros_lista_2.Find(Function(x) x.Pier_name = ListaMurosAranas(i).Muros_arania(j).NombreMuro).fc

            Next
        Next

        'Formar Poligono


        For i = 0 To ListaMurosAranas.Count - 1
            Dim Xmin As Single = 999999
            Dim Ymin As Single = 999999

            For j = 0 To ListaMurosAranas(i).Muros_arania.Count - 1
                If Xmin > ListaMurosAranas(i).Muros_arania(j).Xmin Then
                    Xmin = ListaMurosAranas(i).Muros_arania(j).Xmin
                End If
                If Ymin > ListaMurosAranas(i).Muros_arania(j).Ymin Then
                    Ymin = ListaMurosAranas(i).Muros_arania(j).Ymin
                End If

            Next
            ListaMurosAranas(i).Lista_XY_Min(0) = Xmin
            ListaMurosAranas(i).Lista_XY_Min(1) = Ymin
        Next

        Dim ListaTextoSectionDesig As New List(Of String)
        Dim ListaTextoPierSection As New List(Of String)



        For i = 0 To ListaMurosAranas.Count - 1

            With ListaMurosAranas(i)
                .ListNumerosShaesPorPiso.Clear()
                Dim NoShapes
                For m = .Muros_arania(0).Hw.Count - 1 To 0 Step -1
                    NoShapes = .Muros_arania.Count
                    For j = 0 To .Muros_arania.Count - 1
                        Try
                            NoShapes = .Muros_arania(j).ListaRefuerzosPorPiso(m).Count + NoShapes
                        Catch
                            NoShapes = NoShapes + 0
                        End Try


                    Next
                    .ListNumerosShaesPorPiso.Add(NoShapes)
                Next
            End With
        Next



        For i = 0 To ListaMurosAranas.Count - 1

            With ListaMurosAranas(i)
                .Lista_Arañas_Muro.Clear()
                For m = 0 To .ListNumerosShaesPorPiso.Count - 1
                    Dim NomenclaturaMuro As String = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  TYPE {Chr(34)}PIER{Chr(34)}"
                    ListaTextoSectionDesig.Add(NomenclaturaMuro)
                    .Lista_Arañas_Muro.Add(NomenclaturaMuro)
                    NomenclaturaMuro = $"  PIERSECTION  {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  BASEMATERIAL {Chr(34)}H{ .Muros_arania(0).Fc(.ListNumerosShaesPorPiso.Count - 1 - m)}{Chr(34)}"
                    ListaTextoPierSection.Add(NomenclaturaMuro)
                    Dim NoShape As Integer = 1
                    'Secciones
                    For j = 0 To .Muros_arania.Count - 1
                        Dim DeltaElMismo As Single = 0
                        Dim Delta1_Vecino As Single = 0
                        Dim Delta2_Vecino As Single = 0
                        Dim Xmin, Xmax, Ymin, Ymax
                        Xmin = .Muros_arania(j).XminE - .Lista_XY_Min(0)
                        Xmax = .Muros_arania(j).XmaxE - .Lista_XY_Min(0)
                        Ymin = .Muros_arania(j).YminE - .Lista_XY_Min(1)
                        Ymax = .Muros_arania(j).YmaxE - .Lista_XY_Min(1)
                        If .Muros_arania(j).DireccionMuro = "Horizontal" Then
                            'Cambio de Espesor
                            Try
                                DeltaElMismo = Math.Abs(.Muros_arania(j).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1))
                            Catch
                                DeltaElMismo = 0
                            End Try

                            If .Muros_arania(j).CambioDireccion = Reduccion.Abajo Then
                                Ymax =
                                    Ymax - DeltaElMismo
                            ElseIf .Muros_arania(j).CambioDireccion = Reduccion.Arriba Then
                                Ymin = Ymin + DeltaElMismo
                            ElseIf .Muros_arania(j).CambioDireccion = Reduccion.Centro Then
                                Ymin = Ymin + DeltaElMismo / 2
                                Ymax = Ymax - DeltaElMismo / 2
                            End If

                            'Cambio de Longitud
                            If .Muros_arania(j).MurosVecinosIzquierda.Count > 0 Then

                                If .Muros_arania(j).MurosVecinosIzquierda(0).CambioDireccion = Reduccion.Izquierda Then
                                    Try
                                        Delta1_Vecino = Math.Abs(.Muros_arania(j).MurosVecinosIzquierda(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosIzquierda(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1))
                                    Catch
                                        Delta1_Vecino = 0
                                    End Try
                                End If
                                If .Muros_arania(j).MurosVecinosIzquierda(0).CambioDireccion = Reduccion.Centro Then
                                    Try
                                        Delta1_Vecino = Math.Abs((.Muros_arania(j).MurosVecinosIzquierda(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosIzquierda(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1)) / 2)
                                    Catch
                                        Delta1_Vecino = 0
                                    End Try
                                End If


                            End If

                            If .Muros_arania(j).MurosVecinosDerecha.Count > 0 Then

                                If .Muros_arania(j).MurosVecinosDerecha(0).CambioDireccion = Reduccion.Derecha Then
                                    Try
                                        Delta2_Vecino = Math.Abs(.Muros_arania(j).MurosVecinosDerecha(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosDerecha(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1))
                                    Catch
                                        Delta2_Vecino = 0
                                    End Try
                                End If
                                If .Muros_arania(j).MurosVecinosDerecha(0).CambioDireccion = Reduccion.Centro Then
                                    Try
                                        Delta2_Vecino = Math.Abs((.Muros_arania(j).MurosVecinosDerecha(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosDerecha(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1)) / 2)
                                    Catch
                                        Delta2_Vecino = 0
                                    End Try
                                End If


                            End If

                            Xmin = Xmin - Delta1_Vecino
                            Xmax = Xmax + Delta2_Vecino


                        Else

                            'Cambio de Espesor
                            Try
                                DeltaElMismo = Math.Abs(.Muros_arania(j).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1))
                            Catch
                                DeltaElMismo = 0
                            End Try

                            If .Muros_arania(j).CambioDireccion = Reduccion.Izquierda Then
                                Xmax = Xmax - DeltaElMismo
                            ElseIf .Muros_arania(j).CambioDireccion = Reduccion.Derecha Then
                                Xmin = Xmin + DeltaElMismo
                            ElseIf .Muros_arania(j).CambioDireccion = Reduccion.Centro Then
                                Xmin = Xmin + DeltaElMismo / 2
                                Xmax = Xmax - DeltaElMismo / 2
                            End If

                            'Cambio de Longitud
                            If .Muros_arania(j).MurosVecinosAbajo.Count > 0 Then

                                If .Muros_arania(j).MurosVecinosAbajo(0).CambioDireccion = Reduccion.Abajo Then
                                    Try
                                        Delta1_Vecino = Math.Abs(.Muros_arania(j).MurosVecinosAbajo(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosAbajo(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1))
                                    Catch
                                        Delta1_Vecino = 0
                                    End Try
                                End If
                                If .Muros_arania(j).MurosVecinosAbajo(0).CambioDireccion = Reduccion.Centro Then
                                    Try
                                        Delta1_Vecino = Math.Abs((.Muros_arania(j).MurosVecinosAbajo(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosAbajo(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1)) / 2)
                                    Catch
                                        Delta1_Vecino = 0
                                    End Try
                                End If


                            End If

                            If .Muros_arania(j).MurosVecinosArriba.Count > 0 Then

                                If .Muros_arania(j).MurosVecinosArriba(0).CambioDireccion = Reduccion.Arriba Then
                                    Try
                                        Delta2_Vecino = Math.Abs(.Muros_arania(j).MurosVecinosArriba(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosArriba(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1))
                                    Catch
                                        Delta2_Vecino = 0
                                    End Try
                                End If
                                If .Muros_arania(j).MurosVecinosArriba(0).CambioDireccion = Reduccion.Centro Then
                                    Try
                                        Delta2_Vecino = Math.Abs((.Muros_arania(j).MurosVecinosArriba(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosArriba(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1)) / 2)
                                    Catch
                                        Delta2_Vecino = 0
                                    End Try
                                End If


                            End If

                            Ymin = Ymin - Delta1_Vecino
                            Ymax = Ymax + Delta2_Vecino

                        End If

                        NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  MATERIAL {Chr(34)}H{ .Muros_arania(j).Fc(.ListNumerosShaesPorPiso.Count - 1 - m)}{Chr(34)}  SHAPETYPE {Chr(34)}POLYGON{Chr(34)}  NUMCORNERPTS {4}"
                        ListaTextoSectionDesig.Add(NomenclaturaMuro)
                        NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  POLYCORNER {1}  X {Xmin}  Y {Ymin}"
                        ListaTextoSectionDesig.Add(NomenclaturaMuro)
                        NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  POLYCORNER {2}  X {Xmax}  Y {Ymin}"
                        ListaTextoSectionDesig.Add(NomenclaturaMuro)
                        NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  POLYCORNER {3}  X {Xmax}  Y {Ymax}"
                        ListaTextoSectionDesig.Add(NomenclaturaMuro)
                        NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  POLYCORNER {4}  X {Xmin}  Y {Ymax}"
                        ListaTextoSectionDesig.Add(NomenclaturaMuro)
                        NoShape = NoShape + 1

                    Next

                    'Barras
                    For j = 0 To .Muros_arania.Count - 1
                        Try
                            For s = 0 To .Muros_arania(j).ListaRefuerzosPorPiso(.ListNumerosShaesPorPiso.Count - 1 - m).Count - 1
                                Dim XC, YC, NoBarra

                                XC = .Muros_arania(j).ListaRefuerzosPorPiso(.ListNumerosShaesPorPiso.Count - 1 - m)(s).CoordenadasXyY(0) - .Lista_XY_Min(0)
                                YC = .Muros_arania(j).ListaRefuerzosPorPiso(.ListNumerosShaesPorPiso.Count - 1 - m)(s).CoordenadasXyY(1) - .Lista_XY_Min(1)

                                NoBarra = .Muros_arania(j).ListaRefuerzosPorPiso(.ListNumerosShaesPorPiso.Count - 1 - m)(s).NoBarra
                                NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  MATERIAL {Chr(34)}A615Gr60{Chr(34)}  SHAPETYPE {Chr(34)}REBAR{Chr(34)}  BARSIZE {Chr(34)}{NoBarra}{Chr(34)}  XC {XC}  YC {YC}"
                                ListaTextoSectionDesig.Add(NomenclaturaMuro)
                                NoShape = NoShape + 1

                            Next
                        Catch
                        End Try
                    Next


                    'Mallas 

                    For j = 0 To .Muros_arania.Count - 1


                        Dim MuroMalla As Muros_Consolidados = Muros_lista_2.Find(Function(x) x.Pier_name = .Muros_arania(j).NombreMuro)

                        For s = 0 To MuroMalla.Malla(.ListNumerosShaesPorPiso.Count - 1 - m).Count - 1

                            Dim DeltaElMismo As Single = 0
                            Dim Delta1_Vecino As Single = 0
                            Dim Delta2_Vecino As Single = 0
                            Dim Xmin, Xmax, Ymin, Ymax
                            Xmin = .Muros_arania(j).XminE - .Lista_XY_Min(0)
                            Xmax = .Muros_arania(j).XmaxE - .Lista_XY_Min(0)
                            Ymin = .Muros_arania(j).YminE - .Lista_XY_Min(1)
                            Ymax = .Muros_arania(j).YmaxE - .Lista_XY_Min(1)
                            If .Muros_arania(j).DireccionMuro = "Horizontal" Then
                                'Cambio de Espesor
                                Try
                                    DeltaElMismo = Math.Abs(.Muros_arania(j).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1))
                                Catch
                                    DeltaElMismo = 0
                                End Try

                                If .Muros_arania(j).CambioDireccion = Reduccion.Abajo Then
                                    Ymax = Ymax - DeltaElMismo
                                ElseIf .Muros_arania(j).CambioDireccion = Reduccion.Arriba Then
                                    Ymin = Ymin + DeltaElMismo
                                ElseIf .Muros_arania(j).CambioDireccion = Reduccion.Centro Then

                                    Ymin = Ymin + DeltaElMismo / 2
                                    Ymax = Ymax - DeltaElMismo / 2
                                End If

                                'Cambio de Longitud
                                If .Muros_arania(j).MurosVecinosIzquierda.Count > 0 Then

                                    If .Muros_arania(j).MurosVecinosIzquierda(0).CambioDireccion = Reduccion.Izquierda Then
                                        Try
                                            Delta1_Vecino = Math.Abs(.Muros_arania(j).MurosVecinosIzquierda(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosIzquierda(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1))
                                        Catch
                                            Delta1_Vecino = 0
                                        End Try
                                    End If
                                    If .Muros_arania(j).MurosVecinosIzquierda(0).CambioDireccion = Reduccion.Centro Then
                                        Try
                                            Delta1_Vecino = Math.Abs((.Muros_arania(j).MurosVecinosIzquierda(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosIzquierda(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1)) / 2)
                                        Catch
                                            Delta1_Vecino = 0
                                        End Try
                                    End If


                                End If

                                If .Muros_arania(j).MurosVecinosDerecha.Count > 0 Then

                                    If .Muros_arania(j).MurosVecinosDerecha(0).CambioDireccion = Reduccion.Derecha Then
                                        Try
                                            Delta2_Vecino = Math.Abs(.Muros_arania(j).MurosVecinosDerecha(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosDerecha(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1))
                                        Catch
                                            Delta2_Vecino = 0
                                        End Try
                                    End If
                                    If .Muros_arania(j).MurosVecinosDerecha(0).CambioDireccion = Reduccion.Centro Then
                                        Try
                                            Delta2_Vecino = Math.Abs((.Muros_arania(j).MurosVecinosDerecha(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosDerecha(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1)) / 2)
                                        Catch
                                            Delta2_Vecino = 0
                                        End Try
                                    End If


                                End If

                                Xmin = Xmin - Delta1_Vecino
                                Xmax = Xmax + Delta2_Vecino

                            Else

                                'Cambio de Espesor
                                Try
                                    DeltaElMismo = Math.Abs(.Muros_arania(j).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1))
                                Catch
                                    DeltaElMismo = 0
                                End Try

                                If .Muros_arania(j).CambioDireccion = Reduccion.Izquierda Then
                                    Xmax = Xmax - DeltaElMismo
                                ElseIf .Muros_arania(j).CambioDireccion = Reduccion.Derecha Then
                                    Xmin = Xmin + DeltaElMismo
                                ElseIf .Muros_arania(j).CambioDireccion = Reduccion.Centro Then
                                    Xmin = Xmin + DeltaElMismo / 2
                                    Xmax = Xmax - DeltaElMismo / 2
                                End If

                                'Cambio de Longitud
                                If .Muros_arania(j).MurosVecinosAbajo.Count > 0 Then

                                    If .Muros_arania(j).MurosVecinosAbajo(0).CambioDireccion = Reduccion.Abajo Then
                                        Try
                                            Delta1_Vecino = Math.Abs(.Muros_arania(j).MurosVecinosAbajo(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosAbajo(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1))
                                        Catch
                                            Delta1_Vecino = 0
                                        End Try
                                    End If
                                    If .Muros_arania(j).MurosVecinosAbajo(0).CambioDireccion = Reduccion.Centro Then
                                        Try
                                            Delta1_Vecino = Math.Abs((.Muros_arania(j).MurosVecinosAbajo(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosAbajo(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1)) / 2)
                                        Catch
                                            Delta1_Vecino = 0
                                        End Try
                                    End If


                                End If

                                If .Muros_arania(j).MurosVecinosArriba.Count > 0 Then

                                    If .Muros_arania(j).MurosVecinosArriba(0).CambioDireccion = Reduccion.Arriba Then
                                        Try
                                            Delta2_Vecino = Math.Abs(.Muros_arania(j).MurosVecinosArriba(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosArriba(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1))
                                        Catch
                                            Delta2_Vecino = 0
                                        End Try
                                    End If
                                    If .Muros_arania(j).MurosVecinosArriba(0).CambioDireccion = Reduccion.Centro Then
                                        Try
                                            Delta2_Vecino = Math.Abs((.Muros_arania(j).MurosVecinosArriba(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m) - .Muros_arania(j).MurosVecinosArriba(0).EspesorePorPiso(.ListNumerosShaesPorPiso.Count - 1 - m + 1)) / 2)
                                        Catch
                                            Delta2_Vecino = 0
                                        End Try
                                    End If


                                End If

                                Ymin = Ymin - Delta1_Vecino
                                Ymax = Ymax + Delta2_Vecino

                            End If

                            Dim Malla As String = MuroMalla.Malla(.ListNumerosShaesPorPiso.Count - 1 - m)
                            Dim NoMallas As Integer = 0
                            If Malla <> "Sin Malla" And Malla <> "" Then
                                For letras = 0 To Len(Malla) - 1 : If Malla.Chars(letras) = "D" Then : NoMallas = NoMallas + 1 : End If : Next




#Region "Doble Malla"
                                If NoMallas = 2 Then


                                    If .Muros_arania(j).DireccionMuro = "Horizontal" Then

                                        Dim CantidadBarrasMalla As Integer = Math.Ceiling((((Xmax - Xmin) - (2 * 0.04)) / Separaciones_Malla(Malla)))

                                        For h = 0 To CantidadBarrasMalla
                                            Dim XC, YC
                                            If h = 0 Then
                                                XC = Xmin + 0.04
                                            Else
                                                XC = Xmin + 0.04 + h * Separaciones_Malla(Malla)
                                            End If
                                            YC = Ymin + 0.04
                                            If XC <= Xmax Then
                                                NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  MATERIAL {Chr(34)}A615Gr60{Chr(34)}  SHAPETYPE {Chr(34)}REBAR{Chr(34)}  BARSIZE {Chr(34)}{"#2"}{Chr(34)}  XC {XC}  YC {YC}"
                                                ListaTextoSectionDesig.Add(NomenclaturaMuro)
                                                NoShape = NoShape + 1
                                            End If

                                            If h = 0 Then
                                                XC = Xmin + 0.04
                                            Else
                                                XC = Xmin + 0.04 + h * Separaciones_Malla(Malla)
                                            End If
                                            YC = Ymax - 0.04
                                            If XC <= Xmax Then
                                                NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  MATERIAL {Chr(34)}A615Gr60{Chr(34)}  SHAPETYPE {Chr(34)}REBAR{Chr(34)}  BARSIZE {Chr(34)}{"#2"}{Chr(34)}  XC {XC}  YC {YC}"
                                                ListaTextoSectionDesig.Add(NomenclaturaMuro)
                                                NoShape = NoShape + 1
                                            End If
                                        Next

                                    Else

                                        Dim CantidadBarrasMalla As Integer = Math.Ceiling((((Ymax - Ymin) - (2 * 0.04)) / Separaciones_Malla(Malla)))

                                        For h = 0 To CantidadBarrasMalla
                                            Dim XC, YC

                                            If h = 0 Then
                                                YC = Ymin + 0.04
                                            Else
                                                YC = Ymin + 0.04 + h * Separaciones_Malla(Malla)
                                            End If
                                            XC = Xmin + 0.04
                                            If YC <= Ymax Then
                                                NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  MATERIAL {Chr(34)}A615Gr60{Chr(34)}  SHAPETYPE {Chr(34)}REBAR{Chr(34)}  BARSIZE {Chr(34)}{"#2"}{Chr(34)}  XC {XC}  YC {YC}"
                                                ListaTextoSectionDesig.Add(NomenclaturaMuro)
                                                NoShape = NoShape + 1
                                            End If
                                            XC = Xmax - 0.04
                                            If h = 0 Then
                                                YC = Ymin + 0.04
                                            Else
                                                YC = Ymin + 0.04 + h * Separaciones_Malla(Malla)
                                            End If
                                            If YC <= Ymax Then
                                                NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  MATERIAL {Chr(34)}A615Gr60{Chr(34)}  SHAPETYPE {Chr(34)}REBAR{Chr(34)}  BARSIZE {Chr(34)}{"#2"}{Chr(34)}  XC {XC}  YC {YC}"
                                            ListaTextoSectionDesig.Add(NomenclaturaMuro)
                                                NoShape = NoShape + 1
                                            End If
                                        Next

                                    End If




                                End If

#End Region

#Region "Malla Central"

                                If NoMallas = 1 Then


                                    If .Muros_arania(j).DireccionMuro = "Horizontal" Then

                                        Dim CantidadBarrasMalla As Integer = Math.Ceiling((((Xmax - Xmin) - (2 * 0.04)) / Separaciones_Malla(Malla)))

                                        For h = 0 To CantidadBarrasMalla
                                            Dim XC, YC
                                            If h = 0 Then
                                                XC = Xmin + 0.04
                                            Else
                                                XC = Xmin + 0.04 + h * Separaciones_Malla(Malla)
                                            End If
                                            YC = Ymin + (Ymax - Ymin) / 2
                                            If XC <= Xmax Then
                                                NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  MATERIAL {Chr(34)}A615Gr60{Chr(34)}  SHAPETYPE {Chr(34)}REBAR{Chr(34)}  BARSIZE {Chr(34)}{"#2"}{Chr(34)}  XC {XC}  YC {YC}"
                                                ListaTextoSectionDesig.Add(NomenclaturaMuro)
                                                NoShape = NoShape + 1
                                            End If
                                        Next

                                    Else

                                        Dim CantidadBarrasMalla As Integer = Math.Ceiling((((Ymax - Ymin) - (2 * 0.04)) / Separaciones_Malla(Malla)))

                                        For h = 0 To CantidadBarrasMalla
                                            Dim XC, YC

                                            XC = Xmin + (Xmax - Xmin) / 2
                                            If h = 0 Then
                                                YC = Ymin + 0.04
                                            Else
                                                YC = Ymin + 0.04 + h * Separaciones_Malla(Malla)
                                            End If
                                            If YC <= Ymax Then
                                                NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  MATERIAL {Chr(34)}A615Gr60{Chr(34)}  SHAPETYPE {Chr(34)}REBAR{Chr(34)}  BARSIZE {Chr(34)}{"#2"}{Chr(34)}  XC {XC}  YC {YC}"
                                                ListaTextoSectionDesig.Add(NomenclaturaMuro)
                                                NoShape = NoShape + 1
                                            End If
                                        Next

                                    End If




                                End If

#End Region

                            End If

                        Next


                    Next

                    .ListNumerosShaesPorPiso(m) = NoShape
                Next
            End With
        Next


        For i = 0 To ListaMurosAranas.Count - 1

            For m = 0 To ListaMurosAranas(i).ListNumerosShaesPorPiso.Count - 1

                Dim Indice = ListaTextoSectionDesig.FindIndex(Function(x) x.Contains($"  SDSECTION {Chr(34)}{ ListaMurosAranas(i).Label }-Story{m + 1}{Chr(34)}  TYPE {Chr(34)}PIER{Chr(34)}"))
                ListaTextoSectionDesig(Indice) = $"  SDSECTION {Chr(34)}{ ListaMurosAranas(i).Label }-Story{m + 1}{Chr(34)}  TYPE {Chr(34)}PIER{Chr(34)}  NUMSHAPES {ListaMurosAranas(i).ListNumerosShaesPorPiso(m) - 1}"

            Next



        Next



        Dim IndicePierSections As Integer = ArchivoET.FindIndex(Function(x) x.Contains("$ PIER SECTIONS")) + 1
        ArchivoET.InsertRange(IndicePierSections, ListaTextoPierSection)


        Dim IndiceSectionDesinger As Integer = ArchivoET.FindIndex(Function(x) x.Contains("$ SECTION DESIGNER SECTIONS")) + 1
        ArchivoET.InsertRange(IndiceSectionDesinger, ListaTextoSectionDesig)


        Dim Save As SaveFileDialog
        Save = New SaveFileDialog
        Save.Title = "Archivo Modificado  $et"
        Save.Filter = "File |*.$et"
        Save.ShowDialog()

        Dim Escritor As StreamWriter = New StreamWriter(Save.FileName)

        For i = 0 To ArchivoET.Count - 1
            Escritor.WriteLine(ArchivoET(i))
        Next

        Escritor.Close()

        MsgBox("Modificado con Éxito.", MsgBoxStyle.Information, "efe Prima Ce")



    End Sub









End Class
