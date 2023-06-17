﻿namespace TheRat.InventorySystem
{
    public abstract class PassiveItem : Item
    {
        public abstract void OnPutOn();

        public virtual void OnRun() { }
        public virtual void OnPutOut() { }
    }
}