namespace Base.Interfaces {
    public interface IManager { }

    public interface IClickable {
        void OnClick();

        void OnSecondaryClick();
    }

    public interface IHoverable {
        void OnHoverStart();

        void OnHoverEnd();
    }
}
