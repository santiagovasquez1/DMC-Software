<Serializable>
Public Class Muro
    Public Pier, Story As String
    Public bw, lw, hw, Fc, dw, h_acumulado As Single
    Public Rho_l_Inicial, Spacing, Rho_l_Def As Double
    Public Loc As List(Of String) = New List(Of String)()
    Public Load As List(Of String) = New List(Of String)()
    Public P As List(Of Double) = New List(Of Double)()
    Public V2 As List(Of Double) = New List(Of Double)()
    Public V3 As List(Of Double) = New List(Of Double)()
    Public M2 As List(Of Double) = New List(Of Double)()
    Public M3 As List(Of Double) = New List(Of Double)()
    <NonSerialized> Public Pc As Double
    <NonSerialized> Public Error_pc As String
    Public Shells_Muro As List(Of Shells_Prop) = New List(Of Shells_Prop)()

#Region "Cortante muros"

    Public Phi_Vc As List(Of Double)
    Public Phi_Vs As List(Of Double)
    Public Pt_min As List(Of Double)
    Public pl_min As List(Of Double)
    Public pt_requerido1 As List(Of Double) 'Según C.11.9.91
    Public ptt As List(Of Double) 'Cuantia transversal requerida por C.21.9.4.1
    Public pt_definitivo As List(Of Double)
    Public Phi_Vn_Max1 As Double 'Capacidad maxima de la sección segun C11.9.3
    Public Phi_Vn_Max2 As List(Of Double) = New List(Of Double)() 'Capacidad maxima de la sección segun C21.9.4.1
    Public Phi_Vs_Max As Double 'Capacidad maxima del acero según C.11.4.7.9
    Public Pt_max As Double 'Cuantia maxima de acero según C.11.4.7.9
    Public Cortinas As List(Of Integer)
    Public Error_Cortante As List(Of String)

#End Region

#Region "Flexo Compresion"

    Public C_def As List(Of Double)
    Public L_Conf As List(Of Double)
    Public Fa As List(Of Double)
    Public Fv As List(Of Double)
    Public Sigma_Max As List(Of Double)
    Public Sigma_Min As List(Of Double)
    Public Relacion As List(Of Double)
    Public Error_Flexion As List(Of String)
    Public C_balanceado, P_balanceado As Double

#End Region

    Public Sub Calc_pc()

        Dim E_c As Double = 0 'Modulo elasticidad del concreto
        Dim Ix As Double = 0 'Inercias en la seccion debil del muro
        Dim k As Double = 1 'Factor de longitud efectiva
        Dim pmax As Double = 0

        E_c = 15000 * Math.Sqrt(Fc)
        Ix = lw * Math.Pow(bw, 3) / 12
        Pc = Math.Pow(Math.PI, 2) * 0.25 * E_c * Ix / Math.Pow(k * hw, 2) / 1000
        pmax = Math.Abs(P.Min())

        If pmax > Pc Then
            Error_pc = $"La carga de {Math.Round(pmax, 2)} es mayor que {Math.Round(Pc, 2)}"
        Else
            Error_pc = "Ok"
        End If

    End Sub

End Class