using Dapper;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using PaymentGateway.Helper;
using PaymentGateway.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;

namespace PaymentGateway.Repositories
{
    public class ModelRepository<T>
    {
        IConfiguration configuration;
        Type classType = typeof(T);
        public ModelRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public void Add(T context)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var _properties = classType.GetProperties().Where(w => w.GetCustomAttributes().FirstOrDefault(a => a.GetType() != typeof(KeyAttribute).GetType()) == null);
                //var _primaryKey = classType.GetProperties().FirstOrDefault(w => w.GetCustomAttributes().FirstOrDefault(a => a.GetType() != typeof(KeyAttribute).GetType()) != null)?.Name;          

                var insertProperties = String.Join(',', _properties.ToList().Select(s => s.Name));
                var insertValues = String.Join(',', _properties.ToList().Select(s => ":" + s.Name));
                string sQuery = "INSERT INTO " + classType.Name + "s (" + insertProperties + ")"
                                + " VALUES(" + insertValues + ")";
                dbConnection.Open();
                dbConnection.Execute(sQuery, context);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            using (IDbConnection dbConnection = GetConnection())
            {

                dbConnection.Open();
                return dbConnection.Query<Product>("SELECT * FROM " + classType.Name + "s");
            }
        }



        public T GetByID(int id)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                //var _properties = classType.GetProperties().Where(w => w.GetCustomAttributes().FirstOrDefault(a => a.GetType() != typeof(KeyAttribute).GetType()) == null);
                var _primaryKey = classType.GetProperties().FirstOrDefault(w => w.GetCustomAttributes().FirstOrDefault(a => a.GetType() != typeof(KeyAttribute).GetType()) != null)?.Name;

                //var insertProperties = String.Join(',', _properties.ToList().Select(s => s.Name));
                //var insertValues = String.Join(',', _properties.ToList().Select(s => ":" + s.Name));               
                string sQuery = "SELECT * FROM " + classType.Name + "s"
                               + " WHERE " + _primaryKey + " = :Id";
                dbConnection.Open();
                return dbConnection.Query<T>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                //var _properties = classType.GetProperties().Where(w => w.GetCustomAttributes().FirstOrDefault(a => a.GetType() != typeof(KeyAttribute).GetType()) == null);
                var _primaryKey = classType.GetProperties().FirstOrDefault(w => w.GetCustomAttributes().FirstOrDefault(a => a.GetType() != typeof(KeyAttribute).GetType()) != null)?.Name;

                string sQuery = "DELETE FROM " + classType.Name + "s"
                             + " WHERE " + _primaryKey + " = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }

        public void Update(Product prod)
        {
            using (IDbConnection dbConnection = GetConnection())
            {
                var _properties = classType.GetProperties().Where(w => w.GetCustomAttributes().FirstOrDefault(a => a.GetType() != typeof(KeyAttribute).GetType()) == null);
                var _primaryKey = classType.GetProperties().FirstOrDefault(w => w.GetCustomAttributes().FirstOrDefault(a => a.GetType() != typeof(KeyAttribute).GetType()) != null)?.Name;

                var updateProperties = String.Join(',', _properties.ToList().Select(s => s.Name + "= :" + s.Name));

                string sQuery = "UPDATE "+classType.Name+"s SET " + updateProperties
                               + " WHERE " + _primaryKey + " = :" + _primaryKey;
                dbConnection.Open();
                dbConnection.Query(sQuery, prod);
            }
        }


        public IDbConnection GetConnection()
        {
            var connectionString = configuration.GetSection("ConnectionStrings").GetSection("EmployeeConnection").Value;
            var conn = new OracleConnection(connectionString);
            return conn;
        }
    }
}
