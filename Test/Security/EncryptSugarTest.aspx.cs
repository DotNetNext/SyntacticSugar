using SyntacticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test.Security
{
    public partial class EncryptSugarTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             var es= EncryptSugar.GetInstance();
             es.SetMaxCacheNum(0);
             string word = "abc";
             var wordEncrypt = es.Encrypto(word);
             var wordDecrypt = es.Decrypto(wordEncrypt);
             var wordMd5 = es.MD5(word);
        }
    }
}