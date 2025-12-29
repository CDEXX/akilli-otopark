using System.Drawing;

namespace GP_Project
{
    public interface ITitledPage
    {
        string PageTitle { get; }
        Image PageIcon { get; }
    }
}
