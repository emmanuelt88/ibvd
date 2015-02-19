<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     
        <% using (Html.BeginForm("ImportarCanciones", "Cancion", null, FormMethod.Post, new { id = "frmImportarCanciones" }))
           {%>
           
            <% 
            StringBuilder comentario = new StringBuilder();
            comentario.Append("Utilice el siguiente formulario importar canciones desde la base de EasySlides<br />");
            comentario.Append("Ingrese el archivo XML que puede exportar desde el programa<br/>");
            Html.RenderPartial("UserControls/CommentPanel", comentario.ToString());
            %>
                <fieldset class="formulario">
        <legend>Importaci&oacute;n de canciones al sistema</legend>
            <p>
                <label for="ReemplazarExistentes">
                    Reemplazar existentes</label>
                <input type="checkbox"  id="ReemplazarExistentes"  /><em>*</em>
                <input type="hidden" name="reemplazarExistentes" id="hReemplazarExistentes" value="false" />
            </p>
        </fieldset>
            <div>
                   <%
                        Html.RenderPartial("UserControls/UploadControl", new Dictionary<string,string>(){
                            {"ControlID","CancionesImportarLista"},
                            {"OnComplete","cancionesImportadas"},
                            {"FilesType","*.xml"},
                            {"FilesDescription","Archivos XML"},
                            {"GuardarText","Subir"},
                            {"ItemsToDisableOnProgress", "#btnImportarArchivos"}
                        });
                    %>
            </div>
            <input type="hidden" name="Importacion_ArchivoGUID" id="ArchivoCancionesGUID" />
          <div class="PanelBotonera">
            <hr />
            <input type="button" id="btnImportarArchivos" onclick="importarArchivos();" value="Guardar" class="button" disabled="disabled" />
            <%= Html.ActionButton<CancionController>("Listado de canciones", m => m.Index()) %>
        </div>
        <%} %>
        
    <script type="text/javascript">
        $(document).ready(function() {
            $("#frmImportarCanciones").submit(function() { return false; });

            $("#ReemplazarExistentes").click(function() {
                var resultado = $(this).attr('checked');

                $("#hReemplazarExistentes").val(resultado);
            });
        });
            function cancionesImportadas(fileObj, response, data) {
                
                var resultData = eval('(' + response + ');');

                $("#ArchivoCancionesGUID").val(resultData.Data.Ticket);

               
            }

            function importarArchivos() {
                blockUI("Importando...");

                $.post('<%= Url.Action("ImportarCanciones","Cancion") %>',
	            $("#frmImportarCanciones").serialize(),
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
</asp:Content>
