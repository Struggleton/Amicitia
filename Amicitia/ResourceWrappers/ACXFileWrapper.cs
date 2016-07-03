namespace Amicitia.ResourceWrappers
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using AtlusLibSharp.FileSystems.ACX;
    using AtlusLibSharp.IO;

    internal class ACXFileWrapper : ResourceWrapper
    {
        /*********************/
        /* File filter types */
        /*********************/
        public static readonly new SupportedFileType[] FileFilterTypes = new SupportedFileType[]
        {
            SupportedFileType.ACXFile
        };

        /*****************************************/
        /* Import / Export delegate dictionaries */
        /*****************************************/
        public static readonly new Dictionary<SupportedFileType, Action<ResourceWrapper, string>> ImportDelegates = new Dictionary<SupportedFileType, Action<ResourceWrapper, string>>()
        {
            {
                SupportedFileType.ACXFile, (res, path) =>
                res.WrappedObject = new ACXFile(path)
            }
        };

        public static readonly new Dictionary<SupportedFileType, Action<ResourceWrapper, string>> ExportDelegates = new Dictionary<SupportedFileType, Action<ResourceWrapper, string>>()
        {
            {
                SupportedFileType.ACXFile, (res, path) =>
                (res as ACXFileWrapper).WrappedObject.Save(path)
            },
        };

        /************************************/
        /* Import / export method overrides */
        /************************************/
        protected override Dictionary<SupportedFileType, Action<ResourceWrapper, string>> GetImportDelegates()
        {
            return ImportDelegates;
        }

        protected override Dictionary<SupportedFileType, Action<ResourceWrapper, string>> GetExportDelegates()
        {
            return ExportDelegates;
        }

        protected override SupportedFileType[] GetSupportedFileTypes()
        {
            return FileFilterTypes;
        }

        /***************/
        /* Constructor */
        /***************/
        public ACXFileWrapper(string text, ACXFile res)
            : base(text, res, SupportedFileType.ACXFile, false)
        {
            m_canExport = false;
            m_canReplace = true;
            InitializeContextMenuStrip();
        }

        /*****************************/
        /* Wrapped object properties */
        /*****************************/
        [Browsable(false)]
        public new ACXFile WrappedObject
        {
            get { return (ACXFile)m_wrappedObject; }
            set { SetProperty(ref m_wrappedObject, value); }
        }

        /*********************************/
        /* Base wrapper method overrides */
        /*********************************/
        internal override void RebuildWrappedObject()
        {
            var archive = new ACXFile();
            foreach (ResourceWrapper node in Nodes)
            {
                archive._Data.Add(node.GetBytes());
            }

            m_wrappedObject = archive;
            m_isDirty = false;
        }

        internal override void InitializeWrapper()
        {
            Nodes.Clear();

            int idx = 0;
            foreach (byte[] chunk in WrappedObject._Data)
            {
                var wrap = new ResourceWrapper(string.Format("{0}.adx", idx++), new GenericBinaryFile(chunk), SupportedFileType.Resource, false);
                wrap.m_canReplace = true;
                wrap.m_canRename = false;
                wrap.InitializeContextMenuStrip();
                Nodes.Add(wrap);
            }

            base.InitializeWrapper();
        }
    }
}