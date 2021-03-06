﻿extern alias PIPE;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBT = SoulsFormats.FLVER.LayoutType;
using MBS = SoulsFormats.FLVER.LayoutSemantic;
using PIPE::Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using PIPE::Microsoft.Xna.Framework.Content.Pipeline;

namespace FBX2FLVER
{
    public class FBX2FLVERImportJobConfig
    {

        public string FBXPath { get; set; } = "";

        public string MTDBNDPath { get; set; } = "";

        public FBX2FLVERMaterialLibrary MaterialLibrary = new FBX2FLVERMaterialLibrary();

        public void LoadMTDBND(string path, bool isBND4)
        {
            MaterialLibrary.LoadMTDBND(path, isBND4);
        }

        public string OutputFlverPath { get; set; } = null;
        public string OutputTpfPath { get; set; } = null;   // Points to a folder now

        public double Scale { get; set; } = 1.0;

        public float SceneRotationY { get; set; } = 0;
        public float SceneRotationZ { get; set; } = 0;
        public float SceneRotationX { get; set; } = 0;

        public Microsoft.Xna.Framework.Vector3 SceneRotation
            => new Microsoft.Xna.Framework.Vector3(SceneRotationX, SceneRotationY, SceneRotationZ);

        public Microsoft.Xna.Framework.Vector3 SkeletonRotation = Microsoft.Xna.Framework.Vector3.Zero;

        public string ImportSkeletonFromFLVER = null;

        public bool ImportSkeletonFromFLVER_ImportDummies = false;

        public bool IsDoubleSided { get; set; } = true;
        public bool GenerateBackup { get; set; } = true;

        public bool RotateNormalsBackward { get; set; } = false;
        public bool ConvertNormalsAxis { get; set; } = false;

        public bool UseAbsoluteVertPositions { get; set; } = true;

        public SoulsFormats.FLVER2.BufferLayout BufferLayout = null; /* DEPRECATED :: Use FBX2FLVERLayoutHelper.getLayout */

        public bool GenerateLodAndMotionBlur = false;

        

        public string PlaceholderMaterialShaderName = "P_Metal[DSB]";
        public string PlaceholderMaterialName = "FBX2FLVER_Material";

        public string FakeTextureDirectory = "";

        public bool OnlyCreate1BufferLayout = false;

        public bool IsStatic = false;

        public float NormalWValue = 0;

        public float UVScaleX = 1;
        public float UVScaleY = 1;

        public bool UseDirectBoneIndices = false;

        public byte Unk0x5CValue = 0;
        public int Unk0x68Value = 0;

        public Action<SoulsFormats.FLVER2, SoulsFormats.TPF> BeforeSaveAction = null;


        // Gundam Unicorn: 0x20005, 0x2000E
        // DS1: 0x2000C, 0x2000D
        // DS2: 0x20009, 0x20010
        // SFS: 0x20010
        // BB:  0x20013, 0x20014
        // DS3: 0x20013, 0x20014
        // SDT: 0x2001A, 0x20016 (test chr)
        public int FlverVersion = 0x2000C;

        public byte TpfEncoding = 2;
        public byte TpfFlag2 = 3;

        public Dictionary<string, string> FBXMaterialChannelMap = new Dictionary<string, string>() /* DEPRECATED :: Use FBX2FLVERLayoutHelper.getTextureMap */
        {
            { "Texture", "g_Diffuse" },
            { "Specular", "g_Specular" },
            { "NormalMap", "g_Bumpmap" },
        };

        public Dictionary<string, string> MTDHardcodedTextureChannels = new Dictionary<string, string>() /* DEPRECATED :: Use FBX2FLVERLayoutHelper.getHardcodedTextureMap */
        {
            { "g_DetailBumpmap", "" },
        };

        public enum FlverGamePreset
        {
            DS1Static,
            DS1Skinned,
            /*DS2Static,
            DS2Skinned,
            DS3Static,
            DS3Skinned,*/  // DEPRECATED :: would need a Material Info json file for each game to make these works too
        };


        public FlverGamePreset Preset { get; set; } = FlverGamePreset.DS1Static;

