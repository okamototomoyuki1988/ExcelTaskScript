using System;

public class ProcBase
{
    public Action<string, string> ShowMessage = null;
    public Action<string, string> SetRange = null;
    public Func<string, string> GetRange = null;
}