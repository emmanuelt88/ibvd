<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<string>" %>

<% 
    
    string idBuscador = "buscador" + Model;
    string cancionesElegidas = "canciones" + Model; 
%>


<script type="text/javascript" language="javascript">
 var idCancionesElegidas = "<%= cancionesElegidas %>";
 var idBuscador = "<%= idBuscador %>";
 
    $(document).ready(function() {
        $("#filterLoading").hide();
        $("#filterLoading").html(getSmallSpinner());
        
        
         $("#" + idBuscador).ajaxStart(function(){
            $("#filterLoading").show();
         });
         
         $("#" + idBuscador).ajaxStop(function(){
         setTimeout('$("#filterLoading").hide();',400);
         });
         
        $("#" + idBuscador).autocomplete('<%=Url.Action("BuscarCanciones", "Cancion") %>',
        {
            dataType: 'json',
            mustMatch: true,
            parse: function(data) {

                var rows = new Array();
                for (var i = 0; i < data.length; i++) {
                    rows[i] = { data: data[i], value: data[i].id, result: data[i].titulo };
                }
                return rows;
            },
                formatItem: function(row, i, max) {
                    var div = $("<div style='text-align:left; width:100%;'/>")
                    var container = $("<div></div>");
                    div.append(row.titulo);
                    container.append(div);
                    
                    return container.html();
                },
               
               
            width: 400,
            matchSubset: false,
        });
        
        $("#" + idBuscador).result(function(event, data, formatted) {
		    if (data){
			        agregarCancion(data.id, data.titulo);
			         $("#" + idBuscador).val('');
			    }
	        });
        
        // Lista ordenable de canciones
        $("#listaCancionesElegidas").sortable({
			placeholder: 'ui-state-highlight',
			beforeStop: function(event, ui){
			    actualizarCanciones();
			},
			scroll:true
		});

		$("#listaCancionesElegidas").disableSelection();

    });
    
    
        var canciones = new Array();
        function agregarCancion(id, titulo){
              canciones[id] = titulo;
              var tituloCorto = getTextToShow(titulo, 50);
              
              if(tituloCorto != titulo)
                 tituloCorto += "...";
              var item =  $("<LI class='ui-state-default' name='titulo"+id+"' ><SPAN class='ui-icon ui-icon-arrowthick-2-n-s'></SPAN><label title='" + titulo +"' style='width:97%;'>"+ tituloCorto +"</label><div style='float:right;'><SPAN class='ui-icon ui-icon-closethick hand' name='deleteButton' title='Eliminar la canción de la lista'><input type='hidden' name='valor' value='"+ id+"'></SPAN></div></LI>");
              
              var buttonDelete = $("span[name=deleteButton]", item);
              buttonDelete.click(function(){
                  item.remove();
                  eliminarCancion(id);
              });
              
              $("#listaCancionesElegidas").append(item);
              
              actualizarCanciones();
        }
        
        function actualizarCanciones(){
            var cancionesList = '';
            
            var items = $("#listaCancionesElegidas :hidden");
            
            for(var i =  0; i < items.length; i++){
                if( items[i].value != null &&  items[i].value != undefined )
                cancionesList += items[i].value+';';
            }
            
            if(items.length > 0){
                cancionesList = cancionesList.substring(0,cancionesList.length - 1);
            }
            
            $("#" + idCancionesElegidas).val(cancionesList);
        }
        function eliminarCancion(id){
            canciones[id] = null;
            actualizarCanciones();
        }
</script>




<div class="parrafo">
    
    <input type="hidden" id="<%= cancionesElegidas %>" name="<%= cancionesElegidas %>" />
    <label for="<%=idBuscador %>">
        T&iacute;tulo:</label>
    <input type="text" id="<%=idBuscador %>" size="20" style="float: left;" />
    <div style="width: 20px; float: left; text-align: center;">
        <div id="filterLoading" style="margin-left: 3px; margin-right: 3px;">
        </div>
        &nbsp;</div>
    <div style="float: left; width: 400px; padding: 10px;" class="ui-state-highlight">
        <h3>
            Listado de canciones elegidas:</h3>
        <ul id="listaCancionesElegidas" >
        </ul>
    </div>
</div>
