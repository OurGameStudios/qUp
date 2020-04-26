namespace Extensions {
    static class IntExtensions {
        public static float OnOdd(this int value, float ifOdd) => value % 2 * ifOdd;

        public static int IfZero(this int value, int otherValue) => value == 0 ? otherValue : value;
    }
}
