Imports netDxf
Imports netDxf.Entities
Imports netDxf.Tables
Imports netDxf.Header
Imports netDxf.Units
Public Class Muros

    Public NombreMuro As String
    Public CoordenadasX As List(Of Double)
    Public CoordenadasY As List(Of Double)
    Public MurosVecinos As New List(Of String)

    Public Lista_Refuerzos As New List(Of RefuerzoCirculo)
    Public Lista_Refuerzos_Original As New List(Of RefuerzoCirculo)

    Public Lista_Refuerzos_Fila_Min As New List(Of RefuerzoCirculo)
    Public Lista_Refuerzos_Fila_Max As New List(Of RefuerzoCirculo)
    Public RecubrimientoRefuerzo As Double


    Public Lista_NoBarras As New List(Of String)
    Public Lista_LongitudBarras As New List(Of Double)
    Public Malla As String
    Public Capas_RefuerzoHorizontal As Integer
    Public RefuerzoHorizontalLabel As String

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
    Public CoordenadasXyY(2) As Double
    Public IndiceMuroPerteneciente As Integer
    Public Gancho As Boolean
End Class


Public Class TextoRefuerzo
    Public CoordenasdasXyY(2) As Double
    Public NombreTexto As Double


End Class


Public Class CircunferenciaBloque

    Public Nombre As String
    Public Radio As Double
    Public CoordenadasXyY(2) As Double

    'Public Property CoordenadasXyY() As Double
    '    Get

    '    End Get
    '    Set(value As Double)

    '    End Set
    'End Property

End Class

Public Class MuroArana

    Public MurosComponene As New List(Of String)

End Class



Public Class Muros_Consolidados

    Public Pier_name As String
    Public Stories As List(Of String) = New List(Of String)
    Public Reduccion As String = "Sin Reducc"

    Public Bw As List(Of Single) = New List(Of Single)
    Public lw As List(Of Single) = New List(Of Single)
    Public Hw As List(Of Single) = New List(Of Single)
    Public fc As List(Of Single) = New List(Of Single)
    Public Rho_T As List(Of Double) = New List(Of Double)
    Public Rho_l As List(Of Double) = New List(Of Double)
    Public Malla As List(Of String) = New List(Of String)
    Public Sigma_piso As List(Of Double) = New List(Of Double)
    Public Confinamiento As List(Of String) = New List(Of String)
    Public C_max As List(Of Double) = New List(Of Double)
    Public C_min As List(Of Double) = New List(Of Double)
    Public C_esfuerzo As List(Of Double) = New List(Of Double)
    Public C_Def As List(Of Double) = New List(Of Double)
    Public L_esfuerzo As List(Of Double) = New List(Of Double)
    Public L_Conf_Max As List(Of Double) = New List(Of Double)
    Public L_Conf_Min As List(Of Double) = New List(Of Double)
    Public Lebe_Izq As List(Of Double) = New List(Of Double)
    Public Lebe_Der As List(Of Double) = New List(Of Double)
    Public Lebe_Centro As List(Of Double) = New List(Of Double)
    Public Zc_Izq As List(Of Double) = New List(Of Double)
    Public Zc_Der As List(Of Double) = New List(Of Double)
    'Nuevas variables
    Public Est_ebe As List(Of Integer) = New List(Of Integer)
    Public Sep_ebe As List(Of Double) = New List(Of Double)
    Public Est_Zc As List(Of Integer) = New List(Of Integer)
    Public Sep_Zc As List(Of Double) = New List(Of Double)
    Public As_Long As List(Of Double) = New List(Of Double)
    '
    Public ramas_der As List(Of Integer) = New List(Of Integer)
    Public ramas_izq As List(Of Integer) = New List(Of Integer)
    Public ramas_centro As List(Of Integer) = New List(Of Integer)
    Public As_htal As List(Of Double) = New List(Of Double)
    Public Ref_htal As List(Of String) = New List(Of String)
    Public Capas_htal As List(Of Integer) = New List(Of Integer)
    Public sep_htal As List(Of Double) = New List(Of Double)
    Public As_Htal_Total As List(Of Double) = New List(Of Double)

    Public NoBarras As List(Of Double) = New List(Of Double)

    Public NombreBarras As New List(Of List(Of String))

    Public LongitudBarras As New List(Of List(Of Double))
End Class