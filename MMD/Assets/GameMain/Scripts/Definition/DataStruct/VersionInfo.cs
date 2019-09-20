//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace Penny
{
    public class VersionInfo
    {
        public bool GameNeedUpdate
        {
            get;
            set;
        }

        public string LatestGameVersion
        {
            get;
            set;
        }

        public int InternalApplicationVersion
        {
            get;
            set;
        }

        public int InternalResourceVersion
        {
            get;
            set;
        }

        public string GameUpdateUrl
        {
            get;
            set;
        }

        public int VersionListLength
        {
            get;
            set;
        }

        public int VersionListHashCode
        {
            get;
            set;
        }

        public int VersionListZipLength
        {
            get;
            set;
        }

        public int VersionListZipHashCode
        {
            get;
            set;
        }

        public string GameAppUrl
        {
            get;
            set;
        }
    }
}
