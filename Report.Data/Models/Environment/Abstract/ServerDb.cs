using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Data.Models.Environment.Abstract;

public abstract class ServerDb
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }
}
