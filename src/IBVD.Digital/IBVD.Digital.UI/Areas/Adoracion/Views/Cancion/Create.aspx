<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IBVD.Digital.UI.Models.CancionViewData>" %>

<%@ Import Namespace="MVCSecrets.Helpers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%
string root = Url.RequestContext.HttpContext.Request.Url.OriginalString;
string rest = Url.RequestContext.HttpContext.Request.Url.PathAndQuery;
root = root.Replace(rest, string.Empty);
    %>
    
    <script type="text/javascript">
        function createCancion() {
            blockUIWaiting();

            $.post('<%= Url.Action("NuevaCancion","Cancion") %>',
	            $("#frmCreateCancion").serialize(),
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

        $(document).ready(function() {
        onlyInputInt("#txtDuracionEstimada", 3);

        inputText("Ingrese el título de la canción", "Cancion_Titulo");
        });
    </script>
    <% using (Html.BeginForm("NuevaCancion", "Cancion", null, FormMethod.Post, new { id = "frmCreateCancion" }))
       {
    %>
      <% {
             StringBuilder comentarioDatos = new StringBuilder();

             comentarioDatos.Append("Utilice el siguiente formulario registrar una nueva canci&oacute;n<br />");
             Html.RenderPartial("UserControls/CommentPanel", comentarioDatos.ToString());
         }
    %>
    <%= Html.Hidden("Cancion_MODO", IBVD.Digital.UI.Areas.Adoracion.Helpers.Binders.MODO_VIEW.ALTA)%>
    <fieldset class="formulario">
        <legend>Datos de la canci&oacute;n</legend>
        <p>
            <label>
                T&iacute;tulo:
            </label>
            <%= Html.TextBox("Cancion_Titulo", "", new { id = "Cancion_Titulo", maxlength = 100, style = "width:500px;" })%><em>*</em>
        </p>
        <p>
            <label>
                Duraci&oacute;n estimada en minutos:
            </label>
            <%= Html.TextBox("Cancion_DuracionEstimada", "0", new { id = "txtDuracionEstimada", maxlength = 3, style = "width:30px;" })%>&nbsp;<i>Minutos</i><em>*</em>
        </p>
        <p>
            <label>
                Comp&aacute;s:
            </label>
            <%= Html.DropDownList("Cancion_Compas", Model.Compaces)%>
        </p>
        <p>
            <label>
                Tono:
            </label>
            <%= Html.DropDownList("Cancion_Tono", Model.Tonos)%>
        </p>
         <p >
                <label>Fondo de la canci&oacute;n:</label>
                    <div class="Tabla">
                        <div style="display:table-row" class="ImageItemToHide">
                              <div style="display:table-cell;vertical-align:top; width:100px; margin-top:10px; padding-right:30px; text-align:center;">
                                <img class="Perfil" alt="Fondo de la canción" id="cancionImage" width="100"  src="<%= Model.CancionPath  %>" />
                            </div>
                            <div style="display:table-cell;vertical-align:top;" class="ImageItemToHide">
                                 <input type="button" onclick="seleccionarImagen();" value="Seleccionar"/>
                            </div>
                            <div style="display:none;display:table-cell;vertical-align:top;" id="ImageSelector">
                                    
                                    
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
            <label>
                Letra:
            </label>
            <textarea id="txtLetra" cols="83" rows="20" style="width: 80%;" name="Cancion_Letra"><%= Model.Cancion.Letra%></textarea>
        </p>
    </fieldset>
    
    <% } %>
    
    
    <div id="divGallerSelector" style="display:none;">
    <% Html.RenderPartial("GallerySelect"); %>
    </div>
    <div class="PanelBotonera">
        <hr />
        <input type="button" id="btnGuardarCancion" value="Guardar" class="button" onclick="createCancion();" />
        <input type="button" id="btnGuardarImagen" value="Seleccionar imagen" class="button" onclick="GuardarImagen();" style="display:none;" />
    </div>
    
    
    
      <script type="text/javascript">
          $(document).ready(function() {
              onlyInputInt("#txtDuracionEstimada", 3);
          });
          function GuardarImagen() {
              $("#btnGuardarImagen").fadeOut();
              $("#btnGuardarCancion").fadeIn();
              $("#divGallerSelector").fadeOut(function() {
                  $("#frmCreateCancion").fadeIn();
              });
              var valor = $("#MainImageIMG").attr('src').replace('<%= root %>', '');
              $("#cancionImage").attr('src', valor);
              $("#Cancion_FotoURI").val(valor);


          }
          function SaveCancion() {
              blockUIWaiting();

              $.post("SaveCancion",
	            $("#frmCreateCancion").serialize(),
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
              setTextoCancion($("#txtLetra").val());
              $("#btnGuardarImagen").fadeIn();
              $("#btnGuardarCancion").fadeOut();
              $("#frmCreateCancion").fadeOut(function() {
              $("#divGallerSelector").fadeIn();
              setFirstActive();
              });

          }

    </script>
    
    
</asp:Content>
