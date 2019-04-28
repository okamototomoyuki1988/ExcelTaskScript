
/************************************************************
 * CS と CSX の拡張メソッドの仕様齟齬で出るエラー回避用宣言
 ************************************************************/
using System;

static class _DummyExtensions
{
    public static bool IsNullOrEmpty(this string value) => default;

    public static bool IsNotEmpty(this string value) => default;

    public static float ToFloat(this string value, float error = -1) => default;

    public static int ToInt(this string value, int error = -1) => default;

    public static DateTime ToDateTime(this string value, DateTime error = default) => default;

    public static string NextA(this string value, int day = 1) => default;

    //public static string PrevA(this string value, int day = 1) => default;
}
