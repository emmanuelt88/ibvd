<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>


<link href="/Content/Themes/base/reset.css" />
<%= ContentHelper.CurrentDirectory.ThemesFiles() %>


<link href='<%=  Url.Content("~/Content/menu.css")%>' rel="stylesheet" type="text/css" />

<script src='<%= Url.Content("~/Scripts/jquery-1.6.1.min.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery-ui-1.8.14.min.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/i18n/grid.locale-es.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.jqGrid.min.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/ui.common.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.cookie.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.treeview.js") %>' type="text/javascript"></script>

<script src='<%= Url.Content("~/Scripts/ui.common.js") %>' type="text/javascript"></script>

<script src='<%= Url.Content("~/Scripts/jquery.cookie.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.treeview.js") %>' type="text/javascript"></script>

<script src='<%= Url.Content("~/Scripts/TimeEntry/jquery.timeentry.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/TimeEntry/jquery.timeentry.pack.js") %>' type="text/javascript"></script>

<script src='<%= Url.Content("~/Scripts/jquery.validate.min.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.blockUI.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.limit-1.2.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.maskedinput-1.2.2.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/i18n/grid.locale-sp.js") %>' type="text/javascript"></script>

<script src='<%= Url.Content("~/Scripts/jquery.form.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/swfobject.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.uploadify.v2.1.0.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.galleria.js") %>' type="text/javascript"></script>

<%--<script src='<%= Url.Content("~/Scripts/jquery.bgiframe.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.dimensions.js") %>' type="text/javascript"></script>
<script src='<%= Url.Content("~/Scripts/jquery.tooltip.min.js") %>' type="text/javascript"></script>
--%>
<script src='<%= Url.Content("~/Scripts/jquery.tooltip.js") %>' type="text/javascript"></script>
<%--<script src='<%= Url.Content("~/Scripts/stickytooltip.js") %>' type="text/javascript"></script>--%>


<script language="JavaScript">
    window.onerror = manejador;
    function manejador(msj, url, linea) {
        var texto = "Se ha producido el siguiente error:\n";
        texto += msj + "\n";
        texto += "(URL: " + url + ". Línea " + linea + ")";
        alert(texto);
        return true;
    }
    
