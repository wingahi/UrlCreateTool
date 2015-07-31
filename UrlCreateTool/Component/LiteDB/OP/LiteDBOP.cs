using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UrlCreateTool.LiteDB.Convert;

namespace UrlCreateTool.LiteDB.OP
{
    public static class LiteDBOP
    {
        public static int Save<T>(T obj)
            where T:new()
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(DB.DB.Path()))
            {
                // Get a collection (or create, if not exits)
                var col = db.GetCollection<T>("record");
                // Insert new customer document
                var value = col.Insert(obj);
                return value.AsInt32;
            }
        }
        public static bool Update<T>(T obj)
            where T : new()
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(DB.DB.Path()))
            {
                // Get a collection (or create, if not exits)
                var col = db.GetCollection<T>("record");
                // Update a document inside a collection
                var success = col.Update(obj);
                return success;
            }
        }
        public static bool Delete(int docId)
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(DB.DB.Path()))
            {
                // Get a collection (or create, if not exits)
                var col = db.GetCollection("record");
                var success = col.Delete(docId);
                return success;
            }
        }
        public static BsonDocument FindById(int docId)
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(DB.DB.Path()))
            {
                // Get a collection (or create, if not exits)
                var col = db.GetCollection("record");
                var doc = col.FindById(docId);
                return doc;
            }
        }
        public static T FindById<T>(int docId)
            where T:new()
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(DB.DB.Path()))
            {
                // Get a collection (or create, if not exits)
                var col = db.GetCollection("record");
                var doc = col.FindById(docId);
                return BsonToObject.ConvertTo<T>(doc);
            }
        }
        public static IList<BsonDocument> FindAll()
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(DB.DB.Path()))
            {
                // Get a collection (or create, if not exits)
                var col = db.GetCollection("record");
                var doc = col.FindAll().ToList();
                return doc;
            }
        }
        public static IList<T> FindAll<T>()
            where T:new()
        {
            // Open data file (or create if not exits)
            using (var db = new LiteDatabase(DB.DB.Path()))
            {
                // Get a collection (or create, if not exits)
                var col = db.GetCollection<T>("record");
                var docs = col.FindAll();
                return docs.ToList();
            }
        }
    }
}
