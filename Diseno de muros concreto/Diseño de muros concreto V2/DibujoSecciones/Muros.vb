<Serializable>
Public Enum Reduccion

    Arriba
    Abajo
    Izquierda
    Derecha
    Centro
    NoAplica

End Enum
<Serializable>
Public Enum TipoRefuerzo

    Ninguno
    R  'Recto
    R1G 'Recto con 1 Gancho
    R2G 'Recto con 2 Ganchos
    L  ' En L
    S1G  ' En S con un gancho
    L1G ' En L con un gancho
    C ' En C
    S  ' En S(C)


End Enum


Public Class Muros

    Public NombreMuro As String
    Public CoordenadasX As List(Of Double)
    Public CoordenadasY As List(Of Double)
    Public MurosVecinos As New List(Of String)

    Public Lista_Refuerzos As New List(Of RefuerzoCirculo)
    Public Lista_Refuerzos_Original2 As New List(Of Double())

    Public Lista_Refuerzos_Original As New List(Of Double())

    Public Lista_Refuerzos_Fila_Min As New List(Of RefuerzoCirculo)
    Public Lista_Refuerzos_Fila_Max As New List(Of RefuerzoCirculo)
    Public RecubrimientoRefuerzo As Double

    Public Lista_NoBarras As New List(Of String)
    Public Lista_LongitudBarras As New List(Of Double)
    Public Malla As String
    Public Capas_RefuerzoHorizontal As Integer
    Public RefuerzoHorizontalLabel As String
    Public Sep_RefuerzoHorizontal As String

    Public MurosVecinosClase As New List(Of Muros)
    Public MurosVecinosDerecha As New List(Of Muros)
    Public MurosVecinosIzquierda As New List(Of Muros)
    Public MurosVecinosArriba As New List(Of Muros)
    Public MurosVecinosAbajo As New List(Of Muros)

    Public CentroideX As Double
    Public CentroideY As Double
    Public Property Xmax As Double
    Public Property Xmin As Double
    Public Property Ymin As Double
    Public Property Ymax As Double
    Public Property XmaxE As Double
    Public Property XminE As Double
    Public Property YminE As Double
    Public Property YmaxE As Double

    Public LEB_Iz As Double
    Public LEB_Dr As Double

    Public XmaxM As Double
    Public XminM As Double
    Public YminM As Double
    Public YmaxM As Double

    Public Hatch_pattern_Izq As String
    Public Hatch_pattern_Der As String
    Public Hatch_Layer_Izq As String
    Public Hatch_Layer_Der As String

    Public Longitud As Double
    Public DireccionMuro As String
    Public EspesorEscalado As Double
    Public EspesorReal As Double
    Public MurosVecinosP As New List(Of Integer)
    Public MurosVecinosPY As New List(Of Integer)
    Public MurosVeciosYmin As New List(Of Double)
    Public MurosVeciosXmin As New List(Of Double)
    Public CoordenadasaGraficas As New List(Of Double)
    Public PuntosHatchIz As New List(Of Double)
    Public PuntosHatchDer As New List(Of Double)
    Public CambioDireccion As Reduccion
    Public EspesorePorPiso As New List(Of Single)

    Public Recubrimiento_Malla As Double = 0.02
    Public LongMallaHoriz As Double = 0

    'Nuevas Propiedades

    Public FormaRefuerzoHorizontal As TipoRefuerzo
    Public LongMallaHorziPorPiso As New List(Of Single)
    Public Leb_Dr_PorPiso As New List(Of Single)
    Public Leb_Izq_PorPiso As New List(Of Single)
    Public FormaRefuerzoHorizontal_PorPiso As New List(Of TipoRefuerzo)
    Public Sep_RefuerzoHorizontal_PorPiso As New List(Of Single)
    Public Hw As New List(Of Single)
    Public Capas_RefuerzoHorizontalPorPiso As List(Of Integer)
    Public RefuerzoHorizontalLabelPorPiso As New List(Of String)


    Public ListaRefuerzosPorPiso As New List(Of List(Of RefuerzoCirculo))

    Public Fc As List(Of Single)
    Public XC As Single
    Public YC As Single

    Sub CalculoCentroide2()
        XC = XminE + (XmaxE - XminE) / 2
        YC = YminE + (YmaxE - YminE) / 2
    End Sub




    Sub AsignarBarras()
        ListaRefuerzosPorPiso.Clear()

        For No_Piso = Muros_lista_2(0).Hw.Count - 1 To 0 Step -1

            If alzado_lista.Exists(Function(x) x.pier = NombreMuro And x.story = "Story" & (No_Piso + 1)) Then
                Dim ListasRefuerzos_Aux As New List(Of RefuerzoCirculo)
                Dim MuroConAlzado = alzado_lista.Find(Function(x) x.pier = NombreMuro And x.story = "Story" & (No_Piso + 1))
                For NomencBarra = 0 To MuroConAlzado.alzado.Count - 1
                    For i = 0 To Lista_Refuerzos.Count - 1
                        Dim Nomen As String = Str(NomencBarra + 1).Trim()
                        If Nomen = Lista_Refuerzos(i).Label And MuroConAlzado.alzado(NomencBarra) <> "" Then
                            Lista_Refuerzos(i).CoordenadasXyY = Lista_Refuerzos_Original2(i).ToArray
                            ListasRefuerzos_Aux.Add(Lista_Refuerzos(i))

                        End If
                    Next
                Next
                ListaRefuerzosPorPiso.Add(ListasRefuerzos_Aux.ToList)
            End If
        Next



    End Sub



    Sub ClasificacionMuros()
        For i = 0 To MurosVecinosClase.Count - 1
            If Math.Round(XminE, 2) = Math.Round(MurosVecinosClase(i).XmaxE, 2) Then
                MurosVecinosIzquierda.Add(MurosVecinosClase(i))
            End If
            If Math.Round(XmaxE, 2) = Math.Round(MurosVecinosClase(i).XminE, 2) Then
                MurosVecinosDerecha.Add(MurosVecinosClase(i))
            End If
            If Math.Round(YmaxE, 2) = Math.Round(MurosVecinosClase(i).YminE, 2) Then
                MurosVecinosArriba.Add(MurosVecinosClase(i))
            End If
            If Math.Round(YminE, 2) = Math.Round(MurosVecinosClase(i).YmaxE, 2) Then
                MurosVecinosAbajo.Add(MurosVecinosClase(i))
            End If
        Next

    End Sub

    Sub ActualizarCoordenadas()
        XmaxM = Xmax
        XminM = Xmin
        YmaxM = Ymax
        YminM = Ymin

    End Sub

    Sub PutosHatchFuc()

        If DireccionMuro = "Horizontal" Then
            If LEB_Iz >= Math.Round(Longitud, 2) Or LEB_Dr >= Math.Round(Longitud, 2) Then
                PuntosHatchIz.Add(Xmin)
                PuntosHatchIz.Add(Ymin)

                PuntosHatchIz.Add(Xmax)
                PuntosHatchIz.Add(Ymin)

                PuntosHatchIz.Add(Xmax)
                PuntosHatchIz.Add(Ymax)

                PuntosHatchIz.Add(Xmin)
                PuntosHatchIz.Add(Ymax)
            Else

                'ELEMENTO IZQUIERDO
                PuntosHatchIz.Add(Xmin)
                PuntosHatchIz.Add(Ymin)

                PuntosHatchIz.Add(Xmin + LEB_Iz)
                PuntosHatchIz.Add(Ymin)

                PuntosHatchIz.Add(Xmin + LEB_Iz)
                PuntosHatchIz.Add(Ymax)

                PuntosHatchIz.Add(Xmin)
                PuntosHatchIz.Add(Ymax)

                'ELEMENTO DERECHO
                PuntosHatchDer.Add(Xmax)
                PuntosHatchDer.Add(Ymin)

                PuntosHatchDer.Add(Xmax - LEB_Dr)
                PuntosHatchDer.Add(Ymin)

                PuntosHatchDer.Add(Xmax - LEB_Dr)
                PuntosHatchDer.Add(Ymax)

                PuntosHatchDer.Add(Xmax)
                PuntosHatchDer.Add(Ymax)
            End If
        Else
            If LEB_Iz >= Math.Round(Longitud, 2) Or LEB_Dr >= Math.Round(Longitud, 2) Then
                PuntosHatchIz.Add(Xmin)
                PuntosHatchIz.Add(Ymin)

                PuntosHatchIz.Add(Xmin)
                PuntosHatchIz.Add(Ymax)

                PuntosHatchIz.Add(Xmax)
                PuntosHatchIz.Add(Ymax)

                PuntosHatchIz.Add(Xmax)
                PuntosHatchIz.Add(Ymin)
            Else

                'ELEMENTO IZQUIERDO
                PuntosHatchIz.Add(Xmin)
                PuntosHatchIz.Add(Ymin)

                PuntosHatchIz.Add(Xmin)
                PuntosHatchIz.Add(Ymin + LEB_Iz)

                PuntosHatchIz.Add(Xmax)
                PuntosHatchIz.Add(Ymin + LEB_Iz)

                PuntosHatchIz.Add(Xmax)
                PuntosHatchIz.Add(Ymin)

                'ELEMENTO DERECHO

                PuntosHatchDer.Add(Xmin)
                PuntosHatchDer.Add(Ymax)

                PuntosHatchDer.Add(Xmax)
                PuntosHatchDer.Add(Ymax)

                PuntosHatchDer.Add(Xmax)
                PuntosHatchDer.Add(Ymax - LEB_Dr)

                PuntosHatchDer.Add(Xmin)
                PuntosHatchDer.Add(Ymax - LEB_Dr)

            End If
        End If

    End Sub

End Class

Public Class RefuerzoCirculo

    Public Label As String
    Public MuroPerteneciente As String
    Dim mCoordenadasXyY(2) As Double
    Public IndiceMuroPerteneciente As Integer
    Public Gancho As Boolean = False
    Public NoBarra As String



    Public Property CoordenadasXyY() As Double()
        Get
            CoordenadasXyY = mCoordenadasXyY
        End Get
        Set(value As Double())
            mCoordenadasXyY = value
        End Set
    End Property

End Class

Public Class TextoRefuerzo
    Public CoordenasdasXyY(2) As Double
    Public NombreTexto As Double

End Class

Public Class CircunferenciaBloque

    Public Nombre As String
    Public Radio As Double
    Dim mCoordenadasXyY(2) As Double

    Public Property CoordenadasXyY() As Double()
        Get
            CoordenadasXyY = mCoordenadasXyY
        End Get
        Set(value As Double())
            mCoordenadasXyY = value
        End Set
    End Property

End Class

Public Class MuroArana

    Public MurosComponene As New List(Of String)

End Class
