<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IBVD.Digital.UI.Models.UserViewData>" %>

<%@ Import Namespace="MVCSecrets.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="MainContents">
        <% using (Html.BeginForm("SaveUser", "User", null, FormMethod.Post, new { id = "frmSaveUser" }))
           {%>
        <input type="hidden" name="Usuario_UserName" value="<%= Model.User.UserName %>" />
        <%= Html.Hidden("Usuario_MODO", IBVD.Digital.UI.Areas.Adoracion.Helpers.Binders.MODO_VIEW.EDICION)%>
        <% 
            Html.RenderPartial("UserControls/CommentPanel",
                string.Format("Modifique sus datos personales y luego presione guardar"));
        %>
        <fieldset class="formulario">
            <legend>Datos personales</legend>
            <p>
                <label>
                    Usuario:</label>
                <strong>
                    <%= User.Identity.Name %></strong>
            </p>
              <p>
            <label>
                    Contrase&ntilde;a:</label>
                    <%= Html.Password("UsuarioEdit_Password", Model.User.Password)%><em>*</em
            </p>
            <p>
                  <label>
                    Confirmaci&oacute;n de contrase&ntilde;a:</label>
                    <%= Html.Password("UsuarioEdit_ConfirmPassword", Model.User.Password)%><em>*</em
            </p>
            <p>
                <label>
                    Nombre:</label>
                <%= Html.TextBox("UsuarioEdit_Nombre", Model.User.Nombre) %><em>*</em>
            </p>
            <p>
                <label>
                    Apellido:</label>
                <%= Html.TextBox("UsuarioEdit_Apellido", Model.User.Apellido)%><em>*</em>
            </p>
            <p>
                <label>
                    Email:</label>
                <%= Model.User.Email %>
                <%= Html.Hidden("Email", Model.User.Email)%>
            </p>
            <p>
                <label>
                    Imagen de perfil:</label>
                    <div class="Tabla">
                        <div class="Fila">
                              <div class="Celda" style="vertical-align:top; width:100px; margin-top:10px; padding-right:30px; text-align:center;">
                                <img class="Perfil" alt="Foto de perfil" id="userPerfilImage" width="100" style="<%= Model.UserFotoURL != string.Empty? "": "display:none;"%>" src="<%= Model.UserFotoURL %>" />
                            </div>
                            <div style="vertical-align:top;" class="Celda">
                                <%
                                    Html.RenderPartial("UserControls/UploadControl", new Dictionary<string,string>(){
                                        {"ControlID","UserEditImageControl"},
                                        {"OnComplete","imageSelectOnComplete"},
                                        {"FilesType","*.jpg;*.png;*.gif"},
                                        {"FilesDescription","Archivos de imagen"},
                                        {"GuardarText","Subir"},
                                        {"ItemsToDisableOnProgress", "#btnGuardarUsuario"}
                                    });
                                %>
                            </div>
                        </div>
                    </div>
                    <input type="hidden" name="UsuarioEdit_FotoGUID" id="UsuarioEdit_FotoGUID" value="<%= Model.User.Foto.Id %>" />
                    
<script type="text/javascript">
    function imageSelectOnComplete(fileObj, response, data) {
        var resultData = eval('(' + response + ');');
        $("#userPerfilImage").hide();
        $("#userPerfilImage").attr('src', "/Upload/GetTempFile/" + resultData.Data.Ticket);
        $("#userPerfilImage").fadeIn(1000);
        
        $("#UsuarioEdit_FotoGUID").val(resultData.Data.Ticket);
    }
</script>

            </p>
            <p>
                <label>
                    Fecha de Nacimiento:</label>
                <%= Html.TextBox("UsuarioEdit_FechaNacimiento", Model.User.FechaNacimiento.ToString("dd/MM/yyyy"), new { @class = "fecha", @readonly = "readonly", id = "FechaNacimiento" })%><em>*</em>
            </p>
            <p>
                <label>
                    Sexo:</label>
                <%= Html.DropDownList("UsuarioEdit_Sexo", Model.Sexos)%><em>*</em>
            </p>
            <p>
                <label>
                    Domicilio:</label>
                <%= Html.TextBox("UsuarioEdit_Domicilio", Model.User.Domicilio)%>
            </p>
            <p>
                <label>
                    Tel&eacute;fono particular:</label>
                <%= Html.TextBox("UsuarioEdit_TelefonoParticular", Model.User.TelefonoParticular, new { maxlength = 15 })%>
            </p>
            <p>
                <label>
                    Tel&eacute;fono celular:</label>
                <%= Html.TextBox("UsuarioEdit_TelefonoCelular", Model.User.TelefonoCelular, new { maxlength = 15 })%>
            </p>
        </fieldset>
        <div class="PanelBotonera">
            <hr />
            <input type="button" id="btnGuardarUsuario" onclick="guardarUsuario();" value="Guardar" class="button" />
            <%= Html.ActionButton<HomeController>("Cancelar", m => m.Index()) %>
        </div>
        <% } %>

        <script type="text/javascript">
            $(document).ready(function() {
                $("#FechaNacimiento").datepicker({ showOn: 'button', buttonImage: '<%= ContentHelper.CurrentDirectory.CurrentTheme() %>/images/calendar.gif', buttonImageOnly: true, changeMonth: true, yearRange: '<%= System.DateTime.Now.Year-80 %>:<%= System.DateTime.Now.Year %>',
                    changeYear: true
                });


            });

            function guardarUsuario() {
                blockUIWaiting();

                $.post("SaveUsuario",
	            $("#frmSaveUser").serialize(),
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
