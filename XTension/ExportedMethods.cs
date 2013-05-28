using System;
using System.Runtime.InteropServices;
using RGiesecke.DllExport;

/*
 * In order to to allow the DLL functions to be exported 
 * Robert Giesecke's Unmanaged Exports solution is used
 * https://nuget.org/packages/UnmanagedExports 
*/

namespace XTension
{
    public class ExportedMethods
    {
        /*
        Original:
        ---------
        LONG __stdcall XT_Init(CallerInfo info, DWORD nFlags, HANDLE hMainWnd,
            void* lpReserved);
        */

        [DllExport]
        public static Int32 XT_Init(CallerInfo nVersion, XTInitFlags nFlags, IntPtr hMainWnd
            , IntPtr lpReserved)
        {           
            /*
            //you can check for flag presence in nFlags
            if ((nFlags & XTInitFlags.XT_INIT_XWF) > 0)
            {
                //called by X-Ways Forensics
            }

            if ((nFlags & XTInitFlags.XT_INIT_BETA) > 0)
            {
                //called by a Beta version of the application
            }

            if ((nFlags & XTInitFlags.XT_INIT_QUICKCHECK) > 0)
            {
                //called just to check whether the API accepts the calling application
            }
            */

            /*
            Importing functions:
                if importing functions has failed we return -1 
                and prevent further use of the DLL
            */
            if (!ImportedMethods.Import()) return -1;

            ImportedMethods.XWFOutputMessage(String.Format("C# Dll: XT_Init called,"
               + "nVersion[Version: {0}, ServiceRelease: {1}, Language: {2}], "
               + "nFlags = {3}, hMainWnd = {4}"
               , nVersion.version
               , nVersion.ServiceRelease
               , nVersion.lang
               , nFlags
               , hMainWnd));

            //from the docs:
            //to prevent further use of the DLL return value should be -1
            //otherwise return value should be 1
            return 1;
        }
        
        /*
        Original:
        ---------
        LONG __stdcall XT_Done(void* lpReserved)
        */

        [DllExport]
        public static Int32 XT_Done(IntPtr lpReserved)
        {
            ImportedMethods.XWFOutputMessage("C# Dll: XT_Done called");
            return 0;
        }

        /*
        Original:
        ---------
        LONG __stdcall XT_About(HANDLE hParentWnd, void* lpReserved)
        */

        [DllExport]
        public static Int32 XT_About(IntPtr hParentWnd, IntPtr lpReserved)
        {
            ImportedMethods.XWFOutputMessage("C# Dll: XT_About called");
            return 0;
        }

        /*
        Original:
        ---------
        LONG __stdcall XT_Prepare(HANDLE hVolume, HANDLE hEvidence, DWORD nOpType, 
            void* lpReserved)
        */

        private static IntPtr _currentVolumeHandle;
        private static XTActionType? _currentAction = null;

