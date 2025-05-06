namespace OnlineLibrary;

public static class StringExtensions
{
    public static string GetSubstringUntilCharacter(this string input, char stopChar)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));
        
        var index = input.IndexOf(stopChar);
        if (index == -1)
            return input;
        else
            return input.Substring(0, index);
    }
}