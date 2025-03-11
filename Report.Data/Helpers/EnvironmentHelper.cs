using Report.Data.Models.Environment;
using Report.Data.Models.Environment.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.Helpers;

public class EnvironmentHelper
{
    public static OmsDb GetOmsDb()
    {
        return new()
        {
            Host = Environment.GetEnvironmentVariable("OMS_DB_HOST"),
            Name = Environment.GetEnvironmentVariable("OMS_DB_NAME"),
            Port = Environment.GetEnvironmentVariable("OMS_DB_PORT"),
            Username = Environment.GetEnvironmentVariable("OMS_DB_USERNAME"),
            Password = Environment.GetEnvironmentVariable("OMS_DB_PASSWORD"),
        };
    }
    public static PaymentDb GetPaymentDb()
    {
        return new()
        {
            Host = Environment.GetEnvironmentVariable("PAYMENT_DB_HOST"),
            Name = Environment.GetEnvironmentVariable("PAYMENT_DB_NAME"),
            Port = Environment.GetEnvironmentVariable("PAYMENT_DB_PORT"),
            Username = Environment.GetEnvironmentVariable("PAYMENT_DB_USERNAME"),
            Password = Environment.GetEnvironmentVariable("PAYMENT_DB_PASSWORD"),
        };
    }
    public static ReportDb GetReportDb()
    {
        return new()
        {
            Host = Environment.GetEnvironmentVariable("REPORT_DB_HOST"),
            Name = Environment.GetEnvironmentVariable("REPORT_DB_NAME"),
            Port = Environment.GetEnvironmentVariable("REPORT_DB_PORT"),
            Username = Environment.GetEnvironmentVariable("REPORT_DB_USERNAME"),
            Password = Environment.GetEnvironmentVariable("REPORT_DB_PASSWORD"),
        };
    }
    public static Jwt GetJwt()
    {
        return new()
        {
            Key = Environment.GetEnvironmentVariable("JWT_KEY"),
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            Origin = Environment.GetEnvironmentVariable("JWT_ORIGIN")
        };
    }
}
