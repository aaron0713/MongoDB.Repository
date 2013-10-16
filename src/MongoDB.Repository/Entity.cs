﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace MongoDB.Repository
{
    public abstract class Entity : IEntity
    {
        [BsonId]
        public string Id
        {
            get
            {
                if (_id == ObjectId.Empty)
                    _id = ObjectId.GenerateNewId(DateTime.Now);
                return _id.ToString();
            }
            set
            {
                ObjectId.TryParse(value, out __id);
                if (__id != ObjectId.Empty)
                    _id = __id; ;
            }
        }
        private ObjectId _id;
        private ObjectId __id;

        /// <summary>
        /// save document
        /// </summary>
        public void Save()
        {
            //this.DBSave();
            EntityOperationExtensions.DBSave(this.GetType(), this);
        }
        /// <summary>
        /// remove document
        /// </summary>
        public void Remove()
        {
            this.DBRemove();
        }

        private static bool IsTypeCanBeUsed(Type type)
        {
            if (type.IsAbstract || type.IsInterface || type.IsEnum)
                throw new MongoException(string.Format("Type of {0} can not be used, because of it is not IEntity", type.FullName));
            if (!type.GetConstructors().Any(c => c.GetParameters().Length == 0))
                throw new MongoException(string.Format("Type of {0} can not be used, because of it has not less parameters constructor", type.FullName));
            return true;
        }

        public static T Get<T>(string id) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBFind<T>(id);
        }

        public static T Get<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBFind<T>(where);
        }
        public static bool Remove<T>(string id) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBRemove<T>(id);
        }
        public static long RemoveAll<T>() where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBRemoveAll<T>();
        }
        public static long RemoveAll<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBRemoveAll<T>(where);
        }
        public static IQueryable<T> Select<T>(Expression<Func<T, bool>> where) where T : IEntity
        {
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBSelect<T>(where);
        }
        public static IQueryable<T> Select<T>(Expression<Func<T, bool>> where, Expression<Func<T, object>> orderby, int pageIndex, int pageSize, out int pageCount, out int allCount) where T : IEntity
        {
            pageCount = 0;
            allCount = 0;
            IsTypeCanBeUsed(typeof(T));
            return EntityOperationExtensions.DBSelect<T>(where, orderby, pageIndex, pageSize, out pageCount, out allCount);
        }
        public static void Save<T>(List<T> entities) where T : IEntity
        {
            entities.DBSave<T>();
        }

    }
}