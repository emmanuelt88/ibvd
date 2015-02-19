<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IBVD.Digital.UI.Models.DirectorioViewData>" %>
<div class="ui-helper-reset ui-widget-content ui-corner-all ItemGallery Imagen" style="display: none;"
    id="TemplateImagedirectoryGallery">
    <input type="hidden" name="ItemID" />
    <input type="hidden" name="Carpeta" />
    <div>
        <div>
            <img width="96" height="72" class="moverHand" alt="" />
        </div>
    </div>
</div>
<div class="ui-helper-reset ui-widget-content ui-corner-all ItemGallery Carpeta"
    title="Raiz" style="display: none;" id="TemplateFolderdirectoryGallery">
    <input type="hidden" name="Carpeta" />
    <input type="hidden" name="DirectorioID" />
    <div style="margin: auto;">
        <div>
            <img alt="" src="/Content/Themes/base/images/Iconos/FolderPictures.png" />
        </div>
        <div>
            <span class="Titulo"></span>
        </div>
    </div>
</div>
<div class="Fila" id="templateFolderToRemove" style="display: none;">
    <div class="Celda" style="width: 10px; text-align: right;">
        <img class="handHover" src="../../Content/Themes/base/images/Iconos/ErrorCircle16.png"
            title="Quitar de papelera" />
    </div>
    <div class="Celda Carpeta" style="width: 100%;">
        <span class="Titulo"></span>
    </div>
</div>
<%@ Import Namespace="IBVD.Digital.UI.Areas.Adoracion.Helpers" %>
<style type="text/css">
    .DivImageGalleryEditor
    {
        width: 100%;
    }
    .DivImageGalleryFolders, .DivImageGalleryImages
    {
        width: 100%;
        height: 100%;
        height: 1%;
        overflow: hidden;
    }
    .DivImageGalleryEditor div.ItemGallery
    {
        display: inline;
        float: left;
        list-style-type: none;
        text-align: center;
        width: 80px;
        height: 60px;
        margin-right: 2px;
        text-align: center;
        cursor: hand;
        cursor: pointer;
        padding: 0px;
    }
    .DivImageGalleryEditor div.ItemGallery div
    {
        margin: 0px;
        padding: 0px;
    }
    .DivImageGalleryEditor div.ItemGallery img
    {
        width: 80px;
        height: 60px;
        margin: auto;
        list-style-type: none;
    }
    .DivImageGalleryEditor div.ItemGallery.Carpeta
    {
        width: 80px;
        margin: auto;
        list-style-type: none;
        margin-right: 4px;
        height: 70px;
    }
    .DivImageGalleryEditor div.ItemGallery.Carpeta img
    {
        width: 50px;
        height: 50px;
    }
</style>

