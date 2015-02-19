<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
    $(document).ready(function() {
        $("#GallerySelectorContainer").load('<%= Url.Action("GetImageSelectorControl","Cancion") %>',
        function(data) {
        });
    });
</script>

<div id="GallerySelectorContainer">
</div>
