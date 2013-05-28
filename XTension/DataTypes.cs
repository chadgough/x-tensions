using System;
using System.Runtime.InteropServices;

namespace XTension
{
    /*
    Most common type conversions:
    
    C++ type       C# equivalent
    -------------  -------------
    HANDLE         IntPtr
    DWORD          UInt32, uint
    WORD           UInt16
    LONG           Int32, int
    LARGE_INTEGER  Int64
    */

    //---------------------------------------------------------------------
    //                  Data types used by exported methods
    //---------------------------------------------------------------------

    /*
    Original:
    ---------
    struct CallerInfo
    {
        byte lang, ServiceRelease;
        WORD version;
    };
    */

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct CallerInfo
    {
        public byte lang;
        public byte ServiceRelease;
        public UInt16 version;
    }    

    [Flags]
    public enum XTInitFlags : uint
    {
          XT_INIT_XWF = 0x00000001u //X-Ways Forensics
        , XT_INIT_WHX = 0x00000002u //WinHex
        , XT_INIT_XWI = 0x00000004u //X-Ways Investigator
        , XT_INIT_BETA = 0x00000008u //beta version
        , XT_INIT_QUICKCHECK = 0x00000020u //called just to check whether the API accepts the calling application (used by v16.5 and later)
        , XT_INIT_ABOUTONLY = 0x00000040u //called just to prepare for XT_About (used by v16.5 and later)
    }

    [Flags]
    public enum XTActionType : uint
    {
          XT_ACTION_RUN = 0u //simply run directly from the main menu, not for any particular volume, since v16.6
        , XT_ACTION_RVS = 1u //volume snapshot refinement starting
        , XT_ACTION_LSS = 2u //logical simultaneous search starting
        , XT_ACTION_PSS = 3u //physical simultaneous search starting
        , XT_ACTION_DBC = 4u //directory browser context menu command invoked
        , XT_ACTION_SHC = 5u //search hit context menu command invoked
    }

    /*
    Original:
    ---------
    #pragma pack(2)
    struct PrepareSearchInfo {
       LONG iSize,
       LPWSTR lpSearchTerms,
       DWORD nBufLen,
       DWORD nFlags
    };
    */

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct PrepareSearchInfo
    {
        public Int32 iSize;
        
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpSearchTerms;

        public UInt32 nBufLen;
        public PrepareSearchInfoFlags nFlags;
    };

    [Flags]
    public enum PrepareSearchInfoFlags : uint
    {
          XWF_SEARCH_MATCHCASE = 0x00000010u //match case
        , XWF_SEARCH_WHOLEWORDS = 0x00000020u //whole words only
        , XWF_SEARCH_GREP = 0x00000040u //GREP syntax
        , XWF_SEARCH_WHOLEWORDS2 = 0x00004000u //whole words only for search terms that are specially marked
        , XWF_SEARCH_GREP2 = 0x00008000u //GREP syntax only search terms that start with "grep:"
    }

    /*
    Original:
    ---------
    #pragma pack(2)
    struct SearchHitInfo {
       LONG iSize;
       LONG nItemID;
       LARGE_INTEGER nRelOfs;
       LARGE_INTEGER nAbsOfs;
       void* lpOptionalHitPtr;
       WORD lpSearchTermID;
       WORD nLength;
       WORD nCodePage;
       WORD nFlags;
    };
    */

    [StructLayout(LayoutKind.Sequential, Pack=2)]
    public struct SearchHitInfo
    {
        public Int32 iSize;
        public Int32 nItemID;
        public Int64 nRelOfs;
        public Int64 nAbsOfs;
        public IntPtr lpOptionalHitPtr;
        public UInt16 lpSearchTermID;
        public UInt16 nLength;
        public UInt16 nCodePage;
        public SearchHitInfoFlags nFlags;
    };

    [Flags]
    public enum SearchHitInfoFlags : ushort
    {
          ResidesInTheExtractedText = 0x0001 //resides in the text that was extracted from the file, nRelOfs is not an offset in the file
        , Notable = 0x0002 //notable
        , Deleted = 0x0008 //deleted, set to discard the search hit
        , IndexSearchHit = 0x0040 //index search hit
        , InSlackSpace = 0x0080 //in slack space etc.
    }

    //---------------------------------------------------------------------
    //                  Data types used by imported methods
    //---------------------------------------------------------------------

    [Flags]
    public enum XWFOutputMessageFlags : uint
    {
          NoLineBreak = 0x00000001u //append without line break
        , DoNotLogError = 0x00000002u //don't log this error message
        , Ansi = 0x00000004u //lpMessage points to an ANSI string, not a Unicode string (v16.5 and later)
    }

