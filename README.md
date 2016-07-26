# Vault [![Travis](https://img.shields.io/travis/robertcoltheart/Vault.svg)](https://travis-ci.org/robertcoltheart/Vault) [![AppVeyor](https://img.shields.io/appveyor/ci/robertcoltheart/Vault.svg)](https://ci.appveyor.com/project/robertcoltheart/Vault) [![NuGet](https://img.shields.io/nuget/v/Vault.svg)](https://www.nuget.org/packages/Vault)
A basic embedded key-value database store.

## Usage
Install the package from NuGet with `nuget install Vault`.

```csharp
var database = new VaultDatabase();

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
Vault is released under the [MIT License](LICENSE)
