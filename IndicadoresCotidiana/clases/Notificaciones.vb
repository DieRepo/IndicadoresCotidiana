Public Class Notificaciones
    Private _anio As String
    Private _mes As String
    Private _fechaResgistro As String
    Private _fechaActualizacion As String
    Private _expediente As String
    Private _difHoras As String
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

    Public Property FechaResgistro As String
        Get
            Return _fechaResgistro
        End Get
        Set(value As String)
            _fechaResgistro = value
        End Set
    End Property

    Public Property FechaActualizacion As String
        Get
            Return _fechaActualizacion
        End Get
        Set(value As String)
            _fechaActualizacion = value
        End Set
    End Property

    Public Property Expediente As String
        Get
            Return _expediente
        End Get
        Set(value As String)
            _expediente = value
        End Set
    End Property

    Public Property DifHoras As String
        Get
            Return _difHoras
        End Get
        Set(value As String)
            _difHoras = value
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
