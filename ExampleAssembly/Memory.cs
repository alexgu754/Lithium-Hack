﻿using System.Runtime.InteropServices;
using RemoteAdmin;
using UnityEngine;
using System.Collections.Generic;
using Dissonance;
using System;
using System.Linq;
using UnityEngine.Networking;
using System.Reflection;
using System.Threading;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dissonance.Integrations.UNet_HLAPI;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;
namespace Cheat
{
    // Token: 0x02000006 RID: 6

    public class empty
    {
        public void CallCmdSelfDeduct(PlayerStats.HitInfo info)
        {

        }
    }
    public class fudgedinvd : MonoBehaviour
    {
        private static InventoryDisplay cInventoryDisplay = PlayerManager.localPlayer.GetComponent<InventoryDisplay>();
        private void UpdateVisibility()
        {
            if (Input.GetKeyDown(this.changeVisibilityKey) && (this.isVisible || !Cursor.visible))
            {
                this.ToggleInventory();
            }
            if (!Cursor.visible && Input.GetKeyDown(KeyCode.P))
            {
                foreach (Canvas canvas in UnityEngine.Object.FindObjectsOfType<Canvas>())
                {
                    if (canvas.transform.parent == null)
                    {
                        canvas.enabled = !canvas.enabled;
                    }
                }
            }
            if (this.isVisible)
            {
                Inventory.inventoryCooldown = 0.2f;
            }
        }

        private void ToggleInventory()
        {
            this.isVisible = !this.isVisible;
            this.localplayer.GetComponent<FirstPersonController>().m_MouseLook.isOpenEq = this.isVisible;
            CursorManager.singleton.eqOpen = this.isVisible;
            this.rootObject.SetActive(this.isVisible);
        }
        // Token: 0x04000708 RID: 1800
        [HideInInspector]
        public Inventory localplayer = cInventoryDisplay.localplayer;

        // Token: 0x04000709 RID: 1801
        public GameObject rootObject = cInventoryDisplay.rootObject;

        public Color selectedColor = Color.white;

        // Token: 0x04000712 RID: 1810
        public float itemAlpha = 0.5f;

        // Token: 0x04000713 RID: 1811
        private KeyCode changeVisibilityKey = KeyCode.Tab;

        // Token: 0x04000714 RID: 1812
        private bool isVisible = false;
    }


    internal class Memory
    {




        // Token: 0x06000017 RID: 23
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, IntPtr dwSize, out IntPtr lpNumberOfBytesRead);

