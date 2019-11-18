<Serializable>
Public Class Muros_Consolidados

#Region "Variables"

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
    ''Public Error_cuantia As List(Of String) = New List(Of String)

    'Nuevas variables
    Public Est_ebe As List(Of Integer) = New List(Of Integer)

    Public Sep_ebe As List(Of Double) = New List(Of Double)
    Public Est_Zc As List(Of Integer) = New List(Of Integer)
    Public Sep_Zc As List(Of Double) = New List(Of Double)
    Public As_Long As List(Of Double) = New List(Of Double)

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

    Public CantidaddeMallas_Fic As New List(Of Single)
    Public MallasIndv As New List(Of String)
    Public MallasConCantidad As New List(Of String)
    Public Desperdicio As New List(Of Single)
    Public CantidadMallasDllNet As New List(Of String)

    Public Peso_Long As List(Of Double) = New List(Of Double)
    Public Peso_Transv As List(Of Double) = New List(Of Double)
    Public Peso_malla As List(Of Double) = New List(Of Double)
    Public ReadOnly Volumen As List(Of Double) = New List(Of Double)

    Public Reduccion As Reduccion = Reduccion.NoAplica
    Public NombreBarras As New List(Of List(Of String))
    Public LongitudBarras As New List(Of List(Of Double))
    Public NoBarras As New List(Of Double)

