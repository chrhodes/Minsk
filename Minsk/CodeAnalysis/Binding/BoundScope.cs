using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using VNC;

namespace Minsk.CodeAnalysis.Binding
{
    internal sealed class BoundScope
    {
        private Dictionary<string, VariableSymbol> _variables = new Dictionary<string, VariableSymbol>();

        public BoundScope Parent { get; }

        public BoundScope(BoundScope parent)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter", Common.LOG_CATEGORY);

            Parent = parent;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public bool TryDeclare(VariableSymbol variable)
        {
            Int64 startTicks = Log.BINDER($"Enter", Common.LOG_CATEGORY);

            // var x = 10
            // {
            //    var x = false
            // }

            if (_variables.ContainsKey(variable.Name))
            {
                Log.BINDER($"Exit false", Common.LOG_CATEGORY, startTicks);

                return false;
            }

            _variables.Add(variable.Name, variable);

            Log.BINDER($"Exit true", Common.LOG_CATEGORY, startTicks);

            return true;
        }

        public bool TryLookup(string name, out VariableSymbol variable)
        {
            Int64 startTicks = Log.BINDER($"Enter", Common.LOG_CATEGORY);

            if (_variables.TryGetValue(name, out variable))
            {
                Log.BINDER($"Exit true", Common.LOG_CATEGORY, startTicks);

                return true;
            }

            if (Parent == null)
            {
                Log.BINDER($"Exit false", Common.LOG_CATEGORY, startTicks);

                return false;
            }

            Log.BINDER($"Exit Parent.TryLookup({name})", Common.LOG_CATEGORY, startTicks);

            return Parent.TryLookup(name, out variable);
        }

        public ImmutableArray<VariableSymbol> GetDeclaredVariables()
        {
            Int64 startTicks = Log.BINDER($"Enter", Common.LOG_CATEGORY);

            Log.BINDER($"Exit", Common.LOG_CATEGORY, startTicks);

            return _variables.Values.ToImmutableArray();
        }
    }
}
