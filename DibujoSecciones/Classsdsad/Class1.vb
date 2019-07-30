

Public Class Class1


    <Autodesk.AutoCAD.Runtime.CommandMethod("Ventana")> Sub HOLA()

        Dim Formulario As New Form1

        Formulario.Show()

    End Sub






End Class


Public Class Muros

    Public NombreMuro As String
    Public CoordenadasX As List(Of Double)
    Public CoordenadasY As List(Of Double)
    Public MurosVecinos As New List(Of String)
    Public Pendientes As New List(Of Double)
    Public CentroideX As Double
    Public CentroideY As Double
    Public Xmax As Double
    Public Xmin As Double
    Public Ymin As Double
    Public Ymax As Double
    Public XmaxE As Double
    Public XminE As Double
    Public YminE As Double
    Public YmaxE As Double


    Public Longitud As Double
    Public DireccionMuro As String
    Public EspesorEscalado As Double
    Public EspesorReal As Double
    Public MurosVecinosP As New List(Of Integer)
    Public MurosVecinosPY As New List(Of Integer)
    Public MurosVeciosYmin As New List(Of Double)


    Public CoordenadasaGraficas As New List(Of Double)

    Event CambiarX()

    Sub Cambio()

    End Sub




    Sub VerticesAGraficar()

        For i = 0 To 3
            CoordenadasaGraficas.Add(Xmin)
            CoordenadasaGraficas.Add(Ymin)

            CoordenadasaGraficas.Add(Xmax)
            CoordenadasaGraficas.Add(Ymin)

            CoordenadasaGraficas.Add(Xmax)
            CoordenadasaGraficas.Add(Ymax)

            CoordenadasaGraficas.Add(Xmin)
            CoordenadasaGraficas.Add(Ymax)


        Next

    End Sub




End Class


Public Class CircunferenciaBloque

    Public Nombre As String
    Public Radio As Double
    Public CoordenadasXyY(2) As Double

End Class

Public Class MuroArana

    Public MurosComponene As New List(Of String)

End Class