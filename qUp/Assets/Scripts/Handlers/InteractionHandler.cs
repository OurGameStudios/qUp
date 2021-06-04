using Actors.Tiles;
using Actors.Units;
using Base.Singletons;
using Handlers.PhaseHandlers;
using Handlers.PlayerHandlers;
using UI;

namespace Handlers {
    public class InteractionHandler : SingletonClass<InteractionHandler> {

        public static void ListenToUi() {
            // TODO unsubscribe from this
           Instance.AddToDispose(SpawnUi.SpawnUnitSelected.Subscribe(OnUnitSpawnSelected));
        }

        public static void OnUnitSpawnSelected(UnitData unit) {
            PlayerHandler.NotifySpawnTilesUnitSpawnSelected(unit);
        }

        public static void OnSpawnTileSelected(ITile tile, UnitData previousUnit, UnitData unit) {
            if (unit != null) {
                tile.SelectedForSpawn(unit); 
                PlayerHandler.NotifySpawnTileSelected(previousUnit, unit);
            }
        }
        

        public static void OnUnitSelected(IUnit unit, ITile tile) {
            if (unit.Owner == PlayerHandler.GetCurrentPlayer()) {
                GridHandler.OnUnitSelected(unit, tile);
            }
        }

        public static void OnEmptyTileSelected(ITile tile) {
            GridHandler.OnUnitDeselected();
            PlayerHandler.NotifySpawnTileSelected(null, null);
        }

        public static void OnContinueToNextPhase() {
            PhaseHandler.ContinuePhase();
        }
    }
}
