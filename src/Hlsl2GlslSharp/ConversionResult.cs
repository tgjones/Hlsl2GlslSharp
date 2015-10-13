namespace Hlsl2GlslSharp
{
    public class ConversionResult
    {
        public string GlslCode { get; }
        public UniformInfo[] Uniforms { get; }

        internal ConversionResult(string glslCode, UniformInfo[] uniforms)
        {
            GlslCode = glslCode;
            Uniforms = uniforms;
        }
    }
}