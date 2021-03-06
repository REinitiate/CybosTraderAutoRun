﻿using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;



namespace CybosAutoLogin
{
    class Automation
    {
        IntPtr mainWndHandler;

        public void Action()
        {
            string id = Setting.ReadIniValueByKey(@"C:\settings\cybos.ini", "id");
            string password = Setting.ReadIniValueByKey(@"C:\settings\cybos.ini", "password");

            Module.KillProcess();
            Module.Pause(2000);

            System.Diagnostics.Process.Start(@"C:\DAISHIN\STARTER\ncStarter.exe", "/prj:cp");

            Module.Pause(3000);

            if (Module.CheckWindowIsExist("대신증권 CYBOS FAMILY"))
            {
                // 대신증권 CYBOS FAMILY 화면 존재
                // 보안프로그램이 사용하지 않음으로 선택되어 있습니다.
                Module.ButtonClick(Module.FindWindowByName("대신증권 CYBOS FAMILY"), 6);
            };

            Module.Pause(2000);

            IntPtr windowHandler = Module.FindWindowByName("CYBOS Starter");
            mainWndHandler = windowHandler; // 메인 윈도우 핸들러 등록 (WinEventProc에서 사용해야되기 때문)

            Ut.Log("메인핸들러 : " + mainWndHandler.ToString("X8"));

            if (Module.CheckVirtualBtnClicked())
            {
                // 모의투자 버튼 눌러져 있음.
            }
            else
            {
                // 모의투자 버튼 눌러져 있지 않음. 눌러야함.
                Module.ButtonClick(mainWndHandler, 327);
            }

            Module.mainWndHander = mainWndHandler;
            Ut.Log("메인핸들러 : " + mainWndHandler.ToString("X8"));

            //IntPtr hhook = Win32.SetWinEventHook(Win32.EVENT_SYSTEM_FOREGROUND, Win32.EVENT_SYSTEM_FOREGROUND,
            //   IntPtr.Zero, new Win32.WinEventDelegate(Module.WinEventProc), 0, 0, Win32.WINEVENT_OUTOFCONTEXT);

            Module.Pause(2000);
            Module.SetTextInEdit(windowHandler, 156, id);
            Module.Pause(500);
            Module.SetTextInEdit(windowHandler, 157, password);
            Module.Pause(500);
            Module.ButtonClick(windowHandler, 203);
            Module.Pause(20000);

            Win32.Rect rec = Module.WindowPosisionByName(mainWndHandler);
            rec.Top = rec.Top + 128 + 10;
            rec.Left = rec.Left + 492 + 10;

            Win32.POINT p = new Win32.POINT();
            p.x = rec.Left;
            p.y = rec.Top;

            Win32.SetCursorPos(p.x, p.y);
            Win32.SetCursorPos(p.x, p.y);
            
            Module.Click(rec);
            Module.Click(rec);
            Module.Click(rec);

            Ut.Log(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle.ToString("X8"));
            
        }
    }



    class Module
    {
        public static IntPtr mainWndHander;
        
        bool isExecuting = false;
        // FindWindow 사용을 위한 코드       

        // 프로세스가 실행되고 있는지 판단하는 프로그램.
        public static bool CheckProcessIsRunning(string pcName)
        {
            bool result = false;
            Process[] processes = Process.GetProcesses();
            foreach (Process proc in processes)
            {
                if (proc.ProcessName.Equals(pcName))
                {
                    Console.WriteLine(proc.ProcessName + " 실행중");
                    result = true;
                }
            }
            return result;
        }

        // 윈도우가 실행되고 있는지를 판단.
        public static bool CheckWindowIsExist(string windowName)
        {
            IntPtr intPtr = Module.FindWindowByName(windowName);
            if (intPtr.Equals(IntPtr.Zero))
                return false;
            else
                return true;
        }

        // 윈도우 앞으로
        public static void ShowWindow(IntPtr wdwHandler)
        {
            Win32.ShowWindow(wdwHandler, Win32.SW_SHOWNORMAL);
        }

        // 버튼 클릭 하는 동작
        public static void ButtonClick(IntPtr wdwHandler, int btnId)
        {
            IntPtr boxHwnd = Win32.GetDlgItem(wdwHandler, btnId);
            HandleRef hrefHWndTarget = new HandleRef(null, boxHwnd);
            Win32.SendMessage(boxHwnd, Win32.BM_CLICK, IntPtr.Zero, IntPtr.Zero);
            Ut.Log(wdwHandler.ToString() + " " + btnId + " " + "클릭");
        }

        // 에디트 다이얼로그에 문자열 세팅하는 동작
        public static void SetTextInEdit(IntPtr wdwHandler, int editId, string text)
        {
            IntPtr boxHwnd = Win32.GetDlgItem(wdwHandler, editId);
            HandleRef hrefHWndTarget = new HandleRef(null, boxHwnd);
            Win32.SendMessage(hrefHWndTarget, Win32.WM_SETTEXT, IntPtr.Zero, text);
            Ut.Log(wdwHandler.ToString() + " " + editId + " " + text);
        }

        // 윈도우 핸들 반환하는 함수 
        public static IntPtr FindWindowByName(string windowName)
        {
            IntPtr procHandler = Win32.FindWindow(null, windowName);
            if (procHandler != null && procHandler != IntPtr.Zero)
            {
                Ut.Log(windowName + "(" + procHandler.ToString() + ")" + " 윈도우를 찾았습니다.");
            }
            else
            {
                Ut.Log("윈도우가 실행되지 않았습니다.");
            }
            return procHandler;
        }

