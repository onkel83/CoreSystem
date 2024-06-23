using Core.Interface;
using Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Core.Database
{
    public class XmlDatabase<T> : BaseDatabase<T>, IDatabase<T> where T : BaseModel, new()
    {
        public XmlDatabase(string fileName, string folderName):base(fileName,folderName)
        {
            EnsureFolderExists(Path.GetDirectoryName(FullPath));
        }

        public override void Save(List<T> items)
        {
            try
            {
                EnsureFolderExists(FolderName);
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                using TextWriter writer = new StreamWriter(FullPath);
                serializer.Serialize(writer, items);
                writer.Flush();
                writer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving XML database: {ex.Message}");
                throw;
            }
        }

        public override List<T> Load()
        {
            try
            {
                if (File.Exists(FullPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                    using TextReader reader = new StreamReader(FullPath);
                    return (List<T>)serializer.Deserialize(reader);
                }
                else
                {
                    Console.WriteLine($"XML database file '{FullPath}' not found. Returning empty list.");
                    return new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading XML database: {ex.Message}");
                throw;
            }
        }
    }
}
