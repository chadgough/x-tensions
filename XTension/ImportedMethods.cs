using System;
using System.Runtime.InteropServices;

namespace XTension
{    
    public static class ImportedMethods
    {
        /*       
        Original:
        ---------
        typedef HANDLE (__stdcall *fptr_XWF_GetVolumeName) (HANDLE hVolume
            , wchar_t* lpString, DWORD nType);

        from the docs: lpString max length - 255 characters

        Notes:
        (1) wchar_t* lpString is a pointer to a null-terminated array of Unicode chars            
            that is why wchar_t* lpString is tranlated to IntPtr lpString
        
        (2) XWF_GetVolumeName WRITES the volume name to the address provided by this pointer
            please see sample usage in ExportedMethods.XT_Init
        */
        
        public static readonly int XWFVolumeNameBufferLength = 256 * 2;
        //(max 255 chars + 1 for zero (to be sure)) * 2 bytes for each Unicode char

        public delegate IntPtr XWFGetVolumeNameDelegate(IntPtr hVolume
            , IntPtr lpString
            , XWFVolumeNameType nType);

        private static XWFGetVolumeNameDelegate _XWFGetVolumeName;

        /*       
        Original:
        ---------
        typedef void (__stdcall * fptr_XWF_OutputMessage) (const wchar_t* lpMessage
            , DWORD nFlags); 
        */

        public delegate void XWFOutputMessageDelegate(
              [MarshalAs(UnmanagedType.LPWStr)] string lpMessage
            , XWFOutputMessageFlags nFlags = 0);
        
        public static XWFOutputMessageDelegate XWFOutputMessage;

        /*       
        Original:
        ---------
        LONG XWF_CreateFile(LPWSTR lpName, DWORD nCreationFlags, LONG nParentItemID
           , PVOID pSourceInfo);
        */

        public delegate Int32 XWFCreateFileDelegate(
              [MarshalAs(UnmanagedType.LPWStr)] string lpName
            , XWFCreateFileFlags nCreationFlags, Int32 nParentItemID, IntPtr pSourceInfo);

        public static XWFCreateFileDelegate XWFCreateFile;

        /*       
        Original:
        ---------
        LARGE_INTEGER XWF_GetSize(HANDLE hVolumeOrItem, LPVOID lpOptional);            
        */

        public delegate Int64 XWFGetSizeDelegate(IntPtr hVolumeOrItem
            , IntPtr lpOptional);

        public static XWFGetSizeDelegate XWFGetSize;

        /*       
        Original:
        ---------
        typedef DWORD (__stdcall *fptr_XWF_Read) (HANDLE hVolumeOrItem, LARGE_INTEGER Offset, char* lpBuffer, 
	        DWORD nNumberOfBytesToRead);
        */

        public delegate UInt32 XWFReadDelegate(IntPtr hVolumeOrItem, Int64 Offset, IntPtr lpBuffer,
            UInt32 nNumberOfBytesToRead);

        public static XWFReadDelegate XWFRead;

        /*       
        Original:
        ---------
        typedef void (__stdcall *fptr_XWF_GetVolumeInformation) (HANDLE hVolume, 
	        LPLONG lpFileSystem, DWORD* nBytesPerSector, DWORD* nSectorsPerCluster, 
	        INT64* nClusterCount, INT64* nFirstClusterSectorNo);
        */

        public delegate void XWFGetVolumeInformationDelegate(IntPtr hVolume
            , out XWFVolumeFileSystem lpFileSystem, out UInt32 nBytesPerSector, out UInt32 nSectorsPerCluster
            , out Int64 nClusterCount, out Int64 nFirstClusterSectorNo);

        private static XWFGetVolumeInformationDelegate _XWFGetVolumeInformation;

        /*       
        Original:
        ---------
        typedef BOOL (__stdcall *fptr_XWF_GetSectorContents) (HANDLE hVolume, LARGE_INTEGER nSectorNo, 
	        wchar_t* lpDescr, LPLONG lpItemID);
        */

        public delegate bool XWFGetSectorContentsDelegate(IntPtr hVolume, Int64 nSectorNo
            , IntPtr lpDescr, out Int32 lpItemID);

        private static XWFGetSectorContentsDelegate _XWFGetSectorContents;

        public static readonly int XWFSectorContentsBufferLength = 512 * 2;
        //from the docs: 511 characters and a terminating null

        /*       
        Original:
        ---------
        typedef void (__stdcall *fptr_XWF_SelectVolumeSnapshot) (HANDLE hVolume);
        */

