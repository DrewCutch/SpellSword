namespace SpellSword.Logging
{
    public interface ILinkable
    {
        public void OnLinkHover();

        public void OnLinkClick();

        public void OnLinkExit();
    }
}