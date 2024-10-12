using System;

namespace DexiDev.Game.Data.Fields
{
    public interface IUseField : IDataField, IValueToString
    {
        event Action<IUseField> OnUse;
        event Action<IUseField> OnUseCancel;
        event Action<IUseField> OnUseCompleted;

        public void UseButton();

        public void CancelUseButton();

        public void CompleteUse();
    }
}