        public delegate void XWFSelectVolumeSnapshotDelegate(IntPtr hVolume);

        public static XWFSelectVolumeSnapshotDelegate XWFSelectVolumeSnapshot;

        /*       
        Original:
        ---------
        typedef DWORD (__stdcall *fptr_XWF_GetItemCount) (HANDLE hVolume);
        */

        public delegate UInt32 XWFGetItemCountDelegate(IntPtr hVolume);

        public static XWFGetItemCountDelegate XWFGetItemCount;

        /*       
        Original:
        ---------
        typedef long int (__stdcall *fptr_XWF_CreateItem) (wchar_t* lpName, DWORD flags); 
        */

        public delegate Int32 XWFCreateItemDelegate(
              [MarshalAs(UnmanagedType.LPWStr)] string lpName
            , UInt32 flags);

        public static XWFCreateItemDelegate XWFCreateItem;

        /*       
        Original:
        ---------
        typedef const wchar_t* (__stdcall *fptr_XWF_GetItemName) (LONG nItemID);
        */

        [return: MarshalAs(UnmanagedType.LPWStr)]
        public delegate string XWFGetItemNameDelegate(Int32 nItemID);

        public static XWFGetItemNameDelegate XWFGetItemName;

        /*       
        Original:
        ---------
        typedef LARGE_INTEGER (__stdcall *fptr_XWF_GetItemSize) (LONG nItemID); 
        */

        public delegate Int64 XWFGetItemSizeDelegate(Int32 nItemID);

        public static XWFGetItemSizeDelegate XWFGetItemSize;

        /*       
        Original:
        ---------
        typedef void (__stdcall *fptr_XWF_SetItemSize) (LONG nItemID, LARGE_INTEGER size); 
        */

        public delegate void XWFSetItemSizeDelegate(Int32 nItemID, Int64 size);

        public static XWFSetItemSizeDelegate XWFSetItemSize;

        /*       
        Original:
        ---------
        typedef void (__stdcall *fptr_XWF_GetItemOfs) (LONG nItemID, LARGE_INTEGER* lpDefOfs, 
	        LARGE_INTEGER* lpStartSector); 
        */

        public delegate void XWFGetItemOfsDelegate(Int32 nItemID, out Int64 lpDefOfs
            , out Int64 lpStartSector);

        public static XWFGetItemOfsDelegate XWFGetItemOfs;

        /*       
        Original:
        ---------
        typedef void (__stdcall *fptr_XWF_SetItemOfs) (LONG nItemID, LARGE_INTEGER nDefOfs, 
	        LARGE_INTEGER nStartSector); 
        */

        public delegate void XWFSetItemOfsDelegate(Int32 nItemID, Int64 nDefOfs,
            Int64 nStartSector);

        public static XWFSetItemOfsDelegate XWFSetItemOfs;

        /*       
        Original:
        ---------
        typedef LARGE_INTEGER (__stdcall *fptr_XWF_GetItemInformation) (LONG nItemID, 
            LONG InfoType, LPBOOL lpSuccess); 
        */

        public delegate Int64 XWFGetItemInformationDelegate(Int32 nItemID
            , XWFItemInformationType InfoType, out bool lpSuccess);

        public static XWFGetItemInformationDelegate XWFGetItemInformation;

        /*       
        Original:
        ---------
        typedef BOOL (__stdcall *fptr_XWF_SetItemInformation) (LONG nItemID, 
            LONG InfoType, LARGE_INTEGER nInfoValue);
        */

        public delegate bool XWFSetItemInformationDelegate(Int32 nItemID
            , XWFItemInformationType InfoType, Int64 nInfoValue);

        public static XWFSetItemInformationDelegate XWFSetItemInformation;

        /*       
        Original:
        ---------
        typedef DWORD (__stdcall *fptr_XWF_GetItemType) (LONG nItemID, wchar_t*lpTypeDescr, 
	        LONG nBufferLen); 
        */

        public delegate UInt32 XWFGetItemTypeDelegate(Int32 nItemID, IntPtr lpTypeDescr, 
	        Int32 nBufferLen);
        
        private static XWFGetItemTypeDelegate _XWFGetItemType;

        public static readonly int XWFItemTypeDescBufferLength = 1024 * 2;
        //the docs do not define the max length for type desc buffer

        /*       
        Original:
        ---------
        typedef void (__stdcall *fptr_XWF_SetItemType) (LONG nItemID, wchar_t*lpTypeDescr, 
	        LONG nTypeStatus); 
        */
        
