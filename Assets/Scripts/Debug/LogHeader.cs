using UnityEngine;

// example :
//     Debug.Log(LogHeader.Function + "any log text will be here");
// result :    
//    CallStackLogMain:func_2() (at Assets/CallStackLogMain.cs:17)    
//    any log text will be here

public static class LogHeader
{
    public static string Path
    {
        get
        {
#if UNITY_EDITOR || USE_DEBUGGING
            string callStack = StackTraceUtility.ExtractStackTrace();
            callStack = callStack.Substring(callStack.IndexOf("\n") + 1);
            callStack = callStack.Substring(0, callStack.IndexOf("\n") + 1);
            return callStack;
#else
            return "";
#endif
        }
    }

    public static string Function
    {
        get
        {
#if UNITY_EDITOR || USE_DEBUGGING
            string callStack = StackTraceUtility.ExtractStackTrace();
            callStack = callStack.Substring(callStack.IndexOf("\n") + 1);
            callStack = callStack.Substring(0, callStack.IndexOf("("));
            return "[" + callStack + "] ";
#else
            return "";
#endif        
        }
    }
}