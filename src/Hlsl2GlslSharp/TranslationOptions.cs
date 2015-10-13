namespace Hlsl2GlslSharp
{
    /// <summary>
    /// Translation options
    /// </summary>
    public enum TranslationOptions : uint
    {
        None = 0,
        Intermediate = (1 << 0),

        /// Some drivers (e.g. OS X 10.6.x) have bugs with GLSL 1.20 array
        /// initializer syntax. If you need to support this configuration,
        /// use this flag to generate compatible syntax. You'll need
        /// to prepend HLSL2GLSL_ENABLE_ARRAY_120_WORKAROUND to the shader.
        ///
        /// Example of emitted code for a simple array declaration:
        /// (HLSL Source)
        ///		float2 samples[] = {
        ///			float2(-1, 0.1),
        ///			float2(0, 0.5),
        ///			float2(1, 0.1)
        ///		};
        /// (GLSL Emitted result)
        ///		#if defined(HLSL2GLSL_ENABLE_ARRAY_120_WORKAROUND)
        ///			vec2 samples[];
        ///			samples[0] = vec2(-1.0, 0.1);
        ///			samples[1] = vec2(0.0, 0.5);
        ///			samples[2] = vec2(1.0, 0.1);
        ///		#else
        ///			const vec2 samples[] = vec2[](vec2(-1.0, 0.1), vec2(0.0, 0.5), vec2(1.0, 0.1)); 
        ///		#endif
        EmitGLSL120ArrayInitWorkaround = (1 << 1),

        // Instead of using built-in "gl_MultiTexCoord0" for "appdata_t.texcoord : TEXCOORD0"
        //  we will output an attribute "xlat_attrib_TEXCOORD0". Targeting GLSL ES forces this
        //  as there are no built-in attributes in that variant.
        AvoidBuiltinAttribNames = (1 << 2),

        // Always use "gl_MultiTexCoord0" for "TEXCOORD0" and so on,
        // even in GLSL ES. It is expected that client code will add #defines to handle them
        // later on.
        ForceBuiltinAttribNames = (1 << 3),

        // When not using built-in attribute names (due to ETranslateOpAvoidBuiltinAttribNames or GLSL ES),
        //  instead of outputting e.g. "xlat_attrib_TEXCOORD0" for "appdata_t.texcoord : TEXCOORD0"
        //  we will output "appdata_t_texcoord"
        PropogateOriginalAttribNames = (1 << 4),
    }
}
