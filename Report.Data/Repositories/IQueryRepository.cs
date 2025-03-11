using Report.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.Repositories
{
    public interface IQueryRepository<T> where T : class
    {
        Task<List<T>> GetAllSql(Expression<Func<T, bool>> predicate);
        Task TruncateTable(string sqlQuery);
        IQueryable<T> GetSqlQuery(Expression<Func<T, bool>> predicate);
        Task<List<T>> GetSqlFunction(string sqlQuery);
        Task<List<PaymentResponseDto>> GetPaymentReport(List<string> omsResponseDtos);
        Task<List<OmsResponseDto>> GetOmsReport();
    }
}
