using System.IO;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;


namespace Evaluation2_CS
{
    public class P_S
    {
        public Double Slope { get; set; }
        public Double X1 { get; set; }
        public Double Y1 { get; set; }
        public Double X2 { get; set; }
        public Double Y2 { get; set; }
    }
    public class P_Same
    {
        public int A { get; set; }
        public int B { get; set; }
    }
    class Program
    {
        public static void Main()
        {
            var A = new List<string>();
            var B = new List<string>();

            var P_Tx = new List<Double>();
            var P_Ty = new List<Double>();
            var P_Rota = new List<Double>();
            var P_Count_L = new List<int>();
            var P_X = new List<Double>();
            var P_Y = new List<Double>();
            var P_New_X = new List<Double>();
            var P_New_Y = new List<Double>();
            var P_All = new List<Double>();//每個圖的周長
            double P_All_Long = 0;

            double UR_X = 0;//X
            double UR_Y = 0;//Y
            double Usage_Rate = 0;//使用率

            using (XmlReader reader = XmlReader.Create(@"C:\Users\user\Desktop\專題\NestingCShaprt-master\NestingCShaprt-master\NestingConsole\bin\Debug\outputXML.xml"))
            {


                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        if (reader.HasAttributes)
                        {
                            if (reader.Name == "g")
                            {
                                reader.MoveToAttribute("transform");
                                //Console.Write(" {0}={1}", reader.Name, reader.Value);
                                A.Add(reader.Value);
                                reader.MoveToElement();
                            }
                            if (reader.Name == "path")
                            {
                                reader.MoveToAttribute("d");
                                //Console.Write(" {0}={1}", reader.Name, reader.Value);
                                B.Add(reader.Value);
                                reader.MoveToElement();
                            }
                        }
                    }
                }
                A.RemoveAt(0); //刪掉板子
                var tran = A.ToArray();
                var poin = B.ToArray();

