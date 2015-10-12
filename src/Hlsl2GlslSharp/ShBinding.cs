using System.Runtime.InteropServices;

namespace Hlsl2GlslSharp
{
    /// <summary>
    /// Binding table.  This can be used for locating attributes, uniforms, globals, etc., as needed.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ShBinding
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string Name;

        public int Binding;
    }
}