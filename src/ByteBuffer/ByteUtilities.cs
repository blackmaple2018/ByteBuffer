using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace ByteBuffer
{
    public static unsafe class ByteUtilities
    {
        public static readonly MemSetType MemSet = CreateMemset();
        public static readonly MemCopyType MemCopy = CreateMemCopy();
        //----------------------------------------------------------------------------------------------------
        public delegate void MemSetType(void* array, byte value, int count);
        public delegate void MemCopyType(void* dst, void* src, int count);
        //----------------------------------------------------------------------------------------------------
        private static MemSetType CreateMemset()
        {
            var opcodes = new[]
            {
                OpCodes.Ldarg_0,    // address
                OpCodes.Ldarg_1,    // initialization value
                OpCodes.Ldarg_2,    // number of bytes
                OpCodes.Initblk,    // memset
                OpCodes.Ret         // return
            };

            return CreateMethod<MemSetType>("memset", opcodes);
        }
        private static MemCopyType CreateMemCopy()
        {
            var opcodes = new[]
            {
                OpCodes.Ldarg_0,    // dest address
                OpCodes.Ldarg_1,    // src address
                OpCodes.Ldarg_2,    // number of bytes
                OpCodes.Cpblk,      // memcpy
                OpCodes.Ret         // return
            };

            return CreateMethod<MemCopyType>("memcopy", opcodes);
        }
        //----------------------------------------------------------------------------------------------------
        private static TType CreateMethod<TType>(string name, OpCode[] opCodes) where TType : class
        {
            var targetType = typeof(TType);
            
            //if (!targetType.IsSubclassOf(typeof(Delegate))) { throw new InvalidOperationException(); }

            var attributes = MethodAttributes.Public | MethodAttributes.Static;
            var convention = CallingConventions.Standard;
            var returnType = typeof(void);
            var paramTypes = targetType.GetMethod("Invoke").GetParameters().Select(p => p.ParameterType).ToArray();
            var owner = typeof(ByteUtilities);

            var method = new DynamicMethod(name, attributes, convention, returnType, paramTypes, owner, true);
            var il = method.GetILGenerator();

            foreach (var op in opCodes) { il.Emit(op); }
            
            return method.CreateDelegate(targetType) as TType;
        }
    }
}
