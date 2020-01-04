using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] intarry = { 6, 4, -3, 0, 5, -2, -1, 0, 1, -9 };
            Console.WriteLine("原数组：" + string.Join(',', intarry));
            Console.WriteLine("方法一：" + string.Join(',', convertArray1(intarry)));
            Console.WriteLine("方法二：" + string.Join(',', convertArray2(intarry)));

            Console.ReadKey();
            Console.WriteLine();

            string json = "{\"1\":\"bar\";\"2\":\"foo.bar\";\"3\":\"foo.foo\";\"4\":\"baz.cloudmall.com\";\"5\":\"baz.cloudmall.ai\"}";

            Console.WriteLine("格式化"+json);
            Console.WriteLine("格式化结果：");
            Console.Write(formatJson(json));

            Console.Read();
        }

        static int[] convertArray1(int[] values)
        {
            return values.ToList().OrderByDescending(x => x).ToArray();
        }

        static int[] convertArray2(int[] values)
        {
            int[] result;
            for (int i = 0; i < values.Length; i++)
            {
                for (int j = i; j < values.Length; j++)
                {
                    if (values[i] < values[j])
                    {
                        int temp = values[i];
                        values[i] = values[j];
                        values[j] = temp;
                    }
                }
            }
            return values;
        }

        static string formatJson(string json)
        {
            json = json.Replace("\"", "");
            var root = json.Trim('{').TrimEnd('}').Split(';');
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var mem in root)
            {
                var dic = mem.Split(":");
                handleName(result, dic[1].Split('.'), dic[0]);
            }

            var printstr = "{\r\n" + print(result) + "}\r\n";
            return printstr;
        }

        static Dictionary<string, object> handleName(Dictionary<string, object> parent, string[] names, string value)
        {
            if (names.Length == 1)
            {
                parent[names[0]] = value;
            }
            else
            {

                if (!parent.ContainsKey(names[0]))
                {
                    parent.Add(names[0], new Dictionary<string, object>());
                }

                var childnames = names.ToList();
                childnames.RemoveAt(0);
                handleName((Dictionary<string, object>)parent[names[0]], childnames.ToArray(), value);

            }

            return parent;
        }

        static string print(Dictionary<string, object> names, int level = 0)
        {
            var result = "";
            for(int i=0;i<names.Count;i++)
            {
                var mem = names.ElementAt(i);
                if (mem.Value is Dictionary<string, object>)
                {
                    result += " ".PadLeft((1 + level) * 4) + mem.Key + ":{\r\n";
                    result += print((Dictionary<string, object>)mem.Value, level + 1);
                    result += " ".PadLeft((1 + level) * 4) + "},\r\n";
                }
                else
                {
                    result += " ".PadLeft((1 + level) * 4) + mem.Key + ":" + mem.Value + ",\r\n";
                }

            }
            
            return result;
        }
    }
}