</script>
<script type="text/javascript" >
    var currentTheme = '<%= ContentHelper.CurrentDirectory.CurrentTheme()%>';
    var itemsCargando = new Array();
    var allFilter = ['eq', 'ne', 'lt', 'le', 'gt', 'ge', 'bw', 'bn', 'in', 'ni', 'ew', 'en', 'cn', 'nc'];
    var textFilter = ['eq', 'ne', 'bw', 'bn', 'in', 'ni', 'ew', 'en', 'cn', 'nc'];
    var comboFilter = ['eq', 'ne'];
    var dateFilter = ['eq', 'ne', 'lt', 'le', 'gt', 'ge'];
    var numberFilter = ['eq', 'ne', 'lt', 'le', 'gt', 'ge'];
    function loadCommonEvents() {
        $().ajaxStart(function() {
            ocultarPanelErrores();
        });

        $().ajaxError(function() {
            location.href = '/Error/ErrorNotHandled';
        });

        $.ajaxSetup(
        {
            cache: false
        });
    
        $(".button,.Button, :button, :submit").button();

    }
    
    function autoCompleteMatchCheck(event, ui) {
                // provide must match checking if what is in the input
        // is in the list of results. HACK!
                var source = $(this).val();
                var found = $('.ui-autocomplete li').text().search(source);

                if (found < 0 ) {
                    $(this).val('');
                }
            }


            function autoCompleteExactMatchCheck(event, ui) {
                // provide must match checking if what is in the input
                // is in the list of results. HACK!
                var source = $(this).val();
                    var children = $('.ui-autocomplete li');

                    for (var i = 0; i < children.length; i++) {
                    if ($(children[i]).find(':hidden.value').length) {
                        if ($(children[i]).find(':hidden.value').val() == source)
                            return;
                    }
                    else {
                        if ($(children[i]).text() == source)
                            return;
                    }
                }
                    
                $(this).val('');
            }

            $(function () {
             
                $("div.OcultoInicio").fadeIn(1000);
                $("h1.OcultoInicio").fadeIn(500);
                $(".Inhabilitado").disable();


                var h = $(window).height();
                var w = $(window).width();
                $("#statusSize").html("Height:" + h + ", Width:" + w);
                loadCommonEvents();


                //                $('.button,:input[type=submit],:button, :input[type=button]').addClass('ui-state-default ui-corner-all ui-widget-content');
                //                $('.button,:input[type=submit],:button, :input[type=button]').hover(
                //					function() { $(this).addClass('ui-state-hover'); },
                //					function() { $(this).removeClass('ui-state-hover'); }
                //				);


                $('.menuItem').hover(
					function () {
					    $(this).children('.menuItem :span').removeClass('ui-icon-radio-off');
					    $(this).children('.menuItem :span').addClass('ui-icon-circle-arrow-e');
					},
					function () {
					    $(this).children('.menuItem :span').addClass('ui-icon-radio-off');
					    $(this).children('.menuItem :span').removeClass('ui-icon-circle-arrow-e');
					}
				);

                // $.datepicker.setDefaults($.extend({ showMonthAfterYear: false }, $.datepicker.regional['es']));

                //                $("#ToolTipGroup div.stickyContenedor").resizable({
                //                    minHeight: 150,
                //                    minWidth: 200
                //                });


            });

           
    function onlyInputInt(selector, max) {
        if (selector == null || $(selector) == null)
            return;
        $(selector).keypress(function(e) {

            var expression = new RegExp("[0-9]");

            var chr = String.fromCharCode(e.which);
            if (!expression.test(chr)) {
                e.preventDefault();
            }
            else {
                var result = parseInt($(selector).val() + chr)
                if (result > 2147483647) {
                    e.preventDefault();
                }
            }

            if (max != null && ($(selector).val().length + 1) > max) {
                e.preventDefault();
            }

        });

        preventWithExpression(selector, "[^0-9]");
    }

    function onlyNormalize(selector, max) {
        if (selector == null || $(selector) == null)
            return;
        $(selector).keypress(function(e) {
            if (max != null && ($(selector).val().length + 1) > max) {
                e.preventDefault();
            }

        });


        onlyValidString(selector);
    }

    function onlyValidString(el) {
        preventWithExpression(el, "['\"]");
    }
    function preventWithExpression(selector, expressionInvalidCharacters) {
        if (selector == null || $(selector) == null)
            return;

        $(selector).keypress(function(e) {
            var expression = new RegExp(expressionInvalidCharacters);

            var chr = String.fromCharCode(e.which);
            if (expression.test(chr)) {
                e.preventDefault();
            }
        });

        PreventWithExpressionCopyPaste(selector, expressionInvalidCharacters);
    }

    function PreventWithExpressionCopyPaste(elem, expressionInvalidCharacters) {
        $(elem).bind("input paste",
            function(e) {
                try {
                    var expression = new RegExp(expressionInvalidCharacters);
                    var hasInvalidCharacters = expression.test(clipboardData.getData("text"));

                    if (hasInvalidCharacters) {
                        e.preventDefault();
                    }
                }
                catch (ex) {
                    e.preventDefault();
                    $(this).val(sanitize($(this).val()));
                }
            });
    }

    function PreventCopyPaste(elem) {
        $(elem).bind("input paste",
            function(e) {
                try {
                    var ok = /^\d+$/.test(clipboardData.getData("text"));
                    if (!ok) {
                        e.preventDefault();
                    }
                }
                catch (ex) {
                    e.preventDefault();
                    $(this).val(sanitize($(this).val()));
                }
            });
    }
    function hayItemsCargando() {
        var hay = false;

        if (itemsCargando != null) {
            for (var key in itemsCargando) {
                if (itemsCargando[key] == true) {
                    hay = true;
                    break;
                }
            }
        }

        return hay;
    }

    function setItemCargando(key, estaCargado) {
        itemsCargando[key] = estaCargado;
        refrescarItemsLoading();
    }

    var itemsBlockLoading = new Array();
    function agregarItemsBlockLoading(selector) {
        itemsBlockLoading.push(selector);
    }

    function refrescarItemsLoading() {
        var disabled = '';

        if (hayItemsCargando()) {
            disabled = 'disabled';
        }

        for (var i in itemsBlockLoading) {
            $(itemsBlockLoading[i].attr('disabled', disabled));
        }

    }

    function fillEmptyFields() {
        var items = $(".formulario span");

        for (var i = 0; i < items.length; i++) {
            if (items[i].outerText == '') {
                items[i].outerText = '-';
            }
        }
    }

    var smallSpinner = $("<img src='/Content/Themes/Base/Images/ajax-loader1.gif'/>");
    var normalSpinner = $("<img src='/Content/Themes/Base/Images/ajax-loader1.gif'/>");
    function getNormalSpinner() {
        return $("<img style='width:' src='/Content/Themes/Base/Images/ajax-loader1.gif'/>");
    }

    function getSmallSpinner() {
        return $("<img style='width:16px;height:16px;margin:auto;' src='/Content/Themes/Base/Images/ajax-loader1.gif'/>");
    }

    function handleResultData(resultData, postAction) {
        if (resultData.Success) {
            if (resultData.ShowPopup) {
                mostrarNotificacion(resultData.Message, postAction);
            }
        }
        else {
            if (resultData.ShowPopup) {
                mostrarErrorPopup(resultData.Error, postAction);
            }
            else {
                mostrarErrores(resultData.Errores, resultData.Error);
            }
        }
    }
    function mostrarNotificacion(mensaje, postAction) {
        $("#popupNotificationMessage").html(mensaje);

        $("#divPopupOK").dialog({
            bgiframe: true,
            modal: true,
            width: 350,
            close: function(event, ui) { $(this).dialog('destroy'); },
            buttons: {
                Continuar: function() {
                    $(this).dialog('close');


                    try {
                        postAction();
                    }
                    catch (e) {
                    }
                }
            }
        });
    }

    function mostrarErrorPopup(mensaje, postAction) {
        $("#popupErrorMessage").html(mensaje);

        $("#divPopupError").dialog({
        bgiframe: true,
        width: 350,
            modal: true,
            close: function(event, ui) {
                $(this).dialog('destroy');
            },
            buttons: {
                Continuar: function() {
                    $(this).dialog('close');

                    try {
                        postAction();
                    }
                    catch (e) {
                    }
                }
            }
        });
    }

    function redirectTo(url) {
        document.location.href = url;
    }
    function mostrarConfirmacionPopup(mensaje, confirmarAction, cancelarAction) {
        $("#popupConfirmacionMensaje").html(mensaje);

        $("#divPopupConfirmacion").dialog({
            bgiframe: true,
            modal: true,
            width: 350,
            close: function(event, ui) {
                $(this).dialog('destroy');
            },
            buttons: {
                Cancelar: function() {
                    $(this).dialog('close');


                    try {
                        cancelarAction();
                    }
                    catch (e) {
                    }
                },
                Continuar: function() {
                    $(this).dialog('close');
                    $(this).dialog('destroy');

                    try {
                        confirmarAction();
                    }
                    catch (e) {
                    }
                }

            }
        });
    }
    function getToolTipCancion(item) {
      
        var template = $("#TemplateToolTipCancion").clone();

        template.children('div').attr('title',item.Titulo);

        template.find('.Letra').html(item.Data.Letra);

        return template.html();
    }

    function getToolTipItemLibre(item) {
        var template = $("#TemplateToolTipItemLibre").clone();

        template.children('div').attr('title', item.Titulo);
        var texto = template.find('.Detalle');

        texto.html(item.Data);

        return template.html();
    }
    function getToolTipVersiculo(item) {
        var template = $("#TemplateToolTipVersiculo").clone();

        template.children('div').attr('title', item.Titulo);
        var texto = template.find('.Versiculo');
        var templateVersiculo = $("#TemplateVersiculo").clone();
        for (var i = 0; i < item.Data.length; i++) {

            var versiculo = item.Data[i];
            templateVersiculo.find('.NroVersiculo').html(versiculo.Numero);
            templateVersiculo.find('.Texto').html(versiculo.Texto);

            texto.append(templateVersiculo.html());
        }

        return template.html();
    }

    function dateTime(el) {
        $(el).datepicker({dateFormat:'mm-dd-yyyy'});$(el).attr('readonly', 'readonly');
    }

    function getTemplateToolTipWith(html) {
        var resultado = $("#divTemplateTooltip").clone();

        resultado.show();
        resultado.append(html);

        return resultado;
    }

    function addToolTipItemLibre(id) {
        var resultado = $("#ToolTipGroup").children('div.stickyContenedor');
        var html = $("#" + id).html();
        $("#" + id).remove();
        var newItem = $('<div id="' + id + '" class="atip"/>');
        
        newItem.html(html);
        resultado.append(newItem);
    }

    function addToolTipItemWithHTML(id, html) {
        var resultado = $("#ToolTipGroup").children('div.stickyContenedor');
        var newItem = $('<div id="' + id + '" class="atip"/>');
        newItem.html(html);
        resultado.append(newItem);
    }

    function clearSticky() {
        $("#ToolTipGroup").children('div.stickyContenedor').html('');
    }

   
    $(document).mousemove(function(e) {
    $('#statusMouse').html("x:"+e.pageX + ', y:' + e.pageY);
});

