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

    Private refuerzoHPreFinal As New List(Of String)

    Public Property RefuerzoHNomenPreFinal() As List(Of String)
        Get
            Return refuerzoHPreFinal
        End Get
        Set(ByVal value As List(Of String))
            refuerzoHPreFinal = value
        End Set
    End Property

    Private refuerzoHorzontalDllnet_ As New List(Of String)

    Public Property RefuerzoHorzontalDllnet() As List(Of String)
        Get
            Return refuerzoHorzontalDllnet_
        End Get
        Set(ByVal value As List(Of String))
            refuerzoHorzontalDllnet_ = value
        End Set
    End Property

    Private lista_Estribos_ As New List(Of Seccion_Estribos)

    Public Property Lista_Estribos() As List(Of Seccion_Estribos)
        Get
            Return lista_Estribos_
        End Get
        Set(ByVal value As List(Of Seccion_Estribos))
            lista_Estribos_ = value
        End Set
    End Property

    Private lista_ganchos_ As New List(Of Seccion_Ganchos)

    Public Property Lista_Ganchos() As List(Of Seccion_Ganchos)
        Get
            Return lista_ganchos_
        End Get
        Set(ByVal value As List(Of Seccion_Ganchos))
            lista_ganchos_ = value
        End Set
    End Property

    Sub Nomenclatura(ByVal e_Losa As Single, Optional R As Single = 0.05)
        CalcularCantidadPorPiso(e_Losa, R)
        RefuerzoPreFinal()
        RefuerzoFinal()
    End Sub

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
                NomenclaturaRefuerzo.Add($"#{No_Barra(i)}L={Longitud(i)}-{FormaRefuerzo(i)} ")
            Else
                NomenclaturaRefuerzo.Add("")
            End If
        Next

    End Sub

    Sub RefuerzoPreFinal()

        Dim VectoIndices As New List(Of Integer)
        RefuerzoHNomenPreFinal.Clear()
        For i = 0 To NomenclaturaRefuerzo.Count - 1
            Dim Cantidad1 As Integer = Cantidad(i)

            If VectoIndices.Exists(Function(x) x = i) = False Then

                For j = i + 1 To NomenclaturaRefuerzo.Count - 1

                    If NomenclaturaRefuerzo(i) = NomenclaturaRefuerzo(j) Then
                        Cantidad1 = Cantidad1 + Cantidad(j)
                        VectoIndices.Add(j)
                    End If
                Next
                RefuerzoHNomenPreFinal.Add(Cantidad1 & NomenclaturaRefuerzo(i))
            End If

        Next

    End Sub

    Sub RefuerzoFinal()
        RefuerzoHorzontalDllnet.Clear()
        For i = 0 To RefuerzoHNomenPreFinal.Count - 1
            Dim Nomenclatura = RefuerzoHNomenPreFinal(i)
            Dim Cantidad1 As Integer = 0 : Dim PosiciF_Canti As Integer = 0
            Dim No_Barra_ As Single = 0 : Dim PosiciF_No_Barra As Integer = 0
            Dim Nom_barra As String = ""
            Dim Longitud_Barra As Single = 0 : Dim PisicF__ As Integer = 0
            Dim FormaRefuerzo As String : Dim NomenclaturaFinal As String
            Dim LongitudGancho As Single
            If Nomenclatura.Chars(0) <> "0" Then

                For n = 0 To Len(Nomenclatura) - 1
                    If Nomenclatura.Chars(n) = "#" Then
                        PosiciF_Canti = n
                    End If
                    If Nomenclatura.Chars(n) = "L" Then
                        If PosiciF_No_Barra = 0 Then
                            PosiciF_No_Barra = n
                        End If
                    End If
                    If Nomenclatura.Chars(n) = "-" Then
                        PisicF__ = n
                    End If
                Next

                Cantidad1 = Val(Nomenclatura.Substring(0, PosiciF_Canti))
                No_Barra_ = Val(Nomenclatura.Substring(PosiciF_Canti + 1, PosiciF_No_Barra - 1))
                Longitud_Barra = Val(Nomenclatura.Substring(PosiciF_No_Barra + 2, PisicF__ - (PosiciF_No_Barra + 2)))
                FormaRefuerzo = (Nomenclatura.Substring(PisicF__ + 1))

                If No_Barra_ = 4.5 Then
                    Nom_barra = No_Barra_ & "M"
                Else
                    Nom_barra = No_Barra_.ToString
                End If

                LongitudGancho = ganchos_180(Nom_barra)
                If FormaRefuerzo = "R" Then
                    NomenclaturaFinal = $"{Cantidad1} {No_Barra_} {Longitud_Barra}"
                End If
                If FormaRefuerzo = "R1G" Then
                    NomenclaturaFinal = $"{Cantidad1} {No_Barra_} {Longitud_Barra - LongitudGancho} U{LongitudGancho}"
                End If
                If FormaRefuerzo = "R2G" Then
                    NomenclaturaFinal = $"{Cantidad1} {No_Barra_} {Longitud_Barra - 2 * LongitudGancho} U{LongitudGancho} U{LongitudGancho}"
                End If

                'RefuerzoHorzontalDllnet.Add(NomenclaturaFinal)

            End If
        Next
    End Sub

End Class