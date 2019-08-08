using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace rsoni.Common
{
    public static class ExtendedMethods
    {

        /// <summary>
        /// This method returns the index of the nth occurence of the value passed in the specified string
        /// </summary>
        /// <param name="target">The String in which search has to be done</param>
        /// <param name="value">The value of which occurence's index to be found</param>
        /// <param name="n">the occurence number for which the index will be returned</param>
        /// <returns></returns>
        public static int NthIndexOf(this string target, string value, int n)
        {
            Match m = Regex.Match(target, "((" + value + ").*?){" + n + "}");

            if (m.Success)
                return m.Groups[2].Captures[n - 1].Index;
            else
                return -1;
        }

        public static string GetFileName(this string fileNamewithPath)
        {
            string returnString = string.Empty;
            int startindex = fileNamewithPath.LastIndexOf("\\");
            returnString = fileNamewithPath.Substring(startindex + 1);
            return returnString;
        }


        public static string FullException(this Exception ex)
        {
            string exceptionMsgandTrace = "";
            int counter = 0;
            Exception tempEx = ex;
            string innnerExceptionErrorMsg = tempEx.Message;
            while (innnerExceptionErrorMsg != string.Empty)
            {
                exceptionMsgandTrace = exceptionMsgandTrace + Environment.NewLine + "-------------------" + Environment.NewLine;
                exceptionMsgandTrace = exceptionMsgandTrace + " , Message : " + tempEx.Message + " StackTrace: " + tempEx.StackTrace;
                exceptionMsgandTrace = exceptionMsgandTrace + Environment.NewLine + "-------------------" + Environment.NewLine;
                tempEx = tempEx.InnerException;
                if (tempEx != null)
                    innnerExceptionErrorMsg = tempEx.Message;
                else
                    innnerExceptionErrorMsg = string.Empty;
                counter++;
                if (counter > 20) break;
            }

            return exceptionMsgandTrace;
        }

    }
}
