namespace BudgetApp.Application.Common.Services;

public static class CsvService
{
    /*public static async Task<string> ConvertFileToBase64(IBrowserFile file)
    {
        using (var ms = new MemoryStream())
        {
            await file.OpenReadStream().CopyToAsync(ms);
            var bytes = ms.ToArray();
            return Convert.ToBase64String(bytes);
        }
    }
    
    public static async Task<List<ExpandoObject>> GetRecordsFromCsvFile(IBrowserFile file)
    {
        var results = new List<ExpandoObject>();
        
        await using var streamData = file.OpenReadStream();
        using var reader = new StreamReader(streamData);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        await csv.ReadAsync();
        csv.ReadHeader();
        while (await csv.ReadAsync())
        {
            var record = csv.GetRecord<dynamic>();
            if (record is not null)
            {
                results.Add(record);
            }
        }

        return results;
    }*/
}