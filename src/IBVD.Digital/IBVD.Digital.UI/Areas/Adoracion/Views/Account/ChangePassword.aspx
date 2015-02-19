<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%= ViewData[IBVD.Digital.UI.Models.UICommonKeys.TITLE] %></h2>
    <% 
        StringBuilder comentario = new StringBuilder();

        comentario.Append("Utilice el siguiente formulario para modificar su contrase&ntilde;a<br />");
        comentario.AppendFormat("La contrase&ntildea debe tener como m&iacute;nimo de {0} carateres", ViewData["PasswordLength"]);
        Html.RenderPartial("UserControls/CommentPanel", comentario.ToString());
    %>
    <% using (Html.BeginForm())
       { %>
    <div>
        <fieldset class="formulario">
            <legend>Account Information</legend>
            <p>
                <label for="currentPassword">
                    Current password:</label>
                <%= Html.Password("currentPassword") %>
                <em>*</em>
            </p>
            <p>
                <label for="newPassword">
                    New password:</label>
                <%= Html.Password("newPassword") %>
                <em>*</em>
            </p>
            <p>
                <label for="confirmPassword">
                    Confirm new password:</label>
                <%= Html.Password("confirmPassword") %>
                <em>*</em>
            </p>
          
        </fieldset>
          <div class="PanelBotonera">
                <hr />
                <input type="submit" value="Change Password" />
            </div>
    </div>
    <% } %>
</asp:Content>