        // Token: 0x06000018 RID: 24
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out UIntPtr lpNumberOfBytesWritten);

        // Token: 0x06000019 RID: 25
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(Memory.ProcessAccessFlags dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        // Token: 0x0600001A RID: 26 RVA: 0x0000439A File Offset: 0x0000259A
        public static void Init()
        {
            Memory._hHandle = Memory.OpenProcess(Memory.ProcessAccessFlags.All, false, Process.GetCurrentProcess().Id);
        }


        private void UpdateVisibility()
        {

        }

        

        public static void install(int funcNum)
        {
            MethodInfo methodToReplace = typeof(InventoryDisplay).GetMethod("UpdateVisibility", BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo methodToInject = typeof(fudgedinvd).GetMethod("UpdateVisibility", BindingFlags.Instance | BindingFlags.NonPublic);
            RuntimeHelpers.PrepareMethod(methodToReplace.MethodHandle);
            RuntimeHelpers.PrepareMethod(methodToInject.MethodHandle);

            unsafe
            {
                if (IntPtr.Size == 4)
                {
                    int* inj = (int*)methodToInject.MethodHandle.Value.ToPointer() + 2;
                    int* tar = (int*)methodToReplace.MethodHandle.Value.ToPointer() + 2;
#if DEBUG
                    Console.WriteLine("\nVersion x86 Debug\n");

                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;

                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    Console.WriteLine("\nVersion x86 Release\n");
                    *tar = *inj;
#endif
                }
                else
                {

                    long* inj = (long*)methodToInject.MethodHandle.Value.ToPointer() + 1;
                    long* tar = (long*)methodToReplace.MethodHandle.Value.ToPointer() + 1;
#if DEBUG
                    Console.WriteLine("\nVersion x64 Debug\n");
                    byte* injInst = (byte*)*inj;
                    byte* tarInst = (byte*)*tar;


                    int* injSrc = (int*)(injInst + 1);
                    int* tarSrc = (int*)(tarInst + 1);

                    *tarSrc = (((int)injInst + 5) + *injSrc) - ((int)tarInst + 5);
#else
                    Console.WriteLine("\nVersion x64 Release\n");
                    *tar = *inj;
#endif
                }
            }
        }


        public static void Hook()
        {
            bool flag = !Memory._memoryHooked;
            if (flag)
            {
                Memory._memoryHooked = true;
                Memory._pCallCmdSyncData = typeof(PlyMovementSync).GetMethod("CallCmdSyncData").MethodHandle.GetFunctionPointer();
                Memory._pRadio = typeof(Radio).GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic).MethodHandle.GetFunctionPointer();
                Memory.ReadMemory(Memory._pCallCmdSyncData + 473, Memory._pCallBytes, 10L);
                Memory._isMemory = true;

            }
            bool flag2 = !Memory._H;
            if (flag2)
            {
                Memory._H = true;
                Memory._pCallCmdSyncData = typeof(PlyMovementSync).GetMethod("CallCmdSyncData").MethodHandle.GetFunctionPointer();

            }
        }

        // Token: 0x0600001C RID: 28 RVA: 0x0000451C File Offset: 0x0000271C
        public static void SetRadio(bool val)
        {
            Memory._bAllRadio = val;
            Radio.roundEnded = Memory._bAllRadio;
        }

        // Token: 0x0600001D RID: 29 RVA: 0x00004530 File Offset: 0x00002730
        public static void SetSendPacket(bool val)
        {
            bool flag = !Memory._isMemory;
            if (!flag)
            {
                bool flag2 = !val;
                if (flag2)
                {
                    Memory._bSendPatched = true;
                    byte[] data = new byte[]
                    {
                        144,
                        144,
                        144
                    };
                    Memory.WriteMemory(Memory._pCallCmdSyncData + 473, data, 3u);
                }
                else
                {
                    Memory._bSendPatched = false;
                    Memory.WriteMemory(Memory._pCallCmdSyncData + 473, Memory._pCallBytes, 3u);
                }
            }
        }

        // Token: 0x0600001E RID: 30 RVA: 0x000045AC File Offset: 0x000027AC
        public static long ReadMemory(IntPtr address, byte[] buffer, long size)
        {
            IntPtr value;
            Memory.ReadProcessMemory(Memory._hHandle, address, buffer, (IntPtr)size, out value);
            return (long)value;
        }

        // Token: 0x0600001F RID: 31 RVA: 0x000045DC File Offset: 0x000027DC
        public static uint WriteMemory(IntPtr address, byte[] data, uint length)
        {
            UIntPtr uintPtr;
            Memory.WriteProcessMemory(Memory._hHandle, address, data, length, out uintPtr);
            return uintPtr.ToUInt32();
        }

        // Token: 0x06000020 RID: 32 RVA: 0x00004608 File Offset: 0x00002808
        public static uint WriteMemory(IntPtr address, string _data)
        {
            byte[] array = new byte[_data.Length];
            for (int i = 0; i < _data.Length; i++)
            {
                array[i] = (byte)_data[i];
            }
            UIntPtr uintPtr;
            Memory.WriteProcessMemory(Memory._hHandle, address, array, (uint)array.Length, out uintPtr);
            return uintPtr.ToUInt32();
        }

        // Token: 0x04000020 RID: 32
        private static bool _H;

        // Token: 0x04000021 RID: 33
        private static IntPtr _hHandle;

        // Token: 0x04000023 RID: 35
        public static IntPtr _pCallCmdSyncData;

        // Token: 0x04000024 RID: 36
        public static IntPtr _pRadio;

        // Token: 0x04000025 RID: 37
        public static bool _bAllRadio;

        // Token: 0x04000026 RID: 38
        private static bool _memoryHooked;

        // Token: 0x04000027 RID: 39
        private static bool _isMemory;

        // Token: 0x04000028 RID: 40
        public static bool _bSendPatched;

        // Token: 0x04000029 RID: 41
        private static readonly byte[] _pCallBytes = new byte[10];

        // Token: 0x02000008 RID: 8
        [Flags]
        public enum ProcessAccessFlags : uint
        {
            // Token: 0x0400002D RID: 45
            All = 2035711u,
            // Token: 0x0400002E RID: 46
            Terminate = 1u,
            // Token: 0x0400002F RID: 47
            CreateThread = 2u,
            // Token: 0x04000030 RID: 48
            VMOperation = 8u,
            // Token: 0x04000031 RID: 49
            VMRead = 16u,
            // Token: 0x04000032 RID: 50
            VMWrite = 32u,
            // Token: 0x04000033 RID: 51
            DupHandle = 64u,
            // Token: 0x04000034 RID: 52
            SetInformation = 512u,
            // Token: 0x04000035 RID: 53
            QueryInformation = 1024u,
            // Token: 0x04000036 RID: 54
            Synchronize = 1048576u
        }
    }
}
