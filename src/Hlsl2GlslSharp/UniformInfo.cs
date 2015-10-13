using System;
using System.Runtime.InteropServices;

namespace Hlsl2GlslSharp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct UniformInfo
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Semantic;

        [MarshalAs(UnmanagedType.LPStr)]
        public string RegisterSpec;

        public VariableType Type;

        public int ArraySize;

        public IntPtr Init;
    }
}