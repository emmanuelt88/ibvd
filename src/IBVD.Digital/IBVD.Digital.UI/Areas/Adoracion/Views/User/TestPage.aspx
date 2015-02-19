<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <% Html.RenderPartial("~/Views/Upload/UploadControl.ascx", new Dictionary<string, string>()
                   {
                       {"FormId","FormPrueba1"},
                       {"ContentTypes","JPG,PNG,GIF"},
                       {"FunctionLoadComplete","loadComplete"},
                       {"TextButton","Guardar imagen"}
                       
                   }); %>
                   

                   
                      <script type="text/javascript">
                          $("#pepe").dialog();
                          function loadComplete(data) {

                          }
                   </script>

</asp:Content>
