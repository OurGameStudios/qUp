using Base.Singletons;

namespace Handlers {
    public class Configuration : SingletonClass<Configuration> {
        
        private int maxTick;

        public void TrySetMaxTick(int maxTick) {
            if (maxTick > this.maxTick) {
                this.maxTick = maxTick;
            }
        }

        public static int GetMaxTick() => Instance.maxTick;

        public static int GetPointTileIncrease() => 1;
        
        public static int GetMaxPoints() => 15;
    }
}
