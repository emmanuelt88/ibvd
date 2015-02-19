<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IBVD.Digital.UI.Models.DirectorioViewData>" %>

<%@ Import Namespace="MVCSecrets.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <% 
        StringBuilder comentario = new StringBuilder();
        comentario.Append("Utilice el siguiente formulario importar im&aacute;genes al diretorio.<br />");
        comentario.Append("Seleccione las im&aacute;genes, luego seleccione la carpeta de destino (Por defecto la carpeta es la Raiz), y presione el boton 'Subir im&aacute;genes'<br/>");
        Html.RenderPartial("UserControls/CommentPanel", comentario.ToString());
            
    %>
    <%
        Html.RenderPartial("UserControls/UploadControl", new Dictionary<string, string>(){
        {"ControlID","DirectoryGalleryManager"},
        {"FilesType","*.jpg;*.png;*.gif"},
        {"FilesDescription","Archivos de imagen"},
        {"GuardarText","Subir im&aacute;genes"},
        {"Multiple","true"},
        {"Auto","false"},
        {"LabelSeleccionar","Seleccionar"},
        {"ItemsToDisableOnProgress", "#btnGuardarDirectorio"},
        {"OnComplete","imageSelectOnComplete"}
    });
    %>

    <script type="text/javascript">

        var temporales = new Array();
        function imageSelectOnComplete(fileObj, response, data) {

            var resultData = eval('(' + response + ');');
            
            addImage("/Directorio/GetTempImage/" + resultData.Data.Ticket, "", resultData.Data.Ticket);
            temporales.push(resultData.Data.Ticket);
        }

        var folderImages = new Array();
        function moveImageToFolder(folder, image) {
            folderImages[image] = folder.replace('_', '/');
        }

        function cargarModificaciones() {
            var nuevosIDs = '';
            var modificaciones = '';
            for (var i = 0; i < temporales.length; i++) {
                nuevosIDs += temporales[i];

                if (i < temporales.length - 1) {
                    nuevosIDs += ',';
                }
            }

            var changes = false;
            for (var id in folderImages) {
                modificaciones += id + ":" + folderImages[id].replace();


                modificaciones += ',';
                changes = true;
            }

            if (changes)
                modificaciones = modificaciones.substring(0, modificaciones.length - 1);

            $("#DirectorioEdit_NuevosIDs").val(nuevosIDs);
            $("#DirectorioEdit_Modificaciones").val(modificaciones);
            $("#DirectorioEdit_CarpetasBorrar").val(getCarpetasBorrar());

        }

        function GuardarModificaciones() {
            cargarModificaciones();
            if ($("#DirectorioEdit_NuevosIDs").val() == '' && $("#DirectorioEdit_Modificaciones").val() == '' && $("#DirectorioEdit_CarpetasBorrar").val() == '') {
                mostrarNotificacion("No hay modificaciones realizadas", null);
                return;
            }
            blockUIWaiting();


            $.post("SaveDirectory",
            $("#frmSaveDirectorio").serialize(),
            function(resultData) {

                $.unblockUI();

                handleResultData(resultData, function() {
                    folderImages = new Array();
                    temporales = new Array();
                    cargarModificaciones();
                    resetImageGallery();
                });
            }, 'json'
            )
        }
    </script>
<% Html.RenderPartial("GalleryManager"); %>

    <% using (Html.BeginForm("SaveDirectory", "Directorio", null, FormMethod.Post, new { id = "frmSaveDirectorio" }))
       {%>
    
    <input type="hidden" value="" name="DirectorioEdit_NuevosIDs" id="DirectorioEdit_NuevosIDs" />
    <input type="hidden" value="" name="DirectorioEdit_Modificaciones" id="DirectorioEdit_Modificaciones" />
    <input type="hidden" value="" name="DirectorioEdit_CarpetasBorrar" id="DirectorioEdit_CarpetasBorrar" />
    
    <div class="PanelBotonera">
        <hr />
        <input type="button" id="btnGuardarDirectorio"  value="Guardar" onclick="GuardarModificaciones();" />
        <%= Html.ActionButton<HomeController>("Cancelar", m => m.Index()) %>
    </div>
    <%}%>
</asp:Content>
