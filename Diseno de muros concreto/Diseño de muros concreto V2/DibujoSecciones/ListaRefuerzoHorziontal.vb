Public Class RefuerzoHorizontal

    Private nombreMuro_ As String

    Public Property NombreMuro() As String
        Get
            Return nombreMuro_
        End Get
        Set(ByVal value As String)
            nombreMuro_ = value
        End Set
    End Property

    Private longitud_ As New List(Of Single)

    Public Property Longitud() As List(Of Single)
        Get
            Return longitud_
        End Get
        Set(ByVal value As List(Of Single))
            longitud_ = value
        End Set
    End Property

    Private no_barra_ As List(Of String)

    Public Property No_Barra() As List(Of String)
        Get
            Return no_barra_
        End Get
        Set(ByVal value As List(Of String))
            no_barra_ = value
        End Set
    End Property

    Private capas_ As List(Of Integer)

    Public Property No_Capas() As List(Of Integer)
        Get
            Return capas_
        End Get
        Set(ByVal value As List(Of Integer))
            capas_ = value
        End Set
    End Property

    Private separacion_ As List(Of Single)

    Public Property Separacion() As List(Of Single)
        Get
            Return separacion_
        End Get
        Set(ByVal value As List(Of Single))
            separacion_ = value
        End Set
    End Property

    Private formaRefuerzo_ As List(Of TipoRefuerzo)

    Public Property FormaRefuerzo() As List(Of TipoRefuerzo)
        Get
            Return formaRefuerzo_
        End Get
        Set(ByVal value As List(Of TipoRefuerzo))
            formaRefuerzo_ = value
        End Set
    End Property

    Private hw_ As List(Of Single)

    Public Property Hw() As List(Of Single)
        Get
            Return hw_
        End Get
        Set(ByVal value As List(Of Single))
            hw_ = value
        End Set
    End Property

    Private cantidad_ As New List(Of Integer)

    Public Property Cantidad() As List(Of Integer)
        Get
            Return cantidad_
        End Get
        Set(ByVal value As List(Of Integer))
            cantidad_ = value
        End Set
    End Property

    Private nomenclaturaRefuerzo_ As New List(Of String)

    Public Property NomenclaturaRefuerzo() As List(Of String)
        Get
            Return nomenclaturaRefuerzo_
        End Get
        Set(ByVal value As List(Of String))
            nomenclaturaRefuerzo_ = value
        End Set
    End Property

    Sub CalcularCantidadPorPiso(ByVal e_Losa As Single, Optional R As Single = 0.05)
        Cantidad.Clear() : NomenclaturaRefuerzo.Clear()
        For i = 0 To Hw.Count - 1
            If No_Capas(i) <> 0 Then
                Cantidad.Add(((Hw(i) - 2 * R - e_Losa) / (Separacion(i))) * (No_Capas(i)))
            Else
                Cantidad.Add(0)
            End If
        Next

        For i = 0 To Cantidad.Count - 1
            If No_Capas(i) <> 0 Then
                NomenclaturaRefuerzo.Add($"{Cantidad(i)} #{No_Barra(i)} L={Longitud(i)} - {FormaRefuerzo(i)} ")
            Else
                NomenclaturaRefuerzo.Add("")
            End If
        Next

    End Sub

End Class