using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class TaskProc : ProcBase
{
    // 計画出力
    public void WritePlan()
    {
        List<string> nmStack = new List<string>(); // 大項目
        List<string> smStack = new List<string>(); // 小項目
        List<string> fnStack = new List<string>(); // 終了条件
        List<DateTime> tmStack = new List<DateTime>(); // 慣例
        int from = _ToN("E");
        int to = _ToN("DH");

        int recIndex = 0;
        for (int i = from; i <= to; i++)
        {
            string a = _ToA(i);
            string nm = GetRange(a + 4);
            string sm = GetRange(a + 5);
            string fn = GetRange(a + 6);
            if (nm.IsNotEmpty() ||
                sm.IsNotEmpty())
            {
                DateTime tm = new DateTime();
                tm = tm.AddHours(10)
                  .AddMinutes((i - from) * 5);
                tmStack.Add(tm);
                nmStack.Add(nm);
                smStack.Add(sm);
                fnStack.Add(fn);
                recIndex++;
            }
        }
        // 最後の日付追加
        const int START = 10;
        for (int i = 0; i < nmStack.Count; i++)
        {
            if (nmStack[i].IsNotEmpty() ||
                smStack[i].IsNotEmpty())
            {
                DateTime tm = tmStack[i];
                DateTime tmN = default(DateTime);
                if (i + 1 < tmStack.Count)
                {
                    tmN = tmStack[i + 1];
                }
                else
                {
                    tmN = new DateTime()
                        .AddHours(19);
                }
                DateTime dv = new DateTime();
                dv = dv.AddHours(0)
                    .AddMilliseconds(tmN.Subtract(tm).TotalMilliseconds);
                string nm = nmStack[i];
                string sm = smStack[i];
                string fn = fnStack[i];
                if (nm != "実務" && nm != "その他" && sm.IsNullOrEmpty())
                {
                    if (nm == "昼休憩")
                    {
                        SetRange("AZ" + (i + START), "昼休憩");
                        SetRange("BA" + (i + START), "");
                        SetRange("BB" + (i + START), "");
                    }
                    else
                    {
                        SetRange("AZ" + (i + START), nm);
                        SetRange("BA" + (i + START), nm);
                        SetRange("BB" + (i + START), "時間経過");
                    }
                }
                else if ((nm == "実務" || nm == "その他") && sm.IsNullOrEmpty())
                {
                    // 一つ前を取得
                    string prevNm = null;
                    string prevSm = null;
                    string prevFn = null;
                    for (var j = i; j >= 0; j--)
                    {
                        if (nmStack[j] != "昼休憩" && nmStack[j] != "休憩" && nmStack[j] != "朝礼")
                        {
                            if (prevNm == null && nmStack[j].IsNotEmpty())
                            {
                                prevNm = nmStack[j];
                            }
                            if (prevSm == null && smStack[j].IsNotEmpty())
                            {
                                prevSm = smStack[j];
                            }
                            if (prevFn == null && fnStack[j].IsNotEmpty())
                            {
                                prevFn = fnStack[j];
                            }
                            if (prevNm != null && prevSm != null)
                            {
                                break;
                            }
                        }
                    }

                    SetRange("AZ" + (i + START), prevNm);
                    SetRange("BA" + (i + START), prevSm);
                    SetRange("BB" + (i + START), prevFn);
                }
                else
                {
                    // 一つ前を取得
                    string prevNm = null;
                    for (var j = i; j >= 0; j--)
                    {
                        if (prevNm == null && nmStack[j].IsNotEmpty())
                        {
                            prevNm = nmStack[j];
                            break;
                        }
                    }

                    SetRange("AZ" + (i + START), prevNm);
                    SetRange("BA" + (i + START), sm);
                    SetRange("BB" + (i + START), fn);
                }

                SetRange("BC" + (i + START), "未完了");
                SetRange("BD" + (i + START), dv.ToString("HH:mm"));
                SetRange("BE" + (i + START), tm.ToString("HH:mm"));
                SetRange("BF" + (i + START), tmN.ToString("HH:mm"));
            }
        }
    }

    // 実働出力
    public void WriteAct()
    {
        List<string> smStack = new List<string>(); // 小項目
        List<DateTime> tmStack = new List<DateTime>(); // 慣例
        int from = _ToN("E");
        int to = _ToN("DH");

        int recIndex = 0;
        for (int i = from; i <= to; i++)
        {
            string a = _ToA(i);
            string sm = GetRange(a + 8);
            if (sm.IsNotEmpty())
            {
                DateTime tm = new DateTime()
                    .AddHours(10)
                    .AddMinutes((i - from) * 5);
                tmStack.Add(tm);// 0, 0, 0, 15, (i - from) * 5, 0, DateTimeKind.Utc);
                smStack.Add(sm);
                recIndex++;
            }
        }
        // 最後の日付追加
        const int START = 10;
        for (int i = 0; i < smStack.Count; i++)
        {
            if (smStack[i].IsNotEmpty())
            {
                DateTime tm = tmStack[i];
                DateTime tmN = default(DateTime);
                if (i + 1 < tmStack.Count)
                {
                    tmN = tmStack[i + 1];
                }
                else
                {
                    tmN = new DateTime()
                        .AddHours(19);
                }
                DateTime dv = new DateTime();

                dv = dv.AddHours(0)
                  .AddMilliseconds(tmN.Subtract(tm).TotalMilliseconds);
                string sm = smStack[i];

                SetRange("BN" + (i + START), sm);
                // 隙間
                SetRange("BX" + (i + START), tm.ToString("HH:mm"));
                SetRange("BY" + (i + START), tmN.ToString("HH:mm"));
                SetRange("BZ" + (i + START), dv.ToString("HH:mm"));
            }
        }

    }

    string _ToA(int column)
    {
        int temp;
        string letter = string.Empty;
        while (column > 0)
        {
            temp = (column - 1) % 26;
            letter = char.ConvertFromUtf32(temp + 65).ToString() + letter;
            column = (column - temp - 1) / 26;
        }
        return letter;
    }

    int _ToN(string letter)
    {
        int column = 0, length = letter.Length;
        for (int i = 0; i < length; i++)
        {
            column += ((int)letter[i] - 64) * (int)Math.Pow(26, length - i - 1);
        }
        return column;
    }
}

