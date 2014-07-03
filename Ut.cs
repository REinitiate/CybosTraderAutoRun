using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;

namespace CybosAutoLogin
{
    class Ut
    {
        public static void Log(string text)
        {   
            Console.Write(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "\t" + text + "\n");
        }
    }

    class Setting
    {
        public static Hashtable ReadIniFile(string path)
        {
            Hashtable result = new Hashtable();
            return result;
        }

        public static string ReadIniValueByKey(string path, string key)
        {
            string result = null;
            StreamReader sr = new StreamReader(path);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("="))
                {
                    string[] words = line.Split('=');
                    if(words[0].CompareTo(key) == 0)
                        result = words[1];
                }
            }
            sr.Close();

            if (result == null)
            {
                Ut.Log(key + " 설정 정보가 없습니다!");
                throw new Exception();
            }
            else
            {
                Ut.Log("ini loading " + key + " : " + result);
            }

            return result;
        }
    }
}
