using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Inventory.Installer
{
    public class SlotPoolInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _slotPrefab;
        [SerializeField] private Transform _slotRoot;
        [SerializeField] private int _capacity = 50;

        public override void InstallBindings()
        {
            Container
                .Bind<InventorySlotPool>()
                .AsSingle()
                .WithArguments(_slotPrefab, _slotRoot,  _capacity)
                .NonLazy();
                
        }
    }
}