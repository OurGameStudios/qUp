using System;
using Base.Common;
using Base.Interfaces;
using Base.MonoBehaviours;
using UnityEngine;
using static Base.StateDocumentation;

namespace Base {
    public class
        BaseMonoBehaviourDocumentation : BaseMonoBehaviour<BaseControllerDocumentation, StateDocumentation
        > {
        protected override void OnStateHandler(StateDocumentation inStateDocumentation) {
            if (inStateDocumentation is TestStateDocumentation) {
                Debug.Log("First test");
            }

            if (inStateDocumentation is SecondTestStateDocumentation) {
                Debug.Log("Second Test");
            }
        }

        public BaseMonoBehaviourDocumentation test;

        public bool commitTest;
        public bool commitSecondTest;
        public bool commitThirdTest;
        public bool commitFourthTest;

        // Start is called before the first frame update
        void Start() {
            //Safely exposing this to controller
            Controller.ProvideScript += () => this;
        }

        // Update is called once per frame
        void Update() {
            if (commitTest) {
                commitTest = false;
                Controller.triggerTest();
            }

            if (commitSecondTest) {
                commitSecondTest = false;
                Controller.triggerSecondTest();
            }

            if (commitThirdTest) {
                commitThirdTest = false;
                Controller.triggerThirdTest();
            }

            if (commitFourthTest) {
                commitFourthTest = false;
                test.Controller.triggerFourthTest();
            }
        }

        public void ThirdTest() => Debug.Log("Third test's the charm");

        public void FourthTest() => Debug.Log("Four is the number of wheels");
    }

    public class BaseControllerDocumentation : BaseController<StateDocumentation> {
        
        //Safe way of providing Behaviour/Script references to controller
        //This needs to be set in the Behaviour/Script to { Controller.ProvideScript += () => this }, where this is reference to itself
        public Func<BaseMonoBehaviourDocumentation> ProvideScript;

        public void triggerTest() {
            SetState(new TestStateDocumentation());
        }

        public void triggerSecondTest() {
            SetState(new SecondTestStateDocumentation());
        }

        public void triggerThirdTest() {
            ProvideScript?.Invoke().ThirdTest();
        }

        public void triggerFourthTest() {
            Debug.Log("But I (controller) am still alive");
            ProvideScript?.Invoke().FourthTest();
        }
    }

    public class BaseDataDocumentation : BaseData { }

    public abstract class StateDocumentation : IState {
        public class TestStateDocumentation : StateDocumentation {
            public string TestMe() => "test me one";
        }

        public class SecondTestStateDocumentation : StateDocumentation {
            public string MeToo() => "test me too 2";
        }
    }
}
