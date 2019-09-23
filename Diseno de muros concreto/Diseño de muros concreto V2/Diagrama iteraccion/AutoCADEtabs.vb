Imports System.IO

Public Class AutoCADEtabs

    Public Sub New(ByVal ListaMurosAranas As List(Of Arania))
        Dim ArchivoTexto As New List(Of String)
        Dim OpenFile As OpenFileDialog : OpenFile = New OpenFileDialog
        With OpenFile
            .Title = "Abrir Proyecto de ETABS"
            .Filter = "Proyecto ETABS |*.$et"
            .ShowDialog()
        End With

        If OpenFile.FileName <> "" Then
            Dim LectorArchivo As StreamReader
            Dim LineaTexto As String
            LectorArchivo = New StreamReader(OpenFile.FileName)
            Do
                LineaTexto = LectorArchivo.ReadLine()
                ArchivoTexto.Add(LineaTexto)
            Loop Until LineaTexto Is Nothing

            LectorArchivo.Close()
            If ListaMurosAranas.Count <> 0 Then
                Inicio(ArchivoTexto, ListaMurosAranas)
            Else
                MsgBox("Agregue por lo menos una Araña.", MsgBoxStyle.Information, "efe Prima Ce")
            End If
        Else

        End If

    End Sub

    Public Sub Inicio(ByVal ArchivoET As List(Of String), ByVal ListaMurosAranas As List(Of Arania))

        'OrganizarBarrasPorPiso
        For i = 0 To ListaMurosAranas.Count - 1

            For j = 0 To ListaMurosAranas(i).Muros_arania.Count - 1
                ListaMurosAranas(i).Muros_arania(j).AsignarBarras()
                ListaMurosAranas(i).Muros_arania(j).CalculoCentroide2()

                ListaMurosAranas(i).Muros_arania(j).Fc = Muros_lista_2.Find(Function(x) x.Pier_name = ListaMurosAranas(i).Muros_arania(j).NombreMuro).fc

            Next
        Next

        For i = 0 To ListaMurosAranas.Count - 1
            Dim XminC As Single = 999999
            Dim YminC As Single = 999999

            For j = 0 To ListaMurosAranas(i).Muros_arania.Count - 1
                If XminC > ListaMurosAranas(i).Muros_arania(j).XC Then
                    XminC = ListaMurosAranas(i).Muros_arania(j).XC
                End If
                If YminC > ListaMurosAranas(i).Muros_arania(j).YC Then
                    YminC = ListaMurosAranas(i).Muros_arania(j).YC
                End If

            Next
            ListaMurosAranas(i).Lista_XC_YC_Min(0) = XminC
            ListaMurosAranas(i).Lista_XC_YC_Min(1) = YminC
        Next

        Dim ListaTextoSectionDesig As New List(Of String)

        For i = 0 To ListaMurosAranas.Count - 1

            With ListaMurosAranas(i)
                .ListNumerosShaesPorPiso.Clear()
                Dim NoShapes = 0

                For j = 0 To .Muros_arania.Count - 1

                    For m = .Muros_arania(j).Hw.Count - 1 To 0 Step -1
                        NoShapes = .Muros_arania.Count
                        Try
                            NoShapes = .Muros_arania(j).ListaRefuerzosPorPiso(m).Count + NoShapes
                        Catch
                            NoShapes = NoShapes + 0
                        End Try
                        .ListNumerosShaesPorPiso.Add(NoShapes)
                    Next
                Next

            End With
        Next

        For i = 0 To ListaMurosAranas.Count - 1

            With ListaMurosAranas(i)
                For m = .Muros_arania(0).Hw.Count - 1 To 0 Step -1
                    Dim NomenclaturaMuro As String = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  TYPE {Chr(34)}PIER{Chr(34)}  NUMSHAPES { .ListNumerosShaesPorPiso(m)}"
                    ListaTextoSectionDesig.Add(NomenclaturaMuro)
                    Dim NoShape As Integer = 1
                    For j = 0 To .Muros_arania.Count - 1
                        Dim B, D, XC, YC
                        XC = .Muros_arania(j).XC - .Lista_XC_YC_Min(0)
                        YC = .Muros_arania(j).YC - .Lista_XC_YC_Min(1)
                        If .Muros_arania(j).DireccionMuro = "Horizontal" Then
                            B = .Muros_arania(j).Longitud
                            D = .Muros_arania(j).EspesorePorPiso(m)
                        Else
                            B = .Muros_arania(j).EspesorePorPiso(m)
                            D = .Muros_arania(j).Longitud
                        End If
                        NomenclaturaMuro = $"  SDSECTION {Chr(34)}{ .Label }-Story{m + 1}{Chr(34)}  SHAPE {NoShape}  MATERIAL {Chr(34)}H{ .Muros_arania(j).Fc(m)}{Chr(34)}  SHAPETYPE {Chr(34)}CONCRETE RECTANGULAR{Chr(34)} D {D} B {B} XC {XC}  YC {YC}"
                        ListaTextoSectionDesig.Add(NomenclaturaMuro)

                        NoShape = NoShape + 1

                    Next

                Next
            End With
        Next

        Dim Save As SaveFileDialog
        Save = New SaveFileDialog
        Save.Title = "Prueba"
        Save.Filter = "Archivo de Texto|*.txt"
        Save.ShowDialog()

        Dim Escritor As StreamWriter = New StreamWriter(Save.FileName)

        For i = 0 To ListaTextoSectionDesig.Count - 1
            Escritor.WriteLine(ListaTextoSectionDesig(i))
        Next
        Escritor.Close()

    End Sub

End Class