$(window).resize(function() {
    var h = $(window).height();
    var w = $(window).width();
    $("#statusSize").html("Height:" + h + ", Width:" + w);
});

$(window).scroll(function() {
    var left = $(window).scrollLeft();
    var top = $(window).scrollTop();
    var h = $(window).height();
    var w = $(window).width();
    
    $("#statusScroll").html("Left:" + left + ", Top:" + top);

    $("#statusSizeFinal").html("Height:" + (h + top) + ", Width:" + (w + left));
    
});
function setTituloPagina(titulo) {
    $("#TituloPagina").html(titulo);
    document.title = titulo;
}

</script>

<div id="divPopupOK" class="PopupMensaje" style="display: none;">
    <p>
        <span class="ui-icon ui-icon-circle-check" ></span>
         <span id="popupNotificationMessage">
         </span>
     </p>
</div>
<div id="divPopupError" class="PopupMensaje" style="display: none;">
      <p>
        <span class="ui-icon ui-icon-alert" ></span>
         <span id="popupErrorMessage">
         </span>
     </p>
</div>
<div id="divPopupConfirmacion" class="PopupMensaje" style="display: none;">
    <p>
        <span class="ui-icon ui-icon-alert" ></span>
         <span id="popupConfirmacionMensaje">
         </span>
     </p>
