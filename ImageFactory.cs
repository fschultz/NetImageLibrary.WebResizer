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

namespace Kaliko.ImageLibrary.WebResizer {
    using System.Web.Mvc;
    using Configuration;
    using Providers;

    public class ImageFactory {
        #region Private members

        private readonly ImageProviderBase _imageProvider;
        private readonly ImagePreset _preset;

        #endregion

        #region Constructors

        public ImageFactory(string presetName, string path) {
            _preset = GetPreset(presetName);
            var version = _preset?.Version ?? 0;
            _imageProvider = GetImageProvider(presetName, path, version);
        }

        #endregion

        #region Public functions

        public string GetETag() {
            return _imageProvider.GetETag();
        }

        public ImageProviderBase GetImageProvider(string presetName, string path, int version) {
            return ImageCacheConfiguration.GetImageProvider(presetName, path, version);
        }

        public ActionResult GetImageResult() {
            return ImageCacheConfiguration.ImageStore.GetImageResult(_imageProvider, _preset);
        }
        
        public bool HasChanged(string requestETag) {
            if (string.IsNullOrEmpty(requestETag)) {
                return true;
            }

            return requestETag != GetETag();
        }

        public bool IsValid() {
            if (!_imageProvider.ImageExists) {
                return false;
            }

            if (_preset == null) {
                return false;
            }

            return true;
        }

        #endregion

        #region Private functions

        private static ImagePreset GetPreset(string presetName) {
            ImagePreset preset;
            ImageCacheConfiguration.Presets.TryGetValue(presetName, out preset);

            return preset;
        }

        #endregion
    }
}
