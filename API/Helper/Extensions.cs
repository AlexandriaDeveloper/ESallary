using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace API.Helper {
    public static class Extensions {
        public static void AddApplicationError (this HttpResponse response, string message) {
            response.Headers.Add ("Application-Error", message);
            response.Headers.Add ("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add ("Access-Control-Allow-Origin", "*");
        }
        public static void AddPageination (this HttpResponse response, int currentPage, int itemPerPage, int totalItems, int totalPages) {
            var paginationHeader = new PaginationHeader (currentPage, itemPerPage, totalItems, totalPages);
            response.Headers.Add ("Pagination", JsonConvert.SerializeObject (paginationHeader));
            response.Headers.Add ("Access-Control-Expose-Headers", "Pagination");
        }
        public static string CheckGenderByNationalId (this string NationalId) {

            int extractedNum = int.Parse (NationalId.Substring (12, 1));
            if (extractedNum % 2 == 0) {
                return "Female";
            } else {
                return "Male";
            }
        }
        public static bool ValidateNtionalId (this string nationalId) {
            bool valid = false;
            if (nationalId.Length != 14) {
                valid = false;
                return valid;
            }
            int centery = int.Parse (nationalId.Substring (0, 1));
            if (centery != 1 && centery != 2 && centery != 3) {
                valid = false;
                return valid;
            }

            bool pares = nationalId.All (char.IsDigit);
            if (!pares) {
                valid = false;
                return valid;
            }

            int month = int.Parse (nationalId.Substring (3, 2));
            if (month > 12) {
                valid = false;
                return valid;
            }
            int day = int.Parse (nationalId.Substring (5, 2));
            int millineum = int.Parse (nationalId.Substring (0, 1));

            int year = int.Parse (nationalId.Substring (1, 2));

            switch (millineum) {
                case 1:
                    year += 1800;
                    break;
                case 2:
                    year += 1900;
                    break;
                case 3:
                    year += 2000;
                    break;

            }
            int daysInMonth = DateTime.DaysInMonth (year, month);
            if (day > daysInMonth) {
                valid = false;
                return valid;
            }
            return true;
        }
        public static Nullable<DateTime> GetDOBFromNationalId (this string NationalId) {

            int millineum = int.Parse (NationalId.Substring (0, 1));

            int year = int.Parse (NationalId.Substring (1, 2));

            switch (millineum) {
                case 1:
                    year += 1800;
                    break;
                case 2:
                    year += 1900;
                    break;
                case 3:
                    year += 2000;
                    break;

            }

            int month = int.Parse (NationalId.Substring (3, 2));
            int day = int.Parse (NationalId.Substring (5, 2));
            try {
                return new DateTime (year, month, day);
            } catch (Exception ex) {
                throw new Exception (ex.InnerException.ToString ());
            }

        }
        public static DataTable ReplaceCoulmnName (this DataTable Table, string[] Predicted, string Replacment) {
            DataColumnCollection data2 = Table.Columns;
            string nameItem = "";
            var valueExist = Predicted.FirstOrDefault (name => {
                if (data2.Contains (name)) {
                    nameItem = name;
                    return true;
                }
                return false;

            });
            if (nameItem != "") {

                var colIndex = data2.IndexOf (nameItem);
                Table.Columns[colIndex].ColumnName = Replacment;
                return Table;
            }
            throw new Exception (Replacment);
            return null;
        }

        public static string ToNormalizedString (this String input) {
            if (!string.IsNullOrEmpty (input)) {
                //normalize the incoming string considering it as a unicode
                //NormalizationForm.FormD Indicates that a Unicode string is normalized using full canonical decomposition.
                //https://docs.microsoft.com/en-us/dotnet/api/system.text.normalizationform?view=netframework-4.8                
                string text = input.Normalize (NormalizationForm.FormD);

                //Make sure that the unicode category is not NonSpacingMark
                var chars = text.Where (c => CharUnicodeInfo.GetUnicodeCategory (c) != UnicodeCategory.NonSpacingMark).ToArray ();

                //Normalize using fully compatibility decomposition.
                string str = new string (chars).Normalize (NormalizationForm.FormKD);

                //The normalization can't convert two characters only so you'd have to do them manually
                // if (str.StartsWith ("أ")) {
                //     str = str.Replace ("أ", "ا", true, CultureInfo.CurrentCulture);
                // }
                // if (str.StartsWith ("إ")) {
                //     str = str.Replace ("أ", "ا", true, CultureInfo.CurrentCulture);
                // }
                str = str.Trim ();
                string[] strArr = str.Split (" ");

                str = string.Empty;
                foreach (string strItem in strArr) {
                    string modifiedStr = "";
                    if (strItem.EndsWith ("ة")) {

                        modifiedStr = strItem.Remove (strItem.Length - 1, 1) + "ه "; // ("ة", "ه");
                    }

                    if (strItem.EndsWith ("ي")) {
                        modifiedStr = strItem.Remove (strItem.Length - 1, 1) + "ى ";
                        // modifiedStr = strItem.Replace ("ي", "ى");
                    }
                    if (modifiedStr != "") {
                        str += modifiedStr + " ";
                    } else {
                        str += strItem + " ";
                    }

                }

                return str.Trim ();

            }

            return string.Empty;
        }
    }
}