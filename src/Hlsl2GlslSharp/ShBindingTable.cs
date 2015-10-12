using System.Runtime.InteropServices;

namespace Hlsl2GlslSharp
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct ShBindingTable
    {
        private const int MaxBindings = 1000;

        public int NumBindings;

        /// <summary>
        /// array of bindings
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxBindings)]
        public ShBinding[] Bindings;
    }
}