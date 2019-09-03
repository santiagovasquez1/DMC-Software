Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Public Class Serializador2
    Public Sub New(ByVal Ruta_Carpeta_Clase As String, Optional Guardar As Boolean = True)
        If Lista_Cantidades1 Is Nothing Then
            Lista_Cantidades1 = New Lista_Cantidades
            Lista_Cantidades1.ListaRefuerzoHorzontal = New List(Of RefuerzoHorizontal)
        End If
        Dim Lista_Cantidades_Seri As Lista_Cantidades = New Lista_Cantidades With {
            .ListaRefuerzoHorzontal = Lista_Cantidades1.ListaRefuerzoHorzontal}

        Dim Ruta = Ruta_Carpeta_Clase & "\Quantities.qmdc"

        If Guardar Then
            Serializar(Ruta, Lista_Cantidades_Seri)
        Else
            Try
                Deserializar(Ruta)
            Catch ex As Exception

            End Try
        End If

    End Sub


    Private Sub Serializar(ByVal Ruta As String, ByVal Lista_Serializar As Lista_Cantidades)
        Dim Formatter As BinaryFormatter = New BinaryFormatter
        Dim Escritor As Stream = New FileStream(Ruta, FileMode.Create, FileAccess.Write, FileShare.None)
        Formatter.Serialize(Escritor, Lista_Serializar)
        Escritor.Close()

    End Sub
    Private Sub Deserializar(ByVal Ruta As String)
        Dim Formatter As BinaryFormatter = New BinaryFormatter
        Dim ListaCantidades_Cargada As Lista_Cantidades
        Dim Lector As Stream = New FileStream(Ruta, FileMode.Open, FileAccess.Read, FileShare.None)
        ListaCantidades_Cargada = CType(Formatter.Deserialize(Lector), Lista_Cantidades)
        Lector.Close()
        LLenarListasCantidades(ListaCantidades_Cargada)
    End Sub

    Sub LLenarListasCantidades(ByVal ListaCantidades_Cargada As Lista_Cantidades)
        Lista_Cantidades1.ListaRefuerzoHorzontal = ListaCantidades_Cargada.ListaRefuerzoHorzontal
    End Sub


End Class
