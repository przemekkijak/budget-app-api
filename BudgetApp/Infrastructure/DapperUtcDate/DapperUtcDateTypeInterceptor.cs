using System.Data;
using Dapper;

namespace BudgetApp.Infrastructure.DapperUtcDate;

public class DapperUtcDateTypeInterceptor : SqlMapper.TypeHandler<DateTime>
{
    public override DateTime Parse(object value)
    {
        if (value is DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
        }
        return (DateTime)value;
    }

    public override void SetValue(IDbDataParameter parameter, DateTime value)
    {
        parameter.Value = value;
        parameter.DbType = DbType.DateTime;
    }
}