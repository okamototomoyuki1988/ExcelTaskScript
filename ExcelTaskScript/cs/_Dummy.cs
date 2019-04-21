
/************************************************************
 * CS と CSX の拡張メソッドの仕様齟齬で出るエラー回避用宣言
 ************************************************************/
static class StringExtensions
{
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static bool IsNotEmpty(this string value)
    {
        return string.IsNullOrEmpty(value) == false;
    }
}
