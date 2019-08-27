Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Public Class Serializador

    Public Sub New(ByVal Ruta As String, ByVal Guardar As Boolean, ByVal Lista_serializadas As Listas_serializadas)

        If Guardar = True Then
            Serializar(Ruta, Lista_serializadas)
        Else
            Deserializar(Ruta, Lista_serializadas)
        End If

    End Sub

    Public Shared Sub Serializar(ByRef ruta As String, ByVal Lista_serializadas As Listas_serializadas)

        Dim Formatter As BinaryFormatter = New BinaryFormatter

        If ruta = "" Then
            Dim Myfile As SaveFileDialog = New SaveFileDialog
            With Myfile
                .Title = "Información Muros"
                .Filter = "Guardar Archivo |*.dmc"
            End With

            Myfile.ShowDialog()
            ruta = Myfile.FileName
        End If

        Dim Escritor As Stream = New FileStream(ruta, FileMode.Create, FileAccess.Write, FileShare.None)
        Formatter.Serialize(Escritor, Lista_serializadas)
        Escritor.Close()

    End Sub

    Public Shared Sub Deserializar(ByVal Ruta As String, ByRef Lista_Serializadas As Listas_serializadas)

        Dim Formatter As BinaryFormatter = New BinaryFormatter

        Dim Myfile As OpenFileDialog = New OpenFileDialog

        With Myfile
            .Title = "Información Muros"
            .Filter = "Guardar Archivo |*.dmc"
        End With

        Myfile.ShowDialog()
        Ruta_archivo = Myfile.FileName

        Dim Lector As Stream = New FileStream(Ruta_archivo, FileMode.Open, FileAccess.Read, FileShare.None)
        Lista_Serializadas = CType(Formatter.Deserialize(Lector), Listas_serializadas)

        Lector.Close()
    End Sub

End Class