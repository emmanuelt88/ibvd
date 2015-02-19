<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IBVD.Digital.UI.Models.BibliaBuscadorModel>" %>

<%

    if (Model.Biblias.Count > 0)
    {
%>

<div style="width: 100%;" class="ui-widget " id="DivFormBuscadorVersiculosContainer">
    <%
        StringBuilder librosBuilder = new StringBuilder();

        librosBuilder.Append("[");
        foreach (var libro in Model.Libros)
        {
            librosBuilder.Append("{");
            librosBuilder.AppendFormat("value : {0}, label : '{1}'", libro.Numero, libro.Descripcion);
            librosBuilder.Append("},");
        }

        librosBuilder.Append("]");

        string librosJSON = librosBuilder.ToString().Replace("},]", "}]");
        string descripcionLibro = Model.Libros.Count > 0 ? Model.Libros.First().Descripcion : string.Empty;
        string idLibro = Model.Libros.Count > 0 ? Model.Libros.First().Numero.ToString() : string.Empty;
    %>

    <script type="text/javascript">
        $(document).ready(function() {
            
            $('#ddlLibros').focus().autocomplete({
			minLength: 0,
			delay: 0,
			source: <%= librosJSON %>,
			focus: function(event, ui) {
				$('#ddlLibros').val(ui.item.label);
				return false;
			},
			select: function(event, ui) {
				$('#Buscador_LibroSeleccionado').val(ui.item.value);
				
				return false;
			},
			 change: autoCompleteMatchCheck,
			 close: autoCompleteExactMatchCheck
		}).data( "autocomplete" )._renderItem = function( ul, item ) {
			return $( "<li></li>" )
				.data( "item.autocomplete", item )
				.append( "<a><div style='text-align:left;'>"+ item.label + "</b></div></a>" )
				.appendTo( ul );
		};
	        
	      inputText("Ingrese el pasaje", "Buscador_PasajeSeleccionado");
	      inputText("Ingrese el libro", "ddlLibros");
        });
        
      
    </script>

    <input type="hidden" id="Buscador_LibroSeleccionado" name="Buscador_LibroSeleccionado" />
    <fieldset class="Formulario">
        <div class="Tabla" style="width: 650px; min-height:50px;" id="DivFormBuscadorVersiculos">
            <div class="Fila">
                <div style=" padding-top:15px;vertical-align: top;" class="Celda">
                    
                    <select name="Buscador_BibliaSeleccionada" id="Buscador_BibliaSeleccionada" multiple="multiple">
                        <% 
                            
                            string codigoSelected = Model.Biblias.First().Codigo;
                            foreach (var item in Model.Biblias)
                            {
                                string selected = item.Codigo.Equals(codigoSelected) ? " selected='selected' " : string.Empty;
                        %>
                        <option <%= selected %> value="<%= item.Codigo %>">
                            <%= item.Nombre%></option>
                        <%
                            }
                        %>
                    </select>
                </div>
                <div style=" vertical-align: top;"  class="Celda">
                    <p class="Parrafo">
                        <label for="ddlLibros" class="Label" style="float:none;">
                            Libro:</label>
                        <input type="text" style="width: 120px;" id="ddlLibros" name="Libro" class=" " />
                    </p>
                </div>
                <div style="vertical-align: top;"  class="Celda">
                    <p class="Parrafo">
                        <label for="txtPasaje" class="Label" style="float:none;">
                            Pasaje:</label>
                        <input type="text" id="Buscador_PasajeSeleccionado" name="Buscador_PasajeSeleccionado"
                            class="" />
                    </p>
                </div>
                 <div style="vertical-align: top; text-align: right;"  class="Celda">
                    <a onclick="buscarPasaje();" class="button">
                        <img src="/Content/Themes/base/images/Iconos/Search.png" />Seleccionar</a>
                </div>
            </div>
           
        </div>
    </fieldset>
</div>
<%
    }
    else
    {
%>
<h4>
    No hay biblias cargadas en la base de datos</h4>
<%
    }
%>



