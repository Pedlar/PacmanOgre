﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanOgre.Utilities
{
    static class StandardExtensions
    {
        /*
         * Calls the specified function [block] and returns its result.
         */
        public static R Run<R>(this R self, Func<R, R> block) => block(self); 

        public static R Let<T, R>(this T self, Func<T, R> block) => block(self);

        public static T Also<T>(this T self, Action<T> block)
        {
            block(self);
            return self;
        }
    }
}
