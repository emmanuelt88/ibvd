<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IBVD.Digital.UI.Models.UsuariosUCViewData>" %>
 <%
        string txtDisplayName = Model.Id + "DisplayName";
        string txtUserNameValue = Model.Id + "UserName";
        string imgUserCombo = Model.Id + "Img";
        string imgSrc = "/Images/PerfilHombreMujer.jpg";
        string imgSrcDefault = "/Images/PerfilHombreMujer.jpg";
        string fullNameSelected = string.Empty;
        StringBuilder builder = new StringBuilder();

        if (!string.IsNullOrEmpty(Model.Value))
        {
            var user = Model.Usuarios.FirstOrDefault(m => m.UserName.Equals(Model.Value));
            imgSrc = user.UserFotoURL;
        }
        builder.Append("[");

        foreach (var usuario in Model.Usuarios)
        {
            builder.Append("{");
            builder.AppendFormat("label:'{1} | <b>{0}</b>', value:'{0}', fullName:'{1}', imgSrc:'{2}'", usuario.UserName, usuario.FullNameOrUser, usuario.UserFotoURL);
            builder.Append("},");

            if (usuario.UserName.Equals(Model.Value))
            {
                fullNameSelected = usuario.FullNameOrUser;
                imgSrc = usuario.UserFotoURL;
            }
        }

        builder.Append("]");

        string jsonData = builder.ToString().Replace("},]", "}]");
    
    %>
    
<script type="text/javascript">
    $(document).ready(function() {
        var usuarios = <%= jsonData%>;
        var imgSrcDefault = '<%= imgSrcDefault %>';
        var imgSrc = '<%= imgSrc %>';
        $('#<%= txtDisplayName %>').autocomplete({
			minLength: 0,
			delay: 0,
			source: usuarios,
			focus: function(event, ui) {
				$('#<%= txtDisplayName %>').val(ui.item.fullName);
				return false;
			},
			select: function(event, ui) {
				$('#<%= txtUserNameValue %>').val(ui.item.value);
		
				$('#<%= imgUserCombo %>').attr('src',ui.item.imgSrc);
				return false;
			},
			change: autoCompleteMatchCheck,
			close: autoCompleteExactMatchCheck
		})
		.data( "autocomplete" )._renderItem = function( ul, item ) {
			return $( "<li></li>" )
				.data( "item.autocomplete", item )
				.append( "<a><div style='text-align:left;'>"+ item.label + "<input type='hidden' class='value' value='"+ item.fullName +"'/>" + "</b></div></a>" )
				.appendTo( ul );
		};
		
		$('#<%= txtDisplayName %>').change(function(){
		    if($(this).val() == '' || $(this).val() == 'Ingrese el usuario'){
		        $('#<%=  txtUserNameValue %>').val('');
		        $('#<%= imgUserCombo %>').attr('src',imgSrcDefault);
		    }
		 
		    
		});
		
		 
		 <%
		 if(!string.IsNullOrEmpty(fullNameSelected)){
		 %>
		 $('#<%= txtDisplayName %>').val('<%=fullNameSelected %>');
		 $('#<%=  txtUserNameValue %>').val('<%= Model.Value %>');
		 <%
		 }
		 else{
		 %>
		 inputText("Ingrese el usuario", "<%= txtDisplayName %>");
		 <%
		} 
		 %>
		
		$('#<%= imgUserCombo %>').attr('src',imgSrc);
		
    });
</script>
<table>
    <tr>
        <td valign="top" style="height:40px;" >
<img  alt="Imagen de perfil" class='Perfil Miniatura' id="<%= imgUserCombo %>" style="margin:0px;padding:0px; margin-bottom:-8px;"  />
        
        </td>
        <td valign="top">
        <input type="text" id="<%= txtDisplayName %>"  size="35" value="<%= Model.DisplayValue %>" style="margin:0px;padding:0px; height:16px;"  />
<input type="hidden" id="<%=  txtUserNameValue %>"  name="<%= Model.Name %>"  value="<%= Model.Value %>" //>
        </td>
        <%
            if (Model.Obligatorio)
            {
                %>
                <td valign="top">
                <em>*</em>
                </td>
                <%
            }
         %>
    </tr>
</table>




