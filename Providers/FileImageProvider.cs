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

namespace Kaliko.ImageLibrary.WebResizer.Providers {
    using System.IO;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Helpers;

    public class FileImageProvider : ImageProviderBase {

        #region Private members

        private readonly bool _isInvalid;
        private readonly string _physicalPath;
        private readonly string _presetName;
        private readonly int _version;
        private string _eTag;

        #endregion

        #region Constructors

        public FileImageProvider(string presetName, string path, int version) : base(presetName, path, version) {
            _presetName = presetName;
            _version = version;
            _isInvalid = string.IsNullOrEmpty(path);

            if (_isInvalid) {
                return;
            }

            _physicalPath = HostingEnvironment.MapPath($"~/{path}");
            _isInvalid = !File.Exists(_physicalPath);
        }

        #endregion

        #region Public functions

        public override string GetETag() {
            if (_isInvalid) {
                return null;
            }

            return _eTag ?? (_eTag = CalculateETag());
        }

        public override KalikoImage GetImage() {
            if (_isInvalid) {
                return null;
            }

            return new KalikoImage(_physicalPath);
        }

        public override bool ImageExists => !_isInvalid;

        public override ActionResult ServeOriginal(string contentType) {
            return new FilePathResult(_physicalPath, contentType);
        }

        #endregion

        #region Private functions

        private string CalculateETag() {
            var fileInfo = new FileInfo(_physicalPath);

            var lastWriteTime = fileInfo.LastWriteTimeUtc.Ticks;
            var creationTime = fileInfo.CreationTimeUtc.Ticks;
            var size = fileInfo.Length;
            var cacheKey = $"{_presetName}_{_physicalPath}_{size}_{lastWriteTime}_{creationTime}_{_version}";

            return CryptographyHelper.CalculateSHA1(cacheKey);
        }

        #endregion
    }
}