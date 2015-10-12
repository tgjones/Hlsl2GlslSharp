using System.Runtime.InteropServices;

namespace Hlsl2GlslSharp
{
    /// <summary>
    /// Uniform info struct
    /// </summary>
    internal struct ShUniformInfo
    {
        private const int MaxInit = 10;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        [MarshalAs(UnmanagedType.LPStr)]
        public string Semantic;

        [MarshalAs(UnmanagedType.LPStr)]
        public string RegisterSpec;

        public EShType Type;

        public int ArraySize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MaxInit)]
        public float[] Init;
    }
}