        [DllExport]
        public static Int32 XT_Prepare(IntPtr hVolume, IntPtr hEvidence
            , XTActionType nOpType, IntPtr lpReserved)
        {
            /*            
            Note:
                XT_Prepare may get called with a zero handle, 
                which means there is no volume to prepare for.
                So before calling any function on hVolume passed from XT_Prepare
                you have to check that it is not zero: hVolume != IntPtr.Zero                       
            */
            
            //storing the volume handle for further use in ProcessItemEx
            _currentVolumeHandle = hVolume;

            //storing the current action type, so we can check it in ProcessItemEx
            _currentAction = nOpType;

            //XT_Prepare parameters
            ImportedMethods.XWFOutputMessage(string.Format(
                  "C# Dll: XT_Prepare called, hVolume = {0}, nOpType = {1}"
                , hVolume, nOpType));

            if (hVolume != IntPtr.Zero)
            {                            
                //XWFGetVolumeInformation
                var volumeInformation = ImportedMethods.XWFGetVolumeInformation(hVolume);

                ImportedMethods.XWFOutputMessage(string.Format(
                      "XWF_GetVolumeInformation: fileSystem = {0}, bytesPerSector = {1}"
                    + " , sectorsPerCluster = {2}, clusterCount = {3}, firstClusterSectorNo = {4}"
                    , volumeInformation.FileSystem, volumeInformation.BytesPerSector
                    , volumeInformation.SectorsPerCluster, volumeInformation.ClusterCount
                    , volumeInformation.FirstClusterSectorNo));

                //XWFGetSectorContents
                string sectorDesc;
                Int32 sectorItemId;

                bool sectorIsUsed = ImportedMethods.XWFGetSectorContents(hVolume
                    , volumeInformation.FirstClusterSectorNo
                    , out sectorDesc, out sectorItemId);

                ImportedMethods.XWFOutputMessage(string.Format(
                      "XWF_GetSectorContents: Sector Description = {0}, Sector Item Id = {1}, Sector Is Used = {2}"
                    , sectorDesc, sectorItemId, sectorIsUsed));

                //XWFGetVolumeName       
                ImportedMethods.XWFOutputMessage("XWF_GetVolumeName: Volume Name = " 
                    + ImportedMethods.XWFGetVolumeName(hVolume, XWFVolumeNameType.Type3));
            }
            /*
            from the docs:

            Negative return values:
            -4 if you want X-Ways Forensics to stop the whole operation (e.g. volume snapshot refinement) altogether
            -3 if you want to prevent further use of the X-Tension for the remainder of the whole operation, for example because your X-Tension is not supposed to do anything for that kind of operation as indicated by nOpType.
            -2 if you want this particular volume excluded from the operation
            -1 if you don't want other functions of this X-Tension to be called for this particular volume, not even XT_Finalize

            Positive return values/combination of flags:
            0x00 default, if you just want XT_Finalize to be called, will also be assumed if you do not export XT_Prepare
            0x01 of you want X-Ways Forensics to call your implementation of XT_ProcessItem or XT_ProcessItemEx (whichever is exported) for each item this volume snapshot
            0x02 in case of XT_ACTION_RVS, same, but to receive calls of XT_ProcessItem (if exported) after all other individual item refinement operations instead of before
            0x04 in case of XT_ACTION_RVS, to signal XWF that you may create more items in the volume snapshot, so that for example the user will definitely be informed of how many item were added (v16.5 and later only)

            Full return value evaluation only for XT_ACTION_RVS.
            */

            return 1; //1 - to call XT_ProcessItemEx
        }

        /*
        Original:
        ---------
        LONG __stdcall XT_Finalize(HANDLE hVolume, HANDLE hEvidence, DWORD nOpType, 
            void* lpReserved)
        */

        [DllExport]
        public static Int32 XT_Finalize(IntPtr hVolume, IntPtr hEvidence, XTActionType nOpType
            , IntPtr lpReserved)
        {
            ImportedMethods.XWFOutputMessage("C# Dll: XT_Finalize called");
            
            //indicating that there is no current action executing
            _currentAction = null;
            return 0;
        }

        /*
        Original:
        ---------
        LONG __stdcall XT_ProcessItem(LONG nItemID, void* lpReserved)
        */

        //Note: 
        //XT_ProcessItem is commented out since XT_ProcessItemEx is used
        //You should not export both at the same time

        /*
        [DllExport]
        public static long XT_ProcessItem(long nItemID, IntPtr lpReserved)
        {
            ImportedMethods.XWFOutputMessage("C# Dll: XT_ProcessItem called");

            //from the docs: Return -1 if you want X-Ways Forensics to stop the current operation (e.g. volume snapshot refinement), -2 if you want have X-Ways Forensics skip all other volume snapshot refinement operations for this file, otherwise 0.
            return 0;
        }
        */

        /*
        Original:
        ---------
        LONG __stdcall XT_ProcessItemEx(LONG nItemID, HANDLE hItem, void* lpReserved)
        */        

