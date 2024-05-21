namespace Our.Umbraco.TheDashboard.Models.Frontend;

public class UserAvatarFrontendModel
{
    public UserAvatarFrontendModel()
    {
        Src = "";
        SrcSet = "";
    }

    public UserAvatarFrontendModel(string src, string srcSet)
    {
        Src = src;
        SrcSet = srcSet;
    }

    public string Src { get; set; }
    public string SrcSet { get; set; }
}
