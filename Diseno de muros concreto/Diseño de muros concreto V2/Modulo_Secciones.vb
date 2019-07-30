Imports Autodesk.AutoCAD.Interop
Imports Autodesk.AutoCAD.Interop.Common
Imports MathNet.Numerics.LinearAlgebra

Module Modulo_Secciones
    Private AcadApp As Autodesk.AutoCAD.Interop.AcadApplication 'Variable para abrir autocad
    Private AcadDoc As Autodesk.AutoCAD.Interop.AcadDocument 'Variabla para el documento de autocad
    Private AcadDictionary As AcadDictionary
    Private Polyline As AcadPolyline
    Private Polyline_hatch As AcadPolyline
    Private cota As AcadDimAligned
    Private cota_resistencia, cota_seccion As AcadDimRotated
    Private linea As AcadLine
    Private texto_nivel, Texto_malla, Texto_Horizontal, Texto_info, Multi_Text As AcadMText
    Private Texto_muro, Num_capas As AcadText
    Private selection_set As AcadSelectionSet
    Private Base_datos As List(Of AcadObject)
    Private objeto As AcadObject
    Private Lista_polyline_muro As New List(Of polyline_muro)
    Private lista_nombre_muros As New List(Of AcadBlockReference)
    Private Refbloc As AcadBlockReference
    Private refuerzo As AcadCircle
    Private lista_refuerzo As New List(Of AcadCircle)

    Public Class polyline_muro
        Public object_type As String
        Public coordinates As New List(Of Double)
        Public coordinatesX As New List(Of Double)
        Public coordinatesY As New List(Of Double)
        Public lw, bw As Double
        Public separar As Boolean = False
        Public Dx, Dy As Double
        Public MinX, MinY, MaxX, MaxY As Double
        Public pier_name As String = Nothing

        Public Sub dimensions(ByVal coordX As List(Of Double), ByVal coordY As List(Of Double))

            MinX = coordX.Min
            MaxX = coordX.Max
            MinY = coordY.Min
            MaxY = coordY.Max

            Dx = MaxX - MinX
            Dy = MaxY - MinY

            If Dx > Dy Then
                lw = Dx : bw = Dy
            Else
                lw = Dy : bw = Dx
            End If

        End Sub
    End Class
    Private Function Rectangulo(ByRef coord As Double()) As Boolean
        Dim coordinates As Double()
        coordinates = coord

        If coordinates.Count = 8 Then
            Rectangulo = True
        Else
            Rectangulo = False
        End If

    End Function

    Private Function Remove_element(Of T)(ByRef arr As T(), ByVal indice As Integer) As T()
        Dim uBound = arr.GetUpperBound(0)
        Dim lBound = arr.GetLowerBound(0)
        Dim arrLen = arr.Count - 1

        If indice < lBound OrElse indice > uBound Then
            Throw New ArgumentOutOfRangeException(
            String.Format("Index must be from {0} to {1}.", lBound, uBound))
        Else
            'create an array 1 element less than the input array
            Dim outArr(arrLen - 1) As T
            'copy the first part of the input array
            Array.Copy(arr, 0, outArr, 0, indice)
            'then copy the second part of the input array
            Array.Copy(arr, indice + 1, outArr, indice, uBound - indice)
            Return outArr
        End If

    End Function


    Sub Lectura_datos()

        Dim rnd As New Random
        Dim i As Integer
        Dim pl_coordinates As Double()
        Dim x As Integer
        Dim layer As String

        Lista_polyline_muro.Clear()
        lista_nombre_muros.Clear()
        lista_muros_planta.Clear()

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

        selection_set = AcadApp.ActiveDocument.SelectionSets.Add(i.ToString)
        MsgBox("Seleccione la planta estructural", vbInformation, "Efe prima Ce")
        selection_set.SelectOnScreen()

        x = 0
        Form6.ProgressBar1.Visible = True
        Try
            Form6.ProgressBar1.Maximum = selection_set.Count - 1
        Catch ex As Exception

        End Try

        For Each objeto In selection_set

            ''Reconocimiento de polilineas en la planta estructural
            If objeto.ObjectName = "AcDbPolyline" Then
                layer = objeto.layer
                If Rectangulo(objeto.coordinates) = True And layer = "FC_BORDES" Then
                    Dim poly As New polyline_muro
                    pl_coordinates = objeto.coordinates

                    For i = 0 To pl_coordinates.Count - 1 Step 2
                        poly.coordinatesX.Add(pl_coordinates(i))
                        poly.coordinatesY.Add(pl_coordinates(i + 1))
                        poly.coordinates.Add(pl_coordinates(i))
                        poly.coordinates.Add(pl_coordinates(i + 1))
                    Next

                    poly.dimensions(poly.coordinatesX, poly.coordinatesY)
                    Lista_polyline_muro.Add(poly)
                End If
            End If

            ''Reconomiento de los nombres de los muros 
            If objeto.ObjectName = "AcDbBlockReference" Then
                Refbloc = objeto
                If Refbloc.Layer = "FC_NUMERO" Then
                    lista_nombre_muros.Add(Refbloc)
                End If
            End If

            ''Reconocimiento de refuerzo en la planta estructural de muros
            If objeto.ObjectName = "AcDbCircle" Then
                refuerzo = objeto
                If refuerzo.Layer = "FC_REFUERZO 2" Then
                    lista_refuerzo.Add(refuerzo)
                End If
            End If

            x = x + 1
            Form6.ProgressBar1.Increment(1)
        Next
        Form6.ProgressBar1.Visible = False
        Procesar_datos()

    End Sub

    Private Sub Procesar_datos()
        Dim i As Integer
        Dim xc, yc, a, b As Double
        Dim menor_d, Dist, Dist2, Angulo, min_ang, metrica As Double
        Dim menc As Integer
        Dim V_dist, V_ang As Vector(Of Double)
        Dim suma As Vector(Of Double)

        V_dist = Vector(Of Double).Build.Dense(lista_nombre_muros.Count)
        V_ang = Vector(Of Double).Build.Dense(lista_nombre_muros.Count)

        ''Separar zona de estribos dentro de los muros
        For i = 0 To Lista_polyline_muro.Count - 1
            For j = 0 To Lista_polyline_muro.Count - 1
                If i <> j Then
                    If Lista_polyline_muro(i).coordinatesX.Min < Lista_polyline_muro(j).coordinatesX.Min And Lista_polyline_muro(j).coordinatesX.Max < Lista_polyline_muro(i).coordinatesX.Max AndAlso Lista_polyline_muro(i).coordinatesY.Min < Lista_polyline_muro(j).coordinatesY.Min And Lista_polyline_muro(j).coordinatesY.Max < Lista_polyline_muro(i).coordinatesY.Max Then
                        Lista_polyline_muro(j).separar = True
                    End If
                End If
            Next
        Next

        ''Eliminar las polilineas sobrantes
        For i = 0 To Lista_polyline_muro.Count - 1
            If Lista_polyline_muro(i).separar = False Then
                lista_muros_planta.Add(Lista_polyline_muro(i))
            End If
        Next

        Lista_polyline_muro.Clear()

        'Vincular nombres a dimensiones de los muros
        For i = 0 To lista_muros_planta.Count - 1
            xc = (lista_muros_planta(i).Dx / 2) + lista_muros_planta(i).MinX
            yc = (lista_muros_planta(i).Dy / 2) + lista_muros_planta(i).MinY
            menor_d = 99999
            V_dist.Clear()
            V_ang.Clear()

            For j = 0 To lista_nombre_muros.Count - 1

                a = xc - lista_nombre_muros(j).InsertionPoint(0)
                b = yc - lista_nombre_muros(j).InsertionPoint(1)
                V_dist(j) = Math.Sqrt((a ^ 2) + (b ^ 2))

                If lista_muros_planta(i).Dx > lista_muros_planta(i).Dy Then
                    V_ang(j) = Math.Atan2(Math.Abs(b), Math.Abs(a)) * 180 / Math.PI
                Else
                    V_ang(j) = Math.Atan2(Math.Abs(a), Math.Abs(b)) * 180 / Math.PI
                End If

            Next

            suma = (0.6 * V_dist.Divide(V_dist.Maximum)) + (0.4 * V_ang.Divide(V_ang.Maximum))
            menc = suma.MinimumIndex
            lista_muros_planta(i).pier_name = lista_nombre_muros(menc).GetAttributes()(0).TextString
        Next
        lista_nombre_muros.Clear()


    End Sub
End Module