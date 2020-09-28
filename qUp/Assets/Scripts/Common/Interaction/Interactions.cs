namespace Common.Interaction {
    public interface IClickable {
        void OnInteraction(ClickInteraction interaction);
    }

    public interface IHoverable {
        void OnHoverStart();

        void OnHoverEnd();
    }
}