﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator
{
    public static class ControllerBindGenerator
    {
        private static string GetBind(TypeElement type)
        {
            return $"\t\t\tbuilder.Register<{type.TypeName}>(Lifetime.Singleton).AsImplementedInterfaces();\t// {type.Order:0000}";
        }
        
        public static IEnumerable<string> GetBinds(IEnumerable<TypeElement> types)
        {
            types = types.OrderBy(t => t.Order).ThenBy(t => t.TypeName);

            var previous = 100000;
            foreach (var t in types)
            {
                if (Math.Abs(previous - t.Order) > 10)
                    yield return $"\n			// {t.Order:0000}";
                yield return GetBind(t);
                previous = t.Order;
            }
        }
    }
}