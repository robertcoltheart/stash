# Stash

[![NuGet](https://img.shields.io/nuget/v/Stash?style=for-the-badge)](https://www.nuget.org/packages/Stash) [![Build](https://img.shields.io/github/workflow/status/robertcoltheart/Stash/build?style=for-the-badge)](https://github.com/robertcoltheart/Stash/actions?query=workflow:build) [![License](https://img.shields.io/github/license/robertcoltheart/Stash?style=for-the-badge)](https://github.com/robertcoltheart/Stash/blob/master/LICENSE)

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
