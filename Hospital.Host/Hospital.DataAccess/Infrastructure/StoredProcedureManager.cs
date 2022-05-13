using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Hospital.DataAccess.Infrastructure.Interfaces;

namespace Hospital.DataAccess.Infrastructure
{
    public class StoredProcedureManager : IStoredProcedureManager
    {
        public DynamicParameters GetParameters<T>(T dto)
            where T : class
        {
            var parameters = new DynamicParameters();
            var props = dto.GetType().GetProperties();

            foreach (var prop in props)
            {
                if (prop.PropertyType.Namespace == "System")
                {
                    var name = prop.Name.Substring(0, 1).ToLower() + prop.Name.Substring(1, prop.Name.Length - 1);
                    parameters.Add($"@{name}", prop.GetValue(dto));
                }
            }

            return parameters;
        }
    }
}
