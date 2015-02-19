<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IBVD.Digital.UI.Models.CancionViewData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  
<% Html.RenderPartial("DetailsUC", Model);%>

    <div class="PanelBotonera">
        <hr />
        <input type="button" class="button" onclick="<%=string.Format("location.href = '{0}'", Url.Action("Edit", "Cancion", new { Cancion_IdCancion = Model.Cancion.Id }))%>"
            value="Editar la canci&oacute;n" />
        <input type="button" class="button" onclick="<%= string.Format("location.href = '{0}'", Url.Action("Index", "Cancion"))%>"
            value="Ir al listado de canciones" />
    </div>
</asp:Content>
