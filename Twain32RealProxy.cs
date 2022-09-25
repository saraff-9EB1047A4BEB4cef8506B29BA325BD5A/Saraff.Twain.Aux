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
#if !NETCOREAPP
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
#endif
using System.Security.Permissions;
using System.Reflection;

namespace Saraff.Twain.Aux {

#if !NETCOREAPP
    internal sealed class Twain32RealProxy : RealProxy {
        private TwainExternalProcess.AuxProcess _aux;
        private TwainCapabilities _capabilities;
        private Twain32.TwainPalette _palette;

        internal Twain32RealProxy(TwainExternalProcess.AuxProcess aux) : base(typeof(Twain32)) {
            this._aux = aux;
            this._aux.FireEvent += this._FireEvent;
        }

        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override IMessage Invoke(IMessage msg) {
            var _msg = msg as IMethodCallMessage;
            try {
                var _args = _msg.Args;

    #region SpecialName

                if(_msg.MethodBase.IsSpecialName) {
                    var _name = _msg.MethodBase.Name.Split(new string[] { "_" }, 2, StringSplitOptions.None);
                    switch(_name[0]) {
                        case "get":
                            switch(_name[1]) {
                                case "Capabilities":
                                    return new ReturnMessage(this.Capabilities, _args, 0, _msg.LogicalCallContext, _msg);
                                case "Palette":
                                    return new ReturnMessage(this.Palette, _args, 0, _msg.LogicalCallContext, _msg);
                            }
                            break;
                        case "add":
                            switch(_name[1]) {
                                case Twain32Events.AcquireCompleted:
                                    this.AcquireCompleted += _args[0] as EventHandler;
                                    break;
                                case Twain32Events.AcquireError:
                                    this.AcquireError += _args[0] as EventHandler<Twain32.AcquireErrorEventArgs>;
                                    break;
                                case Twain32Events.XferDone:
                                    this.XferDone += _args[0] as EventHandler<Twain32.XferDoneEventArgs>;
                                    break;
                                case Twain32Events.EndXfer:
                                    this.EndXfer += _args[0] as EventHandler<Twain32.EndXferEventArgs>;
                                    break;
                                case Twain32Events.SetupMemXferEvent:
                                    this.SetupMemXferEvent += _args[0] as EventHandler<Twain32.SetupMemXferEventArgs>;
                                    break;
                                case Twain32Events.MemXferEvent:
                                    this.MemXferEvent += _args[0] as EventHandler<Twain32.MemXferEventArgs>;
                                    break;
                                case Twain32Events.SetupFileXferEvent:
                                    this.SetupFileXferEvent += _args[0] as EventHandler<Twain32.SetupFileXferEventArgs>;
                                    break;
                                case Twain32Events.FileXferEvent:
                                    this.FileXferEvent += _args[0] as EventHandler<Twain32.FileXferEventArgs>;
                                    break;
                                case Twain32Events.TwainStateChanged:
                                    this.TwainStateChanged += _args[0] as EventHandler<Twain32.TwainStateEventArgs>;
                                    break;
                                default:
                                    return new ReturnMessage(new NotImplementedException(), _msg);
                            }
                            return new ReturnMessage(null, _args, 0, _msg.LogicalCallContext, _msg);
                        case "remove":
                            switch(_name[1]) {
                                case Twain32Events.AcquireCompleted:
                                    this.AcquireCompleted -= _args[0] as EventHandler;
                                    break;
                                case Twain32Events.AcquireError:
                                    this.AcquireError -= _args[0] as EventHandler<Twain32.AcquireErrorEventArgs>;
                                    break;
                                case Twain32Events.XferDone:
                                    this.XferDone -= _args[0] as EventHandler<Twain32.XferDoneEventArgs>;
                                    break;
                                case Twain32Events.EndXfer:
                                    this.EndXfer -= _args[0] as EventHandler<Twain32.EndXferEventArgs>;
                                    break;
                                case Twain32Events.SetupMemXferEvent:
                                    this.SetupMemXferEvent -= _args[0] as EventHandler<Twain32.SetupMemXferEventArgs>;
                                    break;
                                case Twain32Events.MemXferEvent:
                                    this.MemXferEvent -= _args[0] as EventHandler<Twain32.MemXferEventArgs>;
                                    break;
                                case Twain32Events.SetupFileXferEvent:
                                    this.SetupFileXferEvent -= _args[0] as EventHandler<Twain32.SetupFileXferEventArgs>;
                                    break;
                                case Twain32Events.FileXferEvent:
                                    this.FileXferEvent -= _args[0] as EventHandler<Twain32.FileXferEventArgs>;
                                    break;
                                case Twain32Events.TwainStateChanged:
                                    this.TwainStateChanged -= _args[0] as EventHandler<Twain32.TwainStateEventArgs>;
                                    break;
                                default:
                                    return new ReturnMessage(new NotImplementedException(), _msg);
                            }
                            return new ReturnMessage(null, _args, 0, _msg.LogicalCallContext, _msg);
                    }
                }

    #endregion

                TwainCommand _command;
                var _result = this._aux.Execute(_command = new MethodTwainCommand { Member = _msg.MethodBase, Parameters = _args });
                for(Exception _ex = _result as Exception, _ex2 = _result as TargetInvocationException; _ex != null;) {
                    return new ReturnMessage(_ex2 != null ? _ex2.InnerException : _ex, _msg);
                }
                return new ReturnMessage(_result, _args, 0, _msg.LogicalCallContext, _msg);
            } catch(Exception ex) {
                return new ReturnMessage(ex, _msg);
            }
        }

