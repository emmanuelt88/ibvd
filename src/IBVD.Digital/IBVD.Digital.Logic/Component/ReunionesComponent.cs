using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IBVD.Digital.Logic.Entities;
using IBVD.Digital.Common.Fault;

using IBVD.Digital.Common.Notifications;
using System.IO;
using System.Xml;
using PdfSharp.Pdf;
using IBVD.Digital.Logic.Helper;
using Ionic.Zip;
using System.Net.Mail;
using System.Net.Mime;
using IBVD.Digital.Logic.Component.Mapper;
using System.Web.Hosting;
using System.Web.Mvc;
using IBVD.Digital.Common.Entities;
using IBVD.Digital.IBVD.Cache;
using System.Globalization;

namespace IBVD.Digital.Logic.Component
{
    public static class ReunionesComponent
    {
        public static Reunion GetReunion(int idReunion)
        {
            Reunion reunion = ReunionMapper.GetReunion(idReunion);

            return reunion;
        }

        public static int Create(Reunion reunion)
        {
            int idReunion = ReunionMapper.Create(reunion);

            ReunionMapper.EliminarItemsReunion(idReunion);

            foreach (ItemReunion item in reunion.ItemsReunion)
            {
                switch (item.GetTipo())
                {
                    case TipoItemReunion.PASAJE:
                        {
                            var pasaje = (Pasaje)item;
                            item.SetId(reunion.ItemsReunion.IndexOf(item));
                            BibliaMapper.CrearPasaje(pasaje, idReunion);

                        }
                        break;
                    case TipoItemReunion.ITEM_LIBRE:
                        {
                            
                            item.SetId(reunion.ItemsReunion.IndexOf(item));
                            ReunionMapper.CrearItemLibre(item as ItemLibre, idReunion);
                        }
                        break;
                    default:
                        break;
                }
              

                ReunionMapper.AgregarItemReunion(item, reunion.ItemsReunion.IndexOf(item), idReunion);
            }

            NotificarModificacionReunion(idReunion, string.Format("Se ha generado el culto para el dia {0}", reunion.FechaCulto.ToString("D", CultureInfo.CurrentCulture)));
            return idReunion;
        }

        public static void NotificarModificacionReunion(int idReunion, string subject)
        {
            Reunion reunion = ReunionMapper.GetReunion(idReunion);

            FileContentResult resultData = GenerarArchivos(reunion, "ExportarCancionesXML-ExportarReunionPDF-ExportarItemsPDF-ExportarCancionesPDF");

            var usuarios = SeguridadComponent.GetUsuariosByOperaciones(new List<string>() {Operaciones.REUNIONES_NOTIFICACIONES });
            usuarios.Add(reunion.Encargado);

            IList<string> emailsTo = usuarios.Select(m => m.Email).Distinct().ToList();
            MemoryStream stream = new MemoryStream(resultData.FileContents);
            string html = MapearTemplateHTML(reunion, subject, ConfigurationHelper.DirectorioMailTemplateModificacionReunion);

            Attachment zip = new Attachment(stream,resultData.FileDownloadName,"application/octet-stream");

            MailSender.Send(emailsTo, subject, html, true, new Attachment[] { zip });
        }

        public static void NotificarCancelacionReunion(int idReunion, string subject)
        {
            Reunion reunion = ReunionMapper.GetReunion(idReunion);

            var usuarios = SeguridadComponent.GetUsuariosByOperaciones(new List<string>() { Operaciones.REUNIONES_NOTIFICACIONES });
            usuarios.Add(reunion.Encargado);

            IList<string> emailsTo = usuarios.Select(m => m.Email).Distinct().ToList();
            string html = MapearTemplateHTML(reunion, subject, ConfigurationHelper.DirectorioMailTemplateCancelacionReunion);

            MailSender.Send(emailsTo, subject, html,true);
        }
      
