namespace AaryanBillingApp.Models.Entities;

public class Setting
{
    public int Id { get; set; }

    public string SettingKey { get; set; } = string.Empty;

    public string? SettingValue { get; set; }

    public string? Description { get; set; }
}