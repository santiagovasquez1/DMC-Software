Imports B_Procesar_Info
Imports Excel = Microsoft.Office.Interop.Excel

Public Class ExportExcel
    Private Ruta_archivo_1 As String
    Private m_Excel As Excel.Application
    Private objLibroExcel As Excel.Workbook
    Private objHojaExcel As Excel.Worksheet
    'Private Errores As CProcesar

    Sub Set_errores()

        'Errores = New CProcesar

        'For i = 0 To Muros_lista_2.Count - 1
        '    For j = 0 To Muros_lista_2(i).Stories.Count - 1

        '        Dim temp = Muros_generales.Find(Function(x1) x1.Pier = Muros_lista_2(i).Pier_name And x1.Story = Muros_lista_2(i).Stories(j))
        '        temp.Calc_pc()
        '        With Muros_lista_2(i)

        '            Errores.Set_errores_muros(.Pier_name, .Stories(j), .Rho_l(j), .Bw(j), .Lebe_Izq(j), .Lebe_Der(j), Math.Abs(temp.P.Min), temp.Pc)

        '        End With

        '    Next
        'Next

    End Sub

    Sub Deserializar()
        Dim Lista_i As New Listas_serializadas

        If Muros_lista_2 Is Nothing Then
            Muros_lista_2 = New List(Of Muros_Consolidados)
            Serializador.Deserializar(Ruta_1, Lista_i)
        End If

        Muros_lista_2 = Muros_lista_2.OrderBy(Function(x) x.Pier_name).ToList()
        Muros_generales = Muros_generales.OrderBy(Function(x) x.Pier).ToList()

    End Sub

    Sub Exportar(ByVal Route_File As String)
        'Ruta_archivo_1 = Route_File
        'If Ruta_archivo_1 <> "" Then

        '    Deserializar()
        '    m_Excel = New Excel.Application
        '    objLibroExcel = m_Excel.Workbooks.Add()
        '    objHojaExcel = objLibroExcel.Worksheets(1)

        '    Set_errores()
        '    If Errores.Muros_errores.Count > 0 Then
        '        objHojaExcel = objLibroExcel.Worksheets.Add()
        '        ExportarExcel_Reporte(True)
        '    End If

        '    objHojaExcel = objLibroExcel.Worksheets.Add()
        '    ExportarExcel_Reporte(False)

        '    objHojaExcel = objLibroExcel.Worksheets.Add()
        '    ExportarExcel_FlexuralStress()

        '    objHojaExcel = objLibroExcel.Worksheets.Add()
        '    ExportarExcel_ShearDesing()

        '    m_Excel.Windows(1).DisplayGridlines = False
        '    m_Excel.Visible = True
        'Else
        '    MsgBox("Proyecto sin Salvar", MsgBoxStyle.Exclamation, "efe Prima Ce")

        'End If
    End Sub

    Private Sub ExportarExcel_Reporte(ByVal Condicion As Boolean)
        'objHojaExcel.Activate()

        'If Condicion = False Then
        '    objHojaExcel.Name = "3.Report"
        'Else
        '    objHojaExcel.Name = "4.Errors"
        'End If

        'Dim Index1 As Integer = 0 : Dim Index2 As Integer = 0
        'Dim TLT = "Arial"
        'Dim mensaje As String = ""

        'objHojaExcel.Columns(1).ColumnWidth = 5.71
        'objHojaExcel.Columns(2).ColumnWidth = 2.86
        'objHojaExcel.Columns(3).ColumnWidth = 4.86
        'objHojaExcel.Columns(4).ColumnWidth = 5
        'objHojaExcel.Columns(5).ColumnWidth = 6.57
        'objHojaExcel.Columns(6).ColumnWidth = 7
        'objHojaExcel.Columns(7).ColumnWidth = 7
        'objHojaExcel.Columns(8).ColumnWidth = 7.86
        'objHojaExcel.Columns(9).ColumnWidth = 5.29
        'objHojaExcel.Columns(10).ColumnWidth = 7
        'objHojaExcel.Columns(11).ColumnWidth = 7.57
        'objHojaExcel.Columns(12).ColumnWidth = 6.43
        'objHojaExcel.Columns(13).ColumnWidth = 6.29
        'objHojaExcel.Columns(14).ColumnWidth = 6.29
        'objHojaExcel.Columns(15).ColumnWidth = 6.29
        'objHojaExcel.Columns(16).ColumnWidth = 6.29

        'If Condicion = True Then
        '    objHojaExcel.Columns(17).ColumnWidth = 24
        'End If

        'EstiloTexto(objHojaExcel.Range("A1:A2"), "Story", TLT)
        'EstiloTexto(objHojaExcel.Range("B1:B2"), "Pier", TLT)
        'EstiloTexto(objHojaExcel.Range("C1:C2"), "Lw(m)", TLT)
        'EstiloTexto(objHojaExcel.Range("D1:D2"), "Bw(m)", TLT)
        'EstiloTexto(objHojaExcel.Range("E1:E2"), "Fc (kgf/cm²)", TLT)
        'EstiloTexto(objHojaExcel.Range("F1:F2"), "rt", TLT, True, 1)
        'EstiloTexto(objHojaExcel.Range("G1:G2"), "rl", TLT, True, 1)
        'EstiloTexto(objHojaExcel.Range("H1:H2"), "Malla", TLT)
        'EstiloTexto(objHojaExcel.Range("I1:I2"), "C (cm)", TLT)
        'EstiloTexto(objHojaExcel.Range("J1:J2"), "Lebe_Izq (cm)", TLT)
        'EstiloTexto(objHojaExcel.Range("K1:K2"), "Lebe_Der (cm)", TLT)
        'EstiloTexto(objHojaExcel.Range("L1:L2"), "Lebe_Centro (cm)", TLT)
        'EstiloTexto(objHojaExcel.Range("M1:M2"), "Zc_Izq (cm)", TLT)
        'EstiloTexto(objHojaExcel.Range("N1:N2"), "Zc_Der (cm)", TLT)
        'EstiloTexto(objHojaExcel.Range("O1:O2"), "Pmax (tonf)", TLT)
        'EstiloTexto(objHojaExcel.Range("P1:P2"), "Pc (tonf)", TLT)

        'If Condicion = True Then
        '    EstiloTexto(objHojaExcel.Range("Q1:Q2"), "Mensaje", TLT)
        'End If

        'Dim x As Integer = 0

        'If Condicion = False Then
        '    For i = 0 To Muros_lista_2.Count - 1
        '        For j = 0 To Muros_lista_2(i).Stories.Count - 1
        '            x += 1
        '        Next
        '    Next
        'Else
        '    'x = Errores.Muros_errores.Count
        'End If

        'Dim ArregloDatos3(x - 1, 16) As Object

        'x = 0

        'If Condicion = False Then
        '    For j = 0 To Muros_lista_2.Count - 1
        '        For k = 0 To Muros_lista_2(j).Stories.Count - 1

        '            Dim temp = Muros_generales.Find(Function(x1) x1.Pier = Muros_lista_2(j).Pier_name And x1.Story = Muros_lista_2(j).Stories(k))
        '            temp.Calc_pc()

        '            With Muros_lista_2(j)

        '                ArregloDatos3(x, 0) = .Stories(k)
        '                ArregloDatos3(x, 1) = .Pier_name
        '                ArregloDatos3(x, 2) = Format(Math.Round(.lw(k) / 100, 2), "#0.00")
        '                ArregloDatos3(x, 3) = Format(Math.Round(.Bw(k) / 100, 2), "#0.00")
        '                ArregloDatos3(x, 4) = Format(Math.Round(.fc(k), 2), "#0.00")
        '                ArregloDatos3(x, 5) = Format(Math.Round(.Rho_T(k), 4), "#0.0000")
        '                ArregloDatos3(x, 6) = Format(Math.Round(.Rho_l(k), 4), "#0.0000")
        '                ArregloDatos3(x, 7) = .Malla(k)
        '                ArregloDatos3(x, 8) = Format(Math.Round(.C_Def(k), 2), "#0.00")
        '                ArregloDatos3(x, 9) = Format(Math.Round(.Lebe_Izq(k), 2), "#0.00")
        '                ArregloDatos3(x, 10) = Format(Math.Round(.Lebe_Der(k), 2), "#0.00")
        '                ArregloDatos3(x, 11) = Format(Math.Round(.Lebe_Centro(k), 2), "#0.00")
        '                ArregloDatos3(x, 12) = Format(Math.Round(.Zc_Izq(k), 2), "#0.00")
        '                ArregloDatos3(x, 13) = Format(Math.Round(.Zc_Der(k), 2), "#0.00")
        '                ArregloDatos3(x, 14) = Format(Math.Round(Math.Abs(temp.P.Min()), 2), "#0.00")
        '                ArregloDatos3(x, 15) = Format(Math.Round(temp.Pc, 2), "#0.00")

        '            End With

        '            x += 1
        '        Next
        '    Next
        'Else

        '    For i = 0 To Errores.Muros_errores.Count - 1

        '        Index1 = Muros_lista_2.FindIndex(Function(x1) x1.Pier_name = Errores.Muros_errores(i).Piername)
        '        Index2 = Muros_lista_2(Index1).Stories.FindIndex(Function(x1) x1 = Errores.Muros_errores(i).Story)

        '        Dim temp = Muros_generales.Find(Function(x1) x1.Pier = Muros_lista_2(Index1).Pier_name And x1.Story = Muros_lista_2(Index1).Stories(Index2))
        '        temp.Calc_pc()

        '        With Muros_lista_2(Index1)

        '            ArregloDatos3(x, 0) = .Stories(Index2)
        '            ArregloDatos3(x, 1) = .Pier_name
        '            ArregloDatos3(x, 2) = Format(Math.Round(.lw(Index2) / 100, 2), "#0.00")
        '            ArregloDatos3(x, 3) = Format(Math.Round(.Bw(Index2) / 100, 2), "#0.00")
        '            ArregloDatos3(x, 4) = Format(Math.Round(.fc(Index2), 2), "#0.00")
        '            ArregloDatos3(x, 5) = Format(Math.Round(.Rho_T(Index2), 4), "#0.0000")
        '            ArregloDatos3(x, 6) = Format(Math.Round(.Rho_l(Index2), 4), "#0.0000")
        '            ArregloDatos3(x, 7) = .Malla(Index2)
        '            ArregloDatos3(x, 8) = Format(Math.Round(.C_Def(Index2), 2), "#0.00")
        '            ArregloDatos3(x, 9) = Format(Math.Round(.Lebe_Izq(Index2), 2), "#0.00")
        '            ArregloDatos3(x, 10) = Format(Math.Round(.Lebe_Der(Index2), 2), "#0.00")
        '            ArregloDatos3(x, 11) = Format(Math.Round(.Lebe_Centro(Index2), 2), "#0.00")
        '            ArregloDatos3(x, 12) = Format(Math.Round(.Zc_Izq(Index2), 2), "#0.00")
        '            ArregloDatos3(x, 13) = Format(Math.Round(.Zc_Der(Index2), 2), "#0.00")
        '            ArregloDatos3(x, 14) = Format(Math.Round(Math.Abs(temp.P.Min()), 2), "#0.00")
        '            ArregloDatos3(x, 15) = Format(Math.Round(temp.Pc, 2), "#0.00")
        '            mensaje = Errores.Muros_errores(i).ToString()
        '            ArregloDatos3(x, 16) = mensaje
        '        End With
        '        x += 1
        '    Next

        'End If

        'objHojaExcel.Range("A3").Resize(x, 17).Value = ArregloDatos3

        'EstiloTextoSimple(objHojaExcel.Range("A3").Resize(x, 17))

        'objHojaExcel.Visible = True

    End Sub

    Private Sub ExportarExcel_ShearDesing()

        objHojaExcel.Activate()
        objHojaExcel.Name = "1.Shear Design"

        objHojaExcel.Columns(1).ColumnWidth = 5.71
        objHojaExcel.Columns(2).ColumnWidth = 2.86
        objHojaExcel.Columns(3).ColumnWidth = 4.86
        objHojaExcel.Columns(4).ColumnWidth = 5
        objHojaExcel.Columns(5).ColumnWidth = 6.57
        objHojaExcel.Columns(6).ColumnWidth = 5.43
        objHojaExcel.Columns(7).ColumnWidth = 7.29
        objHojaExcel.Columns(8).ColumnWidth = 5.57
        objHojaExcel.Columns(9).ColumnWidth = 6.14
        objHojaExcel.Columns(10).ColumnWidth = 6.29
        objHojaExcel.Columns(11).ColumnWidth = 9
        objHojaExcel.Columns(12).ColumnWidth = 8.29
        objHojaExcel.Columns(13).ColumnWidth = 8.29
        objHojaExcel.Columns(14).ColumnWidth = 7.57
        objHojaExcel.Columns(15).ColumnWidth = 10.57
        objHojaExcel.Columns(16).ColumnWidth = 6.86
        objHojaExcel.Columns(17).ColumnWidth = 7.57
        objHojaExcel.Columns(18).ColumnWidth = 7
        objHojaExcel.Columns(19).ColumnWidth = 8
        objHojaExcel.Columns(20).ColumnWidth = 6.29

        Dim TLT = "Arial"

        EstiloTexto(objHojaExcel.Range("A1:A2"), "Story", TLT)
        EstiloTexto(objHojaExcel.Range("B1:B2"), "Pier", TLT)
        EstiloTexto(objHojaExcel.Range("C1:C2"), "Lw(m)", TLT)
        EstiloTexto(objHojaExcel.Range("D1:D2"), "Bw(m)", TLT)
        EstiloTexto(objHojaExcel.Range("E1:E2"), "Fc (kgf/cm²)", TLT)
        EstiloTexto(objHojaExcel.Range("F1:F2"), "Ht (m)", TLT)
        EstiloTexto(objHojaExcel.Range("G1:G2"), "Load", TLT)
        EstiloTexto(objHojaExcel.Range("H1:H2"), "P (Tonf)", TLT)
        EstiloTexto(objHojaExcel.Range("I1:I2"), "V2 (Tonf)", TLT)
        EstiloTexto(objHojaExcel.Range("J1:J2"), "M3     (Tonf-m)", TLT)
        EstiloTexto(objHojaExcel.Range("K1:K2"), "f Vc(Tonf) (C.11.9.5)", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("L1:L2"), "f Vn(Tonf) (C.11.9.3)", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("M1:M2"), "f Vn(Tonf) (C.21.9.4.1)", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("N1:N2"), "f Vs(Tonf) Requerido", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("O1:O2"), "f Vs max(Tonf) (C.11.4.7.9)", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("P1:P2"), "rtmax (cm" & Chr(178) & "/m)", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("Q1:Q2"), "rt-col (cm" & Chr(178) & "/m)", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("R1:R2"), "rl-col (cm" & Chr(178) & "/m)", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("S1:S2"), "# Cortinas (C.21.9.2.3)", TLT)
        EstiloTexto(objHojaExcel.Range("T1:T2"), "Sección OK?", TLT)

        Dim x As Integer = 0

        For i = 0 To Muros_generales.Count - 1
            For j = 0 To Muros_generales(i).Load.Count - 1
                x += 1
            Next
        Next

        Dim ArregloDatos(x - 1, 19) As Object
        x = 0

        For i = 0 To Muros_generales.Count - 1
            For j = 0 To Muros_generales(i).Load.Count - 1

                With Muros_generales(i)

                    ArregloDatos(x, 0) = .Story
                    ArregloDatos(x, 1) = .Pier
                    ArregloDatos(x, 2) = Format(Math.Round(.lw / 100, 2), "#0.00")
                    ArregloDatos(x, 3) = Format(Math.Round(.bw / 100, 2), "#0.00")
                    ArregloDatos(x, 4) = Format(Math.Round(.Fc, 2), "#0.00")
                    ArregloDatos(x, 5) = Format(Math.Round(.h_acumulado / 100, 2), "#0.00")
                    ArregloDatos(x, 6) = .Load(j)
                    ArregloDatos(x, 7) = Format(Math.Round(.P(j), 2), "#0.00")
                    ArregloDatos(x, 8) = Format(Math.Round(.V2(j), 2), "#0.00")
                    ArregloDatos(x, 9) = Format(Math.Round(.M3(j), 2), "#0.00")
                    ArregloDatos(x, 10) = Format(Math.Round(.Phi_Vc(j), 2), "#0.00")
                    ArregloDatos(x, 11) = Format(Math.Round(.Phi_Vn_Max1, 2), "#0.00")
                    ArregloDatos(x, 12) = Format(Math.Round(.Phi_Vn_Max2(j), 2), "#0.00")
                    ArregloDatos(x, 13) = Format(Math.Round(.Phi_Vs(j), 2), "#0.00")
                    ArregloDatos(x, 14) = Format(Math.Round(.Phi_Vs_Max, 2), "#0.00")
                    ArregloDatos(x, 15) = Format(Math.Round(.Pt_max, 4), "#0.0000")
                    ArregloDatos(x, 16) = Format(Math.Round(.pt_definitivo(j), 4), "#0.0000")
                    ArregloDatos(x, 17) = Format(Math.Round(.Rho_l_Def, 4), "#0.0000")
                    ArregloDatos(x, 18) = .Cortinas(j)
                    ArregloDatos(x, 19) = .Error_Cortante(j)
                End With

                x += 1
            Next
        Next

        objHojaExcel.Range("A3").Resize(x, 20).Value = ArregloDatos
        EstiloTextoSimple(objHojaExcel.Range("A3").Resize(x, 20))
        objHojaExcel.Visible = True

    End Sub

    Sub ExportarExcel_FlexuralStress()

        objHojaExcel.Activate()
        objHojaExcel.Name = "2.Flexural Stress"

        objHojaExcel.Columns.ColumnWidth = 13

        objHojaExcel.Columns(1).ColumnWidth = 5.71
        objHojaExcel.Columns(2).ColumnWidth = 2.86
        objHojaExcel.Columns(3).ColumnWidth = 4.86
        objHojaExcel.Columns(4).ColumnWidth = 5
        objHojaExcel.Columns(5).ColumnWidth = 6.57
        objHojaExcel.Columns(6).ColumnWidth = 5.43
        objHojaExcel.Columns(7).ColumnWidth = 7.29
        objHojaExcel.Columns(8).ColumnWidth = 6
        objHojaExcel.Columns(9).ColumnWidth = 6.86
        objHojaExcel.Columns(10).ColumnWidth = 8.29
        objHojaExcel.Columns(11).ColumnWidth = 8.57
        objHojaExcel.Columns(12).ColumnWidth = 8.29
        objHojaExcel.Columns(13).ColumnWidth = 7.57
        objHojaExcel.Columns(14).ColumnWidth = 8.14
        objHojaExcel.Columns(15).ColumnWidth = 6.14
        objHojaExcel.Columns(16).ColumnWidth = 7.57
        objHojaExcel.Columns(17).ColumnWidth = 8.29

        Dim TLT = "Arial"

        EstiloTexto(objHojaExcel.Range("A1:A2"), "Story", TLT)
        EstiloTexto(objHojaExcel.Range("B1:B2"), "Pier", TLT)
        EstiloTexto(objHojaExcel.Range("C1:C2"), "Lw(m)", TLT)
        EstiloTexto(objHojaExcel.Range("D1:D2"), "Bw(m)", TLT)
        EstiloTexto(objHojaExcel.Range("E1:E2"), "Fc (kgf/cm²)", TLT)
        EstiloTexto(objHojaExcel.Range("F1:F2"), "Ht(m)", TLT)
        EstiloTexto(objHojaExcel.Range("G1:G2"), "Load", TLT)
        EstiloTexto(objHojaExcel.Range("H1:H2"), "P(Tonf)", TLT)
        EstiloTexto(objHojaExcel.Range("I1:I2"), "M3     (Tonf-m)", TLT)
        EstiloTexto(objHojaExcel.Range("J1:J2"), "Fa (kgf/cm²)", TLT)
        EstiloTexto(objHojaExcel.Range("K1:K2"), "Fv(kgf/cm", TLT)
        EstiloTexto(objHojaExcel.Range("L1:L2"), "smax (kgf/cm²)", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("M1:M2"), "smin (kgf/cm²)", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("N1:N2"), "smax / f'c (%)", TLT, True, 1)
        EstiloTexto(objHojaExcel.Range("O1:O2"), "C (cm)", TLT)
        EstiloTexto(objHojaExcel.Range("P1:P2"), "L_conf (cm)", TLT)
        EstiloTexto(objHojaExcel.Range("Q1:Q2"), "smax > f'c OK?", TLT, True, 1)

        Dim x As Integer = 0

        For i = 0 To Muros_generales.Count - 1
            For j = 0 To Muros_generales(i).Load.Count - 1
                x += 1
            Next
        Next

        Dim ArregloDatos1(x - 1, 16) As Object
        x = 0

        For i = 0 To Muros_generales.Count - 1
            For j = 0 To Muros_generales(i).Load.Count - 1

                With Muros_generales(i)

                    ArregloDatos1(x, 0) = .Story
                    ArregloDatos1(x, 1) = .Pier
                    ArregloDatos1(x, 2) = Format(Math.Round(.lw / 100, 2), "#0.00")
                    ArregloDatos1(x, 3) = Format(Math.Round(.bw / 100, 2), "#0.00")
                    ArregloDatos1(x, 4) = Format(Math.Round(.Fc, 2), "#0.00")
                    ArregloDatos1(x, 5) = Format(Math.Round(.h_acumulado / 100, 2), "#0.00")
                    ArregloDatos1(x, 6) = .Load(j)
                    ArregloDatos1(x, 7) = Format(Math.Round(.P(j), 2), "#0.00")
                    ArregloDatos1(x, 8) = Format(Math.Round(.M3(j), 2), "#0.00")
                    ArregloDatos1(x, 9) = Format(Math.Round(.Fa(j), 2), "#0.00")
                    ArregloDatos1(x, 10) = Format(Math.Round(.Fv(j), 2), "#0.00")
                    ArregloDatos1(x, 11) = Format(Math.Round(.Sigma_Max(j), 2), "#0.00")
                    ArregloDatos1(x, 12) = Format(Math.Round(.Sigma_Min(j), 2), "#0.00")
                    ArregloDatos1(x, 13) = Format(Math.Round(.Relacion(j), 2), "#0.00")
                    ArregloDatos1(x, 14) = Format(Math.Round(.C_def(j), 2), "#0.00")
                    ArregloDatos1(x, 15) = Format(Math.Round(.L_Conf(j), 2), "#0.00")
                    ArregloDatos1(x, 16) = .Error_Flexion(j)
                End With
                x += 1
            Next
        Next

        objHojaExcel.Range("A3").Resize(x, 17).Value = ArregloDatos1
        EstiloTextoSimple(objHojaExcel.Range("A3").Resize(x, 17))
        objHojaExcel.Visible = True

    End Sub

    Private Sub Reporte(ByVal ListaCiclos As List(Of Muros_Consolidados), ByVal Inicio As Integer)

        m_Excel = New Excel.Application

        objLibroExcel = m_Excel.Workbooks.Add()
        objHojaExcel = objLibroExcel.Worksheets(1)

        objHojaExcel.Activate()

        objHojaExcel.Columns.ColumnWidth = 13

        Dim Inicio2 As Integer = 0

        For i = Inicio To ListaCiclos.Count - 1

            objHojaExcel.Range("C" & 5 + i - Inicio).Value = i + 1
            objHojaExcel.Range("C" & 5 + i - Inicio).HorizontalAlignment = Excel.Constants.xlCenter
            objHojaExcel.Range("C" & 5 + i - Inicio).Font.Name = "Arial"
        Next

        m_Excel.Windows(1).DisplayGridlines = False
        m_Excel.Visible = True
        objHojaExcel.Visible = True

    End Sub

    Private Sub EstiloTexto(ByVal Rango As Excel.Range, ByVal Text As String, ByVal TipoLetra As String, Optional ByVal LetrasGrigas As Boolean = False, Optional ByVal NoLetra As Integer = 0, Optional ByVal isInteriorColor As Boolean = False, Optional ByVal Color As Tuple(Of Integer, Integer, Integer) = Nothing, Optional ByVal EspesorCelda As Excel.XlBorderWeight = Excel.XlBorderWeight.xlThin, Optional ByVal TamanoLetra As Integer = 8)

        Rango.Merge()
        Rango.Value = Text
        Rango.HorizontalAlignment = Excel.Constants.xlCenter
        Rango.VerticalAlignment = Excel.Constants.xlCenter
        Rango.WrapText = True

        If isInteriorColor Then
            Rango.Interior.Color = RGB(Color.Item1, Color.Item2, Color.Item3)
        End If

        Rango.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
        Rango.Borders.Weight = EspesorCelda
        Rango.Font.Name = TipoLetra
        Rango.Font.Size = TamanoLetra

        If LetrasGrigas Then
            Rango.Characters(NoLetra, 1).Font.Name = "Symbol"
        End If

    End Sub

    Private Sub EstiloTextoSimple(ByVal Rango As Excel.Range, Optional ByVal TamanoLetra As Integer = 8, Optional ByVal TipoLetra As String = "Arial")
        Rango.HorizontalAlignment = Excel.Constants.xlCenter
        Rango.VerticalAlignment = Excel.Constants.xlCenter

        Rango.Font.Name = TipoLetra
        Rango.Font.Size = TamanoLetra
        Rango.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
        Rango.Borders.Weight = Excel.XlBorderWeight.xlThin
    End Sub

End Class