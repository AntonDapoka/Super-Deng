public interface IDataServiceScript
{
    bool SaveData<T>(string RelativePath, T Data, bool Encrypted);

    T LoadData<T>(string RelativePath, bool Encrypted);
}