<script type="text/javascript">
    var $templatePopup = null;

    function buscarPasaje() {
        ocultarPanelErrores();
        showLoadingDiv('#DivFormBuscadorVersiculosContainer', 'Buscando...');
        var pasaje = $("#Buscador_PasajeSeleccionado").val();

        if (pasaje == 'Ingrese el pasaje') {
            pasaje = '';
        }
        $.post('<%= Url.Action("BuscarPasaje","Biblia") %>',
              {
                  Buscador_LibroSeleccionado: $("#Buscador_LibroSeleccionado").val(),
                  Buscador_PasajeSeleccionado: pasaje,
                  Buscador_BibliaSeleccionada: $("#Buscador_BibliaSeleccionada").val()
              },
              function(resultData) {

                  unBlockDIV("#DivFormBuscadorVersiculosContainer");
                  if (resultData.Success == true) {

                      // Si requiero comparar el pasaje en mas de 1 biblia
                      $("#ddlLibros").val(resultData.Data.LibroTexto);

                      if (resultData.Data.Comparar == 'true') {
                          mostrarComparadorPasajes(resultData.Data, resultData.Data.LibroTexto);
                      }
                      else {
                          try {
                              var pasajes = eval('(' + resultData.Data.Pasajes + ');');
                              var pasaje = pasajes[0];

                              pasajeBibliaEncontrado(resultData.Data.KeyCache, pasaje.Versiculos, { Nombre: pasaje.Biblia, Codigo: pasaje.BibliaCodigo }, $("#ddlLibros").val(), pasaje.PasajeTexto);
                              $("#Buscador_PasajeSeleccionado").val('');
                          }
                          catch (e) {
                          }
                      }

                  }
                  else {
                      handleResultData(resultData, function() {
                      });
                  }
              },
              'json'
              );

    }

    function getComparadorItemHTML(biblia, bibliaCodigo, versiculos, data) {

        var template = $("#comparadorPasajeItemTemplate").clone();
        var templateVersiculo = $("#TemplateVersiculo").clone();
        template.find('a.Header').html(biblia);


        for (var i = 0; i < versiculos.length; i++) {
            var versiculo = versiculos[i];
            var temp = templateVersiculo.clone();

            temp.find('.NroVersiculo').html(versiculo.Numero);
            temp.find('.Texto').html(versiculo.Texto);

            template.find('p').append(temp.html());

        }

        template.find('.PanelBotonera').addClass(bibliaCodigo);
        template.find(':button').button();
        template.find(':hidden[name=CodigoBiblia]').val(bibliaCodigo);
        return template.html();

    }
    function mostrarComparadorPasajes(data, libro) {
        var templatePopup = $("#popupComparadorPasajes").clone();
        var pasajes = eval('(' + data.Pasajes + ');');
        var pasajesByBibleCode = new Array();
        for (var i = 0; i < pasajes.length; i++) {
            var pasaje = pasajes[i];
            pasajesByBibleCode[pasaje.BibliaCodigo] = pasaje;
            var htmlText = getComparadorItemHTML(pasaje.Biblia, pasaje.BibliaCodigo, pasaje.Versiculos, data);
            templatePopup.find('.TabContainer').append(htmlText);

            templatePopup.find('.PanelBotonera.' + pasaje.BibliaCodigo + ' :button').click(function() {
                try {

                    var bibliaCode = $(this).parent().children(':hidden[name=CodigoBiblia]').val();
                    var currentPasaje = pasajesByBibleCode[bibliaCode];
                    pasajeBibliaEncontrado(data.KeyCache, currentPasaje.Versiculos, { Nombre: currentPasaje.Biblia, Codigo: currentPasaje.BibliaCodigo }, libro, currentPasaje.PasajeTexto);
                }
                catch (e) {
                }

                $templatePopup.dialog('close');
                $("#Buscador_PasajeSeleccionado").val('');
            });

        }
        // Acordion de buscadores
        var icons = {
            header: "ui-icon-circle-arrow-e",
            headerSelected: "ui-icon-circle-arrow-s"
        };

        templatePopup.find('div.TabContainer').accordion({
            icons: icons,
            autoHeight: false
        });
        $(templatePopup).attr('title', (libro + " " + data.PasajeTexto));
        $(templatePopup).dialog({ autoOpen: true, modal: true, heigth: 600, width: 800 });

        $templatePopup = $(templatePopup);
    }
    
</script>

<div id="popupComparadorPasajes">
    <div class="TabContainer" style=" text-align:left;">
        
    </div>
</div>


<div id="comparadorPasajeItemTemplate" style="display:none; text-align:left;">
  <h3>
    <a href="" class="Header"></a></h3>
    <div>
        
        <p class="ui-widget" style="padding: 0px 5px 5px 5px; text-align:left;">
        </p>
          <div class="PanelBotonera">
            <input type="button" value="Agregar" />
            <input type="hidden" name="CodigoBiblia" />
    </div>
    </div>    
</div>