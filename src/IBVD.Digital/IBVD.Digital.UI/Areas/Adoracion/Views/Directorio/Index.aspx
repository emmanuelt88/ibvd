<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IBVD.Digital.UI.Models.DirectorioViewData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


<script type="text/javascript">
    $(document).ready(function() {
        if ($("#DirectorioImagenesVacio").val() == 'true') {
            $("#btnDescargarDirectorioImagenes").hide();
        }
        else {
            cargarGaleria();
        }
    });

    function cargarGaleria() {
        blockDIV("#GaleriaContenedor");

        $("#GaleriaContenedor").load('/Directorio/GetGalleryViewerUC',
        {carpeta : ""});
    }
  

    
</script>


<a id="btnDescargarDirectorioImagenes" href='<%= Url.Action("Descargar","Directorio") %>' class="hand">Exportar directorio de im&aacute;genes</a>
<br /><br />

<div id="GaleriaContenedor">
<%--<% Html.RenderPartial("GalleryViewer", Model); %>--%>
</div>

</asp:Content>