        public delegate void XWFSetItemTypeDelegate(Int32 nItemID
            , [MarshalAs(UnmanagedType.LPWStr)] string lpTypeDescr
            , Int32 nTypeStatus);

        public static XWFSetItemTypeDelegate XWFSetItemType;

        /*       
        Original:
        ---------
        typedef LONG (__stdcall *fptr_XWF_GetItemParent) (LONG nItemID); 
        */

        public delegate Int32 XWFGetItemParentDelegate(Int32 nItemID);

        public static XWFGetItemParentDelegate XWFGetItemParent;

        /*       
        Original:
        ---------
        typedef void (__stdcall *fptr_XWF_SetItemParent) (LONG nChildItemID, LONG nParentItemID);
        */

        public delegate void XWFSetItemParentDelegate(Int32 nChildItemID, Int32 nParentItemID);

        public static XWFSetItemParentDelegate XWFSetItemParent;

        /*       
        Original:
        ---------
        typedef LONG (__stdcall *fptr_XWF_GetReportTableAssocs) (LONG nItemID, 
	        wchar_t* lpBuffer, LONG nBufferLen);
        */

        public delegate Int32 XWFGetReportTableAssocsDelegate(Int32 nItemID
            , IntPtr lpBuffer, Int32 nBufferLen);

        private static XWFGetReportTableAssocsDelegate _XWFGetReportTableAssocs;

        /*       
        Original:
        ---------
        typedef LONG (__stdcall *fptr_XWF_AddToReportTable) (LONG nItemID, 
	        wchar_t* lpReportTableName, DWORD nFlags); 
        */

        public delegate Int32 XWFAddToReportTableDelegate(Int32 nItemID
            , [MarshalAs(UnmanagedType.LPWStr)] string lpReportTableName
            , XWFAddToReportTableFlags nFlags);

        public static XWFAddToReportTableDelegate XWFAddToReportTable;

        /*       
        Original:
        ---------
        typedef wchar_t* (__stdcall *fptr_XWF_GetComment) (LONG nItemID);
        */        

        [return: MarshalAs(UnmanagedType.LPWStr)]
        public delegate string XWFGetCommentDelegate(Int32 nItemID);

        public static XWFGetCommentDelegate XWFGetComment;

        /*       
        Original:
        ---------
        typedef BOOL (__stdcall *fptr_XWF_AddComment) (LONG nItemID, wchar_t* lpComment, 
	        DWORD nHowToAdd); 
        */

        public delegate bool XWFAddCommentDelegate(Int32 nItemID
            , [MarshalAs(UnmanagedType.LPWStr)] string lpComment
            , XWFHowToAddComment nHowToAdd);

        public static XWFAddCommentDelegate XWFAddComment;

        /*       
        Original:
        ---------
        BOOL XWF_GetHashValue(LONG nItemID, LPVOID lpBuffer);
        */

        public delegate bool XWFGetHashValueDelegate(Int32 nItemID, IntPtr lpBuffer);

        public static XWFGetHashValueDelegate XWFGetHashValue;

        /*       
        Original:
        ---------
        typedef void (__stdcall * fptr_XWF_ShowProgress) (wchar_t* lpCaption, DWORD nFlags);
        */

        public delegate void XWFShowProgressDelegate(
              [MarshalAs(UnmanagedType.LPWStr)] string lpCaption
            , XWFShowProgressFlags nFlags);

        public static XWFShowProgressDelegate XWFShowProgress;

        /*       
        Original:
        ---------
        typedef void (__stdcall * fptr_XWF_SetProgressPercentage) (DWORD nPercent)
        */

        public delegate void XWFSetProgressPercentageDelegate(UInt32 nPercent);

        public static XWFSetProgressPercentageDelegate XWFSetProgressPercentage;

        /*       
        Original:
        ---------
        typedef void (__stdcall * fptr_XWF_SetProgressDescription) (wchar_t* lpStr); 
        */

        public delegate void XWFSetProgressDescriptionDelegate(
            [MarshalAs(UnmanagedType.LPWStr)] string lpStr);

        public static XWFSetProgressDescriptionDelegate XWFSetProgressDescription;

        /*       
        Original:
        ---------
        typedef BOOL (__stdcall * fptr_XWF_ShouldStop) (void);
        */

        public delegate bool XWFShouldStopDelegate();

        public static XWFShouldStopDelegate XWFShouldStop;

        /*       
        Original:
        ---------
        typedef void (__stdcall * fptr_XWF_HideProgress) (void);
        */

