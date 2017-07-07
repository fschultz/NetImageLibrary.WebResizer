#region License and copyright notice
/* 
 * Kaliko Image Library Web Cache
 * 
 * Copyright (c) Fredrik Schultz / Kaliko
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 * http://www.gnu.org/licenses/lgpl-3.0.html
 */
#endregion

namespace Kaliko.ImageLibrary.WebResizer.Helpers {
    using System.Security.Cryptography;
    using System.Text;

    public class CryptographyHelper {

        #region Public functions

        // ReSharper disable once InconsistentNaming
        public static string CalculateSHA1(string text) {
            using (var sha1 = new SHA1Managed()) {
                return CalculateHash(sha1, text);
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string CalculateMD5(string text) {
            using (var md5 = new MD5CryptoServiceProvider()) {
                return CalculateHash(md5, text);
            }
        }
        
        public static string CalculateHash(HashAlgorithm hashAlgorithm, string text) {
            var hash = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
            var stringBuilder = new StringBuilder(hash.Length * 2);

            foreach (var b in hash) {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        #endregion

    }
}
