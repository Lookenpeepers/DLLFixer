using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLLFixer
{
    public static class Games
    {
        public struct HaloCE
        {
            public static int fileSize = 29179304;
            public static string InGameAOB = "64 61 65 68 07 00 00 00 ??";
            public static string ThirdPersonMask = "0F 85 80 00 00 00 48 8B 05 4C B3 F4 01 48 8D 54 24 48 49 6B CE 54 8B 4C 01 10 E8 2A FB FF FF 66 83 F8 01 75";
            public static string ThirdPersonPokedMask = "90 90 90 90 90 90 48 8B 05 4C B3 F4 01 48 8D 54 24 48 49 6B CE 54 8B 4C 01 10 E8 2A FB FF FF 66 05 01 00 74";
            public static string BumpPossessionMask = "B4 57 ?? ?? ?? 02";
            public static string BumpPossessionPokedMask = "B4 57 ?? ?? ?? 02";
            public static string PanCamMask = "00 00 00 68 39 ?? ?? ?? 02 00 00";
            public static string PanCamPokedMask = "01 00 00 68 39 ?? ?? ?? 02 00 00";
        }
        public struct Halo2A
        {
            public static int fileSize = 16402344;
            public static string InGameAOB = "64 61 65 68 0C 00 00 00 00 ?? ?? ?? 00 00 00 00 ?? ?? ?? F4 01 00 00 00 00 ?? ?? ?? 00 00 ?? 06 67 72 6F 75 6E 64 68 6F 67 5C";
            public static string AIMask = "F6 00 01 0F 84 ?? 00 00 00 F3";
            public static string AIPokedMask = "F6 00 01 90 84 ?? 00 00 00 F3";
            public static string ThirdPersonMask = "74 03 44 89 2F 41";
            public static string ThirdPersonPokedMask = "90 90 44 89 2F 41";
        }
        public struct Halo3
        {
            public static int fileSize = 10857896;
            public static string InGameAOB = "64 61 65 68 0B 00 00 00 00 ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? F4 7F 00 00 00 ?? ?? ?? 00 00 ?? 03 68 61 6C 6F 33 5C 6D 61 70 73";
            public static string AIMask = "F6 00 01 0F 84 ?? 00 00 00 F3 0F";
            public static string AIPokedMask = "F6 00 01 90 84 ?? 00 00 00 F3 0F";
            public static string ThirdPersonMask = "74 0E 44 8D 52 03";
            public static string ThirdPersonPokedMask = "90 90 44 8D 52 03";
        }
        public struct HaloODST
        {
            public static int fileSize = 10496424;
            public static string InGameAOB = "64 61 65 68 0B 00 00 00 00 ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? F4 7F 00 00 00 ?? ?? ?? 00 00 ?? ?? 68 61 6C 6F 33 6F 64 73 74 5C";
            public static string AIMask = "F6 00 01 0F 84 ?? 00 00 00 F3 0F";
            public static string AIPokedMask = "F6 00 01 90 90 90 90 90 90 F3 0F";
            public static string ThirdPersonMask = "75 0E B9 05 00 00";
            public static string ThirdPersonPokedMask = "EB 0E B9 05 00 00";
        }
        public struct HaloReach
        {
            public static int fileSize = 11891112;
            public static string InGameAOB = "64 61 65 68 0D 00 00 00 00 ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? 01 00 00 00 00 ?? ?? ?? 00 00 ??";
            public static string AIMask = "F6 00 01 0F 84 ?? 00 00 00 F3 0F";
            public static string AIPokedMask = "F6 00 01 90 90 90 90 90 90 F3 0F";
            public static string ScriptedAIMask = "00 ?? CB 8A 02 49 03 D5 84 C0 74 0C 49 03 CD 48";
            public static string ScriptedAIPokedMask = "90 90 CB 8A 02 49 03 D5 84 C0 74 0C 49 03 CD 48";
            public static string ThirdPersonMask = "74 0E C7 06 04 00 00 00";
            public static string ThirdPersonPokedMask = "90 90 C7 06 04 00 00 00";
            public static string PanCamMask = "0F 84 A4 01 00 00 40 38 B9 4A 09 00 00 0F 84 97 01 00 00";
            public static string PanCamPokedMask = "90 90 90 90 90 90 40 38 B9 4A 09 00 00 90 90 90 90 90 90";
            public static string BetaRecoilMask = "84 CB 00 00 00 48 8B 45 97 41 83";
            public static string BetaRecoilPokedMask = "85 CB 00 00 00 48 8B 45 97 41 83";
        }
        public struct Halo4
        {
            public static int fileSize = 16537000;
            public static string InGameAOB = "64 61 65 68 0C 00 00 00 00 ?? ?? ?? 00 00 00 00 ?? ?? ?? ?? 01 00 00 00 00 ??";
            public static string AIMask = "74 02 B3 01 8A C3 48 83 C4 20 5B C3 CC CC CC 48 83 EC 28 E8";
            public static string AIPokedMask = "90 90 B3 01 8A C3 48 83 C4 20 5B C3 CC CC CC 48 83 EC 28 E8";
            public static string ThirdPersonMask = "74 03 44 89 2F 41";
            public static string ThirdPersonPokedMask = "90 90 44 89 2F 41";
            public static string CampaignTheaterMask = "0F 44 C7 88 85";
            public static string CampaignTheaterPokedMask = "8B C7 90 88 85";
        }
    }
}