    /*
    from the docs:
        3 types of names are available (1, 2 or 3).
        For example, 3 can be more generic than 2 ("Hard disk 1" instead of "WD12345678"). 
    */
    public enum XWFVolumeNameType : uint
    {
          Type1 = 1u
        , Type2 = 2u
        , Type3 = 3u
    }

    public enum XWFVolumeFileSystem : int
    {
          MainMemory = 9
        , CDFS = 8
        , ViaOS = 7
        , XWFS = 6
        , UDF = 5
        , exFAT = 4
        , FAT32 = 3
        , FAT16 = 2
        , FAT12 = 1
        , Unknown = 0
        , NTFS = -1
        , HPFS = -2
        , Ext2 = -3
        , Ext3 = -4
        , ReiserFS = -5
        , Reiser4 = -6
        , Ext4 = -7
        , JFS = -9
        , XFS = -10
        , UFS = -11
        , HFS = -12
        , HFSPlus = -13
        , NTFSBitlocker = -15
        , PartitionedDisks = -16
    }

    [Flags]
    public enum XWFCreateFileFlags : uint
    {
          ExcerptFromParent = 0x00000002u //Create a file that is defined as an excerpt from its parent, where pExtraInfo points to a 64-bit start offset within that parent file.
        , AttachExternalFile = 0x00000004u //Attach an external file, and pExtraInfo is an LPWSTR pointer to the path of that file.
        , KeepExternalFile = 0x00000008u //Keep that external file that you designate if you still need it yourself after calling this function. Performance in v16.7 is best if this flag is not set and if the external file resides in the same file system as the case. 
    }

    /*
    Original:
    ---------
    #pragma pack(2)
    struct SearchInfo {
       LONG iSize;
       HANDLE hVolume;
       LPWSTR lpSearchTerms;
       DWORD nFlags;
       DWORD nSearchWindow;
    };
    */

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct SearchInfo
    {
        public Int32 iSize;
        public IntPtr hVolume;
        
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpSearchTerms;        

        public SearchInfoFlags nFlags;
        public UInt32 nSearchWindow;
    };

    /*
    Original:
    ---------    
    #pragma pack(2)
    struct CodePages {
       LONG iSize;
       WORD nCodePage1;
       WORD nCodePage2;
       WORD nCodePage3;
       WORD nCodePage4;
       WORD nCodePage5;
    };
    */

    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct CodePages
    {
        public Int32 iSize;
        public UInt16 nCodePage1;
        public UInt16 nCodePage2;
        public UInt16 nCodePage3;
        public UInt16 nCodePage4;
        public UInt16 nCodePage5;
    };

    [Flags]
    public enum SearchInfoFlags : uint
    {
          XWF_SEARCH_LOGICAL = 0x00000001u //logical search instead of physical search (only logical search currently available)
        , XWF_SEARCH_TAGGEDOBJ = 0x00000004u //tagged objects in volume snapshot only
        , XWF_SEARCH_MATCHCASE = 0x00000010u //match case
        , XWF_SEARCH_WHOLEWORDS = 0x00000020u //whole words only
        , XWF_SEARCH_GREP = 0x00000040u //GREP syntax
        , XWF_SEARCH_OVERLAPPED = 0x00000080u //allow overlapping hits
        , XWF_SEARCH_COVERSLACK = 0x00000100u //cover slack space
        , XWF_SEARCH_COVERSLACKEX = 0x00000200u //cover slack/free space transition
        , XWF_SEARCH_DECODETEXT = 0x00000400u //decode text in standard file types
        , XWF_SEARCH_DECODETEXTEX = 0x00000800u //decode text in specified file types // not yet supported 
        , XWF_SEARCH_1HITPERFILE = 0x00001000u //1 hit per file needed only
        , XWF_SEARCH_OMITIRRELEVANT = 0x00010000u //omit files classified as irrelevant
        , XWF_SEARCH_OMITHIDDEN = 0x00020000u //omit hidden files
        , XWF_SEARCH_OMITFILTERED = 0x00040000u //omit files that are filtered out
        , XWF_SEARCH_DATAREDUCTION = 0x00080000u //recommendable data reduction
        , XWF_SEARCH_OMITDIRS = 0x00100000u //omit directories
        , XWF_SEARCH_CALLPSH = 0x01000000u //X-Ways Forensics will call XT_ProcessSearchHit, if exported, for each hit
        , XWF_SEARCH_DISPLAYHITS = 0x04000000u //display search hit list when the search completes
    }

