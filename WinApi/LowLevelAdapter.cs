﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.ComponentModel;
using IWshRuntimeLibrary;
using System.Reflection;
using System.Windows.Automation;
using System.Collections.Generic;

namespace AutoMova.WinApi
{
    public static partial class LowLevelAdapter
    {

        public static IntPtr SetHook(int type, HookProc callback)
        {
            var process = Process.GetCurrentProcess();
            var module = process.MainModule;
            var handle = GetModuleHandle(module.ModuleName);
            return SetWindowsHookEx(type, callback, handle, 0);
        }
        public static void ReleaseHook(IntPtr id)
        {
            UnhookWindowsHookEx(id);
        }
        public static IntPtr NextHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public static bool KeyPressed(Keys keyCode)
        {
            return (GetKeyState((int)keyCode) & 0x8000) == 0x8000;
        }

        private static IntPtr GetFocusedHandle()
        {
            var threadId = GetCurrentThreadId();
            var wndThreadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);

            if (threadId == wndThreadId)
            {
                return IntPtr.Zero;
            }

            AttachThreadInput(wndThreadId, threadId, true);
            IntPtr focusedHandle = GetFocus();
            AttachThreadInput(wndThreadId, threadId, false);
            return focusedHandle;
        }

        private static IntPtr GetWindowHandle()
        {
            IntPtr hWnd = IntPtr.Zero;
            var threadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            var info = new GUITHREADINFO();
            info.cbSize = Marshal.SizeOf(info);
            var success = GetGUIThreadInfo(threadId, ref info);
            var focusedHandle = GetFocusedHandle();
            if (success)
            {
                if (info.hwndCaret != IntPtr.Zero)
                {
                    hWnd = info.hwndCaret;
                }
                else if (info.hwndFocus != IntPtr.Zero)
                {
                    hWnd = info.hwndFocus;
                }
                else if (focusedHandle != IntPtr.Zero)
                {
                    hWnd = focusedHandle;
                }
                else if (info.hwndActive != IntPtr.Zero)
                {
                    hWnd = info.hwndActive;
                }
            }
            else
            {
                hWnd = focusedHandle;
            }
            if (hWnd == IntPtr.Zero)
            {
                hWnd = GetForegroundWindow();
            }
            return hWnd;
        }

        public static IntPtr GetCurrentLayout()
        {
            var wndThreadId = GetWindowThreadProcessId(GetWindowHandle(), IntPtr.Zero);
            return GetKeyboardLayout(wndThreadId);
        }

        public static IntPtr[] GetLayoutList()
        {
            uint nElements = GetKeyboardLayoutList(0, null);
            IntPtr[] ids = new IntPtr[nElements];
            GetKeyboardLayoutList(ids.Length, ids);
            return ids;
        }

        //public static uint LayoutToUint(IntPtr layout)
        //{
        //    return LoadKeyboardLayout((layout.ToString("x8")), KLF_ACTIVATE);
        //}
        
        public static void SetNextKeyboardLayoutByKeypress()
        {
            Debug.WriteLine("SetNextKeyboardLayoutByKeyPress()...");            

            var shiftDown = MakeKeyInput(Keys.LShiftKey, true);
            var shiftUp = MakeKeyInput(Keys.LShiftKey, false);
            var altDown = MakeKeyInput(Keys.LMenu, true);
            var altUp = MakeKeyInput(Keys.LMenu, false);

            SendInput(2, new INPUT[2] { altDown, shiftDown }, Marshal.SizeOf(typeof(INPUT)));
            //Thread.Sleep(1);
            SendInput(2, new INPUT[2] { altUp, shiftUp }, Marshal.SizeOf(typeof(INPUT)));
            //Thread.Sleep(1);
        }

        public static void SetNextKeyboardLayout()
        {
            Debug.WriteLine("SetNextKeyboardLayout()...");

            PostMessage(GetWindowHandle(), WM_INPUTLANGCHANGEREQUEST, INPUTLANGCHANGE_FORWARD, HKL_NEXT);            
        }

