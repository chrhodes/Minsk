using System;

using VNC;

namespace Minsk.CodeAnalysis
{
    public sealed class VariableSymbol
    {
        internal VariableSymbol(string name, bool isReadOnly, Type type)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter: name:{name}, isReadOnly:{isReadOnly} type:{type}", Common.LOG_CATEGORY);

            Name = name;
            IsReadOnly = isReadOnly;
            Type = type;

            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public string Name { get; }
        public bool IsReadOnly { get; }
        public Type Type { get; }
    }
}