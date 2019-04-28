using System;
using System.Collections.Generic;

class GanttProc : ProcBase
{
    List<GanttItem> _items = null;

    List<DateTime> _holidays = null;

    public void ReloadGantt()
    {
        _ReadItem();

        _ReadHoliday();

        _ReloadSchedule();
    }

    void _ReadItem()
    {
        _items = new List<GanttItem>();
        int i = 8;
        while (true)
        {
            var planTime = GetRange($"E{i}");
            var planStart = GetRange($"F{i}");
            var actStart = GetRange($"H{i}");
            var actEnd = GetRange($"I{i}");
            if (planTime.IsNotEmpty())
            {
                _items.Add(new GanttItem
                {
                    planTime = planTime.ToFloat(),
                    planStart = planStart.ToDateTime(),
                    actStart = actStart.ToDateTime(),
                    actEnd = actEnd.ToDateTime(),
                });
            }
            else
            {
                break;
            }
            i++;
        }
    }

    void _ReadHoliday()
    {
        _holidays = new List<DateTime>();
        int i = 0;
        while (true)
        {
            var a = "J".NextA(i);
            var dateStr = GetRange(a + 4);
            if (dateStr.IsNotEmpty())
            {
                var text = GetRange(a + 5);
                if (text.IsNotEmpty())
                {
                    _holidays.Add(dateStr.ToDateTime());
                }
            }
            else
            {
                break;
            }
            i++;
        }
    }

    void _ReloadSchedule()
    {
        var hourDay = GetRange("F3").ToFloat();
        var sumHour = 0f;
        var now = DateTime.Now;

        var startDate = DateTime.Now.AddHours(-now.Hour).AddMinutes(-now.Minute).AddSeconds(-now.Second);
        DateTime planStartDate = default;

        foreach (var item in _items)
        {
            if (item.actStart == default)
            {
                if (planStartDate == default || item.planStart < planStartDate)
                {
                    planStartDate = item.planStart;
                }
            }
        }
        if (planStartDate != default)
        {
            startDate = planStartDate;
        }

        int prevI = -1;
        for (int i = 0, iLen = _items.Count; i < iLen; i++)
        {
            GanttItem item = _items[i];

            if (item.actEnd == default)
            {
                if (i == 0)
                {
                    item.planStart = startDate;
                }
                else
                {
                    item.planStart = _items[prevI].planEnd;
                }
                prevI = i;

                sumHour += item.planTime;
                item.planEnd = startDate.AddDays(sumHour);

                var shiftDays = item.ShiftEndByWeekend(_holidays);
                sumHour += shiftDays;

                SetRange($"F{i + 8}", item.planStart.ToString("MM/dd"));
                SetRange($"G{i + 8}", item.planEnd.ToString("MM/dd"));
            }
        }
    }
}