        public delegate void XWFHideProgressDelegate();

        public static XWFHideProgressDelegate XWFHideProgress;

        /*       
        Original:
        ---------
        HANDLE XWF_OpenItem(HANDLE hVolume, LONG nItemID, DWORD nFlags);
        */

        public delegate IntPtr XWFOpenItemDelegate(IntPtr hVolume, Int32 nItemID, UInt32 nFlags);

        public static XWFOpenItemDelegate XWFOpenItem;         

        /*       
        Original:
        ---------
        typedef void (__stdcall * fptr_XWF_Close) (HANDLE hVolumeOrItem);
        */

        public delegate void XWFCloseDelegate(IntPtr hVolumeOrItem);

        public static XWFCloseDelegate XWFClose;

        /*       
        Original:
        ---------
        typedef HANDLE (__stdcall * fptr_XWF_CreateEvObj) (DWORD nType, LONG nDiskID,
            LPWSTR lpPath, PVOID pReserved);
        */

        public delegate IntPtr XWFCreateEvObjDelegate(XWFEvidenceObjType nType
            , Int32 nDiskID, [MarshalAs(UnmanagedType.LPWStr)] string lpPath, IntPtr pReserved);

        public static XWFCreateEvObjDelegate XWFCreateEvObj;

        /*       
        Original:
        ---------
        typedef LONG (__stdcall * fptr_XWF_Search) (SearchInfo* SInfo, 
            CodePages* CPages);
        */

        public delegate Int32 XWFSearchDelegate(ref SearchInfo SInfo, ref CodePages CPages);
        public delegate Int32 XWFSearchWithPtrToPagesDelegate(ref SearchInfo SInfo, IntPtr CPages);

        public static XWFSearchDelegate XWFSearch;
        private static XWFSearchWithPtrToPagesDelegate _XWFSearchWithPtrToPages;

        /*       
        Original:
        ---------
        typedef HANDLE (__stdcall * fptr_XWF_CreateContainer) (LPWSTR lpFileName, 
            DWORD nFlags, LPVOID pReserved);
        */

        public delegate IntPtr XWFCreateContainerDelegate(
              [MarshalAs(UnmanagedType.LPWStr)] string lpFileName
            , XWFCreateContainerFlags nFlags, IntPtr pReserved);

        public static XWFCreateContainerDelegate XWFCreateContainer;

        /*       
        Original:
        ---------
        typedef LONG (__stdcall * fptr_XWF_CopyToContainer) (HANDLE hContainer, 
            HANDLE hItem, DWORD nFlags, DWORD nMode, LARGE_INTEGER nStartOfs, 
            LARGE_INTEGER nEndOfs, LPVOID pReserved);
        */

        public delegate Int32 XWFCopyToContainerDelegate(IntPtr hContainer, IntPtr hItem
            , XWFCopyToContainerFlags nFlags
            , XWFCopyToContainerMode nMode
            , Int64 nStartOfs, Int64 nEndOfs, IntPtr pReserved);

        public static XWFCopyToContainerDelegate XWFCopyToContainer;

        /*       
        Original:
        ---------
        typedef LONG (__stdcall * fptr_XWF_CloseContainer) (HANDLE hContainer, 
            LPVOID pReserved);
        */

        public delegate Int32 XWFCloseContainerDelegate(IntPtr hContainer,
            IntPtr pReserved);

        public static XWFCloseContainerDelegate XWFCloseContainer;

        //---------------------------------------------------------------------
        //                      Code for importing methods
        //---------------------------------------------------------------------
        
        private static T GetMethodDelegate<T>(IntPtr moduleHandle, string methodName) 
            where T : class
        {
            var ptr = NativeMethods.GetProcAddress(moduleHandle, methodName);

            if (ptr != IntPtr.Zero)
                return Marshal.GetDelegateForFunctionPointer(ptr, typeof(T)) as T;

            throw new ArgumentException(methodName + " not found!");
        }

