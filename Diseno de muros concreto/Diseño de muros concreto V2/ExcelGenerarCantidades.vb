
'Imports Excel = Microsoft.Office.Interop.Excel


'Module ExcelGenerarCantidades



'    Public m_Excel As Excel.Application

'    Public objLibroExcel As Excel.Workbook
'    Public objHojaExcel As Excel.Worksheet

'    Public objChar As Excel.ChartObject



'    Sub Reporte(ByVal ListaAlzados As List(Of alzado_muro))


'        m_Excel = New Excel.Application



'        objLibroExcel = m_Excel.Workbooks.Add()
'        objHojaExcel = objLibroExcel.Worksheets(1)






'        objHojaExcel.Activate()


'        objHojaExcel.Columns.ColumnWidth = 13

'        EstiloTitulo(objHojaExcel.Range("J4:L4"), "Axial stiffnes", "Arial")
'        EstiloTitulo(objHojaExcel.Range("B5"), "Def.", "Arial")
'        EstiloTitulo(objHojaExcel.Range("C5"), "Cicle", "Arial")
'        EstiloTitulo(objHojaExcel.Range("D5"), "h", "Symbol")
'        EstiloTitulo(objHojaExcel.Range("E5"), "Cum h", "Arial")

'        EstiloTitulo(objHojaExcel.Range("F5"), " ", "Arial")
'        EstiloTitulo(objHojaExcel.Range("G5"), " ", "Arial")
'        EstiloTitulo(objHojaExcel.Range("H5"), "x (%)", "Arial")
'        EstiloTitulo(objHojaExcel.Range("I5"), "Comp/Tens", "Arial")
'        EstiloTitulo(objHojaExcel.Range("J5"), "Kten", "Arial")
'        EstiloTitulo(objHojaExcel.Range("K5"), "Kcomp", "Arial")
'        EstiloTitulo(objHojaExcel.Range("L5"), "Kave/Kini", "Arial")
'        EstiloTitulo(objHojaExcel.Range("M5"), "Energy", "Arial")
'        EstiloTitulo(objHojaExcel.Range("N5"), "Cum Energy", "Arial")
'        Dim Inicio2 As Integer = 0

'        For i = Inicio To ListaCiclos.Count - 1

'            objHojaExcel.Range("C" & 5 + i - Inicio).Value = i + 1
'            objHojaExcel.Range("C" & 5 + i - Inicio).HorizontalAlignment = Excel.Constants.xlCenter
'            objHojaExcel.Range("C" & 5 + i - Inicio).Font.Name = "Arial"
'        Next


'        m_Excel.Windows(1).DisplayGridlines = False
'        m_Excel.Visible = True
'        objHojaExcel.Visible = True


'    End Sub

'    Sub EstiloTitulo(ByVal Rango As Excel.Range, ByVal Titulo As String, ByVal EstilodeLetra As String)

'        Rango.Merge()
'        Rango.Value = Titulo
'        Rango.HorizontalAlignment = Excel.Constants.xlCenter
'        Rango.Interior.Color = RGB(217, 217, 217)
'        Rango.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
'        Rango.Borders.Weight = Excel.XlBorderWeight.xlMedium
'        Rango.Font.Name = EstilodeLetra

'        If Titulo = "Cum h" Then
'            Rango.Characters(5, 6).Font.Name = "Symbol"
'        End If
'        If Titulo = "x (%)" Then
'            Rango.Characters(1, 1).Font.Name = "Symbol"
'        End If
'    End Sub




'End Module
