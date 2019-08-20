Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Public Class Serializador

    Public Shared Sub Serializar(ByVal Lista_serializadas As Listas_serializadas)

        Dim Formatter As BinaryFormatter = New BinaryFormatter

        Dim Myfile As SaveFileDialog = New SaveFileDialog
        With Myfile
            .Title = "Información Muros"
            .Filter = "Guardar Archivo |*.txt"
        End With

        Myfile.ShowDialog()

        Dim Escritor As Stream = New FileStream(Myfile.FileName, FileMode.Create, FileAccess.Write, FileShare.None)
        Formatter.Serialize(Escritor, Lista_serializadas)

        Escritor.Close()

    End Sub

    Public Shared Sub Deserializar(ByVal Ruta As String, ByRef Lista_Serializadas As Listas_serializadas)

        Dim Formatter As BinaryFormatter = New BinaryFormatter

        Dim Myfile As OpenFileDialog = New OpenFileDialog

        With Myfile
            .Title = "Información Muros"
            .Filter = "Guardar Archivo |*.txt"
        End With

        Myfile.ShowDialog()
        Ruta_archivo = Myfile.FileName

        Dim Lector As Stream = New FileStream(Ruta_archivo, FileMode.Open, FileAccess.Read, FileShare.None)
        Lista_Serializadas = CType(Formatter.Deserialize(Lector), Listas_serializadas)

        Lector.Close()
    End Sub

End Class
