﻿Imports System.IO

Module Cantidades1

    Sub GenerarCantidades(ByVal Lista_Alzado As List(Of alzado_muro), ByVal Lista_Muros As List(Of Muros_Consolidados))

        Dim ArchivoTexto As New List(Of String)
        Dim Lista_Muros_Refuerzo As New List(Of Datos_Refuerzo)
        organizar_Alzados_1()

        For i = 0 To Lista_Muros.Count - 1
            OrganizarAlzados2(Lista_Muros(i))
        Next
        For i = 0 To Lista_Muros.Count - 1

            If alzado_lista.Count > 0 And alzado_lista.Exists(Function(x) x.pier = Lista_Muros(i).Pier_name) = True Then
                Dim MuroDatosRefuerzo As New Datos_Refuerzo
                MuroDatosRefuerzo.Nombre_muro = Lista_Muros(i).Pier_name

                MuroDatosRefuerzo.Load_Coordinates(Lista_Muros(i).Pier_name, 0)
                Lista_Muros_Refuerzo.Add(MuroDatosRefuerzo)
            End If
        Next

        For i = 0 To Lista_Muros_Refuerzo.Count - 1
            Lista_Muros_Refuerzo(i).ActivarNuevasFunciones()
        Next

        For i = 0 To Lista_Muros.Count - 1
            Lista_Muros(i).CantidadMallas_()
        Next

        For i = 0 To Muros_lista_2.Count - 1


            Dim MuroAnalizar As Muros_Consolidados = Muros_lista_2(i)
            Dim Ca_RefuerzoHorizontal As Integer = 0
            Dim Ca_Estribos As Integer = 0 : Dim Ca_Ganchos As Integer = 0
            Dim CantidadMuros As Integer = 1
            Dim CantidadMuros_Totales As Integer = 0
            For j = 0 To Muros_lista_2.Count - 1
                If Muros_lista_2(j).isMuroMaestro Then
                    CantidadMuros_Totales = CantidadMuros_Totales + 1
                End If
            Next
            If i = 0 Then
                    ArchivoTexto.Add(CantidadMuros_Totales)
                End If
                If MuroAnalizar.isMuroMaestro Then

                    For j = 0 To Muros_lista_2.Count - 1



                        If MuroAnalizar.Pier_name <> Muros_lista_2(j).Pier_name And Muros_lista_2(j).MuroSimilar IsNot Nothing Then
                            If Muros_lista_2(j).MuroSimilar.Pier_name = MuroAnalizar.Pier_name Then
                                CantidadMuros = CantidadMuros + 1
                            End If
                        End If
                    Next


                    ArchivoTexto.Add("Muro " & Lista_Muros_Refuerzo(i).Nombre_muro)


                    ArchivoTexto.Add(CantidadMuros)
                    With MuroAnalizar


                        Dim ListaAuxiliarMuro = Lista_Muros_Refuerzo.Find(Function(x) x.Nombre_muro = .Pier_name)
                        Dim ListaAuxiliarMuro2 = Lista_Cantidades1.ListaRefuerzoHorzontal.Find(Function(x) x.NombreMuro = .Pier_name)

                        If ListaAuxiliarMuro2 IsNot Nothing Then
                            Ca_RefuerzoHorizontal = ListaAuxiliarMuro2.RefuerzoHorzontalDllnet.Count
                            Ca_Estribos = ListaAuxiliarMuro2.Lista_Nomeclatura_EstribosDllNet.Count
                            Ca_Ganchos = ListaAuxiliarMuro2.Lista_Ganchos_NomencDllNet.Count
                        End If

                        ArchivoTexto.Add(ListaAuxiliarMuro.ListaRefuerzo_DllNet.Count + .CantidadMallasDllNet.Count + Ca_RefuerzoHorizontal + Ca_Ganchos + Ca_Estribos)

                        For j = 0 To ListaAuxiliarMuro.ListaRefuerzo_DllNet.Count - 1
                            ArchivoTexto.Add(ListaAuxiliarMuro.ListaRefuerzo_DllNet(j))
                        Next

                        For j = 0 To .CantidadMallasDllNet.Count - 1
                            ArchivoTexto.Add(.CantidadMallasDllNet(j))
                        Next
                        If ListaAuxiliarMuro2 IsNot Nothing Then
                            For j = 0 To Ca_RefuerzoHorizontal - 1
                                ArchivoTexto.Add(ListaAuxiliarMuro2.RefuerzoHorzontalDllnet(j))
                            Next

                            For j = 0 To ListaAuxiliarMuro2.Lista_Ganchos_NomencDllNet.Count - 1
                                ArchivoTexto.Add(ListaAuxiliarMuro2.Lista_Ganchos_NomencDllNet(j))
                            Next
                            For j = 0 To ListaAuxiliarMuro2.Lista_Nomeclatura_EstribosDllNet.Count - 1
                                ArchivoTexto.Add(ListaAuxiliarMuro2.Lista_Nomeclatura_EstribosDllNet(j))
                            Next
                        End If


                    End With
                End If
            Next



            EscrbirTexto(ArchivoTexto)

    End Sub


    Sub OrganizarAlzados2(Muro As Muros_Consolidados)

        Dim alzado_lista_aux As List(Of alzado_muro) = New List(Of alzado_muro)
        Dim Num_cols As Integer
        Dim indice As Integer
        If alzado_lista.Count > 0 And alzado_lista.Exists(Function(x) x.pier = Muro.Pier_name) = True Then

            alzado_lista_aux = alzado_lista.FindAll(Function(x) x.pier = Muro.Pier_name)
            Num_cols = alzado_lista_aux.Select(Function(x) x.alzado.Count).ToList().Max

            If Num_cols > 0 Then

                For j = 0 To alzado_lista_aux.Count - 1

                    For k = 0 To alzado_lista_aux(j).alzado.Count - 1
                        If alzado_lista_aux(j).alzado(k) = Nothing Then
                            alzado_lista_aux(j).alzado(k) = ""
                        End If
                    Next

                    If alzado_lista_aux(j).alzado.Count < Num_cols Then
                        For k = alzado_lista_aux(j).alzado.Count To Num_cols - 1
                            indice = alzado_lista.FindIndex(Function(x) x.pier = alzado_lista_aux(j).pier And x.story = alzado_lista_aux(j).story)
                            alzado_lista(indice).alzado.Add("")
                        Next
                    End If

                Next

            End If
        End If


    End Sub
    Sub EscrbirTexto(ByVal TextoAEscribir As List(Of String))
        Dim RutaTxt As String
        Dim GuardarTxt As New SaveFileDialog
        With GuardarTxt
            .Title = "Cantidades"
            .Filter = "Guardar Cantidades|*.txt"
            .ShowDialog()
        End With
        RutaTxt = GuardarTxt.FileName


        If RutaTxt <> "" Then
            Dim Escritor As StreamWriter
            Escritor = New StreamWriter(RutaTxt)

            For i = 0 To TextoAEscribir.Count - 1
                Escritor.WriteLine(TextoAEscribir(i))
            Next
            Escritor.Close()
        End If


    End Sub

End Module