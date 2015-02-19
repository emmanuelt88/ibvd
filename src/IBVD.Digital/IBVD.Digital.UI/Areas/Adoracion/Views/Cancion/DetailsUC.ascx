<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IBVD.Digital.UI.Models.CancionViewData>" %>
<%= Html.Hidden("IdCancion", Model.Cancion.Id) %>
    <fieldset class="formulario">
        <legend>Datos de la canci&oacute;n</legend>
        <p>
            <label>
                T&iacute;tulo:</label>
            <b><span style="text-decoration: underline;">
                <%= Model.Cancion.Titulo%></span></b>
        </p>
         <p>
            <label>
                Duraci&oacute;n estimada</label>
            <span>
                <%= Model.Cancion.DuracionEstimada%></span>&nbsp;<i>Minutos</i>
        </p>
        <p>
            <label>
                Comp&aacute;s:</label>
            <span>
                <%= string.IsNullOrEmpty(Model.Cancion.Compas)?"Sin seleccionar": Model.Cancion.Compas%></span>
        </p>
        <p>
            <label>
                Tono:</label>
            <span>
                <%= string.IsNullOrEmpty(Model.Cancion.Tono) ? "Sin seleccionar" : Model.Cancion.Tono%></span>
        </p>
        <p>
            <label>
                Fondo de la canci&oacute;n:</label>
            <div style="display: table">
                <div style="display: table-row">
                    <div style="display: table-cell; vertical-align: top; width: 100px; margin-top: 10px;
                        padding-right: 30px; text-align: center;">
                        <img class="Perfil" alt="Fondo de la canción" id="userPerfilImage" width="100" src="<%= Model.CancionPath  %>" />
                    </div>
                    <div style="display: table-cell; vertical-align: top;">
                    </div>
                </div>
            </div>
            <input type="hidden" name="Cancion_FotoURI" id="UsuarioEdit_FotoGUID" value="<%= Model.Cancion.FotoURI %>" />

            <script type="text/javascript">
                function seleccionarImagen() {

                }
            </script>

        </p>
        <p>
            <label>
                Letra:</label>
            <textarea cols="83" rows="20" style="width: 80%;" readonly="readonly"><%= Model.Cancion.Letra%></textarea>
        </p>
    </fieldset>
  

    <script type="text/javascript">
        $(document).ready(function() {
        
            fillEmptyFields();
        });
    </script>