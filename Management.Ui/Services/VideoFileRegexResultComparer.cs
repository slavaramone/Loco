using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Management.Ui.Services
{
    public class VideoFileRegexResultComparer : IEqualityComparer<VideoFileRegexResult>
    {
        public bool Equals([AllowNull] VideoFileRegexResult x, [AllowNull] VideoFileRegexResult y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.Cam == y.Cam && x.FileName == x.FileName && x.Nuc == y.Nuc && x.StartDateTime == y.StartDateTime)
                return true;
            else
                return false;
        }

        public int GetHashCode([DisallowNull] VideoFileRegexResult obj)
        {
            int hCode = int.Parse(obj.Cam) ^ int.Parse(obj.Nuc) ^ (int)obj.StartDateTime.Value.Millisecond;
            return hCode.GetHashCode();
        }
    }
}
