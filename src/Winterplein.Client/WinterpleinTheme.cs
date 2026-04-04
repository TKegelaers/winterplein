using MudBlazor;

namespace Winterplein.Client;

public static class WinterpleinTheme
{
    public static MudTheme Theme { get; } = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = "#2E7D32",
            PrimaryDarken = "#1B5E20",
            PrimaryLighten = "#4CAF50",
            Secondary = "#FF8F00",
            SecondaryDarken = "#E65100",
            SecondaryLighten = "#FFB300",
            AppbarBackground = "#2E7D32",
            AppbarText = Colors.Shades.White,
            DrawerBackground = "#F1F8E9",
            DrawerText = "#33691E",
            Surface = Colors.Shades.White,
            Background = "#FAFAFA",
            TextPrimary = "#212121",
            TextSecondary = "#757575",
        },
        PaletteDark = new PaletteDark
        {
            Primary = "#66BB6A",
            Secondary = "#FFB74D",
        },
        Typography = new Typography
        {
            Default = new DefaultTypography
            {
                FontFamily = ["Roboto", "Helvetica", "Arial", "sans-serif"]
            }
        }
    };
}
