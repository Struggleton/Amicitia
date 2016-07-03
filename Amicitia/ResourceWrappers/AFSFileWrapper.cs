namespace Amicitia.ResourceWrappers
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using AtlusLibSharp.FileSystems.AFS;
    using AtlusLibSharp.IO;

    internal class AFSFileWrapper : ResourceWrapper
    {
        /*********************/
        /* File filter types */
        /*********************/
        public static readonly new SupportedFileType[] FileFilterTypes = new SupportedFileType[]
        {
            SupportedFileType.AFSFile
        };

        /*****************************************/
        /* Import / Export delegate dictionaries */
        /*****************************************/
        public static readonly new Dictionary<SupportedFileType, Action<ResourceWrapper, string>> ImportDelegates = new Dictionary<SupportedFileType, Action<ResourceWrapper, string>>()
        {
            {
                SupportedFileType.AFSFile, (res, path) =>
                res.WrappedObject = new AFSFile(path)
            }
        };

        public static readonly new Dictionary<SupportedFileType, Action<ResourceWrapper, string>> ExportDelegates = new Dictionary<SupportedFileType, Action<ResourceWrapper, string>>()
        {
            {
                SupportedFileType.AFSFile, (res, path) =>
                (res as AFSFileWrapper).WrappedObject.Save(path)
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
        public AFSFileWrapper(string text, AFSFile res)
            : base(text, res, SupportedFileType.AFSFile, false)
        {
            m_canExport = false;
            m_canReplace = true;
            InitializeContextMenuStrip();
        }

        /*****************************/
        /* Wrapped object properties */
        /*****************************/
        [Browsable(false)]
        public new AFSFile WrappedObject
        {
            get { return (AFSFile)m_wrappedObject; }
            set { SetProperty(ref m_wrappedObject, value); }
        }

        /*********************************/
        /* Base wrapper method overrides */
        /*********************************/
        internal override void RebuildWrappedObject()
        {
            var archive = new AFSFile();
            List<string> Names = new List<string>();
            foreach (ResourceWrapper node in Nodes)
            {
                archive.Data.Add(node.GetBytes());
                Names.Add(node.Text);
            }

            archive.Names = Names.ToArray();
            m_wrappedObject = archive;
            m_isDirty = false;
        }

        internal override void InitializeWrapper()
        {
            Nodes.Clear();

            int idx = 0;
            foreach (byte[] chunk in WrappedObject.Data)
            {
                var wrap = new ResourceWrapper(string.Format("{0}", WrappedObject.Names[idx++]), new GenericBinaryFile(chunk), SupportedFileType.Resource, false);
                wrap.m_canReplace = true;
                wrap.m_canRename = false;
                wrap.InitializeContextMenuStrip();
                Nodes.Add(wrap);
            }

            base.InitializeWrapper();
        }
    }
}