                //var P_Tx = new List<Double>();
                //var P_Ty = new List<Double>();
                //var P_Rota = new List<Double>();
                foreach (string i in tran)
                {
                    Console.WriteLine("{0} ", i);
                    string[] tran_Sp = i.Split(new string[] { "translate(", " ", ") rotate(", ")" }, StringSplitOptions.RemoveEmptyEntries);
                    P_Tx.Add(Convert.ToDouble(tran_Sp[0]));
                    P_Ty.Add(Convert.ToDouble(tran_Sp[1]));
                    P_Rota.Add(Convert.ToDouble(tran_Sp[2]));
                    /*foreach (string a in tran_Sp)
                    {
                        //Console.Write($"{a}\t"); //印出為string
                        //Console.Write($"{ float.Parse(a, CultureInfo.InvariantCulture.NumberFormat)}\t"); //字串轉浮點數-法一
                        Console.Write($"{Convert.ToDouble(a)}\t"); //字串轉浮點數-法二
                    }*/
                    Console.WriteLine();
                }
                int P_Count;
                //var P_Count_L = new List<int>();
                //var P_X = new List<Double>();
                //var P_Y = new List<Double>();
                //var P_New_X = new List<Double>();
                //var P_New_Y = new List<Double>();
                foreach (string i in poin)
                {
                    Console.WriteLine("{0}", i);
                    P_Count = Regex.Matches(i, "L").Count + 1;
                    P_Count_L.Add(P_Count);
                    Console.WriteLine("點的個數：{0}", P_Count);
                    string[] poin_Sp = i.Split(new string[] { "M", " ", "L", "Z" }, StringSplitOptions.RemoveEmptyEntries);
                    /*foreach (string a in poin_Sp)
                    {
                        Console.Write($"{Convert.ToDouble(a)}\t");
                        P_X.Add(Convert.ToDouble(a));
                    }*/
                    for (int j = 0; j < P_Count * 2; j++)
                    {
                        //Console.Write($"{Convert.ToDouble(poin_Sp[j])}\t");
                        if (j % 2 == 0)
                        {
                            P_X.Add(Convert.ToDouble(poin_Sp[j]));
                            Console.Write($"X  {Convert.ToDouble(poin_Sp[j])}\t");
                        }
                        else
                        {
                            P_Y.Add(Convert.ToDouble(poin_Sp[j]));
                            Console.Write($"Y  {Convert.ToDouble(poin_Sp[j])}\t");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

                int temp_Count = 0;
                for (int i = 0; i < tran.Length; i++)
                {
                    double angle = (360 - P_Rota[i]) * Math.PI / 180;
                    for (int j = 0; j < P_Count_L[i]; j++)
                    {
                        double temp_X = (P_X[temp_Count + j] - P_Tx[i]) * Math.Cos(angle) + (P_Y[temp_Count + j] - P_Ty[i]) * Math.Sin(angle) + P_Tx[i];
                        double temp_Y = -(P_X[temp_Count + j] - P_Tx[i]) * Math.Sin(angle) + (P_Y[temp_Count + j] - P_Ty[i]) * Math.Cos(angle) + P_Ty[i];

                        if (P_Rota[i] == 0)
                        {
                            temp_X = temp_X + P_Tx[i];
                            temp_Y = temp_Y + P_Ty[i];
                        }
                        else if (P_Rota[i] == 90)
                        {
                            temp_X = temp_X - P_Ty[i];
                            temp_Y = temp_Y + P_Tx[i];
                        }
                        else if (P_Rota[i] == 180)
                        {
                            temp_X = temp_X - P_Tx[i];
                            temp_Y = temp_Y - P_Ty[i];
                        }
                        else
                        {
                            temp_X = temp_X + P_Ty[i];
                            temp_Y = temp_Y - P_Tx[i];
                        }
                        P_New_X.Add(Math.Round(temp_X, 0));
                        P_New_Y.Add(Math.Round(temp_Y, 0));
                    }

                    temp_Count += P_Count_L[i];
                }
                int k = 0;
                for (int i = 0; i < tran.Length; i++)
                {
                    Console.WriteLine("Tx {0} Ty {1} Rota {2} PointCount {3}", P_Tx[i], P_Ty[i], P_Rota[i], P_Count_L[i]);
                    for (int j = 0; j < P_Count_L[i]; j++)
                    {
                        Console.WriteLine("舊X {0} , 舊y {1}", P_X[k + j], P_Y[k + j]);
                        Console.WriteLine("新X {0} , 新y {1}", P_New_X[k + j], P_New_Y[k + j]);
                    }
                    k += P_Count_L[i];
                    Console.WriteLine();
                }
                UR_X = P_New_X.Max();
                UR_Y = P_New_Y.Max();
                Usage_Rate = (UR_X * UR_Y) / (1000 * 1000);

                int temp_Count2 = 0;
                for (int i = 0; i < tran.Length; i++)
                {
                    Double Perimeter = 0; //周長
                    Double distance = 0;
                    for (int j = 0; j < P_Count_L[i]; j++)
                    {
                        if (j + 1 == P_Count_L[i])
                        {
                            distance = Math.Sqrt((Math.Pow(P_New_X[temp_Count2 + P_Count_L[i] - 1] - P_New_X[temp_Count2], 2) + Math.Pow(P_New_Y[temp_Count2 + P_Count_L[i] - 1] - P_New_Y[temp_Count2], 2)));
                        }
                        else
                        {
                            distance = Math.Sqrt((Math.Pow(P_New_X[temp_Count2 + j] - P_New_X[temp_Count2 + j + 1], 2) + Math.Pow(P_New_Y[temp_Count2 + j] - P_New_Y[temp_Count2 + j + 1], 2)));
                        }
                        Perimeter += distance;
                    }
                    Console.WriteLine();
                    Console.WriteLine("單個圖形周長：" + Math.Round(Perimeter, 0));
                    P_All.Add(Math.Round(Perimeter, 0));
                    temp_Count2 += P_Count_L[i];
                }
                P_All_Long = P_All.Sum();
                Console.WriteLine("總長度：" + P_All_Long);
                Console.WriteLine();
            }
            List<P_S> P_D = new List<P_S>();
            //Slope 斜率
            double Slope = 0;
            //Console.WriteLine(Math.Round(Slope, 2));
            int temp_Count3 = 0;
            for (int i = 0; i < A.ToArray().Length; i++)
            {
                for (int j = 0; j < P_Count_L[i]; j++)
                {
                    if (j + 1 == P_Count_L[i])
                    {
                        Slope = (double)((P_New_Y[temp_Count3 + P_Count_L[i] - 1] - P_New_Y[temp_Count3]) / (P_New_X[temp_Count3 + P_Count_L[i] - 1] - P_New_X[temp_Count3]));
                        if (Slope == -1.0 / 0.0)//負無限大轉正
                        {
                            Slope = -Slope;
                        }
                        P_D.Add(new P_S() { Slope = Math.Round(Slope, 2), X1 = P_New_X[temp_Count3 + P_Count_L[i] - 1], Y1 = P_New_Y[temp_Count3 + P_Count_L[i] - 1], X2 = P_New_X[temp_Count3], Y2 = P_New_Y[temp_Count3] });
                    }
                    else
                    {
                        Slope = (double)((P_New_Y[temp_Count3 + j] - P_New_Y[temp_Count3 + j + 1]) / (P_New_X[temp_Count3 + j] - P_New_X[temp_Count3 + j + 1]));
                        if (Slope == -1.0 / 0.0)//負無限大轉正
                        {
                            Slope = -Slope;
                        }
                        P_D.Add(new P_S() { Slope = Math.Round(Slope, 2), X1 = P_New_X[temp_Count3 + j], Y1 = P_New_Y[temp_Count3 + j] , X2 = P_New_X[temp_Count3 + j + 1], Y2 = P_New_Y[temp_Count3 + j + 1] });
                    }
                    Console.Write("{0}\t", Math.Round(Slope, 2));
                    
                }
                Console.WriteLine();
                temp_Count3 += P_Count_L[i];
            }
            //Console.WriteLine(P_Count_L.Sum());
            Console.WriteLine(P_D.Count);
            int www = 0;
            foreach(var P_DD in P_D)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", www, P_DD.Slope, P_DD.X1, P_DD.Y1, P_DD.X2, P_DD.Y2);
                www++;
            }
            
            double cut_Side = 0;//與板子重和部分
            for (int i = 0; i < P_D.Count; i++)
            {
                if (P_D[i].Slope == 1.0 / 0.0 && P_D[i].X1 == 0)
                {
                    cut_Side = Math.Abs(P_D[i].Y1 - P_D[i].Y2);
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", P_D[i].Slope, P_D[i].X1, P_D[i].Y1, P_D[i].X2, P_D[i].Y2);
                    P_All_Long = P_All_Long - cut_Side;
                }
                if (P_D[i].Slope == 0 && P_D[i].Y1 == 0)
                {
                    cut_Side = Math.Abs(P_D[i].X1 - P_D[i].X2);
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", P_D[i].Slope, P_D[i].X1, P_D[i].Y1, P_D[i].X2, P_D[i].Y2);
                    P_All_Long = P_All_Long - cut_Side;
                }                
            }
            Console.WriteLine("{0}變成{1}", P_All.Sum(), P_All_Long);


            var P_SameA = new List<int>();
            var P_SameB = new List<int>();
            for (int i = 0; i < (P_D.Count) - 1; i++)
            {
                for (int j = i+1; j < P_D.Count; j++)
                {
                    if (P_D[i].Slope == P_D[j].Slope)
                    {
                        P_SameA.Add(i);
                        P_SameB.Add(j);
                    }
                }
            }
            /*Console.WriteLine(P_SameA.Count);
            for (int i = 0; i < P_SameA.Count; i++)
            {
                Console.WriteLine("{0}={1}", P_SameA[i], P_SameB[i]);
            }*/
            double b1 = 0;//截距1
            double b2 = 0;//截距2
            List<P_Same> P_All_Same = new List<P_Same>();
            int hhh = 0;
            for (int i = 0; i < P_SameA.Count; i++)
            {
                if(P_D[P_SameA[i]].Slope == 1.0 / 0.0)
                {
                    if(P_D[P_SameA[i]].X1 == P_D[P_SameB[i]].X1)
                    {
                        if ((P_D[P_SameA[i]].Y1 > P_D[P_SameB[i]].Y1 && P_D[P_SameA[i]].Y1 > P_D[P_SameB[i]].Y2 && P_D[P_SameA[i]].Y2 > P_D[P_SameB[i]].Y1 && P_D[P_SameA[i]].Y2 > P_D[P_SameB[i]].Y2) || (P_D[P_SameB[i]].Y1 > P_D[P_SameA[i]].Y1 && P_D[P_SameB[i]].Y1 > P_D[P_SameA[i]].Y2 && P_D[P_SameB[i]].Y2 > P_D[P_SameA[i]].Y1 && P_D[P_SameB[i]].Y2 > P_D[P_SameA[i]].Y2))
                        {
                            ;
                        }
                        else
                        {
                            P_All_Same.Add(new P_Same() { A = P_SameA[i], B = P_SameB[i] });
                            hhh++;
                        }
                    }
                    
                }
                else
                {
                    b1 = P_D[P_SameA[i]].Y1 - (P_D[P_SameA[i]].Slope * P_D[P_SameA[i]].X1);
                    b2 = P_D[P_SameB[i]].Y1 - (P_D[P_SameB[i]].Slope * P_D[P_SameB[i]].X1);

                    if (b1 == b2)
                    {
                        if ((P_D[P_SameA[i]].X1 > P_D[P_SameB[i]].X1 && P_D[P_SameA[i]].X1 > P_D[P_SameB[i]].X2 && P_D[P_SameA[i]].X2 > P_D[P_SameB[i]].X1 && P_D[P_SameA[i]].X2 > P_D[P_SameB[i]].X2) || (P_D[P_SameB[i]].X1 > P_D[P_SameA[i]].X1 && P_D[P_SameB[i]].X1 > P_D[P_SameA[i]].X2 && P_D[P_SameB[i]].X2 > P_D[P_SameA[i]].X1 && P_D[P_SameB[i]].X2 > P_D[P_SameA[i]].X2))
                        {
                            ;
                        }
                        else
                        {
                            P_All_Same.Add(new P_Same() { A = P_SameA[i], B = P_SameB[i] });
                            hhh++;
                        }
                    }
                }
                
            }
            Console.WriteLine(hhh);
            /*for (int i = 0; i < P_All_Same.Count; i++)
            {
                Console.WriteLine("{0}={1}", P_All_Same[i].A , P_All_Same[i].B);
                Console.WriteLine("{0}={1}", P_D[P_All_Same[i].A].Slope, P_D[P_All_Same[i].B].Slope);
            }*/

            var cut = new List<Double>();
            var cut2 = new List<Double>();
            double cut_Long = 0;//邊重和部分
            double max = 0, max2 = 0;
            double min = 0, min2 = 0;
            for (int i = 0; i < P_All_Same.Count; i++)
            {
                cut.Clear();
                cut2.Clear();
                if (P_D[P_All_Same[i].A].Slope == 1.0 / 0.0)
                {
                    cut.Add(P_D[P_All_Same[i].A].Y1);
                    cut.Add(P_D[P_All_Same[i].A].Y2);
                    cut.Add(P_D[P_All_Same[i].B].Y1);
                    cut.Add(P_D[P_All_Same[i].B].Y2);
                    max = cut.Max();
                    cut.Remove(max);
                    min = cut.Min();
                    cut.Remove(min);
                    cut_Long = Math.Abs(cut[0] - cut[1]);
                }
                else
                {
                    cut.Add(P_D[P_All_Same[i].A].X1);
                    cut.Add(P_D[P_All_Same[i].A].X2);
                    cut.Add(P_D[P_All_Same[i].B].X1);
                    cut.Add(P_D[P_All_Same[i].B].X2);
                    max = cut.Max();
                    cut.Remove(max);
                    min = cut.Min();
                    cut.Remove(min);
                    
                    cut2.Add(P_D[P_All_Same[i].A].Y1);
                    cut2.Add(P_D[P_All_Same[i].A].Y2);
                    cut2.Add(P_D[P_All_Same[i].B].Y1);
                    cut2.Add(P_D[P_All_Same[i].B].Y2);
                    max2 = cut.Max();
                    cut2.Remove(max2);
                    min2 = cut.Min();
                    cut2.Remove(min2);
                    cut_Long = Math.Sqrt((Math.Pow(cut[0] - cut[1], 2) + Math.Pow(cut2[0] - cut2[1], 2)));
                }
                
                P_All_Long = P_All_Long - cut_Long;
                
            }
            Console.WriteLine("-------------");
            Console.WriteLine("雷切需總長：{0}",P_All_Long);
            Console.WriteLine("最大矩形面積：{0}x{1}={2}\n基板使用率：{3}%", UR_X, UR_Y, UR_X*UR_Y, Math.Round(Usage_Rate * 100, 2));

            Console.WriteLine("-------------");

            string filePath = @"C:\Users\user\Desktop\專題\成本計算.csv";
            StreamReader reader_ = new StreamReader(filePath);
            var lines = new List<string[]>();
            int Row = 0;
            int Col = 0;
            while (!reader_.EndOfStream)
            {
                string[] Line = reader_.ReadLine().Split(',');
                lines.Add(Line);
                Row++;
                //Console.WriteLine(Row);
                Col = Line.Length;
            }
            var data = lines.ToArray();
            //var row = data.GetLength(0);//列
            //var col = data.GetLength(1);//行
            //Console.WriteLine(data);

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    Console.Write($"{data[i][j]}\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("-------------");
            int a = 0, b = 0, c = 0;//方法&厚度&材質
            Double n = 0;//長度(mm)
            Double m = 0;//所要花費的價錢
            int flag = 1;//停止鈕
            while (flag==1) 
            {
                Console.WriteLine("使用方法? 焊接一(1)焊接二(2)雷切(3)");
                a = Convert.ToInt32(Console.ReadLine());//方式                
                
                switch (a) 
                {
                    case 1:
                        Console.WriteLine("板材厚度? <3mm(1) 3-6mm(2) 6-8mm(3) 8-12mm(4) 12mm以上(5)");
                        b = Convert.ToInt32(Console.ReadLine());
                        while (b > 5 || b < 1)
                        {
                            Console.WriteLine("無此厚度選項 請重選");
                            b = Convert.ToInt32(Console.ReadLine());
                        }
                        Console.WriteLine("選擇材質? 黑鐵(1) 白鐵(2) 鋁(3)");
                        c = Convert.ToInt32(Console.ReadLine());
                        while (c > 3 || c < 1)
                        {
                            Console.WriteLine("無此材質選項 請重選");
                            c = Convert.ToInt32(Console.ReadLine());
                        }
                        Console.WriteLine("輸入所要焊接之長度(mm)?");
                        n = float.Parse(Console.ReadLine());
                        Console.WriteLine("所需焊接之長度為{0}mm", n);
                        m += n / Double.Parse(data[b][4]) * Double.Parse(data[b][c]);
                        break;

                    case 2:
                        Console.WriteLine("板材厚度? <3mm(1) 3-6mm(2) 6-8mm(3) 8-12mm(4) 12mm以上(5)");
                        b = Convert.ToInt32(Console.ReadLine());
                        while (b > 5 || b < 1)
                        {
                            Console.WriteLine("無此厚度選項 請重選");
                            b = Convert.ToInt32(Console.ReadLine());
                        }
                        Console.WriteLine("選擇材質? 黑鐵(1) 白鐵(2) 鋁(3)");
                        c = Convert.ToInt32(Console.ReadLine());
                        while (c > 3 || c < 1)
                        {
                            Console.WriteLine("無此材質選項 請重選");
                            c = Convert.ToInt32(Console.ReadLine());
                        }
                        Console.WriteLine("輸入所要焊接之長度(mm)?");
                        n = float.Parse(Console.ReadLine());
                        Console.WriteLine("所需焊接之長度為{0}mm", n);
                        m += n / Double.Parse(data[b + 7][4]) * Double.Parse(data[b + 7][c]);
                        break;

                    case 3:
                        Console.WriteLine("板材厚度? <3mm(1) 4-6mm(2) 8-10mm(3) 12mm以上(4)");
                        b = Convert.ToInt32(Console.ReadLine());
                        while (b > 4 || b < 1)
                        {
                            Console.WriteLine("無此厚度選項 請重選");
                            b = Convert.ToInt32(Console.ReadLine());
                        }
                        if (b > 2) 
                        {
                            Console.WriteLine("選擇材質? 黑鐵(1) 白鐵(2)");
                            c = Convert.ToInt32(Console.ReadLine());
                            while (c > 2 || c < 1)
                            {
                                Console.WriteLine("無此材質選項 請重選");
                                c = Convert.ToInt32(Console.ReadLine());
                            }                            
                            n = P_All_Long;
                            Console.WriteLine("估算所要雷切之長度為{0}mm", n);
                            m += n / Double.Parse(data[b + 14][4]) * Double.Parse(data[b + 14][c]);
                        }else
                        {
                            Console.WriteLine("選擇材質? 黑鐵(1) 白鐵(2) 鋁(3)");
                            c = Convert.ToInt32(Console.ReadLine());
                            while (c > 3 || c < 1)
                            {
                                Console.WriteLine("無此材質選項 請重選");
                                c = Convert.ToInt32(Console.ReadLine());
                            }
                            n = P_All_Long;
                            Console.WriteLine("估算所要雷切之長度為{0}mm", n);
                            m += n / Double.Parse(data[b + 14][4]) * Double.Parse(data[b + 14][c]);
                        }                        
                        break;
                    default:
                        Console.WriteLine("格式錯誤");
                        break;

                }
                Console.WriteLine("-------------");
                Console.WriteLine("目前花費為 {0} 元", m);
                Console.WriteLine("-------------");
                Console.WriteLine("是否繼續運算? YES(1)/NO(0)");
                flag = Convert.ToInt32(Console.ReadLine());

            }
            Console.WriteLine("-------------");
            Console.WriteLine("總花費為 {0} 元", m);

        }
    }
}