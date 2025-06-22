public class SphereFigureDataService
{
    private readonly IDataServiceScript dataService = new JsonDataServiceScript();
    private readonly bool isEncrypted = false;

    public void Save<T>(string relativePath, T data) where T : ISphereFigureData
    {
        dataService.SaveData(relativePath, data, isEncrypted);
    }

    public T Load<T>(string relativePath) where T : ISphereFigureData
    {
        return dataService.LoadData<T>(relativePath, isEncrypted);
    }
}
