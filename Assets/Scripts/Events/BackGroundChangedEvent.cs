namespace DefaultNamespace.Events
{
    public struct BackGroundChangedEvent
    {
        public bool ShouldChangeBackGround { get;}

        public BackGroundChangedEvent(bool shouldChangeBackGround )
        {
            ShouldChangeBackGround = shouldChangeBackGround;
        }
    }
}