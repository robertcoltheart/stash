# Vault
A basic embedded key-value database store.

## Usage
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
