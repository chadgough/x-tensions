using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace XTension
{
    public static class HelperMethods
    {        
        public static byte[] ReadItem(IntPtr hItem)
        {
            //If successfull - returns contents of the item as a byte array,
            //if failed - returns null.

            if (ImportedMethods.XWFGetSize != null && ImportedMethods.XWFRead != null)
                try
                {
                    Int64 size = ImportedMethods.XWFGetSize(hItem, IntPtr.Zero);
                    var bufferSize = (int)size;

                    var bufferPtr = Marshal.AllocHGlobal(bufferSize);
                    ImportedMethods.XWFRead(hItem, 0, bufferPtr, (uint)bufferSize);

                    var contents = new byte[bufferSize];
                    Marshal.Copy(bufferPtr, contents, 0, bufferSize);
                    Marshal.FreeHGlobal(bufferPtr);

                    return contents;
                }
                catch {}                            

            return null;
        }

        public static SearchInfo CreateSearchInfo(string searchTerms, SearchInfoFlags flags)
        {
            var info = new SearchInfo
            {
                  hVolume = IntPtr.Zero //the docs say that hVolume should be 0
                , lpSearchTerms = searchTerms
                , nFlags = flags
                , nSearchWindow = 0
            };

            info.iSize = Marshal.SizeOf(info);
            return info;
        }

        public static string GetFullPath(Int32 itemId)
        {
            /*
            from the docs:
            
            XWF_GetItemParent returns the ID of the parent of the specified item,
            or -1 if the item is the root directory.                         
            */
            
            var sb = new StringBuilder();
            while (true)
            {
                var parentItemId = ImportedMethods.XWFGetItemParent(itemId);

                /*
                XWFGetItemName returns text "(Root directory)" for the root directory.
                I don't see any sense in putting such kind of a string into the path,
                so, if (parentItemId < 0) then this is a root directory
                and we don't need it's name to be added.
                */
                if (parentItemId < 0) return sb.ToString();

                sb.Insert(0, Path.DirectorySeparatorChar
                    + ImportedMethods.XWFGetItemName(itemId));

                itemId = parentItemId;
            }            
        }

        public static Int32 CreateFileFromExternalFile(string name
            , string externalFilename
            , Int32 parentItemId
            , bool keepExternalFile = false)
        {
            var extFilenamePtr = Marshal.StringToHGlobalUni(externalFilename);       

            var itemId = ImportedMethods.XWFCreateFile(name
                , XWFCreateFileFlags.AttachExternalFile
                    | (keepExternalFile ? XWFCreateFileFlags.KeepExternalFile : 0)
                , parentItemId
                , extFilenamePtr);

            Marshal.FreeHGlobal(extFilenamePtr);
            return itemId;
        }
    }
}
