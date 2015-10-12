namespace Hlsl2GlslSharp
{
    /// <summary>
    /// GLSL shader variable types
    /// </summary>
    internal enum EShType
    {
        EShTypeVoid,
        EShTypeBool,
        EShTypeBVec2,
        EShTypeBVec3,
        EShTypeBVec4,
        EShTypeInt,
        EShTypeIVec2,
        EShTypeIVec3,
        EShTypeIVec4,
        EShTypeFloat,
        EShTypeVec2,
        EShTypeVec3,
        EShTypeVec4,
        EShTypeMat2,
        EShTypeMat2x3,
        EShTypeMat2x4,
        EShTypeMat3x2,
        EShTypeMat3,
        EShTypeMat3x4,
        EShTypeMat4x2,
        EShTypeMat4x3,
        EShTypeMat4x4,
        EShTypeSampler,
        EShTypeSampler1D,
        EShTypeSampler1DShadow,
        EShTypeSampler2D,
        EShTypeSampler2DShadow,
        EShTypeSampler3D,
        EShTypeSamplerCube,
        EShTypeSamplerRect,
        EShTypeSamplerRectShadow,
        EShTypeSampler2DArray,
        EShTypeStruct
    }
}