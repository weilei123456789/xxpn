
namespace GameFramework.Resource
{
    public sealed partial class CoursewareUpdate
    {
        private readonly CoursewareResourceName m_ResourceName;
        private readonly int m_LoadType;
        private readonly int m_Length;
        private readonly int m_HashCode;
        private readonly int m_ZipLength;
        private readonly int m_ZipHashCode;
        private readonly string m_DownloadPath;
        private readonly string m_DownloadUri;
        private readonly int m_RetryCount;
        private readonly bool m_IsNeedUpdate;

        /// <summary>
        /// 初始化更新信息的新实例。
        /// </summary>
        /// <param name="resourceName">资源名称。</param>
        /// <param name="loadType">资源加载方式。</param>
        /// <param name="length">资源大小。</param>
        /// <param name="hashCode">资源哈希值。</param>
        /// <param name="zipLength">压缩包大小。</param>
        /// <param name="zipHashCode">压缩包哈希值。</param>
        /// <param name="downloadPath">资源更新下载后存放路径。</param>
        /// <param name="downloadUri">资源更新下载地址。</param>
        /// <param name="retryCount">已重试次数。</param>
        public CoursewareUpdate(CoursewareResourceName resourceName, int loadType, int length, int hashCode, int zipLength, int zipHashCode, string downloadPath, string downloadUri, int retryCount, bool isNeedUpdate)
        {
            m_ResourceName = resourceName;
            m_LoadType = loadType;
            m_Length = length;
            m_HashCode = hashCode;
            m_ZipLength = zipLength;
            m_ZipHashCode = zipHashCode;
            m_DownloadPath = downloadPath;
            m_DownloadUri = downloadUri;
            m_RetryCount = retryCount;
            m_IsNeedUpdate = isNeedUpdate;
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public CoursewareResourceName ResourceName
        {
            get
            {
                return m_ResourceName;
            }
        }

        /// <summary>
        /// 获取资源加载方式。
        /// </summary>
        public int LoadType
        {
            get
            {
                return m_LoadType;
            }
        }

        /// <summary>
        /// 获取资源大小。
        /// </summary>
        public int Length
        {
            get
            {
                return m_Length;
            }
        }

        /// <summary>
        /// 获取资源哈希值。
        /// </summary>
        public int HashCode
        {
            get
            {
                return m_HashCode;
            }
        }

        /// <summary>
        /// 获取压缩包大小。
        /// </summary>
        public int ZipLength
        {
            get
            {
                return m_ZipLength;
            }
        }

        /// <summary>
        /// 获取压缩包哈希值。
        /// </summary>
        public int ZipHashCode
        {
            get
            {
                return m_ZipHashCode;
            }
        }

        /// <summary>
        /// 获取下载后存放路径。
        /// </summary>
        public string DownloadPath
        {
            get
            {
                return m_DownloadPath;
            }
        }

        /// <summary>
        /// 获取下载地址。
        /// </summary>
        public string DownloadUri
        {
            get
            {
                return m_DownloadUri;
            }
        }

        /// <summary>
        /// 获取已重试次数。
        /// </summary>
        public int RetryCount
        {
            get
            {
                return m_RetryCount;
            }
        }

        /// <summary>
        /// 是否要更新
        /// </summary>
        public bool IsNeedUpdate
        {
            get
            {
                return m_IsNeedUpdate;
            }
        }
    }
}
