namespace Amicitia.ResourceWrappers
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using AtlusLibSharp.FileSystems.AMD;

    internal class AMDArchiveWrapper : ResourceWrapper
    {
        protected internal static readonly SupportedFileType[] FileFilterTypes = new SupportedFileType[]
        {
            SupportedFileType.AMDFile

        };
        public AMDArchiveWrapper(string text, AMDFile arc) : base(text, arc) { }
        public SupportedFileType FileType
        {
            get { return SupportedFileType.AMDFile; }
        }

        public int EntryCount
        {
            get { return Nodes.Count; }
        }

        public int ChunkSize
        {
            get { return Nodes.Count; }
        }

        protected internal new AMDFile WrappedObject
        {
            get { return (AMDFile)base.WrappedObject; }
            set { base.WrappedObject = value; }
        }

        public override void Replace(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDlg = new OpenFileDialog())
            {
                openFileDlg.FileName = Text;
                openFileDlg.Filter = SupportedFileHandler.GetFilteredFileFilter(FileFilterTypes);

                if (openFileDlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                int supportedFileIndex = SupportedFileHandler.GetSupportedFileIndex(openFileDlg.FileName);

                if (supportedFileIndex == -1)
                {
                    return;
                }

                switch (FileFilterTypes[openFileDlg.FilterIndex - 1])
                {
                    case SupportedFileType.AMDFile:
                        WrappedObject = new AMDFile(openFileDlg.FileName);
                        break;
                }

                // re-init the wrapper
                InitializeWrapper();
            }
        }

        public override void Export(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDlg = new SaveFileDialog())
            {
                saveFileDlg.FileName = Text;
                saveFileDlg.Filter = SupportedFileHandler.GetFilteredFileFilter(FileFilterTypes);

                if (saveFileDlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                // rebuild wrapped object before export
                RebuildWrappedObject();

                switch (FileFilterTypes[saveFileDlg.FilterIndex - 1])
                {
                    case SupportedFileType.AMDFile:
                        WrappedObject.Save(saveFileDlg.FileName);
                        break;
                }
            }
        }

        protected internal override void RebuildWrappedObject()
        {
            WrappedObject.Entries.Clear();
            foreach (ResourceWrapper node in Nodes)
            {
                node.RebuildWrappedObject();
                WrappedObject.Entries.Add(new AMDChunk(node.Text, node.GetBytes()));
            }
        }

        protected internal override void InitializeWrapper()
        {
            Nodes.Clear();
            foreach (AMDChunk entry in WrappedObject.Entries)
            {
                Nodes.Add(ResourceFactory.GetResource(entry.Name, new MemoryStream(entry.Data)));
            }

            if (IsInitialized)
            {
                MainForm.Instance.UpdateReferences();
            }
            else
            {
                IsInitialized = true;
            }
        }

    }
}
