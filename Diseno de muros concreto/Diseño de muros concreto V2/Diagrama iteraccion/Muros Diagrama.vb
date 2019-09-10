Imports Diseño_de_muros_concreto_V2

Public Class Diagrama

    Private _muros_i As List(Of Muros) = New List(Of Muros)
    Private _coordX As List(Of List(Of Double)) = New List(Of List(Of Double))
    Private _coordY As List(Of List(Of Double)) = New List(Of List(Of Double))
    Private _centroide As Double()

    Public Property Centroide As Double()
        Get
            Return _centroide
        End Get
        Set(value As Double())
            _centroide = value
        End Set
    End Property

    Public Property Muros_i As List(Of Muros)
        Get
            Return _muros_i
        End Get
        Set(value As List(Of Muros))
            _muros_i = value
        End Set
    End Property

    Public Property CoordX As List(Of List(Of Double))
        Get
            Return _coordX
        End Get
        Set(value As List(Of List(Of Double)))
            _coordX = value
        End Set
    End Property

    Public Property CoordY As List(Of List(Of Double))
        Get
            Return _coordY
        End Get
        Set(value As List(Of List(Of Double)))
            _coordY = value
        End Set
    End Property

    Sub New(ByVal Lista_i As List(Of Muros))
        Muros_i = Lista_i
        Calculo_Centroide()
    End Sub

    Sub Calculo_Centroide()

        Dim SumatoriaX As Double
        Dim SumatoriaY As Double
        Dim n As Integer

        SumatoriaX = 0 : SumatoriaY = 0
        n = 0

        For i = 0 To Muros_i.Count - 1

            For j = 0 To Muros_i(i).CoordenadasX.Count - 1

                SumatoriaX += Muros_i(i).CoordenadasX(j)
                SumatoriaY += Muros_i(i).CoordenadasY(j)
                n += 1
            Next

        Next

        Centroide = New Double() {SumatoriaX / n, SumatoriaY / n}

    End Sub

    Sub Datos_Diagrama()

        Dim Area_comp As Double
        Dim beta As Single
        Dim fc As Single
        Dim muroi As Muros_Consolidados

        For i = 0 To Muros_i.Count - 1

            muroi = Muros_lista_2.Find(Function(x) x.Pier_name = Muros_i(i).NombreMuro)
            fc = muroi.fc(0)

            If fc <= 280 Then beta = 0.75 Else beta = 0.8
            If fc = 350 Then beta = 0.8

        Next





    End Sub

End Class
