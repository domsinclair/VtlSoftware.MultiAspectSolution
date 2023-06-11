#if !NET5_0_OR_GREATER

namespace System
{
    public partial class String
    {
        //public string Replace(string oldValue, string? newValue, StringComparison comparisonType)
        //{
        //    switch (comparisonType)
        //    {
        //        case StringComparison.CurrentCulture:
        //        case StringComparison.CurrentCultureIgnoreCase:
        //            return ReplaceCore(oldValue, newValue, CultureInfo.CurrentCulture.CompareInfo, GetCaseCompareOfComparisonCulture(comparisonType));

        //        case StringComparison.InvariantCulture:
        //        case StringComparison.InvariantCultureIgnoreCase:
        //            return ReplaceCore(oldValue, newValue, CompareInfo.Invariant, GetCaseCompareOfComparisonCulture(comparisonType));

        //        case StringComparison.Ordinal:
        //            return Replace(oldValue, newValue);

        //        case StringComparison.OrdinalIgnoreCase:
        //            return ReplaceCore(oldValue, newValue, CompareInfo.Invariant, CompareOptions.OrdinalIgnoreCase);

        //        default:
        //            throw new ArgumentException(SR.NotSupported_StringComparison, nameof(comparisonType));
        //    }
        //}

        //private string ReplaceCore(string oldValue, string? newValue, CompareInfo? ci, CompareOptions options)
        //{
        //    if (oldValue is null)
        //    {
        //        throw new ArgumentNullException(nameof(oldValue));
        //    }

        //    if (oldValue.Length == 0)
        //    {
        //        throw new ArgumentException(SR.Argument_StringZeroLength, nameof(oldValue));
        //    }

        //    // If they asked to replace oldValue with a null, replace all occurrences
        //    // with the empty string. AsSpan() will normalize appropriately.
        //    //
        //    // If inner ReplaceCore method returns null, it means no substitutions were
        //    // performed, so as an optimization we'll return the original string.

        //    return ReplaceCore(this, oldValue.AsSpan(), newValue.AsSpan(), ci ?? CultureInfo.CurrentCulture.CompareInfo, options)
        //        ?? this;
        //}

        //private static string? ReplaceCore(ReadOnlySpan<char> searchSpace, ReadOnlySpan<char> oldValue, ReadOnlySpan<char> newValue, CompareInfo compareInfo, CompareOptions options)
        //{
        //    Debug.Assert(!oldValue.IsEmpty);
        //    Debug.Assert(compareInfo != null);

        //    var result = new ValueStringBuilder(stackalloc char[256]);
        //    result.EnsureCapacity(searchSpace.Length);

        //    bool hasDoneAnyReplacements = false;

        //    while (true)
        //    {
        //        int index = compareInfo.IndexOf(searchSpace, oldValue, options, out int matchLength);

        //        // There's the possibility that 'oldValue' has zero collation weight (empty string equivalent).
        //        // If this is the case, we behave as if there are no more substitutions to be made.

        //        if (index < 0 || matchLength == 0)
        //        {
        //            break;
        //        }

        //        // append the unmodified portion of search space
        //        result.Append(searchSpace.Slice(0, index));

        //        // append the replacement
        //        result.Append(newValue);

        //        searchSpace = searchSpace.Slice(index + matchLength);
        //        hasDoneAnyReplacements = true;
        //    }

        //    // Didn't find 'oldValue' in the remaining search space, or the match
        //    // consisted only of zero collation weight characters. As an optimization,
        //    // if we have not yet performed any replacements, we'll save the
        //    // allocation.

        //    if (!hasDoneAnyReplacements)
        //    {
        //        result.Dispose();
        //        return null;
        //    }

        //    // Append what remains of the search space, then allocate the new string.

        //    result.Append(searchSpace);
        //    return result.ToString();
        //}
    }
}

#endif