<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="IndicadoresPjem.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Inicio de sesión</title>

    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <!-- jQuery library -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"></script>
    <!-- Latest compiled JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <style>
        body {
            position: absolute;
            height: 100%;
            width: 100%;
            background: #9d2424;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        form {
            vertical-align: middle;
            background: white;
            width: 30%;
            padding: 45px 25px 45px 25px;
            border-radius: 8px;
        }
    </style>

</head>
<body>

    <form runat="server" class="center-block">
        <div class="form-group">
            <asp:Image runat="server" ImageUrl="~/img/logopj.png" CssClass="center-block" Width="90%" />
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="usr">Usuario: </asp:Label>
            <div class="input-group">
                <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                <asp:TextBox runat="server" ID="usr" CssClass="form-control" placeholder="Ingrese usuario" />               
            </div>
            <asp:Label runat="server" ID="usrError" CssClass="text-danger"></asp:Label>
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="pwd">Contraseña: </asp:Label>
            <div class="input-group">
                <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                <asp:TextBox runat="server" ID="pwd" TextMode="Password" CssClass="form-control" placeholder="Ingrese contraseña" />              
            </div>
            <asp:Label runat="server" ID="passError" CssClass="text-danger"></asp:Label>
        </div>
        <div class="form-group">
            <asp:Button runat="server" Text="Entrar" CssClass="btn" OnClick="Login" />
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="formError" CssClass="text-danger"></asp:Label>
        </div>
    </form>

</body>
</html>
