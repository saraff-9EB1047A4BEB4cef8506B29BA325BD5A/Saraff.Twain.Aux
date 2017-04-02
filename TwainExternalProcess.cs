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
using System.IO;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;

namespace Saraff.Twain.Aux {

    public sealed class TwainExternalProcess {

        public static void Execute(string fileName,Action<Twain32> execCallback) {
            using(var _proc=TwainExternalProcess.AuxProcess.CreateProcess(fileName)) {
                _proc.Begin();

                execCallback(new Twain32RealProxy(_proc).GetTransparentProxy() as Twain32);

                _proc.End();
            }
        }

        public static void Handler(Twain32 twain32) {
            try {

                #region Method Handler

                Action _method=() => {
                    var _result=TwainProxy.Execute(twain32,Convert.FromBase64String(Console.In.ReadLine()));

                    Console.Out.WriteLine(Separators.BeginResponse);
                    Console.Out.WriteLine(Convert.ToBase64String(_result));
                    Console.Out.WriteLine(Separators.EndResponse);
                };

                #endregion

                #region Event Handler

                Action<EventHandlerTwainCommand> _event=command => {
                    Console.Out.WriteLine(Separators.BeginEvent);
                    try {
                        Console.Out.WriteLine(Convert.ToBase64String(command.ToArray()));
                        for(string _responce=null; !string.IsNullOrEmpty(_responce=Console.In.ReadLine()); ) {
                            if(_responce==Separators.BeginRequest) {
                                _method();
                                continue;
                            }

                            var _res=((EventHandlerTwainCommand)TwainCommand.FromArray(Convert.FromBase64String(_responce))).Args;
                            foreach(var _property in _res.GetType().GetProperties()) {
                                var _setter = _property.GetSetMethod();
                                if(_setter!=null) {
                                    _setter.Invoke(command.Args,new object[] { _property.GetValue(_res,null) });
                                }
                            }
                            break;
                        }
                    } finally {
                        Console.Out.WriteLine(Separators.EndEvent);
                    }
                };

                #endregion

                #region AcquireError

                twain32.AcquireError+=(sender,e) => {
                    _event(new EventHandlerTwainCommand {
                        Member=typeof(Twain32).GetEvent(Twain32Events.AcquireError),
                        Args=e
                    });
                };

                #endregion

                #region XferDone

                twain32.XferDone+=(sender,e) => {
                    var _args=new Twain32.SerializableCancelEventArgs { Cancel=e.Cancel };
                    _event(new EventHandlerTwainCommand {
                        Member=typeof(Twain32).GetEvent(Twain32Events.XferDone),
                        Args=_args
                    });
                    e.Cancel=_args.Cancel;
                };

                #endregion

                #region EndXfer

                twain32.EndXfer+=(sender,e) => {
                    _event(new EventHandlerTwainCommand {
                        Member=typeof(Twain32).GetEvent(Twain32Events.EndXfer),
                        Args=e
                    });
                };

                #endregion

                #region SetupMemXferEvent

                twain32.SetupMemXferEvent+=(sender,e) => {
                    _event(new EventHandlerTwainCommand {
                        Member=typeof(Twain32).GetEvent(Twain32Events.SetupMemXferEvent),
                        Args=e
                    });
                };

                #endregion

                #region MemXferEvent

                twain32.MemXferEvent+=(sender,e) => {
                    _event(new EventHandlerTwainCommand {
                        Member=typeof(Twain32).GetEvent(Twain32Events.MemXferEvent),
                        Args=e
                    });
                };

                #endregion

                #region SetupFileXferEvent

                twain32.SetupFileXferEvent+=(sender,e) => {
                    _event(new EventHandlerTwainCommand {
                        Member=typeof(Twain32).GetEvent(Twain32Events.SetupFileXferEvent),
                        Args=e
                    });
                };

                #endregion

                #region FileXferEvent

                twain32.FileXferEvent+=(sender,e) => {
                    _event(new EventHandlerTwainCommand {
                        Member=typeof(Twain32).GetEvent(Twain32Events.FileXferEvent),
                        Args=e
                    });
                };

                #endregion

                #region TwainStateChanged

                twain32.TwainStateChanged+=(sender,e) => {
                    _event(new EventHandlerTwainCommand {
                        Member=typeof(Twain32).GetEvent(Twain32Events.TwainStateChanged),
                        Args=e
                    });
                };

                #endregion

                #region AcquireCompleted

                twain32.AcquireCompleted+=(sender,e) => {
                    _event(new EventHandlerTwainCommand {
                        Member=typeof(Twain32).GetEvent(Twain32Events.AcquireCompleted),
                        Args=e
                    });
                };

                #endregion

                Console.Out.WriteLine(Separators.Ready);
                for(string _query=null; !string.IsNullOrEmpty(_query=Console.In.ReadLine())&&_query!=Separators.End; ) {
                    if(_query==Separators.BeginRequest) {
                        _method();
                    }
                }
            } catch(Exception ex) {
                try {
                    Console.Error.WriteLine(Separators.BeginException);
                    for(var _ex=ex; _ex!=null; _ex=_ex.InnerException) {
                        Console.Error.WriteLine("{0}: {1}",_ex.GetType().Name,_ex.Message);
                        Console.Error.WriteLine(_ex.StackTrace);
                        Console.Error.WriteLine();
                    }
                    Console.Error.WriteLine(Separators.EndException);
                } catch {
                }
            }
        }

