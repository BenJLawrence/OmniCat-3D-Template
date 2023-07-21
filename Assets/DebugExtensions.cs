using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Reflection;
using UnityEngine.Internal;

public interface IDebuggable
{
    public object OmniDebug();
}

public class Debug
{
    public static void Assert(bool condition, string format, params object[] args)
    {

    }
    public static void Assert(bool condition, string message, UnityEngine.Object context)
    {

    }
    public static void Assert(bool condition)
    {

    }
    public static void Assert(bool condition, object message, UnityEngine.Object context)
    {

    }
    public static void Assert(bool condition, string message)
    {

    }
    public static void Assert(bool condition, object message)
    {

    }
    public static void Assert(bool condition, UnityEngine.Object context)
    {

    }
    public static void AssertFormat(bool condition, UnityEngine.Object context, string format, params object[] args)
    {

    }
    public static void AssertFormat(bool condition, string format, params object[] args)
    {

    }

    public static void Break()
    {
        UnityEngine.Debug.Break();
    }

    public static void ClearDeveloperConsole()
    {

    }

    public static void DebugBreak()
    {

    }

    public static void Log(object message)
    {
        //Reader for OmniEnums
        if (message is IDebuggable debuggable) UnityEngine.Debug.Log(debuggable.OmniDebug());
        else UnityEngine.Debug.Log(message);
    }

    public static void Log(object message, UnityEngine.Object context)
    {
        //Reader for OmniEnums
        if (message is IDebuggable debuggable) UnityEngine.Debug.Log(debuggable.OmniDebug(), context);
        else UnityEngine.Debug.Log(message, context);
    }

    public static void LogWarning(object message)
    {
        //Reader for OmniEnums
        if (message is IDebuggable debuggable) UnityEngine.Debug.LogWarning(debuggable.OmniDebug()); 
        else UnityEngine.Debug.LogWarning(message);
    }

    public static void LogWarning(object message, UnityEngine.Object context)
    {
        //Reader for OmniEnums
        if (message is IDebuggable debuggable) UnityEngine.Debug.LogWarning(debuggable.OmniDebug(), context);
        else UnityEngine.Debug.LogWarning(message, context);
    }

    public static void LogError(object message)
    {
        //Reader for OmniEnums
        if (message is IDebuggable debuggable) UnityEngine.Debug.LogError(debuggable.OmniDebug());
        else UnityEngine.Debug.LogError(message);
    }

    public static void LogError(object message, UnityEngine.Object context)
    {
        //Reader for OmniEnums
        if (message is IDebuggable debuggable) UnityEngine.Debug.LogError(debuggable.OmniDebug(), context);
        else UnityEngine.Debug.LogError(message, context);
    }

    public static void LogException(Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }

    public static void LogException(Exception exception, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogException(exception, context);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {

    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {

    }
    public static void DrawLine(Vector3 start, Vector3 end)
    {

    }
    public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
    {
        
    }
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
    {

    }
    public static void DrawRay(Vector3 start, Vector3 dir, Color color)
    {

    }
    public static void DrawRay(Vector3 start, Vector3 dir, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
    {

    }
    public static void DrawRay(Vector3 start, Vector3 dir)
    {

    }
    public static void LogAssertion(object message, UnityEngine.Object context)
    {

    }
    public static void LogAssertion(object message)
    {

    }
    public static void LogAssertionFormat(UnityEngine.Object context, string format, params object[] args)
    {

    }

    public static void LogAssertionFormat(string format, params object[] args)
    {

    }
    public static void LogErrorFormat(string format, params object[] args)
    {

    }
    public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
    {

    }
    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {

    }
    public static void LogFormat(LogType logType, LogOption logOptions, UnityEngine.Object context, string format, params object[] args)
    {

    }
    public static void LogFormat(string format, params object[] args)
    {

    }
    
    public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
    {

    }

    public static void LogWarningFormat(string format, params object[] args)
    {

    }
}
