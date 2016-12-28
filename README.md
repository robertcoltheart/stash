# Stash [![Travis](https://img.shields.io/travis/robertcoltheart/Stash.svg)](https://travis-ci.org/robertcoltheart/Stash) [![AppVeyor](https://img.shields.io/appveyor/ci/robertcoltheart/Stash.svg)](https://ci.appveyor.com/project/robertcoltheart/Stash) [![NuGet](https://img.shields.io/nuget/v/Stash.svg)](https://www.nuget.org/packages/Stash)
A basic embedded key-value database store.

## Usage
Install the package from NuGet with `nuget install Stash`.

```csharp
var database = new StashDatabase();

string key = "key";
string value = "value";

byte[] keyBytes = BitConverter.GetBytes(key);
byte[] valueBytes = BitConverter.GetBytes(value);

database.Set(keyBytes, valueBytes);
database.Get(keyBytes)
database.Remove(keyBytes);
```

## Contributing
Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on how to contribute to this project.

## License
Stash is released under the [MIT License](LICENSE)
