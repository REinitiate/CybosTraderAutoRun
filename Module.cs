using System;
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

        void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            IntPtr foregroundWinHandle = Win32.GetForegroundWindow();
            //Do something (f.e check if that is the needed window)

            if (hwnd == mainWndHandler)
            {   
                Module.ButtonClick(Win32.GetForegroundWindow(), 1);
            }
            Ut.Log(hwnd.ToString("X8") + "\t" + Win32.GetForegroundWindow().ToString("X8") + "\t" + hWinEventHook.ToString("X8"));

        }

        public void Action()
        {
            Module.KillProcess();
            Module.Pause(3000);

            System.Diagnostics.Process.Start(@"C:\DAISHIN\STARTER\ncStarter.exe", "/prj:cp");

            Module.Pause(3000);

            if (Module.CheckWindowIsExist("대신증권 CYBOS FAMILY"))
            {
                // 대신증권 CYBOS FAMILY 화면 존재
                // 보안프로그램이 사용하지 않음으로 선택되어 있습니다.
                Module.ButtonClick(Module.FindWindowByName("대신증권 CYBOS FAMILY"), 6);
            };

            Module.Pause(5000);

            IntPtr windowHandler = Module.FindWindowByName("대신증권 CYBOS FAMILY");            
            windowHandler = Module.FindWindowByName("CYBOS Starter");
            mainWndHandler = windowHandler; // 메인 윈도우 핸들러 등록 (WinEventProc에서 사용해야되기 때문)

            Module.Pause(5000);
            Module.SetTextInEdit(windowHandler, 156, "REINEX4");
            Module.Pause(3000);
            Module.SetTextInEdit(windowHandler, 157, "TURTLE");
            Module.Pause(3000);
            Module.ButtonClick(windowHandler, 203);


            IntPtr hhook = Win32.SetWinEventHook(Win32.EVENT_SYSTEM_FOREGROUND, Win32.EVENT_SYSTEM_FOREGROUND,
                IntPtr.Zero, new Win32.WinEventDelegate(WinEventProc), 0, 0, Win32.WINEVENT_OUTOFCONTEXT);

            Module.Pause(10000);

            Ut.Log(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle.ToString("X8"));
        }
    }



    class Module
    {
        
        bool isExecuting = false;
        // FindWindow 사용을 위한 코드
        

        //sends a windows message to the specified window
        

        public static void Run()
        {
        }

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
            Ut.Log(milliseconds.ToString() + "초 동안 대기");
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

        public static Win32.Rect WindowPosisionByName(IntPtr wdwHandler)
        {
            Win32.Rect rect = new Win32.Rect();
            Win32.GetWindowRect(wdwHandler, ref rect);
            return rect;
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
    }
}
