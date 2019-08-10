public class UICamera : vertigoGames.hexColorGame.CameraManager
{
    public static UICamera instance;

    protected override void Awake()
    {
        instance = this;
        base.Awake();
    }
}