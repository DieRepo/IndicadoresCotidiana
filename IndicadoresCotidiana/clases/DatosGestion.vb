<Serializable()>
Public Class DatosGestion

    Private _Anio As String
    Private _Mes As String
    Private _Fecha As String
    Private _Valor As String
    Private _Total As String
    Private _Calculo As String
    Private _Semaforo As String

    Public Property Anio As String
        Get
            Return _Anio
        End Get
        Set(value As String)
            _Anio = value
        End Set
    End Property

    Public Property Mes As String
        Get
            Return _Mes
        End Get
        Set(value As String)
            _Mes = value
        End Set
    End Property

    Public Property Valor As String
        Get
            Return _Valor
        End Get
        Set(value As String)
            _Valor = value
        End Set
    End Property

    Public Property Total As String
        Get
            Return _Total
        End Get
        Set(value As String)
            _Total = value
        End Set
    End Property

    Public Property Calculo As String
        Get
            Return _Calculo
        End Get
        Set(value As String)
            _Calculo = value
        End Set
    End Property

    Public Property Semaforo As String
        Get
            Return _Semaforo
        End Get
        Set(value As String)
            _Semaforo = value
        End Set
    End Property

    Public Property Fecha As String
        Get
            Return _Fecha
        End Get
        Set(value As String)
            _Fecha = value
        End Set
    End Property
End Class
