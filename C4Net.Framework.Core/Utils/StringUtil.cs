using System.IO;
using System.Text;

namespace C4Net.Framework.Core.Utils
{
    /// <summary>
    /// Static class for helper methods for use with strings.
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// If the texts starts with trimText, then remove trimText from text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="trimText">The trim text.</param>
        /// <returns></returns>
        public static string TrimStringStart(string text, string trimText)
        {
            return text.StartsWith(trimText) ? text.Remove(0, trimText.Length) : text;
        }

        /// <summary>
        /// Camelizes one string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="fistCapital">if set to <c>true</c> [fist capital].</param>
        /// <returns></returns>
        public static string Camelize(string name, bool fistCapital = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }
            StringBuilder builder = new StringBuilder();
            name = name.ToLower();
            bool capitalize = fistCapital;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (char.IsLetter(c))
                {
                    builder.Append(capitalize ? char.ToUpper(c) : c);
                    capitalize = false;
                }
                else
                {
                    if (char.IsDigit(c))
                    {
                        builder.Append(c);
                    }
                    capitalize = true;
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Pascalizes one string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string Pascalize(string name)
        {
            return StringUtil.Camelize(name, true);
        }

        /// <summary>
        /// Pops the substring of a string between two strings.
        /// Example:   
        /// - str = abcd[[efg]]hij 
        /// - initStr = [[ 
        /// - endStr = ]] 
        /// => returns efg
        /// => str = abcdhij
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="initStr">The init string.</param>
        /// <param name="endStr">The end string.</param>
        /// <returns></returns>
        public static string PopBetween(ref string str, string initStr, string endStr)
        {
            int start = str.IndexOf(initStr);
            int end = str.IndexOf(endStr);
            if ((start != -1) && (end != -1))
            {
                string result = str.Substring(start + initStr.Length, end - start + initStr.Length);
                str = str.Remove(start, end + endStr.Length);
                return result;
            }
            return "";
        }

        /// <summary>
        /// Loads the file and return the content as string.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string LoadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            return string.Empty;
        }

        /// <summary>
        /// Writes content into one file, forcing the directory.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
        public static void WriteFile(string fileName, string content)
        {
            FileInfo info = new FileInfo(fileName);
            Directory.CreateDirectory(info.Directory.FullName);
            File.WriteAllText(fileName, content);
        }

    }
}
