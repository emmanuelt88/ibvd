<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IBVD.Digital.UI.Models.UserViewData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        <%= ViewData[IBVD.Digital.UI.Models.UICommonKeys.TITLE] %></h2>
    <fieldset class="formulario">
        <legend>Datos personales</legend>
         <p>
            <img class="Perfil" width="100" alt="Imagen de perfil" src="<%= Model.UserFotoURL %>" />
        </p>
        <p>
            <label>
                Usuario:
            </label>
            <strong>
                <%= Model.User.UserName %></strong>
        </p>
        <p>
            <label>
                Nombre:
            </label>
            <span><%= Model.User.Nombre %></span>
        </p>
        <p>
            <label>
                Apellido:
            </label>
            <span><%= Model.User.Apellido %></span>
        </p>
        <p>
            <label>
                Email:
            </label>
            <strong>
                <%= Model.User.Email %></strong>
        </p>
        <p>
            <label>
                Fecha de Nacimiento:
            </label>
            <%= Model.User.FechaNacimiento.ToString("dd/MM/yyyy")%>
        </p>
     <%--   <p>
            <label>
                Sexo:
            </label>
            <span><%= Model.Sexos.FirstOrDefault(m=> m.Selected)!= null? Model.Sexos.Single(m=> m.Selected).Text : string.Empty%></span>
        </p>--%>
        <p>
            <label>
                Domicilio:
            </label>
            <span><%= Model.User.Domicilio %></span>
        </p>
        <p>
            <label>
                Tel&eacute;fono particular:
            </label>
           <span><%= Model.User.TelefonoParticular %></span>
        </p>
        <p>
            <label>
                Tel&eacute;fono celular:
            </label>
            <span><%= Model.User.TelefonoCelular %></span>
        </p>
       
    </fieldset>
    
    <script type="text/javascript">
        $(document).ready(function() {
        
            fillEmptyFields();
        });
    </script>
</asp:Content>
