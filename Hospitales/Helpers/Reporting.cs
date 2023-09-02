using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using OfficeOpenXml;
using Syncfusion.DocIO.DLS;
using cm = System.ComponentModel;

namespace Hospitales.Helpers
{
    public class Reporting : IReporting
    {
        public byte[] Excel<T>(List<T> list, string[] nombrePropiedades)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage ep = new ExcelPackage())
                {
                    ep.Workbook.Worksheets.Add("Hoja1");
                    ExcelWorksheet ews = ep.Workbook.Worksheets[0];

                    if (nombrePropiedades != null && nombrePropiedades.Length > 0)
                    {
                        Dictionary<string, string> cabeceras = cm.TypeDescriptor.GetProperties(typeof(T)).Cast<cm.PropertyDescriptor>().ToDictionary(p => p.Name, p => p.DisplayName);

                        for (int i = 0; i < nombrePropiedades.Length; i++)
                        {
                            ews.Cells[1, i + 1].Value = cabeceras[nombrePropiedades[i]];
                            ews.Column(i + 1).Width = 50;
                        }

                        int fila = 2;
                        int col = 1;

                        if (list != null)
                        {
                            foreach (var item in list)
                            {
                                col = 1;

                                foreach (var prop in nombrePropiedades)
                                {
                                    ews.Cells[fila, col].Value = item.GetType().GetProperty(prop).GetValue(item).ToString();
                                    col++;
                                }

                                fila++;
                            }
                        }

                    }

                    ep.SaveAs(ms);

                    byte[] buffer = ms.ToArray();

                    return buffer;
                }
            }
        }

        public byte[] Pdf<T>(string titulo, string[] nombrePropiedades, List<T> list)
        {
            Dictionary<string, string> cabeceras = cm.TypeDescriptor.GetProperties(typeof(T)).Cast<cm.PropertyDescriptor>().ToDictionary(p => p.Name, p => p.DisplayName);

            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);

                using (var pdfDoc = new PdfDocument(writer))
                {
                    Document doc = new Document(pdfDoc);

                    //Titulo
                    Paragraph c1 = new Paragraph(titulo);
                    c1.SetFontSize(20);
                    c1.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                    doc.Add(c1);

                    if (nombrePropiedades != null && nombrePropiedades.Length > 0)
                    {
                        //Tabla
                        Table table = new Table(nombrePropiedades.Length);
                        Cell celda;
                        for (int i = 0; i < nombrePropiedades.Length; i++)
                        {
                            celda = new Cell();
                            celda.Add(new Paragraph(cabeceras[nombrePropiedades[i]]));
                            celda.SetFontSize(12);
                            table.AddHeaderCell(celda);
                        }

                        if (list != null)
                        {
                            foreach (object item in list)
                            {
                                foreach (string prop in nombrePropiedades)
                                {
                                    celda = new Cell();
                                    celda.Add(new Paragraph(item.GetType().GetProperty(prop).GetValue(item).ToString()));
                                    celda.SetFontSize(10);
                                    table.AddCell(celda);
                                }
                            }
                        }

                        doc.Add(table);
                    }

                    doc.Close();
                    writer.Close();
                }

                return ms.ToArray();
            }
        }

        public byte[] Word<T>(string titulo, string[] nombrePropiedades, List<T> list)
        {
            using (var ms = new MemoryStream())
            {
                WordDocument doc = new WordDocument();
                WSection section = doc.AddSection() as WSection;
                section.PageSetup.Margins.All = 72;
                section.PageSetup.PageSize = new Syncfusion.Drawing.SizeF(612, 792);

                //Título
                IWParagraph paragraph = section.AddParagraph();
                paragraph.ApplyStyle("Normal");
                paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                WTextRange textTitulo = paragraph.AppendText(titulo) as WTextRange;
                textTitulo.CharacterFormat.FontSize = 20f;
                textTitulo.CharacterFormat.FontName = "Calibri";
                textTitulo.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Red;

                if (nombrePropiedades != null && nombrePropiedades.Length > 0)
                {
                    //Tabla
                    IWTable table = section.AddTable() as IWTable;
                    int numCol = nombrePropiedades.Length;
                    int numFilas = list.Count;
                    Dictionary<string, string> cabeceras = cm.TypeDescriptor.GetProperties(typeof(T)).Cast<cm.PropertyDescriptor>().ToDictionary(p => p.Name, p => p.DisplayName);
                    table.ResetCells(numFilas + 1, numCol);

                    for (int i = 0; i < numCol; i++)
                    {
                        table[0, i].AddParagraph().AppendText(cabeceras[nombrePropiedades[i]]);
                    }

                    if (list != null)
                    {
                        int fila = 1;
                        int col = 0;
                        foreach (var item in list)
                        {
                            col = 0;
                            foreach (var prop in nombrePropiedades)
                            {
                                table[fila, col].AddParagraph().AppendText(item.GetType().GetProperty(prop).GetValue(item).ToString());
                                col++;
                            }
                            fila++;
                        }
                    }

                }

                doc.Save(ms, Syncfusion.DocIO.FormatType.Docx);

                return ms.ToArray();
            }
        }
    }
}
