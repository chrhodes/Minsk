using System;
using System.Linq;

using VNC;

namespace Minsk.CodeAnalysis
{
    public sealed class VariableSymbol
    {
        internal VariableSymbol(string name, Type type)
        {
            Int64 startTicks = Log.CONSTRUCTOR($"Enter name:{name}, type:{type}", Common.LOG_CATEGORY);

            Name = name;
            Type = type;
            Log.CONSTRUCTOR($"Exit", Common.LOG_CATEGORY, startTicks);
        }

        public string Name { get; }
        public Type Type { get; }
    }
}