        public static string MapearTemplateHTML(Reunion reunion, string titulo, string templatePath)
        {
            Dictionary<string, string> mapper = new Dictionary<string, string>();

            mapper.Add("{nroReunion}", reunion.Id.ToString());
            mapper.Add("{reunionTitulo}", reunion.Titulo);
            mapper.Add("{encargado}", string.Format("{0}", reunion.Encargado.FullNameUser));
            mapper.Add("{fechaCulto}", reunion.FechaCulto.ToString("D", CultureInfo.CurrentUICulture));
            mapper.Add("{fechaEnsayo}", reunion.FechaEnsayo.ToString("D", CultureInfo.CurrentUICulture));
            mapper.Add("{descripcion}", reunion.Descripcion);

            mapper.Add("{linkReunion}", string.Format("{0}/Reunion/Details?Reunion_IdReunion={1}", ConfigurationHelper.SiteRoot,reunion.Id));

            string template = EmailTemplateHelper.FillTemplate(titulo, templatePath, mapper);

            return template;
        }

        public static void SaveData(Reunion reunion)
        {

            ReunionMapper.Save(reunion);
            if (reunion.Estado == Reunion.EstadoENUM.CANCELADO)
            {
                NotificarCancelacionReunion(reunion.Id, string.Format("Se ha cancelado el culto para el dia {0}", reunion.FechaCulto.ToString("D", CultureInfo.CurrentCulture)));
            }
        }

        public static void Save(Reunion reunion)
        {
            SaveData(reunion);
            ReunionMapper.EliminarItemsReunion(reunion.Id);

            foreach (var item in reunion.ItemsReunion)
            {
                switch (item.GetTipo())
                {
                    case TipoItemReunion.PASAJE:
                        {
                            var pasaje = (Pasaje)item;
                            item.SetId(reunion.ItemsReunion.IndexOf(item));
                            BibliaMapper.CrearPasaje(pasaje, reunion.Id);
                            
                        }
                        break;
                    case TipoItemReunion.ITEM_LIBRE:
                        {
                            item.SetId(reunion.ItemsReunion.IndexOf(item));
                            ReunionMapper.CrearItemLibre(item as ItemLibre, reunion.Id);
                        }
                        break;
                    default:
                        break;
                }
              

                ReunionMapper.AgregarItemReunion(item, reunion.ItemsReunion.IndexOf(item), reunion.Id);

            }

            NotificarModificacionReunion(reunion.Id, string.Format("Se ha modificado el culto para el dia {0}", reunion.FechaCulto.ToString("D", CultureInfo.CurrentCulture)));

        }

        private static string GetStringBy(List<int> list)
        {
            string texto = string.Empty;

            for (int i = 0; i < list.Count; i++)
            {
                texto += list[i];

                if (i < list.Count - 1)
                    texto += ",";
            }

            return texto;
        }

