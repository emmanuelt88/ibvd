<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>



<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div id="MainContents">
        <% using (Html.BeginForm("Registrar", "User", null, FormMethod.Post, new { id = "frmRegistrarUsuario" }))
           {%>
        <%= Html.Hidden("Usuario_MODO", IBVD.Digital.UI.Areas.Adoracion.Helpers.Binders.MODO_VIEW.ALTA)%>
        <% 
            Html.RenderPartial("UserControls/CommentPanel",
                string.Format("Modifique sus datos personales y luego presione guardar"));
        %>
        <fieldset class="formulario">
            <legend>Datos personales</legend>
            <p>
                <label>
                    Usuario:</label>
                    <%= Html.TextBox("Usuario_UserName")%><em>*</em
            </p>
            <p>
                <label>
                    Email:</label>
                <%= Html.TextBox("UsuarioAlta_Email")%><em>*</em>
            </p>
        </fieldset>
        <div class="PanelBotonera">
            <hr />
            <input type="button" id="btnRegistrarUsuario" onclick="registrarUsuario();" value="Registrar" class="button" />
            <%= Html.ActionButton<HomeController>( "Cancelar", m => m.Index())%>
        </div>
        <% } %>

        <script type="text/javascript">
            $(document).ready(function() {
            $("#frmRegistrarUsuario").submit(function() { return false; });
            });
            function registrarUsuario() {
                blockUIWaiting();

                $.post('<%= Url.Action("Registrar","User") %>',
	            $("#frmRegistrarUsuario").serialize(),
	            function(resultData) {
	                $.unblockUI();

	                handleResultData(resultData, function() {
	                    if (resultData.Success) {
	                        document.location.href = resultData.Action;
	                    }
	                });
	            },
	            'json');
            }

    
        </script>
        </div>
</asp:Content>

