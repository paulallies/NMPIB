<%@ Page Language="C#" MasterPageFile="~/Views/Shared/nmp.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log On
</asp:Content>
<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="titleBox">
        <div class="inputDiv">
            <h2>Log On</h2>
        </div>
    </div>
    <div class="titleBox">
        <div class="inputDiv">
            <%= Html.ValidationSummary("Login was unsuccessful. Please correct the errors and try again.") %>
            <% using (Html.BeginForm())
               { %>
            <p>
                <label for="username">Username:</label>
                <%= Html.TextBox("username") %>
                <%= Html.ValidationMessage("username") %>
            </p>
            <p>
                <label for="password">Password:</label>
                <%= Html.Password("password") %>
                <%= Html.ValidationMessage("password") %>
            </p>
            <p>
                <%= Html.CheckBox("rememberMe") %>
                <label class="inline" for="rememberMe">Remember me?</label>
            </p>
            <p>
                <input type="submit" value="Log On" />
            </p>
        </div>
        <% } %>
    </div>
</asp:Content>
<asp:Content ID="head" ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
            $(function(){
                $("#username").focus();

            });



    </script>

</asp:Content>
