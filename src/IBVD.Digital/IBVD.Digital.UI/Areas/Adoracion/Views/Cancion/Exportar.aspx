<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% 
    StringBuilder comentarioDatos = new StringBuilder();

    comentarioDatos.Append("Utilice este formulario para exportar las canciones de la base de datos");
    Html.RenderPartial("UserControls/CommentPanel", comentarioDatos.ToString());
%>

<fieldset class="formulario Exportacion">
    <legend>Exportaci&oacute;n de las canciones</legend>
    <p>
        <label for="ExportarCancionesXML" style="width:80%;">XML para importar a la base de EasySlides</label>
        <a class="Button"  id="btnDescargarReunionArchivos" href='<%=Url.Action("GenerarCanciones", "Cancion") %>'>Descargar</a>&nbsp;&nbsp;
        
    </p>
    
</fieldset>
</asp:Content>
