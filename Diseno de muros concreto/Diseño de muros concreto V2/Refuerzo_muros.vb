<Serializable>
Public Class Refuerzo_muros
    Public piername As String
    Public pierstory As String
    Public bw As Double
    Public rho As Double
    Public as_req As Double
    Public cantidad As New List(Of Integer)
    Public diametro As New List(Of Integer)
    Public total As Double
    Public porcentaje As Double
    Public Ebe_Izq As Double
    Public Ebe_Der As Double
    Public Zc_Izq As Double
    Public Zc_Der As Double

    'Nuevas Variables
    Public IsMuroMaestro As Boolean = False
    Public MuroSimilar As Refuerzo_muros
    Public MuroCreadoDespues As Boolean = False



    Sub Calculo_Porcentaje()

        Dim Auxiliar As Double
        total = 0

        For i = 0 To cantidad.Count - 1
            Auxiliar = cantidad(i) * areas_refuerzo(diametro(i))
            total = total + Auxiliar
        Next

        If total > 0 And as_req > 0 Then
            porcentaje = total / as_req
        Else
            porcentaje = 0
        End If

    End Sub
End Class
