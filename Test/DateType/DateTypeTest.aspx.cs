using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.DateType
{
    public partial class DateTypeTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //两个时间差距60秒
           var res60= DateSugar.DateDiff(DateSugar.DateInterval.Second,"2016-1-1 11:12:00".TryToDate(),"2015-1-1 11:13:00".TryToDate());

           var res2 = DateSugar.ComparisonTime("2015-1-1 18:13:02".TryToDate(), "2015-1-1 11:13:00".TryToDate(), "2015-1-1 4:14:01".TryToDate());
        }
    }
}