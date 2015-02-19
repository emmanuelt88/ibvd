<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% using (Html.BeginForm("ImportarBiblias", "Biblia", null, FormMethod.Post, new { id = "frmImportarBiblias" }))
       {%>
    <% 
        StringBuilder comentario = new StringBuilder();
        comentario.Append("Utilice el siguiente formulario importar biblias desde la base de EasySlides<br />");
        comentario.Append("Comprima los archivos .mdb de las biblias en un zip, y adjuntelo aqui para incorporarlo a la base de datos<br/>");
        Html.RenderPartial("UserControls/CommentPanel", comentario.ToString());
        string guid = Guid.NewGuid().ToString();
            
    %>
    <%= Html.Hidden("importacionGUID", guid, new { id = "importacionGUID" })%>
  
        <div>
            <%
                Html.RenderPartial("UserControls/UploadControl", new Dictionary<string, string>(){
                            {"ControlID","BibliasImportarLista"},
                            {"OnComplete","bibliasImportadas"},
                            {"FilesType","*.zip"},
                            {"FilesDescription","Archivos zip"},
                            {"GuardarText","Subir"},
                            {"Multiple","true"},
                            {"ItemsToDisableOnProgress", "#btnImportarArchivos"}
                        });
            %>
        </div>
        <div id="ContenedorArchivosGUID">
        </div>
        <div class="PanelBotonera">
            <hr />
            <input type="button" id="btnImportarArchivos" onclick="importarArchivos();" value="Guardar"
                class="button"  />
            <%= Html.ActionButton<HomeController>( "Cancelar", m => m.Index()) %>
        </div>
        <%} %>

        <script type="text/javascript">
            $(document).ready(function() {
                $("#btnImportarArchivos").disable();
                $("#frmImportarBiblias").submit(function() { return true; });

                $("#ReemplazarExistentes").click(function() {
                    var resultado = $(this).attr('checked');

                    $("#hReemplazarExistentes").val(resultado);
                });
            });

            function checkearImportacion() {
                var action = '<%= Url.Action("ObtenerEstadoImportacion","Biblia")  %>' + "?importacionGUID=" + $("#importacionGUID").val();

                $.getJSON(action, function(resultData) {
                    var message = '';
                    var items = eval('(' + resultData.Data.estados + ');')
                   
                    var finished = true;
                    for (var i = 0; i < items.length; i++) {
                        var item = items[i];
                        var template = $("#divTemplateItemLoading").clone();

                        template.find('div.Celda.Archivo').html(item.nombre);
                        var icon = 'IconProcess';

                        if (item.cargado)
                            icon = 'IconProcessAccepted';
                        else if (item.fallido) {
                            icon = 'IconProcessRemove';

                        }

                        finished = finished && (item.cargado || item.fallido);

                        template.find('div.Celda.Imagen div').attr('class', icon);
                        message += template.html();
                    }

                    if (finished) {


                        function finalizar() {
                            $.unblockUI();
                            mostrarNotificacion(
                        "La importación ha finalizado",
                        function() {

                        });
                        }

                        setTimeout(finalizar, 1000);

                    }
                    else {
                        $("#divImportandoMessage div").html(message);
                        setTimeout('checkearImportacion();', 1500);
                    }
                });
            }
            function bibliasImportadas(fileObj, response, data) {
                var resultData = eval('(' + response + ');');
                $("#ContenedorArchivosGUID").append($("<input type='hidden' name='Importacion_ArchivoGUID' value='" + resultData.Data.Ticket + "'/>"))
            }

            function importarArchivos() {
                blockUI("<div style='text-align:left;width:100%;'><div id='divImportandoMessage' style='text-align:left;'>Importando...<div style='text-align:left;' class='contenidoMessage'></div></div></div>");
                $("#btnImportarArchivos").disable();
                $.post('<%= Url.Action("ImportarBiblias","Biblia") %>',
	            $("#frmImportarBiblias").serialize(),
	                function(resultData) {
	                     setTimeout('checkearImportacion();', 2000);
	                },
	            'json');
            }
        </script>

        <div id="divTemplateItemLoading" style="display: none;">
            <div class="Tabla">
                <div class="Fila">
                    <div class="Celda Imagen">
                        <div class='IconProcess'>
                            &nbsp;</div>
                    </div>
                    <div class="Celda Archivo">
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
