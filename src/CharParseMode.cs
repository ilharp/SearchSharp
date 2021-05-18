using System;

namespace SearchSharp
{
    [Flags]
    public enum CharParseMode
    {
        Default = 0,
        EnableChineseCharSearch = 1 << 0,
        EnablePinyinSearch = (1 << 0) | (1 << 1)
    }
}
