Imports System.IO

Module Datos__refuerzo_long
    Public Pesos_Refuerzos As New List(Of Refuerzo_procesar)
    Public Lista_texto_refuerzo As List(Of String)
    Public Class Refuerzo_procesar

        Public Pier, Story As String
        Public bw, lw, hw, fc As Single
        Public Long_ref As Single
        Public Peso_barra, Peso_traslapo As Double
        Public rho As Double
    End Class

    Public Function Traslapo(i As Integer, k As Integer, fc As Double, diametro As Integer) As Single
        Traslapo = 0
        Try
            If fc = 560 Then
                Traslapo = traslapo_560(diametro)
            End If

            If fc = 490 Then
                Traslapo = traslapo_490(diametro)
            End If

            If fc = 420 Then
                Traslapo = traslapo_420(diametro)
            End If

            If fc = 350 Then
                Traslapo = traslapo_350(diametro)
            End If

            If fc = 280 Then
                Traslapo = traslapo_280(diametro)
            End If

            If fc = 210 Then
                Traslapo = traslapo_210(diametro)
            End If
        Catch ex As Exception
            Traslapo = 0
        End Try


    End Function

    Public Sub Procesar_info(ByVal Datos_Alzados_procesar As List(Of Refuerzo))
        Dim refuerzo_i As Refuerzo_procesar

        For i = 0 To Datos_Alzados_procesar.Count - 1
            If Datos_Alzados_procesar(i).cantidad > 0 Then
                refuerzo_i = New Refuerzo_procesar With {
                    .Pier = Datos_Alzados_procesar(i).pier,
                    .Story = Datos_Alzados_procesar(i).story,
                    .fc = Datos_Alzados_procesar(i).fc,
                    .hw = Datos_Alzados_procesar(i).hw,
                    .bw = Datos_Alzados_procesar(i).bw,
                    .rho = Datos_Alzados_procesar(i).rho,
                    .lw = Datos_Alzados_procesar(i).lw
                    }

                With refuerzo_i
                    If Datos_Alzados_procesar(i).tipo_traslapo <> "T0" And Datos_Alzados_procesar(i).tipo_traslapo <> "T3" Then
                        .Long_ref = .hw + (Traslapo(i, 1, .fc, Datos_Alzados_procesar(i).diametro) * 100)
                    Else
                        .Long_ref = .hw
                    End If
                    .Peso_barra = Datos_Alzados_procesar(i).cantidad * .Long_ref * areas_refuerzo(Datos_Alzados_procesar(i).diametro) * (7850 / Math.Pow(100, 3))
                    .Peso_traslapo = Datos_Alzados_procesar(i).cantidad * (Traslapo(i, 1, .fc, Datos_Alzados_procesar(i).diametro) * 100) * areas_refuerzo(Datos_Alzados_procesar(i).diametro) * (7850 / Math.Pow(100, 3))
                End With
                Pesos_Refuerzos.Add(refuerzo_i)
            End If

        Next

    End Sub

    Public Sub Procesar_Info_2()

        Dim Espesores_distintos = From c In Pesos_Refuerzos Select New With {Key c.bw} Distinct.ToList
        Dim cuantias_Distintas = From c In Pesos_Refuerzos Select New With {Key c.rho} Distinct.ToList
        Dim Rango, Paso As Double
        Dim Rango_inf, Rango_Sup, Suma_peso, Suma_traslapo, Relacion As Double
        Dim numero_intervalos As Integer
        Dim T_pesos As List(Of Refuerzo_procesar)
        Dim T2_pesos As List(Of Refuerzo_procesar)

        Rango = cuantias_Distintas.Max(Function(x) x.rho) - cuantias_Distintas.Min(Function(x) x.rho)
        numero_intervalos = 20
        Paso = Rango / numero_intervalos
        Rango_inf = cuantias_Distintas.Min(Function(x) x.rho)
        Rango_Sup = Rango_inf + Paso
        Lista_texto_refuerzo = New List(Of String)

        For i = 0 To cuantias_Distintas.Count - 1

            T_pesos = Pesos_Refuerzos.FindAll(Function(x) x.rho = cuantias_Distintas(i).rho)

            For j = 0 To Espesores_distintos.Count - 1

                Suma_peso = 0
                Suma_traslapo = 0
                T2_pesos = T_pesos.FindAll(Function(x) x.bw = Espesores_distintos(j).bw)
                Suma_peso = T2_pesos.Sum(Function(x) x.Peso_barra)
                Suma_traslapo = T2_pesos.Sum(Function(x) x.Peso_traslapo)
                Relacion = Suma_traslapo / Suma_peso
                If Suma_peso > 0 Then
                    'Lista_texto_refuerzo.Add("(" & Rango_inf & "-" & Rango_Sup & ")" & vbTab & Espesores_distintos(j).bw & vbTab & Suma_peso & vbTab & Suma_traslapo & vbTab & Relacion)
                    Lista_texto_refuerzo.Add(cuantias_Distintas(i).rho & vbTab & Espesores_distintos(j).bw & vbTab & Suma_peso & vbTab & Suma_traslapo & vbTab & Relacion)
                End If
            Next
            'Rango_inf = Rango_Sup
            'Rango_Sup = Rango_Sup + Paso
        Next

    End Sub

    Public Sub Generar_informe()

        Dim guardar_archivo As New SaveFileDialog()
        Dim Ruta_2 As String
        With guardar_archivo
            .Title = "Guardar archivo txt"
            .Filter = "Guardar Archivo |*.txt"
            .ShowDialog()
        End With
        Ruta_2 = guardar_archivo.FileName

        Try
            Dim escritor As StreamWriter = New StreamWriter(guardar_archivo.FileName)
            escritor.WriteLine("Cuantia" & vbTab & "Bw" & vbTab & "Peso Total" & vbTab & "Peso Traslapo" & vbTab & "Relacion")

            For i = 0 To Lista_texto_refuerzo.Count - 1
                escritor.WriteLine(Lista_texto_refuerzo(i))
            Next
            escritor.Close()
        Catch ex As Exception

        End Try

    End Sub
End Module
