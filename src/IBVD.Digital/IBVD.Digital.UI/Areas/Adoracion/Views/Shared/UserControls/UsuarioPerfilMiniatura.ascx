<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IBVD.Digital.Logic.Entities.Usuario>" %>
<%
    string idControl = "DetallePerfil" + Model.UserName.GetHashCode().ToString().Replace("-", "_");
%>
<b><a id="<%= idControl %>" CoolToolTipID="divDetalleUsuario<%= idControl %>">
    <%= Model.UserName %></a></b>

<script type="text/javascript">
    $(document).ready(function() {
        $("#<%= idControl %>").CoolToolTip({ hideVelocity: 100 });
    });
    
</script>

<div id="divDetalleUsuario<%= idControl %>" class="CoolToolTipPopup" >
    <table>
        <tr>
            <td valign="top" >
                <img alt="Imagen de perfil" class='Perfil'  src="<%= Model.UserFotoURL %>" width="100" />
            </td>
            <td valign="top">
                <fieldset class="formulario" style="border-width:0px;">
                    <p  style="text-align:left;">
                        <label>
                            Usuario:</label>
                        <span style="text-align:left;">
                            <%= Model.UserName %></span>
                    </p>
                    <p  style="text-align:left;">
                        <label>
                            Nombre y Apellido:</label>
                        <span style="text-align:left;">
                            <%= Model.FullName %></span>
                    </p>
                     <p  style="text-align:left;">
                        <label>
                            Correo:</label>
                        <span style="text-align:left;"><a href="mailTo:<%= Model.Email %>"><%= Model.Email %></a></span>
                    </p>
                </fieldset>
            </td>
        </tr>
    </table>
</div>