<script type="text/javascript">
    var currentDirectory = "<%= Model.Root %>";
    var rootDirectoryID = '<%= string.Format("Directorio_{0}", Model.Root.GetHashCode()) %>';
    var currentDirectoryID = '<%= string.Format("Directorio_{0}", Model.Root.GetHashCode()) %>';

    var $directoryGallery = $('#DirectoryGallery');
    var carpetasEliminadas = new Array();
    $(document).ready(function() {
        inputText("Nombre de la carpeta", "txtFolderName", "#btnAddFolder");
        $directoryGallery = $('#DirectoryGallery');

        $('.ItemGallery.Imagen', $directoryGallery).draggable({
            cancel: 'a.ui-icon', // clicking an icon won't initiate dragging
            revert: 'invalid', // when not dropped, the item will revert back to its initial position
            opacity: 0.7,
            helper: 'clone',
            cursor: 'move'
        });

        $('.ItemGallery.Carpeta.Removible', $directoryGallery).draggable({
            cancel: 'a.ui-icon', // clicking an icon won't initiate dragging
            revert: 'invalid', // when not dropped, the item will revert back to its initial position
            opacity: 0.7,
            helper: 'clone',
            cursor: 'move'
        });

        var carpetas = $('.ItemGallery.Carpeta:not(.Papelera)', $directoryGallery);
        for (var i = 0; i < carpetas.length; i++) {
            var carpeta = $(carpetas[i]);
            var directorioClass = carpeta.children("input[name=DirectorioID]").val();
            carpeta.droppable({
                accept: '.ItemGallery.Imagen:not(.' + directorioClass + ')',
                activeClass: 'ui-state-highlight',
                drop: function(ev, ui) {
                    moverImagen(ui.draggable, $(this));

                }
            });
        }

        $('.ItemGallery.Carpeta.Papelera', $directoryGallery).droppable({
            accept: '.ItemGallery:not(.ItemGallery.PapeleraID)',
            activeClass: 'ui-state-highlight',
            drop: function(ev, ui) {

                if ($(ui.draggable).attr('class').search("Imagen") > 0) {
                    moverImagen(ui.draggable, $(this));
                }
                else {
                    borrarCarpeta(ui.draggable);
                }

            }
        });



        $('.ItemGallery.Carpeta').click(function() {
            setActiveFolder($(this));
        });


        function borrarCarpeta($item) {
            var classActive = "ui-state-focus";
            var directorioIDOld = $item.children('input[name=Carpeta]').val();
            var nombre = $item.find('span.Titulo').text();
            carpetasEliminadas[directorioIDOld] = $item;

            if ($item.attr('class').search(classActive) >= 0) {
                $("#" + rootDirectoryID).click();
            }
            $item.fadeOut();
            var directorioDestino = $item.children("input[name=Carpeta]").val();


            var template = $("#templateFolderToRemove").clone();

            template.find('span.Titulo').html(nombre);
            template.show();
            template.find('img').click(function() {
                $(template).remove();
                $item.fadeIn();
                if ($("#CarpetasToDelete").find('.Fila').length == 0) {

                    carpetasEliminadas[directorioIDOld] = false;
                }
            });

            $("#CarpetasToDelete").find('.Tabla').append(template);

            carpetasEliminadas[directorioIDOld] = true;


        }



    });

    function moverImagen($item, $carpetaDestino) {
        var itemID = $item.children('input[name=ItemID]').val();
        var directorioIDOld = $item.children('input[name=Carpeta]').val();
        var directorioIDNew = $carpetaDestino.children("input[name=DirectorioID]").val();
        var directorioDestino = $carpetaDestino.children("input[name=Carpeta]").val();
       
        $item.fadeOut();
        $item.addClass(directorioIDNew);
        $item.removeClass(directorioIDOld);
        $item.children('input[name=Carpeta]').val(directorioIDNew);
        moveImageToFolder(directorioDestino, itemID);
    }
    function addImage(src, name, ticket) {

        var template = $("#TemplateImagedirectoryGallery").clone();
        template.attr('id', ticket);
        template.css('display', 'inline');
        template.find('img').attr('src', src);
        template.find('input[name=ItemID]').val(ticket);
        template.find('input[name=Carpeta]').val(currentDirectoryID);
        template.addClass(currentDirectoryID);

        $(".DivImageGalleryImages").append(template);

        // let the directoryGallery items be draggable
        $(template).draggable({
            cancel: 'a.ui-icon', // clicking an icon won't initiate dragging
            revert: 'invalid', // when not dropped, the item will revert back to its initial position
            helper: 'clone',
            opacity: 0.7,
            cursor: 'move'
        });

        moveImageToFolder(currentDirectory, ticket);

    }
    function setActiveFolder($folder) {
        var classActive = "ui-state-focus";
        var directorioIDNew = $folder.children("input[name=DirectorioID]").val();
        var carpeta = $folder.children("input[name=Carpeta]").val();
        var directorioIDOld = $('.ItemGallery.Carpeta.' + classActive, $directoryGallery).children("input[name=DirectorioID]").val();
        $("." + directorioIDOld).hide();


        $('.ItemGallery.Carpeta.' + classActive, $directoryGallery).removeClass(classActive);
        $("." + directorioIDNew).show();
        if ($("." + directorioIDNew).length == 0) {
            $(".DivImageGalleryImages").find('h3').show();

        }
        else {
            $(".DivImageGalleryImages").find('h3').hide();
        }

        $folder.addClass(classActive);
        currentDirectory = carpeta;
        currentDirectoryID = directorioIDNew;

    }

    function resetImageGallery() {
        $(".PapeleraID").remove();
        carpetasEliminadas = new Array();
        $("#CarpetasToDelete .Fila").remove();
        $("#CarpetasToDelete").hide();

    }

    function addFolder() {
        if ($.trim($("#txtFolderName").val()) == '') {
            mostrarErrorPopup('Debe ingresar un nombre de carpeta primero', null);
            return;
        }
        blockUIWaiting();

        $.post('<%= Url.Action("AddFolder","Directorio")  %>',
        { name: $("#txtFolderName").val() },
        function(resultData) {
            $.unblockUI();
            if (resultData.Success) {
                var template = $("#TemplateFolderdirectoryGallery").clone();

                template.find('input[name=DirectorioID]').val("Directorio_" + resultData.Data.ID);
                template.find('input[name=Carpeta]').val("/Files/Directorio/" + $("#txtFolderName").val());
                template.attr('title', $("#txtFolderName").val());
                template.find('span.Titulo').html(resultData.Data.TituloCorto);
                template.removeAttr('id');
                $(".DivImageGalleryFolders").append(template);
                template.fadeIn();
                $(template).draggable({
                    cancel: 'a.ui-icon', // clicking an icon won't initiate dragging
                    revert: 'invalid', // when not dropped, the item will revert back to its initial position
                    opacity: 0.7,
                    helper: 'clone',
                    cursor: 'move'
                });

                $(template).droppable({
                    accept: '.ItemGallery.Imagen',
                    activeClass: 'ui-state-highlight',
                    drop: function(ev, ui) {

                        moverImagen(ui.draggable, $(this));

                    }
                });

                $(template).click(function() {
                    setActiveFolder($(this));
                });

                $(".DivImageGalleryFolders").find('h3').hide();
            }
            else {
                handleResultData(resultData, null);
            }

            $("#txtFolderName").val('');
            $("#txtFolderName").blur();



        });


    }

    function getCarpetasBorrar() {
        var carpetas = "";

        for (var i in carpetasEliminadas) {
            if (carpetasEliminadas[i] == true) {
                carpetas += i + ",";
            }
        }

        return carpetas;
    }
