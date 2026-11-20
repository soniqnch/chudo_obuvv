namespace WinFormsApp1;

internal static class AppStyle
{
    internal static readonly Color Secondary = Color.FromArgb(210, 246, 231);
    private static readonly Color Accent = Color.FromArgb(112, 178, 175);
    private static Icon? windowIcon;
    internal static void Apply(Form form, params Button[] primaryActions)
    {
        form.BackColor = Color.White;
        string iconPath = Path.Combine(AppContext.BaseDirectory, "Resources", "app.ico");
        if (File.Exists(iconPath))
        {
            windowIcon ??= new Icon(iconPath);
            form.Icon = windowIcon;
        }
        foreach (Button button in primaryActions)
        {
            button.UseVisualStyleBackColor = false;
            button.BackColor = Accent;
            button.ForeColor = Color.Black;
        }
    }
}
