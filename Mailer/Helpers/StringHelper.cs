﻿using System;
using System.Linq;

namespace Mailer.Helpers
{
    public static class StringHelper
    {
        public static string FormatSize(double bytes, string[] orders = null)
        {
            const int scale = 1024;

            if (orders == null)
                orders = new[] {"GB", "MB", "KB", "Bytes"};

            var max = (long) Math.Pow(scale, orders.Length - 1);

            foreach (var order in orders)
            {
                if (bytes > max)
                    return $"{decimal.Divide((decimal) bytes, max):##.##} {order}";

                max /= scale;
            }

            return "0 Bytes";
        }

        public static string ToUtf8(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return new string(input.ToCharArray().Select(x => x + 848 >= 'А' && x + 848 <= 'ё' ? (char) (x + 848) : x)
                .ToArray());
        }
    }
}