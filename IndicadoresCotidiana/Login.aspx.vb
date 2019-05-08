Imports System.Security.Cryptography
Imports MySql.Data.MySqlClient

Public Class Login
    Inherits System.Web.UI.Page

    Dim UserSession As String = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.AppendHeader("Cache-Control", "no-store")
        Dim sesion As New Dictionary(Of String, String)

        If Session("Sesion") IsNot Nothing Then
            sesion = CType(Session("Sesion"), Dictionary(Of String, String))
            Response.Redirect("Indi/pages/index.html")

        End If


    End Sub

    Public Sub Login()

        Dim user, pass As String

        usrError.Text = ""
        passError.Text = ""
        formError.Text = ""

        user = usr.Text
        pass = pwd.Text

        If user.Trim <> "" Then
            If pass.Trim <> "" Then
                If VerificaSesion(user, MD5(pass)) Then

                    Dim sesion As New Dictionary(Of String, String)
                    sesion.Add("usuario", user)
                    Session("Sesion") = sesion

                    Response.Redirect("Indi/pages/index.html")
                Else
                    formError.Text = "* El usuario y/o contrasela son invalidos, Intente de nuevo por favor"
                End If
            Else
                passError.Text = "* El campo usuario es obligatorio"
            End If
        Else
            usrError.Text = "* El campo usuario es obligatorio"
        End If

    End Sub


    Public Function VerificaSesion(usu As String, pass As String) As Boolean
        Try
            Dim Conexion As Conexion = New Conexion()
            Dim Coneccion = Conexion.conexion_global(1)
            Dim sqlquery As String = "SELECT if(count(idUsuario) = 1, true, false) existe FROM indicadores_seguridad.`seguridad.tblusuarios`
                                      WHERE usuario = @usu and pass = @pass and activo = 1;"

            Dim cmd As MySqlCommand = New MySqlCommand(sqlquery, Coneccion)
            cmd.Parameters.AddWithValue("@usu", usu)
            cmd.Parameters.AddWithValue("@pass", pass)
            Dim read As MySqlDataReader = cmd.ExecuteReader()
            Dim existe As String = 0

            While read.Read()
                existe = read.GetString("existe")
            End While

            read.Close()
            Conexion.cerrar()

            If existe = 1 Then
                Return True
            Else
                Return False
            End If

        Catch ex As MySqlException
            Console.WriteLine("Error al verificar la sesion")
            Debug.WriteLine("Error al verificar la sesion" & ex.Message)
            Return False
        End Try
    End Function


    Public Function MD5(pass As String) As String

        Dim md As MD5 = MD5CryptoServiceProvider.Create()
        Dim encoding As ASCIIEncoding = New ASCIIEncoding()
        Dim stream() As Byte = Nothing
        Dim sb As StringBuilder = New StringBuilder()
        stream = md.ComputeHash(encoding.GetBytes(pass))
        Dim j As Int32 = (stream.Length - 1)

        For i As Int32 = 0 To j
            sb.AppendFormat("{0:x2}", stream(i))
        Next

        Debug.WriteLine("MD5: " + sb.ToString)

        Return sb.ToString

    End Function

End Class