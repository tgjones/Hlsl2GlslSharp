using System;
using Hlsl2GlslSharp.Util;

namespace Hlsl2GlslSharp
{
    public static class HlslToGlslConverter
    {
        public static string Convert(ShaderType shaderType, TargetVersion targetVersion, string hlslCode, string entryPoint)
        {
            if (NativeMethods.Hlsl2Glsl_Initialize() != 1)
                throw new Exception("Failed to initialize Hlsl2Glsl");

            try
            {
                var compilerPtr = NativeMethods.Hlsl2Glsl_ConstructCompiler(shaderType);

                if (compilerPtr == IntPtr.Zero)
                    throw new Exception("Failed to construct Hlsl2Glsl compiler");

                try
                {
                    var callbacks = new NativeMethods.Hlsl2Glsl_ParseCallbacks
                    {
                        IncludeOpenCallback = IncludeOpenCallback,
                        Data = IntPtr.Zero
                    };
                    var parseResult = NativeMethods.Hlsl2Glsl_Parse(compilerPtr, hlslCode, targetVersion, ref callbacks, 0);
                    // TODO: What is parseResult?

                    var translateResult = NativeMethods.Hlsl2Glsl_Translate(compilerPtr, entryPoint, targetVersion, 0);
                    // TODO: What is translateResult?

                    var glsl = NativeMethods.Hlsl2Glsl_GetShader(compilerPtr);

                    return glsl;
                }
                finally
                {
                    NativeMethods.Hlsl2Glsl_DestructCompiler(compilerPtr);
                }
            }
            finally
            {
                NativeMethods.Hlsl2Glsl_Shutdown();
            }
        }

        private static void IncludeOpenCallback(bool isSystem, string fname, string parentfname, out IntPtr output, out IntPtr data)
        {
            throw new NotImplementedException();
        }
    }
}