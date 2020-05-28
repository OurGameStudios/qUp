using Base.MonoBehaviours;
using TMPro;

namespace UI.ResourceUis {
    public class ResourceUiBehaviour : BaseMonoBehaviour<ResourceUi, IResourceUiState> {

        public TextMeshProUGUI incomeText;
        public TextMeshProUGUI upkeepText;
        public TextMeshProUGUI totalText;
        public TextMeshProUGUI resourceUnitsText;

        protected override void OnStateHandler(IResourceUiState inState) {
            if (inState is IncomeChanged incomeState) {
                incomeText.SetText(incomeState.Income);
                totalText.SetText(incomeState.Total);
            } else if (inState is UpkeepChanged upkeepState) {
                upkeepText.SetText(upkeepState.Upkeep);
                totalText.SetText(upkeepState.Total);
            } else if (inState is ResourceUnitsChanged resourceUnitsState) {
                resourceUnitsText.SetText(resourceUnitsState.ResourceUnitCount);
            }
        }
    }
}
