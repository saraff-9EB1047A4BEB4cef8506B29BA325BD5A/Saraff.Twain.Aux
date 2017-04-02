/* Этот файл является частью примеров использования библиотеки Saraff.Twain.NET
 * © SARAFF SOFTWARE (Кирножицкий Андрей), 2011.
 * Saraff.Twain.NET - свободная программа: вы можете перераспространять ее и/или
 * изменять ее на условиях Меньшей Стандартной общественной лицензии GNU в том виде,
 * в каком она была опубликована Фондом свободного программного обеспечения;
 * либо версии 3 лицензии, либо (по вашему выбору) любой более поздней
 * версии.
 * Saraff.Twain.NET распространяется в надежде, что она будет полезной,
 * но БЕЗО ВСЯКИХ ГАРАНТИЙ; даже без неявной гарантии ТОВАРНОГО ВИДА
 * или ПРИГОДНОСТИ ДЛЯ ОПРЕДЕЛЕННЫХ ЦЕЛЕЙ. Подробнее см. в Меньшей Стандартной
 * общественной лицензии GNU.
 * Вы должны были получить копию Меньшей Стандартной общественной лицензии GNU
 * вместе с этой программой. Если это не так, см.
 * <http://www.gnu.org/licenses/>.)
 * 
 * This file is part of samples of Saraff.Twain.NET.
 * © SARAFF SOFTWARE (Kirnazhytski Andrei), 2011.
 * Saraff.Twain.NET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * Saraff.Twain.NET is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * You should have received a copy of the GNU Lesser General Public License
 * along with Saraff.Twain.NET. If not, see <http://www.gnu.org/licenses/>.
 * 
 * PLEASE SEND EMAIL TO:  twain@saraff.ru.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Reflection;

namespace Saraff.Twain.Aux {

    internal static class ImpersonationUtils {

        public static Process RunAsCurrentUser(string fileName) {
            var _token=ImpersonationUtils._GetPrimaryToken();
            try {
                for(var _env=IntPtr.Zero; Win32.CreateEnvironmentBlock(ref _env,_token,false)&&_env!=IntPtr.Zero; ) {
                    try {
                        IntPtr _hStdIn,_hStdOut,_hStdError;
                        var _result=ImpersonationUtils._LaunchProcessAsUser(fileName,_token,_env,out _hStdIn,out _hStdOut,out _hStdError);

                        var _type=typeof(Process);
                        for(var _field=_type.GetField("standardInput",BindingFlags.Instance|BindingFlags.NonPublic); _field!=null&&_hStdIn!=IntPtr.Zero&&_hStdIn!=new IntPtr(-1); ) {
                            _field.SetValue(_result,new StreamWriter(new FileStream(new SafeFileHandle(_hStdIn,true),FileAccess.Write),Encoding.GetEncoding(866)) {
                                AutoFlush=true
                            });
                            break;
                        }
                        for(var _field=_type.GetField("standardOutput",BindingFlags.Instance|BindingFlags.NonPublic); _field!=null&&_hStdOut!=IntPtr.Zero&&_hStdOut!=new IntPtr(-1); ) {
                            _field.SetValue(_result,new StreamReader(new FileStream(new SafeFileHandle(_hStdOut,true),FileAccess.Read),Encoding.GetEncoding(866),true));
                            break;
                        }
                        for(var _field=_type.GetField("standardError",BindingFlags.Instance|BindingFlags.NonPublic); _field!=null&&_hStdError!=IntPtr.Zero&&_hStdError!=new IntPtr(-1); ) {
                            _field.SetValue(_result,new StreamReader(new FileStream(new SafeFileHandle(_hStdError,true),FileAccess.Read),Encoding.GetEncoding(866),true));
                            break;
                        }
                        return _result;
                    } finally {
                        Win32.DestroyEnvironmentBlock(_env);
                    }
                }
                throw new Win32Exception(Marshal.GetLastWin32Error(),"CreateEnvironmentBlock failed.");
            } finally {
                Win32.CloseHandle(_token);
            }
        }

        public static IDisposable ImpersonateCurrentUser() {
            return new WindowsIdentity(ImpersonationUtils._GetPrimaryToken()).Impersonate();
        }

        private static Process _LaunchProcessAsUser(string fileName,IntPtr token,IntPtr envBlock,out IntPtr hStdIn,out IntPtr hStdOut,out IntPtr hStdError) {
            var _saProcess=new _SecurityAttributes {
                nLength=Marshal.SizeOf(typeof(_SecurityAttributes))
            };
            var _saThread=new _SecurityAttributes {
                nLength=Marshal.SizeOf(typeof(_SecurityAttributes))
            };
            var _pi=new _ProcessInformation();

            var _si=new _StartupInfo {
                cb=Marshal.SizeOf(typeof(_StartupInfo)),
                lpDesktop=@"WinSta0\Default",
                wShowWindow=SW.Hide,
                dwFlags=StartFlags.UseShowWindow|StartFlags.UseStdHandles
            };

            ImpersonationUtils._CreatePipe(out _si.hStdInput,out hStdIn);
            if(!Win32.SetHandleInformation(_si.hStdInput,HandleFlags.Inherit,HandleFlags.Inherit)) {
                throw new Win32Exception(Marshal.GetLastWin32Error(),"SetHandleInformation failed.");
            }

            ImpersonationUtils._CreatePipe(out hStdOut,out _si.hStdOutput);
            if(!Win32.SetHandleInformation(_si.hStdOutput,HandleFlags.Inherit,HandleFlags.Inherit)) {
                throw new Win32Exception(Marshal.GetLastWin32Error(),"SetHandleInformation failed.");
            }

            ImpersonationUtils._CreatePipe(out hStdError,out _si.hStdError);
            if(!Win32.SetHandleInformation(_si.hStdError,HandleFlags.Inherit,HandleFlags.Inherit)) {
                throw new Win32Exception(Marshal.GetLastWin32Error(),"SetHandleInformation failed.");
            }

            if(!Win32.CreateProcessAsUser(token,null,fileName,_saProcess,_saThread,true,ProcessCreationFlags.CreateUnicodeEnvironment,envBlock,null,_si,_pi)) {
                throw new Win32Exception(Marshal.GetLastWin32Error(),"CreateProcessAsUser failed.");
            }
            Win32.CloseHandle(_pi.hProcess);
            Win32.CloseHandle(_pi.hThread);
            Win32.CloseHandle(_si.hStdInput);
            Win32.CloseHandle(_si.hStdOutput);
            Win32.CloseHandle(_si.hStdError);
            return Process.GetProcessById(_pi.dwProcessId);
        }

        private static IntPtr _GetPrimaryToken() {
            for(var _process=Process.GetProcessesByName("explorer").FirstOrDefault(); _process!=null; ) {
                var _token=IntPtr.Zero;
                if(Win32.OpenProcessToken(_process.Handle,TokenAccessLevels.Duplicate,ref _token)) {
                    try {
                        var _sa=new _SecurityAttributes {
                            nLength=Marshal.SizeOf(typeof(_SecurityAttributes))
                        };

                        var _primaryToken=IntPtr.Zero;
                        if(!Win32.DuplicateTokenEx(_token,TokenAccessLevels.AllAccess,_sa,TokenImpersonationLevel.Impersonation,TokenType.Primary,ref _primaryToken)) {
                            throw new Win32Exception(Marshal.GetLastWin32Error(),"DuplicateTokenEx failed.");
                        }
                        return _primaryToken;
                    } finally {
                        Win32.CloseHandle(_token);
                    }
                } else {
                    throw new Win32Exception(Marshal.GetLastWin32Error(),"OpenProcessToken failed.");
                }
            }
            throw new InvalidOperationException("Could not find explorer.exe.");
        }

        private static void _CreatePipe(out IntPtr hReadPipe,out IntPtr hWritePipe) {
            var _saPipe=new _SecurityAttributes {
                nLength=Marshal.SizeOf(typeof(_SecurityAttributes)),
                bInheritHandle=true
            };
            if(!Win32.CreatePipe(out hReadPipe,out hWritePipe,_saPipe,0)) {
                throw new Win32Exception(Marshal.GetLastWin32Error(),"CreatePipe failed.");
            }
        }

        #region Nested

        [StructLayout(LayoutKind.Sequential)]
        private class _SecurityAttributes {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Auto)]
        private class _StartupInfo {
            public int cb;

            [MarshalAs(UnmanagedType.LPStr)]
            public string lpReserved;

            [MarshalAs(UnmanagedType.LPStr)]
            public string lpDesktop;

            [MarshalAs(UnmanagedType.LPStr)]
            public string lpTitle;

            public int dwX;
            public int dwY;
            public int dwXSize;
            public int dwYSize;
            public int dwXCountChars;
            public int dwYCountChars;
            public int dwFillAttribute;

            [MarshalAs(UnmanagedType.U4)]
            public StartFlags dwFlags;

            [MarshalAs(UnmanagedType.U2)]
            public SW wShowWindow;

            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        private class _ProcessInformation {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        private enum TokenType {
            Primary=1,
            Impersonation=2
        }

        [Flags]
        private enum StartFlags:uint {
            ForceOnFeedback=0x00000040,
            ForceOffFeedback=0x00000080,
            PreventPinning=0x00002000,
            RunFullSCREEN=0x00000020,
            TitleIsAppId=0x00001000,
            TitleIsLinkName=0x00000800,
            UntrustedSource=0x00008000,
            UseCountChars=0x00000008,
            UseFillAttribute=0x00000010,
            UseHotKey=0x00000200,
            UsePosition=0x00000004,
            UseShowWindow=0x00000001,
            UseSize=0x00000002,
            UseStdHandles=0x00000100
        }

        private enum SW:ushort {
            ForceMinimize=11,
            Hide=0,
            Maximize=3,
            Minimize=6,
            Restore=9,
            Show=5,
            ShowDefault=10,
            ShowMaximized=3,
            ShowMinimized=2,
            ShowMinNoActive=7,
            ShowNA=8,
            ShowNoActivate=4,
            ShowNormal=1
        }

        [Flags]
        private enum ProcessCreationFlags {
            CreateBreakawayFromJob=0x01000000,
            CreateDefaultErrorMode=0x04000000,
            CreateNewConsole=0x00000010,
            CreateNewProcess_Group=0x00000200,
            CreateNoWindow=0x08000000,
            CreateProtectedProcess=0x00040000,
            CreatePreserveCodeAuthzLevel=0x02000000,
            CreateSeparateWowVdm=0x00000800,
            CreateSharedWowVdm=0x00001000,
            CreateSuspended=0x00000004,
            CreateUnicodeEnvironment=0x00000400,
            DebugOnlyThisProcess=0x00000002,
            DebugProcess=0x00000001,
            DetachedProcess=0x00000008,
            ExtendedStartupInfoPresent=0x00080000,
            InheritParentAffinity=0x00010000
        }

        [Flags]
        enum HandleFlags:uint {
            None=0,
            Inherit=1,
            ProtectFromClose=2
        }

        #endregion

        private sealed class Win32 {

            [DllImport("advapi32.dll",SetLastError=true)]
            internal static extern bool CreateProcessAsUser(
                IntPtr hToken,
                string lpApplicationName,
                string lpCommandLine,
                [In,Out] _SecurityAttributes lpProcessAttributes,
                [In,Out] _SecurityAttributes lpThreadAttributes,
                bool bInheritHandles,
                ProcessCreationFlags dwCreationFlags,
                IntPtr lpEnvironment,
                string lpCurrentDirectory,
                [In,Out] _StartupInfo lpStartupInfo,
                [Out] _ProcessInformation lpProcessInformation);

            [DllImport("advapi32.dll",SetLastError=true)]
            internal static extern bool DuplicateTokenEx(IntPtr hExistingToken,TokenAccessLevels dwDesiredAccess,[In,Out] _SecurityAttributes lpThreadAttributes,TokenImpersonationLevel ImpersonationLevel,TokenType dwTokenType,ref IntPtr phNewToken);

            [DllImport("advapi32.dll",SetLastError=true)]
            internal static extern bool OpenProcessToken(IntPtr hProcess,TokenAccessLevels dwDesiredAccess,ref IntPtr hToken);

            [DllImport("userenv.dll",SetLastError=true)]
            internal static extern bool CreateEnvironmentBlock(ref IntPtr lpEnvironment,IntPtr hToken,bool bInherit);

            [DllImport("userenv.dll",SetLastError=true)]
            internal static extern bool DestroyEnvironmentBlock(IntPtr lpEnvironment);

            [DllImport("kernel32.dll",SetLastError=true)]
            internal static extern bool CloseHandle(IntPtr handle);

            [DllImport("kernel32.dll")]
            internal static extern bool CreatePipe(out IntPtr hReadPipe,out IntPtr hWritePipe,[In,Out] _SecurityAttributes lpPipeAttributes,uint nSize);

            [DllImport("kernel32.dll",SetLastError=true)]
            internal static extern bool SetHandleInformation(IntPtr handle,HandleFlags dwMask,HandleFlags dwFlags);
        }
    }
}
