using System;
using System.Collections.Generic;
using System.Text;
using MBT = SoulsFormats.FLVER.LayoutType;
using MBS = SoulsFormats.FLVER.LayoutSemantic;
using Newtonsoft.Json.Linq;

using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections;

namespace FBX2FLVER
{
    public class FBX2FLVERLayoutHelper
    {
        private static JArray MTD_INFO_LIST;

        public static SoulsFormats.FLVER2.BufferLayout getLayout(string MTDName, bool isStatic)
        {

            if (MTD_INFO_LIST == null) {
                loadMTDInfoList();
            }

            JObject MTD_INFO = getMTDInfo(MTDName);
            JArray MTD_LAYOUT_MEMBERS = (JArray)MTD_INFO["LayoutMembers"];

            SoulsFormats.FLVER2.BufferLayout BL = new SoulsFormats.FLVER2.BufferLayout();
            for (int i = 0; i < MTD_LAYOUT_MEMBERS.Count; i++)
            {
                MBT mbt = (MBT)uint.Parse(MTD_LAYOUT_MEMBERS[i]["Type"].ToString());
                MBS mbs = (MBS)uint.Parse(MTD_LAYOUT_MEMBERS[i]["Semantic"].ToString());
                if(isStatic && mbs == MBS.BoneWeights) { continue; }
                BL.Add(new SoulsFormats.FLVER.LayoutMember(mbt, mbs));
            }

            return BL;
        }

        public static List<TextureKey> getTextureMap(string MTDName)
        {
            if (MTD_INFO_LIST == null)
            {
                loadMTDInfoList();
            }

            JObject MTD_INFO = getMTDInfo(MTDName);
            JArray MTD_TEXTURE_MEMBERS = (JArray)MTD_INFO["TextureMembers"];
            List<TextureKey> TM = new List<TextureKey>();
            for (int i=0;i<MTD_TEXTURE_MEMBERS.Count; i++)
            {
                string TexMem = MTD_TEXTURE_MEMBERS[i].ToString();
                switch (TexMem)
                {
                    case "g_Diffuse": TM.Add(new TextureKey("Texture", TexMem, 0x1, true));  break;
                    case "g_Diffuse_2": TM.Add(new TextureKey("", TexMem, 0x1, true)); break;
                    case "g_Specular": TM.Add(new TextureKey("Specular", TexMem, 0x1, true)); break;
                    case "g_Specular_2": TM.Add(new TextureKey("", TexMem, 0x1, true)); break;
                    case "g_Bumpmap": TM.Add(new TextureKey("NormalMap", TexMem, 0x1, true)); break;
                    case "g_Bumpmap_2": TM.Add(new TextureKey("", TexMem, 0x1, true)); break;
                    case "g_Envmap": TM.Add(new TextureKey("", TexMem, 0x1, true)); break;
                    case "g_Lightmap": TM.Add(new TextureKey("Emissive", TexMem, 0x1, true)); break;
                    default: break;
                }
            }
            return TM;
        }

        public static List<TextureKey> getHardcodedTextureMap()
        {
            List<TextureKey> TM = new List<TextureKey>();
            TM.Add(new TextureKey("g_DetailBumpmap", "", 0x0, false));
            return TM;
        }


        private static JObject getMTDInfo(string MTDName)
        {
            for (int i = 0; i < MTD_INFO_LIST.Count; i++)
            {
                if (MTD_INFO_LIST[i]["mtdName"].ToString().Equals(MTDName))
                {
                    return (JObject)MTD_INFO_LIST[i];
                }
            }
            /* This is bad times if this happens */
            return null;
        }

        private static void loadMTDInfoList()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "FBX2FLVER.EmbeddedResources.DS1_MTD_INFO.json";
            string data;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
            }

            JObject json = JObject.Parse(data);
            MTD_INFO_LIST = (JArray)json["mtds"];
        }
    }

    public class TextureKey
    {
        public string Key, Value;
        public byte Unk10;
        public bool Unk11;
        public TextureKey(string k, string v, byte u, bool uu)
        {
            Key = k; Value = v; Unk10 = u; Unk11 = uu;
        }
    }
}