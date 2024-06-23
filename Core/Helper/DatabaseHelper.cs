using System.Collections.Generic;
using Core.Interface;

namespace Core.Helper
{
    public static class DatabaseHelper
    {
        /// <summary>
        /// Speichert eine Liste von Objekten in einer Datenbank.
        /// </summary>
        /// <typeparam name="T">Der Typ der Objekte in der Liste.</typeparam>
        /// <param name="database">Die Datenbank-Instanz, die verwendet werden soll.</param>
        /// <param name="items">Die Liste von Objekten, die gespeichert werden soll.</param>
        public static void Insert<T>(IDatabase<T> database, List<T> items)
        {
            string fileName = GenerateFileName<T>();
            database.FileName = fileName;
            database.Save(items);
        }

        /// <summary>
        /// Aktualisiert eine Liste von Objekten in einer Datenbank.
        /// </summary>
        /// <typeparam name="T">Der Typ der Objekte in der Liste.</typeparam>
        /// <param name="database">Die Datenbank-Instanz, die verwendet werden soll.</param>
        /// <param name="items">Die Liste von Objekten, die aktualisiert werden soll.</param>
        public static void Update<T>(IDatabase<T> database, List<T> items)
        {
            string fileName = GenerateFileName<T>();
            database.FileName = fileName;
            foreach (var item in items)
            {
                database.Update(item);
            }
        }

        /// <summary>
        /// Löscht eine Liste von Objekten aus einer Datenbank.
        /// </summary>
        /// <typeparam name="T">Der Typ der Objekte in der Liste.</typeparam>
        /// <param name="database">Die Datenbank-Instanz, die verwendet werden soll.</param>
        public static void Delete<T>(IDatabase<T> database)
        {
            string fileName = GenerateFileName<T>();
            database.FileName = fileName;
            database.Delete(database.LastID.ToString());
        }

        /// <summary>
        /// Lädt eine Liste von Objekten aus einer Datenbank.
        /// </summary>
        /// <typeparam name="T">Der Typ der Objekte in der Liste.</typeparam>
        /// <param name="database">Die Datenbank-Instanz, die verwendet werden soll.</param>
        /// <returns>Die geladene Liste von Objekten aus der Datenbank.</returns>
        public static List<T> Load<T>(IDatabase<T> database)
        {
            string fileName = GenerateFileName<T>();
            database.FileName = fileName;
            return database.Load();
        }

        /// <summary>
        /// Generiert den Dateinamen basierend auf dem Typ T.
        /// </summary>
        /// <typeparam name="T">Der Typ, für den der Dateiname generiert werden soll.</typeparam>
        /// <returns>Der generierte Dateiname.</returns>
        private static string GenerateFileName<T>()
        {
            string typeName = typeof(T).Name;
            return $"{typeName}.wta";
        }
    }
}
