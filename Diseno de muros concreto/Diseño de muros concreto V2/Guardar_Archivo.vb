Imports System.IO

Public Class Guardar_Archivo

    Dim Ruta As String
    Dim Lista_Texto As List(Of String)

    Public Sub New(ByVal Ruta_archivo As String, ByVal Data_alzado As DataGridView)

        Ruta = Ruta_archivo



        If Muros_lista_2 IsNot Nothing Then
            Actualizar_Resumen()

            Try
                Add_Refuerzoi(data_info_f3)
                Actualizar_Info_ref()
                Actualizar_Alzado()
                Actualizar_long_Alzados()
                Sobre_Escribir()
            Catch ex As Exception
                Actualizar_Info_ref()
                Actualizar_Alzado()
                Actualizar_long_Alzados()
                Sobre_Escribir()
            End Try

        End If

    End Sub

    Sub Actualizar_Resumen()

        Lista_Texto = New List(Of String)
        Dim Texto As String

        For Each muro_i As Muros_Consolidados In Muros_lista_2

            For i = 0 To muro_i.Stories.Count - 1
                Texto = muro_i.Stories(i) & vbTab & muro_i.Pier_name & vbTab & muro_i.lw(i) & vbTab & muro_i.Bw(i) & vbTab & muro_i.fc(i) & vbTab & muro_i.Rho_T(i) & vbTab & muro_i.Rho_l(i) & vbTab & muro_i.Malla(i) & vbTab & muro_i.C_Def(i) & vbTab & muro_i.Lebe_Izq(i) & vbTab & muro_i.Lebe_Der(i) & vbTab & muro_i.Lebe_Centro(i) & vbTab & muro_i.Zc_Izq(i) & vbTab & muro_i.Zc_Der(i) & vbTab & muro_i.Hw(i) & vbTab & muro_i.Est_ebe(i) & vbTab & muro_i.Sep_ebe(i) & vbTab & muro_i.Est_Zc(i) & vbTab & muro_i.Sep_Zc(i) & vbTab & muro_i.As_Long(i) & vbTab & muro_i.ramas_izq(i) & vbTab & muro_i.ramas_der(i) & vbTab & muro_i.ramas_centro(i) & vbTab & muro_i.As_htal(i) & vbTab & muro_i.Ref_htal(i) & vbTab & muro_i.Capas_htal(i) & vbTab & muro_i.sep_htal(i) & vbTab & muro_i.As_Htal_Total(i)
                Lista_Texto.Add(Texto)
            Next

        Next

    End Sub

    Sub Add_Refuerzoi(ByVal Data_info As DataGridView)

        Dim Datos_refuerzo As Refuerzo_muros
        Dim indice As Integer
        Dim suma As Double

        For i = 0 To Data_info.Rows.Count - 1
            Datos_refuerzo = New Refuerzo_muros With {
                .piername = Data_info.Rows(i).Cells(0).Value,
                .pierstory = Data_info.Rows(i).Cells(1).Value,
                .bw = Data_info.Rows(i).Cells(2).Value,
                .as_req = Data_info.Rows(i).Cells(6).Value
            }

            For j = 9 To Data_info.ColumnCount - 1
                Datos_refuerzo.diametro.Add(Int(Mid(Data_info.Columns(j).HeaderText, 2)))
                Datos_refuerzo.cantidad.Add(Data_info.Rows(i).Cells(j).Value)
            Next

            suma = 0
            For j = 0 To Datos_refuerzo.diametro.Count - 1
                suma = suma + areas_refuerzo(Datos_refuerzo.diametro(j)) * Datos_refuerzo.cantidad(j)
            Next

            Datos_refuerzo.total = suma
            Datos_refuerzo.porcentaje = Datos_refuerzo.total / Datos_refuerzo.as_req

            If refuerzo_lista.Count = 0 Or refuerzo_lista.Exists(Function(x) x.piername = Datos_refuerzo.piername And x.pierstory = Datos_refuerzo.pierstory) = False Then
                refuerzo_lista.Add(Datos_refuerzo)
            Else
                indice = refuerzo_lista.FindIndex(Function(x) x.piername = Datos_refuerzo.piername And x.pierstory = Datos_refuerzo.pierstory)
                refuerzo_lista(indice) = Datos_refuerzo
            End If

        Next

    End Sub

    Sub Actualizar_Info_ref()

        Dim Texto As String

        For Each muro_i As Refuerzo_muros In refuerzo_lista

            Texto = muro_i.piername & vbTab & muro_i.pierstory & vbTab & muro_i.bw & vbTab & muro_i.rho & vbTab & muro_i.as_req & vbTab

            For j = 0 To muro_i.diametro.Count - 1
                If j < muro_i.diametro.Count - 1 Then
                    Texto = Texto & muro_i.diametro(j) & vbTab & muro_i.cantidad(j) & vbTab
                Else
                    Texto = Texto & muro_i.diametro(j) & vbTab & muro_i.cantidad(j)
                End If
            Next

            Lista_Texto.Add(Texto)
        Next

    End Sub

    Sub Actualizar_Alzado()
        Dim texto As String
        Dim indice As Integer

        alzado_lista.OrderBy(Function(x) x.pier)
        alzado_lista.OrderBy(Function(x) x.story)
        indice = Lista_Texto.FindIndex(Function(x) x.Contains("Datos de alzado refuerzo longitudinal"))

        If indice < 0 Then
            Lista_Texto.Add("7.Datos de alzado refuerzo longitudinal")
        End If

        For Each Alzado_i As alzado_muro In alzado_lista

            texto = Alzado_i.pier & vbTab & Alzado_i.story & vbTab

            For j = 0 To Alzado_i.alzado.Count - 1
                If j < Alzado_i.alzado.Count - 1 Then
                    texto = texto & Alzado_i.alzado(j) & vbTab
                Else
                    texto = texto & Alzado_i.alzado(j)
                End If
            Next
            Lista_Texto.Add(texto)
        Next
    End Sub

    Sub Actualizar_long_Alzados()

        Dim texto, texto2 As String
        Dim indice, pos As Integer

        alzado_lista.OrderBy(Function(x) x.pier)
        alzado_lista.OrderBy(Function(x) x.story)
        indice = Lista_Texto.FindIndex(Function(x) x.Contains("8.Datos de alzado refuerzo longitudinal - Longitud"))

        If indice < 0 Then
            Lista_Texto.Add("8.Datos de alzado refuerzo longitudinal - Longitud")
        End If

        For Each Alzado_i As alzado_muro In alzado_lista

            texto = Alzado_i.pier & vbTab & Alzado_i.story & vbTab

            For j = 0 To Alzado_i.Alzado_Longitud.Count - 1
                pos = Alzado_i.Alzado_Longitud(j).IndexOf("L=") + 2

                If pos > 2 Then
                    texto2 = Alzado_i.Alzado_Longitud(j).Substring(pos)
                Else
                    texto2 = Alzado_i.Alzado_Longitud(j)
                End If

                If j < Alzado_i.Alzado_Longitud.Count - 1 Then
                    texto = texto & texto2 & vbTab
                Else
                    texto = texto & texto2
                End If
            Next
            Lista_Texto.Add(texto)
        Next

    End Sub

    Sub Sobre_Escribir()

        Dim Inicio, Fin As Integer
        Dim Vector_Texto_aux As New List(Of String)
        Dim sline As String
        Dim X1, indice As Integer
        Dim Lector As New StreamReader(Ruta_1)
        Dim Escritor As StreamWriter

        Do
            sline = Lector.ReadLine()
            Vector_Texto_aux.Add(sline)

        Loop Until sline Is Nothing
        Lector.Close()

        Inicio = Vector_Texto_aux.FindIndex(Function(x) x.Contains("5.Reporte")) + 2

        ''Guardar resumen de diseño
        Try
            Fin = Vector_Texto_aux.FindIndex(Function(x) x.Contains("6.Datos de Refuerzo Adicional")) - 2
        Catch ex As Exception
            Fin = Vector_Texto_aux.FindIndex(Function(x) x.Contains("Fin")) - 2
        End Try

        For i = Inicio To Fin
            Vector_Texto_aux(i) = Lista_Texto(X1)
            X1 += 1
        Next

        ''
        Vector_Texto_aux(Fin + 2) = "6.Datos de Refuerzo Adicional"
        indice = Lista_Texto.FindIndex(Function(x) x.Contains("7.Datos de alzado refuerzo longitudinal"))

        For i = X1 To indice - 1
            Vector_Texto_aux.Add(Lista_Texto(i))
            X1 += 1
        Next

        Vector_Texto_aux.AddRange({"", "7.Datos de alzado refuerzo longitudinal", ""})
        indice = Lista_Texto.FindIndex(Function(x) x.Contains("8.Datos de alzado refuerzo longitudinal - Longitud"))

        For i = X1 + 1 To indice - 1
            Vector_Texto_aux.Add(Lista_Texto(i))
            X1 += 1
        Next


        Vector_Texto_aux.AddRange({"", "8.Datos de alzado refuerzo longitudinal - Longitud", ""})

        For i = indice + 1 To Lista_Texto.Count - 1
            Vector_Texto_aux.Add(Lista_Texto(i))
        Next

        Vector_Texto_aux.AddRange({"", "Fin"})
        Escritor = New StreamWriter(Ruta_1)

        For i = 0 To Vector_Texto_aux.Count - 1
            Escritor.WriteLine(Vector_Texto_aux(i))
        Next


        Escritor.Close()

    End Sub

End Class
