namespace Amicitia.ResourceWrappers
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using AtlusLibSharp.FileSystems.EPL;
    using AtlusLibSharp.IO;

    internal class EPLFileWrapper : ResourceWrapper
    {
        /*********************/
        /* File filter types */
        /*********************/
        public static readonly new SupportedFileType[] FileFilterTypes = new SupportedFileType[]
        {
            SupportedFileType.EPLFile
        };

        /*****************************************/
        /* Import / Export delegate dictionaries */
        /*****************************************/
        public static readonly new Dictionary<SupportedFileType, Action<ResourceWrapper, string>> ImportDelegates = new Dictionary<SupportedFileType, Action<ResourceWrapper, string>>()
        {
            {
                SupportedFileType.EPLFile, (res, path) =>
                res.WrappedObject = new EPLFile(path)
            }
        };

        /*
        public static readonly new Dictionary<SupportedFileType, Action<ResourceWrapper, string>> ExportDelegates = new Dictionary<SupportedFileType, Action<ResourceWrapper, string>>()
        {
            {
                SupportedFileType.ACXFile, (res, path) =>
                (res as ACXFileWrapper).WrappedObject.Save(path)
            },
        };
        */

        /************************************/
        /* Import / export method overrides */
        /************************************/
        protected override Dictionary<SupportedFileType, Action<ResourceWrapper, string>> GetImportDelegates()
        {
            return ImportDelegates;
        }

        /*
        protected override Dictionary<SupportedFileType, Action<ResourceWrapper, string>> GetExportDelegates()
        {
            return ExportDelegates;
        }
        */

        protected override SupportedFileType[] GetSupportedFileTypes()
        {
            return FileFilterTypes;
        }
        

        /***************/
        /* Constructor */
        /***************/
        public EPLFileWrapper(string text, EPLFile res) : base(text, res, SupportedFileType.EPLFile, true)
        {
            m_canExport = true;
            m_canReplace = false;
            InitializeContextMenuStrip();
        }

        /*****************************/
        /* Wrapped object properties */
        /*****************************/
        [Browsable(false)]
        public new EPLFile WrappedObject
        {
            get { return (EPLFile)m_wrappedObject; }
            set { SetProperty(ref m_wrappedObject, value); }
        }

        /*********************************/
        /* Base wrapper method overrides */
        /*********************************/
        internal override void RebuildWrappedObject()
        {
            var archive = new EPLFile();
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
                var wrap = new ResourceWrapper(string.Format("{0}", WrappedObject.Names[idx++]), new GenericBinaryFile(chunk), SupportedFileType.Resource, true);
                wrap.m_canReplace = false;
                wrap.m_canRename = false;
                wrap.InitializeContextMenuStrip();
                Nodes.Add(wrap);
            }

            base.InitializeWrapper();
        }
    }
}