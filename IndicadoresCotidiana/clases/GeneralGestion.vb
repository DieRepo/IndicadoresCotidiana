Imports IndicadoresPjem

Public Class GeneralGestion

    Private _IdGestion As String
    Private _Datos As New List(Of DatosGestion)

    Public Property IdGestion As String
        Get
            Return _IdGestion
        End Get
        Set(value As String)
            _IdGestion = value
        End Set
    End Property

    Public Property Datos As List(Of DatosGestion)
        Get
            Return _Datos
        End Get
        Set(value As List(Of DatosGestion))
            _Datos = value
        End Set
    End Property
End Class
