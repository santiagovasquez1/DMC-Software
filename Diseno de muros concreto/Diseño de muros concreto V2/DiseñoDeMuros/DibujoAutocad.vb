Imports Autodesk.AutoCAD.Interop
Imports Autodesk.AutoCAD.Interop.Common

Module DibujoAutocad
    Private AcadApp As AcadApplication 'Variable para abrir autocad
    Private AcadDoc As AcadDocument 'Variabla para el documento de autocad
    Private AcadDictionary As AcadDictionary
    Private Bloque_Malla As AcadBlockReference
    Private Polyline As AcadPolyline
    Private Polyline_hatch As AcadLWPolyline
    Private cota As AcadDimRotated
    Private cota_resistencia, cota_seccion As AcadDimRotated
    Private linea As AcadLine
    Private texto_nivel, Texto_malla, Texto_Horizontal, Texto_info, Multi_Text As AcadMText
    Private Texto_muro, Num_capas As AcadText
    Private Texto As String
    Private j, pos As Integer
    Private altura As Double
    Private puntos(11), puntos_vigas(5), puntos_hatch_EBE(8), puntos_hatch_Zc(8), puntos_leader(8), Puntos_malla(7) As Double
    Dim puntos_leader_2(5) As Double
    Private Hviga As Single
    Private hatch As AcadHatch
    Private pattername, Ref_bloc_name As String
    Private outerLoop(0 To 0) As AcadEntity
    Private sentityObj As AcadObject
    Private arr(0) As AcadObject
    Private p1c(2), pl1(2), pm1(2) As Double
    Private p2c(2), pl2(2), pm2(2) As Double
    Private p_t3(), texto_t3() As Double
    Private location(2) As Double
    Private contador As Integer
    Private Refbloc, Refbloc1 As AcadBlockReference
    Private TextId As Long
    Private pl_malla As AcadPolyline
    Private circulo As AcadCircle
    Private pc(2), pc1(2) As Double
    Private radio As Double
    Private Hfunda As Single
    Private Annotation As AcadEntity
    Private leader As AcadLeader
    Private datos_muros As Muros_Consolidados
    Public datos_alzado As New List(Of Refuerzo)
    Private profundidad As Single
    Private mayorX1 As Double
    Private mayorx2 As Double
    Private menorx As Double
    Private menory As Double
    Private minpoint(2), maxpoint(2) As Double
    Private menc, menc1 As Integer
    Private multileader As AcadMLeader

    Public Class puntos_lineas
        Public x As Double
        Public y As Double
        Public puntos_pl(), puntos_texto() As Double
        Public z As Double
        Public cx As Double
        Public cy As Double
        Public cz As Double
        Public c2x As Double
        Public c2y As Double
        Public c2z As Double
        Public cantidad As Integer
        Public diametro As Integer
        Public tipo_traslapo As String
        Public fc As Single
        Public Altura As Double
        Public encontre As Boolean = False
    End Class

    Public Class Refuerzo
        Public pier As String
        Public story As String
        Public hw As Double
        Public fc As Double
        Public bw, lw, rho As Single
        Public diametro As Integer
        Public cantidad As Integer
        Public tipo_traslapo As String
    End Class

    Private Function Extraer_Nums(ByVal Cadena As String) As Object
        Dim solonumero As String = ""
        Dim index As Integer
        For index = 1 To Len(Cadena)
            If (Mid$(Cadena, index, 1) Like "#") _
                Or Mid$(Cadena, index, 1) = "-" Then
                solonumero = solonumero & Mid$(Cadena, index, 1)
            End If
        Next
        Return solonumero
    End Function

    Private Function Gancho_horizontal(ByVal LI As Double, ByVal LD As Double)
        Dim Tipo_gancho As String = ""
        If LI < 0.45 And LD >= 0.45 Then
            Tipo_gancho = "Gancho izquierda"
        End If

        If LD < 0.45 And LI >= 0.45 Then
            Tipo_gancho = "Gancho derecha"
        End If

        If LI < 0.45 And LD < 0.45 Then
            Tipo_gancho = "Doble gancho"
        End If

        If LI >= 0.45 And LD >= 0.45 Then
            Tipo_gancho = "Sin Gancho"
        End If
        Return Tipo_gancho
    End Function

    Private Function position(ByVal tipo_gancho As String) As Integer
        Dim pos As Integer = 0
        If tipo_gancho = "Gancho izquierda" Then
            pos = 1
        End If

        If tipo_gancho = "Gancho derecha" Then
            pos = 3
        End If

        If tipo_gancho = "Doble gancho" Then
            pos = 5
        End If

        If tipo_gancho = "Sin Gancho" Then
            pos = 7
        End If
        Return pos
    End Function

    Private Sub Find_Bloque_Malla()
        Dim prueba = AcadDoc.ModelSpace

        For Each Objeto As AcadObject In prueba

            If Objeto.Name = Ref_bloc_name Then
                Bloque_Malla = Objeto
                Exit For
            End If
        Next

    End Sub

    Sub dibujar_alzado(ByVal Nombre_muro As String)

        Dim p1_resistencia(2) As Double
        Dim p2_resistencia(2) As Double
        Dim p1_seccion(2), p2_seccion(2) As Double
        Dim location2(2) As Double
        Dim color1 As AcadAcCmColor
        Dim mirror As Integer '0 indica que no se le hace mirror, 1 indica que si se hace mirror
        Dim indice, Indice2 As Integer

        mirror = 0
        If Nombre_muro <> Nothing Then
            Try
                AcadApp = GetObject(, "Autocad.Application")
            Catch ex As Exception
                Dim ruta As New OpenFileDialog()
                AcadApp = New AcadApplication
                AcadApp.Visible = True
                With ruta
                    .Multiselect = True
                    .Filter = "Elegir Plantilla|*.dwt"
                    .Title = "Archivo de Plantilla"
                    .ShowDialog()
                End With
                AcadDoc = AcadApp.Documents.Add(ruta.FileName)
            End Try

            AcadDoc = AcadApp.ActiveDocument
            AcadDoc.SetVariable("CANNOSCALE", "1:50")

            'Graficar alzado

            'cargar datos del muro a dibujar
            datos_muros = Muros_lista_2.Find(Function(x) x.Pier_name = Nombre_muro)

            'layers = AcadDoc.Layers

            contador = 2
            'cargar datos del muro a dibujar

            Hviga = f_variables.T_Hviga.Text

            altura = 0
            j = 0

            Dim Espesores As List(Of Single) = datos_muros.Bw.Distinct.ToList
            Dim Longitudes As List(Of Single) = datos_muros.lw.Distinct.ToList
            Dim Resistencias As List(Of Single) = datos_muros.fc.Distinct.ToList

            'Dibujo de Viga de fundacion
            Hfunda = f_variables.T_Vf.Text
            profundidad = prof
            pl1(0) = -0.5 + coordX : pl1(1) = altura : pl1(2) = 0

            If Espesores.Count <= 1 Then
                pl2(0) = coordX + datos_muros.lw(0) / 100 + 0.5 : pl2(1) = altura : pl2(2) = 0
            Else
                pl2(0) = coordX + datos_muros.lw(0) / 100 + 0.8 : pl2(1) = altura : pl2(2) = 0
            End If

            linea = AcadDoc.ModelSpace.AddLine(pl1, pl2)
            linea.Layer = "FC_BORDES"
            linea.Update()

            pl1(0) = -0.5 + coordX : pl1(1) = altura - Hfunda : pl1(2) = 0
            If Espesores.Count <= 1 Then
                pl2(0) = coordX + datos_muros.lw(0) / 100 + 0.5 : pl2(1) = altura - Hfunda : pl2(2) = 0
            Else
                pl2(0) = coordX + datos_muros.lw(0) / 100 + 0.8 : pl2(1) = altura - Hfunda : pl2(2) = 0
            End If

            linea = AcadDoc.ModelSpace.AddLine(pl1, pl2)
            linea.Layer = "FC_BORDES"
            linea.Update()

            'Añadir cota de viga de fundación
            p1c(0) = coordX : p1c(1) = altura - Hfunda : p1c(2) = 0
            p2c(0) = coordX : p2c(1) = altura : p2c(2) = 0
            location(0) = coordX - 0.3 : location(1) = altura - Hfunda / 2 : location(2) = 0
            cota = AcadDoc.ModelSpace.AddDimRotated(p1c, p2c, location, Math.PI / 2)

            With cota
                .Layer = "FC_COTAS"
                .StyleName = "FC_COTAS"
                .TextHeight = 0.0015
                .Arrowhead1Type = AcDimArrowheadType.acArrowDot
                .Arrowhead2Type = AcDimArrowheadType.acArrowDot
                .ArrowheadSize = 0.001
            End With

            cota.Update()

            'Añadir linea de Corte

            Ref_bloc_name = "FC_B_Linea de corte"

            p1c(0) = coordX - 0.5 : p1c(1) = altura - Hfunda / 2 : p1c(2) = 0
            Refbloc = AcadDoc.ModelSpace.InsertBlock(p1c, Ref_bloc_name, 1, 1, 1, 0)
            Refbloc.XScaleFactor = 50
            Refbloc.Visible = False

            Dim dynamic_property1 As Object = Refbloc.GetDynamicBlockProperties
            Dim editar_property1 As AcadDynamicBlockReferenceProperty

            editar_property1 = dynamic_property1(0)
            editar_property1.Value = Hfunda + 0.26

            editar_property1 = dynamic_property1(2)
            editar_property1.Value = Math.PI / 2

            Refbloc.Visible = True
            Refbloc.Update()

            Ref_bloc_name = "FC_B_Linea de corte"

            If Espesores.Count <= 1 Then
                p1c(0) = coordX + datos_muros.lw(0) / 100 + 0.5 : p1c(1) = altura - Hfunda / 2 : p1c(2) = 0
            Else
                p1c(0) = coordX + datos_muros.lw(0) / 100 + 0.8 : p1c(1) = altura - Hfunda / 2 : p1c(2) = 0
            End If

            Refbloc = AcadDoc.ModelSpace.InsertBlock(p1c, Ref_bloc_name, 1, 1, 1, 0)
            Refbloc.XScaleFactor = 50
            Refbloc.Visible = False
            Dim dynamic_property2 As Object = Refbloc.GetDynamicBlockProperties
            Dim editar_property2 As AcadDynamicBlockReferenceProperty

            editar_property2 = dynamic_property2(0)
            editar_property2.Value = Hfunda + 0.26

            editar_property2 = dynamic_property2(2)
            editar_property2.Value = Math.PI / 2

            Refbloc.Visible = True
            Refbloc.Update()

            'Agregar linea de division del piso
            p1c(0) = coordX : p1c(1) = altura : p1c(2) = 0
            p2c(0) = coordX - 1.5 : p2c(1) = altura : p2c(2) = 0
            linea = AcadDoc.ModelSpace.AddLine(p1c, p2c)

            With linea
                .Layer = "FC_LINEA CORTE"
                .LinetypeScale = 0.35
            End With

            'Agregar texto para diferenciar el nivel de piso
            If altura + Arranque >= 0 Then
                Texto = "Piso1" & vbNewLine & "Fundación " & vbNewLine & "N+" & Format(Arranque, "##0.00")
            Else
                Texto = "Piso1" & vbNewLine & "Fundación " & vbNewLine & "N" & Format(Arranque, "##0.00")
            End If

            p1c(0) = coordX - 1.5 : p1c(1) = altura + 0.15 : p1c(2) = 0
            texto_nivel = AcadDoc.ModelSpace.AddMText(p1c, 0.75, Texto)

            With texto_nivel
                .Layer = "FC_R-80"
                .StyleName = "FC_TEXT1"
                .Height = 0.1
            End With

            'Agregar anotacion de viga de fundacion
            puntos_leader(6) = coordX - 1.5 : puntos_leader(7) = altura - (2 * Hfunda / 3) : puntos_leader(8) = 0
            puntos_leader(3) = coordX - 0.2 : puntos_leader(4) = altura - (2 * Hfunda / 3) : puntos_leader(5) = 0
            puntos_leader(0) = coordX + 0.2 : puntos_leader(1) = altura - (2 * Hfunda / 3) : puntos_leader(2) = 0
            Annotation = Nothing

            leader = AcadDoc.ModelSpace.AddLeader(puntos_leader, Annotation, AcLeaderType.acLineWithArrow)
            With leader
                .Layer = "FC_LINEA CORTE"
                .ArrowheadType = AcDimArrowheadType.acArrowDefault
                .ArrowheadSize = 0.001
            End With

            'Agregar texto para diferenciar la viga de fundacion
            Texto = "Viga de " & vbNewLine & "fundación"
            p1c(0) = coordX - 1.5 : p1c(1) = altura + 0.15 - (2 * Hfunda / 3) : p1c(2) = 0
            texto_nivel = AcadDoc.ModelSpace.AddMText(p1c, 0.6, Texto)

            With texto_nivel
                .Layer = "FC_R-80"
                .StyleName = "FC_TEXT1"
                .Height = 0.1
            End With
            color1 = texto_nivel.TrueColor

            Dim g As Single = 1.75

            Ref_bloc_name = "FC_B_Nomenclatura"
            p1c = {coordX + datos_muros.lw(0) / 200, altura - g + 0.3 - Hfunda, 0}
            Refbloc = AcadDoc.ModelSpace.InsertBlock(p1c, Ref_bloc_name, 1, 1, 1, 0)
            Refbloc.XScaleFactor = 50

            With Refbloc
                .Layer = "FC_COTAS"
                .color = 11
                .GetAttributes()(0).TextString = Format(datos_muros.Pier_name)
                .GetAttributes()(0).styleName = "FC_TEXT"
                .GetAttributes()(0).Height = 0.1
            End With
            Refbloc.Update()

            'Dibujar secciones de muros en planta
            For i = 0 To Espesores.Count - 1

                indice = datos_muros.Bw.FindIndex(Function(x) x = Espesores(i))
                Indice2 = datos_muros.Bw.FindLastIndex(Function(x) x = Espesores(i))

                puntos = {coordX, altura - Espesores(i) / 100 - g - Hfunda, 0, coordX + datos_muros.lw(indice) / 100 _
                          , altura - Espesores(i) / 100 - g - Hfunda, 0, coordX + datos_muros.lw(indice) / 100, altura - g - Hfunda, 0 _
                          , coordX, altura - g - Hfunda, 0}

                Polyline = AcadDoc.ModelSpace.AddPolyline(puntos)
                Polyline.Layer = "FC_MURO CONCRETO"
                Polyline.Closed = True
                Polyline.Update()

                pattername = "SOLID"
                hatch = AcadDoc.ModelSpace.AddHatch(0, pattername, True)
                outerLoop(0) = Polyline

                With hatch
                    .AppendOuterLoop(outerLoop)
                    .Layer = "FC_HATCH MUROS"
                    .LinetypeScale = 0.9
                    .Update()
                End With

                Hatch_Back(hatch)

                'Agregar cotas
                p1c = {coordX, altura - Espesores(i) / 100 - g - Hfunda, 0}
                p2c = {coordX, altura - g - Hfunda, 0}
                location = {p1c(0) - 0.15, ((p2c(1) - p1c(1)) / 2) + p1c(1), 0}
                cota = AcadDoc.ModelSpace.AddDimRotated(p1c, p2c, location, Math.PI / 2)

                With cota
                    .Layer = "FC_COTAS"
                    .StyleName = "FC_COTAS"
                    .TextHeight = 0.0015
                    location = {p1c(0) - 0.3, ((p2c(1) - p1c(1)) / 2) + p1c(1), 0}
                    .TextPosition = location
                    .Arrowhead1Type = AcDimArrowheadType.acArrowDot
                    .Arrowhead2Type = AcDimArrowheadType.acArrowDot
                    .ArrowheadSize = 0.001
                End With

                cota.Update()

                p1c = {coordX, altura - Espesores(i) / 100 - g - Hfunda, 0}
                p2c = {coordX + datos_muros.lw(indice) / 100, altura - Espesores(i) / 100 - g - Hfunda, 0}
                location = {((p2c(0) - p1c(0)) / 2) + p1c(0), altura - Espesores(i) / 100 - g - Hfunda - 0.24, 0}
                cota = AcadDoc.ModelSpace.AddDimRotated(p1c, p2c, location, 0)

                With cota
                    .Layer = "FC_COTAS"
                    .StyleName = "FC_COTAS"
                    .TextHeight = 0.0015
                    location = {((p2c(0) - p1c(0)) / 2) + p1c(0), altura - Espesores(i) / 100 - g - Hfunda - 0.14, 0}
                    .TextPosition = location
                    .Arrowhead1Type = AcDimArrowheadType.acArrowDot
                    .Arrowhead2Type = AcDimArrowheadType.acArrowDot
                    .ArrowheadSize = 0.001
                End With
                cota.Update()

                'Agregar Texto para definir el rango de pisos de los espesores

                If Espesores.Count > 1 Then
                    Texto = Nombre_Nivel & " " & Val(Extraer_Nums(datos_muros.Stories(Indice2))) & " a " & Nombre_Nivel & " " & Val(Extraer_Nums(datos_muros.Stories(indice)))
                Else
                    Texto = Nombre_Nivel & " " & Val(Extraer_Nums(datos_muros.Stories(Indice2)))
                End If

                p1c = {coordX + datos_muros.lw(indice) / 100 + 0.1, Polyline.Coordinates(10), 0}
                Multi_Text = AcadDoc.ModelSpace.AddMText(p1c, 2, Texto)

                With Multi_Text
                    .Layer = "FC_R-80"
                    .StyleName = "FC_TEXT1"
                    .Height = 0.1
                    .AttachmentPoint = AcAttachmentPoint.acAttachmentPointTopLeft
                End With
                Multi_Text.Update()
                g = g + 0.6
            Next

            Polyline.GetBoundingBox(minpoint, maxpoint)

            'Agregar nombre del muro
            Texto = "%%UALZADO DE MURO " & datos_muros.Pier_name
            Ref_bloc_name = "FC_B_Titulo 1"
            p1c(0) = coordX + datos_muros.lw(0) / 200 : p1c(1) = minpoint(1) - 0.75 : p1c(2) = 0
            Bloque_Info_alzado(p1c, 0.175)

            'Escribir información acerca del muro
            If Espesores.Count = 1 Then
                Texto = "Espesor de muro e=" & Format(Espesores(0) / 100, "##,0.00") & vbNewLine & "Es 1"
            Else
                Texto = "Espesor de muro e=Vble" & vbNewLine & "Es 1"
            End If
            p1c = {p1c(0), p1c(1) - 0.25, 0}
            Texto_info = AcadDoc.ModelSpace.AddMText(p1c, 2, Texto)
            With Texto_info
                .Layer = "FC_R-80"
                .StyleName = "FC_TEXT1"
                .Height = 0.1
                .AttachmentPoint = AcAttachmentPoint.acAttachmentPointMiddleCenter
                .InsertionPoint = p1c
            End With
            Texto_info.Update()

            'punto inicial para adicionar cota de resistencia
            p1_resistencia(0) = datos_muros.lw(0) / 100 + coordX : p1_resistencia(1) = altura : p1_resistencia(2) = 0

            If Espesores.Count > 1 Then
                'punto inicial para adicionar cota de cambio de sección
                p1_seccion = {datos_muros.lw(0) / 100 + coordX, altura, 0}
            End If

            'Dibujo Coco de Muros
            For i = datos_muros.Stories.Count - 1 To 0 Step -1

                If i = 0 Then
                    p1c = {coordX, altura + datos_muros.Hw(i) / 100, 0}
                    p2c = {coordX + datos_muros.lw(i) / 100, altura + datos_muros.Hw(i) / 100, 0}
                    location = {coordX + datos_muros.lw(i) / 200, p1c(1) + 0.3, 0}
                    Add_Cotas(p1c, p2c, location, location, 0, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDot, True, "")
                End If

                puntos(0) = coordX : puntos(1) = altura : puntos(2) = 0
                puntos(3) = datos_muros.lw(i) / 100 + coordX : puntos(4) = altura : puntos(5) = 0
                puntos(6) = datos_muros.lw(i) / 100 + coordX : puntos(7) = altura + datos_muros.Hw(i) / 100 : puntos(8) = 0
                puntos(9) = coordX : puntos(10) = altura + datos_muros.Hw(i) / 100 : puntos(11) = 0

                'Punto para dibujo de la linea de losa
                puntos_vigas(0) = coordX : puntos_vigas(1) = altura + datos_muros.Hw(i) / 100 - Hviga : puntos_vigas(2) = 0
                puntos_vigas(3) = datos_muros.lw(i) / 100 + coordX : puntos_vigas(4) = altura + datos_muros.Hw(i) / 100 - Hviga : puntos_vigas(5) = 0

                Add_Polilineas("FC_BORDES", puntos, 0.9)
                Add_Polilineas("FC_BORDES", puntos_vigas, 0.9)

                'Agregar Cotas
                p1c(0) = coordX : p1c(1) = altura : p1c(2) = 0
                p2c(0) = coordX : p2c(1) = altura + datos_muros.Hw(i) / 100 - Hviga : p2c(2) = 0
                location(0) = coordX - 0.3 : location(1) = altura + ((datos_muros.Hw(i) / 100) - Hviga) / 2 : location(2) = 0

                Add_Cotas(p1c, p2c, location, location, Math.PI / 2, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDot, True, "")

                'Agregar Cotas para la losa
                p1c(0) = coordX : p1c(1) = altura + datos_muros.Hw(i) / 100 - Hviga : p1c(2) = 0
                p2c(0) = coordX : p2c(1) = altura + datos_muros.Hw(i) / 100 : p2c(2) = 0
                location(0) = coordX - 0.3 : location(1) = altura + (datos_muros.Hw(i) / 100) - Hviga / 2 : location(2) = 0

                Dim Loc_Auxiliar As Double()
                Loc_Auxiliar = {coordX - 0.3 - 0.15, altura + (datos_muros.Hw(i) / 100) - Hviga / 2 - 0.05, 0}
                Add_Cotas(p1c, p2c, location, Loc_Auxiliar, Math.PI / 2, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDot, True, "")

                'Agregar linea de division del piso
                p1c(0) = coordX : p1c(1) = altura + datos_muros.Hw(i) / 100 : p1c(2) = 0
                p2c(0) = coordX - 1.5 : p2c(1) = altura + datos_muros.Hw(i) / 100 : p2c(2) = 0
                Add_Linea(p1c, p2c, "FC_LINEA CORTE")

                'Agregar texto para diferenciar el nivel de piso
                If (altura + datos_muros.Hw(i) / 100) + Arranque >= 0 Then
                    Texto = Nombre_Nivel & " " & contador & vbNewLine & "N+" & Format((altura + datos_muros.Hw(i) / 100) + Arranque, "##0.00")
                Else
                    Texto = Nombre_Nivel & " " & contador & vbNewLine & "N" & Format((altura + datos_muros.Hw(i) / 100) + Arranque, "##0.00")
                End If

                p1c(0) = coordX - 1.5 : p1c(1) = altura + datos_muros.Hw(i) / 100 + 0.15 : p1c(2) = 0
                texto_nivel = AcadDoc.ModelSpace.AddMText(p1c, 0.75, Texto)

                With texto_nivel
                    .Layer = "FC_R-80"
                    .StyleName = "FC_TEXT1"
                    .Height = 0.1
                End With

                ''Dibujo de elemementos de borde
                Add_Confinamiento(datos_muros.Lebe_Izq(i), datos_muros.Lebe_Der(i), datos_muros.Zc_Izq(i), datos_muros.Zc_Der(i), i)

                'Dibujo de Malla Electro Soldada
                If datos_muros.Malla(i) <> "Sin Malla" Then
                    Add_Malla(datos_muros.Lebe_Izq(i), datos_muros.Lebe_Der(i), datos_muros.Zc_Izq(i), datos_muros.Zc_Der(i), i)
                End If

                'Dibujar Refuerzo Horizontal Adcicional

                If datos_muros.Ref_htal(i) <> "" And datos_muros.Capas_htal(i) <> 0 Then
                    Add_Ref_Htal(datos_muros.Lebe_Izq(i), datos_muros.Lebe_Der(i), datos_muros.Zc_Izq(i), datos_muros.Zc_Der(i), i)
                End If

                contador += 1
                altura += (datos_muros.Hw(i) / 100)
            Next

            ''Add Cotas cambio de resistencia
            Add_Cotas_Especiales(indice, Indice2, Resistencias, 0.3, "Resistencia")

            ''Add Cotas cambio de espesor
            Add_Cotas_Especiales(indice, Indice2, Espesores, 0.6, "Espesores")

            AcadDoc.Regen(AcRegenType.acActiveViewport)
            coordX += 4 + datos_muros.lw(0) / 100
        End If

    End Sub

    Sub Dibujar_Refuerzo(ByVal Muro_i As Datos_Refuerzo)

        altura = 0
        Dim Text_pos As Double()
        Dim Indice, indice2 As Integer
        Dim Max_x As Single = Find_max_x(Muro_i.Lista_Coordenadas)
        Dim Min_x As Single = Find_min_x(Muro_i.Lista_Coordenadas)
        Dim Loc_Auxiliar As Double()
        Dim Texto_barra As String

        Try
            AcadApp = GetObject(, "Autocad.Application")
        Catch ex As Exception
            Dim ruta As New OpenFileDialog()
            AcadApp = New AcadApplication
            AcadApp.Visible = True
            With ruta
                .Multiselect = True
                .Filter = "Elegir Plantilla|*.dwt"
                .Title = "Archivo de Plantilla"
                .ShowDialog()
            End With
            AcadDoc = AcadApp.Documents.Add(ruta.FileName)
        End Try

        AcadDoc = AcadApp.ActiveDocument

        ''Dibujo de viga de fundacion
        Hfunda = f_variables.T_Vf.Text
        profundidad = prof

        ''Eliminar datos innecesarios
        Muro_i.Lista_Coordenadas.RemoveAll(Function(x) x.Count = 0)

        p1c = {Min_x - 1.05, 0, Max_x + 0.85, 0}
        Add_LW_PL(p1c, "FC_BORDES", 0.6, False)
        p2c = {Min_x - 1.05, -Hfunda, Max_x + 0.85, -Hfunda}
        Add_LW_PL(p2c, "FC_BORDES", 0.6, False)

        'Añadir cota de viga de fundación
        p1c = {Min_x - 0.85, p1c(1), 0}
        p2c = {Min_x - 0.85, p2c(1), 0}
        location = {p1c(0), -Hfunda / 2, 0}
        Add_Cotas(p1c, p2c, location, location, Math.PI / 2, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDot, True, "")

        'Agregar texto para diferenciar el nivel de piso
        If altura + Arranque >= 0 Then
            Texto = "Piso1" & vbNewLine & "Fundación " & vbNewLine & "N+" & Format(Arranque, "##0.00")
        Else
            Texto = "Piso1" & vbNewLine & "Fundación " & vbNewLine & "N" & Format(Arranque, "##0.00")
        End If

        p1c(0) = Min_x - 2.05 : p1c(1) = altura + 0.15 : p1c(2) = 0
        texto_nivel = AcadDoc.ModelSpace.AddMText(p1c, 0.75, Texto)

        With texto_nivel
            .Layer = "FC_R-80"
            .StyleName = "FC_TEXT1"
            .Height = 0.1
        End With

        'Agregar linea de division del piso
        p1c(0) = Min_x - 0.55 : p1c(1) = altura : p1c(2) = 0
        p2c(0) = Min_x - 2.05 : p2c(1) = altura : p2c(2) = 0
        Add_Linea(p1c, p2c, "FC_LINEA CORTE")

        ''Añadir linea de corte
        Ref_bloc_name = "FC_B_Linea de corte"
        p1c = {Min_x - 1.05, -Hfunda / 2, 0}
        Refbloc = AcadDoc.ModelSpace.InsertBlock(p1c, Ref_bloc_name, 1, 1, 1, 0)
        Refbloc.XScaleFactor = 50
        Refbloc.Visible = False

        Dim dynamic_property1 As Object = Refbloc.GetDynamicBlockProperties
        Dim editar_property1 As AcadDynamicBlockReferenceProperty

        editar_property1 = dynamic_property1(0)
        editar_property1.Value = Hfunda + 0.26

        editar_property1 = dynamic_property1(2)
        editar_property1.Value = Math.PI / 2

        Refbloc.Visible = True
        Refbloc.Update()

        Ref_bloc_name = "FC_B_Linea de corte"
        p2c = {Max_x + 0.85, -Hfunda / 2, 0}
        Refbloc = AcadDoc.ModelSpace.InsertBlock(p2c, Ref_bloc_name, 1, 1, 1, 0)
        Refbloc.XScaleFactor = 50
        Refbloc.Visible = False

        Dim dynamic_property2 As Object = Refbloc.GetDynamicBlockProperties
        Dim editar_property2 As AcadDynamicBlockReferenceProperty

        editar_property2 = dynamic_property2(0)
        editar_property2.Value = Hfunda + 0.26

        editar_property2 = dynamic_property2(2)
        editar_property2.Value = Math.PI / 2

        Refbloc.Visible = True
        Refbloc.Update()

        'Texto para identificar el Alzado

        Texto = "%%UALZADO DE REFUERZO"
        Ref_bloc_name = "FC_B_Titulo 6"
        p1c = {((Max_x + 1.05) - (-1.05 + Min_x)) / 2 + (Min_x - 1.05), 0 - 0.55 - 1.5 - 0.3, 0}
        Bloque_Info_alzado(p1c, 0.125)

        Texto = "%%ULONGITUDINAL DE MURO " & Muro_i.Nombre_muro
        Ref_bloc_name = "FC_B_Titulo 6"
        p1c = {((Max_x + 1.05) - (-1.05 + Min_x)) / 2 + (Min_x - 1.05), 0 - 0.55 - 1.5 - 0.3 - 0.25, 0}
        Bloque_Info_alzado(p1c, 0.125)

        ''Add Pisos con refuerzo
        contador = 2
        Indice = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Muro_i.Nombre_muro)

        For i = 0 To Muro_i.Stories.Count - 1

            Dim prueba As List(Of String) = Muro_i.Alzado_i(i).FindAll(Function(x) x <> "")
            indice2 = Muros_lista_2(Indice).Stories.FindIndex(Function(x) x = Muro_i.Stories(i))

            If prueba.Count > 0 Then

                ''Dibujo de coco de muro en el piso i
                p1c = {Min_x - 0.55, altura, Max_x + 0.55, altura, Max_x + 0.55, altura + Muros_lista_2(Indice).Hw(indice2) / 100, Min_x - 0.55, altura + Muros_lista_2(Indice).Hw(indice2) / 100}
                Add_LW_PL(p1c, "FC_BORDES", 0.6, True)

                ''Dibujo de viga fundacion
                p2c = {Min_x - 0.55, (altura + Muros_lista_2(Indice).Hw(indice2) / 100) - Hviga, Max_x + 0.55, (altura + Muros_lista_2(Indice).Hw(indice2) / 100) - Hviga}
                Add_LW_PL(p2c, "FC_BORDES", 0.6, False)

                ''Cotas altura libre
                p1c = {p1c(0), altura, 0}
                p2c = {p1c(0), altura + (Muros_lista_2(Indice).Hw(indice2) / 100) - Hviga, 0}
                location = {p1c(0) - 0.3, altura + (p2c(1) - p1c(1)) / 2, 0}
                Add_Cotas(p1c, p2c, location, location, Math.PI / 2, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDot, True, "")

                ''Agregar Cotas para la losa
                p1c = {p1c(0), (altura + Muros_lista_2(Indice).Hw(indice2) / 100) - Hviga, 0}
                p2c = {p1c(0), (altura + Muros_lista_2(Indice).Hw(indice2) / 100), 0}
                location = {p1c(0) - 0.3, p2c(1) - Hviga / 2, 0}
                Loc_Auxiliar = {p1c(0) - 0.45, location(1) - 0.05, 0}
                Add_Cotas(p1c, p2c, location, Loc_Auxiliar, Math.PI / 2, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDot, True, "")

                'Agregar texto para diferenciar el nivel de piso
                If (altura + Muros_lista_2(Indice).Hw(indice2) / 100) + Arranque >= 0 Then
                    Texto = Nombre_Nivel & " " & contador & vbNewLine & "N+" & Format((altura + Muros_lista_2(Indice).Hw(indice2) / 100) + Arranque, "##0.00")
                Else
                    Texto = Nombre_Nivel & " " & contador & vbNewLine & "N" & Format((altura + Muros_lista_2(Indice).Hw(indice2) / 100) + Arranque, "##0.00")
                End If

                p1c(0) = p1c(0) - 1.5 : p1c(1) = altura + Muros_lista_2(Indice).Hw(indice2) / 100 + 0.15 : p1c(2) = 0
                texto_nivel = AcadDoc.ModelSpace.AddMText(p1c, 0.75, Texto)

                With texto_nivel
                    .Layer = "FC_R-80"
                    .StyleName = "FC_TEXT1"
                    .Height = 0.1
                End With

                'Agregar linea de division del piso
                p1c(0) = Min_x - 0.55 : p1c(1) = altura + Muros_lista_2(Indice).Hw(indice2) / 100 : p1c(2) = 0
                p2c(0) = Min_x - 2.05 : p2c(1) = altura + Muros_lista_2(Indice).Hw(indice2) / 100 : p2c(2) = 0
                Add_Linea(p1c, p2c, "FC_LINEA CORTE")
            Else
                Exit For
            End If

            contador += 1
            altura += Muros_lista_2(Indice).Hw(indice2) / 100
        Next

        ''Add refuerzo
        contador1 = 1

        For i = 0 To Muro_i.Lista_Coordenadas.Count - 1

            For j = 0 To Muro_i.Lista_Coordenadas(i).Count - 1
                Add_LW_PL(Muro_i.Lista_Coordenadas(i)(j), "FC_REFUERZO", 0.6, False)

                If Muro_i.Lista_Barras(i)(j).Contains("T") = True Then
                    Texto_barra = Muro_i.Lista_Barras(i)(j).Substring(0, Muro_i.Lista_Barras(i)(j).IndexOf("T")) & " L=" & "%<\AcObjProp Object(%<\_ObjId " & Polyline_hatch.ObjectID & ">%).Length \f " & Chr(34) & "%lu2%pr2" & Chr(34) & ">%"
                Else
                    Texto_barra = Muro_i.Lista_Barras(i)(j) & " L=" & "%<\AcObjProp Object(%<\_ObjId " & Polyline_hatch.ObjectID & ">%).Length \f " & Chr(34) & "%lu2%pr2" & Chr(34) & ">%"
                End If

                Add_Texto(Texto_barra, Muro_i.Lista_P_Texto(i)(j), "FC_R-80", "FC_TEXT1", Math.PI / 2)
            Next

            ''Adicionar numero de alzado
            Dim prueba2 As Single = Muro_i.Lista_Coordenadas(i)(0).ToList.FindAll(Function(x) Math.Round(x, 1) >= Math.Round(Min_x, 1)).Min
            p1c = {prueba2, -Hfunda - 0.25, 0}
            Num_capas = AcadDoc.ModelSpace.AddText(Str(contador1), p1c, 0.13)
            With Num_capas
                .Layer = "FC_R-100"
                .StyleName = "FC_TEXT"
                .LinetypeScale = 1
            End With
            Num_capas.Update()
            contador1 += 1
        Next

        p1c = {Min_x, -prof, 0}
        p2c = {Min_x, 0, 0}
        location = {p1c(0) - 0.2, -prof / 2, 0}
        Add_Cotas(p1c, p2c, location, location, Math.PI / 2, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDot, True, "")

        ''Add Cotas
        For i = 0 To Muro_i.Lista_Pc1.Count - 1
            For j = 0 To Muro_i.Lista_Pc1(i).Count - 1
                Text_pos = {Muro_i.Lista_Pc1(i)(j)(0) - 0.2, Muro_i.Lista_Pc1(i)(j)(1) + ((Muro_i.Lista_Pc2(i)(j)(1) - Muro_i.Lista_Pc1(i)(j)(1)) / 2), 0}
                Add_Cotas(Muro_i.Lista_Pc1(i)(j), Muro_i.Lista_Pc2(i)(j), Text_pos, Text_pos, Math.PI / 2, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDot, True, "")
            Next
        Next

        Asignar_refuerzo_piso(Muro_i)

    End Sub

    Private Sub Asignar_refuerzo_piso(ByVal Muro_i As Datos_Refuerzo)

        Dim Limite_Sup, Limite_inf As Single
        Dim Indice1, Indice2, indice3, contador As Integer
        Dim Texto As String
        Dim longitud As Single
        Dim Ymin, Ymax As Single
        Dim Hijos As New List(Of Muros_Consolidados)

        Find_Muros_Hijos(Muros_lista_2.Find(Function(x) x.Pier_name = Muro_i.Nombre_muro), Hijos)

        For i = 0 To Muro_i.Stories.Count - 1

            Indice1 = Muros_lista_2.FindIndex(Function(x) x.Pier_name = Muro_i.Nombre_muro)
            Indice2 = Muros_lista_2(Indice1).Stories.FindIndex(Function(x) x = Muro_i.Stories(i))
            indice3 = alzado_lista.FindIndex(Function(x) x.pier = Muro_i.Nombre_muro And x.story = Muro_i.Stories(i))

            Limite_Sup = Math.Round((Muros_lista_2(Indice1).H_acumulado(Indice2) / 100) - Hviga, 2)
            Limite_inf = Math.Round((Limite_Sup - Muros_lista_2(Indice1).Hw(Indice2) / 100) + Hviga, 2)
            alzado_lista(indice3).Alzado_Longitud.Clear()

            contador = 0

            For j = 0 To Muro_i.Lista_Coordenadas.Count - 1
                For k = 0 To Muro_i.Lista_Coordenadas(j).Count - 1

                    Ymin = Math.Round(Muro_i.Lista_Coordenadas(j)(k)(1), 2)
                    Ymax = Math.Round(Muro_i.Lista_Coordenadas(j)(k).Last, 2)

                    If Limite_inf >= Ymin And Limite_Sup <= Ymax Then

                        If Muro_i.Lista_Barras(j)(k).Contains("T") = True Then
                            Texto = Muro_i.Lista_Barras(j)(k).Substring(0, Muro_i.Lista_Barras(j)(k).IndexOf("T")) & " L="
                        Else
                            Texto = Muro_i.Lista_Barras(j)(k) & " L="
                        End If

                        longitud = Calcular_Longitud(Muro_i.Lista_Coordenadas(j)(k))
                        alzado_lista(indice3).Alzado_Longitud.Add(Texto & longitud)

                        For q = 0 To Hijos.Count - 1
                            Dim Indice4 As Integer
                            Indice4 = alzado_lista.FindIndex(Function(x) x.pier = Hijos(q).Pier_name And x.story = Muro_i.Stories(i))
                            alzado_lista(Indice4).Alzado_Longitud.Add(Texto & longitud)
                        Next

                        contador += 1

                    End If
                Next
            Next
        Next

    End Sub

    Private Function Calcular_Longitud(ByVal Coordenadas As Double()) As Single

        Dim Longitud As Single
        Dim Distancia_i As Single
        Dim X_barra, Y_barra As Single

        For i = 0 To Coordenadas.Count - 3 Step 2

            X_barra = Math.Abs(Coordenadas(i) - Coordenadas(i + 2))
            Y_barra = Math.Abs(Coordenadas(i + 1) - Coordenadas(i + 3))
            Distancia_i = X_barra + Y_barra
            Longitud += Distancia_i

        Next

        Return Longitud
    End Function

    Private Sub Add_Texto(ByVal Texto_1 As String, ByVal P_texto As Double(), ByVal Layer As String, ByVal Style As String, ByVal Angulo As Double)

        texto_nivel = AcadDoc.ModelSpace.AddMText(P_texto, 0.75, Texto_1)

        With texto_nivel
            .Layer = Layer
            .StyleName = Style
            .Height = 0.1
            .Rotation = Angulo
            .Width = 1.3
        End With

    End Sub

    Private Function Find_max_x(ByVal Lista_Coordenadas As List(Of List(Of Double()))) As Single

        Dim Max As Single = -99999999

        For i = 0 To Lista_Coordenadas.Count - 1
            For j = 0 To Lista_Coordenadas(i).Count - 1
                If Lista_Coordenadas(i)(j)(0) >= Max Then
                    Max = Lista_Coordenadas(i)(j)(0)
                End If
            Next
        Next
        Return Max

    End Function

    Private Function Find_min_x(ByVal Lista_Coordenadas As List(Of List(Of Double()))) As Single

        Dim Min As Single = 99999999
        Dim prueba As Single

        For i = 0 To Lista_Coordenadas.Count - 1
            For j = 0 To Lista_Coordenadas(i).Count - 1
                prueba = Lista_Coordenadas(i)(j)(0)
                If Lista_Coordenadas(i)(j)(0) <= Min Then
                    Min = Lista_Coordenadas(i)(j)(0)
                End If
            Next
        Next

        Return Min

    End Function

    Private Sub Add_Cotas_Especiales(ByRef indice As Integer, ByRef Indice2 As Integer, Lista_Gnl As List(Of Single), delta As Double, ByVal Caso As String)
        ''Add Cotas cambio de resistencia

        Dim H_inicial, H_final, Sum_H As Double

        For i = Lista_Gnl.Count - 1 To 0 Step -1

            If Caso = "Espesores" Then
                indice = datos_muros.Bw.FindIndex(Function(x) x = Lista_Gnl(i))
                Indice2 = datos_muros.Bw.FindLastIndex(Function(x) x = Lista_Gnl(i))
                Texto = "Muro e=" & Format(Lista_Gnl(i) / 100, "##,0.00")
            Else
                indice = datos_muros.fc.FindIndex(Function(x) x = Lista_Gnl(i))
                Indice2 = datos_muros.fc.FindLastIndex(Function(x) x = Lista_Gnl(i))
                Texto = "Muro en concreto f'c=" & Lista_Gnl(i) & " kgf/cm²"
            End If

            H_inicial = Sum_H
            For j = Indice2 To indice Step -1
                Sum_H += datos_muros.Hw(j) / 100
            Next

            H_final = Sum_H
            p1c = {coordX + datos_muros.lw(i) / 100, H_inicial, 0}
            p2c = {coordX + datos_muros.lw(i) / 100, H_final, 0}
            location = {datos_muros.lw(i) / 100 + coordX + delta, H_inicial + (H_final - H_inicial) / 2, Math.PI / 2}
            Add_Cotas(p1c, p2c, location, location, Math.PI / 2, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDefault, True, Texto)

        Next
    End Sub

    Private Sub Add_Ref_Htal(L_Ebe_Izq As Double, L_Ebe_Der As Double, L_Zc_Izq As Double, L_Zc_Der As Double, i As Integer)

        Dim L_Izq, L_Der As Double
        Dim Delta_I, Delta_D As Double
        Dim longitud, Distancia As Double

        Ref_bloc_name = "FC_B_Refuerzo horizontal muros"

        If L_Ebe_Izq > 0 Then
            L_Izq = L_Ebe_Izq / 100
        Else
            L_Izq = L_Zc_Izq / 100
        End If

        If L_Ebe_Der > 0 Then
            L_Der = L_Ebe_Der / 100
        Else
            L_Der = L_Zc_Der / 100
        End If

        If L_Izq >= 0.45 Then
            Delta_I = 0.15
            pc = {coordX + 0.112, altura + 0.07, 0}
        Else
            Delta_I = 0.05
            pc = {coordX + 0.012, altura + 0.07, 0}
        End If

        If L_Der >= 0.45 Then
            Delta_D = 0.15
            Distancia = (coordX + (datos_muros.lw(i) / 100) - 0.188) - pc(0)
        Else
            Delta_D = 0.05
            Distancia = (coordX + (datos_muros.lw(i) / 100) - 0.088) - pc(0)
        End If

        Texto = Gancho_horizontal(L_Izq, L_Der)

        If Texto = "Gancho izquierda" Or Texto = "Gancho derecha" Then
            longitud = Distancia + ganchos_180(datos_muros.Ref_htal(i))
        ElseIf Texto = "Doble gancho" Then
            longitud = Distancia + 2 * ganchos_180(datos_muros.Ref_htal(i))
        ElseIf Texto = "Sin Gancho" Then
            longitud = Distancia
        End If

        ''Adicion de refuerzo en la zona inferior
        Set_Ref_htal(pc, Distancia, Texto)
        ''Adicion de refuerzo en la zona superior
        Set_Ref_htal({pc(0), altura + datos_muros.Hw(i) / 100 - Hviga - 0.07, 0}, Distancia, Texto)

        'Agregar cota para indicar el tipo de refuerzo
        p1c = {coordX + datos_muros.lw(i) / 200, pc(1), 0}
        p2c = {coordX + datos_muros.lw(i) / 200, altura + datos_muros.Hw(i) / 100 - Hviga - 0.07, 0}
        location = {coordX + datos_muros.lw(i) / 200, (p2c(1) - p1c(1)) / 2 + altura, 0}

        Add_Cotas(p1c, p2c, location, location, Math.PI * 0.5, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDefault, False, "")

        'Agregar texto para indicar el tipo de refuerzo y el espaciamiento
        If datos_muros.Capas_htal(i) = 1 Then
            If InStr(datos_muros.Ref_htal(i), "mm") Then
                Texto = "%%C" & datos_muros.Ref_htal(i) & " a" & vbNewLine & Format((datos_muros.sep_htal(i)) / 100, "##0.00") & " L= X.XX"
            Else
                Texto = "#" & datos_muros.Ref_htal(i) & " a " & Format(datos_muros.sep_htal(i) / 100, "##0.00") & vbNewLine & " L= X.XX"
            End If

        ElseIf datos_muros.Capas_htal(i) = 2 Then

            If InStr(datos_muros.Ref_htal(i), "mm") Then
                Texto = "2%%C" & datos_muros.Ref_htal(i) & " a" & vbNewLine & Format(datos_muros.sep_htal(i) / 100, "##0.00") & " L= X.XX"
            Else
                Texto = "2#" & datos_muros.Ref_htal(i) & " a" & vbNewLine & Format(datos_muros.sep_htal(i) / 100, "##0.00") & " L= X.XX"
            End If
        End If

        Texto_Horizontal = AcadDoc.ModelSpace.AddMText(location, 1.2, Texto)
        With Texto_Horizontal
            .Rotation = Math.PI * 0.5
            .AttachmentPoint = AcAttachmentPoint.acAttachmentPointMiddleCenter
            .Layer = "FC_R-80"
            .StyleName = "FC_TEXT1"
            .InsertionPoint = location
        End With
        Texto_Horizontal.Update()

    End Sub

    Private Sub Set_Ref_htal(ByVal I_Point As Double(), ByVal distancia As Double, ByVal Texto1 As String)

        Refbloc = AcadDoc.ModelSpace.InsertBlock(I_Point, Ref_bloc_name, 0.75, 0.75, 0.75, 0)
        Dim dynamic_property As Object = Refbloc.GetDynamicBlockProperties
        Dim editar_property As AcadDynamicBlockReferenceProperty

        editar_property = dynamic_property(0)
        editar_property.Value = Texto1

        editar_property = dynamic_property(position(Texto1))
        editar_property.Value = distancia

        With Refbloc
            .Layer = "FC_REFUERZO"
            .Update()
        End With

    End Sub

    Private Sub Add_Confinamiento(L_Ebe_Izq As Double, L_Ebe_Der As Double, L_Zc_Izq As Double, L_Zc_Der As Double, i As Integer)

        Dim L_Izq, L_Der As Double
        Dim Hatch_pattern_Izq, Hatch_pattern_Der As String
        Dim Hatch_Layer_Izq, Hatch_Layer_Der As String
        Dim Est_izq, Est_Der As String
        Dim Sep_Izq, Sep_Der As Double
        Dim mirror As Integer

        If L_Ebe_Izq > 0 Then
            L_Izq = L_Ebe_Izq / 100
            Hatch_pattern_Izq = "SOLID"
            Hatch_Layer_Izq = "FC_HATCH MUROS"
            Est_izq = datos_muros.Est_ebe(i)
            Sep_Izq = datos_muros.Sep_ebe(i) / 100
        Else
            L_Izq = L_Zc_Izq / 100
            Hatch_pattern_Izq = "DOTS"
            Hatch_Layer_Izq = "FC_HATCH 252"
            Est_izq = datos_muros.Est_Zc(i)
            Sep_Izq = datos_muros.Sep_Zc(i) / 100
        End If

        If L_Ebe_Der > 0 Then
            L_Der = L_Ebe_Der / 100
            Hatch_pattern_Der = "SOLID"
            Hatch_Layer_Der = "FC_HATCH MUROS"
            Est_Der = datos_muros.Est_ebe(i)
            Sep_Der = datos_muros.Sep_ebe(i) / 100
        Else
            L_Der = L_Zc_Der / 100
            Hatch_pattern_Der = "DOTS"
            Hatch_Layer_Der = "FC_HATCH 252"
            Est_Der = datos_muros.Est_Zc(i)
            Sep_Der = datos_muros.Sep_Zc(i) / 100
        End If

        If L_Izq > 0 Or L_Der > 0 Then

            If L_Izq > 0 And L_Der = 0 Then

                With datos_muros
                    puntos_hatch_EBE = {coordX, altura, (L_Izq) + coordX, altura, L_Izq + coordX, altura + .Hw(i) / 100 - Hviga,
                       coordX, altura + .Hw(i) / 100 - Hviga}
                End With

                Add_LW_PL(puntos_hatch_EBE, "FC_BORDES", 0.9, True)
                TextId = Polyline_hatch.ObjectID
                Add_Hatch(Polyline_hatch, Hatch_pattern_Izq, Hatch_Layer_Izq, 0.9)

                'Agregar leader para definir el diametro y la separacion de los estribos en las zonas confindas

                Texto = "E#" & Est_izq & " a " & Sep_Izq & "m"
                puntos_leader_2 = {coordX + datos_muros.lw(i) / 100 - 0.2, altura + datos_muros.Hw(i) / 200, 0, coordX + datos_muros.lw(i) / 100 + 0.5, altura + datos_muros.Hw(i) / 200, 0}
                mirror = 0
                Agregar_estribos(datos_muros, puntos_leader_2, mirror, i) ''Metodo para agregar leader en las zonas de confinamiento y de elementos especiales de borde

                'Agregar cota que define el espesor de la zona de confinamiento

                p1c(0) = coordX : p1c(1) = altura + (datos_muros.Hw(i) / 100) * (2 / 3) : p2c(2) = 0
                Add_Long_Ebe("FC_B_Espesor Horizontal", p1c, L_Izq, 0)

            ElseIf L_Der > 0 And L_Izq = 0 Then

                With datos_muros
                    puntos_hatch_EBE = {coordX + (.lw(i) / 100), altura, coordX + (.lw(i) / 100) - (L_Der), altura, coordX + .lw(i) / 100 - L_Der, altura + .Hw(i) / 100 - Hviga,
                       coordX + .lw(i) / 100, altura + .Hw(i) / 100 - Hviga}
                End With

                Add_LW_PL(puntos_hatch_EBE, "FC_BORDES", 0.9, True)
                TextId = Polyline_hatch.ObjectID
                Add_Hatch(Polyline_hatch, Hatch_pattern_Der, Hatch_Layer_Der, 0.9)

                'Agregar leader para definir el diametro y la separacion de los estribos en las zonas confindas

                Texto = "E#" & Est_Der & " a " & Sep_Der & "m"
                puntos_leader_2 = {coordX + datos_muros.lw(i) / 100 - 0.2, altura + datos_muros.Hw(i) / 200, 0, coordX + datos_muros.lw(i) / 100 + 0.5, altura + datos_muros.Hw(i) / 200, 0}
                mirror = 1
                Agregar_estribos(datos_muros, puntos_leader_2, mirror, i) ''Metodo para agregar leader en las zonas de confinamiento y de elementos especiales de borde

                'Agregar cota que define el espesor de la zona de confinamiento

                p1c(0) = datos_muros.lw(i) / 100 + coordX : p1c(1) = altura + (datos_muros.Hw(i) / 100) * (2 / 3) : p2c(2) = 0
                Add_Long_Ebe("FC_B_Espesor Horizontal", p1c, L_Der, 1)
            Else

                With datos_muros
                    Dim puntos_hatch_EBE_1 = {coordX, altura, (L_Izq) + coordX, altura, L_Izq + coordX, altura + .Hw(i) / 100 - Hviga,
                       coordX, altura + .Hw(i) / 100 - Hviga}

                    Dim puntos_hatch_EBE_2 = {coordX + (.lw(i) / 100), altura, coordX + (.lw(i) / 100) - (L_Der), altura, coordX + .lw(i) / 100 - L_Der, altura + .Hw(i) / 100 - Hviga,
                       coordX + .lw(i) / 100, altura + .Hw(i) / 100 - Hviga}

                    Add_LW_PL(puntos_hatch_EBE_1, "FC_BORDES", 0.9, True)
                    TextId = Polyline_hatch.ObjectID
                    Add_Hatch(Polyline_hatch, Hatch_pattern_Izq, Hatch_Layer_Izq, 0.9)

                    'Agregar leader para definir el diametro y la separacion de los estribos en las zonas confindas

                    Texto = "E#" & Est_izq & " a " & Sep_Izq & "m"
                    puntos_leader_2 = {coordX + datos_muros.lw(i) / 100 - 0.2, altura + datos_muros.Hw(i) / 200, 0, coordX + datos_muros.lw(i) / 100 + 0.5, altura + datos_muros.Hw(i) / 200, 0}
                    mirror = 0
                    Agregar_estribos(datos_muros, puntos_leader_2, mirror, i) ''Metodo para agregar leader en las zonas de confinamiento y de elementos especiales de borde

                    'Agregar cota que define el espesor de la zona de confinamiento a la izquierda

                    p1c(0) = coordX : p1c(1) = altura + (datos_muros.Hw(i) / 100) * (2 / 3) : p2c(2) = 0
                    Add_Long_Ebe("FC_B_Espesor Horizontal", p1c, L_Izq, 0)

                    Add_LW_PL(puntos_hatch_EBE_2, "FC_BORDES", 0.9, True)
                    TextId = Polyline_hatch.ObjectID
                    Add_Hatch(Polyline_hatch, Hatch_pattern_Der, Hatch_Layer_Der, 0.9)

                    'Agregar leader para definir el diametro y la separacion de los estribos en las zonas confindas
                    Texto = "E#" & Est_izq & " a " & Sep_Izq & "m"
                    puntos_leader_2 = {coordX + datos_muros.lw(i) / 100 - 0.2, altura + datos_muros.Hw(i) / 200, 0, coordX + datos_muros.lw(i) / 100 + 0.5, altura + datos_muros.Hw(i) / 200, 0}
                    mirror = 1
                    Agregar_estribos(datos_muros, puntos_leader_2, mirror, i) ''Metodo para agregar leader en las zonas de confinamiento y de elementos especiales de borde

                    'Agregar cota que define el espesor de la zona de confinamiento a la derecha

                    p1c(0) = datos_muros.lw(i) / 100 + coordX : p1c(1) = altura + (datos_muros.Hw(i) / 100) * (2 / 3) : p2c(2) = 0
                    Add_Long_Ebe("FC_B_Espesor Horizontal", p1c, L_Der, 1)
                End With

            End If

        End If

    End Sub

    Private Sub Add_Malla(L_Ebe_Izq As Double, L_Ebe_Der As Double, L_Zc_Izq As Double, L_Zc_Der As Double, i As Integer)
        Dim L_Izq, L_Der As Double
        Dim Delta_I, Delta_D As Double

        If L_Ebe_Izq > 0 Then
            L_Izq = L_Ebe_Izq / 100
        Else
            L_Izq = L_Zc_Izq / 100
        End If

        If L_Ebe_Der > 0 Then
            L_Der = L_Ebe_Der / 100
        Else
            L_Der = L_Zc_Der / 100
        End If

        If L_Izq >= 0.45 Then
            Puntos_malla(0) = coordX + 0.15
            Puntos_malla(6) = coordX + 0.15
            Delta_I = 0.15
        Else
            Puntos_malla(0) = coordX + 0.05
            Puntos_malla(6) = coordX + 0.05
            Delta_I = 0.05
        End If

        If L_Der >= 0.45 Then
            Puntos_malla(2) = coordX + datos_muros.lw(i) / 100 - 0.15
            Puntos_malla(4) = coordX + datos_muros.lw(i) / 100 - 0.15
            Delta_D = 0.15
        Else
            Puntos_malla(2) = coordX + datos_muros.lw(i) / 100 - 0.05
            Puntos_malla(4) = coordX + datos_muros.lw(i) / 100 - 0.05
            Delta_D = 0.05
        End If

        Puntos_malla(1) = altura + 0.05
        Puntos_malla(3) = Puntos_malla(1)

        If i > 0 Then
            Puntos_malla(5) = altura + (datos_muros.Hw(i) / 100) + 0.35
            pm1 = {coordX + datos_muros.lw(i) / 200, datos_muros.Hw(i) / 100 + 0.05 + altura, 0}
            pm2 = {coordX + datos_muros.lw(i) / 200, datos_muros.Hw(i) / 100 + 0.35 + altura, 0}
            location = {coordX + datos_muros.lw(i) / 200, (pm1(1) + pm2(1)) / 2, 0}
            ''Agrgar cota
            Add_Cotas(pm1, pm2, location, location, Math.PI / 2, "FC_COTAS", "FC_COTAS", AcDimArrowheadType.acArrowDot, True, "")
        Else
            Puntos_malla(5) = altura + (datos_muros.Hw(i) / 100) - 0.05
        End If

        Puntos_malla(7) = Puntos_malla(5)
        Add_LW_PL(Puntos_malla, "FC_MALLA", 0.35, True)

        'Insertar Bloque
        Add_Bloque_malla(datos_muros.lw(i) / 100, datos_muros.Hw(i) / 100, L_Izq, L_Der, Delta_I, Delta_D, i)
    End Sub

    Private Sub Add_Bloque_malla(ByVal Longitud As Double, Hw As Double, ByVal LI As Double, LD As Double, ByVal Delta_I As Double, ByVal Delta_D As Double, ByVal I As Integer)

        Dim Radio As Double = 0.282
        Dim Puntos_Bloque(2) As Double

        Ref_bloc_name = "FC_B_Convencion de malla"

        If Longitud - 0.3 > Radio * 2 Then
            Puntos_Bloque = {coordX + Longitud / 2, altura + (2 / 3) * (Hw - Hviga), 0}
        Else
            Puntos_Bloque = {coordX + Longitud + 0.8, altura + (2 / 3) * (Hw - Hviga) - 0.4, 0}
        End If

        Refbloc = AcadDoc.ModelSpace.InsertBlock(Puntos_Bloque, Ref_bloc_name, 1, 1, 1, 0)
        Refbloc.XScaleFactor = 50
        Refbloc.Visible = False

        Dim dynamic_property As Object = Refbloc.GetDynamicBlockProperties
        Dim editar_property As AcadDynamicBlockReferenceProperty

        If Longitud - 0.3 > Radio * 2 Then
            editar_property = dynamic_property(4)
            editar_property.Value = Longitud / 2 - Delta_D
            editar_property = dynamic_property(6)
            editar_property.Value = -Longitud / 2 + Delta_I
            editar_property = dynamic_property(7)
            editar_property.Value = -(2 / 3) * (Hw - Hviga) + 0.05
        Else
            editar_property = dynamic_property(4)
            editar_property.Value = -(Longitud + 0.8) + Delta_I
            editar_property = dynamic_property(6)
            editar_property.Value = -(Longitud + 0.8) + Delta_D
            editar_property = dynamic_property(7)
            editar_property.Value = -(2 / 3) * (Hw - Hviga) + 0.45
        End If

        editar_property = dynamic_property(5)

        If I > 0 Then
            editar_property.Value = (1 / 3) * (Hw - Hviga) + Hviga + 0.35
        Else
            editar_property.Value = (1 / 3) * (Hw - Hviga) + Hviga - 0.05
        End If

        'Añadir texto de malla

        If InStr(datos_muros.Malla(I), "DD") Then
            pos = InStr(datos_muros.Malla(I), "DD")
            Texto = "Doble malla " & vbNewLine & "no central " & vbNewLine & Mid(datos_muros.Malla(I), pos + 1, 1) & "-" & Mid(datos_muros.Malla(I), pos + 2)
        Else
            Texto = "Malla " & vbNewLine & "central " & vbNewLine & Mid(datos_muros.Malla(I), 1, 1) & "-" & Mid(datos_muros.Malla(I), 2)
        End If

        With Refbloc
            .Layer = "FC_MALLA"
            .GetAttributes()(0).TextString = Texto
            .GetAttributes()(0).MTextBoundaryWidth = 1.05
            .Visible = True
            .Update()
        End With

        Refbloc.Visible = True

    End Sub

    Private Sub Add_Long_Ebe(ByVal Bloc_name As String, ByVal Coor As Double(), ByVal L_conf As Double, ByVal mirror As Integer)

        Ref_bloc_name = Bloc_name
        Refbloc = AcadDoc.ModelSpace.InsertBlock(Coor, Ref_bloc_name, 0.5, 0.5, 0.5, 0)
        Refbloc.XScaleFactor = 50

        Dim dynamic_property As Object = Refbloc.GetDynamicBlockProperties
        Dim editar_property As AcadDynamicBlockReferenceProperty

        editar_property = dynamic_property(0)
        editar_property.Value = L_conf

        editar_property = dynamic_property(2)
        editar_property.Value = 0.6

        If mirror = 1 Then
            editar_property = dynamic_property(4)
            Dim shortValue1 As Short = 1
            editar_property.Value = shortValue1
        End If

        With Refbloc
            .Layer = "FC_COTAS"
            .GetAttributes()(0).TextString = Format(L_conf, "##,0.00")
            .GetAttributes()(0).Height = 0.1
        End With

        Refbloc.Update()
    End Sub

    Private Sub Add_Hatch(ByVal Acad_Ent As AcadEntity, ByVal Pattern As String, ByVal Layer As String, ByVal Escala As Double)

        pattername = Pattern
        hatch = AcadDoc.ModelSpace.AddHatch(0, pattername, True)
        outerLoop(0) = Acad_Ent

        With hatch
            .AppendOuterLoop(outerLoop)
            .Layer = Layer
            .LinetypeScale = Escala
            .PatternAngle = 45
            .PatternScale = 0.009
            .PatternSpace = 0.009
            .Update()
        End With

        Hatch_Back(hatch)
    End Sub

    Private Sub Add_LW_PL(ByVal coord() As Double, ByVal Layer As String, ByVal Line_Scale As Double, ByVal IsClosed As Boolean)
        Polyline_hatch = AcadDoc.ModelSpace.AddLightWeightPolyline(coord)

        With Polyline_hatch
            .Closed = IsClosed
            .Layer = Layer
            .LinetypeScale = Line_Scale
        End With
        Polyline_hatch.Update()
    End Sub

    Private Sub Add_Linea(ByVal P1 As Double(), ByVal P2 As Double(), ByVal Layer As String)

        linea = AcadDoc.ModelSpace.AddLine(P1, P2)
        With linea
            .Layer = Layer
            .LinetypeScale = 0.35
        End With
    End Sub

    Private Sub Add_Cotas(ByVal P1 As Double(), ByVal P2 As Double(), ByVal Pos As Double(), ByVal Text_Pos As Double(), ByVal Angulo As Double, ByVal Layer As String, ByVal Name_Style As String, ByVal ArrowheadType As ArrowDirection, ByVal Text_inside As Boolean, ByVal Texto As String)
        cota = AcadDoc.ModelSpace.AddDimRotated(P1, P2, Pos, Angulo)

        With cota
            .Layer = Layer
            .StyleName = Name_Style
            .TextHeight = 0.0015
            .Arrowhead1Type = ArrowheadType
            .Arrowhead2Type = ArrowheadType
            .ArrowheadSize = 0.001

            If Texto <> "" Then
                .TextOverride = Texto
                .TextStyle = "FC_TEXT1"
                .TextColor = 11
                .TextRotation = Math.PI / 2
            End If

            If Text_inside = False Then
                .TextInside = False
                .TextOverride = " "
                .TextHeight = 0.1
            End If
            .TextPosition = Text_Pos
            .Update()
        End With
    End Sub

    Private Sub Add_Polilineas(ByVal Layer As String, ByVal Coord As Double(), ByVal Line_Scale As Double)
        Polyline = AcadDoc.ModelSpace.AddPolyline(Coord)
        With Polyline
            .Closed = True
            .Layer = Layer
            .LinetypeScale = Line_Scale
        End With
        Polyline.Update()
    End Sub

    Private Sub Agregar_estribos(ByVal elemento As Muros_Consolidados, ByVal puntos_leader() As Double, ByVal mirror As Integer, ByVal Indice As Integer)
        Dim mirror_object As AcadMLeader
        Dim punto1(0 To 2) As Double
        Dim punto2(0 To 2) As Double

        multileader = AcadDoc.ModelSpace.AddMLeader(puntos_leader, 1)
        'multileader.ScaleFactor = 1 / 50
        With multileader
            .Layer = "FC_TEXTO NO IMPRIMIR"
            .color = ACAD_COLOR.acByLayer
            .ArrowheadType = AcDimArrowheadType.acArrowDefault
            .ArrowheadSize = 0.02
            .TextString = Texto
            .TextJustify = AcAttachmentPoint.acAttachmentPointTopLeft
            .TextStyleName = "FC_TEXT1"
            .TextWidth = 0.0015
            .TextHeight = 0.0015
            .TextLeftAttachmentType = AcTextAttachmentType.acAttachmentBottomOfTopLine
            .color = ACAD_COLOR.acByLayer
            .LandingGap = 0
        End With

        If mirror = 1 Then
            punto1 = {(coordX + puntos_leader(0) + 0.2) / 2, altura + elemento.Hw(Indice) / 200, 0}
            punto2 = {(coordX + puntos_leader(0) + 0.2) / 2, altura, 0}
            mirror_object = multileader.Mirror(punto1, punto2)
            multileader.Delete()
        End If

    End Sub

    Private Sub Hatch_Back(ByVal Acad_object As AcadObject)

        Dim Diccionario As AcadDictionary
        Dim sentityObj As AcadObject
        Dim arr(0) As AcadObject

        'Mover el hacth hacia atras
        Diccionario = AcadDoc.ModelSpace.GetExtensionDictionary

        sentityObj = Diccionario.GetObject("ACAD_SORTENTS")
        sentityObj = Diccionario.AddObject("ACAD_SORTENTS", "AcDbSortentsTable")

        arr(0) = Acad_object
        sentityObj.MoveToBottom(arr.ToArray)

    End Sub

    Private Sub Bloque_Info_alzado(ByRef coordenadas() As Double, ByVal Alto_texto As Double)

        Refbloc = AcadDoc.ModelSpace.InsertBlock(coordenadas, Ref_bloc_name, 1, 1, 1, 0)
        Refbloc.XScaleFactor = 50
        Refbloc.Visible = False

        Dim atributos_bloque As AcadAttributeReference
        atributos_bloque = Refbloc.GetAttributes()(0)

        Dim boundary As Double
        Dim min_point(2) As Double
        Dim max_point(2) As Double

        With atributos_bloque
            .TextString = Texto
            .StyleName = "FC_TEXT1"
            .Height = Alto_texto
            .GetBoundingBox(min_point, max_point)
        End With

        Dim dynamic_property As Object = Refbloc.GetDynamicBlockProperties
        Dim editar_property As AcadDynamicBlockReferenceProperty

        boundary = max_point(0) - min_point(0)
        editar_property = dynamic_property(0)
        editar_property.Value = boundary

        Refbloc.Visible = True
        Refbloc.Update()
    End Sub

End Module