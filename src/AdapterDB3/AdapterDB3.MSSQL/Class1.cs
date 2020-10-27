using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdapterDB3.MSSQL
{
  public class ReadFromDB
  {
    public static string GetMyVersion()
    {
      System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
      System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);

      Version version = Assembly.GetAssembly(typeof(System.Data.SqlClient.SqlAuthenticationMethod)).GetName().Version;
      Version version2 = Assembly.GetAssembly(typeof(log4net.Config.BasicConfigurator)).GetName().Version;
      Version version3 = Assembly.GetAssembly(typeof(Oracle.ManagedDataAccess.Client.OracleConnection)).GetName().Version;

      return $"AdapterDB3.MSSQL Ver={fvi.FileVersion}" +
            $"{Environment.NewLine}Using EXTERNAL MSSQLClientVer {version.ToString()}" +
            $"{Environment.NewLine}Using EXTERNAL OracleClientVer {version3.ToString()}" +
            $"{Environment.NewLine}Using EXTERNAL Log4Net {version2.ToString()}";
    }

    public bool Connect(string sConnectString)
    {
      if (sConnectString.Contains("MSSQL"))
        return true;

      return false;
    }
  }
}
