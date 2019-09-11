Imports Diseño_de_muros_concreto_V2

Public Class Diagrama

    Private _muros_i As List(Of Muros) = New List(Of Muros)
    Private _coordX As List(Of List(Of Double)) = New List(Of List(Of Double))
    Private _coordY As List(Of List(Of Double)) = New List(Of List(Of Double))
    Private _centroide As Double()
    Private _pn_max As List(Of Double) = New List(Of Double)
    Private _phi_pn_max As List(Of Double) = New List(Of Double)

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

    Public Property Pn_max As List(Of Double)
        Get
            Return _pn_max
        End Get
        Set(value As List(Of Double))
            _pn_max = value
        End Set
    End Property

    Public Property Phi_Pn_max As List(Of Double)
        Get
            Return _phi_pn_max
        End Get
        Set(value As List(Of Double))
            _phi_pn_max = value
        End Set
    End Property

    Sub New(ByVal Lista_i As List(Of Muros))
        Muros_i = Lista_i
        Calc_Pn_max()
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

    Sub Calc_Pn_max()

        Dim Ag As List(Of List(Of Double)) = New List(Of List(Of Double))
        Dim As_t As List(Of List(Of Double)) = New List(Of List(Of Double))
        Dim Fc As List(Of List(Of Double)) = New List(Of List(Of Double))
        Dim Fci As List(Of Double)
        Dim Agi As List(Of Double)
        Dim Asi As List(Of Double)
        Dim muroi As Muros_Consolidados
        Dim Pn_aux, Ag_total, Fc_total, fc_prom, As_total, As_aux, Phi_Pn_aux As Double
        Dim n As Double
        Pn_max = New List(Of Double)

#Region "Obtencion_de_datos"
        For i = 0 To Muros_i.Count - 1

            muroi = Muros_lista_2.Find(Function(x) x.Pier_name = Muros_i(i).NombreMuro)

            Asi = New List(Of Double)
            Agi = New List(Of Double)
            Fci = New List(Of Double)

            For j = 0 To muroi.Stories.Count - 1

                If refuerzo_lista.Find(Function(x) x.piername = muroi.Pier_name And x.pierstory = muroi.Stories(j)).total > 0 Then
                    As_aux = refuerzo_lista.Find(Function(x) x.piername = muroi.Pier_name And x.pierstory = muroi.Stories(j)).total
                Else
                    As_aux = muroi.Rho_l(j) * muroi.Bw(j) * muroi.lw(j)
                End If

                Asi.Add(As_aux)
                Agi.Add(muroi.Bw(j) * muroi.lw(j))
                Fci.Add(muroi.fc(j))
            Next

            As_t.Add(Asi)
            Fc.Add(Fci)
            Ag.Add(Agi)


        Next
#End Region

#Region "Calculo de area total de la seccion"

        For i = 0 To Ag(0).Count - 1

            Ag_total = 0
            As_total = 0
            Fc_total = 0
            n = 0

            For j = 0 To Ag.Count - 1

                Ag_total += Ag(j)(i)
                As_total += As_t(j)(i)
                Fc_total += Fc(j)(i)
                n += 1

            Next

            fc_prom = Fc_total / n
            Pn_aux = 0.85 * fc_prom * (Ag_total - As_total) + 4220 * As_total
            Phi_Pn_aux = 0.8 * 0.65 * Pn_aux
            Pn_max.Add(Pn_aux / 1000)
            Phi_Pn_max.Add(Phi_Pn_aux / 1000)
        Next

#End Region

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