        public void ChooseGamePreset(FlverGamePreset preset)
        {
            //Layout
            BufferLayout = new SoulsFormats.FLVER2.BufferLayout();

            void Member(MBT t, MBS s, int i = 0)
            {
                var newMemb = new SoulsFormats.FLVER.LayoutMember(t, s, i);
                BufferLayout.Add(newMemb);
            }

            //Texture Channel Setup

            FBXMaterialChannelMap.Clear();

            void Tex(string fbxChannel, string ingameChannel)
            {
                FBXMaterialChannelMap.Add(fbxChannel, ingameChannel);
            }

            //Hardcoded texture channels

            MTDHardcodedTextureChannels.Clear();

            void HardcodeTex(string ingameChannel, string hardcodedValue)
            {
                MTDHardcodedTextureChannels.Add(ingameChannel, hardcodedValue);
            }

            switch (preset)
            {
                case FlverGamePreset.DS1Static:
                    Member(MBT.Float3, MBS.Position);
                    Member(MBT.Byte4B, MBS.BoneIndices);
                    Member(MBT.Byte4C, MBS.Normal);
                    Member(MBT.Byte4C, MBS.Tangent);
                    Member(MBT.Byte4C, MBS.VertexColor);
                    Member(MBT.UV, MBS.UV);

                    Tex("Texture", "g_Diffuse");
                    Tex("Specular", "g_Specular");
                    Tex("NormalMap", "g_Bumpmap");

                    HardcodeTex("g_DetailBumpmap", "");

                    FlverVersion = 0x2000C;
                    TpfEncoding = 2;
                    TpfFlag2 = 3;

                    IsStatic = true;

                    NormalWValue = 0;
                    UVScaleX = 1;
                    UVScaleY = 1;
                    PlaceholderMaterialShaderName = "P_Metal[DSB]";
                    UseDirectBoneIndices = false;
                    Unk0x5CValue = 0;
                    Unk0x68Value = 0;

                    FakeTextureDirectory = "";

                    break;
                case FlverGamePreset.DS1Skinned:
                    Member(MBT.Float3, MBS.Position);
                    Member(MBT.Byte4B, MBS.BoneIndices);
                    Member(MBT.Short4toFloat4A, MBS.BoneWeights);
                    Member(MBT.Byte4C, MBS.Normal);
                    Member(MBT.Byte4C, MBS.Tangent);
                    Member(MBT.Byte4C, MBS.VertexColor);
                    Member(MBT.UV, MBS.UV);

                    Tex("Texture", "g_Diffuse");
                    Tex("Specular", "g_Specular");
                    Tex("NormalMap", "g_Bumpmap");

                    HardcodeTex("g_DetailBumpmap", "");

                    FlverVersion = 0x2000C;
                    TpfEncoding = 2;
                    TpfFlag2 = 3;

                    IsStatic = false;

                    NormalWValue = 0;
                    UVScaleX = 1;
                    UVScaleY = 1;
                    PlaceholderMaterialShaderName = "P_Metal[DSB]";
                    UseDirectBoneIndices = false;
                    Unk0x5CValue = 0;
                    Unk0x68Value = 0;

                    FakeTextureDirectory = "";

                    break;
/*                case FlverGamePreset.DS2Static:
                    Member(MBT.Float3, MBS.Position);
                    Member(MBT.Byte4B, MBS.Normal);
                    Member(MBT.Byte4B, MBS.Tangent);
                    Member(MBT.Byte4B, MBS.BoneIndices);
                    Member(MBT.Byte4C, MBS.BoneWeights);
                    Member(MBT.Byte4C, MBS.VertexColor, 0);
                    Member(MBT.Byte4C, MBS.VertexColor, 1);
                    Member(MBT.UV, MBS.UV);

                    Tex("Texture", "g_DiffuseTexture");
                    Tex("Specular", "g_SpecularTexture");
                    Tex("NormalMap", "g_BumpmapTexture");

                    HardcodeTex("g_BreakBumpmapTexture", @"N:\FRPG2\data\Model\parts\Common\tex\WP_damage_metal_n.tga");

                    FlverVersion = 0x20010;
                    TpfEncoding = 2;
                    TpfFlag2 = 3;

                    NormalWValue = -1;
                    UVScaleX = 1;
                    UVScaleY = 1;
                    PlaceholderMaterialShaderName = "W[Ibl][DSB]";
                    UseDirectBoneIndices = false;
                    Unk0x5CValue = 1;
                    Unk0x68Value = 1;

                    FakeTextureDirectory = @"N:\FBX2FLVER\CustomTextures\";

                    break;
                case FlverGamePreset.DS2Skinned:
                    Member(MBT.Float3, MBS.Position);
                    Member(MBT.Byte4B, MBS.Normal);
                    Member(MBT.Byte4B, MBS.Tangent);
                    Member(MBT.Byte4B, MBS.BoneIndices);
                    Member(MBT.Byte4C, MBS.BoneWeights);
                    Member(MBT.Byte4C, MBS.VertexColor, 0);
                    Member(MBT.Byte4C, MBS.VertexColor, 1);
                    Member(MBT.Byte4C, MBS.VertexColor, 2);
                    Member(MBT.UV, MBS.UV);

                    Tex("Texture", "g_DiffuseTexture");
                    Tex("Specular", "g_SpecularTexture");
                    Tex("NormalMap", "g_BumpmapTexture");

                    HardcodeTex("g_BreakBumpmapTexture", @"N:\FRPG2\data\Model\parts\Common\tex\WP_damage_metal_n.tga");

                    FlverVersion = 0x20010;
                    TpfEncoding = 2;
                    TpfFlag2 = 3;

                    NormalWValue = -1;
                    UVScaleX = 1;
                    UVScaleY = 1;
                    PlaceholderMaterialShaderName = "W[Ibl][DSB]";
                    UseDirectBoneIndices = false;
                    Unk0x5CValue = 1;
                    Unk0x68Value = 0;

                    FakeTextureDirectory = @"N:\FBX2FLVER\CustomTextures\";

                    break;
                case FlverGamePreset.DS3Static:
                    Member(MBT.Float3, MBS.Position);
                    Member(MBT.Byte4B, MBS.Normal);
                    Member(MBT.Byte4B, MBS.Tangent);
                    Member(MBT.Byte4B, MBS.Tangent, 1);
                    Member(MBT.Byte4B, MBS.BoneIndices);
                    Member(MBT.Byte4C, MBS.BoneWeights);
                    Member(MBT.Byte4C, MBS.VertexColor, 1);
                    Member(MBT.UVPair, MBS.UV);


                    Tex("Texture", "g_DiffuseTexture");
                    //Tex("Specular", "g_SpecularTexture");
                    //Tex("Reflection", "g_ShininessTexture");
                    Tex("Specular", "g_ShininessTexture");
                    Tex("NormalMap", "g_BumpmapTexture");

                    HardcodeTex("g_DamagedNormalTexture", @"N:\SPRJ\data\Other\SysTex\SYSTEX_DummyDamagedNormal.tif");

                    FlverVersion = 0x20014;
                    TpfEncoding = 1;
                    TpfFlag2 = 3;

                    NormalWValue = -1;
                    UVScaleX = 1;
                    UVScaleY = 1;
                    PlaceholderMaterialShaderName = "P_WP[ARSN]";
                    UseDirectBoneIndices = true;
                    Unk0x5CValue = 0;
                    Unk0x68Value = 4;

                    FakeTextureDirectory = "";

                    break;
                case FlverGamePreset.DS3Skinned:
                    Member(MBT.Float3, MBS.Position);
                    Member(MBT.Byte4B, MBS.Normal);
                    Member(MBT.Byte4B, MBS.Tangent);
                    Member(MBT.Byte4B, MBS.Tangent, 1);
                    Member(MBT.Byte4B, MBS.BoneIndices);
                    Member(MBT.Byte4C, MBS.BoneWeights);
                    Member(MBT.UVPair, MBS.UV);
                    Member(MBT.Byte4C, MBS.UV, 1);

                    Tex("Texture", "SAT_Equip_snp_Texture2D_5_AlbedoMap_0");
                    //Tex("Specular", "g_SpecularTexture");
                    Tex("Specular", "SAT_Equip_snp_Texture2D_3_ReflectanceMap_0");
                    Tex("NormalMap", "SAT_Equip_snp_Texture2D_4_NormalMap_0");

                    HardcodeTex("SAT_Equip_snp_Texture2D_1_BlendMask", @"N:\FDP\data\Other\SysTex\SYSTEX_DummyBurn_m.tif");
                    HardcodeTex("SAT_Equip_snp_Texture2D_0_EmissiveMap_0", @"N:\FDP\data\Other\SysTex\SYSTEX_DummyBurn_em.tif");
                    HardcodeTex("SAT_Equip_snp_Texture2D_2_DamageNormal", @"N:\FDP\data\Other\SysTex\SYSTEX_DummyDamagedNormal.tif");

                    FlverVersion = 0x20014;
                    TpfEncoding = 1;
                    TpfFlag2 = 3;

                    NormalWValue = -1;
                    UVScaleX = 1;
                    UVScaleY = 1;
                    PlaceholderMaterialShaderName = "P_BD[ARSN]";
                    UseDirectBoneIndices = true;
                    Unk0x5CValue = 0;
                    Unk0x68Value = 4;

                    FakeTextureDirectory = "";

                    break;*/
                default:
                    throw new NotImplementedException();
            }
        }

