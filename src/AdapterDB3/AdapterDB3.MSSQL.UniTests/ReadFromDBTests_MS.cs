using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterDB3.MSSQL.UniTests
{
  [TestClass]
  public class ReadFromDBTests_MS
  {
    [TestMethod]
    public void IsValidConnectString_ReturnTrue()
    {
      string sMyConnectString = "MSSQL;Server=lanceddb04;Database=evcs_cpovyvoj;User Id=Lancelot_EVCS;Password=Deneb2;";
      ReadFromDB test = new ReadFromDB();
      bool oResult = test.Connect(sMyConnectString);

      Assert.IsTrue(oResult);
    }

    [TestMethod]
    public void IsNotValidConnectString_ReturnFalse()
    {
      string sMyConnectString = "Oracle;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.18.6.88)(PORT=1522))(CONNECT_DATA=(SID=ORCL)));User ID=GAS_OTE_DATA;Password=x";
      ReadFromDB test = new ReadFromDB();
      bool oResult = test.Connect(sMyConnectString);

      Assert.IsFalse(oResult);
    }

    [TestMethod]
    [Ignore()] //"there is a problem with this test"
    public void IsValidConnectString_ReturnTrue_SchvalneChyba()
    {
      string sMyConnectString = "MSxxxSQL;Server=lanceddb04;Database=evcs_cpovyvoj;User Id=Lancelot_EVCS;Password=Deneb2;";
      //string sMyConnectString = "MSSQL;Server=lanceddb04;Database=evcs_cpovyvoj;User Id=Lancelot_EVCS;Password=Deneb2;";
      ReadFromDB test = new ReadFromDB();
      bool oResult = test.Connect(sMyConnectString);

      Assert.IsTrue(oResult);
    }
  }
}
