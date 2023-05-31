Public Class Audiencia
    Private _anio As String
    Private _mes As String
    Private _fechaInicio As String
    Private _fechaFinal As String
    Private _estatus As String
    Private _juzgado As String

    Public Property Anio As String
        Get
            Return _anio
        End Get
        Set(value As String)
            _anio = value
        End Set
    End Property

    Public Property Mes As String
        Get
            Return _mes
        End Get
        Set(value As String)
            _mes = value
        End Set
    End Property

    Public Property FechaInicio As String
        Get
            Return _fechaInicio
        End Get
        Set(value As String)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFinal As String
        Get
            Return _fechaFinal
        End Get
        Set(value As String)
            _fechaFinal = value
        End Set
    End Property

    Public Property Estatus As String
        Get
            Return _estatus
        End Get
        Set(value As String)
            _estatus = value
        End Set
    End Property

    Public Property Juzgado As String
        Get
            Return _juzgado
        End Get
        Set(value As String)
            _juzgado = value
        End Set
    End Property
End Class
