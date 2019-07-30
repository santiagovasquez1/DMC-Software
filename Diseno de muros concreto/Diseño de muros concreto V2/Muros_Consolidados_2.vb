Public Class Muros_Consolidados

    Public Pier_name As String
    Public Stories As List(Of String) = New List(Of String)
    Public Bw As List(Of Single) = New List(Of Single)
    Public lw As List(Of Single) = New List(Of Single)
    Public Hw As List(Of Single) = New List(Of Single)
    Public H_acumulado As List(Of Single) = New List(Of Single)
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

    'Nuevas Propiedades

    Public isMuroMaestro As Boolean = False
    Public MuroSimilar As Muros_Consolidados

    Public Sub Calculo_H_acumulado()

        Dim Sum_H As Double

        For i = 0 To Stories.Count - 1
            H_acumulado.Add(0)
        Next

        For i = Stories.Count - 1 To 0 Step -1
            Sum_H += Hw(i)
            H_acumulado(i) = Sum_H
        Next

    End Sub

End Class