        public static bool Import()
        /*
           - Import() retreives the X-Tensions API function pointers
           - it should be called once upon startup (in XT_Init)
           - returns true upon success, false upon failure
        */
        {
            try
            {            
                var moduleHandle = NativeMethods.GetModuleHandle(IntPtr.Zero);

                XWFOutputMessage = GetMethodDelegate<XWFOutputMessageDelegate>(
                    moduleHandle, "XWF_OutputMessage");
            
                _XWFGetVolumeName = GetMethodDelegate<XWFGetVolumeNameDelegate>(
                    moduleHandle, "XWF_GetVolumeName");

                XWFCreateFile = GetMethodDelegate<XWFCreateFileDelegate>(
                    moduleHandle, "XWF_CreateFile");

                XWFGetSize = GetMethodDelegate<XWFGetSizeDelegate>(
                    moduleHandle, "XWF_GetSize");

                XWFRead = GetMethodDelegate<XWFReadDelegate>(
                    moduleHandle, "XWF_Read");

                _XWFGetVolumeInformation = GetMethodDelegate<XWFGetVolumeInformationDelegate>(
                    moduleHandle, "XWF_GetVolumeInformation");

                _XWFGetSectorContents = GetMethodDelegate<XWFGetSectorContentsDelegate>(
                    moduleHandle, "XWF_GetSectorContents");

                XWFSelectVolumeSnapshot = GetMethodDelegate<XWFSelectVolumeSnapshotDelegate>(
                    moduleHandle, "XWF_SelectVolumeSnapshot");

                XWFGetItemCount = GetMethodDelegate<XWFGetItemCountDelegate>(
                    moduleHandle, "XWF_GetItemCount");

                XWFCreateItem = GetMethodDelegate<XWFCreateItemDelegate>(
                    moduleHandle, "XWF_CreateItem");

                XWFGetItemName = GetMethodDelegate<XWFGetItemNameDelegate>(
                    moduleHandle, "XWF_GetItemName");

                XWFGetItemSize = GetMethodDelegate<XWFGetItemSizeDelegate>(
                    moduleHandle, "XWF_GetItemSize");

                XWFSetItemSize = GetMethodDelegate<XWFSetItemSizeDelegate>(
                    moduleHandle, "XWF_SetItemSize");

                XWFGetItemOfs = GetMethodDelegate<XWFGetItemOfsDelegate>(
                    moduleHandle, "XWF_GetItemOfs");

                XWFSetItemOfs = GetMethodDelegate<XWFSetItemOfsDelegate>(
                    moduleHandle, "XWF_SetItemOfs");

                XWFGetItemInformation = GetMethodDelegate<XWFGetItemInformationDelegate>(
                    moduleHandle, "XWF_GetItemInformation");

                XWFSetItemInformation = GetMethodDelegate<XWFSetItemInformationDelegate>(
                    moduleHandle, "XWF_SetItemInformation");

                _XWFGetItemType = GetMethodDelegate<XWFGetItemTypeDelegate>(
                    moduleHandle, "XWF_GetItemType");

                XWFSetItemType = GetMethodDelegate<XWFSetItemTypeDelegate>(
                    moduleHandle, "XWF_SetItemType");

                XWFGetItemParent = GetMethodDelegate<XWFGetItemParentDelegate>(
                    moduleHandle, "XWF_GetItemParent");

                XWFSetItemParent = GetMethodDelegate<XWFSetItemParentDelegate>(
                    moduleHandle, "XWF_SetItemParent");

                _XWFGetReportTableAssocs = GetMethodDelegate<XWFGetReportTableAssocsDelegate>(
                    moduleHandle, "XWF_GetReportTableAssocs");

                XWFAddToReportTable = GetMethodDelegate<XWFAddToReportTableDelegate>(
                    moduleHandle, "XWF_AddToReportTable");

                XWFGetComment = GetMethodDelegate<XWFGetCommentDelegate>(
                    moduleHandle, "XWF_GetComment");

                XWFAddComment = GetMethodDelegate<XWFAddCommentDelegate>(
                    moduleHandle, "XWF_AddComment");

                XWFGetHashValue = GetMethodDelegate<XWFGetHashValueDelegate>(
                    moduleHandle, "XWF_GetHashValue");

                XWFShowProgress = GetMethodDelegate<XWFShowProgressDelegate>(
                    moduleHandle, "XWF_ShowProgress");

                XWFSetProgressPercentage = GetMethodDelegate<XWFSetProgressPercentageDelegate>(
                    moduleHandle, "XWF_SetProgressPercentage");

                XWFSetProgressDescription = GetMethodDelegate<XWFSetProgressDescriptionDelegate>(
                    moduleHandle, "XWF_SetProgressDescription");

                XWFShouldStop = GetMethodDelegate<XWFShouldStopDelegate>(
                    moduleHandle, "XWF_ShouldStop");

                XWFHideProgress = GetMethodDelegate<XWFHideProgressDelegate>(
                    moduleHandle, "XWF_HideProgress");

                XWFOpenItem = GetMethodDelegate<XWFOpenItemDelegate>(
                    moduleHandle, "XWF_OpenItem");

                XWFClose = GetMethodDelegate<XWFCloseDelegate>(
                    moduleHandle, "XWF_Close");

                XWFCreateEvObj = GetMethodDelegate<XWFCreateEvObjDelegate>(
                    moduleHandle, "XWF_CreateEvObj");

                XWFSearch = GetMethodDelegate<XWFSearchDelegate>(
                    moduleHandle, "XWF_Search");

                _XWFSearchWithPtrToPages = GetMethodDelegate<XWFSearchWithPtrToPagesDelegate>(
                    moduleHandle, "XWF_Search");

                XWFCreateContainer = GetMethodDelegate<XWFCreateContainerDelegate>(
                    moduleHandle, "XWF_CreateContainer");

                XWFCopyToContainer = GetMethodDelegate<XWFCopyToContainerDelegate>(
                    moduleHandle, "XWF_CopyToContainer");

                XWFCloseContainer = GetMethodDelegate<XWFCloseContainerDelegate>(
                    moduleHandle, "XWF_CloseContainer");
            }
            catch
            {
                return false;
            }

            return true;            
        }