        // 인풋 시간만큼 대기(milliseonds)
        public static void Pause(int milliseconds)
        {
            Ut.Log((milliseconds/1000).ToString() + "초 동안 대기");
            System.Threading.Thread.Sleep(milliseconds);

            return;
        }

        // 프로그램 킬
        public static void KillProcess()
        {
            Process[] processList = Process.GetProcesses();

            foreach (var process in processList)
            {
                //Ut.Log(process.ProcessName);
                if (process.ProcessName.CompareTo("CpStart") == 0)
                {
                    process.Kill();
                    Ut.Log("CpStart Process killed.");
                }

                if (process.ProcessName.CompareTo("ncStarter") == 0)
                {
                    process.Kill();
                    Ut.Log("ncStarter Process killed.");
                }
            }

            return;
        }

        // 윈도우 좌표를 가져온다.
        public static Win32.Rect WindowPosisionByName(IntPtr wdwHandler)
        {
            Win32.Rect rect = new Win32.Rect();
            Win32.GetWindowRect(wdwHandler, ref rect);
            return rect;
        }
    
        // 모의투자 버튼이 눌려있는지를 판단.
        public static bool CheckVirtualBtnClicked()
        {
            IntPtr windowHandler = Module.FindWindowByName("CYBOS Starter");
            Win32.Rect rect = Module.WindowPosisionByName(windowHandler);
            //Ut.Log("top:" + rect.Top + " left:" + rect.Left + " bottom:" + rect.Bottom + " right:" + rect.Right);

            // 685, 715
            // 6, 15

            double argb = 0;
            int counter = 0;

            for (int i = 6; i < 16; i++)
            {
                for (int j = 685; j < 716; j++)
                {
                    int x = rect.Left + j;
                    int y = rect.Top + i;
                    argb = argb + Win32.GetPixelColor(x, y).ToArgb();
                    counter++;
                }
            }

            double avgPxl = argb / Convert.ToDouble(counter);

            if (avgPxl < -9000000)
            {
                // avgPxl : -10287651 --> 모의투자버튼 눌러져 있음.
                return true;
            }
            else
            {
                // avgPxl : -8949047 --> 모의투자버튼 눌러져 있지 않음
                return false;
            }
        }

        // 해당 좌표에 클릭 버튼
        public static bool Click(Win32.Rect location)
        {
            Ut.Log("X : " + location.Left + " Y : " + location.Top + " 에 클릭");

            Win32.mouse_event(Win32.MOUSEEVENTF_LEFTDOWN, (uint)location.Left, (uint)location.Top, 0, new IntPtr());
            Win32.mouse_event(Win32.MOUSEEVENTF_LEFTUP, (uint)location.Left, (uint)location.Top, 0, new IntPtr());
            return true;
        }

        public static void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            IntPtr foregroundWinHandle = Win32.GetForegroundWindow();            
            //Do something (f.e check if that is the needed window)
            if (hwnd == mainWndHander)
            {
                Module.ButtonClick(Win32.GetForegroundWindow(), 1);
            }
            Ut.Log(hwnd.ToString("X8") + "\t" + Win32.GetForegroundWindow().ToString("X8") + "\t" + hWinEventHook.ToString("X8"));
        }
    }

    public class Win32
    {
        // DC 관련 ***************************************************
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        // 윈도우 좌표관련
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        // 윈도우 관련 ***********************************************
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string StrWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern void SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public const int SW_SHOWNORMAL = 1;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int Param, string s);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(HandleRef hWnd, uint Msg, IntPtr wParam, string lParam);
        public const uint WM_SETTEXT = 0x000C;

        // 버튼 클릭 관련 ********************************************
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        public const int BM_CLICK = 0x00F5;

        static public System.Drawing.Color GetPixelColor(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                (int)(pixel & 0x0000FF00) >> 8,
                (int)(pixel & 0x00FF0000) >> 16);

            return color;
        }


        // 차일드 윈도우 관련 ****************************************
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        public static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size++ > 0)
            {
                var builder = new StringBuilder(size);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return String.Empty;
        }
        
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

        /// <summary>
        /// Returns a list of child windows
        /// </summary>
        /// <param name="parent">Parent of the windows to return</param>
        /// <returns>List of child windows</returns>
        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> result = new List<IntPtr>();
            GCHandle listHandle = GCHandle.Alloc(result);
            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
            }
            finally
            {
                if (listHandle.IsAllocated)
                    listHandle.Free();
            }
            return result;
        }

        /// <summary>
        /// Callback method to be used when enumerating windows.
        /// </summary>
        /// <param name="handle">Handle of the next window</param>
        /// <param name="pointer">Pointer to a GCHandle that holds a reference to the list to fill</param>
        /// <returns>True to continue the enumeration, false to bail</returns>
        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            GCHandle gch = GCHandle.FromIntPtr(pointer);
            List<IntPtr> list = gch.Target as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            //  You can modify this to check to see if you want to cancel the operation, then return a null here
            return true;
        }
        
        /// <summary>
        /// Delegate for the EnumChildWindows method
        /// </summary>
        /// <param name="hWnd">Window handle</param>
        /// <param name="parameter">Caller-defined variable; we use it for a pointer to our list</param>
        /// <returns>True to continue enumerating, false to bail.</returns>
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);


        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr
           hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess,
           uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        // Constants from winuser.h
        public const uint EVENT_SYSTEM_FOREGROUND = 3;
        public const uint WINEVENT_OUTOFCONTEXT = 0;

        //The GetForegroundWindow function returns a handle to the foreground window.
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        // 마우스 관련 ****************************************************
        public const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;
        

        [DllImport("User32.Dll")]
        public static extern long SetCursorPos(int x, int y);

        [DllImport("User32.Dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [DllImport("user32.dll")]
        public static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);    
    }
}