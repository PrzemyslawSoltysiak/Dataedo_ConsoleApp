﻿using Dataedo_RazorPages.Models;

namespace Dataedo_RazorPages.Data
{
    public class DataReader
    {
        public IEnumerable<ImportedObject> ImportedObjects;

        public void ImportAndPrintData(string fileToImport, bool printData = true)
        {
            ImportedObjects = new List<ImportedObject>();

            var streamReader = new StreamReader(fileToImport);

            var importedLines = new List<string>();
            while (!streamReader.EndOfStream)
            {
                var line = streamReader.ReadLine();
                importedLines.Add(line);
            }

            for (int i = 1; i < importedLines.Count; i++)
            {
                var importedLine = importedLines[i];
                var values = importedLine.Split(';');
                var importedObject = new ImportedObject();
                importedObject.Type = values.ElementAtOrDefault(0) ?? string.Empty;
                importedObject.Name = values.ElementAtOrDefault(1) ?? string.Empty;
                importedObject.Schema = values.ElementAtOrDefault(2) ?? string.Empty;
                importedObject.ParentName = values.ElementAtOrDefault(3) ?? string.Empty;
                importedObject.ParentType = values.ElementAtOrDefault(4) ?? string.Empty;
                importedObject.DataType = values.ElementAtOrDefault(5) ?? string.Empty;
                importedObject.IsNullable = values.ElementAtOrDefault(6) == "1" ? true : false;
                ((List<ImportedObject>)ImportedObjects).Add(importedObject);
            }

            var dataTypeCleaner = new DataTypeCleaner("Data/datatypes.csv");     // for extra clearing of DataType property
            // clear and correct imported data
            foreach (var importedObject in ImportedObjects)
            {
                importedObject.Type = importedObject.Type.Trim().Replace(" ", "").Replace(Environment.NewLine, "").ToUpper();
                importedObject.Name = importedObject.Name.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.Schema = importedObject.Schema.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.ParentName = importedObject.ParentName.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.ParentType = importedObject.ParentType.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                importedObject.DataType = importedObject.DataType.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
                // extra clearing of DataType property
                importedObject.DataType = dataTypeCleaner.ExtraCleaning(importedObject.DataType);
            }

            // assign number of children
            for (int i = 0; i < ImportedObjects.Count(); i++)
            {
                var importedObject = ImportedObjects.ToArray()[i];
                foreach (var impObj in ImportedObjects)
                {
                    if (impObj.ParentType == importedObject.Type)
                    {
                        if (impObj.ParentName == importedObject.Name)
                        {
                            importedObject.NumberOfChildren = 1 + importedObject.NumberOfChildren;
                        }
                    }
                }
            }

            if (printData)
            {
                foreach (var database in ImportedObjects)
                {
                    if (database.Type == "DATABASE")
                    {
                        Console.WriteLine($"Database '{database.Name}' ({database.NumberOfChildren} tables)");

                        // print all database's tables
                        foreach (var table in ImportedObjects)
                        {
                            if (table.ParentType.ToUpper() == database.Type)
                            {
                                if (table.ParentName == database.Name)
                                {
                                    Console.WriteLine($"\tTable '{table.Schema}.{table.Name}' ({table.NumberOfChildren} columns)");

                                    // print all table's columns
                                    foreach (var column in ImportedObjects)
                                    {
                                        if (column.ParentType.ToUpper() == table.Type)
                                        {
                                            if (column.ParentName == table.Name)
                                            {
                                                Console.WriteLine($"\t\tColumn '{column.Name}' with {column.DataType} data type {(column.IsNullable ? "accepts nulls" : "with no nulls")}");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                Console.ReadLine();
            }
        }
    }
}