Imports Autodesk.AutoCAD.Interop
Imports Autodesk.AutoCAD.Interop.Common
Imports MathNet.Numerics.LinearAlgebra

Module Modulo_Autocad

    Private AcadApp As Autodesk.AutoCAD.Interop.AcadApplication 'Variable para abrir autocad
    Private AcadDoc As Autodesk.AutoCAD.Interop.AcadDocument 'Variabla para el documento de autocad
    Private Polyline As AcadLWPolyline
    Private circle_ref As AcadCircle
    Private selection_set As AcadSelectionSet
    Private objeto As AcadObject

    Public Class Bloque_Nom_muro

        Public insertion_point() As Double
        Public Nombre_muro As String

    End Class

    Sub Get_sections()
        Dim rnd As New Random
        Dim i As Integer
        Dim pl_coordinates As Double() = Nothing
        Dim Circle_aux() As Double = Nothing
        Dim Circle_center() As Double = Nothing
        Dim Bloque As AcadBlockReference
        Dim Xc As Double = 0
        Dim Yc As Double = 0
        Dim Y As Integer = 0
        Dim Prueba As Integer
        Dim angulo As Single
        Dim P_recta() As Double = Nothing

        Try
            AcadApp = GetObject(, "Autocad.Application")
        Catch ex As Exception
            Dim ruta As New OpenFileDialog()
            AcadApp = New AcadApplication
            AcadApp.Visible = True
            With ruta
                .Multiselect = True
                .Filter = "Elegir Planta Estructural|*.dwg"
                .Title = "Archivo dwg"
                .ShowDialog()
            End With
            AcadDoc = AcadApp.Documents.Add(ruta.FileName)
        End Try

        i = rnd.Next(0, 100000)

        ''Seleccion de objetos en autocad
        AcadDoc = AcadApp.ActiveDocument
        AcadDoc.Save()
        AcadDoc.PurgeAll()
        selection_set = AcadDoc.SelectionSets.Add(i.ToString)
        MsgBox("Seleccione la planta estructural", vbInformation, "Efe prima Ce")
        selection_set.SelectOnScreen()

        Dim Cantidad As Integer
        Cantidad = selection_set.Count - 1
        'Form1.ProgressBar1.Maximum = Cantidad
        For i = 0 To selection_set.Count - 1

            objeto = selection_set(i)

            If objeto.ObjectName = "AcDbPolyline" Then

                pl_coordinates = objeto.Coordinates

                If pl_coordinates.Length = 8 Then
                    Prueba = direction(pl_coordinates.ToList, angulo, P_recta)
                    Lista_pl_coordinates.Add(pl_coordinates.ToList)
                    Lista_rectas.Add(P_recta)
                End If

            End If

            If objeto.ObjectName = "AcDbCircle" Then
                Circle_aux = objeto.Center
                Circle_center = {Circle_aux(0), Circle_aux(1)}
                Lista_ref.Add(Circle_center.ToList)
            End If

            If objeto.ObjectName = "AcDbBlockReference" Then
                Bloque = objeto

                If Bloque.Name = "N-NUM" Then

                    Dim Bloquei As New Bloque_Nom_muro
                    Bloquei.insertion_point = Bloque.InsertionPoint
                    Bloquei.Nombre_muro = Bloque.GetAttributes(0).TextString
                    Lista_bloque_referencia.Add(Bloquei)

                End If


            End If

            'Form1.ProgressBar1.Increment(1)
            'Form1.ProgressBar1.Update()
        Next

        Circle_aux = Nothing
        i = get_centroid(Lista_pl_coordinates, Xc, Yc)
        i = Transform_coordinates(Xc, Yc, Lista_pl_coordinates, T_coordinates)
        i = Transform_coordinates(Xc, Yc, Lista_ref, T_center_circle)
        'Form1.ProgressBar1.Value = 0
        'Form1.ProgressBar1.Update()

    End Sub

    Private Function get_centroid(ByVal lista_coordenadas As List(Of List(Of Double)), ByRef Xc As Double, ByRef Yc As Double) As Integer

        Dim SumX As Double = 0
        Dim SumY As Double = 0
        Dim CantX As Double = 0
        Dim CantY As Double = 0

        Try
            For i = 0 To lista_coordenadas.Count - 1

                For j = 0 To lista_coordenadas(i).Count - 1

                    If j = 0 OrElse j Mod 2 = 0 Then
                        SumX = SumX + lista_coordenadas(i)(j)
                        CantX = CantX + 1
                    Else
                        SumY = SumY + lista_coordenadas(i)(j)
                        CantY = CantY + 1
                    End If

                Next
            Next

            Xc = SumX / CantX
            Yc = SumY / CantY
            get_centroid = 1

        Catch ex As Exception
            get_centroid = 0
        End Try



    End Function

    Private Function Transform_coordinates(ByVal Xc As Double, ByVal Yc As Double, ByVal Lista_coordenadas As List(Of List(Of Double)), ByRef T_coord As List(Of List(Of Double))) As Integer

        Dim O As Matrix(Of Double)
        Dim P As Matrix(Of Double)
        Dim T_aux As List(Of Double) = Nothing
        P = Matrix(Of Double).Build.DenseOfArray({{1, 0, -Xc}, {0, 1, -Yc}, {0, 0, 1}})

        Try
            'Form1.ProgressBar1.Value = 0
            'Form1.ProgressBar1.Maximum = Lista_coordenadas.Count - 1
            For i = 0 To Lista_coordenadas.Count - 1

                T_aux = New List(Of Double)

                For j = 0 To Lista_coordenadas(i).Count - 1 Step 2

                    O = Matrix(Of Double).Build.DenseOfArray({{Lista_coordenadas(i)(j)}, {Lista_coordenadas(i)(j + 1)}, {1}})
                    Dim T As Matrix(Of Double)
                    T = P * O

                    For k = 0 To T.RowCount - 1
                        If k < T.RowCount - 1 Then
                            T_aux.Add(T(k, 0))
                        End If
                    Next

                Next
                T_coord.Add(T_aux)
                'Form1.ProgressBar1.Increment(i)
                'Form1.ProgressBar1.Update()
            Next
            'Form1.ProgressBar1.Value = 0
            Transform_coordinates = 1
        Catch ex As Exception
            Transform_coordinates = 0
        End Try


    End Function


    Private Function Ortogonalidad(ByVal lista_bloques As List(Of Bloque_Nom_muro), ByVal lista_coordenadas As List(Of List(Of Double))) As Integer

        Dim A As Matrix(Of Double)
        Dim B As Matrix(Of Double)
        Dim Orto As Matrix(Of Double)
        Dim x As Integer = 0
        Dim normB As Double
        Dim NormA As Double
        Dim prueba As Vector(Of Double)
        Dim angulo As Double

        Try
            For i = 0 To lista_coordenadas.Count - 2

                A = Matrix(Of Double).Build.Dense(4, 2)
                B = Matrix(Of Double).Build.Dense(4, 2)

                'A.SetRow(0, {3, -1})
                'A.SetRow(1, {5, -2})

                'B.SetRow(0, {-4, -1})
                'B.SetRow(1, {1, -3})

                For j = 0 To lista_coordenadas(i).Count - 1 Step 2

                    A.SetRow(x, {lista_coordenadas(i)(j), lista_coordenadas(i)(j + 1)})
                    B.SetRow(x, {lista_coordenadas(i + 1)(j), lista_coordenadas(i + 1)(j + 1)})
                    x = x + 1

                Next
                NormA = A.L2Norm
                normB = B.L2Norm
                Orto = (A.Transpose * B)
                prueba = Orto.Diagonal()
                angulo = prueba.Sum() / (NormA * normB)

                Stop

                'For k = 0 To lista_bloques.Count - 1

                '    For q = 0 To B.RowCount - 1
                '        B.SetRow(q, {lista_bloques(k).insertion_point(0), lista_bloques(k).insertion_point(1), 0})
                '    Next

                '    normB = B.L2Norm
                '    Orto = (A.Transpose * B)
                '    normB = Orto.L2Norm
                '    prueba = Orto.Diagonal()


                '    'prueba = Orto.L2Norm


                'Next

            Next


            Ortogonalidad = 1
        Catch ex As Exception


            Ortogonalidad = 0
        End Try

    End Function

    Private Function direction(ByVal pl_coordinates As List(Of Double), ByRef angulo As Single, ByRef P_recta() As Double) As Integer

        Dim MinX, MaxX, MinY, MaxY, Lx, Ly As Double


        MinX = 99999 : MaxX = -99999
        MinY = 99999 : MaxY = -99999

        Try
            For i = 0 To pl_coordinates.Count - 1 Step 2

                If pl_coordinates(i) >= MaxX Then
                    MaxX = pl_coordinates(i)
                End If

                If pl_coordinates(i) <= MinX Then
                    MinX = pl_coordinates(i)
                End If

                If pl_coordinates(i + 1) >= MaxY Then
                    MaxY = pl_coordinates(i + 1)
                End If

                If pl_coordinates(i + 1) <= MinY Then
                    MinY = pl_coordinates(i + 1)
                End If

            Next

            Lx = MaxX - MinX : Ly = MaxY - MinY

            If Lx > Ly Then
                angulo = 0
                P_recta = {MinX, (Ly / 2 + MinY), MaxX, (Ly / 2 + MinY)}
            Else
                angulo = 90
                P_recta = {(Lx / 2 + MinX), MinY, (Lx / 2 + MinX), MaxY}
            End If

            direction = 1
        Catch ex As Exception
            direction = 0
        End Try



    End Function

End Module
