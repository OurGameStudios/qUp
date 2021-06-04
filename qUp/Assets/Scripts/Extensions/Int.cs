namespace Extensions {
    internal static class IntExtensions {

        public static int IfZero(this int value, int otherValue) => value == 0 ? otherValue : value;
    }
}
