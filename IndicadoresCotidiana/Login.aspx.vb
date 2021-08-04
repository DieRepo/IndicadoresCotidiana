Imports System.Security.Cryptography
Imports MySql.Data.MySqlClient

Public Class Login
    Inherits System.Web.UI.Page

    Dim UserSession As String = Nothing

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.AppendHeader("Cache-Control", "no-store")
        Dim sesion As New Dictionary(Of String, String)

        If IsPostBack = False Then

            If Page.Request.ServerVariables("HTTP_REFERER") <> Nothing Then
                VerificaLoginGestion()
            Else
                Response.Redirect("http://gestionjudicial.pjedomex.gob.mx/ideas/")
            End If


        End If

            If Session("Sesion") IsNot Nothing Then
            sesion = CType(Session("Sesion"), Dictionary(Of String, String))
            Response.Redirect("Indi/pages/index.html")

        End If

    End Sub


    Public Sub Login(sender As Object, e As System.EventArgs)

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

                    Response.Redirect("Indi/pages/index.html?mat=0")
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
            Dim Coneccion = Conexion.conexion_global(2)
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

    Public Sub VerificaLoginGestion()

        If Page.Request.ServerVariables("HTTP_REFERER") <> Nothing Then

            Dim referencia As String = Page.Request.ServerVariables("HTTP_REFERER").ToString

            If referencia.Contains("gestionjudicial.pjedomex.gob.mx") Then

                Dim urlString As String = Page.Request.Url.AbsoluteUri
                Dim url = New Uri(urlString)


                If (CType(HttpUtility.ParseQueryString(url.Query).Count, String)) = 2 Then

                    If HttpUtility.ParseQueryString(url.Query).GetKey(0) = "usr" And HttpUtility.ParseQueryString(url.Query).GetKey(1) = "mat" Then

                        Dim usr = HttpUtility.ParseQueryString(url.Query).Get("usr")
                        Dim mat = HttpUtility.ParseQueryString(url.Query).Get("mat")
                        Dim userEncoded As String = usr
                        Dim userDecoded As String
                        Dim data() As Byte
                        data = System.Convert.FromBase64String(userEncoded)
                        userDecoded = System.Text.ASCIIEncoding.ASCII.GetString(data)

                        If userDecoded.Length > 0 And mat.Length > 0 Then

                            Dim base As String = ""

                            Select Case mat
                                Case 1
                                    base = "`htsj_spacusatorio_civil`"
                                Case 2
                                    base = "`htsj_spacusatorio_familiar`"
                                Case 3
                                    base = "`htsj_spacusatorio_mercantil`"
                                Case 5
                                    base = "`htsj_spacusatorio_mixto`"
                            End Select

                            Try
                                Dim Conexion As Conexion = New Conexion()
                                Dim Coneccion = Conexion.conexion_global(3)
                                Dim sqlquery As String = "SELECT if(count(email) > 0, true, false) existe FROM " + base + ".`users`
                                      WHERE email = @usr  and activated = 1;"

                                Dim cmd As MySqlCommand = New MySqlCommand(sqlquery, Coneccion)
                                cmd.Parameters.AddWithValue("@usr", userDecoded.Trim)
                                Dim read As MySqlDataReader = cmd.ExecuteReader()
                                Dim existe As String = 0

                                While read.Read()
                                    existe = read.GetString("existe")
                                End While

                                read.Close()
                                Conexion.cerrar()

                                If existe = 1 Then
                                    Dim sesion As New Dictionary(Of String, String)
                                    sesion.Add("usuario", usr)
                                    Session("Sesion") = sesion

                                    Response.Redirect("Indi/pages/index.html?mat=" + mat + "")
                                Else

                                    Response.Redirect(referencia)
                                End If

                            Catch ex As MySqlException
                                Console.WriteLine("Error al verificar la sesion")
                                Debug.WriteLine("Error al verificar la sesion" & ex.Message)

                            End Try

                        Else
                            Response.Redirect(referencia)
                        End If
                    Else
                        Response.Redirect(referencia)
                    End If
                Else
                    Response.Redirect(referencia)
                End If
            Else
                Response.Redirect("http://gestionjudicial.pjedomex.gob.mx/ideas/")
            End If
        Else
            Response.Redirect("http://gestionjudicial.pjedomex.gob.mx/ideas/")
        End If

    End Sub

End Class