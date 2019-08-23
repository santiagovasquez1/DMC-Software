Public Class Muros_Auxiliares

    Public NombreMuro As String
    Public CoordenadasX As List(Of Double)
    Public CoordenadasY As List(Of Double)
    Public MurosVecinos As New List(Of String)

    Public DeltaX_Label As New List(Of String)
    Public DeltaY_Label As New List(Of String)

    Public DeltaX_Numero As New List(Of Double)
    Public DeltaY_Numero As New List(Of Double)

    Public Lista_Refuerzos As New List(Of RefuerzoCirculo)

    Public Lista_Refuerzos_Fila_Min As New List(Of RefuerzoCirculo)
    Public Lista_Refuerzos_Fila_Max As New List(Of RefuerzoCirculo)
    Public RecubrimientoRefuerzo As Double

    Public Lista_NoBarras As New List(Of String)
    Public Lista_LongitudBarras As New List(Of Double)
    Public Malla As String
    Public Capas_RefuerzoHorizontal As Integer
    Public RefuerzoHorizontalLabel As String

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

            End If
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