#End Region

    Sub CantidadMallas_()

        Dim b_ME As Single = 2.35
        Dim h_ME As Single = 6
        Dim Traslapo As Single = 0.3
        Dim R As Single = 0.02
        Dim Area_piso As Double = 0
        Dim Area_Malla As New List(Of Single)
        Dim Sum_Traslapo As Single = 0
        MallasIndv.Clear()
        MallasConCantidad.Clear()

        For i = 0 To lw.Count - 1
            If (lw(i) / 100) - 2.45 < 0 Then
                Sum_Traslapo = 0
            Else
                Sum_Traslapo = (Math.Ceiling((lw(i) / 100) - 2.45 / 2.05)) * Traslapo
            End If
            Area_Malla.Add(((lw(i) / 100) + Sum_Traslapo) * ((Hw(i) / 100) + 0.3))
        Next

        For i = 0 To Malla.Count - 1
            Dim NoMallas As Integer = 0
            For n = 0 To Len(Malla(i)) - 1 : If Malla(i).Chars(n) = "D" Then : NoMallas = NoMallas + 1 : End If : Next
            Area_Malla(i) = Area_Malla(i) * NoMallas
        Next

        For i = 0 To Malla.Count - 1
            Dim NoMallas As Integer = 0

            For n = 0 To Len(Malla(i)) - 1 : If Malla(i).Chars(n) = "D" Then : NoMallas = NoMallas + 1 : End If : Next
            Dim MallaStr As String = Malla(i)
            If NoMallas = 0 Then
                MallaStr = ""
            End If
            If NoMallas = 2 Then
                MallaStr = Malla(i).Substring(NoMallas - 1)
            End If
            MallasIndv.Add(MallaStr)

        Next

        Dim VectoIndices As New List(Of Integer)

        For i = 0 To MallasIndv.Count - 1
            Dim AreaMalla As Single = Area_Malla(i)

            If VectoIndices.Exists(Function(x) x = i) = False Then

                For j = i + 1 To MallasIndv.Count - 1

                    If MallasIndv(i) = MallasIndv(j) Then
                        AreaMalla = AreaMalla + Area_Malla(j)
                        VectoIndices.Add(j)
                    End If
                Next
                MallasConCantidad.Add(AreaMalla & "-" & MallasIndv(i))
            End If

        Next

        'If CantidaddeMallas_Fic.Count = 0 Then
        '    For i = 0 To lw.Count - 1

        '        Area_piso = (lw(i) / 100) * ((Hw(i) / 100) + Traslapo)
        '        CantidaddeMallas_Fic.Add(Math.Ceiling(Area_piso / (b_ME * h_ME)))

        '        'If Hw(i) + 0.3 > h_ME Then
        '        '    CantidaddeMallas_Fic.Add((Math.Ceiling(((lw(i) / 100 - R - Traslapo) / b_ME) / 2)))
        '        'End If
        '    Next

        '    For i = 0 To Malla.Count - 1
        '        Dim NoMallas As Integer = 0
        '        For n = 0 To Len(Malla(i)) - 1 : If Malla(i).Chars(n) = "D" Then : NoMallas = NoMallas + 1 : End If : Next
        '        CantidaddeMallas_Fic(i) = CantidaddeMallas_Fic(i) * NoMallas

        '    Next

        '    For i = 0 To Malla.Count - 1
        '        Dim NoMallas As Integer = 0

        '        For n = 0 To Len(Malla(i)) - 1 : If Malla(i).Chars(n) = "D" Then : NoMallas = NoMallas + 1 : End If : Next
        '        Dim MallaStr As String = Malla(i)
        '        If NoMallas = 0 Then
        '            MallaStr = ""
        '        End If
        '        If NoMallas = 2 Then
        '            MallaStr = Malla(i).Substring(NoMallas - 1)
        '        End If
        '        MallasIndv.Add(MallaStr)

        '    Next

        '    Dim VectoIndices As New List(Of Integer)

        '    For i = 0 To MallasIndv.Count - 1
        '        Dim Cantidad As Integer = CantidaddeMallas_Fic(i)

        '        If VectoIndices.Exists(Function(x) x = i) = False Then

        '            For j = i + 1 To MallasIndv.Count - 1

        '                If MallasIndv(i) = MallasIndv(j) Then
        '                    Cantidad = Cantidad + CantidaddeMallas_Fic(j)
        '                    VectoIndices.Add(j)
        '                End If
        '            Next
        '            MallasConCantidad.Add(Cantidad & "-" & MallasIndv(i))
        '        End If

        '    Next

        '    For i = 0 To MallasConCantidad.Count - 1

        '        If MallasConCantidad(i) <> "0-" Then

        '            Dim NomenclaturaInicial As String = MallasConCantidad(i)
        '            Dim NomenclaturaFinal As String = ""
        '            Dim Cantidad As Integer = 0 : Dim PosicionDeRaya As Integer : Dim PosiciondeD As Integer = 0
        '            Dim No_Malla As Integer

        '            For n = 0 To Len(NomenclaturaInicial) - 1

        '                If NomenclaturaInicial.Chars(n) = "-" Then
        '                    PosicionDeRaya = n
        '                End If
        '                If NomenclaturaInicial.Chars(n) = "D" Then
        '                    PosiciondeD = n
        '                End If
        '            Next

        '            Cantidad = Val(NomenclaturaInicial.Substring(0, PosicionDeRaya))
        '            No_Malla = Val(NomenclaturaInicial.Substring(PosiciondeD + 1))

        '            NomenclaturaFinal = Cantidad & " " & "M" & " " & "D-" & No_Malla

        '            CantidadMallasDllNet.Add(NomenclaturaFinal)

        '        End If

        '    Next

        'End If

    End Sub

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

    Public Sub Reload_As_Long()

        Dim lw_def As Double

        For i = 0 To As_Long.Count - 1

            lw_def = lw(i) - Lebe_Izq(i) - Lebe_Der(i) - Lebe_Centro(i) - Zc_Der(i) - Zc_Izq(i)
            As_Long(i) -= lw_def * As_malla(Malla(i))

        Next

    End Sub


    Public Overrides Function Equals(obj As Object) As Boolean

        If obj IsNot Nothing Then

            Dim temp As Muros_Consolidados = CType(obj, Muros_Consolidados)

            If temp.MuroSimilar.Pier_name = Pier_name Then
                Return True
            End If
        End If

        Return False
    End Function

    Public Shared Operator =(S1 As Muros_Consolidados, S2 As Muros_Consolidados)

        Try
            Return S1.Equals(S2)
        Catch ex As Exception
            Return False
        End Try

    End Operator

    Public Shared Operator <>(S1 As Muros_Consolidados, S2 As Muros_Consolidados)

        Try
            Return Not S1.Equals(S2)
        Catch ex As Exception
            Return False
        End Try

    End Operator

End Class