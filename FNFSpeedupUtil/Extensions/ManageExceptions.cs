namespace FNFSpeedupUtil.Extensions;

public static class ManageExceptions
{
    public static void DropException(Action action)
    {
        try
        {
            action();
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}