<Serializable>
Public Class Lista_Cantidades
    Private _ListaRefuerzoHorzontal As List(Of RefuerzoHorizontal)
    Private _Ruta_proyecto As String

    Public Property Ruta_proyecto As String
        Get
            Return _Ruta_proyecto
        End Get
        Set
            _Ruta_proyecto = Value
        End Set
    End Property

    Public Property ListaRefuerzoHorzontal As List(Of RefuerzoHorizontal)
        Get
            Return _ListaRefuerzoHorzontal
        End Get
        Set
            _ListaRefuerzoHorzontal = Value
        End Set
    End Property
End Class
