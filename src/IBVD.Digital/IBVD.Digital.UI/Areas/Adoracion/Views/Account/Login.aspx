<%@ Page Language="C#" MasterPageFile="~/Views/Shared/SiteBasicSmall.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">

    <% Html.RenderPartial("UserControls/CommentPanel", "Por favor ingrese su usuario y contraseña"); %>
    <% using (Html.BeginForm("Autenticar", "Account", null, FormMethod.Post, new { id = "frmLogin" }))
       { %>
       <input  type="hidden" name="returnUrl" value="<%= ViewData["returnURL"] %>"/>
    <div class="Logon" align="center">
        <fieldset class="formulario">
            <legend>Informaci&oacute;n para su autenticaci&oacute;n</legend>
            <p>
                <label for="username" style="float:none;display:block;">
                    Usuario:</label>
                <%= Html.TextBox("username", null, new {  })%><em>*</em>
            </p>
            <p>
                <label for="password" style="float:none;display:block;">
                    Contrase&ntilde;a:</label>
                <%= Html.Password("password", null, new {  })%><em>*</em>
            </p>
            <p style="text-align:left;">
            <label class="inline" for="rememberMe" style="float:none;">
                    ¿Recordarme?</label>
                <%= Html.CheckBox("rememberMe") %>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <%= Html.ActionLink("Recordar datos","ForgetPassword") %>
                
            </p>
            <p>
                <input type="button" id="btnAutenticar" value="Autenticar" class="button" style="float:right;"  />
            </p>
        </fieldset>
    </div>
    <% } %>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $("#btnAutenticar").click(function () {
                autenticar(); 
            });
            $("#frmLogin").submit(function () { autenticar(); return false; });

            $("#frmLogin input").keypress(function (e) {
                if (e.which == 13) {
                    $("#frmLogin").submit();
                }
            });
        });

        function autenticar() {
            blockUI('Autenticando usuario');
            ocultarPanelErrores();
            $.post('<%= Url.Action("Autenticar","Account") %>',
                 $("#frmLogin").serialize(),
                 function (resultData) {
                     $.unblockUI();
                     handleResultData(resultData, function () {
                     });
                     if (resultData.Success) {

                         document.location.href = resultData.Action;
                     }
                 }
            );
        }
    </script>
    
   
</asp:Content>
