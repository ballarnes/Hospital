using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Hospital.DataAccess.Infrastructure
{
    internal class StoredProcedureManager
    {
        private static Dictionary<Type, DbType> _typeMap;

        static StoredProcedureManager()
        {
            _typeMap = new Dictionary<Type, DbType>(37)
            {
                [typeof(byte)] = DbType.Byte,
                [typeof(sbyte)] = DbType.SByte,
                [typeof(short)] = DbType.Int16,
                [typeof(ushort)] = DbType.UInt16,
                [typeof(int)] = DbType.Int32,
                [typeof(uint)] = DbType.UInt32,
                [typeof(long)] = DbType.Int64,
                [typeof(ulong)] = DbType.UInt64,
                [typeof(float)] = DbType.Single,
                [typeof(double)] = DbType.Double,
                [typeof(decimal)] = DbType.Decimal,
                [typeof(bool)] = DbType.Boolean,
                [typeof(string)] = DbType.String,
                [typeof(char)] = DbType.StringFixedLength,
                [typeof(Guid)] = DbType.Guid,
                [typeof(DateTime)] = DbType.DateTime,
                [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                [typeof(TimeSpan)] = DbType.Time,
                [typeof(byte[])] = DbType.Binary,
                [typeof(byte?)] = DbType.Byte,
                [typeof(sbyte?)] = DbType.SByte,
                [typeof(short?)] = DbType.Int16,
                [typeof(ushort?)] = DbType.UInt16,
                [typeof(int?)] = DbType.Int32,
                [typeof(uint?)] = DbType.UInt32,
                [typeof(long?)] = DbType.Int64,
                [typeof(ulong?)] = DbType.UInt64,
                [typeof(float?)] = DbType.Single,
                [typeof(double?)] = DbType.Double,
                [typeof(decimal?)] = DbType.Decimal,
                [typeof(bool?)] = DbType.Boolean,
                [typeof(char?)] = DbType.StringFixedLength,
                [typeof(Guid?)] = DbType.Guid,
                [typeof(DateTime?)] = DbType.DateTime,
                [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,
                [typeof(TimeSpan?)] = DbType.Time,
                [typeof(object)] = DbType.Object
            };
        }

        public static DynamicParameters GetParameters<T>(T dto)
            where T : class
        {
            var parameters = new DynamicParameters();
            var props = dto.GetType().GetProperties();

            foreach (var prop in props)
            {
                if (prop.PropertyType.Namespace == "System")
                {
                    var name = prop.Name.Substring(0, 1).ToLower() + prop.Name.Substring(1, prop.Name.Length - 1);
                    parameters.Add($"@{name}", prop.GetValue(dto), _typeMap[prop.PropertyType]);
                }
            }

            return parameters;
        }
    }
}
