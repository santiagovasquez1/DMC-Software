Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports Diseño_de_muros_concreto_V2


Public Class Serializador
    Public Sub New(ByVal Ruta As String, ByVal Guardar As Boolean, Lista_serializadas As Listas_serializadas)

        'Dim Serializar_aux As Listas_serializadas = New Listas_serializadas

        'If Guardar = True Then
        '    If Muros_lista_2 Is Nothing = True Then
        '        Serializar(Ruta, Lista_serializadas)
        '    Else
        '        Serializar_aux = New Listas_serializadas With {
        '            .Lista_Muros = Muros_lista_2,
        '            .Lista_Alzados = alzado_lista,
        '            .lista_refuerzo = refuerzo_lista,
        '            .Capacidad_proyecto = Capacidad,
        '            .Muros_generales = Muros_generales
        '        }
        '        Serializar(Ruta, Serializar_aux)
        '    End If
        'Else
        '    Deserializar(Ruta, Lista_serializadas)
        'End If

    End Sub
    Public Shared Sub Serializar(ByRef ruta As String, ByVal Lista_serializadas As Listas_serializadas)

        'Dim Formatter As BinaryFormatter = New BinaryFormatter

        'If ruta = "" Then
        '    Dim Myfile As SaveFileDialog = New SaveFileDialog
        '    With Myfile
        '        .Title = "Información Muros"
        '        .Filter = "Guardar Archivo |*.dmc"
        '    End With

        '    Myfile.ShowDialog()
        '    ruta = Myfile.FileName
        'End If
        'Ruta_archivo = ruta
        'Dim Escritor As Stream = New FileStream(ruta, FileMode.Create, FileAccess.Write, FileShare.None)
        'Formatter.Serialize(Escritor, Lista_serializadas)
        'Escritor.Close()

    End Sub

    Public Shared Sub Deserializar(ByVal Ruta As String, ByRef Lista_Serializadas As Listas_serializadas)

        Dim Formatter As BinaryFormatter = New BinaryFormatter

        If Ruta = "" Then
            Dim Myfile As OpenFileDialog = New OpenFileDialog

            With Myfile
                .Title = "Información Muros"
                .Filter = "Abrir Archivo |*.dmc"
            End With

            Myfile.ShowDialog()
            Ruta = Myfile.FileName
        End If

        Dim Lector As Stream = New FileStream(Ruta, FileMode.Open, FileAccess.Read, FileShare.None)
        Dim Aux As Object = Formatter.Deserialize(Lector)
        'Lista_Serializadas = CType(Formatter.Deserialize(Lector), Listas_serializadas)
        Lector.Close()

        ''Conversion de listas
        Convert_listas(Aux)

    End Sub

    Private Shared Sub Convert_listas(Lista_i As Object)

        Dim aux = CType(Lista_i, Diseño_de_muros_concreto_V2.Listas_serializadas)
        Dim aux2 As List(Of Muros_Consolidados) = CType(aux.Lista_Muros, IEnumerable)

        For Each prueba As Muros_Consolidados In aux2

            prueba.NoBarras = New List(Of Double)
            prueba.Reduccion = Reduccion.NoAplica
            prueba.NombreBarras = New List(Of List(Of String))
            prueba.LongitudBarras = New List(Of List(Of Double))
            prueba.NombreBarras = New List(Of List(Of String))
            Muros_lista_2.Add(prueba)

        Next



    End Sub
End Class