</div>
<div id="divCargandoDivTemplate" style="display: none;">
    <div style='display: table; margin: auto;' class='divCargandoDivTemplate'>
        <div style="display: table-row;">
            <div style="display: table-cell;">
                <div class='Loader' style='margin: auto;'>
                    &nbsp;</div>
            </div>
            <div style="display: table-cell;">
                &nbsp;&nbsp;Cargando
            </div>
        </div>
    </div>
</div>
<div id="divTemplateLoading" style="display: none;">
    <div style="margin: auto;">
        <div class="Tabla" style="margin: auto;">
            <div class="Fila">
                <div class="Celda" style="vertical-align: top;">
                    <div class="Loader">
                        &nbsp;</div>
                </div>
                <div class="Celda" style="vertical-align: middle;">
                    <span class="Message"></span>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="TemplateToolTipCancion" style="display: none; padding: 0px; margin: 0px;">
        <div class="ToolTipCancion">
        <p class="Letra ui-widget" style="padding: 0px 5px 5px 5px;">
        </p>
        </div>
</div>
<div id="TemplateVersiculo" style="display:none;">
    &nbsp;<sup class="NroVersiculo" style="font-weight:bold;"></sup>&nbsp;<span class="Texto"></span>
</div>
<div id="TemplateToolTipVersiculo" style="display: none; padding: 0px; margin: 0px;">
    <div  class="ToolTipVersiculo">
        <p class="Versiculo ui-widget" style="padding: 0px 5px 5px 5px;">
        </p>
    </div>
</div>

<div id="TemplateToolTipItemLibre" style="display: none; padding: 0px; margin: 0px;">
    <div  class="ToolTipVersiculo">
        <p class="Detalle ui-widget" style="padding: 0px 5px 5px 5px;">
        </p>
    </div>
</div>




<%--
<div class='ui-widget ui-widget-content ui-state-highlight' id="divTemplateTooltip" >
</div>--%>
<div style="position:fixed; top:0px; left:0px; background-color:Black;color:#ccc;z-index:500; font-size:20px;display:none;"   >
Mouse:
<div id="statusMouse">
</div>
Size:
<div id="statusSize">
</div>

Scroll:
<div id="statusScroll">
</div>
Final:
<div id="statusSizeFinal">
</div>

TipPositions:
<div id="statusTipPositions">
</div>
TipSize:
<div id="statusTipSize">
</div>
</div>
