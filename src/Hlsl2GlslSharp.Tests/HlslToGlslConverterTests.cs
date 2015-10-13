using System.Collections.Generic;
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
float4x4 World : WORLD;
float4x4 ViewProjection;
float3 CameraPosition : register(c0) = float3(1, 0, 1);
float MyArray[2];
float4 main(float4 position: POSITION) : SV_Position
{
    return float4(CameraPosition, MyArray[0]) + mul(mul(position, World), ViewProjection);
}";
            const string expectedGlslCode = @"#version 140
uniform mat4 World;
#line 3
uniform mat4 ViewProjection;
uniform vec3 CameraPosition = vec3( 1.0, 0.0, 1.0);
uniform float MyArray[2];
#line 6
vec4 xlat_main( in vec4 position ) {
    #line 8
    return (vec4( CameraPosition, MyArray[0]) + ((position * World) * ViewProjection));
}
void main() {
    vec4 xl_retval;
    xl_retval = xlat_main( vec4(gl_CustomPosition));
    gl_Position = vec4(xl_retval);
}
";
            var overrideAttributeNames = new Dictionary<AttributeSemantic, string>
            {
                { AttributeSemantic.Position, "gl_CustomPosition" }
            };

            ConversionResult result;
            using (var converter = new HlslToGlslConverter())
                result = converter.Convert(ShaderType.VertexShader, TargetVersion.GLSL_140, TranslationOptions.None, hlslCode, "main", overrideAttributeNames);

            Assert.That(
                result.GlslCode.Replace("\r\n", "\n"), 
                Is.EqualTo(expectedGlslCode.Replace("\r\n", "\n")));

            Assert.That(result.Uniforms, Has.Length.EqualTo(4));

            Assert.That(result.Uniforms[0].ArraySize, Is.EqualTo(0));
            Assert.That(result.Uniforms[0].Name, Is.EqualTo("CameraPosition"));
            Assert.That(result.Uniforms[0].RegisterSpec, Is.EqualTo("c0"));
            Assert.That(result.Uniforms[0].Semantic, Is.EqualTo(null));
            Assert.That(result.Uniforms[0].Type, Is.EqualTo(VariableType.Vec3));
            //Assert.That(result.Uniforms[2].Init, Has.Length.EqualTo(1));
            //Assert.That(result.Uniforms[2].Init[0], Is.EqualTo(1.0f));

            Assert.That(result.Uniforms[1].ArraySize, Is.EqualTo(2));
            Assert.That(result.Uniforms[1].Name, Is.EqualTo("MyArray"));
            Assert.That(result.Uniforms[1].RegisterSpec, Is.EqualTo(null));
            Assert.That(result.Uniforms[1].Semantic, Is.EqualTo(null));
            Assert.That(result.Uniforms[1].Type, Is.EqualTo(VariableType.Float));

            Assert.That(result.Uniforms[2].ArraySize, Is.EqualTo(0));
            Assert.That(result.Uniforms[2].Name, Is.EqualTo("ViewProjection"));
            Assert.That(result.Uniforms[2].RegisterSpec, Is.EqualTo(null));
            Assert.That(result.Uniforms[2].Semantic, Is.EqualTo(null));
            Assert.That(result.Uniforms[2].Type, Is.EqualTo(VariableType.Mat4x4));
            //Assert.That(result.Uniforms[1].Init, Has.Length.EqualTo(0));

            Assert.That(result.Uniforms[3].ArraySize, Is.EqualTo(0));
            Assert.That(result.Uniforms[3].Name, Is.EqualTo("World"));
            Assert.That(result.Uniforms[3].RegisterSpec, Is.EqualTo(null));
            Assert.That(result.Uniforms[3].Semantic, Is.EqualTo("WORLD"));
            Assert.That(result.Uniforms[3].Type, Is.EqualTo(VariableType.Mat4x4));
            //Assert.That(result.Uniforms[2].Init, Has.Length.EqualTo(0));
        }

        [TestCase(TargetVersion.GLSL_ES_100, true)]
        [TestCase(TargetVersion.GLSL_110, false)]
        public void TestVersionUsesPrecision(TargetVersion targetVersion, bool expectedResult)
        {
            Assert.That(HlslToGlslConverter.VersionUsesPrecision(targetVersion), Is.EqualTo(expectedResult));
        }
    }
}