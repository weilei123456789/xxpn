using System.Collections.Generic;

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源名称比较器。
    /// </summary>
    public sealed class CoursewareResourceNameComparer : IComparer<CoursewareResourceName>, IEqualityComparer<CoursewareResourceName>
    {
        public int Compare(CoursewareResourceName x, CoursewareResourceName y)
        {
            return x.CompareTo(y);
        }

        public bool Equals(CoursewareResourceName x, CoursewareResourceName y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(CoursewareResourceName obj)
        {
            return obj.GetHashCode();
        }
    }
}