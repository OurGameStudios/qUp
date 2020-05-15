namespace Base.Interfaces {
    public interface IManager { }

    public interface IClickable {
        void OnClick();
    }

    public interface IHoverable {
        void OnHoverStart();

        void OnHoverEnd();
    }
}