        [DllExport]
        public static Int32 XT_ProcessItemEx(Int32 nItemID, IntPtr hItem, IntPtr lpReserved)
        {
            ImportedMethods.XWFOutputMessage(string.Format(
                    "C# Dll: XT_ProcessItemEx called, nItemID = {0}, hItem = {1}"
                , nItemID, hItem));

            //storing the item name for further use
            var itemName = ImportedMethods.XWFGetItemName(nItemID);

            ImportedMethods.XWFOutputMessage("XWF_GetItemName: Item name = " + itemName);
            ImportedMethods.XWFOutputMessage("Full Path: " + HelperMethods.GetFullPath(nItemID));                
            ImportedMethods.XWFOutputMessage("XWF_GetComment: " + ImportedMethods.XWFGetComment(nItemID));

            string associations;
            ImportedMethods.XWFOutputMessage("XWF_GetReportTableAssocs: total number of associations of the file = "
                + ImportedMethods.XWFGetReportTableAssocs(nItemID, out associations));
            ImportedMethods.XWFOutputMessage(", associations = " + associations, XWFOutputMessageFlags.NoLineBreak);

            //reading & processing file contents
            var contents = HelperMethods.ReadItem(hItem);
            if (contents == null)
            {
                ImportedMethods.XWFOutputMessage("Failed to read item contents");
            }
            else
            {
                ImportedMethods.XWFOutputMessage("Item contents read successfully."); 
                //now you can analyze item contents   
            }            
            
            /*
            from the docs:
                Return -1 if you want X-Ways Forensics to stop the current operation 
                (e.g. volume snapshot refinement), otherwise 0.
            */
            return 0;
        }

        /*
        Original:
        ---------
        LONG XT_PrepareSearch(struct PrepareSearchInfo* PSInfo, struct CodePages* CPages);
        */

        [DllExport]
        public static Int32 XT_PrepareSearch(ref PrepareSearchInfo PSInfo, IntPtr CPages)
        {                                    
            ImportedMethods.XWFOutputMessage(string.Format(
                  "C# Dll: XT_PrepareSearch called, PSInfo.lpSearchTerms = {0}, PSInfo.nFlags = {1}"
                , PSInfo.lpSearchTerms, PSInfo.nFlags));            

            //from the docs: CPages == IntPtr.Zero means unused code page
            if (CPages != IntPtr.Zero)
            {
                var codePages = (CodePages)Marshal.PtrToStructure(CPages, typeof(CodePages));
                
                ImportedMethods.XWFOutputMessage(string.Format(", CPages = {0}, {1}, {2}, {3}, {4}"
                    , codePages.nCodePage1
                    , codePages.nCodePage2
                    , codePages.nCodePage3
                    , codePages.nCodePage4
                    , codePages.nCodePage5), XWFOutputMessageFlags.NoLineBreak);
            }

            /*
            //Adjusting search terms
            PSInfo.lpSearchTerms = PSInfo.lpSearchTerms + "adjusted";
            PSInfo.nBufLen = (uint)(PSInfo.lpSearchTerms.Length + 1);
            return 1;
            */

            /*
            from the docs:
            
            Return 1 if you have made adjustments to the search terms,
            or 0 if not, or -1 if you are not happy with the current settings at all and want the X-Tension 
            to be unselected. Adjustments to the flags or the code pages are ignored.
            */
            return 0;
        }


        /*
        Original:
        ---------
        LONG __stdcall XT_ProcessSearchHit(struct SearchHitInfo* info)
        */

        [DllExport]
        public static Int32 XT_ProcessSearchHit(ref SearchHitInfo info)
        {
            ImportedMethods.XWFOutputMessage(string.Format(
                  "C# Dll: XT_ProcessSearchHit called, info.nItemID = {0}, info.nFlags = {1}"
                , info.nItemID, info.nFlags));

            /*
            from the docs:
            
            Return 0, except if you want X-Ways Forensics to abort the search (return -1)
            or if you want X-Ways Forensics to stop calling you (return -2).
            */
            return 0;
        }
    }
}
