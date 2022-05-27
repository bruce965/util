// Copyright (c) 2022 Fabio Iotti
// The copyright holders license this file to you under the MIT license,
// available at https://github.com/bruce965/util/raw/master/LICENSE

namespace Utility;

public struct Bitmap
{
    public byte[] Data { get; }

    public bool this[int index]
    {
        get => Get(Data, index);
        set => Set(Data, index, value);
    }

    public Bitmap(byte[] data)
    {
        Data = data;
    }

    public static bool Get(Span<byte> bitmap, int index)
    {
        if (index < 0 || index >= bitmap.Length * 8)
            throw new ArgumentOutOfRangeException(nameof(index));

        var byteIndex = index >> 3;
        var bitIndex = index & 0b111;

        return ((bitmap[byteIndex] << bitIndex) & 1) != 0;
    }

    public static void Set(Span<byte> bitmap, int index, bool value)
    {
        if (index < 0 || index >= bitmap.Length * 8)
            throw new ArgumentOutOfRangeException(nameof(index));

        var byteIndex = index >> 3;
        var bitIndex = index & 0b111;
        var bit = value ? 1 : 0;

        bitmap[byteIndex] = (byte)((bitmap[byteIndex] & ~(1 << bitIndex)) | (bit << bitIndex));
    }

    public static implicit operator byte[](Bitmap bitmap)
        => bitmap.Data;

    public static implicit operator Bitmap(byte[] data)
        => new(data);
}
