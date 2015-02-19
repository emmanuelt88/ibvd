<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= ViewData[IBVD.Digital.UI.Models.UICommonKeys.TITLE] %></h2>
    <% 
        StringBuilder comentario = new StringBuilder();
        
        comentario.Append("Utilice el siguiente formulario para crear una nueva cuenta de usuario<br />");
        comentario.AppendFormat("La contrase&ntildea debe tener como m&iacute;nimo de {0} carateres",ViewData["PasswordLength"]);
        Html.RenderPartial("UserControls/CommentPanel", comentario.ToString());
         %>
    

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Account Information</legend>
                  <table class="formulario">
                    <tr>
                        <td class="label">
                            Usuario:
                        </td>
                        <td class="field">
                        <%= Html.TextBox("username") %>
                            <em>*</em>
                        </td>
                    </tr>
                        <tr>
                        <td class="label">
                        Email:
                        </td>
                        <td class="field">
                    <%= Html.TextBox("email") %>
                            <em>*</em>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                        Contrase&ntilde;a:
                        </td>
                        <td class="field">
                    <%= Html.Password("password") %>
                            
                        </td>
                    </tr>
                    <tr>
                        <td class="label">
                        Confirmaci&oacute;n de contrase&ntilde;a:
                        </td>
                        <td class="field">
                    <%= Html.Password("confirmPassword") %>
                            <em>*</em>
                        </td>
                    </tr>
                    
                </table>
                
            </fieldset>
        </div>
        
        <div class="PanelBotonera">
            <hr />
            <input type="submit" value="Crear" class="button" ></input>
            <button onclick="javascript:window.location.href = '/User/Index';return false;" class="button" >Cancelar</button>
        </div>
    <% } %>
    
    
</asp:Content>
