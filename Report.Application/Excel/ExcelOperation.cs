using Report.Core.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Report.Application.Excel
{
    public static class ExcelOperation
    {
        public static byte[] Export<T>(List<T> datas, List<string> selectedProperties = null)
        {
            // Set EPPlus license context
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                // Create a worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                DataTable table = CreateTable(datas, worksheet, selectedProperties);
                worksheet.Cells["A1"].LoadFromDataTable(table, true);
                //var direct = Directory.GetCurrentDirectory();
                //var filename = "report.xlsx";
                //var directoryPath = Path.Combine(direct, filename);
                //using FileStream fstream = new FileStream(directoryPath, FileMode.Create, FileAccess.Write);
                //Save to memory stream
                using MemoryStream stream = new MemoryStream();
                package.SaveAs(stream);
                //package.SaveAs(fstream);
                byte[] streamArr = stream.ToArray();
                return streamArr;
            }
        }

        public static List<T> Import<T>(string filePath, List<string> selectedProperties = null) where T : new()
        {
            List<T> result = new List<T>();

            // Set EPPlus license context
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (ExcelPackage package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    List<PropertyInfo> choosenProperties = GetProperties<T>(selectedProperties);
                    List<PropertyInfo> properties = GetProperties<T>(null);

                    for (int row = 1; row < worksheet.Dimension.End.Row; row++)
                    {
                        T data = new T();
                        for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                        {
                            PropertyInfo property = properties[col - 1];
                            var firstCellValue = worksheet.Cells[1, col].Text;

                            if (CheckColumnAndObjectInConsistency(selectedProperties, choosenProperties, property, firstCellValue))
                                continue;

                            var cellValue = worksheet.Cells[row + 1, col].Value;
                            CellValueConverter(cellValue, property, data);
                        }
                        result.Add(data);
                    }
                }
            }

            return result;
        }

        private static DataTable CreateTable<T>(List<T> datas, ExcelWorksheet worksheet, List<string> selectedProperties)
        {
            DataTable table = new DataTable();
            List<PropertyInfo> properties = GetProperties<T>(selectedProperties);

            properties.ForEach(p =>
            {
                string columnName = p.GetCustomAttributes<DescriptionAttribute>()?.FirstOrDefault()?.Description ?? p.Name;
                table.Columns.Add(columnName, p.PropertyType);
            });

            foreach (var data in datas)
            {
                DataRow row = table.NewRow();
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    var value = properties[col].GetValue(data);
                    if (value != null)
                    {
                        row[col] = value;
                    }
                }
                table.Rows.Add(row);
            }

            return table;
        }

        private static bool CheckColumnAndObjectInConsistency(List<string> selectedProperties, List<PropertyInfo> choosenProperties,
            PropertyInfo property, string cellValue)
        {
            if (!choosenProperties.Any(x => x.Name == property.Name))
                return true;

            string columnName = property.GetCustomAttributes<DescriptionAttribute>()?
                .FirstOrDefault()?.Description ?? property.Name;

            return selectedProperties is not null && columnName != cellValue;
        }

        private static void CellValueConverter<T>(object cellValue, PropertyInfo property, T data)
        {
            if (cellValue != null)
            {
                if (property.PropertyType == typeof(int))
                    property.SetValue(data, Convert.ToInt32(cellValue));
                else if (property.PropertyType == typeof(double))
                    property.SetValue(data, Convert.ToDouble(cellValue));
                else if (property.PropertyType == typeof(DateTime))
                    property.SetValue(data, Convert.ToDateTime(cellValue));
                else if (property.PropertyType == typeof(bool))
                    property.SetValue(data, Convert.ToBoolean(cellValue));
                else
                    property.SetValue(data, cellValue.ToString());
            }
        }

        private static List<PropertyInfo> GetProperties<T>(List<string> selectedProperties)
        {
            List<PropertyInfo> properties = new List<PropertyInfo>();
            if (selectedProperties != null)
            {
                selectedProperties.ForEach(prop =>
                {
                    var property = typeof(T).GetProperty(prop) ??
                                   throw new Exception("Excel creation error");
                    properties.Add(property);
                });
            }
            else
            {
                properties = typeof(T).GetProperties().ToList();
            }
            return properties;
        }
    }
}
