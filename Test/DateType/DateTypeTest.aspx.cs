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
           var res60= DateSugar.DateDiff(DateSugar.DateInterval.Second,"2016-1-1 11:12:00".TryToDate(),"2016-1-1 11:13:00".TryToDate());

           var res2 = DateSugar.ComparisonTime("2015-1-1 18:13:02".TryToDate(), "2015-1-1 11:13:00".TryToDate(), "2015-1-1 4:14:01".TryToDate());


           //两个时间是否是同一天
           var isSameDay = DateSugar.IsSameDay(DateTime.Now,DateTime.Now.AddDays(-1));

           var isSameDay2 = DateSugar.IsSameDay(DateTime.Now, DateTime.Now);

           var isSameDay3 = DateSugar.IsSameDay(DateTime.Now.AddDays(-50), DateTime.Now);

           var isSameDay4 = DateSugar.IsSameDay((DateTime?)DateTime.Now,(DateTime?)DateTime.Now);


           var intMinutes = DateSugar.GetTwoTimeAreaIntersection(
               "2014-1-1 12:00:00".TryToDate(),
               "2014-1-1 12:30:00".TryToDate(),
               "2014-1-1 12:15:00".TryToDate(),
               "2014-1-1 13:00:00".TryToDate());
        
        }
    }
}