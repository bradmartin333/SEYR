using System;
using SEYRHelper;

namespace SEYRParser
{
    class Program
    {
        private enum State
        {
            Pass,
            Fail,
            Null,
            Misaligned
        }

        static void Main(string[] args)
        {
            int fieldNum = 0;
            double passNum = 0;
            double failNum = 0;

            Report.Entry[] data = Report.data(@"C:\Users\brad.martin\Desktop\report.txt");
            int num = Report.getNumImages(data) + 1;
            int numX = Report.getNumX(data);
            int numY = Report.getNumY(data);
            for (int k = 0; k < num; k++)
            {
                var thisImg = Report.getImage(data, k);

                for (int i = 0; i < numX; i++)
                {
                    for (int j = 0; j < numY; j++)
                    {
                        var thisCell = Report.getCell(thisImg, i, j);
                        bool pass = true;

                        bool A = true;
                        bool B = true;

                        foreach (Report.Entry item in thisCell)
                        {
                            State state = (State)Enum.Parse(typeof(State), item.Verdict);
                            switch (item.Name)
                            {
                                case "IC":
                                    switch (state)
                                    {
                                        case State.Pass:
                                            break;
                                        case State.Fail:
                                            pass = false;
                                            break;
                                        case State.Null:
                                            pass = false;
                                            break;
                                        case State.Misaligned:
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case "R":
                                    switch (state)
                                    {
                                        case State.Pass:
                                            break;
                                        case State.Fail:
                                            pass = false;
                                            break;
                                        case State.Null:
                                            pass = false;
                                            break;
                                        case State.Misaligned:
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case "G1":
                                    switch (state)
                                    {
                                        case State.Pass:
                                            break;
                                        case State.Fail:
                                            A = false;
                                            break;
                                        case State.Null:
                                            A = false;
                                            break;
                                        case State.Misaligned:
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case "G2":
                                    switch (state)
                                    {
                                        case State.Pass:
                                            break;
                                        case State.Fail:
                                            B = false;
                                            break;
                                        case State.Null:
                                            B = false;
                                            break;
                                        case State.Misaligned:
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                case "B":
                                    switch (state)
                                    {
                                        case State.Pass:
                                            break;
                                        case State.Fail:
                                            pass = false;
                                            break;
                                        case State.Null:
                                            pass = false;
                                            break;
                                        case State.Misaligned:
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                            }
                        }

                        if (!A || !B) pass = false;

                        if (pass)
                            passNum++;
                        else
                            failNum++;

                    }
                }

                if (passNum + failNum == 1600)
                {
                    fieldNum++;
                    Console.WriteLine(string.Format("Field #{0}\t{1}", fieldNum, (passNum / (passNum + failNum)).ToString("P")));
                    passNum = 0;
                    failNum = 0;
                }
            }

            Console.ReadKey();
        }
    }
}