</script>

<% {
       StringBuilder comentario = new StringBuilder();
       comentario.Append("Para crear una nueva carpeta ingrese el nombre y presione el siguiente link");
       Html.RenderPartial("UserControls/CommentPanel", comentario.ToString());
   }    
%>
<input type="text" value="" class="inputItalic" id="txtFolderName" />
<input id="btnAddFolder" type="button" class="ui-state-disabled" onclick="addFolder();"
    value="Nueva carpeta" />
<br />
<br />
<% {
       StringBuilder comentario = new StringBuilder();
       comentario.Append("Para modificar el directorio de la im&aacute;gen arrastrela a la carpeta deseada. Si desea eliminar una carpeta o archivo arr&aacute;strela a la papelera. ");
       comentario.Append("Haciendo click sobre cualquier carpeta podr&aacute; ver el contenido, en caso de la papelera podr&aacute; recuperar los elementos borrados.");
       Html.RenderPartial("UserControls/CommentPanel", comentario.ToString());
   }       
%>
<div class="DivImageGalleryEditor" id="DirectoryGallery">
    <div class="DivImageGalleryFolders">
        <%{
              string directorio = Model.Root;
              string directorioID = string.Format("Directorio_{0}", directorio.GetHashCode());
        %>
        <div class="ui-helper-reset ui-widget-content ui-corner-all ItemGallery Carpeta"
            title="Raiz" id="<%= directorioID %>">
            <input type="hidden" name="Carpeta" value="<%= directorio %>" />
            <input type="hidden" name="DirectorioID" value="<%= directorioID %>" />
            <div style="margin: auto;">
                <div>
                    <img src="/Content/Themes/base/images/Iconos/FolderPictures.png" />
                </div>
                <div>
                    Raiz
                </div>
            </div>
        </div>
        <div class="ui-helper-reset ui-widget-content ui-corner-all ItemGallery Carpeta Papelera"
            style="display: block;" title="Raiz" id="PapeleraID">
            <input type="hidden" name="Carpeta" value="Papelera" />
            <input type="hidden" name="DirectorioID" value="PapeleraID" />
            <div style="margin: auto;">
                <div>
                    <img src="/Content/Themes/base/images/Iconos/Trash-Empty-icon.png" />
                </div>
                <div>
                    Papelera
                </div>
            </div>
        </div>
        <%
            }
          foreach (var carpeta in Model.Carpetas)
          {
              string[] partes = carpeta.Split('/');
              string nombre = partes[partes.Length - 1];
              string nombreCorto = UIStringHelper.GetShortTitle(nombre, 12);

              string directorio = carpeta;
              string directorioID = string.Format("Directorio_{0}", directorio.GetHashCode());
            
        %>
        <div class="ui-helper-reset ui-widget-content ui-corner-all ItemGallery Carpeta Removible"
            title="<%= nombre %>" id="<%= directorioID %>">
            <input type="hidden" name="Carpeta" value="<%= carpeta %>" />
            <input type="hidden" name="DirectorioID" value="<%= directorioID %>" />
            <div style="margin: auto;">
                <div>
                    <img src="/Content/Themes/base/images/Iconos/FolderPictures.png" />
                </div>
                <div>
                    <span class="Titulo">
                        <%= nombreCorto%></span>
                </div>
            </div>
        </div>
        <%
              
            }
        %>
    </div>
    <hr />
    <div class="DivImageGalleryImages">
        <h3 style="display: none;">
            Sin im&aacute;genes para mostrar</h3>
        <%
       
            bool first = true;

            foreach (var item in Model.ObtenerArchivosPorDirectorio())
            {
                string directorio = item.Key;
                string directorioID = string.Format("Directorio_{0}", directorio.GetHashCode());

                if (first && item.Value.Count > 0)
                {
        %>

        <script type="text/javascript">
            $(document).ready(function() {
                setActiveFolder($("#<%= directorioID %>"));
            });
        </script>

        <%
            first = false;
                }


                foreach (var imagen in item.Value)
                {
                    string filePath = string.Format("{0}/{1}", directorio, imagen);
        %>
        <div class="ui-helper-reset ui-widget-content ui-corner-all ItemGallery Imagen ui-state-focus <%=directorioID %>"
            style="display: none;">
            <input type="hidden" name="ItemID" value="<%= IBVD.Digital.UI.Areas.Adoracion.Helpers.UIStringHelper.GetFileNameWithoutExtension(imagen) %>" />
            <input type="hidden" name="Carpeta" value="<%=directorioID %>" />
            
                <img src="<%= filePath %>" class="moverHand" />
            
        </div>
        <%
            }
            }
      
        %>
    </div>
    <br />
    <div id="CarpetasToDelete" class="PapeleraID" style="display: none;">
        <h3>
            Carpetas eliminadas</h3>
        <div style="max-height: 100px; overflow-y: auto;">
            <div class="Tabla">
            </div>
        </div>
    </div>
</div>