        //---------------------------------------------------------------------
        //                              Wrappers
        //---------------------------------------------------------------------        

        public static string XWFGetVolumeName(IntPtr hVolume, XWFVolumeNameType nType)
        {
            if (_XWFGetVolumeName == null || hVolume == IntPtr.Zero) return null;
            
            var bufferPtr = Marshal.AllocHGlobal(XWFVolumeNameBufferLength);
            _XWFGetVolumeName(hVolume, bufferPtr, nType);
            var str = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            return str;            
        }

        public static Int32 XWFGetReportTableAssocs(Int32 nItemID, out string associations)
        {            
            const int bufferLengthStep = 128;
            for (var bufferLength = bufferLengthStep; ; bufferLength += bufferLengthStep)
            {
                var bufferPtr = Marshal.AllocHGlobal(bufferLength);
                var associationsCount = _XWFGetReportTableAssocs(nItemID, bufferPtr, bufferLength);
                
                if (associationsCount <= 0)
                {
                    associations = string.Empty;
                    return associationsCount;
                }

                var str = Marshal.PtrToStringUni(bufferPtr, bufferLength);                
                Marshal.FreeHGlobal(bufferPtr);

                var nullCharIndex = str.IndexOf((char) 0);
                if (nullCharIndex < 0 || nullCharIndex >= bufferLength - 1) continue;

                associations = str.Substring(0, nullCharIndex);
                return associationsCount;
            }
        }

        public static bool XWFGetSectorContents(IntPtr hVolume, Int64 nSectorNo
            , out string description, out Int32 itemId)
        {
            var bufferPtr = Marshal.AllocHGlobal(XWFSectorContentsBufferLength);
            bool result = _XWFGetSectorContents(hVolume, nSectorNo, bufferPtr, out itemId);
            description = Marshal.PtrToStringUni(bufferPtr);
            Marshal.FreeHGlobal(bufferPtr);

            return result;
        }

        public static UInt32 XWFGetItemType(Int32 itemId)
        {
            return _XWFGetItemType(itemId, IntPtr.Zero, 0);
        }

        public static UInt32 XWFGetItemType(Int32 itemId, out string typeDescription)
        {
            var bufferPtr = Marshal.AllocHGlobal(XWFItemTypeDescBufferLength);
            var result = _XWFGetItemType(itemId, bufferPtr, XWFItemTypeDescBufferLength);            
            typeDescription = Marshal.PtrToStringUni(bufferPtr);            
            Marshal.FreeHGlobal(bufferPtr);

            return result;            
        }

        public static VolumeInformation XWFGetVolumeInformation(IntPtr hVolume)
        {
            var info = new VolumeInformation();
            
            _XWFGetVolumeInformation(hVolume, out info.FileSystem
                , out info.BytesPerSector, out info.SectorsPerCluster
                , out info.ClusterCount, out info.FirstClusterSectorNo);

            return info;
        }

        public static Int32 XWFSearchWithoutCodePages(ref SearchInfo SInfo)
        {
            return _XWFSearchWithPtrToPages(ref SInfo, IntPtr.Zero);
        }
    }

    static class NativeMethods
    {                        
        //lpModuleName is declared as IntPtr in order to be able to pass NULL through it
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(IntPtr lpModuleName);
        
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    }
}
