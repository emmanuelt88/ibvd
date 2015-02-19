<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.Dictionary<string, string>>" %>
<% 
    StringBuilder comentarioDatos = new StringBuilder();

    comentarioDatos.Append("Seleccione los archivos que desea exportar, luego presione en el bot&oacute;n descargar");
    Html.RenderPartial("UserControls/CommentPanel", comentarioDatos.ToString());
    %>
    
<fieldset class="formulario Exportacion">
    <legend>Exportaci&oacute;n de la reuni&oacute;n</legend>
    <p>
        <label for="ExportarReunionPDF" style="width:80%;">
            PDF de la reuni&oacute;n:</label>
        <input type="checkbox" id="ExportarReunionPDF"  name="ExportarReunionPDF" checked="checked"/>
    </p>
    <p>
        <label for="ExportarCancionesPDF" style="width:80%;">
            PDF del listado de canciones:</label>
        <input type="checkbox" id="ExportarCancionesPDF"  name="ExportarCancionesPDF" checked="checked"/>
    </p>
    <p>
        <label for="ExportarCancionesPDF" style="width:80%;">
            PDF del listado de items de la reuni&oacute;n:</label>
        <input type="checkbox" id="ExportarItemsPDF"  name="ExportarItemsPDF" checked="checked"/>
    </p>
    <p>
        <label for="ExportarCancionesXML" style="width:80%;">XML para importar a la base de EasySlides:</label>
        <input type="checkbox" id="ExportarCancionesXML" name="ExportarCancionesXML" />
    </p>
    <p style="text-align:right;" id="downloadButtons">
        <a  id="btnDescargarReunionArchivos" href='<%=Url.Action("GenerarArchivo", "Reunion", new {Reunion_IdReunion = Model["idReunion"]}) %>'>Descargar</a>&nbsp;&nbsp;
    </p>
    
</fieldset>


<script type="text/javascript">
    $(document).ready(function() {
        $("#formDownloadReunion").submit(function() {
            return false;
        });

        $('.formulario.Exportacion :input').click(function() {
            actualizarLinkDescarga();
        });

        actualizarLinkDescarga();
    });

    function actualizarLinkDescarga() {
        var url = '<%=Url.Action("GenerarArchivo", "Reunion", new {Reunion_IdReunion = Model["idReunion"]}) %>';
        
        var itemsCheckeados = $('.formulario.Exportacion :checked');
        var nuevaURL = url;
        var datos = '';
        for (var i = 0; i < itemsCheckeados.length; i++) {
            datos += $(itemsCheckeados[i]).attr('name') + "-";
        }
        nuevaURL += "&itemsToExport=" + datos;

        if (datos.length > 0) {
            $("#btnDescargarReunionArchivos").enable();
            nuevaURL.substring(0, nuevaURL.length - 1);
        }
        else {
            $("#btnDescargarReunionArchivos").disable();
        }
        
        $("#btnDescargarReunionArchivos").attr('href', nuevaURL);
    }
    function inicializarExportarReunion(){
         $("#btmDescargar").hide();

    }
</script>

