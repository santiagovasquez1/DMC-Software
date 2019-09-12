Public Class Arania

    Private _label As String
    Private _muros_arania As List(Of Muros) = New List(Of Muros)

    Public Property Label As String
        Get
            Return _label
        End Get
        Set(value As String)
            _label = value
        End Set
    End Property

    Public Property Muros_arania As List(Of Muros)
        Get
            Return _muros_arania
        End Get
        Set(value As List(Of Muros))
            _muros_arania = value
        End Set
    End Property
End Class
