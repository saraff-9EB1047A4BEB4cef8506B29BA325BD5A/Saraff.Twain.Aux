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
using System.Reflection;

namespace Saraff.Twain.Aux {

#if NETCOREAPP

    internal class TwainCapabilitiesRealProxy : DispatchProxy {
        private readonly Dictionary<MemberInfo, object> _members = new Dictionary<MemberInfo, object>();

        protected override object Invoke(MethodInfo targetMethod, object[] args) {
            if(!this._members.ContainsKey(targetMethod)) {
                var _type = typeof(TwainCapabilityRealProxy<>).MakeGenericType(targetMethod.ReturnType);
                var _inst = Activator.CreateInstance(_type);
                _type.GetProperty(nameof(TwainCapabilityRealProxy<object>.Aux)).SetValue(_inst, this.Aux);
                _type.GetProperty(nameof(TwainCapabilityRealProxy<object>.Member)).SetValue(_inst, targetMethod);

                this._members.Add(targetMethod, _type.GetMethod(nameof(TwainCapabilityRealProxy<object>.GetTransparentProxy), BindingFlags.Instance | BindingFlags.NonPublic).Invoke(_inst, null));
            }
            return this._members[targetMethod];
        }

        internal ITwain32.ITwainCapabilities GetTransparentProxy() {
            var _capabilities = DispatchProxy.Create<ITwain32.ITwainCapabilities, TwainCapabilitiesRealProxy>();
            if(_capabilities as object is TwainCapabilitiesRealProxy _proxy) {
                _proxy.Aux = this.Aux;
            }
            return _capabilities;
        }

        public TwainExternalProcess.AuxProcess Aux { get; set; }
    }

    internal class TwainCapabilityRealProxy<T> : DispatchProxy {

        protected override object Invoke(MethodInfo targetMethod, object[] args) {
            var _result = this.Aux.Execute(new MethodTwainCommand {
                Member = targetMethod,
                Parameters = new IEnumerable<object>[] {
                    new object[] { this.Member.Name },
                    args }.SelectMany(x => x).ToArray()
            });
            if(_result is Exception _ex) {
                if(_ex is TargetInvocationException && _ex.InnerException is TwainException _ex2) {
                    throw _ex2;
                }
                throw new InvalidOperationException(_ex.Message, _ex);
            }
            return _result;
        }

        internal T GetTransparentProxy() {
            var _capability = DispatchProxy.Create<T, TwainCapabilityRealProxy<T>>();
            if(_capability as object is TwainCapabilityRealProxy<T> _proxy) {
                _proxy.Aux = this.Aux;
                _proxy.Member = this.Member;
            }
            return _capability;
        }

        public TwainExternalProcess.AuxProcess Aux { get; set; }

        public MemberInfo Member { get; set; }
    }

#endif

}
