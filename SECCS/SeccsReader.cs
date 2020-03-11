﻿using SECCS.Exceptions;
using System;

namespace SECCS
{
    public sealed class SeccsReader<TReader> : IBufferReader<TReader>
    {
        public FormatCollection<IReadFormat<TReader>> ReadFormats { get; } = new FormatCollection<IReadFormat<TReader>>();

        public object Deserialize(TReader reader, Type objType, ReadFormatContext<TReader>? context = null)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (objType == null)
                throw new ArgumentNullException(nameof(objType));

            var format = ReadFormats.GetFor(objType);
            if (format == null)
                throw new FormatNotFoundException(objType);

            context ??= new ReadFormatContext<TReader>(this, reader, ".");

            return format.Read(reader, objType, context.Value);
        }

        public T Deserialize<T>(TReader reader, ReadFormatContext<TReader>? context = null)
            => (T)Deserialize(reader, typeof(T), context);
    }
}