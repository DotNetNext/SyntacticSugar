using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace SyntacticSugar
{
    /// <summary>
    /// ** 描述：根据条件生成随机字符串
    /// ** 创始时间：2010-2-28
    /// ** 修改时间：-
    /// ** 修改人：sunkaixuan
    /// ** 使用说明： 
    /// </summary>
    public class GenerateRandomString
    {

        private static readonly char[] RandChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
        private static int seed = 1;

        private GenerateRandomString()
        {
        }

        /// <summary>
        /// 获取随机汉字数组
        /// </summary>
        /// <param name="strlength">字符长度</param>
        /// <returns></returns>
        public static string GetRandChineseString(int strlength)
        {
            if (strlength == 0) return string.Empty;
            return string.Join("", GetRandChineseArray(strlength));
        }

        /// <summary>
        /// 获取随机汉字数组
        /// </summary>
        /// <param name="strlength">数组长度</param>
        /// <returns></returns>
        private static object[] GetRandChineseArray(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };


            //定义一个object数组用来
            object[] bytes = new object[strlength];

            /**/
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中
         每个汉字有四个区位码组成
         区位码第1位和区位码第2位作为字节数组第一个元素
         区位码第3位和区位码第4位作为字节数组第二个元素
         */
            for (int i = 0; i < strlength; i++)
            {
                lock (rand)
                {
                   rand= new Random(Convert.ToInt32(GetRandomNum(9)));
                }
                //区位码第1位
                int r1 = rand.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

 
                int r2;
                if (r1 == 13)
                {
                    r2 = rand.Next(0, 7);
                }
                else
                {
                    r2 = rand.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

 
                int r3 = rand.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

              
                int r4;
                if (r3 == 10)
                {
                    r4 = rand.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rand.Next(0, 15);
                }
                else
                {
                    r4 = rand.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中
                byte[] str_r = new byte[] { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中
                bytes.SetValue(str_r, i);

            }
            //获取GB2312编码页（表）
            Encoding gb = Encoding.GetEncoding("gb2312");
            List<string> array = new List<string>();
            foreach (var it in bytes) {
                array.Add(gb.GetString((byte[])Convert.ChangeType(bytes[0], typeof(byte[]))));
            }
            return array.ToArray();

        }

        /// <summary>
        /// 根据规则随机生成字符串
        /// </summary>
        /// <param name="pattern">样式："?"代表一个字符，"#"代表一个一位数字，"*"代表一个字符串或一个一位数字</param>
        /// <returns>随机字符串</returns>
        public static string GetRandStringByPattern(string pattern)
        {
            if (!pattern.Contains("#") && !pattern.Contains("?") && !pattern.Contains("*"))
            {
                return pattern;
            }

            char[] nums = pattern.ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nums.Length; i++)
            {
                switch (nums[i])
                {
                    case '?':
                        nums[i] = RandChar[rand.Next(10, 62)];
                        break;
                    case '#':
                        nums[i] = RandChar[rand.Next(0, 10)];
                        break;
                    case '*':
                        nums[i] = RandChar[rand.Next(62)];
                        break;
                    default:
                        break;
                }

                sb.Append(nums[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成随机的数值
        /// </summary>
        /// <param name="min">随机数可取该下界值</param>
        /// <param name="max">随机数的上界</param>
        /// <returns>随机的数值</returns>
        public static int GetFormatedNumeric(int min, int max)
        {
            int num = 0;
            Random ro = new Random(unchecked(seed * (int)DateTime.Now.Ticks));
            num = ro.Next(min, max);
            seed++;
            return num;
        }

        /// <summary>
        /// 获取指定长度和字符的随机字符串
        /// 通过调用 Random 类的 Next() 方法，先获得一个大于或等于 0 而小于 pwdchars 长度的整数
        /// 以该数作为索引值，从可用字符串中随机取字符，以指定的密码长度为循环次数
        /// 依次连接取得的字符，最后即得到所需的随机密码串了。
        /// </summary>
        /// <param name="pwdchars">随机字符串里包含的字符</param>
        /// <param name="pwdlen">随机字符串的长度</param>
        /// <returns>随机产生的字符串</returns>
        public static string GetRandomString(string pwdchars, int pwdlen)
        {
            StringBuilder tmpstr = new StringBuilder();
            int randNum;

            for (int i = 0; i < pwdlen; i++)
            {
                randNum = rand.Next(pwdchars.Length);
                tmpstr.Append(pwdchars[randNum]);
            }

            return tmpstr.ToString();
        }

        /// <summary>
        /// 获取指定长度的随机字符串
        /// </summary>
        /// <param name="pwdlen">随机字符串的长度</param>
        /// <returns>随机产生的字符串</returns>
        public static string GetRandomString(int pwdlen)
        {
            return GetRandomString("abcdefghijklmnoaspqrstuvwxyzABCDEFGHIJKLMNasdfOPQRSTUVWXYZ0123456789_*", pwdlen);
        }

        /// <summary>
        /// 获取指定长度的纯字母随机字符串
        /// </summary>
        /// <param name="pwdlen">数字串长度</param>
        /// <returns>纯字母随机字符串</returns>
        public static string GetRandWord(int pwdlen)
        {
            return GetRandomString("abcdefghijklmnopqrstuvwxyzdafasfaABCDEFGHIJKLMNOPQRSTUVWXYZ", pwdlen);
        }

        /// <summary>
        /// 获取指定长度的纯数字随机数字串
        /// </summary>
        /// <param name="intlong">数字串长度</param>
        /// <returns>纯数字随机数字串</returns>
        public static string GetRandomNum(int intlong)
        {
            StringBuilder w = new StringBuilder(string.Empty);

            for (int i = 0; i < intlong; i++)
            {
                w.Append(rand.Next(10));
            }

            return w.ToString();
        }

        /// <summary>
        /// 获取按照年月时分秒随机数生成的文件名
        /// </summary>
        /// <returns>随机文件名</returns>
        public static string GetFileRndName()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture) + GetRandomString("0123456789", 4);
        }
    }
}
