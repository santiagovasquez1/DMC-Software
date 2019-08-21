<Serializable>
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

    Public DireccionCambioEspesor As String = "No Aplica"
    Public CantidaddeMallas_Fic As New List(Of Single)
    Public MallasIndv As New List(Of String)
    Public MallasConCantidad As New List(Of String)
    Public Desperdicio As New List(Of Single)
    Public CantidadMallasDllNet As New List(Of String)


    Sub CantidadMallas_()

        Dim b_ME As Single = 2.35
        Dim h_ME As Single = 6
        Dim Traslapo As Single = 0.3
        Dim R As Single = 0.02

        If CantidaddeMallas_Fic.Count = 0 Then
            For i = 0 To lw.Count - 1
                If Hw(i) + 0.3 > h_ME Then
                    CantidaddeMallas_Fic.Add((Math.Ceiling(((lw(i) / 100 - R - Traslapo) / b_ME) / 2)))
                End If
            Next


            For i = 0 To Malla.Count - 1
                Dim NoMallas As Integer = 0
                For n = 0 To Len(Malla(i)) - 1 : If Malla(i).Chars(n) = "D" Then : NoMallas = NoMallas + 1 : End If : Next
                CantidaddeMallas_Fic(i) = CantidaddeMallas_Fic(i) * NoMallas

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
                Dim Cantidad As Integer = CantidaddeMallas_Fic(i)


                If VectoIndices.Exists(Function(x) x = i) = False Then

                    For j = i + 1 To MallasIndv.Count - 1

                        If MallasIndv(i) = MallasIndv(j) Then
                            Cantidad = Cantidad + CantidaddeMallas_Fic(j)
                            VectoIndices.Add(j)
                        End If
                    Next
                    MallasConCantidad.Add(Cantidad & "-" & MallasIndv(i))
                End If

            Next



            For i = 0 To MallasConCantidad.Count - 1

                If MallasConCantidad(i) <> "0-" Then

                    Dim NomenclaturaInicial As String = MallasConCantidad(i)
                    Dim NomenclaturaFinal As String = ""
                    Dim Cantidad As Integer = 0 : Dim PosicionDeRaya As Integer : Dim PosiciondeD As Integer = 0
                    Dim No_Malla As Integer

                    For n = 0 To Len(NomenclaturaInicial) - 1

                        If NomenclaturaInicial.Chars(n) = "-" Then
                            PosicionDeRaya = n
                        End If
                        If NomenclaturaInicial.Chars(n) = "D" Then
                            PosiciondeD = n
                        End If
                    Next

                    Cantidad = Val(NomenclaturaInicial.Substring(0, PosicionDeRaya))
                    No_Malla = Val(NomenclaturaInicial.Substring(PosiciondeD + 1))


                    NomenclaturaFinal = Cantidad & " " & "M" & " " & "D-" & No_Malla


                    CantidadMallasDllNet.Add(NomenclaturaFinal)


                End If

            Next

        End If

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

End Class