        internal sealed class AuxProcess:IDisposable {
            private Process _proc;

            public static AuxProcess CreateProcess(string fileName) {
                return new AuxProcess {
                    _proc=Environment.UserInteractive?Process.Start(new ProcessStartInfo {
                        CreateNoWindow=true,
                        WorkingDirectory=Directory.GetCurrentDirectory(),
                        FileName=fileName,
                        UseShellExecute=false,
                        RedirectStandardInput=true,
                        RedirectStandardOutput=true,
                        StandardOutputEncoding=Encoding.GetEncoding(866),
                        RedirectStandardError=true,
                        StandardErrorEncoding=Encoding.GetEncoding(866)
                    }):ImpersonationUtils.RunAsCurrentUser(fileName)
                };
            }

            public void Begin() {
                for(string _response=null; (this.IsValid=!string.IsNullOrEmpty(_response=this._proc.StandardOutput.ReadLine()))&&_response!=Separators.Ready; ) {
                }
            }

            public void End() {
                if(this.IsValid) {
                    this._proc.StandardInput.WriteLine(Separators.End);
                }
            }

            public object Execute(TwainCommand command) {
                if(this.IsValid) {
                    this._proc.StandardInput.WriteLine(Separators.BeginRequest);
                    this._proc.StandardInput.WriteLine(Convert.ToBase64String(command.ToArray()));
                    for(string _response=null; (this.IsValid=!string.IsNullOrEmpty(_response=this._proc.StandardOutput.ReadLine()))&&_response!=Separators.BeginResponse; ) {
                        if(_response==Separators.BeginEvent) {
                            this._OnEvent();
                            continue;
                        }
                    }
                    for(string _data=null; this.IsValid&&(this.IsValid=!string.IsNullOrEmpty(_data=this._proc.StandardOutput.ReadLine())); ) {
                        try {
                            return TwainCommand.FromArray(Convert.FromBase64String(_data)).Result;
                        } catch(Exception ex) {
                            return ex;
                        } finally {
                            for(string _response=null; this.IsValid&&(this.IsValid=!string.IsNullOrEmpty(_response=this._proc.StandardOutput.ReadLine()))&&_response!=Separators.EndResponse; ) {
                            }
                        }
                    }
                    return this.GetException()??new InvalidOperationException();
                }
                throw this.GetException()??new InvalidOperationException();
            }

            public Exception GetException() {
                for(string _response=null; !string.IsNullOrEmpty(_response=this._proc.StandardError.ReadLine()); ) {
                    if(_response==Separators.BeginException) {
                        var _message=string.Empty;
                        for(string _response2=null; !string.IsNullOrEmpty(_response2=this._proc.StandardError.ReadLine())&&_response2!=Separators.EndException; ) {
                            _message+=string.Format("{0}{1}",_response2,Environment.NewLine);
                        }
                        return new InvalidOperationException(_message);
                    }
                }
                return null;
            }

            private void _OnEvent() {
                for(string _event=null; this.IsValid&&(this.IsValid=!string.IsNullOrEmpty(_event=this._proc.StandardOutput.ReadLine())); ) {
                    var _res=TwainCommand.FromArray(Convert.FromBase64String(_event)) as EventHandlerTwainCommand;
                    if(_res!=null&&this.FireEvent!=null) {
                        this.FireEvent(_res);
                    }
                    this._proc.StandardInput.WriteLine(Convert.ToBase64String(_res.ToArray()));
                    break;
                }
                for(string _response=null; this.IsValid&&(this.IsValid=!string.IsNullOrEmpty(_response=this._proc.StandardOutput.ReadLine()))&&_response!=Separators.EndEvent; ) {
                }
            }

            public bool IsValid {
                get;
                private set;
            }

            #region IDisposable

            public void Dispose() {
                if(this._proc!=null) {
                    try {
                        _proc.Kill();
                    } catch {
                    }
                    if(this._proc.StandardInput!=null) {
                        this._proc.StandardInput.Dispose();
                    }
                    if(this._proc.StandardOutput!=null) {
                        this._proc.StandardOutput.Dispose();
                    }
                    if(this._proc.StandardError!=null) {
                        this._proc.StandardError.Dispose();
                    }
                    this._proc.Dispose();
                }
            }

            #endregion

            public event Action<EventHandlerTwainCommand> FireEvent;
        }

        private static class Separators {
            public const string Ready="{DA63824B-9931-4363-A606-6160A2211979}";
            public const string BeginRequest="{ECE0AE56-349A-4318-92D6-F7848362417B}";
            public const string BeginResponse="{7B261FEB-2DA3-43D7-B64C-7ACFD6F931BF}";
            public const string EndResponse="{315748BD-D336-4C1D-AADE-E5EC6D3B11B7}";
            public const string End="{8ACDCA33-EF39-4CEE-B337-CC6CE832832A}";
            public const string BeginException="{78CD0BB5-6F2B-44BC-B16D-60D564386400}";
            public const string EndException="{0FD9554B-6AC7-4A45-8BB0-52C5587DAA21}";
            public const string BeginEvent="{496D8295-FCAD-4236-AD7D-3BEEA7489FB3}";
            public const string EndEvent="{A1EF8BC7-9016-4FF1-962A-8F2C9726342C}";
        }
    }
}
