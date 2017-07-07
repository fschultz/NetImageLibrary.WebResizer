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
    using System;
    using System.Linq.Expressions;

    public static class ObjectCreation {
        public delegate T Creator<out T>(params object[] args);

        public static Creator<I> GetCreator<T, I>() {
            var constructors = typeof(T).GetConstructors();

            if (constructors.Length < 0) return null;

            var constructor = constructors[0];
            var paramsInfo = constructor.GetParameters();

            if (paramsInfo.Length <= 0) {
                throw new Exception($"Constructor missing in {typeof(T)}");
            }

            var param = Expression.Parameter(typeof(object[]), "args");
            var argsExpressions = new Expression[paramsInfo.Length];

            for (var i = 0; i < paramsInfo.Length; i++) {
                var index = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;
                var paramAccessorExp = Expression.ArrayIndex(param, index);
                var paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                argsExpressions[i] = paramCastExp;
            }

            var newExpression = Expression.New(constructor, argsExpressions);
            var lambda = Expression.Lambda(typeof(Creator<I>), newExpression, param);
            var compiled = (Creator<I>)lambda.Compile();

            return compiled;
        }
    }
}
