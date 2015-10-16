using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
namespace Test.Extenions
{
    public partial class DataTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dt = DataTableSugar.CreateEasyTable("id").AddRow(DBNull.Value);
            var x = dt.DataTableToList<a>();
        }

        public class a {
            public int? id { get; set; }
        }
    }
}