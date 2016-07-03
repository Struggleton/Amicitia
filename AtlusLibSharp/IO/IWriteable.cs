namespace AtlusLibSharp.IO
{
    using Utilities;
    using System.IO;
    internal interface IWriteable
    {
        void InternalWrite(BinaryWriter writer);
    }
}
