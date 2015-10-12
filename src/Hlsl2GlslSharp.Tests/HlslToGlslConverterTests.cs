using NUnit.Framework;

namespace Hlsl2GlslSharp.Tests
{
    [TestFixture]
    public class HlslToGlslConverterTests
    {
        [Test]
        public void CanConvertVertexShader()
        {
            const string hlslCode = @"
float4 main(float4 position: POSITION) : SV_Position
{
    return position;
}";
            const string expectedGlslCode = @"#version 140

#line 2
vec4 xlat_main( in vec4 position ) {
    #line 4
    return position;
}
void main() {
    vec4 xl_retval;
    xl_retval = xlat_main( vec4(gl_Vertex));
    gl_Position = vec4(xl_retval);
}
";

            var glslCode = HlslToGlslConverter.Convert(ShaderType.VertexShader, TargetVersion.GLSL_140, hlslCode, "main");

            Assert.That(
                glslCode.Replace("\r\n", "\n"), 
                Is.EqualTo(expectedGlslCode.Replace("\r\n", "\n")));
        }
    }
}