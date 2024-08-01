using System;

public static class Messages
{
    // Errors
    public static string FileNotFound(string name) => $"B��D:\n Nie znaleziono zapisu o nazwie '{name}'";
    public static string FileReadError(string error) => $"B��D:\n Nie uda�o si� wczyta� zapisu '{error}'";
    public static string FileSaveError(string name, string error) => $"B��D:\n Nie uda�o si� zapisa� '{name}': '{error}'";
    public static string LocationError(string error) => $"B��D:\n Nie uda�o si� pobra� lokalizacji:\n{error}";
    public static string WeatherError(string error) => $"B��D:\n Nie uda�o si� pobra� aktualnej pogody:\n{error}";


    // Confirms
    public static string ConfirmDelete(string name) => $"Czy na pewno chcesz usun�� konfiguracj� o nazwie '{name}'?";
    public static string ConfirmOversave(string name) => $"Konfiguracja o nazwie '{name}' ju� istnieje. Czy chcesz j� nadpisa�?";


    // Info
    public static string InfoProcessingLocation() => $"Trwa ustalanie lokalizacji ...";
    public static string InfoLocationSucces() => $"Pobrano lokalizacj�";
    public static string InfoProcessingWeatherData() => $"Trwa pobieranie danych pogodowych ...";
    public static string InfoWeatherDataSucces() => $"Pobrano dane pogodowe";

    public static string InfoCalculating() => $"Obliczanie ...";
    public static string InfoSaved(string name) => $"Zapisano konfiguracj� '{name}'";
    public static string InfoDeleted(string name) => $"Usuni�to konfiguracj� '{name}'";


}