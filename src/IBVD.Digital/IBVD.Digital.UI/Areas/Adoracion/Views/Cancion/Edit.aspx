<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IBVD.Digital.UI.Models.CancionViewData>" %>

<%@ Import Namespace="MVCSecrets.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<%
string root = Url.RequestContext.HttpContext.Request.Url.OriginalString;
string rest = Url.RequestContext.HttpContext.Request.Url.PathAndQuery;
root = root.Replace(rest, string.Empty);
    %>
    
    
    <% using (Html.BeginForm("SaveCancion", "Cancion", null, FormMethod.Post, new { id = "frmSaveCancion" }))
       {
    %>
      <% {
             StringBuilder comentarioDatos = new StringBuilder();

             comentarioDatos.Append("Utilice el siguiente formulario modificar los datos de la canci&oacute;n<br />");
             Html.RenderPartial("UserControls/CommentPanel", comentarioDatos.ToString());
         }
    %>
    <fieldset class="formulario">
        <legend>Datos de la canci&oacute;n</legend>
        <%= Html.Hidden("Cancion_IdCancion", Model.Cancion.Id)%>
        <%= Html.Hidden("Cancion_MODO", IBVD.Digital.UI.Areas.Adoracion.Helpers.Binders.MODO_VIEW.EDICION)%>
        <p>
            <label>T&iacute;tulo:</label>
            <%= Html.TextBox("Cancion_Titulo", Model.Cancion.Titulo, new { id = "Cancion_Titulo", maxlength = 100, style = "width:500px" })%><em>*</em>
        </p>
          <p>
            <label>
                Duraci&oacute;n estimada :
            </label>
            <%= Html.TextBox("Cancion_DuracionEstimada", Model.Cancion.DuracionEstimada, new { id = "txtDuracionEstimada", maxlength = 3, style = "width:30px;" })%>&nbsp;<i>Minutos</i><em>*</em>
        </p>
        <p>
            <label>Comp&aacute;s:</label>
            <%= Html.DropDownList("Cancion_Compas", Model.Compaces)%>
        </p>
        <p>
           <label> Tono:</label>
            <%= Html.DropDownList("Cancion_Tono", Model.Tonos)%>
        </p>
        
         <p >
                <label>Fondo de la canci&oacute;n:</label>
                    <div class"Tabla">
                        <div style="display:table-row" class="ImageItemToHide">
                              <div style="display:table-cell;vertical-align:top; width:100px; margin-top:10px; padding-right:30px; text-align:center;">
                                <img class="Perfil" alt="Fondo de la canción" id="cancionImage" width="100"  src="<%= Model.CancionPath  %>" />
                            </div>
                            <div style="display:table-cell;vertical-align:top;" class="ImageItemToHide">
                                 <input type="button" onclick="seleccionarImagen();" value="Seleccionar"/>
                            </div>
                        </div>
                        
                    </div>
                    <input type="hidden"  name="Cancion_FotoURI" id="Cancion_FotoURI" value="<%= Model.Cancion.FotoURI %>" />
            </p>
          
          <% {
             StringBuilder comentarioDatos = new StringBuilder();

             comentarioDatos.Append("Recuerde las etiquetas disponibles para la letra:<br />");
             comentarioDatos.Append("<ul>");
             comentarioDatos.Append("<li><b>[n]</b> para cada párrafo donde n es el número de parrafo que no puede ser repetido</li>");
             comentarioDatos.Append("<li><b>[chorus]</b> para el párrafo c&oacute;ro, sólo 1 vez</li>");
                 
             comentarioDatos.Append("</ul>");
             Html.RenderPartial("UserControls/CommentPanel", comentarioDatos.ToString());
         }
    %>
          
        <p>
            <label>Letra:</label>
            <textarea cols="83" rows="20" style="width: 80%;" id="txtLetra" name="Cancion_Letra"><%= Model.Cancion.Letra%></textarea>
        </p>
        
    </fieldset>
    
    
    <% } %>

<div id="divGallerSelector" >
   <% Html.RenderPartial("GallerySelect"); %>
    </div>
    
    <div class="PanelBotonera">
        <hr />
        <input type="button" id="btnGuardarCancion" value="Guardar"  onclick="SaveCancion();" />
        <input type="button" id="btnGuardarImagen" value="Seleccionar imagen" onclick="GuardarImagen();" style="display:none;" />
        <input type="button" id="btnCancelarGuardarImagen" value="Cancelar" onclick="CancelarGuardarImagen();" style="display:none;" />
        <input type="button"  onclick="<%= string.Format("location.href = '{0}'", Url.Action("Index", "Cancion"))%>"
            value="Ir al listado de canciones" />
    </div>
    <script type="text/javascript">
        $(document).ready(function() {
        $("#divGallerSelector").hide();
        onlyInputInt("#txtDuracionEstimada", 3);
        inputText("Ingrese el título de la canción", "Cancion_Titulo");
    });

    function CancelarGuardarImagen() {
        $("#btnGuardarImagen").fadeOut();
        $("#btnCancelarGuardarImagen").fadeOut();
        $("#btnGuardarCancion").fadeIn();
        $("#divGallerSelector").toggle(500, function() {
            $("#frmSaveCancion").fadeIn();

        });
    }
        function GuardarImagen() {
            $("#btnGuardarImagen").fadeOut();
            $("#btnGuardarCancion").fadeIn();
            $("#btnCancelarGuardarImagen").fadeOut();
            $("#divGallerSelector").toggle(500,function() {
            $("#frmSaveCancion").fadeIn();
            
            });
            var valor = $("#slideshow img").attr('src').replace('<%= root %>', '');
            $("#cancionImage").attr('src', valor);
            $("#Cancion_FotoURI").val(valor);
            
            
        }
        function SaveCancion() {
            blockUIWaiting();

            $.post("SaveCancion",
	            $("#frmSaveCancion").serialize(),
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

	        function seleccionarImagen() {
	            try {
	                setTextoCancion($("#txtLetra").val());
	                $("#btnGuardarImagen").fadeIn();
	                $("#btnCancelarGuardarImagen").fadeIn();
	                $("#btnGuardarCancion").fadeOut();
	                $("#frmSaveCancion").fadeOut(function() {

	                    $("#divGallerSelector").fadeIn(500);
//	                    setFirstActive();

	                });
	            }
	            catch (e) {
	            }
	            
	        }

	        $(document).ready(function() {

	    });


    </script>

</asp:Content>
