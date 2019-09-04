<Serializable>
Public Class Seccion_Ganchos
    Private _Longitdud As Double
    Private _Diametro As Double
    Private _Separacion As Double
    Private _Cantidad As Integer
    Private _Story As String
    Private _Pier As String

    Public Property Longitud As Double
        Get
            Return _Longitdud
        End Get
        Set
            _Longitdud = Value
        End Set
    End Property

    Public Property Diametro As Double
        Get
            Return _Diametro
        End Get
        Set
            _Diametro = Value
        End Set
    End Property

    Public Property Separacion As Double
        Get
            Return _Separacion
        End Get
        Set
            _Separacion = Value
        End Set
    End Property

    Public Property Cantidad As Integer
        Get
            Return _Cantidad
        End Get
        Set
            _Cantidad = Value
        End Set
    End Property

    Public Property Story As String

        Get
            Return _Story
        End Get
        Set
            _Story = Value
        End Set
    End Property

    Public Property Pier As String
        Get
            Return _Pier
        End Get
        Set
            _Pier = Value
        End Set
    End Property



    Public Property Espesor As Single

End Class
