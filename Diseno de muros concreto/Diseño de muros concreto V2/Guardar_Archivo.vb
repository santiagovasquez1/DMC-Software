Imports System.IO

Public Class Guardar_Archivo

    Dim Ruta As String
    Dim Lista_Texto As List(Of String)

    Public Sub New(ByVal Ruta_archivo As String, ByVal Borrar As Boolean)

        Ruta = Ruta_archivo

        If Muros_lista_2 IsNot Nothing Then
            Actualizar_Resumen()

            Try
                Add_Refuerzoi(data_info_f3)
                Actualizar_Resumen()
                Actualizar_Info_ref()
                Actualizar_Alzado()
                Actualizar_long_Alzados()
                Sobre_Escribir(Borrar)
                NuevosArchivo()
            Catch ex As Exception
                Actualizar_Resumen()
                Actualizar_Info_ref()
                Actualizar_Alzado()
                Actualizar_long_Alzados()
                Sobre_Escribir(Borrar)
                NuevosArchivo()
            End Try

        End If

    End Sub

    Public Shared Sub Actualizar_Resumen()

        For i = 0 To Muros_lista_2.Count - 1
            If IsNothing(Muros_lista_2(i).MuroSimilar) = False Then

                Muros_lista_2(i).lw = Muros_lista_2(i).MuroSimilar.lw
                Muros_lista_2(i).Malla = Muros_lista_2(i).MuroSimilar.Malla
                Muros_lista_2(i).As_Long = Muros_lista_2(i).MuroSimilar.As_Long
                Muros_lista_2(i).As_htal = Muros_lista_2(i).MuroSimilar.As_htal
                Muros_lista_2(i).Lebe_Izq = Muros_lista_2(i).MuroSimilar.Lebe_Izq
                Muros_lista_2(i).Lebe_Der = Muros_lista_2(i).MuroSimilar.Lebe_Der
                Muros_lista_2(i).Lebe_Centro = Muros_lista_2(i).MuroSimilar.Lebe_Centro
                Muros_lista_2(i).Est_ebe = Muros_lista_2(i).MuroSimilar.Est_ebe
                Muros_lista_2(i).Sep_ebe = Muros_lista_2(i).MuroSimilar.Sep_ebe
                Muros_lista_2(i).ramas_izq = Muros_lista_2(i).MuroSimilar.ramas_izq
                Muros_lista_2(i).ramas_der = Muros_lista_2(i).MuroSimilar.ramas_der
                Muros_lista_2(i).ramas_centro = Muros_lista_2(i).MuroSimilar.ramas_centro
                Muros_lista_2(i).Zc_Izq = Muros_lista_2(i).MuroSimilar.Zc_Izq
                Muros_lista_2(i).Zc_Der = Muros_lista_2(i).MuroSimilar.Zc_Der
                Muros_lista_2(i).Est_Zc = Muros_lista_2(i).MuroSimilar.Est_Zc
                Muros_lista_2(i).Sep_Zc = Muros_lista_2(i).MuroSimilar.Sep_Zc
                Muros_lista_2(i).Ref_htal = Muros_lista_2(i).MuroSimilar.Ref_htal
                Muros_lista_2(i).Capas_htal = Muros_lista_2(i).MuroSimilar.Capas_htal
                Muros_lista_2(i).sep_htal = Muros_lista_2(i).MuroSimilar.sep_htal
            End If

        Next

    End Sub

    Public Shared Sub Add_Refuerzoi(ByVal Data_info As DataGridView)

        Dim Datos_refuerzo, datos_refuerzo_hijo As Refuerzo_muros
        Dim indice, indice2 As Integer
        Dim suma As Double
        Dim Muro_i As Muros_Consolidados
        Dim Muros_hijos As New List(Of Muros_Consolidados)

        Muro_i = Muros_lista_2.Find(Function(x) x.Pier_name = Data_info.Rows(0).Cells(0).Value)
        Find_Muros_Hijos(Muro_i, Muros_hijos)

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
                suma += areas_refuerzo(Datos_refuerzo.diametro(j)) * Datos_refuerzo.cantidad(j)
            Next

            Datos_refuerzo.total = suma
            Datos_refuerzo.porcentaje = Datos_refuerzo.total / Datos_refuerzo.as_req

            If refuerzo_lista.Count = 0 Or refuerzo_lista.Exists(Function(x) x.piername = Datos_refuerzo.piername And x.pierstory = Datos_refuerzo.pierstory) = False Then
                refuerzo_lista.Add(Datos_refuerzo)
            Else
                indice = refuerzo_lista.FindIndex(Function(x) x.piername = Datos_refuerzo.piername And x.pierstory = Datos_refuerzo.pierstory)
                refuerzo_lista(indice).cantidad = Datos_refuerzo.cantidad
                refuerzo_lista(indice).diametro = Datos_refuerzo.diametro
                refuerzo_lista(indice).total = Datos_refuerzo.total
                refuerzo_lista(indice).porcentaje = Datos_refuerzo.porcentaje
            End If

            If Muro_i.isMuroMaestro = True Then
                If Muros_hijos.Count > 0 Then

                    For k = 0 To Muros_hijos.Count - 1

                        datos_refuerzo_hijo = New Refuerzo_muros With {
                        .piername = Muros_hijos(k).Pier_name,
                        .pierstory = Data_info.Rows(i).Cells(1).Value,
                        .bw = Data_info.Rows(i).Cells(2).Value,
                        .as_req = Data_info.Rows(i).Cells(6).Value
                        }

                        datos_refuerzo_hijo.cantidad = Datos_refuerzo.cantidad
                        datos_refuerzo_hijo.diametro = Datos_refuerzo.diametro

                        datos_refuerzo_hijo.total = Datos_refuerzo.total
                        datos_refuerzo_hijo.porcentaje = Datos_refuerzo.total / datos_refuerzo_hijo.as_req

                        If refuerzo_lista.Count = 0 Or refuerzo_lista.Exists(Function(x) x.piername = datos_refuerzo_hijo.piername And x.pierstory = datos_refuerzo_hijo.pierstory) = False Then
                            refuerzo_lista.Add(datos_refuerzo_hijo)
                        Else
                            indice = refuerzo_lista.FindIndex(Function(x) x.piername = datos_refuerzo_hijo.piername And x.pierstory = datos_refuerzo_hijo.pierstory)
                            refuerzo_lista(indice).cantidad = datos_refuerzo_hijo.cantidad
                            refuerzo_lista(indice).diametro = datos_refuerzo_hijo.diametro
                            refuerzo_lista(indice).total = datos_refuerzo_hijo.total
                            refuerzo_lista(indice).porcentaje = datos_refuerzo_hijo.porcentaje
                        End If

                    Next
                End If

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

        alzado_lista = alzado_lista.OrderBy(Function(x) x.pier).ToList
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

    Sub Sobre_Escribir(ByVal Borrar As Boolean)

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

        Vector_Texto_aux(Fin + 2) = "6.Datos de Refuerzo Adicional"
        Vector_Texto_aux(Fin + 3) = ""
        indice = Lista_Texto.FindIndex(Function(x) x.Contains("7.Datos de alzado refuerzo longitudinal"))

        If Borrar = True Then
            Dim numero_Elementos As Integer
            numero_Elementos = Vector_Texto_aux.Count - 1 - (Fin + 3)
            Vector_Texto_aux.RemoveRange(Fin + 4, numero_Elementos)
            indice = Lista_Texto.FindIndex(Function(x) x.Contains("7.Datos de alzado refuerzo longitudinal"))
        End If

        'Vector_Texto_aux.RemoveRange()

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

    Sub NuevosArchivo()
        Dim NombreArchivoIguales As String = "\thesames.SDMC"
        Dim Escritor = New StreamWriter(Ruta_Carpeta & NombreArchivoIguales)

        For Each muro_i As Muros_Consolidados In Muros_lista_2
            Dim NombreMuroSimilar, Maestro, CreadoDespues As String

            If muro_i.MuroSimilar IsNot Nothing Then
                NombreMuroSimilar = muro_i.MuroSimilar.Pier_name
            Else
                NombreMuroSimilar = "SinSimilar"
            End If
            If muro_i.isMuroMaestro = True Then
                Maestro = "True"
            Else
                Maestro = "False"
            End If

            If alzado_lista.Exists(Function(x) x.pier = muro_i.Pier_name) Then
                If alzado_lista.Find(Function(x) x.pier = muro_i.Pier_name).MuroCreadoDespues = True Then
                    CreadoDespues = True
                Else
                    CreadoDespues = False
                End If
            Else
                CreadoDespues = False
            End If

            Escritor.WriteLine(muro_i.Pier_name & vbTab & Maestro & vbTab & NombreMuroSimilar & vbTab & CreadoDespues)
        Next

        Escritor.Close()

    End Sub

    Private Shadows Sub Add_Similare(ByVal Muro_i As Muros_Consolidados, ByVal Story As String)

        If Muro_i.isMuroMaestro = True Then

        End If

    End Sub

End Class