        public static FileContentResult GenerarArchivos(Reunion reunion, string itemsToExport)
        {
            string fileName = string.Format("DatosIBVD_{0}.zip", DateTime.Now.ToString("dd-MM-yyyy"));
            string fileContentType = "application/octet-stream";
            var listaCanciones = reunion.ItemsReunion.Where(m => m.GetTipo() == TipoItemReunion.CANCION).Select(m => (Cancion)m).ToList();
            MemoryStream output = new MemoryStream();
            Ionic.Zip.ZipFile fileZip = new ZipFile();

            Dictionary<string, MemoryStream> files = new Dictionary<string, MemoryStream>();

            if (itemsToExport.Contains("ExportarCancionesXML"))
            {
                MemoryStream fileXmlCanciones = new MemoryStream();
                var xmlDocument = XMLGenerator.GenerarXmlCancionesExport(listaCanciones);
                xmlDocument.Save(fileXmlCanciones);
                files.Add(string.Format("ExportCanciones_{0}.xml", DateTime.Now.ToString("dd-MM-yyyy")), fileXmlCanciones);

                fileContentType = "application/xml";
            }

            if (itemsToExport.Contains("ExportarReunionPDF"))
            {
                MemoryStream filePdfReunion = new MemoryStream();
                var pdfDocument = PDFGenerator.GenerarReunion(reunion);
                pdfDocument.Save(filePdfReunion, false);
                files.Add(string.Format("Planilla_Reunion_{0}.pdf", reunion.Id), filePdfReunion);
                fileContentType = "application/pdf";
            }

            if (itemsToExport.Contains("ExportarItemsPDF"))
            {
                MemoryStream filePdfCanciones = new MemoryStream();
                var pdfCanciones = PDFGenerator.GenerarItems(reunion.ItemsReunion, true);
                pdfCanciones.Save(filePdfCanciones, false);
                files.Add(string.Format("Reunion_{0}_Items.pdf", reunion.Id), filePdfCanciones);
                fileContentType = "application/pdf";
            }

            if (itemsToExport.Contains("ExportarCancionesPDF"))
            {
                MemoryStream filePdfCanciones = new MemoryStream();
                var pdfCanciones = PDFGenerator.GenerarCanciones(listaCanciones, true);
                pdfCanciones.Save(filePdfCanciones, false);
                files.Add(string.Format("Reunion_{0}_Canciones.pdf", reunion.Id), filePdfCanciones);
                fileContentType = "application/pdf";
            }
            

            foreach (var item in files)
            {
                fileZip.AddFileStream(item.Key,string.Empty, item.Value);
                fileContentType = "application/octet-stream";
            }

            if (files.Count > 1)
            {
                fileZip.Save(output);
            }
            else
            {
                var archivo = files.First();
                output = archivo.Value;
                fileName = archivo.Key;
            }
            FileContentResult file = new FileContentResult(output.ToArray(), fileContentType);
            file.FileDownloadName = fileName;
            return file;
        }

        public static ListCollection<Reunion> GetReuniones(Query query)
        {
            ListCollection<Reunion> reuniones = new ListCollection<Reunion>();

            var list = new List<Rule>();

        

            List<string> queryText = new List<string>();
            if (query.Filters != null)
            {
                for (int i = 0; i < query.Filters.Count; i++)
                {
                    var item = query.Filters[i];
                    if (item.Field.Equals("IdEstado"))
                    {
                        var value = int.Parse(item.Value.Replace("'", string.Empty));
                        if ((Reunion.EstadoENUM)value == Reunion.EstadoENUM.FINALIZADO)
                        {
                            list.Add(item);
                            if (item.Op.Equals(Query.Comparator.EQUALS))
                            {
                                queryText.Add(string.Format(" (FechaCulto <= '{0}' AND IdEstado = {1})",
                                    DateTime.Today.ToString("yyyyMMdd"), (int)Reunion.EstadoENUM.CREADO));
                            }
                            else
                            {
                                queryText.Add(string.Format(" (FechaCulto > '{0}' AND IdEstado = {1} OR IdEstado <> {1})",
                                    DateTime.Today.ToString("yyyyMMdd"), (int)Reunion.EstadoENUM.CREADO));
                            }
                        }

                        if ((Reunion.EstadoENUM)value == Reunion.EstadoENUM.CREADO)
                        {
                            list.Add(item);
                            if (item.Op.Equals(Query.Comparator.EQUALS))
                            {
                                queryText.Add(string.Format(" (FechaCulto >= '{0}' AND IdEstado = {1})",
                                    DateTime.Today.ToString("yyyyMMdd"), (int)Reunion.EstadoENUM.CREADO));
                            }
                            else
                            {
                                queryText.Add(string.Format(" (FechaCulto < '{0}' AND IdEstado = {1} OR IdEstado <> {1})",
                                    DateTime.Today.ToString("yyyyMMdd"), (int)Reunion.EstadoENUM.CREADO));
                            }
                        }
                    }
                }
            }

            foreach (var item in list)
            {
                query.Filters.Remove(item);
            }
            string queryTextExtra = string.Empty;

            foreach (var text in queryText)
            {
                if (queryTextExtra.Equals(string.Empty))
                {
                    queryTextExtra = text;
                }
                else
                {
                    queryTextExtra += " AND " + text;
                }
            }

            query.QueryText = queryTextExtra;
            IList<Reunion> result = ReunionMapper.GetReuniones(query);
            int total = ReunionMapper.GetTotalReuniones(query);

            reuniones.AddRange(result);
            reuniones.Total = total;

            return reuniones;
        }

