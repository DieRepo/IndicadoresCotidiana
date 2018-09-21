Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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
                Dim usuario As String = ConfigurationManager.AppSettings("usuario")
                Dim password As String = ConfigurationManager.AppSettings("pass")

                If user = usuario And pass = password Then

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

End Class