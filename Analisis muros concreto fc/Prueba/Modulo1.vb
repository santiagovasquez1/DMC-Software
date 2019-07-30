Imports System.IO 'esta libreria nos va a servir para poder activar el commandialog
Imports System.Data
Imports System.Data.OleDb
Imports System
Imports System.Collections
Imports System.Drawing.Graphics
Imports Microsoft.VisualBasic

Module Modulo1
    Sub cargar_excel()
        Dim myfiledialog As New OpenFileDialog()
        Dim sline As String
        Dim vector_texto As New ArrayList()
        Dim vector_texto2() As String
        Dim k, x As Integer

        With myfiledialog
            .Filter = "Archivos de texto |*.txt"
            .Title = "Abrir Archivo"
            .ShowDialog()
        End With

        Try

            Dim lector As New StreamReader(myfiledialog.FileName)

            If myfiledialog.FileName.ToString <> "" Then
                Do
                    sline = lector.ReadLine()
                    If Not sline Is Nothing Then
                        vector_texto.Add(sline)
                    End If
                Loop Until sLine Is Nothing
                lector.Close()
            k = 0
                For Each elemento As String In vector_texto

                    vector_texto2 = Split(elemento, vbTab)
                    If k > 1 And vector_texto2(0) <> "" Then
                        Dim muroi As Objeto_muro = New Objeto_muro()
                        ReDim Preserve piername(x)

                        muroi.pier = vector_texto2(0)
                        piername(x) = vector_texto2(0)
                        muroi.story = vector_texto2(1)
                        muroi.lw = vector_texto2(2) * 100
                        muroi.bw = vector_texto2(3) * 100
                        muroi.fc = vector_texto2(4)
                        muroi.pl = vector_texto2(6)
                        muroi.malla = vector_texto2(7)

                        If vector_texto2(5) >= 0.002 Then
                            muroi.pt = vector_texto2(5)
                        ElseIf vector_texto2(5) < 0.002 And muroi.malla <> "Sin Malla" Then
                            muroi.pt = 0.002
                            muroi.As_htal = (muroi.pt * 100 * muroi.bw) - As_malla(muroi.malla)
                        ElseIf vector_texto2(5) <= 0.002 And muroi.malla = "Sin Malla" And muroi.pl < 0.01 Then
                            muroi.pt = 0.002
                            muroi.As_htal = (muroi.pt * 100 * muroi.bw)
                        ElseIf muroi.malla = "Sin Malla" And vector_texto2(5) < 0.002 And muroi.pl >= 0.01 Then
                            muroi.pt = 0.002
                            muroi.As_htal = 0
                        End If

                        If muroi.pt > 0.0025 And muroi.malla <> "Sin Malla" Then
                            muroi.As_htal = (vector_texto2(5) * 100 * muroi.bw) - As_malla(muroi.malla)
                        ElseIf muroi.pt = 0.0025 And muroi.pl >= 0.01 OrElse muroi.pt = 0.0025 And muroi.malla <> "Sin Malla" Then
                            muroi.As_htal = 0
                        ElseIf muroi.malla = "Sin Malla" And muroi.pl < 0.01 And muroi.pt >= 0.0025 Then
                            muroi.As_htal = (vector_texto2(5) * 100 * muroi.bw)
                        End If


                        muroi.As_long = vector_texto2(9)
                        muroi.Ebe_izq = vector_texto2(13)
                        muroi.Ebe_der = vector_texto2(13)
                        muroi.Ebe_Centro = 0
                        muroi.ramas_centro = 0
                        muroi.ramas_izq = 0
                        muroi.ramas_centro = 0
                        muroi.zc_izq = vector_texto2(14)
                        muroi.zc_der = vector_texto2(14)
                        muroi.hw = vector_texto2(21) * 100
                        Muros_lista.Add(muroi)
                        x = x + 1
                    End If
                    k = k + 1
                Next
                MsgBox("Se han cargado los datos correctamente", MsgBoxStyle.Information, "Importado con exito")

        End If

        Catch ex As Exception

        End Try

    End Sub

    Sub cargar_lista_muros()
        Try
            Dim muros_distintos As IEnumerable(Of String) = piername.Distinct
            Dim j As Integer = 0
            For Each elemento As String In muros_distintos
                ReDim Preserve list_muros(j)
                list_muros(j) = elemento
                j = j + 1
            Next
        Catch ex As Exception

        End Try
    End Sub

    Sub guardar_como()
        Dim guardar_archivo As New SaveFileDialog()
        With guardar_archivo
            .Title = "Guardar archivo svg "
            .Filter = "Guardar Archivo |*.svg"
            .ShowDialog()
        End With
        Ruta_archivo = guardar_archivo.FileName

        Try
            Dim escritor As StreamWriter = New StreamWriter(guardar_archivo.FileName)
            escritor.WriteLine("Datos de muros")
            escritor.WriteLine("")
            If list_muros.Count <> 0 Then
                For Each elemento As Objeto_muro In Muros_lista
                    escritor.WriteLine(elemento.pier & vbTab & elemento.story & vbTab & elemento.bw & vbTab & elemento.lw & vbTab & elemento.hw & vbTab & elemento.fc & vbTab & elemento.pl & vbTab &
                                       elemento.pt & vbTab & elemento.malla & vbTab & elemento.As_long & vbTab & elemento.Ebe_izq & vbTab & elemento.Ebe_der & vbTab & elemento.Ebe_Centro & vbTab &
                                       elemento.Est_ebe & vbTab & elemento.Sep_ebe & vbTab & elemento.ramas_izq & vbTab & elemento.ramas_der & vbTab & elemento.ramas_centro & vbTab & elemento.zc_izq & vbTab &
                                       elemento.zc_der & vbTab & elemento.Est_zc & vbTab & elemento.Sep_zc & vbTab & elemento.As_htal & vbTab & elemento.Ref_htal & vbTab & elemento.capas_htal & vbTab & elemento.sep_htal & vbTab &
                                       elemento.As_Htal_Total)
                Next
            End If

            escritor.WriteLine("")
            escritor.WriteLine("Datos de Refuerzo Adicional")

            If refuerzo_lista.Count <> 0 Then
                For Each elemento As Refuerzo_muros In refuerzo_lista
                    escritor.WriteLine()
                    escritor.Write(elemento.piername & vbTab)
                    escritor.Write(elemento.pierstory & vbTab)
                    escritor.Write(elemento.rho & vbTab)
                    escritor.Write(elemento.Ebe_Izq & vbTab)
                    escritor.Write(elemento.Ebe_Der & vbTab)
                    escritor.Write(elemento.Zc_Izq & vbTab)
                    escritor.Write(elemento.Zc_Der & vbTab)
                    escritor.Write(elemento.as_req & vbTab)
                    escritor.Write(elemento.total & vbTab)
                    escritor.Write(elemento.porcentaje & vbTab)
                    For i = 0 To elemento.diametro.Count - 1
                        escritor.Write(elemento.diametro(i) & vbTab)
                        escritor.Write(elemento.cantidad(i) & vbTab)
                    Next
                Next
            End If

            escritor.WriteLine("")
            escritor.WriteLine("")
            escritor.WriteLine("Datos de alzado refuerzo longitudinal")

            If alzado_lista.Count <> 0 Then
                For Each elemento As alzado_muro In alzado_lista
                    escritor.WriteLine()
                    escritor.Write(elemento.pier & vbTab)
                    escritor.Write(elemento.story & vbTab)
                    For Each elemento2 As String In elemento.alzado
                        escritor.Write(elemento2 & vbTab)
                    Next
                Next

            End If

            escritor.Close() ' cerramos
        Catch ex As Exception

        End Try


    End Sub

    Sub guardar_proyecto()

        Dim guardar_archivo As New SaveFileDialog()
        Dim escritor As StreamWriter = New StreamWriter(Ruta_archivo)

        escritor.WriteLine("Datos de muros")
        escritor.WriteLine("")
        If list_muros.Count <> 0 Then
            For Each elemento As Objeto_muro In Muros_lista
                escritor.WriteLine(elemento.pier & vbTab & elemento.story & vbTab & elemento.bw & vbTab & elemento.lw & vbTab & elemento.hw & vbTab & elemento.fc & vbTab & elemento.pl & vbTab &
                                    elemento.pt & vbTab & elemento.malla & vbTab & elemento.As_long & vbTab & elemento.Ebe_izq & vbTab & elemento.Ebe_der & vbTab & elemento.Ebe_Centro & vbTab &
                                    elemento.Est_ebe & vbTab & elemento.Sep_ebe & vbTab & elemento.ramas_izq & vbTab & elemento.ramas_der & vbTab & elemento.ramas_centro & vbTab & elemento.zc_izq & vbTab &
                                    elemento.zc_der & vbTab & elemento.Est_zc & vbTab & elemento.Sep_zc & vbTab & elemento.As_htal & vbTab & elemento.Ref_htal & vbTab & elemento.capas_htal & vbTab & elemento.sep_htal & vbTab &
                                    elemento.As_Htal_Total)
            Next
        End If

        escritor.WriteLine("")
        escritor.WriteLine("Datos de Refuerzo Adicional")

        If refuerzo_lista.Count <> 0 Then
            For Each elemento As Refuerzo_muros In refuerzo_lista
                escritor.WriteLine()
                escritor.Write(elemento.piername & vbTab)
                escritor.Write(elemento.pierstory & vbTab)
                escritor.Write(elemento.rho & vbTab)
                escritor.Write(elemento.Ebe_Izq & vbTab)
                escritor.Write(elemento.Ebe_Der & vbTab)
                escritor.Write(elemento.Zc_Izq & vbTab)
                escritor.Write(elemento.Zc_Der & vbTab)
                escritor.Write(elemento.as_req & vbTab)
                escritor.Write(elemento.total & vbTab)
                escritor.Write(elemento.porcentaje & vbTab)
                For i = 0 To elemento.diametro.Count - 1
                    escritor.Write(elemento.diametro(i) & vbTab)
                    escritor.Write(elemento.cantidad(i) & vbTab)
                Next
            Next
        End If

        escritor.WriteLine("")
        escritor.WriteLine("")
        escritor.WriteLine("Datos de alzado refuerzo longitudinal")

        If alzado_lista.Count <> 0 Then
            For Each elemento As alzado_muro In alzado_lista
                escritor.WriteLine()
                escritor.Write(elemento.pier & vbTab)
                escritor.Write(elemento.story & vbTab)
                For Each elemento2 As String In elemento.alzado
                    escritor.Write(elemento2 & vbTab)
                Next
            Next

        End If

        escritor.Close() ' cerramos



    End Sub

    Sub validar_info1()

        Dim fila As Integer = Form1.Data_muros.Rows.Count - 1
        Dim j As Integer = 0

        If Form1.Data_muros.Rows.Count <> 0 Then
            For i = 0 To fila
                Dim muro1 As Objeto_muro = New Objeto_muro()
                j = 0
                muro1.pier = Form1.Data_muros.Rows(i).Cells(0).Value
                muro1.story = Form1.Data_muros.Rows(i).Cells(1).Value
                muro1.bw = Form1.Data_muros.Rows(i).Cells(2).Value
                muro1.lw = Form1.Data_muros.Rows(i).Cells(3).Value
                muro1.hw = Form1.Data_muros.Rows(i).Cells(4).Value
                muro1.fc = Form1.Data_muros.Rows(i).Cells(5).Value
                muro1.pl = Form1.Data_muros.Rows(i).Cells(6).Value
                muro1.pt = Form1.Data_muros.Rows(i).Cells(7).Value
                muro1.malla = Form1.Data_muros.Rows(i).Cells(8).Value
                muro1.As_long = Form1.Data_muros.Rows(i).Cells(9).Value

                muro1.Ebe_izq = Form1.Data_muros.Rows(i).Cells(10).Value
                muro1.Ebe_der = Form1.Data_muros.Rows(i).Cells(11).Value
                muro1.Ebe_Centro = Form1.Data_muros.Rows(i).Cells(12).Value

                muro1.Est_ebe = Form1.Data_muros.Rows(i).Cells(13).Value
                muro1.Sep_ebe = Form1.Data_muros.Rows(i).Cells(14).Value
                muro1.ramas_izq = Form1.Data_muros.Rows(i).Cells(15).Value
                muro1.ramas_der = Form1.Data_muros.Rows(i).Cells(16).Value
                muro1.ramas_centro = Form1.Data_muros.Rows(i).Cells(17).Value

                muro1.zc_izq = Form1.Data_muros.Rows(i).Cells(18).Value
                muro1.zc_der = Form1.Data_muros.Rows(i).Cells(19).Value
                muro1.Est_zc = Form1.Data_muros.Rows(i).Cells(20).Value
                muro1.Sep_zc = Form1.Data_muros.Rows(i).Cells(21).Value

                muro1.As_htal = Form1.Data_muros.Rows(i).Cells(22).Value
                muro1.Ref_htal = Form1.Data_muros.Rows(i).Cells(23).Value
                Try
                    muro1.capas_htal = Form1.Data_muros.Rows(i).Cells(24).Value
                    muro1.sep_htal = Form1.Data_muros.Rows(i).Cells(25).Value
                    muro1.As_Htal_Total = Form1.Data_muros.Rows(i).Cells(26).Value
                Catch ex As Exception
                    muro1.capas_htal = 0
                    muro1.sep_htal = 0
                    muro1.As_Htal_Total = 0
                End Try

                For Each elemento As Objeto_muro In Muros_lista
                    If elemento.pier = muro1.pier And elemento.story = muro1.story Then
                        Muros_lista.Item(j) = muro1
                        Exit For
                    End If
                    j = j + 1
                Next
            Next

            If refuerzo_lista.Count <> 0 Then
                Dim x = 0
                For i = 0 To refuerzo_lista.Count - 1
                    If Form1.LMuros.Text = refuerzo_lista(i).piername Then
                        refuerzo_lista(i).as_req = Form1.Data_muros.Rows(x).Cells(9).Value
                        x = x + 1
                    End If
                Next
            End If
        End If

    End Sub

    Sub validar_info2()
        Dim x, menc, ini, menc1, ini1 As Integer
        If f_alzado.Data_info.Rows.Count <> 0 Then
            If f_alzado.Data_Alzado.Rows.Count <> 0 Then
                For i = 0 To f_alzado.Data_Alzado.Rows.Count - 1
                    menc = 0
                    ini = 0
                    Dim alzadoi As alzado_muro = New alzado_muro()
                    alzadoi.pier = f_alzado.Data_Alzado.Rows(i).Cells(0).Value
                    alzadoi.story = f_alzado.Data_Alzado.Rows(i).Cells(1).Value
                    For j = 2 To f_alzado.Data_Alzado.Columns.Count - 1
                        alzadoi.alzado.Add(f_alzado.Data_Alzado.Rows(i).Cells(j).Value)
                    Next

                    x = 0
                    If alzado_lista.Count <> 0 Then
                        For Each elemento1 As alzado_muro In alzado_lista
                            If elemento1.pier = alzadoi.pier And elemento1.story = alzadoi.story Then
                                alzado_lista.Item(x) = alzadoi
                                menc = 1
                                Exit For
                            End If
                            x = x + 1
                        Next
                    Else
                        alzado_lista.Add(alzadoi)
                        ini = 1
                    End If

                    If alzado_lista.Count <> 0 And menc = 0 And ini = 0 Then
                        alzado_lista.Add(alzadoi)
                    End If
                Next
            End If

            If f_alzado.Data_info.Rows.Count <> 0 Then
                Dim pos As Integer
                For i = 0 To f_alzado.Data_Alzado.Rows.Count - 1
                    menc1 = 0
                    ini1 = 0
                    Dim refuerzoi As New Refuerzo_muros
                    refuerzoi.piername = f_alzado.Data_info.Rows(i).Cells(0).Value
                    refuerzoi.pierstory = f_alzado.Data_info.Rows(i).Cells(1).Value
                    refuerzoi.rho = f_alzado.Data_info.Rows(i).Cells(2).Value

                    If f_alzado.Data_info.Rows(i).Cells(3).Style.BackColor = Color.DarkGreen Then
                        refuerzoi.Ebe_Izq = f_alzado.Data_info.Rows(i).Cells(3).Value
                    Else
                        refuerzoi.Ebe_Izq = 0
                    End If

                    If f_alzado.Data_info.Rows(i).Cells(4).Style.BackColor = Color.DarkGreen Then
                        refuerzoi.Ebe_Der = f_alzado.Data_info.Rows(i).Cells(4).Value
                    Else
                        refuerzoi.Ebe_Der = 0
                    End If

                    If f_alzado.Data_info.Rows(i).Cells(3).Style.BackColor = Color.LightGreen Then
                        refuerzoi.Zc_Izq = f_alzado.Data_info.Rows(i).Cells(3).Value
                    Else
                        refuerzoi.Zc_Izq = 0
                    End If

                    If f_alzado.Data_info.Rows(i).Cells(4).Style.BackColor = Color.LightGreen Then
                        refuerzoi.Zc_Der = f_alzado.Data_info.Rows(i).Cells(4).Value
                    Else
                        refuerzoi.Zc_Der = 0
                    End If

                    refuerzoi.as_req = f_alzado.Data_info.Rows(i).Cells(5).Value
                    refuerzoi.total = f_alzado.Data_info.Rows(i).Cells(6).Value
                    pos = InStr(f_alzado.Data_info.Rows(i).Cells(7).Value, "%")
                    refuerzoi.porcentaje = Mid(f_alzado.Data_info.Rows(i).Cells(7).Value, 1, pos - 1)
                    For j = 8 To f_alzado.Data_info.ColumnCount - 1
                        refuerzoi.diametro.Add(Int(Mid(f_alzado.Data_info.Columns(j).HeaderText, 2)))
                        refuerzoi.cantidad.Add(f_alzado.Data_info.Rows(i).Cells(j).Value)
                    Next

                    x = 0
                    If refuerzo_lista.Count <> 0 Then
                        For Each elemento1 As Refuerzo_muros In refuerzo_lista
                            If elemento1.piername = refuerzoi.piername And elemento1.pierstory = refuerzoi.pierstory Then
                                refuerzo_lista.Item(x) = refuerzoi
                                menc1 = 1
                                Exit For
                            End If
                            x = x + 1
                        Next
                    Else
                        refuerzo_lista.Add(refuerzoi)
                        ini1 = 1
                    End If

                    If refuerzo_lista.Count <> 0 And menc1 = 0 And ini1 = 0 Then
                        refuerzo_lista.Add(refuerzoi)
                    End If
                Next
            End If
        End If
    End Sub

End Module
