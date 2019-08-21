Imports System.IO
Imports Excel = Microsoft.Office.Interop.Excel
Public Class ExportExcel
    Private Lista_TextoPlano As New List(Of String)
    Private Lista_ShearDesing, Lista_FlexuralStress, Lista_Reporte As New List(Of List(Of String))
    Private Ruta_archivo_1 As String
    Private m_Excel As Excel.Application
    Private objLibroExcel As Excel.Workbook
    Private objHojaExcel As Excel.Worksheet

    Sub CargarDatos()


        Dim Lector As New StreamReader(Ruta_archivo_1)
        Dim LineText As String


        Do
            LineText = Lector.ReadLine()
            Lista_TextoPlano.Add(LineText)
        Loop Until LineText Is Nothing

        Lector.Close()


        Dim Inicio_ShearDesing, Final_ShearDesing, Inicio_FlexuralStress, Final_FlexuralStress, Inicio_Reporte, Final_Reporte As Integer

        Inicio_ShearDesing = Lista_TextoPlano.FindIndex(Function(x) x.Contains("3.Shear Design")) + 2
        Final_ShearDesing = Lista_TextoPlano.FindIndex(Function(x) x.Contains("4.Flexural Stress")) - 2

        Inicio_FlexuralStress = Lista_TextoPlano.FindIndex(Function(x) x.Contains("4.Flexural Stress")) + 2
        Final_FlexuralStress = Lista_TextoPlano.FindIndex(Function(x) x.Contains("5.Reporte")) - 2

        Inicio_Reporte = Lista_TextoPlano.FindIndex(Function(x) x.Contains("5.Reporte")) + 2
        Try
            Final_Reporte = Lista_TextoPlano.FindIndex(Function(x) x.Contains("6.Datos de Refuerzo Adicional")) - 2
        Catch
            Final_Reporte = Lista_TextoPlano.FindIndex(Function(x) x.Contains("Fin")) - 2

        End Try


        For i = Inicio_ShearDesing To Final_ShearDesing : Lista_ShearDesing.Add(Lista_TextoPlano(i).Split(vbTab).ToList) : Next
        For i = Inicio_FlexuralStress To Final_FlexuralStress : Lista_FlexuralStress.Add(Lista_TextoPlano(i).Split(vbTab).ToList) : Next
        For i = Inicio_Reporte To Final_Reporte : Lista_Reporte.Add(Lista_TextoPlano(i).Split(vbTab).ToList) : Next








    End Sub

    Sub Exportar(ByVal Route_File As String)
        Ruta_archivo_1 = Route_File
        If Ruta_archivo_1 <> "" Then

            m_Excel = New Excel.Application
            objLibroExcel = m_Excel.Workbooks.Add()
            objHojaExcel = objLibroExcel.Worksheets(1)

            CargarDatos()

            ExportarExcel_Reporte()
            objHojaExcel = objLibroExcel.Worksheets.Add()
            ExportarExcel_FlexuralStress()
            objHojaExcel = objLibroExcel.Worksheets.Add()
            ExportarExcel_ShearDesing()


            m_Excel.Windows(1).DisplayGridlines = False
            m_Excel.Visible = True

        Else
            MsgBox("Proyecto sin Salvar", MsgBoxStyle.Exclamation, "efe Prima Ce")

        End If
    End Sub


    Private Sub ExportarExcel_Reporte()
        objHojaExcel.Activate()
        objHojaExcel.Name = "3.Report"
        Dim TLT = "Arial"

        objHojaExcel.Columns(1).ColumnWidth = 5.71
        objHojaExcel.Columns(2).ColumnWidth = 2.86
        objHojaExcel.Columns(3).ColumnWidth = 4.86
        objHojaExcel.Columns(4).ColumnWidth = 5
        objHojaExcel.Columns(5).ColumnWidth = 6.57
        objHojaExcel.Columns(6).ColumnWidth = 7
        objHojaExcel.Columns(7).ColumnWidth = 7
        objHojaExcel.Columns(8).ColumnWidth = 7.86
        objHojaExcel.Columns(9).ColumnWidth = 5.29
        objHojaExcel.Columns(10).ColumnWidth = 7
        objHojaExcel.Columns(11).ColumnWidth = 7.57
        objHojaExcel.Columns(12).ColumnWidth = 6.43
        objHojaExcel.Columns(13).ColumnWidth = 6.29

        EstioTexto(objHojaExcel.Range("A1:A2"), "Story", TLT)
        EstioTexto(objHojaExcel.Range("B1:B2"), "Pier", TLT)
        EstioTexto(objHojaExcel.Range("C1:C2"), "Lw(m)", TLT)
        EstioTexto(objHojaExcel.Range("D1:D2"), "Bw(m)", TLT)
        EstioTexto(objHojaExcel.Range("E1:E2"), "Fc (kgf/cm²)", TLT)
        EstioTexto(objHojaExcel.Range("F1:F2"), "rt", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("G1:G2"), "rl", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("H1:H2"), "Malla", TLT)
        EstioTexto(objHojaExcel.Range("I1:I2"), "C (m)", TLT)
        EstioTexto(objHojaExcel.Range("J1:J2"), "Lebe_Izq (cm)", TLT)
        EstioTexto(objHojaExcel.Range("K1:K2"), "Lebe_Der (cm)", TLT)
        EstioTexto(objHojaExcel.Range("L1:L2"), "Zc_Izq (cm)", TLT)
        EstioTexto(objHojaExcel.Range("M1:M2"), "Zc_Der (cm)", TLT)


        Dim ArregloDatos3(Lista_Reporte.Count - 1, Lista_Reporte(0).Count - 14) As Object

        For i = 0 To Lista_Reporte.Count - 1
            For j = 0 To Lista_Reporte(i).Count - 14
                If j = 2 Or j = 3 Then
                    ArregloDatos3(i, j) = Val(Lista_Reporte(i)(j)) / 100
                ElseIf j = 0 Or j = 1 Or j = 7 Or j = 5 Or j = 6 Then

                    ArregloDatos3(i, j) = (Lista_Reporte(i)(j))
                Else
                    ArregloDatos3(i, j) = Format(Math.Round(Val(Lista_Reporte(i)(j)), 2), "#0.00")
                End If
            Next
        Next

        objHojaExcel.Range("A3").Resize(Lista_Reporte.Count, Lista_Reporte(0).Count - 15).Value = ArregloDatos3

        EstiloTextoSimple(objHojaExcel.Range("A3").Resize(Lista_Reporte.Count, Lista_Reporte(0).Count - 15))

        objHojaExcel.Visible = True

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


        EstioTexto(objHojaExcel.Range("A1:A2"), "Story", TLT)
        EstioTexto(objHojaExcel.Range("B1:B2"), "Pier", TLT)
        EstioTexto(objHojaExcel.Range("C1:C2"), "Lw(m)", TLT)
        EstioTexto(objHojaExcel.Range("D1:D2"), "Bw(m)", TLT)
        EstioTexto(objHojaExcel.Range("E1:E2"), "Fc (kgf/cm²)", TLT)
        EstioTexto(objHojaExcel.Range("F1:F2"), "Ht (m)", TLT)
        EstioTexto(objHojaExcel.Range("G1:G2"), "Load", TLT)
        EstioTexto(objHojaExcel.Range("H1:H2"), "P (Tonf)", TLT)
        EstioTexto(objHojaExcel.Range("I1:I2"), "V2 (Tonf)", TLT)
        EstioTexto(objHojaExcel.Range("J1:J2"), "M3     (Tonf-m)", TLT)
        EstioTexto(objHojaExcel.Range("K1:K2"), "f Vc(Tonf) (C.11.9.5)", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("L1:L2"), "f Vn(Tonf) (C.11.9.3)", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("M1:M2"), "f Vn(Tonf) (C.21.9.4.1)", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("N1:N2"), "f Vs(Tonf) Requerido", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("O1:O2"), "f Vs max(Tonf) (C.11.4.7.9)", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("P1:P2"), "rtmax (cm" & Chr(178) & "/m)", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("Q1:Q2"), "rt-col (cm" & Chr(178) & "/m)", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("R1:R2"), "rl-col (cm" & Chr(178) & "/m)", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("S1:S2"), "# Cortinas (C.21.9.2.3)", TLT)
        EstioTexto(objHojaExcel.Range("T1:T2"), "Sección OK?", TLT)


        Dim ArregloDatos(Lista_ShearDesing.Count - 1, Lista_ShearDesing(0).Count - 1) As Object
        For i = 0 To Lista_ShearDesing.Count - 1
            For j = 0 To Lista_ShearDesing(i).Count - 1
                If j = 2 Or j = 3 Or j = 5 Then
                    ArregloDatos(i, j) = Val(Lista_ShearDesing(i)(j)) / 100
                ElseIf j = 7 Or j = 8 Or j = 9 Or j = 10 Or j = 11 Or j = 12 Or j = 13 Or j = 14 Then
                    ArregloDatos(i, j) = Format(Math.Round(Val(Lista_ShearDesing(i)(j)), 2), "#0.00")
                ElseIf j = 15 Or j = 16 Or j = 17 Then
                    ArregloDatos(i, j) = Format(Math.Round(Val(Lista_ShearDesing(i)(j)), 3), "#0.0000")
                Else
                    ArregloDatos(i, j) = Lista_ShearDesing(i)(j)
                End If
            Next
        Next
        objHojaExcel.Range("A3").Resize(Lista_ShearDesing.Count, Lista_ShearDesing(0).Count).Value = ArregloDatos

        EstiloTextoSimple(objHojaExcel.Range("A3").Resize(Lista_ShearDesing.Count, Lista_ShearDesing(0).Count))

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

        EstioTexto(objHojaExcel.Range("A1:A2"), "Story", TLT)
        EstioTexto(objHojaExcel.Range("B1:B2"), "Pier", TLT)
        EstioTexto(objHojaExcel.Range("C1:C2"), "Lw(m)", TLT)
        EstioTexto(objHojaExcel.Range("D1:D2"), "Bw(m)", TLT)
        EstioTexto(objHojaExcel.Range("E1:E2"), "Fc (kgf/cm²)", TLT)
        EstioTexto(objHojaExcel.Range("F1:F2"), "Ht(m)", TLT)
        EstioTexto(objHojaExcel.Range("G1:G2"), "Load", TLT)
        EstioTexto(objHojaExcel.Range("H1:H2"), "P(Tonf)", TLT)
        EstioTexto(objHojaExcel.Range("I1:I2"), "M3     (Tonf-m)", TLT)
        EstioTexto(objHojaExcel.Range("J1:J2"), "Fa (kgf/cm²)", TLT)
        EstioTexto(objHojaExcel.Range("K1:K2"), "Fv(kgf/cm", TLT)
        EstioTexto(objHojaExcel.Range("L1:L2"), "smax (kgf/cm²)", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("M1:M2"), "smin (kgf/cm²)", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("N1:N2"), "smax / f'c (%)", TLT, True, 1)
        EstioTexto(objHojaExcel.Range("O1:O2"), "C (m)", TLT)
        EstioTexto(objHojaExcel.Range("P1:P2"), "L_conf (cm)", TLT)
        EstioTexto(objHojaExcel.Range("Q1:Q2"), "smax > f'c OK?", TLT, True, 1)

        Dim ArregloDatos1(Lista_FlexuralStress.Count - 1, Lista_FlexuralStress(0).Count - 1) As Object
        For i = 0 To Lista_FlexuralStress.Count - 1
            For j = 0 To Lista_FlexuralStress(i).Count - 1
                If j = 2 Or j = 3 Or j = 5 Then
                    ArregloDatos1(i, j) = Val(Lista_FlexuralStress(i)(j)) / 100
                ElseIf j = 6 Or j = 16 Or j = 0 Or j = 1 Then
                    ArregloDatos1(i, j) = Lista_FlexuralStress(i)(j)
                ElseIf j = 13 Then
                    ArregloDatos1(i, j) = Format(Math.Round(Val(Lista_FlexuralStress(i)(j)) * 100, 2), "#0.00")
                Else
                    ArregloDatos1(i, j) = Format(Math.Round(Val(Lista_FlexuralStress(i)(j)), 2), "#0.00")
                End If

            Next
        Next
        objHojaExcel.Range("A3").Resize(Lista_FlexuralStress.Count, Lista_FlexuralStress(0).Count).Value = ArregloDatos1

        EstiloTextoSimple(objHojaExcel.Range("A3").Resize(Lista_FlexuralStress.Count, Lista_FlexuralStress(0).Count))


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

    Private Sub EstioTexto(ByVal Rango As Excel.Range, ByVal Text As String, ByVal TipoLetra As String, Optional ByVal LetrasGrigas As Boolean = False, Optional ByVal NoLetra As Integer = 0, Optional ByVal isInteriorColor As Boolean = False, Optional ByVal Color As Tuple(Of Integer, Integer, Integer) = Nothing, Optional ByVal EspesorCelda As Excel.XlBorderWeight = Excel.XlBorderWeight.xlThin, Optional ByVal TamanoLetra As Integer = 8)

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
