Imports System.IO
Module Cantidades1


    Sub GenerarCantidades(ByVal Lista_Alzado As List(Of alzado_muro), ByVal Lista_Muros As List(Of Muros_Consolidados))

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


    End Sub




    Sub ArchivoTexto(ByVal TextoAEscribir As List(Of String))
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
