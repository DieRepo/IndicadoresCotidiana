Imports MySql.Data
Imports MySql.Data.Types
Imports MySql.Data.MySqlClient

Public Class Conexion
    Public _cadena As String ' se usa únicamente para obtener los datos de conexión
    Public _conexion As MySqlConnection 'esta variable se encargara de conectar la BD


    Public Function conexion_global(intTipoConsulta As Integer) As MySqlConnection 'se crea una funcion publica boleana como estamos en un modulo esto evitara colocar el mismo método en diferentes forms 

        Try
            Dim connectionString As String = ""
            If intTipoConsulta = 1 Then
                connectionString = ConfigurationManager.AppSettings("ConnectionLocal")
            ElseIf intTipoConsulta = 2 Then
                connectionString = ConfigurationManager.AppSettings("ConnectionStringSigejepa")
            ElseIf intTipoConsulta = 3 Then
                connectionString = ConfigurationManager.AppSettings("ConnectionSEJ")
            ElseIf intTipoConsulta = 4 Then
                connectionString = ConfigurationManager.AppSettings("ConnectionSeg")
            End If


            _cadena = (connectionString) 'aqui se conecta a la BD de mysql
            _conexion = New MySqlConnection(_cadena)
            _conexion.Open() '.Open hace el enlace con la BD


        Catch ex As Exception 'en caso contrario

            ' MessageBox.Show(ex.Message) 'mostrara un mensaje de error

        End Try 'fin del try

        Return _conexion ' en una funcion siempre debe de existir un return. el caso es que en cuanto termine de revisar mandará exactamente el estado en el que se encuentre


    End Function ' se termina la función


    Public Sub cerrar()
        _conexion.Dispose()
        _conexion.Close()

    End Sub
End Class
