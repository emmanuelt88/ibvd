<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteBasicSmall.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

     <% Html.RenderPartial("UserControls/CommentPanel", "Por favor ingrese su email para proceder a enviarle sus datos"); %>
    <% using (Html.BeginForm("Recuperar", "Account", null, FormMethod.Post, new { id = "frmRecuperar" }))
       { %>
       
    <div class="Logon" align="center">
        <fieldset class="formulario">
            <p>
                <label for="email" style="float:none;display:block;">
                    Email:</label>
                <%= Html.TextBox("email", null, new {  })%><em>*</em>
            </p>
            <p style="text-align:left;">
            <p>
                <input type="button" id="btnRecuperar" value="Recuperar datos" class="button" style="float:right;" onclick="recuperar();" />
            </p>
        </fieldset>
    </div>
    <% } %>
    
    <script type="text/javascript">
        $(document).ready(function() {
        $("#frmRecuperar").submit(function() { return false; });

          $("#frmRecuperar input").keypress(function(e) {
                if (e.which == 13) {
                    $("#btnRecuperar").click();
                }
            });
        });

        function recuperar() {
            blockUI('Enviando datos a su email');
            ocultarPanelErrores();
            $.post('<%= Url.Action("Recuperar","Account") %>',
                 $("#frmRecuperar").serialize(),
                 function(resultData) {
                     $.unblockUI();
                     
                     handleResultData(resultData, function() {
                         if (resultData.Success) {
                             document.location.href = resultData.Action;
                         }
                     });


                 }
                
            );
        }
    </script>
    

</asp:Content>
