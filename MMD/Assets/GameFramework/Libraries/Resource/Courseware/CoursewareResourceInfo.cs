namespace GameFramework.Resource
{
    /// <summary>
    /// 资源信息。
    /// </summary>
    public struct CoursewareResourceInfo
    {
        private readonly CoursewareResourceName m_ResourceName;
        private readonly int m_LoadType;
        private readonly int m_Length;
        private readonly int m_HashCode;
        private readonly bool m_StorageInReadOnly;

        /// <summary>
        /// 初始化资源信息的新实例。
        /// </summary>
        /// <param name="resourceName">资源名称。</param>
        /// <param name="loadType">资源加载方式。</param>
        /// <param name="length">资源大小。</param>
        /// <param name="hashCode">资源哈希值。</param>
        /// <param name="storageInReadOnly">资源是否在只读区。</param>
        public CoursewareResourceInfo(CoursewareResourceName resourceName, int loadType, int length, int hashCode, bool storageInReadOnly)
        {
            m_ResourceName = resourceName;
            m_LoadType = loadType;
            m_Length = length;
            m_HashCode = hashCode;
            m_StorageInReadOnly = storageInReadOnly;
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
        /// 获取资源是否在只读区。
        /// </summary>
        public bool StorageInReadOnly
        {
            get
            {
                return m_StorageInReadOnly;
            }
        }
    }
}