        public static ListCollection<ProximaFecha> GetProximasFechas(Query query)
        {
            ListCollection<ProximaFecha> proximasFechas = new ListCollection<ProximaFecha>();

            IList<ProximaFecha> result = ReunionMapper.GetProximasFechas(query);
            int total = ReunionMapper.GetTotalProximasFechas(query);

            proximasFechas.AddRange(result);
            proximasFechas.Total = total;
            
            return proximasFechas;
        }

        public static void EliminarProximaFecha(int id)
        {
            ReunionMapper.EliminarProximaFecha(id);
        }

        public static ProximaFecha GetProximaFecha(int id)
        {
            Query query = new Query("Fecha", "desc", "");
            query.Paginate = false;
            query.AddRule(new Rule("Id", Query.Comparator.EQUALS, id.ToString()));

            var item = GetProximasFechas(query).FirstOrDefault();

            return item;
        }

        public static int CrearFechaCulto(ProximaFecha proxima)
        {
            int id = ReunionMapper.CrearFechaCulto(proxima);
            return id;
        }

        public static void ActualizarFechaCulto(ProximaFecha proxima)
        {
            ReunionMapper.ActualizarFechaCulto(proxima);
        }

        public static void NotificarActualizacionFechasCulto()
        {
            var usuarios = SeguridadComponent.GetUsuariosByOperaciones(new List<string>() { Operaciones.REUNIONES_FECHAS_NOTIFICACIONES});

            var proximas = GetProximasFechas(DateTime.Now, DateTime.Now.AddMonths(2));

            if (proximas.Count == 0)
                return;

            foreach (var item in proximas.Select(m => m.Encargado).Distinct())
            {
                usuarios.Add(item);
            }

            IList<string> emailsTo = usuarios.Select(m => m.Email).Distinct().ToList();
            

            Dictionary<string, string> mapper = new Dictionary<string, string>();
            StringBuilder builder = new StringBuilder();
            foreach (var item in proximas)
            {
                mapper = new Dictionary<string, string>();
                mapper.Add("{FECHA_ITEM}", item.Fecha.ToString("dd/MM/yyyy"));
                mapper.Add("{ENCARGADO_ITEM}", item.Encargado.FullNameUser);
                mapper.Add("{TEMA_ITEM}", item.Tema);
                builder.Append(EmailTemplateHelper.FillSimpleTemplate(
                ConfigurationHelper.DirectorioMailTemplateActualizacionEncargadosCultos_ITEM, mapper));
            }

            mapper = new Dictionary<string, string>();
            mapper.Add("{linkListado}", string.Format("{0}/Reunion/ProximasFechas", ConfigurationHelper.SiteRoot));
            mapper.Add("{items}", builder.ToString());
            string template = EmailTemplateHelper.FillTemplate("Próximas fechas y encargados de cultos",
                ConfigurationHelper.DirectorioMailTemplateActualizacionEncargadosCultos, mapper);

            MailSender.Send(emailsTo, "Próximas fechas y encargados de cultos", template, true);
        }

        private static ListCollection<ProximaFecha> GetProximasFechas(DateTime fechaDesde, DateTime fechaHasta)
        {
            Query query = new Query("Fecha", "asc", "");
            query.Paginate = false;
            query.AddRule(new Rule("Fecha", Query.Comparator.GREATER_EQUAL, fechaDesde.ToString("yyyyMMdd")));
            query.AddRule(new Rule("Fecha", Query.Comparator.LESS_EQUAL, fechaHasta.AddDays(1).ToString("yyyyMMdd")));

            var resultado = GetProximasFechas(query);

            return resultado;
        }
    }
}
