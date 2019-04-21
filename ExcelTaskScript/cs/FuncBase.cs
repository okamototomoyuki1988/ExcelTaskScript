using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FuncBase
{
    //public Action<string, string> MsgBox = null;
    public Action<string, string> ShowMessage = null;
    public Action<string, string> SetRange = null;
    public Func<string, string> GetRange = null;
}