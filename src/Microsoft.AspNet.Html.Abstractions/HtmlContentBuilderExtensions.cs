// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Extensions.WebEncoders;

namespace Microsoft.AspNet.Html.Abstractions
{
    /// <summary>
    /// Extension methods for <see cref="IHtmlContentBuilder"/>.
    /// </summary>
    public static class HtmlContentBuilderExtensions
    {
        /// <summary>
        /// Appends the specified <paramref name="format"/> to the existing content after replacing each format
        /// item with the HTML encoded <see cref="string"/> representation of the corresponding item in the
        /// <paramref name="args"/> array.
        /// </summary>
        /// <param name="format">
        /// The composite format <see cref="string"/> (see http://msdn.microsoft.com/en-us/library/txafckwd.aspx).
        /// The format string is assumed to be HTML encoded as-provided, and no further encoding will be performed.
        /// </param>
        /// <param name="args">
        /// The object array to format. Each element in the array will be formatted and then HTML encoded.
        /// </param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static IHtmlContentBuilder AppendFormat(
            this IHtmlContentBuilder builder,
            string format,
            params object[] args)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            builder.Append(new HtmlFormatString(format, args));
            return builder;
        }

        /// <summary>
        /// Appends the specified <paramref name="format"/> to the existing content with information from the
        /// <paramref name="provider"/> after replacing each format item with the HTML encoded <see cref="string"/>
        /// representation of the corresponding item in the <paramref name="args"/> array.
        /// </summary>
        /// <param name="formatProvider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">
        /// The composite format <see cref="string"/> (see http://msdn.microsoft.com/en-us/library/txafckwd.aspx).
        /// The format string is assumed to be HTML encoded as-provided, and no further encoding will be performed.
        /// </param>
        /// <param name="args">
        /// The object array to format. Each element in the array will be formatted and then HTML encoded.
        /// </param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        public static IHtmlContentBuilder AppendFormat(
            this IHtmlContentBuilder builder,
            IFormatProvider formatProvider,
            string format,
            params object[] args)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (format == null)
            {
                throw new ArgumentNullException(nameof(format));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            builder.Append(new HtmlFormatString(formatProvider, format, args));
            return builder;
        }

        /// <summary>
        /// Appends an <see cref="Environment.NewLine"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IHtmlContentBuilder"/>.</param>
        /// <returns>The <see cref="IHtmlContentBuilder"/>.</returns>
        public static IHtmlContentBuilder AppendLine(this IHtmlContentBuilder builder)
        {
            builder.Append(HtmlEncodedString.NewLine);
            return builder;
        }

        /// <summary>
        /// Appends an <see cref="Environment.NewLine"/> after appending the <see cref="string"/> value.
        /// The value is treated as unencoded as-provided, and will be HTML encoded before writing to output.
        /// </summary>
        /// <param name="builder">The <see cref="IHtmlContentBuilder"/>.</param>
        /// <param name="unencoded">The <see cref="string"/> to append.</param>
        /// <returns>The <see cref="IHtmlContentBuilder"/>.</returns>
        public static IHtmlContentBuilder AppendLine(this IHtmlContentBuilder builder, string unencoded)
        {
            builder.Append(unencoded);
            builder.Append(HtmlEncodedString.NewLine);
            return builder;
        }

        /// <summary>
        /// Appends an <see cref="Environment.NewLine"/> after appending the <see cref="IHtmlContent"/> value.
        /// </summary>
        /// <param name="builder">The <see cref="IHtmlContentBuilder"/>.</param>
        /// <param name="content">The <see cref="IHtmlContent"/> to append.</param>
        /// <returns>The <see cref="IHtmlContentBuilder"/>.</returns>
        public static IHtmlContentBuilder AppendLine(this IHtmlContentBuilder builder, IHtmlContent htmlContent)
        {
            builder.Append(htmlContent);
            builder.Append(HtmlEncodedString.NewLine);
            return builder;
        }

        /// <summary>
        /// Appends an <see cref="Environment.NewLine"/> after appending the <see cref="string"/> value.
        /// The value is treated as HTML encoded as-provided, and no further encoding will be performed.
        /// </summary>
        /// <param name="builder">The <see cref="IHtmlContentBuilder"/>.</param>
        /// <param name="content">The HTML encoded <see cref="string"/> to append.</param>
        /// <returns>The <see cref="IHtmlContentBuilder"/>.</returns>
        public static IHtmlContentBuilder AppendLineEncoded(this IHtmlContentBuilder builder, string encoded)
        {
            builder.AppendEncoded(encoded);
            builder.Append(HtmlEncodedString.NewLine);
            return builder;
        }

        /// <summary>
        /// Sets the content to the <see cref="string"/> value. The value is treated as unencoded as-provided,
        /// and will be HTML encoded before writing to output.
        /// </summary>
        /// <param name="builder">The <see cref="IHtmlContentBuilder"/>.</param>
        /// <param name="value">The <see cref="string"/> value that replaces the content.</param>
        /// <returns>The <see cref="IHtmlContentBuilder"/>.</returns>
        public static IHtmlContentBuilder SetContent(this IHtmlContentBuilder builder, string unencoded)
        {
            builder.Clear();
            builder.Append(unencoded);
            return builder;
        }

        /// <summary>
        /// Sets the content to the <see cref="IHtmlContent"/> value.
        /// </summary>
        /// <param name="builder">The <see cref="IHtmlContentBuilder"/>.</param>
        /// <param name="value">The <see cref="IHtmlContent"/> value that replaces the content.</param>
        /// <returns>The <see cref="IHtmlContentBuilder"/>.</returns>
        public static IHtmlContentBuilder SetContent(this IHtmlContentBuilder builder, IHtmlContent content)
        {
            builder.Clear();
            builder.Append(content);
            return builder;
        }

        /// <summary>
        /// Sets the content to the <see cref="string"/> value. The value is treated as HTML encoded as-provided, and
        /// no further encoding will be performed.
        /// </summary>
        /// <param name="builder">The <see cref="IHtmlContentBuilder"/>.</param>
        /// <param name="content">The HTML encoded <see cref="string"/> that replaces the content.</param>
        /// <returns>The <see cref="IHtmlContentBuilder"/>.</returns>
        public static IHtmlContentBuilder SetContentEncoded(this IHtmlContentBuilder builder, string encoded)
        {
            builder.Clear();
            builder.AppendEncoded(encoded);
            return builder;
        }

        [DebuggerDisplay("{DebuggerToString()}")]
        private class HtmlEncodedString : IHtmlContent
        {
            public static readonly IHtmlContent NewLine = new HtmlEncodedString(Environment.NewLine);

            private readonly string _value;

            public HtmlEncodedString(string value)
            {
                _value = value;
            }

            public void WriteTo(TextWriter writer, IHtmlEncoder encoder)
            {
                writer.Write(_value);
            }

            private string DebuggerToString()
            {
                using (var writer = new StringWriter())
                {
                    WriteTo(writer, HtmlEncoder.Default);
                    return writer.ToString();
                }
            }
        }

        [DebuggerDisplay("{DebuggerToString()}")]
        private class HtmlFormatString : IHtmlContent
        {
            private readonly IFormatProvider _formatProvider;
            private readonly string _format;
            private readonly object[] _args;

            public HtmlFormatString(string format, object[] args)
                : this(null, format, args)
            {
            }

            public HtmlFormatString(IFormatProvider formatProvider, string format, object[] args)
            {
                Debug.Assert(format != null);
                Debug.Assert(args != null);

                _formatProvider = formatProvider ?? CultureInfo.CurrentCulture;
                _format = format;
                _args = args;
            }

            public void WriteTo(TextWriter writer, IHtmlEncoder encoder)
            {
                if (writer == null)
                {
                    throw new ArgumentNullException(nameof(writer));
                }

                if (encoder == null)
                {
                    throw new ArgumentNullException(nameof(encoder));
                }

                var formatProvider = new EncodingFormatProvider(_formatProvider, encoder);
                writer.Write(string.Format(formatProvider, _format, _args));
            }

            private string DebuggerToString()
            {
                using (var writer = new StringWriter())
                {
                    WriteTo(writer, HtmlEncoder.Default);
                    return writer.ToString();
                }
            }
        }

        // This class implements Html encoding via an ICustomFormatter. Passing an instance of this
        // class into a string.Format method or anything similar will evaluate arguments implementing
        // IHtmlContent without HTML encoding them, and will give other arguments the standard
        // composite format string treatment, and then HTML encode the result.
        //
        // Plenty of examples of ICustomFormatter and the interactions with string.Format here:
        // https://msdn.microsoft.com/en-us/library/system.string.format(v=vs.110).aspx#Format6_Example
        private class EncodingFormatProvider : IFormatProvider, ICustomFormatter
        {
            private readonly IHtmlEncoder _encoder;
            private readonly IFormatProvider _formatProvider;

            public EncodingFormatProvider(IFormatProvider formatProvider, IHtmlEncoder encoder)
            {
                Debug.Assert(formatProvider != null);
                Debug.Assert(encoder != null);

                _formatProvider = formatProvider;
                _encoder = encoder;
            }

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                // This is the case we need to special case. We trust the IHtmlContent instance to do the
                // right thing with encoding.
                var htmlContent = arg as IHtmlContent;
                if (htmlContent != null)
                {
                    using (var writer = new StringWriter())
                    {
                        htmlContent.WriteTo(writer, _encoder);
                        return writer.ToString();
                    }
                }

                // If we get here then 'arg' is not an IHtmlContent, and we want to handle it the way a normal
                // string.Format would work, but then HTML encode the result.
                //
                // First check for an ICustomFormatter - if the IFormatProvider is a CultureInfo, then it's likely
                // that ICustomFormatter will be null.
                var customFormatter = (ICustomFormatter)_formatProvider.GetFormat(typeof(ICustomFormatter));
                if (customFormatter != null)
                {
                    var result = customFormatter.Format(format, arg, _formatProvider);
                    if (result != null)
                    {
                        return _encoder.HtmlEncode(result);
                    }
                }

                // Next check if 'arg' is an IFormattable (DateTime is an example).
                //
                // An IFormattable will likely call back into the IFormatterProvider and ask for more information
                // about how to format itself. This is the typical case when IFormatterProvider is a CultureInfo.
                var formattable = arg as IFormattable;
                if (formattable != null)
                {
                    var result = formattable.ToString(format, _formatProvider);
                    if (result != null)
                    {
                        return _encoder.HtmlEncode(result);
                    }
                }

                // If we get here then there's nothing really smart left to try.
                if (arg != null)
                {
                    var result = arg.ToString();
                    if (result != null)
                    {
                        return _encoder.HtmlEncode(result);
                    }
                }

                return string.Empty;
            }

            public object GetFormat(Type formatType)
            {
                if (formatType == typeof(ICustomFormatter))
                {
                    return this;
                }

                return null;
            }
        }
    }
}
