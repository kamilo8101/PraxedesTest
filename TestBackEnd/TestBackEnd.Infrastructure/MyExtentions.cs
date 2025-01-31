using ClosedXML.Excel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TestBackEnd.Infrastructure
{
    /// <summary>
    /// Clase encargada de aportar nuevas extensiones 
    /// </summary>
    public static class MyExtentions
    {

        /// <summary>
        /// obtienes el name de un enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttributes(typeof(DisplayAttribute), false)
                                 .Cast<DisplayAttribute>()
                                 .FirstOrDefault();

            return attribute?.Name ?? value.ToString();
        }

        private static string NextColumn(int colindex)
        {
            //65 to 90
            string letters = string.Empty;
            int colnumber = colindex;

            while (colnumber > 0)
            {
                var curNumber = (colnumber - 1) % 26;
                var curLetter = Convert.ToChar(curNumber + 65);
                letters = curLetter + letters;
                colnumber = (colnumber - (curNumber + 1)) / 26;
            }
            return letters;
        }

        /// <summary>
        /// Convertir Lista a libro de excel
        /// </summary>
        /// <typeparam name="T">Tipo de la lista</typeparam>
        /// <param name="collection">Lista de elementos</param>
        /// <returns>XLWorkbook con una hoja con los items de la lista</returns>
        public static XLWorkbook ToBook<T>(this IEnumerable<T> collection)
        {
            return collection.ToBook(null);
        }

        /// <summary>
        /// Convertir Lista a libro de excel
        /// </summary>
        /// <typeparam name="T">Tipo de la lista</typeparam>
        /// <param name="collection">Lista de elementos</param>
        /// <param name="customHeaders">Nombre de las columnas</param>
        /// <returns>XLWorkbook con una hoja con los items de la lista</returns>
        /// 
        public static XLWorkbook ToBook<T>(this IEnumerable<T> collection, List<ExcelCell> customHeaders)
        {
            try
            {
                XLWorkbook workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Report");

                int colindex = 1; //Indice de la columna
                int rowindex = 1; //Indice de la fila
                string cell; //Combinacion de la letra y número de la celda
                string letter; //Letra de la celda

                //Lista de headers que se escriben en el archivo
                List<ExcelCell> headers = new List<ExcelCell>();
                //Recorre nombres de columnas de la colección de datos
                foreach (var item in collection.FirstOrDefault().GetType().GetProperties().Select(x => x.Name))
                {
                    if (customHeaders != null && customHeaders.Count > 0)
                    {
                        var head = customHeaders.Where(x => x.ColName.Equals(item)).FirstOrDefault();
                        if (head != null) headers.Add(head);
                        else headers.Add(new ExcelCell(item));
                    }
                    else
                        headers.Add(new ExcelCell(item));
                }
                //Escribir los headers
                foreach (var item in headers)
                {
                    letter = NextColumn(colindex);
                    cell = $"{letter}{rowindex}";
                    worksheet.Cell(cell).Style.Font.Bold = true;
                    worksheet.Cell(cell).Value = item.Header;
                    colindex++;
                }
                //Reiniciar los indices
                rowindex = 2;
                colindex = 1;

                //Escribir los datos de las filas
                foreach (dynamic item in collection)
                {
                    foreach (var col in headers)
                    {
                        letter = NextColumn(colindex); //Obtener la siguiente letra
                        cell = $"{letter}{rowindex}"; //Asignar la celda
                        var value = item.GetType().GetProperty(col.ColName).GetValue(item, null);//Obtener el valor del item
                        worksheet.Cell(cell).Value = value; //Escribir el valor en la celda
                        if (col.Style != null)// Aplicar el stilo
                        {
                            worksheet.Cell(cell).Style.NumberFormat.Format = col.Style.Format;
                        }
                        colindex++; //Aumentar index para obtener la siguiente letra
                    }
                    rowindex++;
                    colindex = 1;
                }
                return workbook;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Metodo para remover caracteres especificados en una cadena de texto y signos de mayor que  y menor que
        /// </summary>
        /// <param name="cadena">Cadena de texto</param>
        /// <param name="busquedad">Caracteres a remover</param>
        public static string Sanitize(this string cadena, string busquedad)
        {
            try
            {
                return Regex.Replace(cadena, "[<>" + busquedad + "]", string.Empty, RegexOptions.IgnoreCase);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Lista de Elementos</param>
        /// <param name="customHeaders">Nombre de las Columnas</param>
        /// <param name="workSheetName">Nombre de la Hoja</param>
        /// <returns></returns>
        public static IXLWorksheet ToWorksheet<T>(this IEnumerable<T> collection, List<ExcelCell> customHeaders, string workSheetName, int colIni = 1, int rowIni = 1)
        {
            try
            {
                XLWorkbook workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(workSheetName);

                int colindex = colIni; //Indice de la columna
                int rowindex = rowIni; //Indice de la fila
                string cell; //Combinacion de la letra y número de la celda
                string letter; //Letra de la celda

                //Lista de headers que se escriben en el archivo
                List<ExcelCell> headers = new List<ExcelCell>();
                //Recorre nombres de columnas de la colección de datos
                foreach (var item in collection.FirstOrDefault().GetType().GetProperties().Select(x => x.Name))
                {
                    if (customHeaders != null && customHeaders.Count > 0)
                    {
                        var head = customHeaders.Where(x => x.ColName.Equals(item)).FirstOrDefault();
                        if (head != null) headers.Add(head);
                        //else headers.Add(new ExcelCell(item));
                    }
                    else
                        headers.Add(new ExcelCell(item));
                }
                //Escribir los headers
                foreach (var item in headers)
                {
                    letter = NextColumn(colindex);
                    cell = $"{letter}{rowindex}";
                    worksheet.Cell(cell).Style.Font.Bold = true;
                    worksheet.Cell(cell).Value = item.Header;
                    colindex++;
                }
                //Reiniciar los indices
                rowindex = rowindex + 1;
                colindex = colIni;

                //Escribir los datos de las filas
                foreach (dynamic item in collection)
                {
                    foreach (var col in headers)
                    {
                        letter = NextColumn(colindex); //Obtener la siguiente letra
                        cell = $"{letter}{rowindex}"; //Asignar la celda
                        var value = item.GetType().GetProperty(col.ColName).GetValue(item, null);//Obtener el valor del item
                        worksheet.Cell(cell).Value = value; //Escribir el valor en la celda
                        if (col.Style != null)// Aplicar el stilo
                        {
                            worksheet.Cell(cell).Style.NumberFormat.Format = col.Style.Format;
                        }
                        colindex++; //Aumentar index para obtener la siguiente letra
                    }
                    rowindex++;
                    colindex = 1;
                }
                return worksheet;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo para dividir lista en sublistas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Lista de Objetos</param>
        /// <param name="indice">Indice por el que se va a dividir la lista</param>
        public static List<List<T>> SplitList<T>(List<T> source, int indice)
        {
            return source
                 .Select((x, i) => new { Index = i, Value = x })
                 .GroupBy(x => x.Index / indice)
                 .Select(x => x.Select(v => v.Value).ToList())
                 .ToList();
        }

    }
}