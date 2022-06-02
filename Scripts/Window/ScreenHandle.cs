using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[StructLayout(LayoutKind.Sequential)]
public struct RECT
{
    public int Left, Top, Right, Bottom;

    public RECT(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
    public int X
    {
        get { return Left; }
        set { Right -= (Left - value); Left = value; }
    }

    public int Y
    {
        get { return Top; }
        set { Bottom -= (Top - value); Top = value; }
    }

    public int Height
    {
        get { return Bottom - Top; }
        set { Bottom = value + Top; }
    }

    public int Width
    {
        get { return Right - Left; }
        set { Right = value + Left; }
    }
    public static bool operator ==(RECT r1, RECT r2)
    {
        return r1.Equals(r2);
    }

    public static bool operator !=(RECT r1, RECT r2)
    {
        return !r1.Equals(r2);
    }

    public bool Equals(RECT r)
    {
        return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
    }

    public override bool Equals(object obj)
    {
        if (obj is RECT)
            return Equals((RECT)obj);
        return false;
    }
    public override string ToString()
    {
        return "right:" + Right + ". left:" + Left + ". top:" + Top + ". bottom:" + Bottom;
    }
}
public class ScreenHandle : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    public static extern IntPtr FindWindow(System.String className, System.String windowName);

    [DllImport("user32.dll")]
    public static extern int GetWindowRect(IntPtr hwnd, out RECT rect);
    public static void SetPosition(int x, int y, int resX = 0, int resY = 0)
    {
        SetWindowPos(FindWindow(null, "Erd"), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
    }
#endif
    RECT last;
    private void Start()
    {
        MinimumWindowSize.Set(580, 340);
        Screen.SetResolution(1024, 580, false);
        GetWindowRect(FindWindow(null, "Erd"), out RECT r);
        last = r;
    }
    private void Update()
    {
        GetWindowRect(FindWindow(null, "Erd"), out RECT r);
        if (r.Bottom != last.Bottom || r.Left != last.Left || r.Top != last.Top || r.Right != last.Right)
        {
            last = r;
            last.Left += 1;
            last.Right += 1;
            SetPosition(last.Left, last.Top);
        }
    }
    private void OnApplicationQuit()
    {
        MinimumWindowSize.Reset();
    }
}
