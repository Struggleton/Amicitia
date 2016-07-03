﻿namespace Amicitia
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using AtlusLibSharp.FileSystems.BVP;
    using AtlusLibSharp.Graphics.RenderWare;
    using AtlusLibSharp.FileSystems.ListArchive;
    using AtlusLibSharp.FileSystems.PAKToolArchive;
    using AtlusLibSharp.Graphics.TMX;
    using AtlusLibSharp.Graphics.SPR;
    using AtlusLibSharp.Graphics.TGA;
    using AtlusLibSharp.FileSystems.ACX;
    using AtlusLibSharp.FileSystems.AFS;
    using AtlusLibSharp.FileSystems.AMD;
    using AtlusLibSharp.FileSystems.AWB;
    using AtlusLibSharp.FileSystems.EPL;

    internal static class SupportedFileHandler
    {
        private static readonly SupportedFileInfo[] _supportedFiles = new SupportedFileInfo[]
        {
            // Export only formats
            new SupportedFileInfo("Raw data",                           SupportedFileType.Resource,             false, ".*"),
            new SupportedFileInfo("Portable Network Graphics",          SupportedFileType.PNGFile,              true,  ".png"),
            new SupportedFileInfo("Truevision TARGA",                   SupportedFileType.TGAFile,              true,  ".tga"),
            new SupportedFileInfo("COLLADA DAE",                        SupportedFileType.DAEFile,              true,  ".dae"),

            // Archive formats
            new SupportedFileInfo("Atlus Generic Archive",              SupportedFileType.PAKToolArchiveFile,          false, ".bin", ".f00", ".f01", ".p00", ".p01", ".fpc", ".pak", ".pac", ".pack", ".se"),
            new SupportedFileInfo("Atlus Generic List Archive",         SupportedFileType.ListArchiveFile,      false, ".arc", ".bin", ".pak", ".pac", ".abin", ".se", ".pse"),
            new SupportedFileInfo("Persona 3/4 Battle Voice Package",   SupportedFileType.BVPArchiveFile,       false, ".bvp"),
            new SupportedFileInfo("Atlus Vita Resource Container",      SupportedFileType.AMDFile,              false, ".amd"),
            new SupportedFileInfo("Criware Generic Sound Package",      SupportedFileType.AWBFile,              false, ".awb"),
            new SupportedFileInfo("Criware Sound Package",              SupportedFileType.AFSFile,              false, ".afs"),
            new SupportedFileInfo("Criware Voice Package",              SupportedFileType.ACXFile,              false, ".acx"),
            new SupportedFileInfo("Atlus General Package",              SupportedFileType.EPLFile,              false, ".epl"),

            // Texture (container) formats
            new SupportedFileInfo("Atlus PS2 Texture",                  SupportedFileType.TMXFile,              false, ".tmx"),
            new SupportedFileInfo("Atlus TMX Sprite Container",         SupportedFileType.SPRFile,              false, ".spr"),
            new SupportedFileInfo("Atlus TGA Sprite Container",         SupportedFileType.SPR4File,             false, ".spr4"),
            new SupportedFileInfo("RenderWare PS2 Texture Container",   SupportedFileType.RWTextureDictionary,  false, ".txd"),
            new SupportedFileInfo("RenderWare PS2 Texture",             SupportedFileType.RWTextureNative,      false, ".txn"),

            // Model formats
            new SupportedFileInfo("Atlus RenderWare Scene Container",   SupportedFileType.RMDScene,             false, ".rmd", ".rws"),
            new SupportedFileInfo("RenderWare Scene",                   SupportedFileType.RWScene,              false, ".dff"),
        };

        private static readonly Dictionary<SupportedFileType, Type> _supportedFileTypeEnumToType = new Dictionary<SupportedFileType, Type>()
        {
            // Archive formats
            { SupportedFileType.BVPArchiveFile,         typeof(BVPFile) },
            { SupportedFileType.ListArchiveFile,        typeof(ListArchiveFile) },
            { SupportedFileType.PAKToolArchiveFile,     typeof(PAKToolArchiveFile) },
            { SupportedFileType.AMDFile,                typeof(AMDFile) },
            { SupportedFileType.AWBFile,                typeof(AWBFile) },
            { SupportedFileType.AFSFile,                typeof(AFSFile) },
            { SupportedFileType.ACXFile,                typeof(ACXFile) },
            { SupportedFileType.EPLFile,                typeof(EPLFile) },

            // Texture formats
            { SupportedFileType.RWTextureDictionary,    typeof(RWTextureDictionary) },
            { SupportedFileType.RWTextureNative,        typeof(RWTextureNative) },
            { SupportedFileType.SPRFile,                typeof(SPRFile) },
            { SupportedFileType.SPR4File,               typeof(SPR4File) },
            { SupportedFileType.TMXFile,                typeof(TMXFile) },
            { SupportedFileType.TGAFile,                typeof(TGAFile) },

            // Model formats
            { SupportedFileType.RMDScene,               typeof(RMDScene) },
            { SupportedFileType.RWScene,                typeof(RWScene) },
        };

        private static readonly string _fileFilter;

        static SupportedFileHandler()
        { 
            _fileFilter = GetFileFilter();
        }

        // Properties
        public static string FileFilter
        {
            get { return _fileFilter; }
        }

        public static SupportedFileType GetType(int index)
        {
            if (index == -1)
            {
                return SupportedFileType.Resource;
            }
            else
            {
                return _supportedFiles[index].Type;
            }
        }

        // Public Methods
        public static int GetSupportedFileIndex(string path)
        {
            int idx = -1;
            using (FileStream stream = File.OpenRead(path))
            {
                idx = GetSupportedFileIndex(path, stream);
            }

            return idx;
        }

        public static int GetSupportedFileIndex(string name, Stream stream)
        {
            // TODO: Add support for multiple possible support formats, and distinguishing between those ala content based file type checks.
            string ext = Path.GetExtension(name).ToLowerInvariant();
            SupportedFileInfo[] matched = Array.FindAll(_supportedFiles, s => s.Extensions.Contains(ext));

            // No matches were found
            if (matched.Length == 0)
                return -1;

            // TODO: Reflection is slow, perhaps speed it up somehow?
            if (matched.Length > 1)
            {
                for (int i = 0; i < matched.Length; i++)
                {
                    Type type = _supportedFileTypeEnumToType[matched[i].Type];
                    MethodInfo methodInfo = type.GetRuntimeMethod("VerifyFileType", new Type[] { typeof(Stream) });
                    bool verifiedSuccess = (bool)methodInfo.Invoke(null, new object[] { stream });
                    if (verifiedSuccess)
                    {
                        return Array.IndexOf(_supportedFiles, matched[i]);
                    }
                }

                return -1;
            }
            else
            {
                return Array.IndexOf(_supportedFiles, matched[0]);
            }
        }

        public static string GetFilteredFileFilter(params SupportedFileType[] includedTypes)
        {
            string filter = string.Empty;
            List<SupportedFileInfo> filteredInfo = new List<SupportedFileInfo>(includedTypes.Length);

            foreach (SupportedFileInfo item in _supportedFiles)
            {
                if (includedTypes.Contains(item.Type))
                {
                    filteredInfo.Add(item);
                }
            }

            filteredInfo = GetSortedFilteredInfo(filteredInfo, includedTypes);

            for (int i = 0; i < filteredInfo.Count; i++)
            {
                filter += SupportedFileInfoToFilterString(filteredInfo[i]);

                if (i != filteredInfo.Count - 1)
                {
                    // For every entry that isn't the last, add a seperator
                    filter += "|";
                }
            }

            return filter;
        }

        // Private Methods
        private static List<SupportedFileInfo> GetSortedFilteredInfo(List<SupportedFileInfo> unsortedInfo, SupportedFileType[] includedTypes)
        {
            List<SupportedFileInfo> filteredInfo = new List<SupportedFileInfo>(unsortedInfo.Count);
            foreach (SupportedFileType fileType in includedTypes)
            {
                filteredInfo.Add(unsortedInfo.Find(info => info.Type == fileType));
            }
            return filteredInfo;
        }

        private static string GetFileFilter()
        {
            string filter = "All files|*.*|";
            for (int i = 0; i < _supportedFiles.Length; i++)
            {
                if (_supportedFiles[i].ExportOnly == true)
                    continue;

                filter += SupportedFileInfoToFilterString(_supportedFiles[i]);

                if (i != _supportedFiles.Length - 1)
                {
                    // For every entry that isn't the last, add a seperator
                    filter += "|";
                }
            }
            return filter;
        }

        private static string SupportedFileInfoToFilterString(SupportedFileInfo info)
        {
            string filter = info.Description + "|";
            for (int i = 0; i < info.Extensions.Length; i++)
            {
                filter += "*" + info.Extensions[i];
                if (i != info.Extensions.Length - 1)
                {
                    // For every entry that isn't the last, add a seperator
                    filter += ";";
                }
            }
            return filter;
        }
    }
}