        public static void SetKeyboadLayout(uint layout)
        {
            Debug.WriteLine("SetKeyboardLayout("+layout.ToString("x8")+")...");
            PostMessage(GetWindowHandle(), WM_INPUTLANGCHANGEREQUEST, INPUTLANGCHANGE_SYSCHARSET, layout);
            //SendMessage(GetWindowHandle(), WM_INPUTLANGCHANGEREQUEST, INPUTLANGCHANGE_SYSCHARSET, (int)layout);
        }

        public static void SendCopy()
        {
            var ctrlDown = MakeKeyInput(Keys.LControlKey, true);
            var ctrlUp = MakeKeyInput(Keys.LControlKey, false);
            var cDown = MakeKeyInput(Keys.C, true);
            var cUp = MakeKeyInput(Keys.C, false);
            SendInput(2, new INPUT[2] { ctrlDown, cDown }, Marshal.SizeOf(typeof(INPUT)));
            Thread.Sleep(20);
            SendInput(2, new INPUT[2] { ctrlUp, cUp }, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendSelectAll()
        {
            var ctrlDown = MakeKeyInput(Keys.LControlKey, true);
            var ctrlUp = MakeKeyInput(Keys.LControlKey, false);
            var aDown = MakeKeyInput(Keys.A, true);
            var aUp = MakeKeyInput(Keys.A, false);
            SendInput(2, new INPUT[2] { ctrlDown, aDown }, Marshal.SizeOf(typeof(INPUT)));
            Thread.Sleep(20);
            SendInput(2, new INPUT[2] { ctrlUp, aUp }, Marshal.SizeOf(typeof(INPUT)));
        }

        

        public static void SendKeyPress(Keys vkCode, bool shift = false)
        {
            var down = MakeKeyInput(vkCode, true);
            var up = MakeKeyInput(vkCode, false);

            if (shift)
            {
                var shiftDown = MakeKeyInput(Keys.ShiftKey, true);
                var shiftUp = MakeKeyInput(Keys.ShiftKey, false);
                SendInput(4, new INPUT[4] { shiftDown, down, up, shiftUp }, Marshal.SizeOf(typeof(INPUT)));
            }
            else
            {
                SendInput(2, new INPUT[2] { down, up }, Marshal.SizeOf(typeof(INPUT)));
            }

        }

        public static Dictionary<Keys, bool> ReleasePressedFnKeys()
        {           
            Debug.WriteLine("ReleasePressedFnKeys()...");
            var fnKeys = new Dictionary<Keys, bool>();
            fnKeys.Add(Keys.LControlKey, ReleasePressedKey(Keys.LControlKey));
            fnKeys.Add(Keys.RControlKey, ReleasePressedKey(Keys.RControlKey));
            fnKeys.Add(Keys.LShiftKey, ReleasePressedKey(Keys.LShiftKey));
            fnKeys.Add(Keys.RShiftKey, ReleasePressedKey(Keys.RShiftKey));
            fnKeys.Add(Keys.LMenu, ReleasePressedKey(Keys.LMenu));
            fnKeys.Add(Keys.RMenu, ReleasePressedKey(Keys.RMenu));
            return fnKeys;
        }

        public static void PressPressedFnKeys(Dictionary<Keys, bool> fnKeys)
        {
            Debug.WriteLine("PressPressedFnKeys()...");
            foreach (var key in fnKeys)
            {
                if (key.Value)
                {
                    PressPressedKey(key.Key);
                }                
            }
        }

        private static bool ReleasePressedKey(Keys keyCode)
        {
            if (!KeyPressed(keyCode)) return false;
            Debug.WriteLine("{0} was down", keyCode);
            var keyUp = MakeKeyInput(keyCode, false);            
            SendInput(1, new INPUT[1] { keyUp }, Marshal.SizeOf(typeof(INPUT)));            
            return true;
        }

        private static void PressPressedKey(Keys keyCode)
        {
            var keyDown = MakeKeyInput(keyCode, true);
            SendInput(1, new INPUT[1] { keyDown }, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendShowSettingsMessage()
        {
            PostMessage((IntPtr)HWND_BROADCAST, WM_SHOW_SETTINGS, 0, 0);
        }


        private static string GetAutorunPath()
        {
            return System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                "AutoMova.lnk");
        }
        public static void CreateAutorunShortcut()
        {
            var currentPath = Assembly.GetExecutingAssembly().Location;
            var shortcutLocation = GetAutorunPath();
            var description = "Automatic keyboard layout switcher";
            if (System.IO.File.Exists(shortcutLocation))
            {
                return;
            }
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
            shortcut.Description = description;
            shortcut.TargetPath = currentPath;
            shortcut.Save();
        }
        public static void DeleteAutorunShortcut()
        {
            System.IO.File.Delete(GetAutorunPath());
        }

        //static Dictionary<string, object> lBackup = new Dictionary<string, object>();
        //static IDataObject lDataObject = null;
        //static string[] lFormats = new string[] {};

        public static Keys ToKey(char ch)
        {
            var layout = GetCurrentLayout();

            short keyNumber = VkKeyScanEx(ch, layout);
            if (keyNumber == -1)
            {
                return System.Windows.Forms.Keys.None;
            }
            return (System.Windows.Forms.Keys)(((keyNumber & 0xFF00) << 8) | (keyNumber & 0xFF));
        }

        public static Keys ToKey(char ch, IntPtr layout)
        {
            short keyNumber = VkKeyScanEx(ch, layout);
            if (keyNumber == -1)
            {
                return System.Windows.Forms.Keys.None;
            }
            return (System.Windows.Forms.Keys)(((keyNumber & 0xFF00) << 8) | (keyNumber & 0xFF));
        }

        public static string KeyCodeToUnicode(Keys keyCode, IntPtr keyboardLayout)
        {
            byte[] keyboardState = new byte[255];
            bool keyboardStateStatus = GetKeyboardState(keyboardState);

            if (!keyboardStateStatus)
            {
                return "";
            }

            uint virtualKeyCode = (uint)keyCode;
            uint scanCode = MapVirtualKey(virtualKeyCode, 0);
            
            StringBuilder result = new StringBuilder();
            ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, result, (int)5, (uint)0, keyboardLayout);

            return result.ToString();
        }

        public static string GetSelectedText()
        {
            var backupText = ClipboardGetTextSTA();
            ClipboardClearSTA();            
            PostMessage(GetWindowHandle(), WM_COPY, 0, 0);
            var word = ClipboardGetTextSTA();
            Debug.WriteLine($"Clipboard: {word}");
            if (!String.IsNullOrEmpty(backupText)) ClipboardSetTextSTA(backupText);            
            return word;
        }

        private static void ClipboardClearSTA()
        {            
            Thread th = new Thread(
                delegate()
                {
                    Clipboard.Clear();
                });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            th.Join();            
        }

        private static string ClipboardGetTextSTA()
        {
            string text = null;
            Thread th = new Thread(
                delegate()
                {
                    text = Clipboard.GetText();
                });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            th.Join();
            return text;
        }

        private static void ClipboardSetTextSTA(string text)
        {            
            Thread th = new Thread(
                delegate ()
                {
                    Clipboard.SetText(text);
                });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
            th.Join();
        }

        public static void BackupClipboard()
        {
            //lDataObject = Clipboard.GetDataObject();
            //if (lDataObject == null) 
            //{
            //    return;
            //}
            //lFormats = lDataObject.GetFormats(false);
            //lBackup = new Dictionary<string, object>();
            //foreach(var lFormat in lFormats)
            //{
            //  lBackup.Add(lFormat, lDataObject.GetData(lFormat, false));
            //}
            //Debug.WriteLine(lDataObject);
            //Debug.WriteLine(lFormats);
        }

        public static void RestoreClipboard()
        {
            //Debug.WriteLine(lDataObject);
            //Debug.WriteLine(lFormats);
            //if (lDataObject == null)
            //{
            //    return;
            //}
            //foreach (var lFormat in lFormats)
            //{
            //    lDataObject.SetData(lBackup[lFormat]);
            //}
            //Clipboard.SetDataObject(lDataObject);
        }

        public static bool ThisIsLaptop()
        {
            LowLevelAdapter.SYSTEM_POWER_CAPABILITIES systemPowerCapabilites;
            LowLevelAdapter.GetPwrCapabilities(out systemPowerCapabilites);
            return systemPowerCapabilites.LidPresent;
        }
    }
}