/*
 * Ported by Ronnie Overby from SQL Server function found at:
 * http://www.planet-source-code.com/vb/scripts/ShowCode.asp?txtCodeId=522&lngWId=5
 */

using System;
using System.Linq;

public static class Metaphone
{
    /// <summary>
    /// Convenient extension method for strings.
    /// </summary>
    public static string MetaphoneEncode(this string input)
    {
        return Encode(input);
    }

    /// <remarks>
    /// I have no idea how this thing works.
    /// I ported it line by line from the TSQL function.
    /// </remarks>
    public static string Encode(string input)
    {
        string Result, str1, str2, strp;
        int strLen, cnt;

        strLen = input.Length;
        cnt = 1;
        Result = "";
        
        str2 = Left(input, 2);

        if (str2.In("ae", "gn", "kn", "pn", "wr"))
        {
            input = Right(input, strLen - 1);
            strLen = strLen - 1;
        }

        if (str2.Eq("wh"))
        {
            input = 'w' + Right(input, strLen - 2);
            strLen = strLen - 1;
        }

        str1 = Left(input, 1);

        if (str1.Eq("x"))
            input = 's' + Right(input, strLen - 1);

        if (str1.In("a", "e", "i", "o", "u"))
        {
            input = Right(input, strLen - 1);
            strLen = strLen - 1;
            Result = str1;
        }

        while (cnt <= strLen)
        {
            str1 = Substring(input, cnt, 1);

            if (cnt != 1)
                strp = Substring(input, (cnt - 1), 1);
            else
                strp = " ";

            if (strp.Neq(str1))
            {
                str2 = Substring(input, cnt, 2);

                if (str1.In("f", "j", "l", "m", "n", "r"))
                    Result = Result + str1;

                if (str1.Eq("q"))
                    Result = Result + 'k';

                if (str1.Eq("v"))
                    Result = Result + 'f';

                if (str1.Eq("x"))
                    Result = Result + "ks";

                if (str1.Eq("z"))
                    Result = Result + 's';

                if (str1.Eq("b"))
                    if (cnt == strLen)
                        if (Substring(input, (cnt - 1), 1).Neq("m"))
                            Result = Result + 'b';
                        else
                            Result = Result + 'b';

                if (str1.Eq("c"))
                    if (str2.Eq("ch") | Substring(input, cnt, 3).Eq("cia"))
                        Result = Result + 'x';
                    else if (str2.In("ci", "ce", "cy") & strp.Neq("s"))
                        Result = Result + 's';
                    else
                        Result = Result + 'k';

                if (str1.Eq("d"))
                    if (Substring(input, cnt, 3).In("dge", "dgy", "dgi"))
                        Result = Result + 'j';
                    else
                        Result = Result + 't';


                if (str1.Eq("g"))
                    if (Substring(input, (cnt - 1), 3).NotIn("dge", "dgy", "dgi", "dha", "dhe", "dhi", "dho", "dhu"))
                        if (str2.In("gi", "ge", "gy"))
                            Result = Result + 'j';
                        else if ((str2.Neq("gn")) | ((str2.Neq("gh")) & ((cnt + 1) != strLen)))
                            Result = Result + 'k';

                if (str1.Eq("h"))
                    if ((strp.NotIn("a", "e", "i", "o", "u")) & (str2.NotIn("ha", "he", "hi", "ho", "hu")))
                        if (strp.NotIn("c", "s", "p", "t", "g"))
                            Result = Result + 'h';

                if (str1.Eq("k"))
                    if (strp.Neq("c"))
                        Result = Result + 'k';

                if (str1.Eq("p"))
                    if (str2.Eq("ph"))
                        Result = Result + "f";
                    else
                        Result = Result + "p";

                if (str1.Eq("s"))
                    if (Substring(input, cnt, 3).In("sia", "sio") | str2.Eq("sh"))
                        Result = Result + "x";
                    else
                        Result = Result + "s";

                if (str1.Eq("t"))
                    if (Substring(input, cnt, 3).In("tia", "tio"))
                        Result = Result + 'x';
                    else if (str2.Eq("th"))
                        Result = Result + '0';
                    else if (Substring(input, cnt, 3).Neq("tch"))
                        Result = Result + 't';

                if (str1.Eq("w"))
                    if (str2.NotIn("wa", "we", "wi", "wo", "wu"))
                        Result = Result + "w";

                if (str1.Eq("y"))
                    if (str2.NotIn("ya", "ye", "yi", "yo", "yu"))
                        Result = Result + "y";
            }

            cnt = cnt + 1;
        }

        return Result;
    }

    /*
     * The methods below were created to ease the porting of the original TSQL function. 
     */

    /// <summary>
    /// Case insensitive string comparison (not equal to)
    /// </summary>
    private static bool Neq(this string a, string b)
    {
        return !a.Eq(b);
    }

    /// <summary>
    /// Case insensitive string comparison (equal to)
    /// </summary>
    private static bool Eq(this string a, string b)
    {
        return string.Equals(a, b, StringComparison.CurrentCultureIgnoreCase);
    }

    /// <summary>
    /// Behaves like TSQL substring function.
    /// Much more forgiving than String.Substring
    /// </summary>
    private static string Substring(string input, int startingAt, int getThisMany)
    {
        return new string(input.Skip(startingAt - 1).Take(getThisMany).ToArray());
    }

    /// <summary>
    /// Mimicks TSQL IN
    /// </summary>
    private static bool In(this string input, params string[] items)
    {
        return items.Contains(input, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Mimicks TSQL NOT IN
    /// </summary>
    private static bool NotIn(this string input, params string[] items)
    {
        return !In(input, items);
    }

    /// <summary>
    /// Mimicks TSQL Left function
    /// </summary>
    private static string Left(this string input, int chars)
    {
        return Substring(input, 0, chars);
    }

    /// <summary>
    /// Mimicks TSQL Right function
    /// </summary>
    private static string Right(this string input, int chars)
    {
        return input.Reverse().Left(chars).Reverse();
    }

    /// <summary>
    /// Reverses a string.
    /// </summary>
    private static string Reverse(this string s)
    {
        var charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}