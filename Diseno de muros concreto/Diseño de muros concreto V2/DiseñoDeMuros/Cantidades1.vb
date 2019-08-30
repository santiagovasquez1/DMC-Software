Imports System.IO

Module Cantidades1

    Sub GenerarCantidades(ByVal Lista_Alzado As List(Of alzado_muro), ByVal Lista_Muros As List(Of Muros_Consolidados))

        Dim ArchivoTexto As New List(Of String)
        Dim Lista_Muros_Refuerzo As New List(Of Datos_Refuerzo)
        organizar_Alzados_1()

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

        For i = 0 To Lista_Muros_Refuerzo.Count - 1
            If i = 0 Then
                ArchivoTexto.Add(Lista_Muros_Refuerzo.Count)
            End If
            ArchivoTexto.Add("Muro " & Lista_Muros_Refuerzo(i).Nombre_muro)
            Dim CantidadMuros As Integer = 1
            ArchivoTexto.Add(CantidadMuros)

            With Lista_Muros_Refuerzo(i)
                Dim ListaAuxiliarMuro = Lista_Muros.Find(Function(x) x.Pier_name = Lista_Muros_Refuerzo(i).Nombre_muro)
                ArchivoTexto.Add(.ListaRefuerzo_DllNet.Count + ListaAuxiliarMuro.CantidadMallasDllNet.Count)

                For j = 0 To .ListaRefuerzo_DllNet.Count - 1
                    ArchivoTexto.Add(.ListaRefuerzo_DllNet(j))
                Next

                For j = 0 To ListaAuxiliarMuro.CantidadMallasDllNet.Count - 1
                    ArchivoTexto.Add(ListaAuxiliarMuro.CantidadMallasDllNet(j))
                Next

            End With

        Next

        EscrbirTexto(ArchivoTexto)

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

        Dim Escritor As StreamWriter
        Escritor = New StreamWriter(RutaTxt)

        For i = 0 To TextoAEscribir.Count - 1
            Escritor.WriteLine(TextoAEscribir(i))
        Next

        Escritor.Close()

    End Sub

End Module