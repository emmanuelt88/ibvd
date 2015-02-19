<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IBVD.Digital.UI.Models.DirectorioViewData>" %>
<%
    if (Model.Imagenes.Count == 0)
    {
      
%>
<input type="hidden" id="DirectorioImagenesVacio" value="true" />
<h2 style='text-align: center;'>
    Directorio sin im&aacute;genes cargadas</h2>
<%
        
    }
    else
    {
%>
<link rel="stylesheet" href="/Content/Themes/base/galleriffic/galleriffic-4.css"
    type="text/css" />

<script type="text/javascript" src="/Scripts/galleriffic/jquery.history.js"></script>

<script type="text/javascript" src="/Scripts/galleriffic/jquery.galleriffic.js"></script>

<script type="text/javascript" src="/Scripts/galleriffic/jquery.opacityrollover.js"></script>

<!-- We only want the thunbnails to display when javascript is disabled -->

<script type="text/javascript">
    document.write('<style>.noscript { display: none; }</style>');
		</script>

<div class="ui-widget" style="margin-bottom: 5px;">
    Mostrar:
    <select id="ddlDirectorios">
        <option value="ImagenItem">Todas las imagenes</option>
        <option value="<%= "Class" + Model.Root.GetHashCode() %>">Raiz</option>
        <%
            string carpetaActual = (string)ViewData["CarpetaActual"];

            foreach (string carpeta in Model.Carpetas)
            {
                var aux = carpeta.Replace("/", "_");
                var items = aux.Split('_');
                string text = items[items.Length - 1];

                string selected = !string.IsNullOrEmpty(carpetaActual)
                                    && ViewData["CarpetaActual"].ToString().Equals(carpeta)
                                    ? "selected = 'selected'" : string.Empty;

                if (!text.Equals("Raiz"))
                {
        %>
        <option value="<%= carpeta %>" <%= selected %>>
            <%= text%></option>
        <%
            }
            }
        %>
    </select>
</div>
<div id="container">
    <!-- Start Advanced Gallery Html Containers -->
    <div id="gallery" class="content">
        <div id="controls" class="controls">
        </div>
        <div class="slideshow-container">
            <div id="loading" class="loader">
            </div>
            <div id="slideshow" class="slideshow">
            </div>
            <div id="caption" class="caption-container">
            </div>
        </div>
        <div id="captionToggle">
            <a href="#toggleCaption" class="off" title="Show Caption">Mostrar detalle</a>
        </div>
    </div>
    <div id="thumbs" class="navigation">
        <ul class="thumbs noscript">
            <%
                bool vacio = true;
                foreach (var item in Model.ObtenerArchivosPorDirectorio())
                {
                    string className = "Class" + item.Key.GetHashCode();

                    if (!string.IsNullOrEmpty(carpetaActual) && !item.Key.Equals(carpetaActual))
                        continue;

                    foreach (var miniatura in item.Value)
                    {
                        vacio = false;
                        string imagenGrande = miniatura.Replace(IBVD.Digital.Logic.Helper.ConfigurationHelper.ImagenMiniatura, string.Empty);
                        string name = string.Format("Name_{0}", Guid.NewGuid());
                                    
            %>
            <li class="<%= className %>"><a name="<%= name %>" class="thumb" href="<%= item.Key + "/" +  imagenGrande %>"
                title="">
                <img src="<%= item.Key + "/" +  miniatura %>" alt="Title #23" width="70px" height="50px" />
            </a>
                <div class="caption">
                    <div class="image-title">
                    </div>
                    <div class="image-desc">
                    </div>
                    <div class="download">
                        <a href="<%= item.Key + "/" +  imagenGrande %>">Descarga</a>
                    </div>
                </div>
            </li>
            <%
                                    
                }
                }
                            %>
        </ul>
    </div>
    <div style="clear: both;">
    </div>
</div>
<%
    if (vacio)
    {
%>
<h2 style='text-align: center;'>
    Carpeta sin im&aacute;genes cargadas</h2>
<%
    }
%>

<script type="text/javascript">
    var gallery = null;
    jQuery(document).ready(function($) {
            <%
                if(!vacio){
                %>
                    inicializarGaleria();
                <%
                }
             %>
    });
    
    function inicializarGaleria() {
      
        // We only want these styles applied when javascript is enabled
        $('div.navigation').css({ 'width': '300px', 'float': 'left' });
        $('div.content').css('display', 'block');

        // Initially set opacity on thumbs and add
        // additional styling for hover effect on thumbs
        var onMouseOutOpacity = 0.67;
        $('#thumbs ul.thumbs li').opacityrollover({
            mouseOutOpacity: onMouseOutOpacity,
            mouseOverOpacity: 1.0,
            fadeSpeed: 'fast',
            exemptionSelector: '.selected'
        });

        // Enable toggling of the caption
        var captionOpacity = 0.0;
        $('#captionToggle a').click(function(e) {
            var link = $(this);
            var isOff = link.hasClass('off');
            var removeClass = isOff ? 'off' : 'on';
            var addClass = isOff ? 'on' : 'off';
            var linkText = isOff ? 'Mostrar detalle' : 'Ocultar detalle';
            captionOpacity = isOff ? 0.7 : 0.0;

            link.removeClass(removeClass).addClass(addClass).text(linkText).attr('title', linkText);
            $('#caption span.image-caption').fadeTo(1000, captionOpacity);

            e.preventDefault();
        });

        // Initialize Advanced Galleriffic Gallery
        gallery = $('#thumbs').galleriffic({
            delay: 2500,
            numThumbs: 15,
            preloadAhead: 10,
            enableTopPager: true,
            enableBottomPager: true,
            maxPagesToShow: 7,
            imageContainerSel: '#slideshow',
            controlsContainerSel: '#controls',
            captionContainerSel: '#caption',
            loadingContainerSel: '#loading',
            renderSSControls: true,
            renderNavControls: true,
            playLinkText: 'Reproducir',
            pauseLinkText: 'Parar',
            prevLinkText: '&lsaquo; Anterior',
            nextLinkText: 'Siguiente &rsaquo;',
            nextPageLinkText: '&rsaquo;&rsaquo;',
            prevPageLinkText: '&lsaquo;&lsaquo;',
            enableHistory: true,
            autoStart: false,
            syncTransitions: true,
            defaultTransitionDuration: 900,
            onSlideChange: function(prevIndex, nextIndex) {
                // 'this' refers to the gallery, which is an extension of $('#thumbs')
                this.find('ul.thumbs').children()
							.eq(prevIndex).fadeTo('fast', onMouseOutOpacity).end()
							.eq(nextIndex).fadeTo('fast', 1.0);
            },
            onTransitionOut: function(slide, caption, isSync, callback) {
                slide.fadeTo(this.getDefaultTransitionDuration(isSync), 0.0, callback);
                caption.fadeTo(this.getDefaultTransitionDuration(isSync), 0.0);
            },
            onTransitionIn: function(slide, caption, isSync) {
                var duration = this.getDefaultTransitionDuration(isSync);
                slide.fadeTo(duration, 1.0);

                // Position the caption at the bottom of the image and set its opacity
                var slideImage = slide.find('img');
                caption.width(slideImage.width())
							.css({
							    'bottom': Math.floor((slide.height() - slideImage.outerHeight()) / 2),
							    'left': Math.floor((slide.width() - slideImage.width()) / 2) + slideImage.outerWidth() - slideImage.width()
							})
							.fadeTo(duration, captionOpacity);
            },
            onPageTransitionOut: function(callback) {
                this.fadeTo('fast', 0.0, callback);
            },
            onPageTransitionIn: function() {
                this.fadeTo('fast', 1.0);
            },
            onImageAdded: function(imageData, $li) {
                $li.opacityrollover({
                    mouseOutOpacity: onMouseOutOpacity,
                    mouseOverOpacity: 1.0,
                    fadeSpeed: 'fast',
                    exemptionSelector: '.selected'
                });
            }
        });

        /**** Functions to support integration of galleriffic with the jquery.history plugin ****/

        // PageLoad function
        // This function is called when:
        // 1. after calling $.historyInit();
        // 2. after calling $.historyLoad();
        // 3. after pushing "Go Back" button of a browser
        function pageload(hash) {
            // alert("pageload: " + hash);
            // hash doesn't contain the first # character.
            if (hash) {
                $.galleriffic.gotoImage(hash);
            } else {
                gallery.gotoIndex(0);
            }
        }

        // Initialize history plugin.
        // The callback is called at once by present location.hash. 
        $.historyInit(pageload, "advanced.html");

        // set onlick event for buttons using the jQuery 1.3 live method
        $("a[rel='history']").live('click', function(e) {
            if (e.button != 0) return true;

            var hash = this.href;
            hash = hash.replace(/^.*#/, '');

            // moves to a new page. 
            // pageload is called at once. 
            // hash don't contain "#", "?"
            $.historyLoad(hash);

            return false;
        });



        /****************************************************************************************/
    }
    
       function setTextoCancion(texto) {
//        while (texto.search('\n') > 0) {
//            texto = texto.replace('\n', '<br/>');
//        }
//        $("#cancionImagenTexto").html(texto);
//        $("#divCancionContainer").css('overflow', 'auto');
    }
		</script>

<script type="text/javascript">
    $(function() {
        $("#ddlDirectorios").change(function() {
            var folder = $(this).val();
            //        inicializarGaleria();
            recargarGaleria(folder);
        });
    });

    function recargarGaleria(carpeta) {
        blockDIV("#GaleriaContenedor");
        $("#GaleriaContenedor").load('/Directorio/GetGalleryViewerUC', {
            carpeta: carpeta
        });
    }
</script>

<%
    }
%>
