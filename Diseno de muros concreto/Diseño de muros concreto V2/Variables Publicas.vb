
Module Variables_Publicas
    Public list_muros() As String
    Public piername() As String
    Public Muros_lista_2 As List(Of Muros_Consolidados)
    Public areas_refuerzo As New Dictionary(Of Single, Single)
    Public colores_refuerzo As New Dictionary(Of Integer, Color)
    Public mallas As New Dictionary(Of String, Double)
    Public Hviga As Single = 0
    Public prof As Single = 0
    Public Hfunda As Single = 0
    Public Arranque As Single = 0
    Public Nombre_Nivel As String = ""
    Public traslapo_560 As New Dictionary(Of Integer, Single)
    Public traslapo_490 As New Dictionary(Of Integer, Single)
    Public traslapo_420 As New Dictionary(Of Integer, Single)
    Public traslapo_350 As New Dictionary(Of Integer, Single)
    Public traslapo_280 As New Dictionary(Of Integer, Single)
    Public traslapo_210 As New Dictionary(Of Integer, Single)
    Public ganchos_90 As New Dictionary(Of Integer, Single)
    Public ganchos_180 As New Dictionary(Of String, Single)
    Public As_malla As New Dictionary(Of String, Single)
    Public refuerzo_lista As New List(Of Refuerzo_muros)
    Public contador As Integer = 1
    Public contador1 As Integer = 0
    Public alzado_lista As New List(Of alzado_muro)
    Public coordX As Double = 0
    Public Lista_mallas As New List(Of Cantidades_Muro_malla)
    Public Ruta_archivo As String = ""
    Public data_grid As New DataGridView
    Public Lista_pl_coordinates As New List(Of List(Of Double))
    Public T_coordinates As New List(Of List(Of Double))
    Public T_center_circle As New List(Of List(Of Double))
    Public Lista_ref As New List(Of List(Of Double))
    Public Ruta_1, Capacidad As String
    Public Lista_Auxiliar As Object
    Public data_info_f3 As DataGridView

    Public Function cuantia_volumetrica(ByVal sep As Single, ByVal fc As Single, ByVal fy As Single, lebe As Single, bw As Single, ByVal Capacidad As String) As Single()

        Dim r As Single = 2 'cm²
        Dim ashX As Single
        Dim ashy As Single
        Dim Ash(0 To 1) As Single

        If Capacidad = "DES" Then
            ashX = 0.09 * sep * lebe * fc / fy
            ashy = 0.09 * sep * bw * fc / fy
        Else
            ashX = 0.06 * sep * lebe * fc / fy
            ashy = 0.06 * sep * bw * fc / fy
        End If

        Ash(0) = ashX
        Ash(1) = ashy

        Return Ash
    End Function

End Module
