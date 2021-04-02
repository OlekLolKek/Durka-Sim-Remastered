using System;


namespace Model
{
    public class AmmoModel
    {
        public int AmmoCount { get; private set; } = 10;
        
        public event Action<int> OnAmmoCountChanged = delegate(int i) {  }; 

        public void SetAmmoCount(int newAmount)
        {
            AmmoCount = newAmount;
            OnAmmoCountChanged.Invoke(AmmoCount);
        }
    }
}