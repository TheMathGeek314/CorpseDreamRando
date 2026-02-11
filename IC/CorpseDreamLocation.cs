using System.Collections.Generic;
using UnityEngine;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Internal;
using ItemChanger.Locations;
using ItemChanger.Util;

namespace CorpseDreamRando {
    public class CorpseDreamLocation: AutoLocation {
        private static readonly Dictionary<string, CorpseDreamLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookCorpseDreams();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookCorpseDreams();
        }

        private static void HookCorpseDreams() {
            On.PlayMakerFSM.OnEnable += editFsm;
        }

        private static void UnhookCorpseDreams() {
            On.PlayMakerFSM.OnEnable -= editFsm;
        }

        private static void editFsm(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);
            (string, string) sceneObject = (self.gameObject.scene.name, recursiveParentSearch(self.gameObject.name, self.gameObject));
            if(CorpseCoords.nameToPlacement.TryGetValue(sceneObject, out string placementName)) {
                FsmState impactState = self.GetState("Impact");
                impactState.RemoveTransitionsOn("FINISHED");
                impactState.AddTransition("FINISHED", "Convo");

                FsmState convoState = self.GetState("Convo");
                convoState.GetFirstActionOfType<CallMethodProper>().Enabled = false;
                AbstractPlacement ap = Ref.Settings.Placements[placementName];
                convoState.AddLastAction(new AsyncLambda(callback => ItemUtility.GiveSequentially(ap.Items, ap, new GiveInfo {
                    FlingType = FlingType.DirectDeposit,
                    Container = Container.Unknown,
                    MessageType = MessageType.Any
                }, callback), "CONVO_FINISH"));

                FsmState boxDownState = self.GetState("Box Down");
                boxDownState.GetFirstActionOfType<SendEventByName>().Enabled = false;
                boxDownState.GetFirstActionOfType<Wait>().Enabled = false;
            }
        }

        private static string recursiveParentSearch(string name, GameObject gameObject) {
            if(gameObject.transform.parent == null)
                return name;
            return recursiveParentSearch($"{gameObject.transform.parent.name}/{name}", gameObject.transform.parent.gameObject);
        }
    }
}
