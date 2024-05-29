
namespace Our.Umbraco.TheDashboard.Counters;

public class DashboardCounterModel
{
    public DashboardCounterModel()
    {
        Style = CounterStyles.Standard;
    }

    /// <summary>
    /// Localization key used in the frontend for translation. Prefer using this over <see cref="Text"/> if possible.
    /// </summary>
    public string LocalizationKey { get; set; }

    /// <summary>
    /// Hardcoded text for the counter.
    /// </summary>
    public string Text { get; set; }

    public int Count { get; set; }

    /// <summary>
    /// Used to provide a click url for a counter, ie linking to a content item or any given URL
    /// </summary>
    public string ClickUrl { get; set; }

    /// <summary>
    /// Provides a way to "click" on a element in the backoffice UI by providing a CSS selector. Ie. *[data-element=tab-contentIntro] > a to show the intro dashboard.
    /// </summary>
    public string ClickElement { get; set; }
        
    public string Style { get; set; }

    public class CounterStyles
    {
        public const string Standard = "standard";
        public const string Success = "success";
        public const string Action = "action";
        public const string Warning = "warning";
        public const string Selected = "selected";
        public const string Danger = "danger";
    }

}
