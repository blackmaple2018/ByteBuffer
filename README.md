
## ByteBuffer ##

An **optimized** binary data processing library with generic read/write support  :smiley:

The goal is to have the least amount of IL instructions possible for every method  :sunglasses:

Please let me know if you have any feature requests or suggestions!  

## Examples ##

#### Unsafe Read/Write

```csharp
var w = new UnsafeByteWriter();
w.Write<byte>(0xFF);
w.Write<short>(0x1337);
w.Write<int>(0xBADF00D);

var buffer = w.GetBuffer();

var r = new UnsafeByteReader(buffer, 0, buffer.Length);
var v1 = r.Read<byte>();
var v2 = r.Read<short>();
var v3 = r.Read<int>();
```

## References ##
 - [Memory.Unsafe](https://github.com/DotNetCross/Memory.Unsafe/)

[![Built with SharpDevelop](https://raw.githubusercontent.com/icsharpcode/SharpDevelop/master/doc/BuiltWithSharpDevelop.png)](https://github.com/icsharpcode/SharpDevelop)