        private void _FireEvent(EventHandlerTwainCommand obj) {
            switch(obj.Member.Name) {
                case Twain32Events.AcquireCompleted:
                    for(var _args = obj.Args; this.AcquireCompleted != null && _args != null;) {
                        this.AcquireCompleted(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.AcquireError:
                    for(var _args = obj.Args as Twain32.AcquireErrorEventArgs; this.AcquireError != null && _args != null;) {
                        this.AcquireError(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.XferDone:
                    for(var _args = obj.Args as Twain32.SerializableCancelEventArgs; this.XferDone != null && _args != null;) {
                        var _info = Delegate.CreateDelegate(typeof(Twain32).GetNestedType("GetImageInfoCallback", BindingFlags.NonPublic), this.GetTransparentProxy(), typeof(Twain32).GetMethod("_GetImageInfo", BindingFlags.Instance | BindingFlags.NonPublic));
                        var _extInfo = Delegate.CreateDelegate(typeof(Twain32).GetNestedType("GetExtImageInfoCallback", BindingFlags.NonPublic), this.GetTransparentProxy(), typeof(Twain32).GetMethod("_GetExtImageInfo", BindingFlags.Instance | BindingFlags.NonPublic));
                        var _args2 = Twain32RealProxy._CreateInstance<Twain32.XferDoneEventArgs>(_info, _extInfo);
                        this.XferDone(this, _args2);
                        _args.Cancel = _args2.Cancel;
                        break;
                    }
                    break;
                case Twain32Events.EndXfer:
                    for(var _args = obj.Args as Twain32.EndXferEventArgs; this.EndXfer != null && _args != null;) {
                        this.EndXfer(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.SetupMemXferEvent:
                    for(var _args = obj.Args as Twain32.SetupMemXferEventArgs; this.SetupMemXferEvent != null && _args != null;) {
                        this.SetupMemXferEvent(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.MemXferEvent:
                    for(var _args = obj.Args as Twain32.MemXferEventArgs; this.MemXferEvent != null && _args != null;) {
                        this.MemXferEvent(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.SetupFileXferEvent:
                    for(var _args = obj.Args as Twain32.SetupFileXferEventArgs; this.SetupFileXferEvent != null && _args != null;) {
                        this.SetupFileXferEvent(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.FileXferEvent:
                    for(var _args = obj.Args as Twain32.FileXferEventArgs; this.FileXferEvent != null && _args != null;) {
                        this.FileXferEvent(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.TwainStateChanged:
                    for(var _args = obj.Args as Twain32.TwainStateEventArgs; this.TwainStateChanged != null && _args != null;) {
                        this.TwainStateChanged(this, _args);
                        break;
                    }
                    break;
            }
        }

        private static T _CreateInstance<T>(params object[] args) where T : class {
            return Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic, null, args, null) as T;
        }

    #region Twain32 Properties

        private TwainCapabilities Capabilities {
            get {
                if(this._capabilities == null) {
                    this._capabilities = Twain32RealProxy._CreateInstance<TwainCapabilities>(this.GetTransparentProxy());
                }
                return this._capabilities;
            }
        }

        private Twain32.TwainPalette Palette {
            get {
                if(this._palette == null) {
                    this._palette = new TwainPaletteRealProxy(this._aux).GetTransparentProxy() as Twain32.TwainPalette;
                }
                return this._palette;
            }
        }

    #endregion

    #region Twain32 Events

        private event EventHandler AcquireCompleted;

        private event EventHandler<Twain32.AcquireErrorEventArgs> AcquireError;

        private event EventHandler<Twain32.XferDoneEventArgs> XferDone;

        private event EventHandler<Twain32.EndXferEventArgs> EndXfer;

        private event EventHandler<Twain32.SetupMemXferEventArgs> SetupMemXferEvent;

        private event EventHandler<Twain32.MemXferEventArgs> MemXferEvent;

        private event EventHandler<Twain32.SetupFileXferEventArgs> SetupFileXferEvent;

        private event EventHandler<Twain32.FileXferEventArgs> FileXferEvent;

        private event EventHandler<Twain32.TwainStateEventArgs> TwainStateChanged;

    #endregion
    }
#else
    internal class Twain32RealProxy : DispatchProxy {
        private ITwain32.ITwainCapabilities _capabilities;
        private ITwain32.ITwainPalette _palette;
        private TwainExternalProcess.AuxProcess _aux;

        protected override object Invoke(MethodInfo targetMethod, object[] args) {

            #region SpecialName

            if(targetMethod.IsSpecialName) {
                var _name = targetMethod.Name.Split(new string[] { "_" }, 2, StringSplitOptions.None);
                switch(_name[0]) {
                    case "get":
                        switch(_name[1]) {
                            case "Capabilities":
                                return this.Capabilities;
                            case "Palette":
                                return this.Palette;
                        }
                        break;
                    case "add":
                        switch(_name[1]) {
                            case Twain32Events.AcquireCompleted:
                                this.AcquireCompleted += args[0] as EventHandler;
                                break;
                            case Twain32Events.AcquireError:
                                this.AcquireError += args[0] as EventHandler<Twain32.AcquireErrorEventArgs>;
                                break;
                            case Twain32Events.XferDone:
                                this.XferDone += args[0] as EventHandler<Twain32.XferDoneEventArgs>;
                                break;
                            case Twain32Events.EndXfer:
                                this.EndXfer += args[0] as EventHandler<Twain32.EndXferEventArgs>;
                                break;
                            case Twain32Events.SetupMemXferEvent:
                                this.SetupMemXferEvent += args[0] as EventHandler<Twain32.SetupMemXferEventArgs>;
                                break;
                            case Twain32Events.MemXferEvent:
                                this.MemXferEvent += args[0] as EventHandler<Twain32.MemXferEventArgs>;
                                break;
                            case Twain32Events.SetupFileXferEvent:
                                this.SetupFileXferEvent += args[0] as EventHandler<Twain32.SetupFileXferEventArgs>;
                                break;
                            case Twain32Events.FileXferEvent:
                                this.FileXferEvent += args[0] as EventHandler<Twain32.FileXferEventArgs>;
                                break;
                            case Twain32Events.TwainStateChanged:
                                this.TwainStateChanged += args[0] as EventHandler<Twain32.TwainStateEventArgs>;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        return null;
                    case "remove":
                        switch(_name[1]) {
                            case Twain32Events.AcquireCompleted:
                                this.AcquireCompleted -= args[0] as EventHandler;
                                break;
                            case Twain32Events.AcquireError:
                                this.AcquireError -= args[0] as EventHandler<Twain32.AcquireErrorEventArgs>;
                                break;
                            case Twain32Events.XferDone:
                                this.XferDone -= args[0] as EventHandler<Twain32.XferDoneEventArgs>;
                                break;
                            case Twain32Events.EndXfer:
                                this.EndXfer -= args[0] as EventHandler<Twain32.EndXferEventArgs>;
                                break;
                            case Twain32Events.SetupMemXferEvent:
                                this.SetupMemXferEvent -= args[0] as EventHandler<Twain32.SetupMemXferEventArgs>;
                                break;
                            case Twain32Events.MemXferEvent:
                                this.MemXferEvent -= args[0] as EventHandler<Twain32.MemXferEventArgs>;
                                break;
                            case Twain32Events.SetupFileXferEvent:
                                this.SetupFileXferEvent -= args[0] as EventHandler<Twain32.SetupFileXferEventArgs>;
                                break;
                            case Twain32Events.FileXferEvent:
                                this.FileXferEvent -= args[0] as EventHandler<Twain32.FileXferEventArgs>;
                                break;
                            case Twain32Events.TwainStateChanged:
                                this.TwainStateChanged -= args[0] as EventHandler<Twain32.TwainStateEventArgs>;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                        return null;
                }
            }

            #endregion

            var _result = this._aux.Execute(new MethodTwainCommand { Member = targetMethod, Parameters = args });
            if(_result is Exception _ex) {
                if(_ex is TargetInvocationException && _ex.InnerException is TwainException _ex2) {
                    throw _ex2;
                }
                throw new InvalidOperationException(_ex.Message, _ex);
            }
            return _result;
        }

        internal ITwain32 GetTransparentProxy() {
            var _twain = DispatchProxy.Create<ITwain32, Twain32RealProxy>();
            if(_twain as object is Twain32RealProxy _proxy) {
                _proxy.Aux = this.Aux;
            }
            return _twain;
        }

        public TwainExternalProcess.AuxProcess Aux {
            get => this._aux;
            set {
                if(this._aux != null) {
                    this._aux.FireEvent -= this._FireEvent;
                }
                this._aux = value;
                if(this._aux != null) {
                    this._aux.FireEvent += this._FireEvent;
                }
            }
        }

        private void _FireEvent(EventHandlerTwainCommand obj) {
            switch(obj.Member.Name) {
                case Twain32Events.AcquireCompleted:
                    for(var _args = obj.Args; this.AcquireCompleted != null && _args != null;) {
                        this.AcquireCompleted(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.AcquireError:
                    for(var _args = obj.Args as Twain32.AcquireErrorEventArgs; this.AcquireError != null && _args != null;) {
                        this.AcquireError(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.XferDone:
                    for(var _args = obj.Args as Twain32.SerializableCancelEventArgs; this.XferDone != null && _args != null;) {
                        var _info = Delegate.CreateDelegate(typeof(Twain32).GetNestedType("GetImageInfoCallback", BindingFlags.NonPublic), this.GetTransparentProxy(), typeof(Twain32).GetMethod("_GetImageInfo", BindingFlags.Instance | BindingFlags.NonPublic));
                        var _extInfo = Delegate.CreateDelegate(typeof(Twain32).GetNestedType("GetExtImageInfoCallback", BindingFlags.NonPublic), this.GetTransparentProxy(), typeof(Twain32).GetMethod("_GetExtImageInfo", BindingFlags.Instance | BindingFlags.NonPublic));
                        var _args2 = Twain32RealProxy._CreateInstance<Twain32.XferDoneEventArgs>(_info, _extInfo);
                        this.XferDone(this, _args2);
                        _args.Cancel = _args2.Cancel;
                        break;
                    }
                    break;
                case Twain32Events.EndXfer:
                    for(var _args = obj.Args as Twain32.EndXferEventArgs; this.EndXfer != null && _args != null;) {
                        this.EndXfer(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.SetupMemXferEvent:
                    for(var _args = obj.Args as Twain32.SetupMemXferEventArgs; this.SetupMemXferEvent != null && _args != null;) {
                        this.SetupMemXferEvent(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.MemXferEvent:
                    for(var _args = obj.Args as Twain32.MemXferEventArgs; this.MemXferEvent != null && _args != null;) {
                        this.MemXferEvent(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.SetupFileXferEvent:
                    for(var _args = obj.Args as Twain32.SetupFileXferEventArgs; this.SetupFileXferEvent != null && _args != null;) {
                        this.SetupFileXferEvent(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.FileXferEvent:
                    for(var _args = obj.Args as Twain32.FileXferEventArgs; this.FileXferEvent != null && _args != null;) {
                        this.FileXferEvent(this, _args);
                        break;
                    }
                    break;
                case Twain32Events.TwainStateChanged:
                    for(var _args = obj.Args as Twain32.TwainStateEventArgs; this.TwainStateChanged != null && _args != null;) {
                        this.TwainStateChanged(this, _args);
                        break;
                    }
                    break;
            }
        }

        private static T _CreateInstance<T>(params object[] args) where T : class => Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic, null, args, null) as T;

        #region Twain32 Properties

        private ITwain32.ITwainCapabilities Capabilities => this._capabilities ??= new TwainCapabilitiesRealProxy { Aux = this.Aux }.GetTransparentProxy();

        private ITwain32.ITwainPalette Palette => this._palette ??= new TwainPaletteRealProxy { Aux = this.Aux }.GetTransparentProxy();

        #endregion

        #region Twain32 Events

        private event EventHandler AcquireCompleted;

        private event EventHandler<Twain32.AcquireErrorEventArgs> AcquireError;

        private event EventHandler<Twain32.XferDoneEventArgs> XferDone;

        private event EventHandler<Twain32.EndXferEventArgs> EndXfer;

        private event EventHandler<Twain32.SetupMemXferEventArgs> SetupMemXferEvent;

        private event EventHandler<Twain32.MemXferEventArgs> MemXferEvent;

        private event EventHandler<Twain32.SetupFileXferEventArgs> SetupFileXferEvent;

        private event EventHandler<Twain32.FileXferEventArgs> FileXferEvent;

        private event EventHandler<Twain32.TwainStateEventArgs> TwainStateChanged;

        #endregion
    }
#endif
    internal sealed class Twain32Events {
        public const string AcquireCompleted = "AcquireCompleted";
        public const string AcquireError = "AcquireError";
        public const string XferDone = "XferDone";
        public const string EndXfer = "EndXfer";
        public const string SetupMemXferEvent = "SetupMemXferEvent";
        public const string MemXferEvent = "MemXferEvent";
        public const string SetupFileXferEvent = "SetupFileXferEvent";
        public const string FileXferEvent = "FileXferEvent";
        public const string TwainStateChanged = "TwainStateChanged";
        public const string DeviceEvent = "DeviceEvent";
    }
}
