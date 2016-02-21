namespace Vault
{
    public interface IVaultDatabase
    {
        byte[] Get(byte[] key);
        void Remove(byte[] key);
        void Set(byte[] key, byte[] value);
    }
}