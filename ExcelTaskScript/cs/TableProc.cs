using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class TableProc : FuncBase
{
    // 計画出力
    public void WritePlan()
    {
        List<string> nmStack = new List<string>(); // 慣例
        List<string> bgStack = new List<string>(); // 大項目
        List<string> mdStack = new List<string>(); // 中項目
        List<string> smStack = new List<string>(); // 小項目
        List<DateTime> tmStack = new List<DateTime>(); // 慣例
        int from = _ToN("E");
        int to = _ToN("DH");

        int recIndex = 0;
        for (int i = from; i <= to; i++)
        {
            string a = _ToA(i);
            string nm = GetRange(a + 4);
            string bg = GetRange(a + 6);
            string md = GetRange(a + 7);
            string sm = GetRange(a + 8);
            if (nm.IsNotEmpty() ||
                bg.IsNotEmpty() ||
                md.IsNotEmpty() ||
                sm.IsNotEmpty())
            {
                DateTime tm = new DateTime();
                tm = tm.AddHours(10)
                  .AddMinutes((i - from) * 5);
                tmStack.Add(tm);
                nmStack.Add(nm);
                bgStack.Add(bg);
                mdStack.Add(md);
                smStack.Add(sm);
                recIndex++;
            }
        }
        // 最後の日付追加
        const int START = 12;
        for (int i = 0; i < nmStack.Count; i++)
        {
            if (nmStack[i].IsNotEmpty() ||
                bgStack[i].IsNotEmpty() ||
                mdStack[i].IsNotEmpty() ||
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
                //dv.AddHours(5);
                //dv.AddMilliseconds(tmN.Subtract(tm).Millisecond);
                //dv.AddMilliseconds(tmN.Subtract(tm).TotalMilliseconds);
                dv = dv.AddHours(0)
                    .AddMilliseconds(tmN.Subtract(tm).TotalMilliseconds);
                string nm = nmStack[i];
                string bg = bgStack[i];
                string md = mdStack[i];
                string sm = smStack[i];
                if (nm != "業務" && bg.IsNullOrEmpty() && md.IsNullOrEmpty() && sm.IsNullOrEmpty())
                {
                    SetRange("Z" + (i + START), nm);
                    SetRange("AA" + (i + START), "");
                    SetRange("AB" + (i + START), "");
                }
                else if (nm == "業務" && bg.IsNullOrEmpty() && md.IsNullOrEmpty() && sm.IsNullOrEmpty())
                {
                    // 一つ前を取得
                    string prevBg = null;
                    string prevMd = null;
                    string prevSm = null;
                    for (var j = i - 1; j >= 0; j--)
                    {
                        if (prevBg == null && bgStack[j].IsNotEmpty())
                        {
                            prevBg = bgStack[j];
                        }
                        if (prevMd == null && mdStack[j].IsNotEmpty())
                        {
                            prevMd = mdStack[j];
                        }
                        if (prevSm == null && smStack[j].IsNotEmpty())
                        {
                            prevSm = smStack[j];
                        }
                        if (prevBg != null)
                        {
                            break;
                        }
                    }
                    if (bg.IsNullOrEmpty() && (md.IsNotEmpty() || sm.IsNotEmpty()))
                    {
                        bg = "↓";
                    }
                    if (md.IsNullOrEmpty() && sm.IsNotEmpty())
                    {
                        md = "↓";
                    }

                    SetRange("Z" + (i + START), prevBg);
                    SetRange("AA" + (i + START), prevMd);
                    SetRange("AB" + (i + START), prevSm);
                }
                else
                {
                    if (bg.IsNullOrEmpty() && (md.IsNotEmpty() || sm.IsNotEmpty()))
                    {
                        bg = "↓";
                    }
                    if (md.IsNullOrEmpty() && sm.IsNotEmpty())
                    {
                        md = "↓";
                    }
                    SetRange("Z" + (i + START), bg);
                    SetRange("AA" + (i + START), md);
                    SetRange("AB" + (i + START), sm);
                }

                SetRange("AC" + (i + START), "");
                SetRange("AD" + (i + START), "");
                SetRange("AE" + (i + START), dv.ToString("HH:mm"));
                SetRange("AF" + (i + START), tm.ToString("HH:mm"));
                SetRange("AG" + (i + START), tmN.ToString("HH:mm"));
                SetRange("AH" + (i + START), dv.ToString("HH:mm"));
                //SetRange("AE" + (i + START), dv.ToString());
                //SetRange("AF" + (i + START), tm.ToString());
                //SetRange("AG" + (i + START), tmN.ToString());
                //SetRange("AH" + (i + START), dv.ToString());
            }
        }
    }

    // 計画出力
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
            string sm = GetRange(a + 10);
            if (sm.IsNotEmpty())
            {
                DateTime tm = new DateTime()
                    .AddHours(10)
                    .AddMinutes((i - from) * 5);
                tmStack.Add(tm);// 0, 0, 0, 15, (i - from) * 5, 0, DateTimeKind.Utc);
                //tmStack.Add(new DateTime());// 0, 0, 0, 15, (i - from) * 5, 0, DateTimeKind.Utc);
                //tmStack[recIndex].AddHours(15);
                //tmStack[recIndex].AddMinutes((i - from) * 5);
                smStack.Add(sm);
                recIndex++;
            }
        }
        // 最後の日付追加
        const int START = 12;
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
                DateTime dv = new DateTime();// 0, 0, 0, 15, (i - from) * 5, 0, DateTimeKind.Utc);
                                             //dv.AddMilliseconds(tmN.Millisecond - tm.Millisecond);
                                             //dv.AddMilliseconds(tmN.Subtract(tm).TotalMilliseconds);
                dv = dv.AddHours(0)
                  .AddMilliseconds(tmN.Subtract(tm).TotalMilliseconds);
                string sm = smStack[i];

                SetRange("AN" + (i + START), sm);
                // 隙間
                SetRange("AX" + (i + START), tm.ToString("HH:mm"));
                SetRange("AY" + (i + START), tmN.ToString("HH:mm"));
                SetRange("AZ" + (i + START), dv.ToString("HH:mm"));
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
