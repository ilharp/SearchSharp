using System;

namespace SearchSharp
{
    /// <summary>
    /// The mode to process char.
    /// </summary>
    [Flags]
    public enum CharParseMode
    {
        /// <summary>
        /// Default.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Enable Chinese char search.
        /// </summary>
        EnableChineseCharSearch = 1 << 0,

        /// <summary>
        /// Enable Pinyin search.
        /// </summary>
        EnablePinyinSearch = (1 << 0) | (1 << 1)
    }
}
