using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Report.Data.Persistance;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Report.Data.Models;
using Npgsql;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Report.Data.Helpers;

namespace Report.Data.Repositories.Concrete
{
    public class QueryRepository<T> : IQueryRepository<T> where T : class
    {
        protected readonly ReportContext Context;
        protected readonly DbSet<T> DbSet;
        public readonly IConfiguration _configuration;

        public QueryRepository(IConfiguration configuration, ReportContext context)
        {
            _configuration = configuration;
            Context = context;
            DbSet = context.Set<T>();
        }

        public async Task<List<OmsResponseDto>> GetOmsReport()
        {
          
            try
            {
                var omsDb = EnvironmentHelper.GetOmsDb();
                var connectionString = $"Host={omsDb.Host};Port={omsDb.Port};Database={omsDb.Name};Username={omsDb.Username};Password={omsDb.Password}";
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var data = await connection.QueryAsync<OmsResponseDto>("SELECT * FROM public.\"ViewName\"");

                    return data.ToList();

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<List<PaymentResponseDto>> GetPaymentReport(List<string> omsResponseDtos)
        {
            try
            {
                var paymentDb = EnvironmentHelper.GetPaymentDb();
                var connectionString = $"Host={paymentDb.Host};Port={paymentDb.Port};Database={paymentDb.Name};Username={paymentDb.Username};Password={paymentDb.Password}";
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var packages = new[] { "" };

                    var query = "SELECT * FROM public.\"FunctionName\" v WHERE \"PackageNumber\" = ANY(@packages)";

                    var data = await connection.QueryAsync<PaymentResponseDto>(query, new { packages = omsResponseDtos });

                    return data.ToList();
                }
            }

            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<List<T>> GetAllSql(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetSqlQuery(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable();

        }

        public async Task<List<T>> GetSqlFunction(string sqlQuery)
        {
            return await Context.Set<T>().FromSqlRaw($"{sqlQuery}").ToListAsync();
        }

        public async Task TruncateTable(string sqlQuery)
        {
            await Context.Set<T>().FromSqlRaw($"{sqlQuery}").ToListAsync();
        }

    }
}
