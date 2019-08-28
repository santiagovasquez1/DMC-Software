Imports System.Collections.Generic
<Serializable>
Public Class Listas_serializadas

    Public Lista_Muros = New List(Of Muros_Consolidados)
    Public lista_refuerzo = New List(Of Refuerzo_muros)
    Public Lista_Alzados = New List(Of alzado_muro)
    Public Muros_generales = New List(Of Muro)
    Public Capacidad_proyecto As String

End Class