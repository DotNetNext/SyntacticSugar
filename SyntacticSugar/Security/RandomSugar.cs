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
    public class  RandomSugar
    {

        private static readonly char[] RandChar = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static int seed = 1;

        private RandomSugar()
        {
        }

 

        /// <summary>
        /// 获取随机汉字数组
        /// </summary>
        /// <param name="strLength">数组长度</param>
        /// <returns></returns>
        public static string GetRandChinese(int strLength=1)
        {
            //定义一个字符串数组储存汉字编码的组成元素 
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random rnd = new Random();

            //定义一个object数组用来 
            object[] bytes = new object[strLength];

            /**/
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中 
每个汉字有四个区位码组成 
区位码第1位和区位码第2位作为字节数组第一个元素 
区位码第3位和区位码第4位作为字节数组第二个元素 
*/
            for (int i = 0; i < strLength; i++)
            {
                //区位码第1位 
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位 
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的 

                //种子避免产生重复值 
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

                //区位码第3位 
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位 
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
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
            Encoding gb = Encoding.GetEncoding("gb2312");
            var charList =bytes.Select(it=> gb.GetString((byte[])Convert.ChangeType(it, typeof(byte[]))));
            string reval = string.Join("", charList);
            return reval;
        }

        /// <summary>
        /// 根据规则随机生成字符串
        /// </summary>
        /// <param name="pattern">样式："?"代表一个字符，"#"代表一个一位数字，"*"代表一个字符串或一个一位数字</param>
        /// <returns>随机字符串</returns>
        public static string GetRandStringByPattern(string pattern)
        {
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
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
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
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
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
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