    public enum XWFItemInformationType : int
    {
          XWF_ITEM_INFO_ORIG_ID = 1
        , XWF_ITEM_INFO_ATTR = 2
        , XWF_ITEM_INFO_FLAGS = 3
        , XWF_ITEM_INFO_DELETION = 4
        , XWF_ITEM_INFO_CLASSIFICATION = 5 //e.g. extracted e-mail message, alternate data stream, etc.
        , XWF_ITEM_INFO_LINKCOUNT = 6 //hard-link count
        , XWF_ITEM_INFO_FILECOUNT = 11 //how many child objects exist recursively that are files
        , XWF_ITEM_INFO_CREATIONTIME = 32
        , XWF_ITEM_INFO_MODIFICATIONTIME = 33
        , XWF_ITEM_INFO_LASTACCESSTIME = 34
        , XWF_ITEM_INFO_ENTRYMODIFICATIONTIME = 35
        , XWF_ITEM_INFO_DELETIONTIME = 36
        , XWF_ITEM_INFO_INTERNALCREATIONTIME = 37
        , XWF_ITEM_INFO_FLAGS_SET = 64 //indicates only flags that should be set, others remain unchanged
        , XWF_ITEM_INFO_FLAGS_REMOVE = 65 //indicates flags that should be removed, others remain unchanged
    }

    [Flags]
    public enum XWFAddToReportTableFlags : uint
    {
          CreatedByApplication = 0x01u //show as created by application, not by examiner
        , IncludeInReport = 0x02u //select for inclusion in report
        , Filtering = 0x04u //select for filtering
        , FutureManualReportTableAssociations = 0x08u //select for future manual report table associations
    }

    public enum XWFHowToAddComment : uint
    {
          Replace = 0u //replace any existing comment
        , Append = 1u //append to any existing comment
        , AppendLine = 2u //append to any existing comment and use line break as delimiter
    }

    [Flags]
    public enum XWFShowProgressFlags : uint
    {
          WindowOnly = 0x00000001u //show just the window, no actual progress bar
        , DisallowInterrupting = 0x00000002u //do not allow the user to interrupt the operation
        , ShowImmediately = 0x00000004u //show window immediately
        , DoubleConfirmAbort = 0x00000008u //double-confirm abort
        , PreventLogging = 0x00000010u //prevent logging
    }

    public enum XWFEvidenceObjType : uint
    {
          File = 0u
        , Image = 1u
        , MemoryDump = 2u
        , Directory = 3u
        , Disk = 4u
    }

    [Flags]
    public enum XWFCreateContainerFlags : uint
    {
          XWF_CTR_OPEN = 0x00000001u //opens an existing container, all other flags ignored
        , XWF_CTR_XWFS2 = 0x00000002u //use new XWFS2 file system
        , XWF_CTR_SECURE = 0x00000004u //mark this container as to be filled indirectly/secure
        , XWF_CTR_TOPLEVEL = 0x00000008u //include evidence object names as top directory level
        , XWF_CTR_INCLDIRDATA = 0x00000010u //include directory data
        , XWF_CTR_FILEPARENTS = 0x00000020u //allow files as parents of files
        , XWF_CTR_USERREPORTTABLES = 0x00000100u //export associations with user-created report table
        , XWF_CTR_SYSTEMREPORTTABLES = 0x00000200u //export associations with system-created report tables (currently requires 0x100)
        , XWF_CTR_ALLCOMMENTS = 0x00000800u //pass on comments
        , XWF_CTR_OPTIMIZE1 = 0x00001000u //optimize for > 1,000 items
        , XWF_CTR_OPTIMIZE2 = 0x00002000u //optimize for > 50,000 items
        , XWF_CTR_OPTIMIZE3 = 0x00004000u //optimize for > 250,000 items
        , XWF_CTR_OPTIMIZE4 = 0x00008000u //optimize for > 1 million items
    }

    [Flags]
    public enum XWFCopyToContainerFlags : uint
    {
          RecreateOriginalPath = 0x00000001u //recreate full original path
        , IncludeParentItemData = 0x00000002u //include parent item data (requires flag 0x1)
        , StoreHash = 0x00000004u //store hash value in container
    }

    public enum XWFCopyToContainerMode : uint
    {
          LogicalContents = 0u //copy logical file contents only
        , PhysicalContents = 1u //copy physical file contents (not supported)
        , LogicalContentsAndSlack = 2u //logical contents and file slack separately
        , Slack = 3u //copy slack only
        , Range = 4u //copy range only (the last 2 parameters, which are otherwise ignored)
        , Metadata = 5u //copy metadata only
    }

    //a struct for more convinent use of XWFGetVolumeInformation
    public struct VolumeInformation
    {
        public XWFVolumeFileSystem FileSystem;
        public UInt32 BytesPerSector;
        public UInt32 SectorsPerCluster;
        public Int64 ClusterCount;
        public Int64 FirstClusterSectorNo;
    }
}
