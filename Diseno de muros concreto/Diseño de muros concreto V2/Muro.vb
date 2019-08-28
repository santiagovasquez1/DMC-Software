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
    Public Shells_Muro As List(Of Shells_Prop) = New List(Of Shells_Prop)()

    'Datos para el diseño a cortante de los muros en concreto
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

    'Datos para el analisis a flexo compresion
    Public C_def As List(Of Double)

    Public L_Conf As List(Of Double)
    Public Fa As List(Of Double)
    Public Fv As List(Of Double)
    Public Sigma_Max As List(Of Double)
    Public Sigma_Min As List(Of Double)
    Public Relacion As List(Of Double)
    Public Error_Flexion As List(Of String)
    Public C_balanceado, P_balanceado As Double
End Class