using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Hlsl2GlslSharp.Util
{
    internal static class NativeMethods
    {
        private const string DllName = "hlslang.dll";

        /// Initialize the HLSL2GLSL translator.  This function must be called once prior to calling any other
        /// HLSL2GLSL translator functions
        /// \return
        ///   1 on success, 0 on failure
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Hlsl2Glsl_Initialize();

        /// Shutdown the HLSL2GLSL translator.  This function should be called to de-initialize the HLSL2GLSL
        /// translator and should only be called once on shutdown.
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Hlsl2Glsl_Shutdown();

        /// Construct a compiler for the given language (one per shader)
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Hlsl2Glsl_ConstructCompiler(ShaderType language);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Hlsl2Glsl_DestructCompiler(IntPtr handle);

        /// File read callback for #include processing.
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Hlsl2Glsl_IncludeOpenFunc(bool isSystem, 
            [MarshalAs(UnmanagedType.LPStr)] string fname, 
            [MarshalAs(UnmanagedType.LPStr)] string parentfname,
            [Out] out IntPtr output,
            [Out] out int outputLength,
            [Out] out IntPtr data);

        [StructLayout(LayoutKind.Sequential)]
        public struct Hlsl2Glsl_ParseCallbacks
        {
            [MarshalAs(UnmanagedType.FunctionPtr)]
            public Hlsl2Glsl_IncludeOpenFunc IncludeOpenCallback;

            public IntPtr Data;
        };

        /// Parse HLSL shader to prepare it for final translation.
        /// \param callbacks
        ///		File read callback for #include processing. If NULL is passed, then #include directives will result in error.
        /// \param options
        ///		Flags of TTranslateOptions
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Hlsl2Glsl_Parse(
            IntPtr handle,
            [MarshalAs(UnmanagedType.LPStr)] string shaderString,
            TargetVersion targetVersion,
            ref Hlsl2Glsl_ParseCallbacks callbacks,
            TranslationOptions options);

        /// After parsing a HLSL shader, do the final translation to GLSL.
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Hlsl2Glsl_Translate(
            IntPtr handle, 
            [MarshalAs(UnmanagedType.LPStr)] string entry, 
            TargetVersion targetVersion, 
            TranslationOptions options);

        /// After translating HLSL shader(s), retrieve the translated GLSL source.
        [DllImport(DllName, EntryPoint = "Hlsl2Glsl_GetShader", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Hlsl2Glsl_GetShader_Impl(IntPtr handle);

        public static string Hlsl2Glsl_GetShader(IntPtr handle)
        {
            var result = Hlsl2Glsl_GetShader_Impl(handle);
            return Marshal.PtrToStringAnsi(result);
        }

        [DllImport(DllName, EntryPoint = "Hlsl2Glsl_GetInfoLog", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Hlsl2Glsl_GetInfoLog_Impl(IntPtr handle);

        public static string Hlsl2Glsl_GetInfoLog(IntPtr handle)
        {
            var result = Hlsl2Glsl_GetInfoLog_Impl(handle);
            return Marshal.PtrToStringAnsi(result);
        }

        /// After translating, retrieve the number of uniforms
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Hlsl2Glsl_GetUniformCount(IntPtr handle);

        /// After translating, retrieve the uniform info table
        [DllImport(DllName, EntryPoint = "Hlsl2Glsl_GetUniformInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Hlsl2Glsl_GetUniformInfo_Impl(IntPtr handle);

        public static UniformInfo[] Hlsl2Glsl_GetUniformInfo(IntPtr handle)
        {
            var numUniforms = Hlsl2Glsl_GetUniformCount(handle);
            var uniformInfosPtr = Hlsl2Glsl_GetUniformInfo_Impl(handle);
            var structSize = Marshal.SizeOf(typeof(UniformInfo));

            var result = new UniformInfo[numUniforms];
            for (int i = 0; i < numUniforms; ++i)
            {
                var data = new IntPtr(uniformInfosPtr.ToInt32() + structSize * i);
                result[i] = (UniformInfo) Marshal.PtrToStructure(data, typeof (UniformInfo));
            }

            return result;
        }

        /// Instead of mapping HLSL attributes to GLSL fixed-function attributes, this function can be used to 
        /// override the  attribute mapping.  This tells the code generator to use user-defined attributes for 
        /// the semantics that are specified.
        ///
        /// \param handle
        ///      Handle to the compiler.  This should be called BEFORE calling Hlsl2Glsl_Translate
        /// \param pSemanticEnums 
        ///      Array of semantic enums to set
        /// \param pSemanticNames 
        ///      Array of user attribute names to use
        /// \param nNumSemantics 
        ///      Number of semantics to set in the arrays
        /// \return
        ///      1 on success, 0 on failure
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Hlsl2Glsl_SetUserAttributeNames(IntPtr handle,
            AttributeSemantic[] pSemanticEnums,
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] pSemanticNames,
            int nNumSemantics);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Hlsl2Glsl_VersionUsesPrecision(TargetVersion version);
    }
}