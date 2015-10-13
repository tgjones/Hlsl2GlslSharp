using System;
using System.Collections.Generic;
using System.Linq;
using Hlsl2GlslSharp.Util;

namespace Hlsl2GlslSharp
{
    public class HlslToGlslConverter : IDisposable
    {
        public HlslToGlslConverter()
        {
            if (NativeMethods.Hlsl2Glsl_Initialize() != 1)
                throw new Exception("Failed to initialize Hlsl2Glsl");
        }

        public ConversionResult Convert(
            ShaderType shaderType, TargetVersion targetVersion, TranslationOptions options,
            string hlslCode, string entryPoint,
            Dictionary<AttributeSemantic, string> overrideAttributeNames = null)
        {
            var compilerPtr = NativeMethods.Hlsl2Glsl_ConstructCompiler(shaderType);

            if (compilerPtr == IntPtr.Zero)
                throw new Exception("Failed to construct Hlsl2Glsl compiler");

            try
            {
                var callbacks = new NativeMethods.Hlsl2Glsl_ParseCallbacks
                {
                    IncludeOpenCallback = null,
                    Data = IntPtr.Zero
                };
                var parseResult = NativeMethods.Hlsl2Glsl_Parse(compilerPtr, hlslCode, targetVersion, ref callbacks, options);
                if (parseResult != 1)
                    throw GetInfoLogAndCreateException(compilerPtr, "Failed to parse HLSL code");

                if (overrideAttributeNames != null)
                {
                    var setUserAttrNamesResult = NativeMethods.Hlsl2Glsl_SetUserAttributeNames(compilerPtr,
                        overrideAttributeNames.Keys.ToArray(), overrideAttributeNames.Values.ToArray(),
                        overrideAttributeNames.Count);
                    if (setUserAttrNamesResult != 1)
                        throw GetInfoLogAndCreateException(compilerPtr, "Failed to user attribute names");
                }

                var translateResult = NativeMethods.Hlsl2Glsl_Translate(compilerPtr, entryPoint, targetVersion, options);
                if (translateResult != 1)
                    throw GetInfoLogAndCreateException(compilerPtr, "Failed to translate HLSL code");

                var glsl = NativeMethods.Hlsl2Glsl_GetShader(compilerPtr);

                var uniforms = NativeMethods.Hlsl2Glsl_GetUniformInfo(compilerPtr);

                return new ConversionResult(glsl, uniforms);
            }
            finally
            {
                NativeMethods.Hlsl2Glsl_DestructCompiler(compilerPtr);
            }
        }

        private Exception GetInfoLogAndCreateException(IntPtr compilerPtr, string prefix)
        {
            var infoLog = NativeMethods.Hlsl2Glsl_GetInfoLog(compilerPtr);
            return new Exception(prefix + ": " + infoLog);
        }

        public void Dispose()
        {
            NativeMethods.Hlsl2Glsl_Shutdown();
        }

        public static bool VersionUsesPrecision(TargetVersion targetVersion)
        {
            return NativeMethods.Hlsl2Glsl_VersionUsesPrecision(targetVersion);
        }
    }
}