        public static string GetBufferLayoutStringFromStruct(SoulsFormats.FLVER2.BufferLayout bufferLayout)
        {
            var sb = new StringBuilder();
            foreach (var member in bufferLayout)
            {
                sb.AppendLine($"    {member.Type} {member.Semantic}{(member.Index > 0 ? $"[{member.Index}]" : "")}");
            }
            return sb.ToString();
        }

        //public static SoulsFormats.FLVER.BufferLayout GetBufferLayoutStructFromString(string bufferLayout)
        //{
        //    int structOffset = 0;
        //    var layout = new SoulsFormats.FLVER.BufferLayout();
        //    var splitIntoLines = bufferLayout.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
        //    var lines = splitIntoLines.Select(line => line.Trim().Split(' ').Select(word => word.Trim()).Where(word => !string.IsNullOrWhiteSpace(word)).ToArray());
        //    int lineNum = 0;
        //    foreach (var line in lines)
        //    {
        //        bool invalidLine = false;

        //        SoulsFormats.FLVER.BufferLayout.MemberType memberType = MBT.Float3;
        //        SoulsFormats.FLVER.BufferLayout.MemberSemantic memberSemantic = MBS.Position;
        //        int memberIndex = 0;

        //        if (line.Length >= 2)
        //        {
        //            if (Enum.TryParse(line[0], out SoulsFormats.FLVER.BufferLayout.MemberType memberTypeEnumValue))
        //            {
        //                memberType = memberTypeEnumValue;
        //            }
        //            else
        //            {
        //                invalidLine = true;
        //            }

        //            if (Enum.TryParse(line[1], out SoulsFormats.FLVER.BufferLayout.MemberSemantic memberSemanticEnumValue))
        //            {
        //                memberSemantic = memberSemanticEnumValue;
        //            }
        //            else
        //            {
        //                invalidLine = true;
        //            }

        //            if (line.Length >= 3)
        //            {
        //                if (int.TryParse(line[2], out int memberIndexIntValue))
        //                {
        //                    memberIndex = memberIndexIntValue;
        //                }
        //                else
        //                {
        //                    invalidLine = true;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            invalidLine = true;
        //        }

        //        if (invalidLine)
        //        {
        //            throw new Exception($"Invalid line in buffer layout declaration: \"{splitIntoLines[lineNum]}\"");
        //        }

        //        var member = new SoulsFormats.FLVER.BufferLayout.Member(0, structOffset, memberType, memberSemantic, memberIndex);

        //        layout.Add(member);

        //        structOffset += member.Size;

        //        lineNum++;
        //    }

        //    return layout;
        //}

    }
}
