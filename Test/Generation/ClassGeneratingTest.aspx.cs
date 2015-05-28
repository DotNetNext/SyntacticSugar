using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SyntacticSugar;
using System.Data;

namespace Test.Generation
{
    public partial class ClassGeneratingTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var cg = new cgtest();
            var classCode = ClassGenerating.DynamicToClass(cg, "newclass");

            //通过datatable生成实体类
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name");

           var   classCode2 = ClassGenerating.DataTableToClass(dt, "classTatabale");

             

        }
    }


    public class newclass
    {

        public int id { get; set; }

        public string name { get; set; }

        public Decimal? money { get; set; }

    }
            
    public class classTatabale
    {

        public int Id { get; set; }

        public string Name { get; set; }

    }
            
    public class newclass
    {

        public int id { get; set; }

        public String name { get; set; }

        public Decimal? money { get; set; }

    }
            
            
    public class cgtest
    {
        [TableAttribute("name")]
        public int id { get; set; }
        public string name { get; set; }
        public List<cgtest> a { get; set; } 
        public decimal? money { get; set; }
    }
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class TableAttribute : Attribute
    {
        private readonly string _TableName = "";

        public TableAttribute(string tableName)
        {
            this._TableName = tableName;
        }

        public string TableName
        {
            get { return this._TableName; }
        }
    }
}