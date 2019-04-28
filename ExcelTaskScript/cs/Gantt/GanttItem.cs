using System;
using System.Collections.Generic;

class GanttItem
{
    public float planTime = 0f;
    public DateTime planStart = default;
    public DateTime planEnd = default;
    public DateTime actStart = default;
    public DateTime actEnd = default;

    public float ShiftEndByWeekend(List<DateTime> holidays)
    {
        float shiftDays = 0;
        int i = 0;
        while (true)
        {
            DateTime currentDate = planStart.AddDays(i);
            string currentStr = currentDate.ToString("yyyyMMdd");
            DayOfWeek dayOfWeek = currentDate.DayOfWeek;
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday || holidays.Exists(e => currentStr == e.ToString("yyyyMMdd")))
            {
                planEnd = planEnd.AddDays(1f);
                shiftDays += 1f;
            }
            else if (currentStr == planEnd.ToString("yyyyMMdd"))
            {
                break;
            }
            i += 1;
        }
        return shiftDays;
    }
}